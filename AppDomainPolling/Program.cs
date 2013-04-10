using System;
using System.Collections.Generic;
using System.Threading;

namespace AppDomainPolling {
	class Program {
		private static readonly string[] _asmProjects = new string[] {
			"NormalLibrary",
			"MemoryAbusingLibrary"
		};

		private static readonly List<RemoteLoader> _remoteObjects = new List<RemoteLoader>();

		private static Timer _statsTimer = null;

		static void Main(string[] args) {
			new Program();

			Console.WriteLine("done.");
			Console.ReadKey(true);
		}

		public Program() {
			AppDomain.MonitoringIsEnabled = true;

			Console.WriteLine("Loading remote objects...");
			foreach (string asmProject in _asmProjects) {
				string path = string.Format(@"..\..\..\{0}\bin\Debug\{1}.dll", asmProject, asmProject);
				ProcessAssembly(path);
			}
			Console.WriteLine("");

			Console.WriteLine("Initializing each remote object...");
			foreach (RemoteLoader obj in _remoteObjects) {
				obj.Init();
			}
			Console.WriteLine("");

			_statsTimer = new Timer(_getStats, null, 1000, 1000);
		}

		public void ProcessAssembly(string assemblyPath) {
			string dllName = assemblyPath.Substring(assemblyPath.LastIndexOf('\\') + 1);

			AppDomain domain = AppDomain.CreateDomain(dllName);
			RemoteLoader remoteLoader = (RemoteLoader)domain.CreateInstanceAndUnwrap(typeof(RemoteLoader).Assembly.FullName, typeof(RemoteLoader).FullName);
			remoteLoader.Load(assemblyPath);
			_remoteObjects.Add(remoteLoader);

			Console.WriteLine("\tLoaded: {0}", dllName);
		}

		private void _getStats(object state) {
			Console.WriteLine("Getting stats...");
			foreach (RemoteLoader obj in _remoteObjects) {
				obj.GetMemoryUsage();
			}
		}
	}
}
