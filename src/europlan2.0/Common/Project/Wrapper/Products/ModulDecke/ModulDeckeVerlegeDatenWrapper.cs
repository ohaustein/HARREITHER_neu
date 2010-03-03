using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class ModulDeckeVerlegeDatenWrapper {

		private string floorId;
		private string floorName;
		private string roomId;
		private string roomName;
		private string teilSystem;

		private int circuit;
		private int teilFlaeche;
		private int reihe;

		public List<KlimaFlaechenModul> Modules = new List<KlimaFlaechenModul>();


		public ModulDeckeVerlegeDatenWrapper() {
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

		public string TeilSystem {
			get { return teilSystem; }
			set { teilSystem = value; }
		}

		public int Circuit {
			get { return circuit; }
			set { circuit = value; }
		}

		public int TeilFlaeche {
			get { return teilFlaeche; }
			set { teilFlaeche = value; }
		}

		public int Reihe {
			get { return reihe; }
			set { reihe = value; }
		}

		public string ModulesAsString {
			get {
				string temp = "";

				KlimaFlaechenModul.ModulTypeEnum prevModuleType = KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30;
				bool first = true;
				int moduleCount = 0;

				foreach (KlimaFlaechenModul modul in Modules) {
					if (first || modul.ModulType == prevModuleType) {
						moduleCount++;
						first = false;
					} else {
						temp += moduleCount.ToString() + "x " + new KlimaFlaechenModul.ModulTypeEnumConverter().ConvertToString(prevModuleType) + ", ";
						moduleCount = 1;
					}
					prevModuleType = modul.ModulType;
				}

				temp += moduleCount.ToString() + "x " + new KlimaFlaechenModul.ModulTypeEnumConverter().ConvertToString(prevModuleType) + ", ";

				temp = temp.Trim();
				temp = temp.TrimEnd(',');
				// TODO Localization
				temp = temp.Replace("Modul ", "");
				return temp;
			}
		}

	}

}
