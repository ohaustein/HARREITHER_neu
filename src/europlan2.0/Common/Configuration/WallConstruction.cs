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
				if (obj is RegisterTypeKey) {
					RegisterTypeKey other = obj as RegisterTypeKey;
					return this.registerType == other.registerType && this.usePlus == other.usePlus && this.dachschraege == other.dachschraege;
				}
				return false;
			}

			public override int GetHashCode() {
				return registerType.GetHashCode() + usePlus.GetHashCode() + dachschraege.GetHashCode();
			}
		}

		[XmlIgnore]
		public Dictionary<RegisterTypeKey, string> HithermCompactRegisterMaterialIds {
			get {
				Dictionary<RegisterTypeKey, string> tmp = new Dictionary<RegisterTypeKey, string>();
				if (this.Id == "SCW01") { // Fermacell 10mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false, false), "HC06");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, false), "HC10");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, false), "HC15");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, false), "HC20");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false, false), "HC25");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, false), "HC11");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, false), "HC16");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, false), "HC21");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true , false), "HC08");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true , false), "HC12");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true , false), "HC17");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true , false), "HC22");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true , false), "HC27");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false, true ), "HC56+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, true ), "HC60+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, true ), "HC65+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, true ), "HC70+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false, true ), "HC75+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, true ), "HC61+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, true ), "HC66+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, true ), "HC71+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true , true ), "HC58+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true , true ), "HC62+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true , true ), "HC67+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true , true ), "HC72+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true , true ), "HC77+");
				} else if (this.Id == "SCW02") { // Fermacell 12.5mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false, false), "HF01");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, false), "HF02");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, false), "HF03");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, false), "HF04");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false, false), "HF05");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, false), "HF06");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, false), "HF07");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, false), "HF08");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true , false), "HF09");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true , false), "HF10");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true , false), "HF11");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true , false), "HF12");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true , false), "HF13");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false, true ), "HF21+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, true ), "HF22+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, true ), "HF23+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, true ), "Hf24+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false, true ), "HF25+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, true ), "HF26+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, true ), "HF27+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, true ), "HF28+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true , true ), "HF33+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true , true ), "HF29+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true , true ), "HF30+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true , true ), "HF31+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true , true ), "HF32+");
				} else if (this.Id == "SCW03") { // Fermacell 15mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false, false), "HF50");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, false), "HF51");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, false), "HF52");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, false), "HF53");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false, false), "HF54");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, false), "HF55");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, false), "HF56");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, false), "HF57");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true , false), "HF58");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true , false), "HF59");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true , false), "HF60");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true , false), "HF61");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true , false), "HF62");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false, true ), "HF71+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, true ), "HF72+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, true ), "HF73+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, true ), "HF74+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false, true ), "HF75+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, true ), "HF76+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, true ), "HF77+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, true ), "HF78+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true , true ), "HF83+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true , true ), "HF79+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true , true ), "HF80+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true , true ), "HF81+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true , true ), "HF82+");
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
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false, false), "HG01");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, false), "HG02");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, false), "HG03");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, false), "HG04");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false, false), "HG05");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, false), "HG06");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, false), "HG07");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, false), "HG08");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true , false), "HG09");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true , false), "HG10");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true , false), "HG11");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true , false), "HG12");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true , false), "HG13");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false, true ), "HG21+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, true ), "HG22+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, true ), "HG23+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, true ), "HG24+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false, true ), "HG25+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, true ), "HG26+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, true ), "HG27+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, true ), "HG28+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true , true ), "HG33+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true , true ), "HG29+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true , true ), "HG30+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true , true ), "HG31+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true , true ), "HG32+");
				} else if (this.Id == "SCW06") { // Gipskarton 15mm
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false, false), "HG50");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, false), "HG51");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, false), "HG52");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, false), "HG53");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false, false), "HG54");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, false), "HG55");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, false), "HG56");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, false), "HG57");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true , false), "HG58");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true , false), "HG59");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true , false), "HG60");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true , false), "HG61");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true , false), "HG62");

					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , false, true ), "HG71+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, false, true ), "HG72+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, false, true ), "HG73+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, false, true ), "HG74+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, false, true ), "HG75+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par, false, true ), "HG76+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par, false, true ), "HG77+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par, false, true ), "HG78+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std , true , true ), "HG83+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std, true , true ), "HG79+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std, true , true ), "HG80+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std, true , true ), "HG81+");
					tmp.Add(new RegisterTypeKey(HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std, true , true ), "HG82+");
				}
				return tmp;
			}
		}

	}
}
