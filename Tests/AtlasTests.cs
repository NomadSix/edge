using NUnit.Framework;
using Edge.Atlas;
using System.Threading.Tasks;

namespace Tests {
	[TestFixture]
	public class AtlasTests {
		[Test]
		public void BasicStartup() {
			var server = new Atlas(2348, true);
			Task.Run(server.Run);
			Assert.False(server.isExiting);
		}
	}
}

