using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Remote {
	public class MemoryAbusingLibrary : IRemoteObject {
		private const int _blockSize = 1024 * 1024;

		private Timer _timer = null;
		private static object _lockObj = new object();

		public override void Init() {
			_timer = new Timer(_timerEvent, null, 10000, 10000);
			Trace.WriteLine("MemoryAbusingLibrary: Init()");
		}

		public void Dispose() {
			_timer.Dispose();
			_timer = null;
		}

		unsafe private static void _timerEvent(object state) {
			lock (_lockObj) {
				int blocks = 1024;
				byte*[] handles = new byte*[blocks];
				Trace.WriteLine("MemoryAbusingLibrary: Starting flood...");
				try {
					for (int i = 0; i < blocks; i++) {
						handles[i] = (byte*)Marshal.AllocHGlobal(_blockSize);
						for (int offset = 0; offset < _blockSize; offset++) *(handles[i] + offset) = 1;
					}
				} finally {
					ManualResetEvent mre = new ManualResetEvent(false);
					Thread memCheck = new Thread(() => {
						int i = 0;
						while (!mre.WaitOne(0, false)) {
							for (int offset = 0; offset < _blockSize; offset++) {
								if (*(handles[i % handles.Length] + offset) != 1) {
									// boo =[
									throw new InvalidOperationException("Cannot read/write to memory =[");
								}
							}
						}
					});
					memCheck.IsBackground = true;
					memCheck.Start();
					mre.Set();
					memCheck.Join();
				}

				try {
					for (int i = 0; i < blocks; i++) Marshal.FreeHGlobal(new IntPtr(handles[i]));
					GC.Collect();
				} finally {
					Trace.WriteLine("MemoryAbusingLibrary: Drained overflow =]");
				}
			}
		}

	}
}
