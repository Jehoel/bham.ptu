using System;
using System.Collections.Generic;
using System.Threading;

namespace Bham.Ptu {
	
	/// <summary>Ensures all PT commands are issued sequentially (in another thread).</summary>
	public class PTQueue {
		
		private Object                     _ql = new Object();
		private Queue<PTQueuedCommandInfo> _q  = new Queue<PTQueuedCommandInfo>();
		private Thread                     _thread;
		private AutoResetEvent             _event;
		
		private PTUnit                     _unit;
		private Boolean                    _stop;
		
		private Object _statusLock = new Object();
		private String _status     = "Idle";
		
		public PTQueue(PTUnit unit) {
			_event = new AutoResetEvent(false);
			_unit = unit;
		}
		
		public void AddCommand(PTQueuedCommandInfo cmd) {
			
			lock( _ql ) _q.Enqueue( cmd );
			
			_event.Set();
		}
		
		public void Start() {
			_stop   = false;
			_thread = new Thread( Thread );
			_thread.Start();
		}
		
		public void Stop() {
			_stop = true;
			_event.Set();
		}
		
		private void Thread() {
			
			while( !_stop ) {
				
				Status = "Idle";
				_event.WaitOne();
				
				if( _stop ) return;
				
				PTQueuedCommandInfo cmd;
				while( (cmd = Get()) != null ) {
					
					if( _stop ) return;
					
					Status = cmd.CommandName;
					cmd.Command( _unit );
				}
				
			}
			
		}
		
		public event EventHandler StatusChanged;
		
		public String Status {
			get {
				lock( _statusLock ) return _status;
			}
			private set {
				lock( _statusLock ) _status = value;
				if( StatusChanged != null ) StatusChanged(this, EventArgs.Empty);
			}
		}
		
		private PTQueuedCommandInfo Get() {
			lock( _ql ) {
				if( _q.Count == 0 ) return null;
				return _q.Dequeue();
			}
		}
		
	}
	
	public class PTQueuedCommandInfo {
		public PTQueuedCommandInfo(String name, PTQueuedCommand cmd) {
			CommandName = name;
			Command     = cmd;
		}
		public String          CommandName { get; set; }
		public PTQueuedCommand Command     { get; set; }
	}
	
	public delegate void PTQueuedCommand(PTUnit unit);
	
}
