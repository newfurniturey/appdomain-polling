using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Remote;

namespace AppDomainPolling {
	public class RemoteLoader : MarshalByRefObject {
		private IRemoteObject _instance = null;

		public void Load(string assemblyPath) {
			string dllName = assemblyPath.Substring(assemblyPath.LastIndexOf('\\') + 1);

			AssemblyName asmName = AssemblyName.GetAssemblyName(assemblyPath);
			Assembly asm = Assembly.Load(asmName);
			_loadType(asm);
		}

		public void Init() {
			AppDomain.MonitoringIsEnabled = true;
			_instance.Init();
			Console.WriteLine("\t{0} initialized.", _instance.GetType().Name);
		}

		public void GetMemoryUsage() {
			_instance.GetMemoryUsage();
		}

		private void _loadType(Assembly asm) {
			Type type = asm.GetTypes().FirstOrDefault(t => typeof(IRemoteObject).IsAssignableFrom(t) && t.IsClass);
			if (type == default(Type)) {
				throw new Exception("Failed to load remote type in " + asm.FullName);
			} else {
				_instance = Activator.CreateInstance(type) as IRemoteObject;
				if (_instance == null) {
					throw new Exception("Failed to create class instance for " + asm.FullName);
				}
			}
		}
	}
}
