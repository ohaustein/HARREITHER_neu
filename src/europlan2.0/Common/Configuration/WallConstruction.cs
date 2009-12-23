using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Europlan.Common {
	public class WallConstruction : Construction {

		private double factor = 1;
		private bool isHithermWall = false;
		private bool isHithermCompactWall = false;
		private HithermWall defaultWall = null;
		//private Dictionary<HithermCompactRegister.HithermCompactRegisterTypeEnum, Material> hithermCompactRegisterMaterials = new Dictionary<HithermCompactRegister.HithermCompactRegisterTypeEnum,Material>();
		//private SerializableDictionary<HithermCompactRegister.HithermCompactRegisterTypeEnum, string> hithermCompactRegisterPartsNrs = null;

		public WallConstruction() : base() {
		}

		public WallConstruction(string id, string name, ConstructionType type, double factor)
			: base(id, name, type) {
			this.factor = factor;
		}

		public override Construction Clone() {
			WallConstruction construction = new WallConstruction(this.Id, this.Name, this.Type, this.Factor);
			return construction;
		}

		public double Factor {
			get { return this.factor; }
			set { this.factor = value; }
		}

		public bool IsHithermWall {
			get { return this.isHithermWall; }
			set { this.isHithermWall = value; }
		}

		public bool IsHithermCompactWall {
			get { return this.isHithermCompactWall; }
			set { this.isHithermCompactWall = value; }
		}

		[XmlIgnore]
		public HithermWall DefaultWall {
			get {
				if (defaultWall == null) {
					defaultWall = new HithermWall(this.Id, this.Name, this, null, null, false, null, -16, 30, true);
				}
				return defaultWall;
			}
		}

		public class RegisterTypeKey {
			HithermCompactRegister.HithermCompactRegisterTypeEnum registerType = HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std;
			bool usePlus = false;
			bool dachschraege = false;

			public RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum registerType, bool dachschraege, bool usePlus) {
				this.registerType = registerType;
				this.usePlus = usePlus;
				this.dachschraege = dachschraege;
			}

			public override bool Equals(object obj) {
				return base.Equals(obj);
			}

			public override int GetHashCode() {
				return registerType.GetHashCode() + usePlus.GetHashCode() + dachschraege.GetHashCode();
			}
		}

		[XmlIgnore]
		public Dictionary<RegisterTypeKey, string> HithermCompactRegisterMaterialIds {
			get {
				/*if (this.hithermCompactRegisterPartsNrs != null) {
					foreach (KeyValuePair<HithermCompactRegister.HithermCompactRegisterTypeEnum, string> kvp in this.hithermCompactRegisterPartsNrs) {
						Project.Instance 
					}
					this.hithermCompactRegisterPartsNrs = null;
				}
				return this.hithermCompactRegisterMaterials;*/
				Dictionary<RegisterTypeKey, string> tmp = new Dictionary<RegisterTypeKey, string>();
				if (this.Id == "SCW01") { // Fermacell 10mm
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true, false), "");

					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, true), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, true), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, true), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, true), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, true), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, true), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true, true), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true, true), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true, true), "");
				} else if (this.Id == "SCW02") { // Fermacell 12.5mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, false), "HF02");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, false), "HF03");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, false), "HF04");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, false), "HF06");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, false), "HF07");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, false), "HF08");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true, false), "HF11");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true, false), "HF12");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, true), "HF22");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, true), "HF23");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, true), "HF24");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, true), "HF26");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, true), "HF27");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, true), "HF28");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true, true), "HF29");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true, true), "HF30");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true, true), "HF31");
				} else if (this.Id == "SCW03") { // Fermacell 15mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true, false), "");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true, true), "");
				} else if (this.Id == "SCW04") { // Gipskarton 10mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true, false), "");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true, true), "");
				} else if (this.Id == "SCW05") { // Gipskarton 12.5mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true, false), "");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true, true), "");
				} else if (this.Id == "SCW06") { // Gipskarton 15mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true, false), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true, false), "");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true, true), "");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true, true), "");
				}
				return tmp;
			}
		}

		/*public SerializableDictionary<HithermCompactRegister.HithermCompactRegisterTypeEnum, string> HithermCompactRegisterPartNrs {
			get {
				this.hithermCompactRegisterPartsNrs = new SerializableDictionary<HithermCompactRegister.HithermCompactRegisterTypeEnum, string>();
				foreach (KeyValuePair<HithermCompactRegister.HithermCompactRegisterTypeEnum, Material> kvp in this.HithermCompactRegisterMaterials) {
					this.hithermCompactRegisterPartsNrs.Add(kvp.Key, kvp.Value.Id);
				}
				return this.hithermCompactRegisterPartsNrs; ;
			}
			set {
				this.hithermCompactRegisterPartsNrs = value;
			}
		}*/
	}
}
