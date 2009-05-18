using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {

	public enum CategoryType {
		Floor,
		Wall,
		Ceiling,
		Distributor,
		Insulation,
		General
	}

	public class Category : IComparable {

		private string id;
		private string name;
		private CategoryType type;
		private List<Material> materials;
		private int order;

		public Category() {
			this.id = System.Guid.NewGuid().ToString();
			this.name = "";
			this.type = CategoryType.General;
			this.materials = new List<Material>();
			this.order = 0;
		}

		public Category(string id, string name, CategoryType type, int order) {
			this.id = id;
			this.name = name;
			this.type = type;
			this.materials = new List<Material>();
			this.order = order;
		}

		public override bool Equals(object obj) {
			if (obj is Category) {
				if ((obj as Category).Id == this.Id) {
					return true;
				}
			}
			return base.Equals(obj);
		}

		public override string ToString() {
			return this.name;
		}

		public int CompareTo(object obj) {
			if (obj is Category) {
				return this.Order.CompareTo((obj as Category).Order);
			}
			return 0;
		}

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public CategoryType Type {
			get { return type; }
			set { type = value; }
		}

		[XmlIgnore]
		public List<Material> Materials {
			get {
				return this.materials;
			}
			set {
				this.materials = value;
			}
		}

		public int Order {
			get { return this.order; }
			set { this.order = value; }
		}

	}

}
