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
		public void TestMaterialConfiguration() {

			config.Materials.Add(new Material("MAT01", "Testmaterial", "MAT01", 100, "Stk", (float)3.14, MaterialTypeEnum.Euroval, true));
			config.Materials.Add(new Material("MAT02", "Testmaterial 2", "MAT02", 10, "m", (float)30.14, MaterialTypeEnum.Euroval, false));
			config.Materials.Add(new Material("MAT03", "Testmaterial 3", "MAT03", 150, "mm²", (float)0.14, MaterialTypeEnum.General, false));

			Assert.AreEqual(0, config.SerializableMaterials.Count);

			config.Type = Configuration.ConfigurationType.AdminConfiguration;
			Assert.AreEqual(2, config.SerializableMaterials.Count);

			config.Type = Configuration.ConfigurationType.ProjectConfiguration;
			Assert.AreEqual(1, config.SerializableMaterials.Count);

		}

	}
}
