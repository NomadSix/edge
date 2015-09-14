using System;
using NUnit.Framework;
using Edge.Maestro;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Tests {
	[TestFixture]
	public class MaestroTests {
		[Test]
		public void BasicStartup() {
			var server = new Maestro();
			Task.Run(server.Run);
			Assert.IsFalse(server.isExiting);
		}
	}
}
