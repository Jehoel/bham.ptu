using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace Bham.Ptu {
	
	/* Notes: the connection system is time-sensitive
	 * if you run the program slowly (i.e. step-by-step) then the SerialPort instance correctly fills the byte buffers
	 * but if you run it fast then you get erroneous data, as though it were prematurely returning from .Read without having the data already in
	 * That's what WaitForData is for: it blocks the thread until there's enough data in the (combined) receive buffer. *NEVER!* read the SerialPort.BaseStream directly because SerialPort does its own caching.
	 */
	
	/// <summary>Represents an active Serial Port connection to the physical Pan/Tilt Unit. All communication is done through this class which ensures all protocol exchanges are atomic.</summary>
	internal class PTConnection : IDisposable {
		
		private Object     _portLock = new Object();
		private SerialPort _port;
		
		public PTConnection(String portName) {
			
			_port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
			_port.NewLine = "\r\n";
			_port.Open();
		}
		
		public void Dispose() {
			
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		protected virtual void Dispose(bool managed) {
			
			if( managed ) {
				
				if( _port != null ) _port.Dispose();
			}
			
		}
		
		/// <summary>This method adds the delimiter to the command by itself.</summary>
		private void WriteCommand(String asciiCommand) {
			
			if( asciiCommand.IndexOf('\n') > -1 || asciiCommand.IndexOf('\r') > -1 ) throw new ArgumentException("ASCII Command strings must not contain any whitespace characters.");
			
			Debug.WriteLine( asciiCommand );
			
			// ASCII commands end with a single space character
			
			Byte[] bytes = Encoding.ASCII.GetBytes( asciiCommand + ' ' );
			
			_port.BaseStream.Write( bytes, 0, bytes.Length );
			
		}
		
		private void WriteCommand(PTCommand cmd, params Byte[] args) {
			
			Debug.WriteLine( cmd.ToString() + "(" + ToCsv( args ) + ")" );
			
			_port.BaseStream.WriteByte( (byte)cmd );
			
			_port.BaseStream.Write( args, 0, args.Length ); // if args.Length == 0 then it won't get written, no need to guard this.
			
			_port.BaseStream.Flush();
		}
		
		private static String ToCsv(Byte[] array) {
			StringBuilder sb = new StringBuilder();
			for(int i=0;i<array.Length;i++) {
				sb.Append( array[i].ToString("X") );
				if( i < array.Length - 1 ) sb.Append(", ");
			}
			return sb.ToString();
		}
		
		private void WaitForData(int nofBytes) {
			
			long ticksAtEnter = DateTime.Now.Ticks;
			
			while( _port.BytesToRead < nofBytes ) {
				Thread.Sleep(1);
				
				long ticksNow = DateTime.Now.Ticks;
				TimeSpan timeWaiting = new TimeSpan( ticksNow - ticksAtEnter );
				if( timeWaiting.Milliseconds > 500 ) throw new PTTimeoutException("Timeout (500ms) exceeded waiting for data on serial port " + _port.PortName + ".");
			}
			
		}
		
		/// <summary>Set to -1 for Infinite timeout.</summary>
		public void SetTimeout(int miliseconds) {
			
			_port.ReadTimeout = miliseconds;
		}
		
		public void ResetParser() {
			
			lock( _portLock ) {
				
				Thread.Sleep(500); // Allow any pending PTU commands to complete, unless it's moving at low speeds from one end to the other in Await mode, in which case this won't work
				
				// Terminate any pending parses
				_port.BaseStream.WriteByte( (byte)' ' );
				_port.BaseStream.Flush();
				
				_port.BaseStream.WriteByte( (byte)' ' );
				_port.BaseStream.Flush();
				
				_port.BaseStream.WriteByte( (byte)' ' );
				_port.BaseStream.Flush();
				
				Thread.Sleep(250); // Allow return of any PTU info from termination (e.g. if firmware info was requested)
				
				_port.DiscardInBuffer();
			}
		}
		
		/////////////////////////////////////////
		
		/// <summary>This command appends the ' ' delimiter at the end itself.</summary>
		public String AsciiExchange(String command) {
			
			lock( _portLock ) {
				
				WriteCommand( command );
				
				String response = _port.ReadLine(); // note that I set SerialPort.NewLine to "\r\n" rather than the default of just "\n"
				return response;
			}
		}
		
		/////////////////////////////////////////
		
		internal Object SyncRoot { get { return _portLock; } }
		
		internal Byte GetByte() {
			
			int data = _port.ReadByte();
			return (byte)data;
		}
		
		public Byte GetByte(PTCommand cmd, UInt16 arg) {
			
			Byte hi = (byte)(arg >> 8); // high-order byte
			Byte lo = (byte)arg;        // low-order byte
			
			return GetByte( cmd, hi, lo );
		}
		
		public Byte GetByte(PTCommand cmd, Int16 arg) {
			
			return GetByte( cmd, (UInt16)arg );
		}
		
		public Byte GetByte(PTCommand cmd, UInt32 arg) {
			
			Byte b1 = (byte)(arg >> 24);
			Byte b2 = (byte)(arg >> 16);
			Byte b3 = (byte)(arg >>  8);
			Byte b4 = (byte)(arg      );
			
			return GetByte( cmd, b1, b2, b3, b4 );
		}
		
		public Byte GetByte(PTCommand cmd, params Byte[] args) {
			
			lock( _portLock ) {
				
				WriteCommand( cmd, args );
				
				int data = _port.BaseStream.ReadByte();
				
				Byte r = (byte)data;
				
				return r;
			}
		}
		
		public UInt16 GetUInt16(PTCommand cmd, params Byte[] args) {
			
			byte[] buffer = GetKnownBuffer( cmd, 2, args );
			
			UInt16 value = BitConverter.ToUInt16(buffer, 0);
			if( BitConverter.IsLittleEndian ) value = Utility.FlipEndian( value );
			return value;
		}
		
		public Int16 GetInt16(PTCommand cmd, params Byte[] args) {
			
			byte[] buffer = GetKnownBuffer( cmd, 2, args );
			
			Int16 value = BitConverter.ToInt16(buffer, 0);
			if( BitConverter.IsLittleEndian ) value = Utility.FlipEndian( value );
			return value;
		}
		
		public Int32 GetInt32(PTCommand cmd, params Byte[] args) {
			
			byte[] buffer = GetKnownBuffer( cmd, 4, args );
			
			Int32 value = BitConverter.ToInt32(buffer, 0);
			if( BitConverter.IsLittleEndian ) value = Utility.FlipEndian( value );
			return value;
		}
		
		public UInt32 GetUInt32(PTCommand cmd, params Byte[] args) {
			
			byte[] buffer = GetKnownBuffer( cmd, 4, args );
			
			UInt32 value = BitConverter.ToUInt32(buffer, 0);
			if( BitConverter.IsLittleEndian ) value = Utility.FlipEndian( value );
			return value;
		}
		
		private Byte[] GetKnownBuffer(PTCommand cmd, int size, params Byte[] args) {
			
			lock( _portLock ) {
				
				WriteCommand( cmd, args );
				
				WaitForData( size );
				
				Byte[] buffer = new Byte[ size ];
				_port.Read( buffer, 0, size );
				
				return buffer;
			}
		}
		
		/// <summary>Strongly recommend setting the timeout to ~250ms before calling. Don't forget to reset it afterwards.</summary>
		public String GetLine(PTCommand cmd, params Byte[] args) {
			
			lock( _portLock ) {
				
				WriteCommand( cmd, args );
				
				String line = _port.ReadLine();
				
				return line;
			}
		}
		
	}
}
