using System;
using NUnit.Framework;


namespace Acr.Settings.Android.Tests {

	[TestFixture]
	public class NativeTests {

		[Test]
		public void StaticInstance() {
			Settings.Instance.Get("Init", "");
		}
	}
}

