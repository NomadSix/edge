using System;

namespace Hyperion.Linux {
	static class Program {
		[STAThread]
		static void Main(string[] args) {
			new Edge.Hyperion.Hyperion().Run();
		}
	}
}