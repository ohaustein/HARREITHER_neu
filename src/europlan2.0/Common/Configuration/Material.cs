using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Europlan.Common {
	public class Material: IEditableObject {

		private string id;
		private string name;
		private string partNumber;
		private Nullable<int> denomination; // Stückelung
		private string unit;
		private float price;
		private string discountGroup;
		private bool userDefined;
		private bool additional;
		private Category category;
		private bool isNew = false;

		public Material() {
			this.id = System.Guid.NewGuid().ToString();
			this.name = "";
			this.partNumber = "";
			this.denomination = null;
			this.unit = "";
			this.price = 0;
			this.discountGroup = "";
			this.userDefined = false;
			this.additional = false;
			this.category = null;
		}

		public Material(string id, string name, string partNumber, Nullable<int> denomination, string unit, float price, string discountGroup, Category category, bool userDefined, bool additional) {
			this.id = id;
			this.name = name;
			this.partNumber = partNumber;
			this.denomination = denomination;
			this.unit = unit;
			this.price = price;
			this.discountGroup = discountGroup;
			this.userDefined = userDefined;
			this.additional = additional;
			this.category = category;
		}

		public override bool Equals(object obj) {
			if (obj is Material) {
				if ((obj as Material).Id == this.Id) {
					return true;
				}
			}
			return base.Equals(obj);
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public string Id {
			get { return this.id; }
			set { this.id = value; }
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
				resId = "Material_" + resId;
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
			get { return this.name; }
			set { this.name = value; }
		}

		public string PartNumber {
			get { return this.partNumber; }
			set { this.partNumber = value; }
		}

		public Nullable<int> Denomination {
			get { return this.denomination; }
			set { this.denomination = value; }
		}

		public string Unit {
			get { return this.unit; }
			set { this.unit = value; }
		}

		public float Price {
			get { return this.price; }
			set { this.price = value; }
		}

		[XmlIgnore]
		public float PricePerUnit {
			get {
				if (Denomination.HasValue && Project.Instance.Config.MaterialIdsWithPricePerPackage.Contains(Id)) {
					return Price / (float)Denomination.Value;
				} else {
					return Price;
				}
			}
		}

		[XmlIgnore]
		public string DiscountGroup {
			get { return this.discountGroup; }
			set { this.discountGroup = value; }
		}

		public bool UserDefined {
			get { return this.userDefined; }
			set { this.userDefined = value; }
		}

		public bool Additional {
			get { return this.additional; }
			set { this.additional = value; }
		}

		[XmlIgnore]
		public Category Category {
			get { return this.category; }
			set { this.category = value; }
		}

		#region IEditableObject Members
		public void BeginEdit() {
			// we only need to implement implement CancelEdit for deleting new rows that were cancelled
		}

		public void CancelEdit() {
			if (this.isNew) {
				if (Configuration.UserTemplate.Materials.Contains(this)) {
					Configuration.UserTemplate.Materials.Remove(this);
				}
				if (Configuration.AdminTemplate.Materials.Contains(this)) {
					Configuration.AdminTemplate.Materials.Remove(this);
				}
			}
		}

		public void EndEdit() {
			// we only need to implement implement CancelEdit for deleting new rows that were cancelled
			this.isNew = false;
		}
		#endregion

		public void Cleanup() {
			if (this.isNew) {
				this.CancelEdit();
			} else {
				this.EndEdit();
			}
		}

		[XmlIgnore]
		internal bool IsNew {
			get { return this.isNew; }
			set { this.isNew = value; }
		}
	}
}
