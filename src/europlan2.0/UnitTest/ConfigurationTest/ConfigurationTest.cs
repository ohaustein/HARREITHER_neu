using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Europlan.Common;

namespace Europlan.UnitTest {

	[TestFixture] // telling NUnit that this class contains test functions 
	public class ConfigurationTest {

		Configuration config;

		[SetUp]
		public void SetUp() {
			config = new Configuration();
		}

		[TearDown]
		public void TearDown() {
		}

		[Test]
		public void TestConfiguration() {
		}

	}
}
