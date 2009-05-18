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

			config.Materials.Add(new Material("MAT01", "Testmaterial", "MAT01", 100, "Stk", (float)3.14, null, true));
			config.Materials.Add(new Material("MAT02", "Testmaterial 2", "MAT02", 10, "m", (float)30.14, null, false));
			config.Materials.Add(new Material("MAT03", "Testmaterial 3", "MAT03", 150, "mm²", (float)0.14, null, false));

			Assert.AreEqual(0, config.SerializableMaterials.Count);

			Category category = new Category("euroval", "Euroval", CategoryType.Floor);
			config.Categories.Add(category);
			category = new Category("modulwand", "Modul Wand", CategoryType.Wall);
			config.Categories.Add(category);
			category = new Category("eigenDecke", "Modul Decke", CategoryType.Ceiling);
			config.Categories.Add(category);

			config.MaterialToCategoryMapping.Add("EV15", "euroval");
			config.MaterialToCategoryMapping.Add("MK30", "modulwand");
			config.MaterialToCategoryMapping.Add("MAT01", "eigenDecke");

			config.Type = Configuration.ConfigurationType.AdminConfiguration;
			Assert.AreEqual(0, config.SerializableMaterials.Count);
			config.Save();

			config.Type = Configuration.ConfigurationType.UserConfiguration;
			Assert.AreEqual(1, config.SerializableMaterials.Count);
			config.Save();


			Material material = new Material("MAT01", "Testmaterial", "MAT01", 100, "Stk", (float)3.14, null, true);
			Assert.IsTrue(config.Materials.Contains(material));
			material = new Material("MAT011", "Testmaterial", "MAT01", 100, "Stk", (float)3.14, null, true);
			Assert.IsFalse(config.Materials.Contains(material));
		}

		[Test]
		public void TestConstructaionConfiguration() {

			Construction construction = new Construction("CON01", "Testconstruction", ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_ESTRICH));
			config.Constructions.Add(construction);
			construction = new Construction("CON02", "Testconstruction 2", ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_STD_ESTRICH));
			config.Constructions.Add(construction);

			Assert.AreEqual(2, config.SerializableConstructions.Count);

			config.Type = Configuration.ConfigurationType.AdminConfiguration;
			Assert.AreEqual(1, config.SerializableConstructions.Count);
			config.Save();

			config.Type = Configuration.ConfigurationType.UserConfiguration;
			Assert.AreEqual(1, config.SerializableConstructions.Count);
			config.Save();


			construction = new Construction("CON01", "Testconstruction", ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_ESTRICH));
			Assert.IsTrue(config.Constructions.Contains(construction));
			construction = new Construction("CON011", "Testconstruction", ConstructionTypeManager.Instance.GetConstructionTypeById(ConstructionTypeManager.CT_USER_ESTRICH));
			Assert.IsFalse(config.Constructions.Contains(construction));
		}

		//[Test]
		//public void TestMaterialToCategoryMapping() {
		//    config = Configuration.AdminTemplate;

		//    Category category = new Category("euroval", "Euroval", CategoryType.Floor);
		//    config.Categories.Add(category);
		//    category = new Category("modulwand", "Modul Wand", CategoryType.Wall);
		//    config.Categories.Add(category);

		//    config.MaterialToCategoryMapping.Add("EV15", "euroval");
		//    config.MaterialToCategoryMapping.Add("MK30", "modulwand");

		//    config.Save();

		//}

	}
}
