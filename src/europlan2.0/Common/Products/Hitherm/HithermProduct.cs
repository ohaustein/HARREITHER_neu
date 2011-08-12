using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;
using System.Windows.Forms;
using Europlan.Licensing;
using System.Threading;
using WW.Math;
using WW.Math.Geometry;

namespace Europlan.Common {

	[Serializable()]
	[ProductName("Product_HithermName", "Product_HithermFullName")]
	public class HithermProduct : Product, IWallProduct<HithermCircuit, HithermRegister> {

		// quick dimensioning
		private static int quickDimensioningHeatPowerPerSquareMeter = 100;
		private static int quickDimensioningCoolPowerPerSquareMeter = 100;
		private static bool canHeat = true;
		private static bool canCool = false;

		// planning
		private static double c = 4.19; /* kJ/(kg*K) ... spezifische Wärmekapazität des Mediums */
		private static double verbindeLeitungInnenquerschnitt = 0.000179071;
		private static double verbindeLeitungInnendurchmesser = 0.015099678;
		private static double rho = 1000; /* kg/m³ ... Dichte des Mediums */
		private static double v = 0.00000101; /* m²/s ... kinematische Viskosität */

		private static double spreizungHeizMin = 4;
		private static double spreizungHeizMax = 12;
		private static double spreizungKuehlMin = 2;
		private static double spreizungKühlMax = 5;

		private static double maxRegisterArea = 10.0;

		private static int maxPressureLost = 15000;
		private static int maxDurchfluss = 240;

		// Hitherm(r) Hochleistungs-Klimawandregister (RA 5) Heizleistung qW in W/m²
		private static double[][] hlRegHeizleistung = {
			//  tHm (°C)  30.0  32.5  35.0  37.5  40.0  42.5  45.0  47.5  50.0
			new double[] { 105,  120,  140,  155,  175,  190,  210,  225,  240}, // ti=15°C
			new double[] {  85,  100,  120,  135,  155,  170,  185,  205,  220}, // ti=18°C
			new double[] {  70,   85,  105,  120,  140,  155,  175,  190,  210}, // ti=20°C
			new double[] {  55,   70,   90,  105,  125,  140,  160,  175,  195}, // ti=22°C
			new double[] {  45,   60,   80,   95,  115,  130,  145,  165,  180}  // ti=24°C
		};

		// Hitherm(r) Standard-Klimawandregister (RA 10) Heizleistung qW in W/m²
		private static double[][] stdRegHeizleistung = {
			//  tHm (°C)  30.0  32.5  35.0  37.5  40.0  42.5  45.0  47.5  50.0
			new double[] {  70,   85,   95,  110,  120,  130,  145,  155,  170}, // ti=15°C
			new double[] {  55,   70,   80,   95,  105,  120,  130,  145,  155}, // ti=18°C
			new double[] {  50,   60,   75,   85,  100,  110,  125,  135,  150}, // ti=20°C
			new double[] {  40,   50,   65,   75,   90,  100,  115,  125,  140}, // ti=22°C
			new double[] {  30,   40,   55,   65,   80,   90,  100,  115,  130}  // ti=24°C
		};

		/*private static double[][] hlRegKuehlleistung = {
			//  tHm (°C) 16.0 18.0 20.0 22.0 25.0
			new double[] { 13,   0},               // ti=18°C
			new double[] { 25,  13,   0},          // ti=20°C
			new double[] { 40,  25,  13,   0},     // ti=22°C
			new double[] { 60,  45,  33,  20,   0} // ti=25°C
		};*/

		//     Diffenz Raumtemp - Kuehlmitteltemp (K):  0   2   3   4   5   6   7   9
		private static double[] hlRegKuehlleistung  = { 0, 13, 20, 25, 33, 40, 45, 60 };
		private static double[] stdRegKuehlleistung = { 0,  9, 14, 18, 24, 29, 32, 43 };

		private static double[][] druckverlustHIT_50_5 = {
			new double[] {0.1 ,0.1 ,0.1 ,0.2 ,0.4 ,0.6 ,0.8 ,1.0 ,1.2 ,1.5 ,2.2 ,3.0 ,3.9 ,5.0 ,6.1 ,7.4 ,8.8 ,10.4 ,12.0 ,13.8 ,15.7 ,17.8 ,19.9 ,22.2 ,24.6},
			new double[] {0.1 ,0.3 ,0.4 ,0.6 ,0.8 ,0.9 ,1.1 ,1.3 ,1.6 ,1.8 ,2.3 ,2.8 ,3.4 ,4.0 ,4.7 ,5.4 ,6.2 ,7.0 ,7.9 ,8.8 ,9.7 ,10.7 ,11.8 ,12.8 ,14.0},
			new double[] {0.1 ,0.2 ,0.3 ,0.5 ,0.6 ,0.8 ,0.9 ,1.1 ,1.3 ,1.5 ,1.9 ,2.4 ,2.9 ,3.5 ,4.1 ,4.8 ,5.5 ,6.3 ,7.1 ,8.0 ,8.9 ,9.8 ,10.8 ,11.9 ,12.9},
			new double[] {0.1 ,0.2 ,0.3 ,0.4 ,0.5 ,0.7 ,0.8 ,1.0 ,1.2 ,1.4 ,1.9 ,2.4 ,2.9 ,3.6 ,4.2 ,5.0 ,5.8 ,6.6 ,7.5 ,8.5 ,9.5 ,10.6 ,11.7 ,12.9 ,14.2},
			new double[] {0.1 ,0.1 ,0.2 ,0.4 ,0.5 ,0.6 ,0.8 ,1.0 ,1.2 ,1.4 ,1.9 ,2.5 ,3.1 ,3.8 ,4.6 ,5.4 ,6.3 ,7.3 ,8.3 ,9.5 ,10.7 ,11.9 ,13.2 ,14.6 ,16.1},
			new double[] {0.1 ,0.1 ,0.2 ,0.3 ,0.4 ,0.6 ,0.8 ,1.0 ,1.2 ,1.5 ,2.1 ,2.8 ,3.6 ,4.4 ,5.4 ,6.5 ,7.7 ,8.9 ,10.3 ,11.8 ,13.3 ,15.0 ,16.7 ,18.6 ,20.5}
		};

		private static double[][] druckverlustHIT_50_10 = {
			new double[] {0.1 ,0.2 ,0.3 ,0.5 ,0.6 ,0.8 ,1.0 ,1.2 ,1.5 ,1.8 ,2.4 ,3.1 ,3.8 ,4.7 ,5.6 ,6.6 ,7.7 ,8.9 ,10.1 ,11.5 ,12.9 ,14.4 ,16.0 ,17.7 ,19.4},
			new double[] {0.1 ,0.3 ,0.4 ,0.5 ,0.7 ,0.9 ,1.1 ,1.3 ,1.5 ,1.7 ,2.2 ,2.7 ,3.2 ,3.8 ,4.5 ,5.2 ,5.9 ,6.7 ,7.5 ,8.4 ,9.3 ,10.2 ,11.2 ,12.3 ,13.4},
			new double[] {0.1 ,0.2 ,0.3 ,0.4 ,0.6 ,0.7 ,0.9 ,1.1 ,1.2 ,1.4 ,1.9 ,2.3 ,2.9 ,3.4 ,4.1 ,4.7 ,5.4 ,6.2 ,7.0 ,7.8 ,8.7 ,9.7 ,10.7 ,11.7 ,12.8},
			new double[] {0.1 ,0.2 ,0.3 ,0.4 ,0.5 ,0.7 ,0.8 ,1.0 ,1.2 ,1.4 ,1.9 ,2.4 ,3.0 ,3.6 ,4.3 ,5.0 ,5.8 ,6.7 ,7.6 ,8.6 ,9.6 ,10.7 ,11.9 ,13.1 ,14.4},
			new double[] {0.1 ,0.1 ,0.2 ,0.4 ,0.5 ,0.7 ,0.8 ,1.0 ,1.3 ,1.5 ,2.0 ,2.7 ,3.3 ,4.1 ,5.0 ,5.9 ,6.9 ,8.0 ,9.2 ,10.4 ,11.7 ,13.1 ,14.6 ,16.1 ,17.8},
			new double[] {0.1 ,0.1 ,0.2 ,0.3 ,0.5 ,0.7 ,0.9 ,1.1 ,1.4 ,1.7 ,2.3 ,3.1 ,4.0 ,4.9 ,6.0 ,7.3 ,8.6 ,10.0 ,11.5 ,13.2 ,14.9 ,16.8 ,18.8 ,20.9 ,23.1}
		};

		private static double[][] druckverlustHIT_100_5 = {
			new double[] {0.3 ,0.7 ,1.1 ,1.5 ,2.0 ,2.5 ,3.0 ,3.6 ,4.2 ,4.9 ,6.2 ,7.7 ,9.4 ,11.2 ,13.1 ,15.1 ,17.3 ,19.7 ,22.1 ,24.7 ,27.4 ,30.3 ,33.3 ,36.4 ,39.7},
			new double[] {0.2 ,0.4 ,0.7 ,0.9 ,1.2 ,1.4 ,1.7 ,2.0 ,2.3 ,2.7 ,3.3 ,4.1 ,4.9 ,5.7 ,6.6 ,7.5 ,8.5 ,9.5 ,10.6 ,11.7 ,12.9 ,14.2 ,15.5 ,16.8 ,18.2},
			new double[] {0.2 ,0.4 ,0.6 ,0.9 ,1.1 ,1.4 ,1.7 ,1.9 ,2.2 ,2.6 ,3.2 ,4.0 ,4.7 ,5.6 ,6.5 ,7.4 ,8.4 ,9.4 ,10.5 ,11.7 ,12.9 ,14.1 ,15.4 ,16.8 ,18.2},
			new double[] {0.2 ,0.4 ,0.6 ,0.8 ,1.1 ,1.3 ,1.6 ,1.9 ,2.2 ,2.5 ,3.3 ,4.1 ,4.9 ,5.8 ,6.8 ,7.9 ,9.0 ,10.2 ,11.5 ,12.8 ,14.2 ,15.7 ,17.2 ,18.8 ,20.5},
			new double[] {0.2 ,0.4 ,0.6 ,0.9 ,1.2 ,1.4 ,1.8 ,2.1 ,2.4 ,2.8 ,3.6 ,4.4 ,5.3 ,6.3 ,7.4 ,8.5 ,9.8 ,11.1 ,12.4 ,13.9 ,15.4 ,17.0 ,18.6 ,20.4 ,22.2},
			new double[] {0.2 ,0.5 ,0.7 ,1.0 ,1.3 ,1.6 ,1.9 ,2.3 ,2.7 ,3.1 ,3.9 ,4.8 ,5.9 ,7.0 ,8.1 ,9.4 ,10.7 ,12.1 ,13.6 ,15.2 ,16.8 ,18.6 ,20.4 ,22.3 ,24.2}
		};

