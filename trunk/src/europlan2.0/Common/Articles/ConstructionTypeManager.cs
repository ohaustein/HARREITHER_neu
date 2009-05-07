using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class ConstructionTypeManager {

		private static ConstructionTypeManager instance = null;

		public ConstructionTypeManager Instance {
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
			ConstructionType type = new ConstructionType("StdEstrich", "Standard Estrichkonstruktion", ConstructionScopeEnum.FloorConstruction, false);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType("StdTrocken", "Standard Trockenkonstruktion", ConstructionScopeEnum.FloorConstruction, false);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType("UserEstrich", "Benutzer Estrichkonstruktion", ConstructionScopeEnum.FloorConstruction, true);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType("StdDämm", "Standard Wärmedämmkonstruktion", ConstructionScopeEnum.PanelConstruction, false);
			this.constructionTypes.Add(type.Id, type);
			type = new ConstructionType("UserDämm", "Benutzer Wärmedämmkonstruktion", ConstructionScopeEnum.PanelConstruction, false);
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
