using NUnit.Framework;
using System;

namespace Edge.Tests {
	[TestFixture]
	public class NUnit {
		//Full List of Constraints can be found here https://github.com/nunit/nunit/wiki/Constraints
		[Test]
		public void BasicAssert() {
			//Just FYI, these are old style tests, maybe a little easier to write,
			// but the constraint engine shows what goes on much better
			Assert.IsTrue(true);
			Assert.AreEqual(1, 1);
		}

		[Test]
		public void HasConstraints() {
			int[] array = { 1, 2, 3 };
			Assert.That(array, Has.Exactly(1).EqualTo(3));
			Assert.That(array, Has.Exactly(2).GreaterThan(1));
			Assert.That(array, Has.Exactly(3).LessThan(100));

			Assert.That(array, Has.None.Negative);
			Assert.That(array, Has.All.Positive);
			Assert.That(array, Has.Some.GreaterThan(2));

			Assert.That(array, Has.Length.AtLeast(2));
		}

		[Test]
		public void IsConstraints() {
			int[] array = { 1, 2, 3 };
			Assert.That(array, Is.Not.Null);
			Assert.That(array, Is.Unique);

			Assert.That("Hello", Is.EqualTo("Hello"));

		}
	}
}

