﻿using System;

namespace Edge.Atlas {
	class AtlasEntry {
		public static void Main(string[] args) {
			if(args.Length > 0)
				new Atlas(Int32.Parse(args[0]), false).Run();
			else
				new Atlas(2348, false).Run();
		}
	}
}
