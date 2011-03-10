using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace Bham.Ptu.UI {
	
	using Native;
	
	public class Joystick {
		
		public static Collection<JoystickInfo> GetJoysticks() {
			
			Collection<JoystickInfo> ret = new Collection<JoystickInfo>();
			int count = NativeMethods.JoyGetNumDevices();
			int size  = Marshal.SizeOf( typeof( JoyCapabilitiesInfo ) );
			
			for(int i=0;i<count;i++) {
				
				JoyCapabilitiesInfo joystick = new JoyCapabilitiesInfo();
				
				JoyResult result = NativeMethods.JoyGetDevCapsW( i, ref joystick, size );
				
				if( result == JoyResult.OK ) ret.Add( new JoystickInfo(i, joystick ) );
			}
			
			return ret;
		}
		
		private JoystickInfo _info;
		
		public Joystick(JoystickInfo deviceInfo) {
			
			_info = deviceInfo;
			
			if( _info.Id < 0 || _info.Id > 15 ) throw new ArgumentOutOfRangeException("deviceInfo", _info.Id, "Joystick ID must be between 0 and 15");
		}
		
		public JoystickStatus PollStatus() {
			
			JoyInfoEx info = new JoyInfoEx();
			info.Size = (uint)Marshal.SizeOf( info );
			info.SetFields = JoySetFields.ReturnAll;
			
			JoyResult result = NativeMethods.JoyGetPosEx( _info.Id, ref info );
			if( result != JoyResult.OK ) throw new InvalidOperationException("Could not poll joystick status. Error returned: " + result);
			
			return new JoystickStatus( _info, info );
		}
		
	}
	
	public class JoystickInfo {
		
		internal JoystickInfo(Int32 id, JoyCapabilitiesInfo capabilities) {
			Id           = id;
			Capabilities = capabilities;
		}
		
		public Int32               Id           { get; private set; }
		public JoyCapabilitiesInfo Capabilities { get; private set; }
		
	}
	
	public class JoystickStatus {
		
		private JoystickInfo _info;
		private JoyInfoEx    _status;
		
		internal JoystickStatus(JoystickInfo deviceInfo, JoyInfoEx statusInfo) {
			_info   = deviceInfo;
			_status = statusInfo;
		}
		
		public JoyButtons Buttons {
			get { return _status.ButtonState; }
		}
		
		public Single PoV {
			get {
				if( _status.PovAngle <= 0x8C3C ) {
					
					return (float)_status.PovAngle / 100f;
				}
				return -1f;
			}
		}
		
		public Single RAxis {
			get { 
				if( (_status.SetFields & JoySetFields.ReturnR) != JoySetFields.ReturnR ) return 0;
				
				float fromMin = _status.RPos - _info.Capabilities.RMin;
				float fromMax = fromMin / _info.Capabilities.RMax;
				
				return ( fromMax * 2f ) - 1f;
			}
		}
		
		public Single UAxis {
			get { 
				if( (_status.SetFields & JoySetFields.ReturnU) != JoySetFields.ReturnU ) return 0;
				
				float fromMin = _status.UPos - _info.Capabilities.UMin;
				float fromMax = fromMin / _info.Capabilities.UMax;
				
				return ( fromMax * 2f ) - 1f;
			}
		}
		
		public Single VAxis {
			get { 
				if( (_status.SetFields & JoySetFields.ReturnV) != JoySetFields.ReturnV ) return 0;
				
				float fromMin = _status.VPos - _info.Capabilities.VMin;
				float fromMax = fromMin / _info.Capabilities.VMax;
				
				return ( fromMax * 2f ) - 1f;
			}
		}
		
		public Single XAxis {
			get { 
				if( (_status.SetFields & JoySetFields.ReturnX) != JoySetFields.ReturnX ) return 0;
				
				float fromMin = _status.XPos - _info.Capabilities.XMin;
				float fromMax = fromMin / _info.Capabilities.XMax;
				
				return ( fromMax * 2f ) - 1f;
			}
		}
		
		public Single YAxis {
			get { 
				if( (_status.SetFields & JoySetFields.ReturnY) != JoySetFields.ReturnY ) return 0;
				
				float fromMin = _status.YPos - _info.Capabilities.YMin;
				float fromMax = fromMin / _info.Capabilities.YMax;
				
				return ( fromMax * 2f ) - 1f;
			}
		}
		
		public Single ZAxis {
			get { 
				if( (_status.SetFields & JoySetFields.ReturnZ) != JoySetFields.ReturnZ ) return 0;
				
				float fromMin = _status.ZPos - _info.Capabilities.ZMin;
				float fromMax = fromMin / _info.Capabilities.ZMax;
				
				return ( fromMax * 2f ) - 1f;
			}
		}
		
	}
	
	namespace Native {
		
		internal class NativeMethods {
			
			[DllImport("winmm.dll", EntryPoint="joyGetDevCapsW")]
			public static extern JoyResult JoyGetDevCapsW(int joyId, ref JoyCapabilitiesInfo pjc, int size);
			
			[DllImport("winmm.dll", EntryPoint="joyGetNumDevs")]
			public static extern Int32 JoyGetNumDevices();
			
			[DllImport("winmm.dll", EntryPoint="joyGetPosEx")]
			public static extern JoyResult JoyGetPosEx(int uJoyID, ref JoyInfoEx pji);
			
			[DllImport("winmm.dll", EntryPoint="joyReleaseCapture")]
			public static extern JoyResult JoyReleaseCapture(int joyId);
			
			[DllImport("winmm.dll", EntryPoint="joySetCapture")]
			public static extern JoyResult JoySetCapture(IntPtr hwnd, int uJoyID, int uPeriod, bool fChanged);
			
		}
		
		internal enum JoyResult : uint {
			OK           = 0,
			Error        = 1,
			BadDeviceID  = 2,
			NoDriver     = 6,
			InvalidParam = 11,
			JoystickInvalidParam        = 0xA5,
			JoystickRequestNotCompleted = 0xA6,
			JoystickUnplugged           = 0xA7,
		}
		
		[Flags]
		public enum JoySetFields {
			ReturnX       = 0x01,
			ReturnY       = 0x02,
			ReturnZ       = 0x04,
			ReturnR       = 0x08,
			ReturnU       = 0x10,
			ReturnV       = 0x20,
			
			ReturnXY      = 0x03,
			ReturnXYZ     = 0x07,
			ReturnXYZR    = 0x0F,
			ReturnXYZRU   = 0x1F,
			ReturnXYZRUV  = 0x3F,
			
			ReturnPov     = 0x40,
			ReturnButtons = 0x80,
			
			ReturnAll     = 0xFF
		}
		
		[Flags]
		public enum JoyButtons : uint {
			None     = 0,
			Button1  = 1,
			Button2  = 2,
			Button3  = 4,
			Button4  = 8,
			Button5  = 0x10,
			Button6  = 0x20,
			Button7  = 0x40,
			Button8  = 0x80,
			Button9  = 0x100,
			Button10 = 0x200,
			Button11 = 0x400,
			Button12 = 0x800,
			Button13 = 0x1000,
			Button14 = 0x2000,
			Button15 = 0x4000,
			Button16 = 0x8000
			// there should also be Button17 through Button32 btw
		}
		
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode,Pack=1)]
		public struct JoyCapabilitiesInfo {
			
			public UInt16 ManufacturerId;
			public UInt16 ProductId;
			
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=32)]
			public String ProductName;
			
			public UInt32 XMin;
			public UInt32 XMax;
			public UInt32 YMin;
			public UInt32 YMax;
			public UInt32 ZMin;
			public UInt32 ZMax;
			
			public UInt32 ButtonCount;
			public UInt32 PeriodMin;
			public UInt32 PeriodMax;
			
			public UInt32 RMin;
			public UInt32 RMax;
			public UInt32 UMin;
			public UInt32 UMax;
			public UInt32 VMin;
			public UInt32 VMax;
			
			public JoyCapabilities Capabilities;
			
			public UInt32 AxesMax;
			public UInt32 AxesCount;
			
			public UInt32 ButtonsMax;
			
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=32)]
			public String RegistryKeyName;
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=260)]
			public String ManufacturerName;
			
		}
		
		[StructLayout( LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode )]
        public class JOYCAPS
        {
            public short mid; 
            public short pid; 
            [MarshalAs( UnmanagedType.ByValTStr, SizeConst = 32 )]
            public string name;
            public int  xMin; 
            public int  xMax; 
            public int  yMin; 
            public int  yMax; 
            public int  zMin; 
            public int  zMax; 
            public int  buttonsNumber; 
            public int  minPeriod; 
            public int  maxPeriod; 
            public int  rMin; 
            public int  rMax; 
            public int  uMin; 
            public int  uMax; 
            public int  vMin; 
            public int  vMax; 
            public int  caps; 
            public int  axesMax; 
            public int  axesNumber; 
            public int  buttonsMax; 
            [MarshalAs( UnmanagedType.ByValTStr, SizeConst = 32 )]
            public string regKey;
            [MarshalAs( UnmanagedType.ByValTStr, SizeConst = 260 )]
            public string oemVxD;
        }
		
		[Flags]
		public enum JoyCapabilities : uint {
			None   = 0x00,
			HasZ   = 0x01,
			HasR   = 0x02,
			HasU   = 0x04,
			HasV   = 0x08,
			HasPoV = 0x10,
			PoVIs4Dir = 0x20,
			PovIsCont = 0x40
		}
		
		[StructLayout(LayoutKind.Sequential)]
		public struct JoyInfoEx {
			
			public UInt32      Size;
			public JoySetFields SetFields;
			
			public UInt32      XPos;
			public UInt32      YPos;
			public UInt32      ZPos;
			public UInt32      RPos;
			public UInt32      UPos;
			public UInt32      VPos;
			
			public JoyButtons  ButtonState;
			public UInt32      ButtonCount;
			public UInt32      PovAngle;
			public UInt32      Reserved1;
			public UInt32      Reserved2;
		}
		
	}
}
