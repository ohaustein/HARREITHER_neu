using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class ModulBodenWrapper {

		private string floorId;
		private string floorName;
		private string heatOrCool;

		private string roomId;
		private string roomName;
		private string teilSystem;
		private string insideConstruction;
		private double insideRValue;
		private string outsideConstruction;
		private double outsideRValue;
		private int circuits;
		private double dichtArea;
		private double modulierendArea;
		private double sonstigeArea;
		private double connectionArea;

		private double roomTemp;
		private double vorlaufTemp;
		private double ruecklaufTemp;
		private double Q_Soll;
		private double Q_FBH;
		private double q_FBH;
		private double t_FB;

		private string circuitsAsString;
		private double totalModules;
		private double lengthConnection;
		private double wassermenge;
		private double druckverlustHeizkreis;
		private double druckverlustVerteiler;
		private double v;

		private double unusedArea;

		private bool subSystem = false;
		private bool otherSystemsConnected = false;
		private bool usedAsCircuitWrapper = false;

		public ModulBodenWrapper() {
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

		public string InsideConstruction {
			get { return insideConstruction; }
			set { insideConstruction = value; }
		}

		public double InsideRValue {
			get { return insideRValue; }
			set { insideRValue = value; }
		}

		public string OutsideConstruction {
			get { return outsideConstruction; }
			set { outsideConstruction = value; }
		}

		public double OutsideRValue {
			get { return outsideRValue; }
			set { outsideRValue = value; }
		}

		public int Circuits {
			get { return circuits; }
			set { circuits = value; }
		}

		public double DichtArea {
			get { return dichtArea; }
			set { dichtArea = value; }
		}

		public double ModulierendArea {
			get { return modulierendArea; }
			set { modulierendArea = value; }
		}

		public double SonstigeArea {
			get { return sonstigeArea; }
			set { sonstigeArea = value; }
		}

		public double ConnectionArea {
			get { return connectionArea; }
			set { connectionArea = value; }
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

		public double QSoll {
			get { return Q_Soll; }
			set { Q_Soll = value; }
		}

		public double QFBH {
		    get { return Q_FBH; }
		    set { Q_FBH = value; }
		}

		public double qFBHSqm {
		    get { return Q_FBH / (DichtArea + ModulierendArea + SonstigeArea + UnusedArea); }
		}

		public double tFB {
		    get { return t_FB; }
		    set { t_FB = value; }
		}

		public string CircuitsAsString {
			get { return circuitsAsString; }
			set { circuitsAsString = value; }
		}

		public double TotalModules {
			get { return totalModules; }
			set { totalModules = value; }
		}
		
		public double LengthConnection {
			get { return lengthConnection; }
			set { lengthConnection = value; }
		}

		public double Wassermenge {
			get { return wassermenge; }
			set { wassermenge = value; }
		}
		
		public double DruckverlustHeizkreis {
			get { return druckverlustHeizkreis; }
			set { druckverlustHeizkreis = value; }
		}
				
		public double DruckverlustVerteiler {
			get { return druckverlustVerteiler; }
			set { druckverlustVerteiler = value; }
		}

		public double V {
			get { return v; }
			set { v = value; }
		}

		public double UnusedArea {
			get { return unusedArea; }
			set { unusedArea = value; }
		}

		public bool SubSystem {
			get { return subSystem; }
			set { subSystem = value; }
		}

		public bool OtherSystemsConnected {
			get { return otherSystemsConnected; }
			set { otherSystemsConnected = value; }
		}

		public bool UsedAsCircuitWrapper {
			get { return usedAsCircuitWrapper; }
			set { usedAsCircuitWrapper = value; }
		}

	}

}
