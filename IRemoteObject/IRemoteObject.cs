using System;
using System.Diagnostics;

namespace Remote {
	public abstract class IRemoteObject {

		public abstract void Init();

		public void GetMemoryUsage() {
			string iName = AppDomain.CurrentDomain.FriendlyName;
			Trace.WriteLine(string.Format("{0} => allocated: {1}, survived: {2}",
				iName,
				AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize,
				AppDomain.CurrentDomain.MonitoringSurvivedMemorySize
			));

			long memAlloc = GC.GetTotalMemory(false);
			Process process = Process.GetCurrentProcess();

			string cpuStats = string.Format("{0} (processor) => user: {1}, privileged: {2}, total: {3}",
				iName,
				process.UserProcessorTime,
				process.PrivilegedProcessorTime,
				process.TotalProcessorTime
			);
			string memStats = string.Format("{0} (memory) => physical: {1}, virtual: {2}, paged: {3}, private: {4}, gc.alloc: {5}",
				iName,
				process.WorkingSet64,
				process.VirtualMemorySize64,
				process.PagedMemorySize64,
				process.PrivateMemorySize64,
				memAlloc
			);
			Trace.WriteLine(cpuStats);
			Trace.WriteLine(memStats);
			
		}

	}
}