		private static double[][] druckverlustHIT_100_10 = {
			new double[] {0.2 ,0.5 ,0.8 ,1.1 ,1.5 ,1.8 ,2.2 ,2.6 ,3.1 ,3.6 ,4.6 ,5.7 ,7.0 ,8.3 ,9.7 ,11.3 ,12.9 ,14.7 ,16.6 ,18.5 ,20.6 ,22.8 ,25.0 ,27.4 ,29.9},
			new double[] {0.2 ,0.4 ,0.6 ,0.8 ,1.0 ,1.3 ,1.6 ,1.8 ,2.1 ,2.4 ,3.1 ,3.8 ,4.6 ,5.4 ,6.3 ,7.2 ,8.2 ,9.3 ,10.4 ,11.5 ,12.7 ,14.0 ,15.3 ,16.7 ,18.2},
			new double[] {0.2 ,0.5 ,0.7 ,1.0 ,1.2 ,1.5 ,1.8 ,2.1 ,2.4 ,2.8 ,3.5 ,4.2 ,5.0 ,5.9 ,6.8 ,7.7 ,8.7 ,9.7 ,10.8 ,12.0 ,13.2 ,14.4 ,15.7 ,17.1 ,18.4},
			new double[] {0.2 ,0.4 ,0.6 ,0.9 ,1.1 ,1.4 ,1.7 ,2.0 ,2.4 ,2.7 ,3.4 ,4.3 ,5.1 ,6.1 ,7.1 ,8.1 ,9.3 ,10.5 ,11.8 ,13.1 ,14.5 ,16.0 ,17.5 ,19.1 ,20.8},
			new double[] {0.2 ,0.5 ,0.7 ,1.0 ,1.3 ,1.6 ,1.9 ,2.3 ,2.6 ,3.0 ,3.8 ,4.7 ,5.7 ,6.7 ,7.8 ,8.9 ,10.2 ,11.5 ,12.8 ,14.3 ,15.8 ,17.4 ,19.0 ,20.8 ,22.5},
			new double[] {0.3 ,0.6 ,0.9 ,1.3 ,1.6 ,2.0 ,2.4 ,2.8 ,3.2 ,3.6 ,4.6 ,5.5 ,6.6 ,7.7 ,8.8 ,10.0 ,11.3 ,12.7 ,14.1 ,15.5 ,17.1 ,18.6 ,20.3 ,22.0 ,23.8}
		};

		private static double[][] druckverlustHIT_150_5 = {
			new double[] {0.4 ,0.8 ,1.2 ,1.7 ,2.3 ,2.8 ,3.5 ,4.1 ,4.8 ,5.6 ,7.2 ,9.0 ,11.0 ,13.1 ,15.4 ,17.9 ,20.5 ,23.3 ,26.3 ,29.4 ,32.8 ,36.2 ,39.9 ,43.7 ,47.7},
			new double[] {0.1 ,0.3 ,0.5 ,0.7 ,0.9 ,1.1 ,1.4 ,1.7 ,2.0 ,2.3 ,3.0 ,3.9 ,4.7 ,5.7 ,6.8 ,7.9 ,9.2 ,10.5 ,11.9 ,13.4 ,15.0 ,16.7 ,18.4 ,20.3 ,22.2},
			new double[] {0.1 ,0.2 ,0.4 ,0.5 ,0.7 ,0.9 ,1.1 ,1.4 ,1.6 ,1.9 ,2.4 ,3.1 ,3.8 ,4.5 ,5.4 ,6.3 ,7.2 ,8.3 ,9.4 ,10.5 ,11.7 ,13.0 ,14.4 ,15.8 ,17.3},
			new double[] {0.1 ,0.2 ,0.3 ,0.4 ,0.6 ,0.7 ,0.9 ,1.1 ,1.3 ,1.5 ,2.0 ,2.5 ,3.1 ,3.8 ,4.5 ,5.3 ,6.2 ,7.1 ,8.0 ,9.0 ,10.1 ,11.3 ,12.5 ,13.8 ,15.1},
			new double[] {0.1 ,0.2 ,0.3 ,0.4 ,0.5 ,0.7 ,0.9 ,1.0 ,1.2 ,1.4 ,1.9 ,2.4 ,3.0 ,3.6 ,4.3 ,5.0 ,5.9 ,6.7 ,7.6 ,8.6 ,9.6 ,10.7 ,11.9 ,13.1 ,14.3},
			new double[] {0.1 ,0.2 ,0.3 ,0.4 ,0.6 ,0.7 ,0.9 ,1.1 ,1.3 ,1.5 ,2.0 ,2.6 ,3.2 ,3.9 ,4.6 ,5.4 ,6.3 ,7.2 ,8.2 ,9.2 ,10.3 ,11.5 ,12.7 ,14.0 ,15.4}
		};

		private static double[][] druckverlustHIT_150_10 = {
			new double[] {0.2 ,0.4 ,0.7 ,1.0 ,1.3 ,1.7 ,2.1 ,2.6 ,3.1 ,3.6 ,4.8 ,6.2 ,7.7 ,9.4 ,11.3 ,13.3 ,15.4 ,17.7 ,20.2 ,22.9 ,25.7 ,28.6 ,31.8 ,35.0 ,38.5},
			new double[] {0.2 ,0.3 ,0.5 ,0.7 ,1.0 ,1.2 ,1.5 ,1.7 ,2.0 ,2.4 ,3.0 ,3.8 ,4.6 ,5.5 ,6.5 ,7.5 ,8.6 ,9.8 ,11.0 ,12.4 ,13.8 ,15.2 ,16.8 ,18.4 ,20.0},
			new double[] {0.1 ,0.2 ,0.3 ,0.5 ,0.6 ,0.8 ,1.0 ,1.2 ,1.4 ,1.6 ,2.2 ,2.7 ,3.4 ,4.1 ,4.8 ,5.7 ,6.6 ,7.5 ,8.5 ,9.6 ,10.7 ,11.9 ,13.2 ,14.5 ,15.9},
			new double[] {0.1 ,0.2 ,0.3 ,0.4 ,0.6 ,0.7 ,0.9 ,1.1 ,1.3 ,1.5 ,1.9 ,2.5 ,3.0 ,3.7 ,4.3 ,5.1 ,5.9 ,6.7 ,7.6 ,8.6 ,9.6 ,10.6 ,11.7 ,12.9 ,14.1},
			new double[] {0.1 ,0.2 ,0.3 ,0.4 ,0.6 ,0.8 ,0.9 ,1.1 ,1.3 ,1.5 ,2.0 ,2.5 ,3.1 ,3.7 ,4.4 ,5.2 ,5.9 ,6.8 ,7.7 ,8.7 ,9.7 ,10.7 ,11.9 ,13.0 ,14.3},
			new double[] {0.1 ,0.2 ,0.4 ,0.5 ,0.7 ,0.8 ,1.0 ,1.2 ,1.5 ,1.7 ,2.2 ,2.8 ,3.4 ,4.1 ,4.8 ,5.7 ,6.5 ,7.4 ,8.4 ,9.5 ,10.6 ,11.7 ,12.9 ,14.2 ,15.6}
		};

		private static double[][] druckverlustHIT_200_5 = {
			new double[] {0.2 ,0.5 ,0.9 ,1.3 ,1.8 ,2.3 ,2.8 ,3.4 ,4.1 ,4.8 ,6.5 ,8.3 ,10.3 ,12.6 ,15.1 ,17.8 ,20.7 ,23.8 ,27.1 ,30.7 ,34.4 ,38.4 ,42.6 ,47.0 ,51.6},
			new double[] {0.2 ,0.4 ,0.7 ,0.9 ,1.2 ,1.5 ,1.9 ,2.2 ,2.6 ,3.0 ,3.8 ,4.8 ,5.8 ,6.9 ,8.1 ,9.3 ,10.7 ,12.1 ,13.7 ,15.3 ,17.0 ,18.7 ,20.6 ,22.6 ,24.6},
			new double[] {0.1 ,0.3 ,0.5 ,0.7 ,1.0 ,1.2 ,1.5 ,1.8 ,2.1 ,2.4 ,3.2 ,4.0 ,4.9 ,5.9 ,7.0 ,8.2 ,9.4 ,10.8 ,12.2 ,13.7 ,15.3 ,17.0 ,18.8 ,20.6 ,22.5},
			new double[] {0.1 ,0.2 ,0.3 ,0.4 ,0.6 ,0.8 ,1.0 ,1.3 ,1.5 ,1.8 ,2.5 ,3.2 ,4.1 ,5.0 ,6.1 ,7.2 ,8.5 ,9.8 ,11.3 ,12.8 ,14.4 ,16.2 ,18.0 ,19.9 ,21.9},
			new double[] {0.1 ,0.1 ,0.1 ,0.2 ,0.3 ,0.5 ,0.7 ,0.9 ,1.1 ,1.4 ,1.9 ,2.6 ,3.4 ,4.3 ,5.4 ,6.5 ,7.7 ,9.0 ,10.5 ,12.0 ,13.7 ,15.4 ,17.3 ,19.2 ,21.3},
			new double[] {0.1 ,0.1 ,0.1 ,0.1 ,0.1 ,0.2 ,0.3 ,0.5 ,0.6 ,0.9 ,1.4 ,2.0 ,2.8 ,3.7 ,4.7 ,5.8 ,7.0 ,8.4 ,9.8 ,11.4 ,13.1 ,15.0 ,16.9 ,19.0 ,21.1}
		};

		private static double[][] druckverlustHIT_200_10 = {
			new double[] {0.1 ,0.3 ,0.5 ,0.7 ,1.0 ,1.3 ,1.7 ,2.2 ,2.6 ,3.2 ,4.4 ,5.7 ,7.3 ,9.0 ,10.9 ,13.1 ,15.3 ,17.8 ,20.5 ,23.3 ,26.4 ,29.6 ,33.0 ,36.6 ,40.3},
			new double[] {0.2 ,0.4 ,0.7 ,1.0 ,1.2 ,1.5 ,1.9 ,2.2 ,2.6 ,3.0 ,3.8 ,4.7 ,5.7 ,6.7 ,7.8 ,9.0 ,10.3 ,11.7 ,13.1 ,14.6 ,16.2 ,17.9 ,19.6 ,21.4 ,23.3},
			new double[] {0.1 ,0.3 ,0.4 ,0.6 ,0.8 ,1.1 ,1.3 ,1.6 ,1.9 ,2.2 ,2.9 ,3.7 ,4.6 ,5.6 ,6.6 ,7.7 ,9.0 ,10.3 ,11.7 ,13.2 ,14.7 ,16.4 ,18.2 ,20.0 ,21.9},
			new double[] {0.1 ,0.1 ,0.2 ,0.4 ,0.5 ,0.7 ,0.9 ,1.1 ,1.4 ,1.6 ,2.2 ,3.0 ,3.8 ,4.7 ,5.7 ,6.8 ,8.0 ,9.3 ,10.7 ,12.2 ,13.8 ,15.4 ,17.2 ,19.1 ,21.1},
			new double[] {0.1 ,0.1 ,0.1 ,0.1 ,0.2 ,0.4 ,0.5 ,0.7 ,0.9 ,1.1 ,1.7 ,2.3 ,3.1 ,3.9 ,4.9 ,6.0 ,7.2 ,8.4 ,9.8 ,11.3 ,12.9 ,14.6 ,16.5 ,18.4 ,20.4},
			new double[] {0.1 ,0.1 ,0.1 ,0.1 ,0.1 ,0.1 ,0.2 ,0.3 ,0.5 ,0.7 ,1.2 ,1.8 ,2.5 ,3.3 ,4.3 ,5.4 ,6.6 ,7.9 ,9.3 ,10.8 ,12.5 ,14.2 ,16.1 ,18.1 ,20.2}
		};

		private static double[][] druckverlustHIT_250_5 = {
			new double[] {0.2, 0.5, 0.9, 1.3, 1.8, 2.3, 2.8, 3.4, 4.1, 4.8, 6.5, 8.3, 10.3, 12.6, 15.1, 17.8, 20.7, 23.8, 27.1, 30.7, 34.4, 38.4, 42.6, 47.0, 51.6},
			new double[] {0.2, 0.4, 0.7, 0.9, 1.2, 1.5, 1.9, 2.2, 2.6, 3.0, 3.8, 4.8, 5.8, 6.9, 8.1, 9.3, 10.7, 12.1, 13.7, 15.3, 17.0, 18.7, 20.6, 22.6, 24.6},
			new double[] {0.1, 0.3, 0.5, 0.7, 1.0, 1.2, 1.5, 1.8, 2.1, 2.4, 3.2, 4.0, 4.9, 5.9, 7.0, 8.2, 9.4, 10.8, 12.2, 13.7, 15.3, 17.0, 18.8, 20.6, 22.5},
			new double[] {0.1, 0.2, 0.3, 0.4, 0.6, 0.8, 1.0, 1.3, 1.5, 1.8, 2.5, 3.2, 4.1, 5.0, 6.1, 7.2, 8.5, 9.8, 11.3, 12.8, 14.4, 16.2, 18.0, 19.9, 21.9},
			new double[] {0.1, 0.1, 0.1, 0.2, 0.3, 0.5, 0.7, 0.9, 1.1, 1.4, 1.9, 2.6, 3.4, 4.3, 5.4, 6.5, 7.7, 9.0, 10.5, 12.0, 13.7, 15.4, 17.3, 19.2, 21.3},
			new double[] {0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.3, 0.5, 0.6, 0.9, 1.4, 2.0, 2.8, 3.7, 4.7, 5.8, 7.0, 8.4, 9.8, 11.4, 13.1, 15.0, 16.9, 19.0, 21.1}
		};

