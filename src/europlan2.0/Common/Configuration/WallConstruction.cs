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
					defaultWall = new HithermWall(this.Id, this.LocalizedName, this, null, null, false, null, -16, 30, true);
				}
				return defaultWall;
			}
		}

		public class RegisterTypeKey {
			HithermCompactRegister.HithermCompactRegisterTypeEnum registerType = HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std;
			bool usePlus = false;

			public RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum registerType, bool usePlus) {
				this.registerType = registerType;
				this.usePlus = usePlus;
			}

			public override bool Equals(object obj) {
				if (obj is RegisterTypeKey) {
					RegisterTypeKey other = obj as RegisterTypeKey;
					return this.registerType == other.registerType && this.usePlus == other.usePlus;
				}
				return false;
			}

			public override int GetHashCode() {
				return registerType.GetHashCode() + usePlus.GetHashCode();
			}
		}

		[XmlIgnore]
		public Dictionary<RegisterTypeKey, string> HithermCompactRegisterMaterialIds {
			get {
				Dictionary<RegisterTypeKey, string> tmp = new Dictionary<RegisterTypeKey, string>();
				if (this.Id == "SCW01") { // Fermacell 10mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false), "HC06");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false), "HC10");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false), "HC15");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false), "HC20");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false), "HC25");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false), "HC11");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false), "HC16");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false), "HC21");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Ds , false), "HC08");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Ds, false), "HC12");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Ds, false), "HC17");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Ds, false), "HC22");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Ds, false), "HC27");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true ), "HC56+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true ), "HC60+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true ), "HC65+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true ), "HC70+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true ), "HC75+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, true ), "HC61+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, true ), "HC66+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, true ), "HC71+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Ds , true ), "HC58+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Ds, true ), "HC62+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Ds, true ), "HC67+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Ds, true ), "HC72+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Ds, true ), "HC77+");
				} else if (this.Id == "SCW02") { // Fermacell 12.5mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false), "HF01");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false), "HF02");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false), "HF03");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false), "HF04");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false), "HF05");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false), "HF06");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false), "HF07");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false), "HF08");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Ds , false), "HF09");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Ds, false), "HF10");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Ds, false), "HF11");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Ds, false), "HF12");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Ds, false), "HF13");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true ), "HF21+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true ), "HF22+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true ), "HF23+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true ), "Hf24+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true ), "HF25+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, true ), "HF26+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, true ), "HF27+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, true ), "HF28+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Ds , true ), "HF33+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Ds, true ), "HF29+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Ds, true ), "HF30+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Ds, true ), "HF31+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Ds, true ), "HF32+");
				} else if (this.Id == "SCW03") { // Fermacell 15mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false), "HF50");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false), "HF51");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false), "HF52");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false), "HF53");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false), "HF54");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false), "HF55");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false), "HF56");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false), "HF57");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Ds , false), "HF58");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Ds, false), "HF59");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Ds, false), "HF60");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Ds, false), "HF61");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Ds, false), "HF62");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true ), "HF71+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true ), "HF72+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true ), "HF73+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true ), "HF74+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true ), "HF75+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, true ), "HF76+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, true ), "HF77+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, true ), "HF78+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Ds , true ), "HF83+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Ds, true ), "HF79+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Ds, true ), "HF80+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Ds, true ), "HF81+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Ds, true ), "HF82+");
				} else if (this.Id == "SCW04") { // Gipskarton 10mm
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true , false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true , false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true , false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true , false), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true , false), "");

					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false, true ), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, true ), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, true ), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, true ), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false, true ), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, true ), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, true ), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, true ), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true , true ), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true , true ), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true , true ), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true , true ), "");
					//tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true , true ), "");
				} else if (this.Id == "SCW05") { // Gipskarton 12.5mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false), "HG01");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false), "HG02");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false), "HG03");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false), "HG04");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false), "HG05");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false), "HG06");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false), "HG07");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false), "HG08");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Ds , false), "HG09");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Ds, false), "HG10");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Ds, false), "HG11");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Ds, false), "HG12");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Ds, false), "HG13");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true ), "HG21+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true ), "HG22+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true ), "HG23+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true ), "HG24+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true ), "HG25+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, true ), "HG26+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, true ), "HG27+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, true ), "HG28+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Ds , true ), "HG33+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Ds, true ), "HG29+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Ds, true ), "HG30+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Ds, true ), "HG31+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Ds, true ), "HG32+");
				} else if (this.Id == "SCW06") { // Gipskarton 15mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false), "HG50");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false), "HG51");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false), "HG52");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false), "HG53");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false), "HG54");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false), "HG55");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false), "HG56");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false), "HG57");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Ds , false), "HG58");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Ds, false), "HG59");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Ds, false), "HG60");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Ds, false), "HG61");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Ds, false), "HG62");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true ), "HG71+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true ), "HG72+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true ), "HG73+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true ), "HG74+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true ), "HG75+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, true ), "HG76+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, true ), "HG77+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, true ), "HG78+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Ds , true ), "HG83+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Ds, true ), "HG79+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Ds, true ), "HG80+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Ds, true ), "HG81+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Ds, true ), "HG82+");
				}
				return tmp;
			}
		}

	}
}
