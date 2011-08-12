using System;
using System.Collections.Generic;
using System.Text;
using WW.Math;

namespace Europlan.Common {
	public interface IWallProduct<CircuitType, RegisterType> 
			where CircuitType: Europlan.Common.Circuit
			where RegisterType: Europlan.Common.IWallRegister {

		CircuitType GetCircuitForRegister(RegisterType register);
	}

	public interface IWallRegister {

		int Heizkreis {
			get;
		}

		bool GraphVorlaufRight {
			get;
			set;
		}
	}

	public interface IWallCircuit<CircuitType, VerbindungType, RegisterType>
			where CircuitType : Europlan.Common.Circuit
			where VerbindungType : GraphicalWallVerbindung
			where RegisterType: Europlan.Common.IWallRegister {

		List<RegisterType> Registers {
			get;
			set;
		}

		List<VerbindungType> Links {
			get;
			set;
		}

		VerbindungType GetInputLink(RegisterType register);
		VerbindungType GetOutputLink(RegisterType register);
	}

	public interface IWallRegisterWrapper<RegisterType>
			where RegisterType : Europlan.Common.IWallRegister {

		RegisterType Register {
			get;
			set;
		}

		Room AssociatedRoom {
			get;
		}

		Point2D GetInputConnectionPoint(double offsetX, double offsetY, double dist);
		Point2D GetOutputConnectionPoint(double offsetX, double offsetY, double dist);

		double Width {
			get;
		}

		double Height {
			get;
		}
	}
}
