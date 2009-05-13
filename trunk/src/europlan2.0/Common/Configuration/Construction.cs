using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	[XmlInclude(typeof(FloorConstruction))]
	public class Construction {
		private string id;
		private string name;
		private ConstructionType type;
		private List<ConstructionLayer> layers;

		public Construction() {
			this.id = "";
			this.name = "";
			this.type = null;
			this.layers = new List<ConstructionLayer>();
		}

		public Construction(string id, string name, ConstructionType type) {
			this.id = id;
			this.name = name;
			this.type = type;
			this.layers = new List<ConstructionLayer>();
		}

		public override bool Equals(object obj) {
			if (obj is Construction) {
				if ((obj as Construction).Id == this.Id) {
					return true;
				}
			}
			return false;
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		[XmlIgnore()]
		public ConstructionType Type {
			get { return type; }
			set { type = value; }
		}

		public string TypeId {
			get { return (type == null ? null : type.Id); }
			set {
				if (value == null) {
					type = null;
				} else {
					type = ConstructionTypeManager.Instance.GetConstructionTypeById(value);
				}
			}
		}

		public ConstructionScopeEnum Scope {
			get {
				if (type == null) {
					return ConstructionScopeEnum.UnknownConstruction;
				} else {
					return this.type.Scope;
				}
			}
		}

		public List<ConstructionLayer> Layers {
			get { return layers; }
			set { layers = value; }
		}

		public float Thickness {
			get {
				float thickness = 0;
				foreach (ConstructionLayer layer in this.layers) {
					thickness += layer.Thickness;
				}
				return thickness;
			}
		}

		public float RValue {
			get {
				float rValue = 0;
				foreach (ConstructionLayer layer in this.layers) {
					rValue += layer.RValue;
				}
				return rValue;
			}
		}
	}
}
