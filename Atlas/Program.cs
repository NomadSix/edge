using System;

namespace Edge.Atlas {
	class AtlasEntry {
		public static void Main(string[] args) {
			new Atlas(Int32.Parse(args[0]), false).Run();
		}
	}
}
