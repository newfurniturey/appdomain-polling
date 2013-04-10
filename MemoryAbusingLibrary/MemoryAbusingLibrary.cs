using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Remote {
	public class MemoryAbusingLibrary : IRemoteObject {
		private const int _blockSize = 1024 * 1024;

		private Timer _timer = null;

		public void Init() {
			_timer = new Timer(_timerEvent, null, 10000, 10000);
			Trace.WriteLine("MemoryAbusingLibrary: Init()");
		}

		public void Dispose() {
			_timer.Dispose();
			_timer = null;
		}

		public void GetMemoryUsage() {

		}

		private static void _timerEvent(object state) {

		}

	}
}
