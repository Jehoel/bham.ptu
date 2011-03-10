using System;
using System.Runtime.Serialization;

namespace Bham.Ptu {
	
	[Serializable]
	public class PTException : Exception {
		public PTException() { }
		public PTException(string message) : base(message) { }
		public PTException(string message, Exception inner) : base(message, inner) { }
		protected PTException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
	
	[Serializable]
	public class PTTimeoutException : PTException {
		public PTTimeoutException() { }
		public PTTimeoutException(string message) : base(message) { }
		public PTTimeoutException(string message, Exception inner) : base(message, inner) { }
		protected PTTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
	
	/// <summary>An exception thrown as a result of an error-code returned from the PT controller unit.</summary>
	[Serializable]
	public class PTControllerException : PTException {
		
		public PTResult  Result  { get; private set; }
		
		public PTControllerException(String message) : base(message) {
			Result = PTResult.Ok;
		}
		
		public PTControllerException(PTResult result) : base("Controller Error: " + result.ToString() ) {
			
			Result = result;
		}
		
		protected PTControllerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
	
}
