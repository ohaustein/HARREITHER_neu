using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Threading;

namespace Europlan.Common {

	public class CategoryTypeEnumConverter : System.ComponentModel.TypeConverter {
		private static readonly string floor = EuroplanRes.Category_Fussbodenheizung; //"Fußbodenheizung"
		private static readonly string wall = EuroplanRes.Category_Wandheizung; //"Wandheizung"
		private static readonly string ceiling = EuroplanRes.Category_Deckenkuehlung; //"Deckenkühlung"
		private static readonly string distributor = EuroplanRes.Category_Verteiler; //"Verteiler"
		private static readonly string insulation = EuroplanRes.Category_Daemmung; //"Dämmung"
		private static readonly string general = EuroplanRes.Category_Allgemein; //"Allgemein"

		private Dictionary<string, CategoryType> mappingFromString = new Dictionary<string, CategoryType>();
		private Dictionary<CategoryType, string> mappingToString = new Dictionary<CategoryType, string>();

		public CategoryTypeEnumConverter() {
			mappingFromString.Add(floor, CategoryType.Floor);
			mappingFromString.Add(wall, CategoryType.Wall);
			mappingFromString.Add(ceiling, CategoryType.Ceiling);
			mappingFromString.Add(distributor, CategoryType.Distributor);
			mappingFromString.Add(insulation, CategoryType.Insulation);
			mappingFromString.Add(general, CategoryType.General);
			mappingToString.Add(CategoryType.Floor, floor);
			mappingToString.Add(CategoryType.Wall, wall);
			mappingToString.Add(CategoryType.Ceiling, ceiling);
			mappingToString.Add(CategoryType.Distributor, distributor);
			mappingToString.Add(CategoryType.Insulation, insulation);
			mappingToString.Add(CategoryType.General, general);
		}

		public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType) {
			return true;
		}

		public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType) {
			return true;
		}

		public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
			if (value is string) {
				if (mappingFromString.ContainsKey((string)value)) {
					return mappingFromString[(string)value];
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
			if (value is CategoryType && destinationType == typeof(string)) {
				if (mappingToString.ContainsKey((CategoryType)value)) {
					return mappingToString[(CategoryType)value];
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

	[System.ComponentModel.TypeConverter(typeof(CategoryTypeEnumConverter))]
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

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public override string ToString() {
			return this.LocalizedName;
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

		public bool ResourceOk {
			get {
				string resKey = this.ResKey;
				if (resKey == null) {
					return false;
				}
				return EuroplanRes.ResourceManager.GetString(resKey) != null;
			}
		}

		private string ResKey {
			get {
				if (string.IsNullOrEmpty(this.id)) {
					return null;
				}
				string resId = this.id.Replace("+", "plus");
				resId = resId.Replace("-", "_");
				resId = "Category_" + resId;
				return resId;
			}
		}

		public string LocalizedName {
			get {
				string localizedName = null;
				string resKey = this.ResKey;
				if (resKey != null) {
					localizedName = EuroplanRes.ResourceManager.GetString(resKey);
					if (!string.IsNullOrEmpty(localizedName)) {
						return localizedName;
					}
				}
				return this.Name;
			}
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
