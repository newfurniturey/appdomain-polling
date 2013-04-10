using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using System.Threading.Tasks;
using System.Timers;

namespace Remote {
	public class NormalLibrary : IRemoteObject {
		private Timer _timer = null;

		public void Init() {
			_timer = new Timer(2000);
			_timer.Elapsed += new ElapsedEventHandler(_timerEvent);
			_timer.Enabled = true;
			System.Diagnostics.Trace.WriteLine("NormalLibrary: Init()");
		}

		public void Dispose() {
			_timer.Elapsed -= _timerEvent;
			_timer.Stop();
			_timer.Dispose();
			_timer = null;
		}

		public void GetMemoryUsage() {

		}

		private void _timerEvent(object source, ElapsedEventArgs args) {
			// misc light operations
			int someVar = 0;
			for (int i = 0; i <= 10; i++) someVar += i;
			someVar -= 13;
		}
	}
}
