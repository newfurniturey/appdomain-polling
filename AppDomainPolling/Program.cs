using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppDomainPolling {
	class Program {
		private static readonly string[] _asmProjects = new string[] {
		};

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
		}

		public void ProcessAssembly(string assemblyPath) {
			string dllName = assemblyPath.Substring(assemblyPath.LastIndexOf('\\') + 1);

			Console.WriteLine("\tLoaded: {0}", dllName);
		}
	}
}
