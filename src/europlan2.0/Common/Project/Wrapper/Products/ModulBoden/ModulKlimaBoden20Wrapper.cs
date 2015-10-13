using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

    public class ModulKlimaBoden20Wrapper
    {

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
		private double connectionArea;

		private double sonstigeVerbindeleitung;

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

        private string distributorId;

		public ModulKlimaBoden20Wrapper() {
		}

		public ModulKlimaBoden20Wrapper(ModulKlimaBoden20Wrapper mw) {
			floorId = mw.floorId;
			floorName = mw.floorName;
			heatOrCool = mw.heatOrCool;

			roomId = mw.roomId;
			roomName = mw.roomName;
			teilSystem = mw.teilSystem;
			insideConstruction = mw.insideConstruction;
			insideRValue = mw.insideRValue;
			outsideConstruction = mw.outsideConstruction;
			outsideRValue = mw.outsideRValue;
			circuits = mw.circuits;
			connectionArea = mw.connectionArea;
            _totalArea = mw._totalArea;
            _coveredArea = mw._coveredArea;

			sonstigeVerbindeleitung = mw.sonstigeVerbindeleitung;

			roomTemp = mw.roomTemp;
			vorlaufTemp = mw.vorlaufTemp;
			ruecklaufTemp = mw.ruecklaufTemp;
			Q_Soll = mw.Q_Soll;
			Q_FBH = mw.Q_FBH;
			q_FBH = mw.q_FBH;
			t_FB = mw.t_FB;

			circuitsAsString = mw.circuitsAsString;
			totalModules = mw.totalModules;
			lengthConnection = mw.lengthConnection;
			wassermenge = mw.wassermenge;
			druckverlustHeizkreis = mw.druckverlustHeizkreis;
			druckverlustVerteiler = mw.druckverlustVerteiler;
			v = mw.v;

			unusedArea = mw.unusedArea;

			subSystem = mw.subSystem;
			otherSystemsConnected = mw.otherSystemsConnected;
			usedAsCircuitWrapper = mw.usedAsCircuitWrapper;

            distributorId = mw.distributorId;
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

        private double _coveredArea;

        public double CoveredArea
        {
            get { return _coveredArea; }
            set { _coveredArea = value; }
        }

        private double _totalArea;

        public double TotalArea
        {
            get { return _totalArea; }
            set { _totalArea = value; }
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

#warning TODO: calculation correct?
		public double qFBHSqm {
		    get { return Q_FBH / (CoveredArea + UnusedArea); }
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

		public double SonstigeVerbindeleitung {
			get { return sonstigeVerbindeleitung; }
			set { sonstigeVerbindeleitung = value; }
		}

        public string DistributorId {
            get { return distributorId; }
            set { distributorId = value; }
        }

	}

}
