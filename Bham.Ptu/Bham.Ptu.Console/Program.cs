using System;
using System.IO.Ports;
using System.Threading;

namespace Bham.Ptu.ConsoleUI {
	
	public static class Program {
		
		public static void Main(String[] args) {
			
			try {
				
				RunTests();
				
			} catch(Exception ex) {
				
				Console.WriteLine("Exception:");
				
				while(ex != null) {
					
					Console.WriteLine( ex.Message );
					Console.WriteLine( ex.StackTrace );
					Console.WriteLine();
					
					ex = ex.InnerException;
				}
				
				Console.ReadLine();
			}
			
		}
		
		private static void RunTests() {
			
			String portName = GetPortName();
			
			PTUnit unit = new PTUnit( portName );
			
			/////////////////////////////////////////
			
			Console.WriteLine("Firmware");
			Console.WriteLine("\t" + unit.GetFirmwareInfo() );
//			Console.WriteLine("Performing reset. . .");
//			
//			unit.Reset();
			
			/////////////////////////////////////////
			
			Console.WriteLine("Resolution and Limits:");
			
			short panMin, panMax, tiltMin, tiltMax;
			
			Console.WriteLine("\tPan Minimum:\t"     + (panMax = unit.GetPanPositionLimitMin() ) );
			Console.WriteLine("\tPan Maximum:\t"     + (panMin = unit.GetPanPositionLimitMax() ) );
			Console.WriteLine("\tPan Resolution:\t"  + unit.GetPanPositionResolution() );
			Console.WriteLine();
			Console.WriteLine("\tTilt Minimum:\t"    + (tiltMax = unit.GetTiltPositionLimitMin() ) );
			Console.WriteLine("\tTilt Maximum:\t"    + (tiltMin = unit.GetTiltPositionLimitMax() ) );
			Console.WriteLine("\tTilt Resolution:\t" + unit.GetTiltPositionResolution() );
			
			/////////////////////////////////////////
			
			Console.WriteLine("Moving:");
			
			Console.WriteLine("\tMoving to Max Limits (both axes):");
			
			unit.SetPanDesiredPosition( panMax );
			unit.SetTiltDesiredPosition( tiltMax );
			Console.WriteLine("\t\tWaiting to finish...");
			unit.AwaitCompletion();
			Console.WriteLine("\t\tFinished...");
			Console.WriteLine();
			
			
			Console.WriteLine("\tMoving to Min Limits (both axes):");
			
			unit.SetPanDesiredPosition( panMin );
			unit.SetTiltDesiredPosition( tiltMin );
			Console.WriteLine("\t\tWaiting to finish...");
			unit.AwaitCompletion();
			Console.WriteLine("\t\tFinished...");
			Console.WriteLine();
			
			
			Console.WriteLine("\tReturning to (0,0):");
			
			unit.SetPanDesiredPosition( 0 );
			unit.SetTiltDesiredPosition( 0 );
			Console.WriteLine("\t\tWaiting to finish...");
			unit.AwaitCompletion();
			Console.WriteLine("\t\tFinished...");
			Console.WriteLine();
			
			/////////////////////////////////////////
			
			Console.WriteLine("Sequential command tests:");
			
			Console.WriteLine("\tMoving to Max Limits (both axes)");
			unit.SetPanDesiredPosition( panMax );
			unit.SetTiltDesiredPosition( tiltMax );
			Console.WriteLine("\tSleeping for 750ms...");
			Thread.Sleep( 750 );
			
			Console.WriteLine("\tMoving to Min Limits (both axes) without waiting for completion:");
			unit.SetPanDesiredPosition( panMin );
			unit.SetTiltDesiredPosition( tiltMin );
			Console.WriteLine("\t\tWaiting to finish...");
			unit.AwaitCompletion();
			Console.WriteLine("\t\tFinished...");
			Console.WriteLine();
			
			/////////////////////////////////////////
			
			Console.ReadLine();
			
		}
		
		private static String GetPortName() {
			
			Console.WriteLine("Select a Serial COM port:");
			
			String[] names = SerialPort.GetPortNames();
			for(int i=0;i<names.Length;i++) {
				
				Console.Write("\t [");
				Console.Write( i );
				Console.Write("] ");
				Console.WriteLine( names[i] );
			}
			
			while( true ) {
				
				Console.WriteLine("Enter the zero-based index of the port to use:");
				
				int idx;
				String l = Console.ReadLine();
				if( Int32.TryParse( l, out idx ) ) {
					
					if( idx >= 0 && idx <= names.Length ) return names[ idx ];
				}
				
			}
			
		}
	}
}