		private static double[][] druckverlustHIT_250_10 = {
			new double[] {0.1, 0.3, 0.5, 0.7, 1.0, 1.3, 1.7, 2.2, 2.6, 3.2, 4.4, 5.7, 7.3, 9.0, 10.9, 13.1, 15.3, 17.8, 20.5, 23.3, 26.4, 29.6, 33.0, 36.6, 40.3},
			new double[] {0.2, 0.4, 0.7, 1.0, 1.2, 1.5, 1.9, 2.2, 2.6, 3.0, 3.8, 4.7, 5.7, 6.7, 7.8, 9.0, 10.3, 11.7, 13.1, 14.6, 16.2, 17.9, 19.6, 21.4, 23.3},
			new double[] {0.1, 0.3, 0.4, 0.6, 0.8, 1.1, 1.3, 1.6, 1.9, 2.2, 2.9, 3.7, 4.6, 5.6, 6.6, 7.7, 9.0, 10.3, 11.7, 13.2, 14.7, 16.4, 18.2, 20.0, 21.9},
			new double[] {0.1, 0.1, 0.2, 0.4, 0.5, 0.7, 0.9, 1.1, 1.4, 1.6, 2.2, 3.0, 3.8, 4.7, 5.7, 6.8, 8.0, 9.3, 10.7, 12.2, 13.8, 15.4, 17.2, 19.1, 21.1},
			new double[] {0.1, 0.1, 0.1, 0.1, 0.2, 0.4, 0.5, 0.7, 0.9, 1.1, 1.7, 2.3, 3.1, 3.9, 4.9, 6.0, 7.2, 8.4, 9.8, 11.3, 12.9, 14.6, 16.5, 18.4, 20.4},
			new double[] {0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.3, 0.5, 0.7, 1.2, 1.8, 2.5, 3.3, 4.3, 5.4, 6.6, 7.9, 9.3, 10.8, 12.5, 14.2, 16.1, 18.1, 20.2}
		};

		private static double[] beplankungRWerte = { 0, 0.01, 0.02, 0.1 };
		private static double[] beplankungFaktoren = { 1, 0.95, 0.91, 0.66 };

		private static double defaultDaemmung = 2.5;

		private static bool usePlus = false;

		private static double leistungsFaktorKuehlen = 1.0;
		private static double leistungsFaktorHeizen = 1.0;
		private static double graphicalRandabstandDefault = 0.1;

		/*private static double factorSpezialputz = 1.15;
		private static double factorMaschinenputz = 1.0;
		private static double factorLehmputz = 0.95;
		private static double factorGkpHohlraum = 0.69;
		private static double factorHolzHohlraum = 0.62;*/

		private Dictionary<HithermRegister, int> registerCircuits = new Dictionary<HithermRegister, int>();
		private Dictionary<int, HithermCircuit> circuitIds = new Dictionary<int, HithermCircuit>();

		private ProductType hithermType = ProductType.WH;
		private float plannedFloorArea = 0;
		private float plannedCeilingArea = 0;
		private float plannedFloorOrCeilingArea = 0;

		public HithermProduct() {
			if (!Licensing.LicenseManager.Instance.License.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdHitherm)) {
				throw new ProductNotLicensedException(this.GetType());
			}
		}

		protected HithermProduct(HithermProduct product) : base(product) {

		}

		public override void Initialize() {
		}

		public override string ImageKey {
            get { return "Hitherm.png"; }
		}

		public override string SelectedImageKey {
            get { return "Hitherm.png"; }
		}

		public override Product.CalculateModeEnum DefaultCalculateMode {
			get { return CalculateModeEnum.HEAT; }
		}

		public new static void StaticInitialize(Configuration config) {
			/*quickDimensioningHeatPowerPerSquareMeter = config.GetProductParameterAsInt<HithermProduct>("ConfigQuickDimensioningHeatPowerPerSquareMeter", 100);
			quickDimensioningCoolPowerPerSquareMeter = config.GetProductParameterAsInt<HithermProduct>("ConfigQuickDimensioningCoolPowerPerSquareMeter", 100);
			canHeat = config.GetProductParameterAsBool<HithermProduct>("ConfigQuickDimensioningCanHeat", true);
			canCool = config.GetProductParameterAsBool<HithermProduct>("ConfigQuickDimensioningCanCool", false);
			usePlus = config.GetProductParameterAsBool<HithermProduct>("ConfigUsePlus", false);
			maxPressureLost = config.GetProductParameterAsInt<HithermProduct>("ConfigMaxPressureLost", 15000);
			maxDurchfluss = config.GetProductParameterAsInt<HithermProduct>("ConfigMaxDurchfluss", 240);
			maxRegisterArea = config.GetProductParameterAsDouble<HithermProduct>("ConfigMaxRegisterArea", 10.0);
			leistungsFaktorHeizen = config.GetProductParameterAsDouble<HithermProduct>("ConfigLeistungsFaktorHeizen", 1.0);
			leistungsFaktorKuehlen = config.GetProductParameterAsDouble<HithermProduct>("ConfigLeistungsFaktorKuehlen", 1.0);*/
			Product.StaticInitialize<HithermProduct>(config);
		}

		public static string GlobalNotificationMessage {
			get {
				string message = null;
				Configuration userConfig = Configuration.UserTemplate;

				double defaultLeistungsFaktorHeizen = userConfig.GetProductParameterAsDouble<HithermProduct>("ConfigLeistungsFaktorHeizen");
				if (leistungsFaktorHeizen != defaultLeistungsFaktorHeizen) {
					if (message == null) {
						message = "";
					} else {
						message += "\n";
					}
					string newMsg = EuroplanRes.NotificationMessage_LeistungsfaktorHeizen;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(leistungsFaktorHeizen, 3).ToString());
					newMsg = newMsg.Replace("%DEFAULT%", Math.Round(defaultLeistungsFaktorHeizen, 3).ToString());
					message += newMsg;
				}

				double defaultLeistungsFaktorKuehlen = userConfig.GetProductParameterAsDouble<HithermProduct>("ConfigLeistungsFaktorKuehlen");
				if (leistungsFaktorKuehlen != defaultLeistungsFaktorKuehlen) {
					if (message == null) {
						message = "";
					} else {
						message += "\n";
					}
					string newMsg = EuroplanRes.NotificationMessage_LeistungsfaktorKuehlen;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(leistungsFaktorKuehlen, 3).ToString());
					newMsg = newMsg.Replace("%DEFAULT%", Math.Round(defaultLeistungsFaktorKuehlen, 3).ToString());
					message += newMsg;
				}

				if (message != null) {
					message = EuroplanRes.HithermProduct_NotificationParameter + /*"Hitherm-Systeme werden mit veränderten Paramtern berechnet. Folgende Parameter weichen von den Standardwerten ab:\n" */
						"\n" + message;
				}

