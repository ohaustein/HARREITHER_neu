using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using log4net;
using System.Globalization;

namespace Europlan.Application {
	
	public class SOLAR32Importer : IBuildingDataImporter {

		private static readonly ILog log = LogManager.GetLogger(typeof(SOLAR32Importer));
		private const string FLOOR = "[EBENE]";
		private const string ROOM = "[FBEZUG]";

		public SOLAR32Importer() {

		}

		public string FileExtension {
			get { 
				return ".h72";
			}
		}

		public string FileExtensionFilter {
			get { 
				return "SOLAR 32 (*.h72)|*.h72";
			}
		}

		public FloorList ImportBuildingDataFromFile(string fileName) {
			try {
				using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read)) {
					using (StreamReader sr = new StreamReader(fs, Encoding.Default)) {
						return ParseData(sr);
					}
				}
			} catch (IOException ex) {
				log.Error("Could not parse file " + fileName, ex);
			}
			return null;
		}

		private FloorList ParseData(StreamReader reader) {
			FloorList floors = new FloorList();
			string line = "";
			Floor floor = null;
			Room room = null;
			while (!reader.EndOfStream) {
				line = reader.ReadLine();
				line = line.Trim();
				if (line == FLOOR) {
					floor = new Floor();
					floors.Add(floor);
				} else if (line == ROOM) {
					room = new Room();
					if (floor != null) {
						floor.Rooms.Add(room);
					}
				} else if (line.StartsWith("CODE")) {
					string code = (line.Split('='))[1].Trim('\"');
					if ((floor != null) && (floor.Id == "")) {
						floor.Id = code;
					} else if ((room != null) && (room.Id == "")) {
						room.Id = code;
					}
				} else if (line.StartsWith("TEXT")) {
					string text = (line.Split('='))[1].Trim('\"');
					if ((floor != null) && (floor.Name == "")) {
						floor.Name = text;
					} else if ((room != null) && (room.Name == "")) {
						room.Name = text;
					}
				} else if (line.StartsWith("TEMP_HEIZ")) {
					string temp = (line.Split('='))[1];
					if (room != null) {
						room.RoomTemperature = (int)float.Parse(temp, System.Globalization.CultureInfo.CreateSpecificCulture("en-us"));
					}
				} else if (line.StartsWith("A_RAUM")) {
					string area = (line.Split('='))[1];
					if (room != null) {
						room.Area = float.Parse(area, System.Globalization.CultureInfo.CreateSpecificCulture("en-us"));
					}
				} else if (line.StartsWith("Q_NORM")) {
					string heat = (line.Split('='))[1];
					if (room != null) {
						room.HeatPower = Int32.Parse(heat);
					}
				} else if (line.StartsWith("Q_BEREIN")) {
					string normHeat = (line.Split('='))[1];
					if (room != null) {
						room.NormalizedHeatPower = Int32.Parse(normHeat);
					}
				}
			}
			return floors;
		}

	}

}
