using System.Timers;

namespace Remote {
	public class NormalLibrary : IRemoteObject {
		private Timer _timer = null;

		public override void Init() {
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

		private void _timerEvent(object source, ElapsedEventArgs args) {
			// misc light operations
			int someVar = 0;
			for (int i = 0; i <= 10; i++) someVar += i;
			someVar -= 13;
		}
	}
}
