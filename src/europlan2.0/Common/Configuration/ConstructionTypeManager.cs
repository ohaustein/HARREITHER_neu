using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class ConstructionTypeManager {

		private static ConstructionTypeManager instance = null;

		public static readonly string CT_STD_ESTRICH = "StdEstrich";
		public static readonly string CT_STD_TROCKEN = "StdTrocken";
		public static readonly string CT_USER_ESTRICH = "UserEstrich";
		public static readonly string CT_USER_TROCKEN = "UserTrocken";
		public static readonly string CT_STD_STAHL = "StdStahl";
		public static readonly string CT_USER_STAHL = "UserStahl";
		public static readonly string CT_STD_TRK_ESTRICH = "StdTrkEstr";
		public static readonly string CT_USER_TRK_ESTRICH = "UserTrkEstr";
		public static readonly string CT_STD_DAEMM = "StdDaemm";
		public static readonly string CT_USER_DAEMM = "UserDaemm";
		public static readonly string CT_STD_DECKE = "StdDecke";
		public static readonly string CT_USER_DECKE = "UserDecke";
		public static readonly string CT_STD_WAND = "StdWand";
		public static readonly string CT_USER_WAND = "UserWand";

		public static ConstructionTypeManager Instance {
			get {
				if (instance == null) {
					instance = new ConstructionTypeManager();
				}
				return instance;
			}
		}

		private Dictionary<String, ConstructionType> constructionTypes;

		private ConstructionTypeManager() {
			this.constructionTypes = new Dictionary<String, ConstructionType>();
			ConstructionType type = new ConstructionType(CT_STD_ESTRICH, "Standard Estrichkonstruktion", ConstructionScopeEnum.FloorConstruction, false);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_STD_TROCKEN, "Standard Trockenkonstruktion", ConstructionScopeEnum.FloorConstruction, false);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_USER_ESTRICH, "Benutzer Estrichkonstruktion", ConstructionScopeEnum.FloorConstruction, true);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_USER_TROCKEN, "Benutzer Trockenkonstruktion", ConstructionScopeEnum.FloorConstruction, true);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_STD_STAHL, "Standard Stahlblechkonstruktion", ConstructionScopeEnum.FloorConstruction, false);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_USER_STAHL, "Benutzer Stahlblechkonstruktion", ConstructionScopeEnum.FloorConstruction, true);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_STD_TRK_ESTRICH, "Standard Trockenestrichkonstruktion", ConstructionScopeEnum.FloorConstruction, false);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_USER_TRK_ESTRICH, "Benutzer Trockenestrichkonstruktion", ConstructionScopeEnum.FloorConstruction, true);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_STD_DAEMM, "Standard Wärmedämmkonstruktion", ConstructionScopeEnum.InsulationConstruction, false);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_USER_DAEMM, "Benutzer Wärmedämmkonstruktion", ConstructionScopeEnum.InsulationConstruction, true);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_STD_DECKE, "Standard Deckenkonstruktion", ConstructionScopeEnum.CeilingConstruction, false);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_USER_DECKE, "Benutzer Deckenkonstruktion", ConstructionScopeEnum.CeilingConstruction, true);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_STD_WAND, "Standard Wandkonstruktion", ConstructionScopeEnum.WallConstruction, false);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_USER_WAND, "Benutzer Wandkonstruktion", ConstructionScopeEnum.WallConstruction, true);
			this.constructionTypes.Add(type.Id, type);
		}

		public ConstructionType GetConstructionTypeById(string id) {
			if (this.constructionTypes.ContainsKey(id)) {
				return this.constructionTypes[id];
			}
			return null;
		}

		public Dictionary<string, ConstructionType>.ValueCollection ConstructionTypes {
			get {
				return this.constructionTypes.Values;
			}
		}
	}
}
