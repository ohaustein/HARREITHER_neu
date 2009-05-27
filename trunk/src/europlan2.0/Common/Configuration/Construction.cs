using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	
	[XmlInclude(typeof(FloorConstruction))]
	public class Construction {
		private string id;
		private int version;
		private string name;
		private ConstructionType type;
		private List<ConstructionLayer> layers;
		private List<Construction> versionedConstructions;
		private bool hasBeenDeleted = false;

		public Construction() {
			this.id = "";
			this.version = 0;
			this.name = "";
			this.type = null;
			this.layers = new List<ConstructionLayer>();
			this.versionedConstructions = new List<Construction>();
		}

		public Construction(string id, string name, ConstructionType type) {
			this.id = id;
			this.version = 0;
			this.name = name;
			this.type = type;
			this.layers = new List<ConstructionLayer>();
			this.versionedConstructions = new List<Construction>();
		}

		public override bool Equals(object obj) {
			if (obj is Construction) {
				Construction construction = obj as Construction;
				if ((construction.Id == this.Id) && 
					(construction.name == this.name) && 
					(construction.version == this.version) && 
					(construction.layers.Count == this.layers.Count)) {
					foreach (ConstructionLayer layer in this.layers) {
						if (!construction.layers.Contains(layer)) {
							return false;
						}
					}
					return true;
				}
			}
			return base.Equals(obj);
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public void InitializeVersion(int version) {
			if (versionedConstructions.Count >= version) {
				Construction versioned = versionedConstructions[version];
				this.name = versioned.name;
				this.type = versioned.type;
				this.layers = versioned.layers;
			}
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public int VersionCount {
			get { return this.versionedConstructions.Count; }
		}

		public int Version {
			get { return this.version; }
			set { this.version = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public bool HasBeenDeleted {
			get { return hasBeenDeleted; }
			set { hasBeenDeleted = value; }
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

		public List<Construction> VersionedConstructions {
			get { return versionedConstructions; }
			set { versionedConstructions = value; }
		}

		public List<ConstructionLayer> Layers {
			get { return layers; }
			set { layers = value; }
		}

		[XmlIgnore]
		public float Thickness {
			get {
				float thickness = 0;
				foreach (ConstructionLayer layer in this.layers) {
					thickness += layer.Thickness;
				}
				return thickness;
			}
		}

		[XmlIgnore]
		public float RValue {
			get {
				float rValue = 0;
				foreach (ConstructionLayer layer in this.layers) {
					rValue += layer.RValue;
				}
				return rValue;
			}
		}

		public void UpdateVersions() {
			Construction versioned = null;
			if (versionedConstructions.Count > 0) {
				Construction versionedConstruction = versionedConstructions[versionedConstructions.Count - 1];
				if (!versionedConstruction.Equals(this)) {
					versioned = this.Clone();
					foreach (ConstructionLayer layer in this.layers) {
						versioned.layers.Add(layer);
					}
				}
			} else {
				versioned = this.Clone();
				foreach (ConstructionLayer layer in this.layers) {
					versioned.layers.Add(layer);
				}
			}
			if (versioned != null) {
				this.versionedConstructions.Add(versioned);
				this.version = versionedConstructions.Count;
				versioned.version = this.version;
			}
		}

		public virtual Construction Clone() {
			throw new Exception("No implementation for Clone() in class Construction");
		}

	}
}
