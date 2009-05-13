using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {
	public class Material {
		private string id;
		private string name;
		private string partNumber;
		private Nullable<int> denomination; // Stückelung
		private string unit;
		private float price;
		private MaterialTypeEnum type;
		private bool userDefined;

		public Material() {
			this.id = "";
			this.name = "";
			this.partNumber = "";
			this.denomination = null;
			this.unit = "";
			this.price = 0;
			this.type = MaterialTypeEnum.General;
			this.userDefined = false;
		}

		public Material(string id, string name, string partNumber, Nullable<int> denomination, string unit, float price, MaterialTypeEnum type, bool userDefined) {
			this.id = id;
			this.name = name;
			this.partNumber = partNumber;
			this.denomination = denomination;
			this.unit = unit;
			this.price = price;
			this.type = type;
			this.userDefined = userDefined;
		}

		public override bool Equals(object obj) {
			if (obj is Material) {
				if ((obj as Material).Id == this.Id) {
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

		public string PartNumber {
			get { return partNumber; }
			set { partNumber = value; }
		}

		public Nullable<int> Denomination {
			get { return denomination; }
			set { denomination = value; }
		}

		public string Unit {
			get { return unit; }
			set { unit = value; }
		}

		public float Price {
			get { return price; }
			set { price = value; }
		}

		public MaterialTypeEnum Type {
			get { return type; }
			set { type = value; }
		}

		public bool UserDefined {
			get { return userDefined; }
			set { userDefined = value; }
		}
	}
}