				return message;
			}
		}

		public override Product Clone(Room room) {
			HithermProduct product = new HithermProduct(this);
			product.AssociatedRoom = room;
			return product;
		}

		#region Product Parameters
		[BoolProductParameter(true)]
		public static bool ConfigQuickDimensioningCanHeat {
			get { return canHeat; }
			set { canHeat = value; }
		}
		public override bool QuickDimensioningCanHeat {
			get { return canHeat; }
		}

		[BoolProductParameter(false)]
		public static bool ConfigQuickDimensioningCanCool {
			get { return canCool; }
			set { canCool = value; }
		}
		public override bool QuickDimensioningCanCool {
			get { return canCool; }
		}

		[IntProductParameter(100)]
		public static int ConfigQuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
			set { quickDimensioningHeatPowerPerSquareMeter = value; }
		}
		public override int QuickDimensioningHeatPowerPerSquareMeter {
			get { return quickDimensioningHeatPowerPerSquareMeter; }
		}

		[IntProductParameter(100)]
		public static int ConfigQuickDimensioningCoolPowerPerSquareMeter {
			get { return quickDimensioningCoolPowerPerSquareMeter; }
			set { quickDimensioningCoolPowerPerSquareMeter = value; }
		}
		public override int QuickDimensioningCoolPowerPerSquareMeter {
			get { return quickDimensioningCoolPowerPerSquareMeter; }
		}

		[DoubleProductParameter(4.19)]
		public static double ConfigC {
			get { return c; }
			set { c = value; }
		}

		[DoubleProductParameter(0.015099678)]
		public static double ConfigVerbindeLeitungInnendurchmesser {
			get { return verbindeLeitungInnendurchmesser; }
			set { verbindeLeitungInnendurchmesser = value; }
		}

		[DoubleProductParameter(0.000179071)]
		public static double ConfigVerbindeLeitungInnenquerschnitt {
			get { return verbindeLeitungInnenquerschnitt; }
			set { verbindeLeitungInnenquerschnitt = value; }
		}

		[DoubleProductParameter(1000)]
		public static double ConfigRho {
			get { return rho; }
			set { rho = value; }
		}

		[DoubleProductParameter(0.00000101)]
		public static double ConfigV {
			get { return v; }
			set { v = value; }
		}

		[IntProductParameter(15000)]
		public static int ConfigMaxPressureLost {
			get { return maxPressureLost; }
			set { maxPressureLost = value; }
		}

		[IntProductParameter(240)]
		public static int ConfigMaxDurchfluss {
			get { return maxDurchfluss; }
			set { maxDurchfluss = value; }
		}
		public static double ConfigMaxMassenstrom {
			get { return maxDurchfluss * rho / 1000; }
		}

		[DoubleProductParameter(10)]
		public static double ConfigMaxRegisterArea {
			get { return maxRegisterArea; }
			set { maxRegisterArea = value; }
		}

		[StringProductParameter("{{105, 120, 140, 155, 175, 190, 210, 225, 240} ,{85, 100, 120, 135, 155, 170, 185, 205, 220} ,{70, 85, 105, 120, 140, 155, 175, 190, 210} ,{55, 70, 90, 105, 125, 140, 160, 175, 195} ,{45, 60, 80, 95, 115, 130, 145, 165, 180}}")]
		public static string ConfigHlRegHeizleistungString {
			get {
				return ConvertArrayToString2(hlRegHeizleistung);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					hlRegHeizleistung = array;
				}
			}
		}
		public static double[][] ConfigHlRegHeizleistung {
			get { return hlRegHeizleistung; }
			set { hlRegHeizleistung = value; }
		}

		[StringProductParameter("{{70, 85, 95, 110, 120, 130, 145, 155, 170} ,{55, 70, 80, 95, 105, 120, 130, 145, 155} ,{50, 60, 75, 85, 100, 110, 125, 135, 150} ,{40, 50, 65, 75, 90, 100, 115, 125, 140} ,{30, 40, 55, 65, 80, 90, 100, 115, 130}}")]
		public static string ConfigStdRegHeizleistungString {
			get {
				return ConvertArrayToString2(stdRegHeizleistung);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					stdRegHeizleistung = array;
				}
			}
		}
		public static double[][] ConfigStdRegHeizleistung {
			get { return stdRegHeizleistung; }
			set { stdRegHeizleistung = value; }
		}

		[StringProductParameter("{0, 13, 20, 25, 33, 40, 45, 60}")]
		public static string ConfigHlRegKuehlleistungString {
			get {
				return ConvertArrayToString(hlRegKuehlleistung);
			}
			set {
				double[] array = ConvertStringToArray(value);
				if (array != null) {
					hlRegKuehlleistung = array;
				}
			}
		}
		public static double[] ConfigHlRegKuehlleistung {
			get { return hlRegKuehlleistung; }
			set { hlRegKuehlleistung = value; }
		}

		[StringProductParameter("{0, 9, 14, 18, 24, 29, 32, 43}")]
		public static string ConfigStdRegKuehlleistungString {
			get {
				return ConvertArrayToString(stdRegKuehlleistung);
			}
			set {
				double[] array = ConvertStringToArray(value);
				if (array != null) {
					stdRegKuehlleistung = array;
				}
			}
		}
		public static double[] ConfigStdRegKuehlleistung {
			get { return stdRegKuehlleistung; }
			set { stdRegKuehlleistung = value; }
		}

		[StringProductParameter("{0, 0.01, 0.02, 0.1}")]
		public static string ConfigBeplankungRWerteString {
			get {
				return ConvertArrayToString(beplankungRWerte);
			}
			set {
				double[] array = ConvertStringToArray(value);
				if (array != null) {
					beplankungRWerte = array;
				}
			}
		}
		public static double[] ConfigBeplankungRWerte {
			get { return beplankungRWerte; }
			set { beplankungRWerte = value; }
		}

		[StringProductParameter("{1, 0.95, 0.91, 0.66}")]
		public static string ConfigBeplankungFaktorenString {
			get {
				return ConvertArrayToString(beplankungFaktoren);
			}
			set {
				double[] array = ConvertStringToArray(value);
				if (array != null) {
					beplankungFaktoren = array;
				}
			}
		}
		public static double[] ConfigBeplankungFaktoren {
			get { return beplankungFaktoren; }
			set { beplankungFaktoren = value; }
		}

		[DoubleProductParameter(2.5)]
		public static double ConfigDefaultDaemmung {
			get { return defaultDaemmung; }
			set { defaultDaemmung = value; }
		}

		[BoolProductParameter(false, saveForUser = true)]
		public static bool ConfigUsePlus {
			get { return usePlus; }
			set { usePlus = value; }
		}

		[DoubleProductParameter(4)]
		public static double ConfigSpreizungHeizMin {
			get { return spreizungHeizMin; }
			set { spreizungHeizMin = value; }
		}

		[DoubleProductParameter(12)]
		public static double ConfigSpreizungHeizMax {
			get { return spreizungHeizMax; }
			set { spreizungHeizMax = value; }
		}

		[DoubleProductParameter(2)]
		public static double ConfigSpreizungKuehlMin {
			get { return spreizungKuehlMin; }
			set { spreizungKuehlMin = value; }
		}

		[DoubleProductParameter(5)]
		public static double ConfigSpreizungKühlMax {
			get { return spreizungKühlMax; }
			set { spreizungKühlMax = value; }
		}

		[StringProductParameter("{{0.1, 0.1, 0.1, 0.2, 0.4, 0.6, 0.8, 1, 1.2, 1.5, 2.2, 3, 3.9, 5, 6.1, 7.4, 8.8, 10.4, 12, 13.8, 15.7, 17.8, 19.9, 22.2, 24.6} ,{0.1, 0.3, 0.4, 0.6, 0.8, 0.9, 1.1, 1.3, 1.6, 1.8, 2.3, 2.8, 3.4, 4, 4.7, 5.4, 6.2, 7, 7.9, 8.8, 9.7, 10.7, 11.8, 12.8, 14} ,{0.1, 0.2, 0.3, 0.5, 0.6, 0.8, 0.9, 1.1, 1.3, 1.5, 1.9, 2.4, 2.9, 3.5, 4.1, 4.8, 5.5, 6.3, 7.1, 8, 8.9, 9.8, 10.8, 11.9, 12.9} ,{0.1, 0.2, 0.3, 0.4, 0.5, 0.7, 0.8, 1, 1.2, 1.4, 1.9, 2.4, 2.9, 3.6, 4.2, 5, 5.8, 6.6, 7.5, 8.5, 9.5, 10.6, 11.7, 12.9, 14.2} ,{0.1, 0.1, 0.2, 0.4, 0.5, 0.6, 0.8, 1, 1.2, 1.4, 1.9, 2.5, 3.1, 3.8, 4.6, 5.4, 6.3, 7.3, 8.3, 9.5, 10.7, 11.9, 13.2, 14.6, 16.1} ,{0.1, 0.1, 0.2, 0.3, 0.4, 0.6, 0.8, 1, 1.2, 1.5, 2.1, 2.8, 3.6, 4.4, 5.4, 6.5, 7.7, 8.9, 10.3, 11.8, 13.3, 15, 16.7, 18.6, 20.5}}")]
		public static string ConfigDruckverlustHIT_50_5String {
			get {
				return ConvertArrayToString2(druckverlustHIT_50_5);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					druckverlustHIT_50_5 = array;
				}
			}
		}
		public static double[][] ConfigDruckverlustHIT_50_5 {
			get { return druckverlustHIT_50_5; }
			set { druckverlustHIT_50_5 = value; }
		}

		[StringProductParameter("{{0.1, 0.2, 0.3, 0.5, 0.6, 0.8, 1, 1.2, 1.5, 1.8, 2.4, 3.1, 3.8, 4.7, 5.6, 6.6, 7.7, 8.9, 10.1, 11.5, 12.9, 14.4, 16, 17.7, 19.4} ,{0.1, 0.3, 0.4, 0.5, 0.7, 0.9, 1.1, 1.3, 1.5, 1.7, 2.2, 2.7, 3.2, 3.8, 4.5, 5.2, 5.9, 6.7, 7.5, 8.4, 9.3, 10.2, 11.2, 12.3, 13.4} ,{0.1, 0.2, 0.3, 0.4, 0.6, 0.7, 0.9, 1.1, 1.2, 1.4, 1.9, 2.3, 2.9, 3.4, 4.1, 4.7, 5.4, 6.2, 7, 7.8, 8.7, 9.7, 10.7, 11.7, 12.8} ,{0.1, 0.2, 0.3, 0.4, 0.5, 0.7, 0.8, 1, 1.2, 1.4, 1.9, 2.4, 3, 3.6, 4.3, 5, 5.8, 6.7, 7.6, 8.6, 9.6, 10.7, 11.9, 13.1, 14.4} ,{0.1, 0.1, 0.2, 0.4, 0.5, 0.7, 0.8, 1, 1.3, 1.5, 2, 2.7, 3.3, 4.1, 5, 5.9, 6.9, 8, 9.2, 10.4, 11.7, 13.1, 14.6, 16.1, 17.8} ,{0.1, 0.1, 0.2, 0.3, 0.5, 0.7, 0.9, 1.1, 1.4, 1.7, 2.3, 3.1, 4, 4.9, 6, 7.3, 8.6, 10, 11.5, 13.2, 14.9, 16.8, 18.8, 20.9, 23.1}}")]
		public static string ConfigDruckverlustHIT_50_10String {
			get {
				return ConvertArrayToString2(druckverlustHIT_50_10);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					druckverlustHIT_50_10 = array;
				}
			}
		}
		public static double[][] ConfigDruckverlustHIT_50_10 {
			get { return druckverlustHIT_50_10; }
			set { druckverlustHIT_50_10 = value; }
		}

		[StringProductParameter("{{0.3, 0.7, 1.1, 1.5, 2, 2.5, 3, 3.6, 4.2, 4.9, 6.2, 7.7, 9.4, 11.2, 13.1, 15.1, 17.3, 19.7, 22.1, 24.7, 27.4, 30.3, 33.3, 36.4, 39.7} ,{0.2, 0.4, 0.7, 0.9, 1.2, 1.4, 1.7, 2, 2.3, 2.7, 3.3, 4.1, 4.9, 5.7, 6.6, 7.5, 8.5, 9.5, 10.6, 11.7, 12.9, 14.2, 15.5, 16.8, 18.2} ,{0.2, 0.4, 0.6, 0.9, 1.1, 1.4, 1.7, 1.9, 2.2, 2.6, 3.2, 4, 4.7, 5.6, 6.5, 7.4, 8.4, 9.4, 10.5, 11.7, 12.9, 14.1, 15.4, 16.8, 18.2} ,{0.2, 0.4, 0.6, 0.8, 1.1, 1.3, 1.6, 1.9, 2.2, 2.5, 3.3, 4.1, 4.9, 5.8, 6.8, 7.9, 9, 10.2, 11.5, 12.8, 14.2, 15.7, 17.2, 18.8, 20.5} ,{0.2, 0.4, 0.6, 0.9, 1.2, 1.4, 1.8, 2.1, 2.4, 2.8, 3.6, 4.4, 5.3, 6.3, 7.4, 8.5, 9.8, 11.1, 12.4, 13.9, 15.4, 17, 18.6, 20.4, 22.2} ,{0.2, 0.5, 0.7, 1, 1.3, 1.6, 1.9, 2.3, 2.7, 3.1, 3.9, 4.8, 5.9, 7, 8.1, 9.4, 10.7, 12.1, 13.6, 15.2, 16.8, 18.6, 20.4, 22.3, 24.2}}")]
		public static string ConfigDruckverlustHIT_100_5String {
			get {
				return ConvertArrayToString2(druckverlustHIT_100_5);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					druckverlustHIT_100_5 = array;
				}
			}
		}
		public static double[][] ConfigDruckverlustHIT_100_5 {
			get { return druckverlustHIT_100_5; }
			set { druckverlustHIT_100_5 = value; }
		}

		[StringProductParameter("{{0.2, 0.5, 0.8, 1.1, 1.5, 1.8, 2.2, 2.6, 3.1, 3.6, 4.6, 5.7, 7, 8.3, 9.7, 11.3, 12.9, 14.7, 16.6, 18.5, 20.6, 22.8, 25, 27.4, 29.9} ,{0.2, 0.4, 0.6, 0.8, 1, 1.3, 1.6, 1.8, 2.1, 2.4, 3.1, 3.8, 4.6, 5.4, 6.3, 7.2, 8.2, 9.3, 10.4, 11.5, 12.7, 14, 15.3, 16.7, 18.2} ,{0.2, 0.5, 0.7, 1, 1.2, 1.5, 1.8, 2.1, 2.4, 2.8, 3.5, 4.2, 5, 5.9, 6.8, 7.7, 8.7, 9.7, 10.8, 12, 13.2, 14.4, 15.7, 17.1, 18.4} ,{0.2, 0.4, 0.6, 0.9, 1.1, 1.4, 1.7, 2, 2.4, 2.7, 3.4, 4.3, 5.1, 6.1, 7.1, 8.1, 9.3, 10.5, 11.8, 13.1, 14.5, 16, 17.5, 19.1, 20.8} ,{0.2, 0.5, 0.7, 1, 1.3, 1.6, 1.9, 2.3, 2.6, 3, 3.8, 4.7, 5.7, 6.7, 7.8, 8.9, 10.2, 11.5, 12.8, 14.3, 15.8, 17.4, 19, 20.8, 22.5} ,{0.3, 0.6, 0.9, 1.3, 1.6, 2, 2.4, 2.8, 3.2, 3.6, 4.6, 5.5, 6.6, 7.7, 8.8, 10, 11.3, 12.7, 14.1, 15.5, 17.1, 18.6, 20.3, 22, 23.8}}")]
		public static string ConfigDruckverlustHIT_100_10String {
			get {
				return ConvertArrayToString2(druckverlustHIT_100_10);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					druckverlustHIT_100_10 = array;
				}
			}
		}
		public static double[][] ConfigDruckverlustHIT_100_10 {
			get { return druckverlustHIT_100_10; }
			set { druckverlustHIT_100_10 = value; }
		}

		[StringProductParameter("{{0.4, 0.8, 1.2, 1.7, 2.3, 2.8, 3.5, 4.1, 4.8, 5.6, 7.2, 9, 11, 13.1, 15.4, 17.9, 20.5, 23.3, 26.3, 29.4, 32.8, 36.2, 39.9, 43.7, 47.7} ,{0.1, 0.3, 0.5, 0.7, 0.9, 1.1, 1.4, 1.7, 2, 2.3, 3, 3.9, 4.7, 5.7, 6.8, 7.9, 9.2, 10.5, 11.9, 13.4, 15, 16.7, 18.4, 20.3, 22.2} ,{0.1, 0.2, 0.4, 0.5, 0.7, 0.9, 1.1, 1.4, 1.6, 1.9, 2.4, 3.1, 3.8, 4.5, 5.4, 6.3, 7.2, 8.3, 9.4, 10.5, 11.7, 13, 14.4, 15.8, 17.3} ,{0.1, 0.2, 0.3, 0.4, 0.6, 0.7, 0.9, 1.1, 1.3, 1.5, 2, 2.5, 3.1, 3.8, 4.5, 5.3, 6.2, 7.1, 8, 9, 10.1, 11.3, 12.5, 13.8, 15.1} ,{0.1, 0.2, 0.3, 0.4, 0.5, 0.7, 0.9, 1, 1.2, 1.4, 1.9, 2.4, 3, 3.6, 4.3, 5, 5.9, 6.7, 7.6, 8.6, 9.6, 10.7, 11.9, 13.1, 14.3} ,{0.1, 0.2, 0.3, 0.4, 0.6, 0.7, 0.9, 1.1, 1.3, 1.5, 2, 2.6, 3.2, 3.9, 4.6, 5.4, 6.3, 7.2, 8.2, 9.2, 10.3, 11.5, 12.7, 14, 15.4}}")]
		public static string ConfigDruckverlustHIT_150_5String {
			get {
				return ConvertArrayToString2(druckverlustHIT_150_5);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					druckverlustHIT_150_5 = array;
				}
			}
		}
		public static double[][] ConfigDruckverlustHIT_150_5 {
			get { return druckverlustHIT_150_5; }
			set { druckverlustHIT_150_5 = value; }
		}

		[StringProductParameter("{{0.2, 0.4, 0.7, 1, 1.3, 1.7, 2.1, 2.6, 3.1, 3.6, 4.8, 6.2, 7.7, 9.4, 11.3, 13.3, 15.4, 17.7, 20.2, 22.9, 25.7, 28.6, 31.8, 35, 38.5} ,{0.2, 0.3, 0.5, 0.7, 1, 1.2, 1.5, 1.7, 2, 2.4, 3, 3.8, 4.6, 5.5, 6.5, 7.5, 8.6, 9.8, 11, 12.4, 13.8, 15.2, 16.8, 18.4, 20} ,{0.1, 0.2, 0.3, 0.5, 0.6, 0.8, 1, 1.2, 1.4, 1.6, 2.2, 2.7, 3.4, 4.1, 4.8, 5.7, 6.6, 7.5, 8.5, 9.6, 10.7, 11.9, 13.2, 14.5, 15.9} ,{0.1, 0.2, 0.3, 0.4, 0.6, 0.7, 0.9, 1.1, 1.3, 1.5, 1.9, 2.5, 3, 3.7, 4.3, 5.1, 5.9, 6.7, 7.6, 8.6, 9.6, 10.6, 11.7, 12.9, 14.1} ,{0.1, 0.2, 0.3, 0.4, 0.6, 0.8, 0.9, 1.1, 1.3, 1.5, 2, 2.5, 3.1, 3.7, 4.4, 5.2, 5.9, 6.8, 7.7, 8.7, 9.7, 10.7, 11.9, 13, 14.3} ,{0.1, 0.2, 0.4, 0.5, 0.7, 0.8, 1, 1.2, 1.5, 1.7, 2.2, 2.8, 3.4, 4.1, 4.8, 5.7, 6.5, 7.4, 8.4, 9.5, 10.6, 11.7, 12.9, 14.2, 15.6}}")]
		public static string ConfigDruckverlustHIT_150_10String {
			get {
				return ConvertArrayToString2(druckverlustHIT_150_10);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					druckverlustHIT_150_10 = array;
				}
			}
		}
		public static double[][] ConfigDruckverlustHIT_150_10 {
			get { return druckverlustHIT_150_10; }
			set { druckverlustHIT_150_10 = value; }
		}

		[StringProductParameter("{{0.2, 0.5, 0.9, 1.3, 1.8, 2.3, 2.8, 3.4, 4.1, 4.8, 6.5, 8.3, 10.3, 12.6, 15.1, 17.8, 20.7, 23.8, 27.1, 30.7, 34.4, 38.4, 42.6, 47, 51.6} ,{0.2, 0.4, 0.7, 0.9, 1.2, 1.5, 1.9, 2.2, 2.6, 3, 3.8, 4.8, 5.8, 6.9, 8.1, 9.3, 10.7, 12.1, 13.7, 15.3, 17, 18.7, 20.6, 22.6, 24.6} ,{0.1, 0.3, 0.5, 0.7, 1, 1.2, 1.5, 1.8, 2.1, 2.4, 3.2, 4, 4.9, 5.9, 7, 8.2, 9.4, 10.8, 12.2, 13.7, 15.3, 17, 18.8, 20.6, 22.5} ,{0.1, 0.2, 0.3, 0.4, 0.6, 0.8, 1, 1.3, 1.5, 1.8, 2.5, 3.2, 4.1, 5, 6.1, 7.2, 8.5, 9.8, 11.3, 12.8, 14.4, 16.2, 18, 19.9, 21.9} ,{0.1, 0.1, 0.1, 0.2, 0.3, 0.5, 0.7, 0.9, 1.1, 1.4, 1.9, 2.6, 3.4, 4.3, 5.4, 6.5, 7.7, 9, 10.5, 12, 13.7, 15.4, 17.3, 19.2, 21.3} ,{0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.3, 0.5, 0.6, 0.9, 1.4, 2, 2.8, 3.7, 4.7, 5.8, 7, 8.4, 9.8, 11.4, 13.1, 15, 16.9, 19, 21.1}}")]
		public static string ConfigDruckverlustHIT_200_5String {
			get {
				return ConvertArrayToString2(druckverlustHIT_200_5);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					druckverlustHIT_200_5 = array;
				}
			}
		}
		public static double[][] ConfigDruckverlustHIT_200_5 {
			get { return druckverlustHIT_200_5; }
			set { druckverlustHIT_200_5 = value; }
		}

		[StringProductParameter("{{0.1, 0.3, 0.5, 0.7, 1, 1.3, 1.7, 2.2, 2.6, 3.2, 4.4, 5.7, 7.3, 9, 10.9, 13.1, 15.3, 17.8, 20.5, 23.3, 26.4, 29.6, 33, 36.6, 40.3} ,{0.2, 0.4, 0.7, 1, 1.2, 1.5, 1.9, 2.2, 2.6, 3, 3.8, 4.7, 5.7, 6.7, 7.8, 9, 10.3, 11.7, 13.1, 14.6, 16.2, 17.9, 19.6, 21.4, 23.3} ,{0.1, 0.3, 0.4, 0.6, 0.8, 1.1, 1.3, 1.6, 1.9, 2.2, 2.9, 3.7, 4.6, 5.6, 6.6, 7.7, 9, 10.3, 11.7, 13.2, 14.7, 16.4, 18.2, 20, 21.9} ,{0.1, 0.1, 0.2, 0.4, 0.5, 0.7, 0.9, 1.1, 1.4, 1.6, 2.2, 3, 3.8, 4.7, 5.7, 6.8, 8, 9.3, 10.7, 12.2, 13.8, 15.4, 17.2, 19.1, 21.1} ,{0.1, 0.1, 0.1, 0.1, 0.2, 0.4, 0.5, 0.7, 0.9, 1.1, 1.7, 2.3, 3.1, 3.9, 4.9, 6, 7.2, 8.4, 9.8, 11.3, 12.9, 14.6, 16.5, 18.4, 20.4} ,{0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.3, 0.5, 0.7, 1.2, 1.8, 2.5, 3.3, 4.3, 5.4, 6.6, 7.9, 9.3, 10.8, 12.5, 14.2, 16.1, 18.1, 20.2}}")]
		public static string ConfigDruckverlustHIT_200_10String {
			get {
				return ConvertArrayToString2(druckverlustHIT_200_10);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					druckverlustHIT_200_10 = array;
				}
			}
		}
		public static double[][] ConfigDruckverlustHIT_200_10 {
			get { return druckverlustHIT_200_10; }
			set { druckverlustHIT_200_10 = value; }
		}

		[StringProductParameter("{{0.2, 0.5, 0.9, 1.3, 1.8, 2.3, 2.8, 3.4, 4.1, 4.8, 6.5, 8.3, 10.3, 12.6, 15.1, 17.8, 20.7, 23.8, 27.1, 30.7, 34.4, 38.4, 42.6, 47, 51.6} ,{0.2, 0.4, 0.7, 0.9, 1.2, 1.5, 1.9, 2.2, 2.6, 3, 3.8, 4.8, 5.8, 6.9, 8.1, 9.3, 10.7, 12.1, 13.7, 15.3, 17, 18.7, 20.6, 22.6, 24.6} ,{0.1, 0.3, 0.5, 0.7, 1, 1.2, 1.5, 1.8, 2.1, 2.4, 3.2, 4, 4.9, 5.9, 7, 8.2, 9.4, 10.8, 12.2, 13.7, 15.3, 17, 18.8, 20.6, 22.5} ,{0.1, 0.2, 0.3, 0.4, 0.6, 0.8, 1, 1.3, 1.5, 1.8, 2.5, 3.2, 4.1, 5, 6.1, 7.2, 8.5, 9.8, 11.3, 12.8, 14.4, 16.2, 18, 19.9, 21.9} ,{0.1, 0.1, 0.1, 0.2, 0.3, 0.5, 0.7, 0.9, 1.1, 1.4, 1.9, 2.6, 3.4, 4.3, 5.4, 6.5, 7.7, 9, 10.5, 12, 13.7, 15.4, 17.3, 19.2, 21.3} ,{0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.3, 0.5, 0.6, 0.9, 1.4, 2, 2.8, 3.7, 4.7, 5.8, 7, 8.4, 9.8, 11.4, 13.1, 15, 16.9, 19, 21.1}}")]
		public static string ConfigDruckverlustHIT_250_5String {
			get {
				return ConvertArrayToString2(druckverlustHIT_250_5);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					druckverlustHIT_250_5 = array;
				}
			}
		}
		public static double[][] ConfigDruckverlustHIT_250_5 {
			get { return druckverlustHIT_250_5; }
			set { druckverlustHIT_250_5 = value; }
		}

		[StringProductParameter("{{0.1, 0.3, 0.5, 0.7, 1, 1.3, 1.7, 2.2, 2.6, 3.2, 4.4, 5.7, 7.3, 9, 10.9, 13.1, 15.3, 17.8, 20.5, 23.3, 26.4, 29.6, 33, 36.6, 40.3} ,{0.2, 0.4, 0.7, 1, 1.2, 1.5, 1.9, 2.2, 2.6, 3, 3.8, 4.7, 5.7, 6.7, 7.8, 9, 10.3, 11.7, 13.1, 14.6, 16.2, 17.9, 19.6, 21.4, 23.3} ,{0.1, 0.3, 0.4, 0.6, 0.8, 1.1, 1.3, 1.6, 1.9, 2.2, 2.9, 3.7, 4.6, 5.6, 6.6, 7.7, 9, 10.3, 11.7, 13.2, 14.7, 16.4, 18.2, 20, 21.9} ,{0.1, 0.1, 0.2, 0.4, 0.5, 0.7, 0.9, 1.1, 1.4, 1.6, 2.2, 3, 3.8, 4.7, 5.7, 6.8, 8, 9.3, 10.7, 12.2, 13.8, 15.4, 17.2, 19.1, 21.1} ,{0.1, 0.1, 0.1, 0.1, 0.2, 0.4, 0.5, 0.7, 0.9, 1.1, 1.7, 2.3, 3.1, 3.9, 4.9, 6, 7.2, 8.4, 9.8, 11.3, 12.9, 14.6, 16.5, 18.4, 20.4} ,{0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.3, 0.5, 0.7, 1.2, 1.8, 2.5, 3.3, 4.3, 5.4, 6.6, 7.9, 9.3, 10.8, 12.5, 14.2, 16.1, 18.1, 20.2}}")]
		public static string ConfigDruckverlustHIT_250_10String {
			get {
				return ConvertArrayToString2(druckverlustHIT_250_10);
			}
			set {
				double[][] array = ConvertStringToArray2(value);
				if (array != null) {
					druckverlustHIT_250_10 = array;
				}
			}
		}
		public static double[][] ConfigDruckverlustHIT_250_10 {
			get { return druckverlustHIT_250_10; }
			set { druckverlustHIT_250_10 = value; }
		}

		[DoubleProductParameter(1)]
		public static double ConfigLeistungsFaktorKuehlen {
			get { return leistungsFaktorKuehlen; }
			set { leistungsFaktorKuehlen = value; }
		}

		[DoubleProductParameter(1)]
		public static double ConfigLeistungsFaktorHeizen {
			get { return leistungsFaktorHeizen; }
			set { leistungsFaktorHeizen = value; }
		}

		[DoubleProductParameter(0.1)]
		public static double ConfigGraphicalRandabstandDefault {
			get { return graphicalRandabstandDefault; }
			set { graphicalRandabstandDefault = value; }
		}
		#endregion Product Parameters

		public override int GetDefaultQuickDimensioningCircuits() {
			return (int)Math.Ceiling(quickDimensioningPlannedArea / 10);
		}

		public override float GetDefaultQuickDimensioningPlannedArea() {
			return 0;
		}

		public override float QuickDimensioningMaximumArea {
			get { return Int32.MaxValue; }
		}

		public override string QuickDimensioningName {
			get { return QuickDimensioningNameStatic; }
		}

		public static string QuickDimensioningNameStatic {
			get { return "Hitherm®\n(m²)"; }
		}

		public override ProductType Type {
			get { return this.hithermType; }
		}

		public ProductType HithermType {
			get { return this.hithermType; }
			set { this.hithermType = value; }
		}

		public override void CalculateHeatAndCoolFlow() {
			base.CalculateHeatAndCoolFlow();
			double spreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
			double spreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
			if (spreizungHeat > HithermProduct.ConfigSpreizungHeizMax) {
				spreizungHeat = HithermProduct.ConfigSpreizungHeizMax;
			}
			if (spreizungHeat < HithermProduct.ConfigSpreizungHeizMin) {
				spreizungHeat = HithermProduct.ConfigSpreizungHeizMin;
			}
			if (spreizungCool > HithermProduct.ConfigSpreizungKühlMax) {
				spreizungCool = HithermProduct.ConfigSpreizungKühlMax;
			}
			if (spreizungCool < HithermProduct.ConfigSpreizungKuehlMin) {
				spreizungCool = HithermProduct.ConfigSpreizungKuehlMin;
			}
			this.plannedRuecklaufTempHeat = this.plannedVorlaufTempHeat - spreizungHeat;
			this.plannedRuecklaufTempCool = this.plannedVorlaufTempCool + spreizungCool;
			if (this.plannedRuecklaufTempHeat - this.associatedRoom.RoomHeatTemperature < 3) {
				this.plannedRuecklaufTempHeat = this.associatedRoom.RoomHeatTemperature + 3;
			}
			if (this.associatedRoom.RoomCoolTemperature - this.plannedRuecklaufTempCool < 3) {
				this.plannedRuecklaufTempCool = this.associatedRoom.RoomCoolTemperature - 3;
			}
		}

		public override bool ConfigureProduct(double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, bool variableSpreizung) {
			this.requestedHeatLoad = requestedHeatLoad;
			this.requestedCoolLoad = requestedCoolLoad;
			this.incompleteCalculation = false;
			if (this.PlannedConnection == null) {
				this.lastErrorMsg = EuroplanRes.ErrorMessage_FehlendeEingaben + " "; //"Fehlende Eingaben: ";
				if (PlannedConnection == null) {
					this.lastErrorMsg += EuroplanRes.ErrorMessage_FehlendeEingabenHkAnschluss + ", "; //"Heizkreisanschluß, ";
				}
				this.lastErrorMsg = this.lastErrorMsg.Substring(0, this.lastErrorMsg.Length - 2);
				this.incompleteCalculation = true;
				return false;
			}
			if (this.PlannedCircuits.Count > 12) {
				this.lastErrorMsg = "Es sind zuviele Heizkreise in diesem Produkt vorhanden"; // TODO
				this.incompleteCalculation = true;
				return false;
			}

			if (this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
				// TODO connect all circuits

				int c = this.PlannedConnection.OtherProduct.Product.PlannedCircuits.Count - this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.Count;
				foreach (Circuit.CircuitConnection cc in this.PlannedConnection.OtherProduct.Product.ConnectedCircuits.Values) {
					if (cc.OtherProduct == this) {
						c++;
					}
				}
				if (c < this.circuits.Count) {
					/*if (this.requestedCircuits.HasValue) {
						// TODO reset circuits
					} else {*/
					//this.circuits.Clear();
					/*}*/
					this.lastErrorMsg = EuroplanRes.ErrorMessage_HkAnschluss; //"Es sind nicht alle Heizkreise dieses Systems angeschloßen";
					this.incompleteCalculation = true;
					return false;
				}
				bool userDefinedOk = true;
				foreach (Circuit.CircuitConnection cc in this.inverseConnectedCircuits.Values) {
					if (cc.OtherCircuit == null) {
						userDefinedOk = false;
					}
				}
				if (!userDefinedOk) {
					this.lastErrorMsg = EuroplanRes.ErrorMessage_HkAnschluss; //"Es sind nicht alle Heizkreise dieses Systems angeschloßen";
					this.incompleteCalculation = true;
					return false;
				}
			}

			double[] vorlaufTotal;
			double[] vorlaufNotIsolated;
			double[] ruecklaufTotal;
			double[] ruecklaufNotIsolated;
			double[] vorlaufWithoutOtherProductTotal;
			double[] vorlaufWithoutOtherProductNotIsolated;
			double[] ruecklaufWithoutOtherProductTotal;
			double[] ruecklaufWithoutOtherProductNotIsolated;
			double longestVorlaufTotal;
			double longestRuecklaufTotal;
			this.CalculateVorlaufRuecklauf(out vorlaufTotal, out vorlaufNotIsolated, out ruecklaufTotal, out ruecklaufNotIsolated, out vorlaufWithoutOtherProductTotal, out vorlaufWithoutOtherProductNotIsolated, out ruecklaufWithoutOtherProductTotal, out ruecklaufWithoutOtherProductNotIsolated, out longestVorlaufTotal, out longestRuecklaufTotal);

			this.CalculateHeatAndCoolFlow();
			int i = 0;
			foreach (HithermCircuit hc in this.circuits) {
				hc.HithermProduct = this;
				hc.NrOfCircuit = i;
				hc.PipeLengthVorlaufTotal = vorlaufTotal[i];
				hc.PipeLengthVorlaufNotIsolated = vorlaufNotIsolated[i];
				hc.PipeLengthRuecklaufTotal = ruecklaufTotal[i];
				hc.PipeLengthRuecklaufNotIsolated = ruecklaufNotIsolated[i];
				hc.PipeLengthVorlaufWithoutOtherProductTotal = vorlaufWithoutOtherProductTotal[i];
				hc.PipeLengthVorlaufWithoutOtherProductNotIsolated = vorlaufWithoutOtherProductNotIsolated[i];
				hc.PipeLengthRuecklaufWithoutOtherProductTotal = ruecklaufWithoutOtherProductTotal[i];
				hc.PipeLengthRuecklaufWithoutOtherProductNotIsolated = ruecklaufWithoutOtherProductNotIsolated[i];
				hc.Calculate();
				i++;
			}

			if (variableSpreizung && this.PlannedConnection != null && this.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.DISTRIBUTOR) {
				double defSpreizungHeat = this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat;
				double defSpreizungCool = this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool;
				// Heizleistung veringern
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat < HithermProduct.ConfigSpreizungHeizMax && this.PlannedHeatLoad > requestedHeatLoad && this.PlannedSpreizungHeat < 1.2 * defSpreizungHeat) {
					this.plannedRuecklaufTempHeat -= 0.1;
					foreach (HithermCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempHeat += 0.1;
				// Heizleistung erhöhen
				while (this.plannedVorlaufTempHeat - this.plannedRuecklaufTempHeat > HithermProduct.ConfigSpreizungHeizMin && this.PlannedHeatLoad < requestedHeatLoad && this.PlannedDeltaRhoHeat < HithermProduct.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhHeat < HithermProduct.ConfigMaxMassenstrom && this.PlannedSpreizungHeat > 0.8 * defSpreizungHeat) {
					this.plannedRuecklaufTempHeat += 0.1;
					foreach (HithermCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				// Kühlleistung verringern
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool < HithermProduct.ConfigSpreizungKühlMax && this.PlannedCoolLoad > requestedCoolLoad && this.PlannedSpreizungCool < 1.2 * defSpreizungCool) {
					this.plannedRuecklaufTempCool += 0.1;
					foreach (HithermCircuit c in this.circuits) {
						c.Calculate();
					}
				}
				this.plannedRuecklaufTempCool -= 0.1;
				// Kühlleistung erhöhen
				while (this.plannedRuecklaufTempCool - this.plannedVorlaufTempCool > HithermProduct.ConfigSpreizungKuehlMin && this.PlannedCoolLoad < requestedCoolLoad && this.PlannedDeltaRhoCool < HithermProduct.ConfigMaxPressureLost / 100.0 && this.PlannedMaxMhCool < HithermProduct.ConfigMaxMassenstrom && this.PlannedSpreizungCool > 0.8 * defSpreizungCool) {
					this.plannedRuecklaufTempCool -= 0.1;
					foreach (HithermCircuit c in this.circuits) {
						c.Calculate();
					}
				}

				// Calculate variable spreizung for connected products
				foreach (KeyValuePair<int, Circuit.CircuitConnection> kvp in this.connectedCircuits) {
					if (kvp.Value != null) {
						kvp.Value.OtherProduct.CalculateHeatAndCoolFlow();
						PlannedProduct pp = Project.Instance.GetPlannedProduct(kvp.Value.OtherProduct);
						if (pp != null) {
							pp.Product.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, true);
						}
					}
				}
			}

			this.lastErrorMsg = "";
			string newMsg;
			foreach (HithermCircuit hc in this.circuits) {
				if (Math.Round(hc.CoveredArea, 1) > Math.Round(ConfigMaxRegisterArea, 1)) {
					newMsg = EuroplanRes.ErrorMessage_Registerflaeche;
					newMsg = newMsg.Replace("%HK%", (hc.NrOfCircuit + 1).ToString());
					newMsg = newMsg.Replace("%VALUE%", Math.Round(hc.CoveredArea, 1).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(ConfigMaxRegisterArea, 1).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			}
			if (this.PlannedMaxMhHeat >= this.PlannedMaxMhCool && this.requestedHeatLoad > 0) {
				if (Math.Round(this.PlannedMaxMhHeat, 1) > HithermProduct.ConfigMaxMassenstrom) {
					newMsg = EuroplanRes.ErrorMessage_DurchflussHeiz;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedMaxMhHeat, 1).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", HithermProduct.ConfigMaxMassenstrom.ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			} else if (this.requestedCoolLoad > 0) {
				if (Math.Round(this.PlannedMaxMhCool, 1) > HithermProduct.ConfigMaxMassenstrom) {
					newMsg = EuroplanRes.ErrorMessage_DurchflussKuehl;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedMaxMhCool, 1).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", HithermProduct.ConfigMaxMassenstrom.ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			}
			if (this.PlannedDeltaRhoHeat >= this.PlannedDeltaRhoCool && this.requestedHeatLoad > 0) {
				if (Math.Round(this.PlannedDeltaRhoHeat, 2) > Math.Round(HithermProduct.ConfigMaxPressureLost / 100.0, 2)) {
					newMsg = EuroplanRes.ErrorMessage_DruckverlustHeiz;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedDeltaRhoHeat, 2).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(HithermProduct.ConfigMaxPressureLost / 100.0, 2).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			} else if (this.requestedCoolLoad > 0) {
				if (Math.Round(this.PlannedDeltaRhoCool, 2) > Math.Round(HithermProduct.ConfigMaxPressureLost / 100.0, 2)) {
					newMsg = EuroplanRes.ErrorMessage_DruckverlustKuehl;
					newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedDeltaRhoCool, 2).ToString());
					newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(HithermProduct.ConfigMaxPressureLost / 100.0, 2).ToString());
					this.lastErrorMsg += newMsg + "\n";
				}
			}
			if (this.hithermType == Product.ProductType.FBH && this.PlannedRegisterArea > this.PlannedFloorArea) {
				newMsg = EuroplanRes.ErrorMessage_Registerflaeche2;
				newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedRegisterArea, 1).ToString());
				newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(this.PlannedFloorArea, 1).ToString());
				this.lastErrorMsg += newMsg + "\n";
			} else if (this.hithermType == Product.ProductType.DH && this.PlannedRegisterArea > this.PlannedCeilingArea) {
				newMsg = EuroplanRes.ErrorMessage_Registerflaeche2;
				newMsg = newMsg.Replace("%VALUE%", Math.Round(this.PlannedRegisterArea, 1).ToString());
				newMsg = newMsg.Replace("%MAXIMUM%", Math.Round(this.PlannedCeilingArea, 1).ToString());
				this.lastErrorMsg += newMsg + "\n";
			}
			if (this.lastErrorMsg.Length == 0) {
				this.lastErrorMsg = null;
			}

			return true;
		}

		public override float PlannedFloorArea {
			get {
				if (this.hithermType == ProductType.FBH) {
					return this.plannedFloorArea;
				}
				return 0;
			}
			set {
				if (this.hithermType == ProductType.FBH) {
					this.plannedFloorArea = value;
				}
			}
		}

		public override float PlannedWallArea {
			get {
				if (this.hithermType == ProductType.WH) {
					return this.PlannedNetArea;
				}
				return 0;
			}
			set { }
		}

		public override float PlannedCeilingArea {
			get {
				if (this.hithermType == ProductType.DH) {
					return this.plannedCeilingArea;
				}
				return 0;
			}
			set {
				if (this.hithermType == ProductType.DH) {
					this.plannedCeilingArea = value;
				}
			}
		}

		public override float PlannedRoofArea {
			get { return 0; }
			set { }
		}

		// Not to be used in code! This property is only intended to be used for (de)serializing
		public float PlannedFloorOrCeilingArea {
			get {
				if (this.hithermType == ProductType.DH) {
					return this.plannedCeilingArea;
				}
				if (this.hithermType == ProductType.FBH) {
					return this.plannedFloorArea;
				}
				return 0;
			}
			set {
				this.plannedFloorOrCeilingArea = value;
			}
		}

		public override double PlannedCoolLoad {
			get {
				if (this.incompleteCalculation || this.requestedCoolLoad == 0) {
					return 0;
				}
				double value = 0;
				foreach (HithermCircuit c in this.circuits) {
					if (!c.QFbhTotalCool.Equals(double.NaN)) {
						value += c.QFbhTotalCool;
					}
				}
				return value;
			}
		}

		public override double PlannedHeatLoad {
			get {
				if (this.incompleteCalculation || this.requestedHeatLoad == 0) {
					return 0;
				}
				double value = 0;
				foreach (HithermCircuit c in this.circuits) {
					if (!c.QFbhTotalHeat.Equals(double.NaN)) {
						value += c.QFbhTotalHeat;
					}
				}
				return value;
			}
		}

		public override float PlannedNetArea {
			get {
				double area = 0;
				foreach (HithermCircuit hc in this.circuits) {
					area += hc.CoveredArea;
				}
				return (float)area;
			}
		}

		/*public override int GetIndexOfCircuit(Circuit c) {
			return -1;
		}*/

		public override ConnectionPipe.PipeTypeEnum DefaultPipeType {
			get { return ConnectionPipe.PipeTypeEnum.PT_21MM; }
		}

		[XmlIgnore]
		public override float PlannedInsideConstructionRValue {
			get { return 0; /*TODO*/ }
		}

		[XmlIgnore]
		public override bool HasInsideConstruction {
			get { return false; }
		}

		[XmlIgnore]
		public override Construction PlannedInsideConstruction {
			get { return null; }
		}

		[XmlIgnore]
		public override float PlannedOutsideConstructionRValue {
			get { return 0; /*TODO*/ }
		}

		[XmlIgnore]
		public override bool HasOutsideConstruction {
			get { return false; }
		}

		[XmlIgnore]
		public override Construction PlannedOutsideConstruction {
			get { return null; }
		}

		public HithermCircuit GetCircuitForRegister(HithermRegister register) {
			if (!this.registerCircuits.ContainsKey(register)) {
				return null;
			}
			if (!this.circuitIds.ContainsKey(this.registerCircuits[register])) {
				return null;
			}
			return this.circuitIds[this.registerCircuits[register]];
		}

		internal void AddRegisterToCircuit(HithermRegister register, int circuitId) {
			this.registerCircuits[register] = circuitId;
			if (!this.circuitIds.ContainsKey(circuitId)) {
				HithermCircuit hc = new HithermCircuit();
				hc.HithermProduct = this;
				this.circuits.Add(hc);
				this.circuitIds[circuitId] = hc;
			}
			this.circuitIds[circuitId].Registers.Add(register);
		}

		internal void MoveRegisterToCircuit(HithermRegister register, int circuitId) {
			HithermCircuit oldCircuit = this.GetCircuitForRegister(register);
			if (oldCircuit != null) {
				List<HithermRegister> registersToMove = oldCircuit.GetAllConnectedRegisters(register);
				HithermCircuit circuit = null;
				foreach (HithermRegister registerToMove in registersToMove) {
					circuit = this.MoveSingleRegisterToCircuit(registerToMove, circuitId);
				}
				if (circuit != null) {
					List<GraphicalHithermVerbindung> linksToMove = new List<GraphicalHithermVerbindung>();
					foreach (GraphicalHithermVerbindung link in oldCircuit.Links) {
						if ((link.Start != null && circuit.Registers.Contains(link.Start)) ||
							(link.End != null && circuit.Registers.Contains(link.End))) {
							linksToMove.Add(link);
						}
					}
					foreach (GraphicalHithermVerbindung link in linksToMove) {
						oldCircuit.Links.Remove(link);
						link.Circuit = circuit;
						circuit.Links.Add(link);
					}
				}
			}
		}

		private HithermCircuit MoveSingleRegisterToCircuit(HithermRegister register, int circuitId) {
			if (this.registerCircuits.ContainsKey(register)) {
				HithermCircuit hc = this.circuitIds[this.registerCircuits[register]];
				hc.Registers.Remove(register);
				if (hc.Registers.Count == 0) {
					this.circuits.Remove(hc);
					this.circuitIds.Remove(this.registerCircuits[register]);
				}
				this.registerCircuits[register] = circuitId;
				if (!this.circuitIds.ContainsKey(circuitId)) {
					hc = new HithermCircuit();
					hc.HithermProduct = this;
					this.circuits.Add(hc);
					this.circuitIds[circuitId] = hc;
				}
				this.circuitIds[circuitId].Registers.Add(register);
				return this.circuitIds[circuitId];
			}
			return null;
		}

		internal void RemoveRegisterFromCircuit(HithermRegister register) {
			if (this.registerCircuits.ContainsKey(register)) {
				HithermCircuit hc = this.circuitIds[this.registerCircuits[register]];
				hc.Registers.Remove(register);
				if (hc.Registers.Count == 0) {
					this.circuits.Remove(hc);
					this.circuitIds.Remove(this.registerCircuits[register]);
				} else {
					// delete connections of the deleted register
					List<GraphicalHithermVerbindung> linksToDelete = new List<GraphicalHithermVerbindung>();
					foreach (GraphicalHithermVerbindung link in hc.Links) {
						if (link.Start == register || link.End == register) {
							linksToDelete.Add(link);
						}
					}
					foreach (GraphicalHithermVerbindung link in linksToDelete) {
						hc.Links.Remove(link);
					}
				}
				this.registerCircuits.Remove(register);
			}
		}

		internal int GetRegisterCircuitId(HithermRegister register) {
			if (this.registerCircuits.ContainsKey(register)) {
				return this.registerCircuits[register];
			}
			return 0;
		}

		internal override void FinalizeLoading(PlannedProduct pp) {
			base.FinalizeLoading(pp);
			switch (this.hithermType) {
				case ProductType.FBH:
					this.plannedFloorArea = this.plannedFloorOrCeilingArea;
					this.plannedFloorOrCeilingArea = 0;
					this.plannedCeilingArea = 0;
					break;

				case ProductType.DH:
					this.plannedCeilingArea = this.plannedFloorOrCeilingArea;
					this.plannedFloorOrCeilingArea = 0;
					this.plannedFloorArea = 0;
					break;

				default:
					this.plannedCeilingArea = 0;
					this.plannedFloorOrCeilingArea = 0;
					this.plannedFloorArea = 0;
					break;
			}
			if (pp != null) {
				int i = 1;
				foreach (HithermCircuit hc in this.circuits) {
					this.circuitIds[i] = hc;
					foreach (HithermRegister hr in hc.Registers) {
						this.registerCircuits[hr] = i;
						hr.PlannedProduct = pp;
					}
					foreach (GraphicalHithermVerbindung link in hc.Links) {
						link.FinalizeLoading();
					}
					i++;
				}
			}
		}

		[XmlIgnore]
		public override double PlannedHeizlastBereinigung {
			get {
				double bereinigung = 0;
				foreach (HithermCircuit hc in this.circuits) {
					bereinigung += hc.HeizleistungBereinigung;
				}
				return bereinigung;
			}
		}

		[XmlIgnore]
		public override double PlannedKuehllastBereinigung {
			get {
				double bereinigung = 0;
				foreach (HithermCircuit hc in this.circuits) {
					bereinigung += hc.KuehlleistungBereinigung;
				}
				return bereinigung;
			}
		}

		/// <summary>
		/// The percentage of the total room area that is occupied by the planned area.
		/// </summary>
		[XmlIgnore]
		public float PlannedFloorAreaPercentage {
			get {
				if (this.hithermType != ProductType.FBH) {
					return 0;
				}
				return (this.AssociatedRoom.Area <= 0 ? 100 : this.PlannedFloorArea * 100 / this.AssociatedRoom.Area);
			}
			set {
				if (this.hithermType == ProductType.FBH) {
					this.PlannedFloorArea = (float)(this.AssociatedRoom.Area * value / 100);
				}
			}
		}

		/// <summary>
		/// The percentage of the total room area that is occupied by the planned area.
		/// </summary>
		[XmlIgnore]
		public float PlannedCeilingAreaPercentage {
			get {
				if (this.hithermType != ProductType.DH) {
					return 0;
				}
				return (this.AssociatedRoom.Area <= 0 ? 100 : this.PlannedCeilingArea * 100 / this.AssociatedRoom.Area);
			}
			set {
				if (this.hithermType == ProductType.DH) {
					this.PlannedCeilingArea = (float)(this.AssociatedRoom.Area * value / 100);
				}
			}
		}

		public override void CalculateRequiredMaterial(SerializableDictionary<string, double> requiredMaterial) {

			this.AddRequiredMaterialForConnections(requiredMaterial, ConfigUsePlus, 0);

			double verbindeLength = 0;
			int teilflaechen = 0;
			double registerCount = 0;
			foreach (HithermCircuit c in this.circuits) {
				foreach (HithermRegister register in c.Registers) {
					teilflaechen++;
					registerCount += register.RegisterCount;

					// Register
					if (register.PartNumber != "") {
						Project.Instance.AddRequiredMaterial(requiredMaterial, register.PartNumber, register.RegisterCount);
					}
#if DEBUG
					else {
						MessageBox.Show("Hitherm Product not found.");
					}
#endif

					// Ovalschweißmuffen bei Hitherm+
					if (ConfigUsePlus && register.RegisterCount > 1) {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "EV10", 2 * (register.RegisterCount - 1));
					}

					// Ovalendkappen
					if (ConfigUsePlus) {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "HR65", 2);
					} else {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "HI65", 2);
					}

					//Wandwinkel
					int amount = 2;
					if (register.Orientation == HithermRegister.RegisterOrientationEnum.ORIENTATION_HORIZONTAL) {
						amount = 4;
					}
					if (ConfigUsePlus) {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "HR66", amount);
					} else {
						Project.Instance.AddRequiredMaterial(requiredMaterial, "HI66", amount);
					}

					verbindeLength += register.PipeHorizontal + register.PipeVertical;

				}
				// Bodenwinkel
				if (ConfigUsePlus) {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HR69", 2);
				} else {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HI68", 2);
				}

			}

			// Ovalmuffen
			if (verbindeLength > 0) {
				if (ConfigUsePlus) {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HR55", verbindeLength / 2);
				} else {
					Project.Instance.AddRequiredMaterial(requiredMaterial, "HI55", verbindeLength / 2);
				}
			}

			// Ovalrohr
			if (ConfigUsePlus) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HR60", verbindeLength);
			} else {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI60", verbindeLength);
			}

			// Dübelhaken
			Project.Instance.AddRequiredMaterial(requiredMaterial, "HI40", (registerCount * 2) + verbindeLength);

			// unknown amount
			if (ConfigUsePlus) {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HR67", Double.NegativeInfinity);
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HR70", Double.NegativeInfinity);
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HR68", Double.NegativeInfinity);

			} else {
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI67", Double.NegativeInfinity);
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI70", Double.NegativeInfinity);
				Project.Instance.AddRequiredMaterial(requiredMaterial, "HI71", Double.NegativeInfinity);
			}
		}

		public double PlannedRegisterArea {
			get {
				double area = 0;
				foreach (HithermCircuit hc in this.PlannedCircuits) {
					area += hc.CoveredArea;
				}
				return area;
			}
		}

		[XmlIgnore]
		public override double WasserInhalt {
			get {
				// Euroval Anbindung
				// 21mm Anbindung
				double pipeEurovalLength = 0;
				double pipe21mmLength = 0;
				foreach (ConnectionPipe pipe in this.PlannedConnectionPipes) {
					if (pipe.PipeType == ConnectionPipe.PipeTypeEnum.PT_21MM) {
						if (pipe.OnlyFirst) {
							pipe21mmLength += (pipe.Vorlauf + pipe.Ruecklauf);
						} else {
							pipe21mmLength += ((pipe.Vorlauf + pipe.Ruecklauf) * this.PlannedCircuitCount);
						}
					} else {
						if (pipe.OnlyFirst) {
							pipeEurovalLength += (pipe.Vorlauf + pipe.Ruecklauf);
						} else {
							pipeEurovalLength += ((pipe.Vorlauf + pipe.Ruecklauf) * this.PlannedCircuitCount);
						}
					}
				}

				double wasserInhalt = 0;
				foreach (HithermCircuit c in this.circuits) {
					foreach (HithermRegister register in c.Registers) {
						wasserInhalt += register.WasserInhaltProRohr * register.Rohre;
					}
				}

				return wasserInhalt + (pipeEurovalLength * EurovalProduct.rohrInnenA * 1000) + (pipe21mmLength * Product.rundrohr21mmInnenA * 1000);
			}
		}

		public override double Dichte {
			get { return HithermProduct.ConfigRho; }
		}

		public override double Waermekapazitaet {
			get { return HithermProduct.ConfigC; }
		}

		public override double Viskositaet {
			get { return HithermProduct.ConfigV; }
		}

		[XmlIgnore]
		public override WW.Math.Geometry.Polygon2D GraphicalArea {
			// TODO
			get { return null; }
		}

		public void ResetProduct() {
			registerCircuits = new Dictionary<HithermRegister, int>();
			circuitIds = new Dictionary<int, HithermCircuit>();

			hithermType = ProductType.WH;
			plannedFloorArea = 0;
			plannedCeilingArea = 0;
			plannedFloorOrCeilingArea = 0;
			this.circuits.Clear();
		}

		[XmlIgnore]
		public override bool AllowToSwitchMode {
			get { return this.circuits.Count == 0; }
		}

		public int GetNewHkId() {
			bool[] hkUsed = new bool[this.PlannedCircuits.Count + 1];
			for (int i = 0; i < hkUsed.Length; i++) {
				hkUsed[i] = false;
			}
			foreach (HithermCircuit c in this.PlannedCircuits) {
				int labelNr = c.HkLabelNr - 1;
				if (labelNr >= 0 && labelNr < hkUsed.Length) {
					hkUsed[labelNr] = true;
				}
			}
			int newHkId = hkUsed.Length;
			for (int i = 0; i < hkUsed.Length; i++) {
				if (!hkUsed[i]) {
					newHkId = i + 1;
					break;
				}
			}
			return newHkId;
		}

		public void CorrectCircuitIds() {
			List<HithermCircuit> notConnectedCircuits = new List<HithermCircuit>();
			for (int i = 0; i < this.PlannedCircuits.Count; i++) {
				HithermCircuit hc = this.PlannedCircuits[i] as HithermCircuit;
				if (!hc.IsConnectedToGround(false, false)) {
					notConnectedCircuits.Add(hc);
					this.PlannedCircuits.RemoveAt(i);
					i--;
				}
			}
			foreach (HithermCircuit hc in notConnectedCircuits) {
				this.PlannedCircuits.Add(hc);
			}
			this.circuitIds.Clear();
			this.registerCircuits.Clear();
			int nr = 1;
			PlannedProduct pp = Project.Instance.GetPlannedProduct(this);
			foreach (HithermCircuit hc in this.circuits) {
				this.circuitIds[nr] = hc;
				foreach (HithermRegister hr in hc.Registers) {
					this.registerCircuits[hr] = nr;
					hr.PlannedProduct = pp;
				}
				nr++;
			}

			/*int newId = this.GetNewHkId();
			while (newId < this.PlannedCircuits.Count + 1) {
				HithermCircuit circuitToRename = null;
				foreach (HithermCircuit c in this.PlannedCircuits) {
					if (this.GetCircuitId(c) > this.PlannedCircuits.Count) {
						circuitToRename = c;
					}
				}
				if (circuitToRename == null) {
					break;
				}
				this.ChangeCircuitId(circuitToRename, newId);
				newId = this.GetNewHkId();
			}*/
		}

		/*private int GetCircuitId(HithermCircuit circuit) {
			foreach (KeyValuePair<int, HithermCircuit> kvp in this.circuitIds) {
				if (kvp.Value == circuit) {
					return kvp.Key;
				}
			}
			return -1;
		}

		public void ChangeCircuitId(HithermCircuit circuit, int newId) {
			while (circuit.Registers != null && circuit.Registers.Count > 0) {
				this.MoveRegisterToCircuit(circuit.Registers[0], newId);
			}
		}*/

		public override PossibleProductConnection GetPossibleProductConnection(bool input, bool output, bool firstCircuit, bool otherCircuits, double measure, bool invertYAxis, Point2D currentMousePoint) {
			if (this.AssociatedRoom.RoomCoordinates.Count < 3 || !Polygon2D.IsInside(currentMousePoint, this.AssociatedRoom.RoomCoordinates) || (!firstCircuit && !otherCircuits) || (!input && !output) || this.circuits == null || this.circuits.Count < 1) {
				return null;
			}

			PossibleProductConnection possibleConnection = null;

			foreach (GraphicalProductConnection connection in this.Connections) {
				if (connection.FirstCircuit && ((input && connection.Vorlauf) || (output && connection.Ruecklauf))) {
					firstCircuit = false;
				}
				if (connection.OtherCircuits && ((input && connection.Vorlauf) || (output && connection.Ruecklauf))) {
					otherCircuits = false;
				}
			}


			int connectionsCount = 0;
			if (firstCircuit) {
				connectionsCount++;
			}
			if (otherCircuits) {
				connectionsCount += this.circuits.Count - 1;
			}
			if (input && output) {
				connectionsCount = connectionsCount * 2;
			}

			if (connectionsCount == 0) {
				return null;
			}

			double width = connectionsCount * 0.05 * measure;

			Segment2D segment;
			double bestDistance = double.MaxValue;
			Segment2D bestSegment = new Segment2D();
			Polygon2D room = new Polygon2D(this.AssociatedRoom.RoomCoordinates);
			if (room.IsClockwise()) {
				room.Reverse();
			}
			Point2D lastPoint = room[room.Count - 1];
			Point2D bestConnectionPoint = new Point2D();
			foreach (Point2D point in room) {
				segment = new Segment2D(lastPoint, point);
				if (segment.GetLength() >= width) {
					Point2D newConnectionPoint = segment.GetClosestPoint(currentMousePoint);
					if ((segment.Start - newConnectionPoint).GetLength() < width / 2) {
						Vector2D v = segment.End - segment.Start;
						v.Normalize();
						newConnectionPoint = segment.Start + v * (width / 2);
					}
					if ((segment.End - newConnectionPoint).GetLength() < width / 2) {
						Vector2D v = (segment.Start - segment.End);
						v.Normalize();
						newConnectionPoint = segment.End + v * (width / 2);
					}
					double distance = segment.GetDistance(currentMousePoint);
					//double distance = (newConnectionPoint - currentMousePoint).GetLength();
					if (distance < bestDistance) {
						bestDistance = distance;
						bestSegment = segment;
						bestConnectionPoint = newConnectionPoint;
					}
				}
				lastPoint = point;
			}
			if (bestDistance < 10) {
				//Point2D connectionPoint = bestSegment.GetClosestPoint(currentMousePoint);
				//if ((connectionPoint - bestSegment.Start).GetLength() >= width / 2 && (connectionPoint - bestSegment.End).GetLength() >= width / 2) {
				Polygon2D polygon = new Polygon2D();
				Vector2D v = bestSegment.End - bestSegment.Start;
				v.Normalize();
				Vector2D v2 = new Vector2D(-v.Y, v.X);
				polygon.Add(bestConnectionPoint + (v * width / 2));
				polygon.Add(bestConnectionPoint + (v * width / 2) + (v2 * 0.05 * measure));
				polygon.Add(bestConnectionPoint - (v * width / 2) + (v2 * 0.05 * measure));
				polygon.Add(bestConnectionPoint - (v * width / 2));

				double angle = -Math.Atan2(v.X, v.Y) * 180.0 / Math.PI;

				possibleConnection = new PossibleProductConnection(bestConnectionPoint, polygon, input, output, angle, this, firstCircuit, otherCircuits);
				//}
			}
			return possibleConnection;
		}
	}
	
}
