using System;
using System.Collections.Generic;
using System.Text;

namespace Bham.Ptu {
	
	public static class Utility {
		
		public static Byte GetUnsignedByteValue(Byte b) {
			
			byte v = (byte)(b & 127);
			if( b >> 7 != 0 ) {
				
				v += 128;
			}
			
			return v;
			
		}
		
		public static UInt16 FlipEndian(UInt16 value) {
			
			Int16 temp = (Int16)value;
			temp = FlipEndian(temp);
			return (UInt16)temp;
		}
		public static UInt32 FlipEndian(UInt32 value) {
			
			Int32 temp = (Int32)value;
			temp = FlipEndian(temp);
			return (UInt32)temp;
		}
		public static UInt64 FlipEndian(UInt64 value) {
			
			Int64 temp = (Int64)value;
			temp = FlipEndian(temp);
			return (UInt64)temp;
		}
		
		
		
		public static Int16 FlipEndian(Int16 value) {
			
			short b1 = (short)((value & 0xFF) << 8);
			short b2 = (short)((value >> 8) & 0xFF);
			
			return (short)(b1 | b2);
		}
		
		public static Int32 FlipEndian(Int32 value) {
			
			int s1 = ( FlipEndian( (short)value ) & 0xFFFF ) << 0x10;
			int s2 = FlipEndian( (short)(value >> 0x10) ) & 0xFFFF;
			
			return s1 | s2;
		}
		
		public static Int64 FlipEndian(Int64 value) {
			
			long i1 = (FlipEndian( (int)value ) & 0xFFFFFFFFL ) << 0x20;
			long i2 = FlipEndian( (int)(value >> 0x20) ) & 0xFFFFFFFFL;
			
			return i1 | i2;
		}
		
		
		public static Double DegreesToRadians(Double degrees) {
			
			return degrees * Math.PI / 180d;
		}
		
		public static Double RadiansToDegrees(Double radians) {
			
			return radians * 180d / Math.PI;
		}
		
	}
	
}
