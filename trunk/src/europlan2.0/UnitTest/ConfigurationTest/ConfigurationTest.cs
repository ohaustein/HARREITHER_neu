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

			Assert.AreEqual(3, config.SerializableMaterials.Count);

			config.Type = Configuration.ConfigurationType.AdminConfiguration;
			Assert.AreEqual(2, config.SerializableMaterials.Count);
			config.Save();

			config.Type = Configuration.ConfigurationType.UserConfiguration;
			Assert.AreEqual(1, config.SerializableMaterials.Count);
			config.Save();


			Material material = new Material("MAT01", "Testmaterial", "MAT01", 100, "Stk", (float)3.14, MaterialTypeEnum.Euroval, true);
			Assert.IsTrue(config.Materials.Contains(material));
			material = new Material("MAT011", "Testmaterial", "MAT01", 100, "Stk", (float)3.14, MaterialTypeEnum.Euroval, true);
			Assert.IsFalse(config.Materials.Contains(material));
		}

		[Test]
		public void TestConstructaionConfiguration() {

			ConstructionType userConType = new ConstructionType("TYPE01", "Testtype", ConstructionScopeEnum.FloorConstruction, true);
			ConstructionType adminConType = new ConstructionType("TYPE02", "Testtype 2", ConstructionScopeEnum.FloorConstruction, false);
			
			Construction construction = new Construction("CON01", "Testconstruction", userConType);
			config.Constructions.Add(construction);
			construction = new Construction("CON02", "Testconstruction 2", adminConType);
			config.Constructions.Add(construction);

			Assert.AreEqual(2, config.SerializableConstructions.Count);

			config.Type = Configuration.ConfigurationType.AdminConfiguration;
			Assert.AreEqual(1, config.SerializableConstructions.Count);
			config.Save();

			config.Type = Configuration.ConfigurationType.UserConfiguration;
			Assert.AreEqual(1, config.SerializableConstructions.Count);
			config.Save();


			construction = new Construction("CON01", "Testconstruction", userConType);
			Assert.IsTrue(config.Constructions.Contains(construction));
			construction = new Construction("CON011", "Testconstruction", userConType);
			Assert.IsFalse(config.Constructions.Contains(construction));
		}

	}
}
