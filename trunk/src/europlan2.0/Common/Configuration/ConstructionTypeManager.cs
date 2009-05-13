using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class ConstructionTypeManager {

		private static ConstructionTypeManager instance = null;

		public static readonly string CT_STD_ESTRICH = "StdEstrich";
		public static readonly string CT_STD_TROCKEN = "StdTrocken";
		public static readonly string CT_USER_ESTRICH = "UserEstrich";
		public static readonly string CT_STD_DAEMM = "StdDaemm";
		public static readonly string CT_USER_DAEMM = "UserDaemm";

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
			type = new ConstructionType(CT_STD_DAEMM, "Standard Wärmedämmkonstruktion", ConstructionScopeEnum.InsulationConstruction, false);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType(CT_USER_DAEMM, "Benutzer Wärmedämmkonstruktion", ConstructionScopeEnum.InsulationConstruction, true);
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
