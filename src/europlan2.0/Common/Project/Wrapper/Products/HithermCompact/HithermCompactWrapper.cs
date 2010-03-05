using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class HithermCompactWrapper {

		private string floorId;
		private string floorName;
		private string heatOrCool;

		private string roomId;
		private string roomName;
		private string teilSystem;

		private int circuit;
		private double ra5Area;
		private double ra10Area;
		private double pipeHorizontal;
		private double pipeVertical;
		public Dictionary<HithermCompactRegister.HithermCompactRegisterTypeEnum, int> Registers = new Dictionary<HithermCompactRegister.HithermCompactRegisterTypeEnum,int>();
		public List<HithermCompactRegister> RegisterList = new List<HithermCompactRegister>();

		private double roomTemp;
		private double vorlaufTemp;
		private double ruecklaufTemp;
		private double q_Delta;
		private double q_Soll;
		private double q_WH;
		private double q_WHSqm;

		private double lengthConnectionVorlauf;
		private double lengthConnectionRuecklauf;
		//private double lengthWall;
		//private double lengthCircuitAll;

		private double wassermenge;
		private double druckverlustRohr;
		private double druckverlustVerteiler;
		private double v;

		private bool subSystem = false;
		private bool otherSystemsConnected = false;

		public HithermCompactWrapper() {
		}

		public string RoomId {
			get { return roomId; }
			set { roomId = value; }
		}

		public string RoomName {
			get { return roomName; }
			set { roomName = value; }
		}

		public string FloorId {
			get { return floorId; }
			set { floorId = value; }
		}

		public string FloorName {
			get { return floorName; }
			set { floorName = value; }
		}

		public string HeatOrCool {
			get { return heatOrCool; }
			set { heatOrCool = value; }
		}

		public string TeilSystem {
			get { return teilSystem; }
			set { teilSystem = value; }
		}

		public int Circuit {
			get { return circuit; }
			set { circuit = value; }
		}

		public double Ra5Area {
			get { return ra5Area; }
			set { ra5Area = value; }
		}

		public double Ra10Area {
			get { return ra10Area; }
			set { ra10Area = value; }
		}

		public double TotalArea {
			get { return ra5Area + ra10Area; }
		}

		public string RegistersAsString {
			get {
				string temp = "";
				foreach (HithermCompactRegister.HithermCompactRegisterTypeEnum item in Enum.GetValues(typeof(HithermCompactRegister.HithermCompactRegisterTypeEnum))) {
					if (Registers.ContainsKey(item)) {
						temp += new HithermCompactRegister.RegisterTypeEnumConverter(true).ConvertToString(item) + " (" + Registers[item] + "), ";
					}
				}
				temp = temp.Trim();
				temp = temp.TrimEnd(',');
				return temp;
			}
		}

		public double PipeHorizontal {
			get { return pipeHorizontal; }
			set { pipeHorizontal = value; }
		}

		public double PipeVertical {
			get { return pipeVertical; }
			set { pipeVertical = value; }
		}

		public double RoomTemp {
			get { return roomTemp; }
			set { roomTemp = value; }
		}

		public double VorlaufTemp {
			get { return vorlaufTemp; }
			set { vorlaufTemp = value; }
		}

		public double RuecklaufTemp {
			get { return ruecklaufTemp; }
			set { ruecklaufTemp = value; }
		}

		public double QDelta {
			get { return q_Delta; }
			set { q_Delta = value; }
		}

		public double QSoll {
			get { return q_Soll; }
			set { q_Soll = value; }
		}

		public double QWH {
			get { return q_WH; }
			set { q_WH = value; }
		}

		public double QWHSqm {
			get { return q_WHSqm; }
			set { q_WHSqm = value; }
		}

		public double LengthConnectionVorlauf {
			get { return lengthConnectionVorlauf; }
			set { lengthConnectionVorlauf = value; }
		}

		public double LengthConnectionRuecklauf {
			get { return lengthConnectionRuecklauf; }
			set { lengthConnectionRuecklauf = value; }
		}

		public double LengthConnection {
			get { return LengthConnectionVorlauf + LengthConnectionRuecklauf; }
		}

		public double LengthWall {
			get { return PipeHorizontal + PipeVertical; }
		}

		public double LengthCircuitAll {
			get { return LengthConnection + LengthWall; }
		}

		public string WallConstruction {
			get {
				string wallConstruction = "";
				foreach (HithermCompactRegister register in RegisterList) {
					wallConstruction += register.WallId + '\n';
				}
				return wallConstruction.Trim();
			}
		}

		public double Wassermenge {
			get { return wassermenge; }
			set { wassermenge = value; }
		}

		public double DruckverlustRohr {
			get { return druckverlustRohr; }
			set { druckverlustRohr = value; }
		}

		public double DruckverlustVerteiler {
			get { return druckverlustVerteiler; }
			set { druckverlustVerteiler = value; }
		}

		public double V {
			get { return v; }
			set { v = value; }
		}

		public bool SubSystem {
			get { return subSystem; }
			set { subSystem = value; }
		}

		public bool OtherSystemsConnected {
			get { return otherSystemsConnected; }
			set { otherSystemsConnected = value; }
		}

		public string VerlegedatenRegisterType {
			get {
				string registerType = "";
				foreach (HithermCompactRegister register in RegisterList) {
					registerType += new HithermCompactRegister.RegisterTypeEnumConverter().ConvertToString(register.RegisterType) + '\n';
				}
				return registerType.Trim();
			}
		}

		public string VerlegedatenRegisterAnzahl {
			get {
				string registerBreite = "";
				foreach (HithermCompactRegister register in RegisterList) {
					registerBreite += register.RegisterCount.ToString() + '\n';
				}
				return registerBreite.Trim();
			}
		}

	}

}
