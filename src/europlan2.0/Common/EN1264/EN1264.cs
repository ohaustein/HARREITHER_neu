using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Europlan.Common {

	public class EN1264 {

		private static EN1264 instance = null;
		private static readonly object padlock = new object();
		private static readonly ILog log = LogManager.GetLogger(typeof(EN1264));
		private System.ComponentModel.ComponentResourceManager resources = ResourcesManager.resources;

		public const double alpha0 = 10.8;
		public const double alphaHeizen = 10.8;
		public const double alphaKuehlen = 6.5;
		public const double B0 = 6.5;
		public const double atmt = 1.06;
		public const double lambdaU0 = 1;
		public const double sU0 = 0.045;
		public const double sU = 0.035;
		public const double lambdaE = 1.2;
		public const double sr = 0.00238; 
		public const double lambdaR = 0.22; 
		public const double sr0 = 0.002; 
		public const double lambdaR0 = 0.35;
		public const double lambdaU = 1.2;
		public const double rAlphaDecke = 0.17;
		public const double c = 4.19;
		public const double k = 0.000004;
		public const double dichte = 1000.0;
		public const double viskositaet = 0.00000101;

		protected EN1264() {
			log.Debug("default constructor called");
		}

		/// <summary>
		/// Gets the instance of the class (Singleton)
		/// </summary>
		public static EN1264 Instance {
			get {
				if (instance == null) {
					lock (padlock) {
						if (instance == null) {
							try {
								instance = new EN1264();
							} catch {
								instance = null;
							}
						}
					}
				}
				return instance;
			}
		}

		public double Heizmitteluebertemperatur(double vorlaufTemperatur, double ruecklaufTemperatur, double raumTemperatur) {
			return (vorlaufTemperatur - ruecklaufTemperatur) / Math.Log((vorlaufTemperatur - raumTemperatur) / (ruecklaufTemperatur - raumTemperatur));
		}

		public double ab(double alpha0, double alpha, double sU0, double lambdaU0, double lambdaE, double RlambdaB) {
			double numerator = (1 / alpha0) + (sU0 / lambdaU0);
			double denominator = (1 / alpha) + (sU0 / lambdaE) + (RlambdaB);
			return numerator / denominator;
		}

		public double abFlaeche(double B, double au, double atmt, double RlambdaB) {
			return 1.0 / (1 + B * au * atmt * RlambdaB);
		}

		public double at(double RlambdaB) {
			double[] x = { 0, 0.05, 0.10, 0.15 };
			double[] y = { 1.23, 1.188, 1.156, 1.134 };
			double[] c = null;
			spline3.buildcubicspline(x, y, 4, 0, 0, 0, 0, ref c);
			return spline3.splineinterpolation(ref c, RlambdaB);
		}

		public double mt(double T) {
			return 1 - (T / 0.075);
		}

		public double au(double T, double RlambdaB) {
			double[] x =    { 0.05 , 0.075, 0.1   , 0.15  , 0.2   , 0.225 , 0.3   , 0.375  };
			double[] y000 = { 1.069, 1.066, 1.0630, 1.0570, 1.0510, 1.0480, 1.0395, 1.0300 };
			double[] y005 = { 1.056, 1.053, 1.0500, 1.0460, 1.0410, 1.0380, 1.0310, 1.0221 };
			double[] y010 = { 1.043, 1.041, 1.0390, 1.0350, 1.0315, 1.0295, 1.0240, 1.0181 };
			double[] y015 = { 1.037, 1.035, 1.0335, 1.0305, 1.0275, 1.0260, 1.0210, 1.0150 };

			double[] c = null;
			double[] yt = new double[4];

			spline3.buildcubicspline(x, y000, 8, 0, 0, 0, 0, ref c);
			yt[0] = spline3.splineinterpolation(ref c, T);
			spline3.buildcubicspline(x, y005, 8, 0, 0, 0, 0, ref c);
			yt[1] = spline3.splineinterpolation(ref c, T);
			spline3.buildcubicspline(x, y010, 8, 0, 0, 0, 0, ref c);
			yt[2] = spline3.splineinterpolation(ref c, T);
			spline3.buildcubicspline(x, y015, 8, 0, 0, 0, 0, ref c);
			yt[3] = spline3.splineinterpolation(ref c, T);

			double[] xr = { 0, 0.05, 0.10, 0.15 };
			spline3.buildcubicspline(xr, yt, 4, 0, 0, 0, 0, ref c);
			return spline3.splineinterpolation(ref c, RlambdaB);
		}

		public double auFlaeche(double alpha0, double alpha, double sU0, double lambdaU0, double sU, double lambdaE) {
			double numerator = (1 / alpha0) + (sU0 / lambdaU0);
			double denominator = (1 / alpha) + (sU / lambdaE);
			return numerator / denominator;
		}

		public double mu(double su) {
			return 100 * (0.045 - su);
		}

		public double ad(double T, double RlambdaB) {
			double[] x =    { 0.05, 0.075, 0.1, 0.15, 0.2, 0.225, 0.3, 0.375 };
			double[] y000 = { 1.013, 1.021, 1.029, 1.040, 1.046, 1.049, 1.053, 1.056 };
			double[] y005 = { 1.013, 1.019, 1.025, 1.034, 1.040, 1.043, 1.049, 1.051 };
			double[] y010 = { 1.012, 1.016, 1.022, 1.029, 1.035, 1.038, 1.044, 1.046 };
			double[] y015 = { 1.011, 1.014, 1.018, 1.024, 1.030, 1.033, 1.039, 1.042 };

			double[] c = null;
			double[] yt = new double[4];

			spline3.buildcubicspline(x, y000, 8, 0, 0, 0, 0, ref c);
			yt[0] = spline3.splineinterpolation(ref c, T);
			spline3.buildcubicspline(x, y005, 8, 0, 0, 0, 0, ref c);
			yt[1] = spline3.splineinterpolation(ref c, T);
			spline3.buildcubicspline(x, y010, 8, 0, 0, 0, 0, ref c);
			yt[2] = spline3.splineinterpolation(ref c, T);
			spline3.buildcubicspline(x, y015, 8, 0, 0, 0, 0, ref c);
			yt[3] = spline3.splineinterpolation(ref c, T);

			double[] xr = { 0, 0.05, 0.10, 0.15 };
			spline3.buildcubicspline(xr, yt, 4, 0, 0, 0, 0, ref c);
			return spline3.splineinterpolation(ref c, RlambdaB);
		}

		public double md(double D) {
			return 250 * (D - 0.02);
		}

		public double PotenzProduktFussboden(double alpha0, double alpha, double sU0, double lambdaU0, double lambdaE, double RlambdaB, double T, double su, double D) {
			return PotenzProduktFussbodenGeometrie(alpha0, alpha, sU0, lambdaU0, lambdaE, RlambdaB, T, su, D, 1);
		}

		public double PotenzProduktFussbodenGeometrie(double alpha0, double alpha, double sU0, double lambdaU0, double lambdaE, double RlambdaB, double T, double su, double D, double ag) {
			return PotenzProduktFussbodenGeometrieUndWandteilungsFaktor(alpha0, alpha, sU0, lambdaU0, lambdaE, RlambdaB, T, su, D, ag, 1, 1);
		}

		public double PotenzProduktFussbodenGeometrieUndWandteilungsFaktor(double alpha0, double alpha, double sU0, double lambdaU0, double lambdaE, double RlambdaB, double T, double su, double D, double ag, double aw, double mw) {
			double ab = this.ab(alpha0, alpha, sU0, lambdaU0, lambdaE, RlambdaB);
			double at = this.at(RlambdaB);
			double mt = this.mt(T);
			double au = this.au(T, RlambdaB);
			double mu = this.mu(su);
			double ad = this.ad(T, RlambdaB);
			double md = this.md(D);

			return ab *
				Math.Pow(at, mt) *
				Math.Pow(au, mu) *
				Math.Pow(ad, md) *
				ag *
				Math.Pow(aw, mw);
		}

		public double SystemabhaengigerKoeffizient(double B0, double alpha0, double alpha, double sU0, double lambdaU0, double lambdaE, double RlambdaB, double T, double su, double D, double sr, double sr0, double lambdaR, double lambdaR0) {
			return SystemabhaengigerKoeffizientGeometrie(B0, alpha0, alpha, sU0, lambdaU0, lambdaE, RlambdaB, T, su, D, 1, sr, sr0, lambdaR, lambdaR0);
		}

		public double SystemabhaengigerKoeffizientGeometrie(double B0, double alpha0, double alpha, double sU0, double lambdaU0, double lambdaE, double RlambdaB, double T, double su, double D, double ag, double sr, double sr0, double lambdaR, double lambdaR0) {
			return SystemabhaengigerKoeffizientGeometrieUndWandteilungsFaktor(B0, alpha0, alpha, sU0, lambdaU0, lambdaE, RlambdaB, T, su, D, ag, 1, 1, sr, sr0, lambdaR, lambdaR0);
		}

		public double SystemabhaengigerKoeffizientGeometrieUndWandteilungsFaktor(double B0, double alpha0, double alpha, double sU0, double lambdaU0, double lambdaE, double RlambdaB, double T, double su, double D, double ag, double aw, double mw, double sr, double sr0, double lambdaR, double lambdaR0) {
			double potenzProdukt = PotenzProduktFussbodenGeometrieUndWandteilungsFaktor(alpha0, alpha, sU0, lambdaU0, lambdaE, RlambdaB, T, su, D, ag, aw, mw);
			return 1.0 / ((1.0 / B0) +
				((1.1 / Math.PI) *
				potenzProdukt *
				T *
				((1.0 / (2.0 * lambdaR)) * (Math.Log(D / (D - (2.0 * sr)))) -
				(1.0 / (2.0 * lambdaR0)) * (Math.Log(D / (D - (2.0 * sr0)))))));
		}

		public double WaermedurchgangsKoeffizientRohr(double B, double potenzProdukt) {
			return B * potenzProdukt;
		}

		public double WaermestromDichteRohr(double K, double heizmittelUebertemperatur) {
			return K * heizmittelUebertemperatur;
		}

		public double WaermestromDichteFlaeche(double B, double ab, double atmt, double au, double heizmittelUebertemperatur) {
			return B * ab * atmt * au * heizmittelUebertemperatur;
		}

		public double WaermestromDichteRegister(double heizmittelTemperatur, double raumTemperatur, double[][] standardTabelle, double faktor) {
			double[] x = { 30.0, 32.5, 35.0, 37.5, 40.0, 42.5, 45.0, 47.5, 50.0 };

			int l = standardTabelle.Length;

			double[] c = null;
			double[] y = new double[l];

			for (int i = 0; i < l; i++) {
				spline3.buildcubicspline(x, standardTabelle[i], 9, 0, 0, 0, 0, ref c);
				y[i] = spline3.splineinterpolation(ref c, heizmittelTemperatur);
			}

			double[] x2 = { 15, 18, 20, 22, 24 };
			spline3.buildcubicspline(x2, y, 5, 0, 0, 0, 0, ref c);
			return spline3.splineinterpolation(ref c, raumTemperatur) * faktor;
		}

		public double OberflaechenTemperatur(double waermestrom, double alpha, double raumTemperatur) {
			return (waermestrom / alpha) + raumTemperatur;
		}

		public double TaupunktTemperatur(double luftFeuchte, double raumTemperatur) {
			return (Math.Pow(luftFeuchte, 0.12468828) * (raumTemperatur + 109.8)) - 109.8;
		}

		public double WaermeverlustUnten(double alpha, double RlambdaB, double sU, double lambdaU, double RalphaDecke, double RlambdaIns, double RlambdaDecke, double RlambdaPutz, double q, double innenTemperatur, double untenTemperatur) {
			//double Ro = (1.0 / alpha) + RlambdaB + (sU / lambdaU);
			double Ru = RlambdaIns + RlambdaDecke + RlambdaPutz + RalphaDecke;
			//double qu = (1.0 / Ru) * ((Ro * q) + innenTemperatur - untenTemperatur);
			//return qu;
			return WaermeverlustUnten(alpha, RlambdaB, sU, lambdaU, Ru, q, innenTemperatur, untenTemperatur);
		}

		public double WaermeverlustUnten(double alpha, double RlambdaB, double sU, double lambdaU, double Ru, double q, double innenTemperatur, double untenTemperatur) {
			double Ro = (1.0 / alpha) + RlambdaB + (sU / lambdaU);
			double qu = (1.0 / Ru) * ((Ro * q) + innenTemperatur - untenTemperatur);
			return qu;
		}

		public double Durchfluss(double leistung, double c, double spreizung) {
			return leistung / (c * (1000.0 / 3600.0) * spreizung);
		}

		public double FlussGeschwindigkeit(double durchfluss, double rohrInnenQuerschnitt, double dichte) {
			return durchfluss / (rohrInnenQuerschnitt * (dichte * 3600.0));
		}

		public double ReynoldsZahl(double flussGeschwindigkeit, double rohrInnenDurchmesser, double viskositaet) {
			return (flussGeschwindigkeit * rohrInnenDurchmesser) / viskositaet;
		}

		public double WiderstandsBeiwert(double reynoldszahl, double k, double rohrInnenDurchmesser) {
			if (reynoldszahl <= 2320) {
				return 64.0 / reynoldszahl;
			} else {
				double lambda = 0.3;
				for (int i = 0; i < 5; i++) {
					lambda = Math.Pow(1.0 / ((-2.0) * Math.Log10((2.51/(reynoldszahl * Math.Sqrt(lambda))) + (k/(3.71 * rohrInnenDurchmesser)))), 2);
				}
				return lambda;
			}
		}

		public double DruckverlustRohr(double durchfluss, double rohrInnenQuerschnitt, double dichte, double rohrInnenDurchmesser, double viskositaet, double k, double rohrLaenge) {
			double flussGeschwindigkeit = FlussGeschwindigkeit(durchfluss, rohrInnenQuerschnitt, dichte);
			double reynoldsZahl = ReynoldsZahl(flussGeschwindigkeit, rohrInnenDurchmesser, viskositaet);
			double lambda = WiderstandsBeiwert(reynoldsZahl, k, rohrInnenDurchmesser);
			return (lambda * (rohrLaenge / rohrInnenDurchmesser) * dichte * (Math.Pow(flussGeschwindigkeit, 2) / 2)) / 100;
		}

		public double DruckverlustRohr(double leistung, double c, double spreizung, double rohrInnenQuerschnitt, double dichte, double rohrInnenDurchmesser, double viskositaet, double k, double rohrLaenge) {
			double durchfluss = Durchfluss(leistung, c, spreizung);
			return DruckverlustRohr(durchfluss, rohrInnenQuerschnitt, dichte, rohrInnenDurchmesser, viskositaet, k, rohrLaenge);
		}

		public double DruckverlustModul_100_40(int anzahl, double durchfluss) {
			double[] x = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500 };
			double[] y = { 0.2, 0.35, 0.65, 0.9, 1.25, 1.5, 1.8, 2.2, 2.6, 3, 3.6, 4.5, 5.4, 6.3, 7.2, 8.1, 9.1, 10, 11, 12, 13, 14, 15, 16.5, 17.8, 19, 20, 21.5, 23, 25 };
			double[] c = null;
			spline3.buildcubicspline(x, y, 30, 0, 0, 0, 0, ref c);
			if (anzahl > 0 && anzahl <= 40) {
				return spline3.splineinterpolation(ref c, durchfluss) * anzahl;
			} else {
				return 0;
			}
		}

		public double DruckverlustModul_120_30(int anzahl, double durchfluss) {
			double[] x = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500 };
			double[] y = { 0.23, 0.47, 0.82, 1.05, 1.5, 1.75, 2.1, 2.6, 3, 3.5, 4.2, 5.25, 6.3, 7.3, 8.4, 9.4, 10.6, 11.7, 12.8, 14, 15.1, 16.3, 17.5, 19.2, 20.7, 22.1, 23.3, 25, 26.8, 29.1 };
			double[] c = null;
			spline3.buildcubicspline(x, y, 30, 0, 0, 0, 0, ref c);
			if (anzahl > 0 && anzahl <= 40) {
				return spline3.splineinterpolation(ref c, durchfluss) * anzahl;
			} else {
				return 0;
			}
		}

		public double DruckverlustModul_100_30(int anzahl, double durchfluss) {
			double[] x = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500 };
			double[] y = { 0.19, 0.37, 0.65, 0.84, 1.2, 1.4, 1.7, 2.1, 2.4, 2.8, 3.3, 4.2, 5, 5.9, 7, 7.5, 8.5, 9.3, 10.3, 11.2, 12.1, 13, 14, 15.4, 16.6, 17.7, 18.6, 20, 21.4, 23.3 };
			double[] c = null;
			spline3.buildcubicspline(x, y, 30, 0, 0, 0, 0, ref c);
			if (anzahl > 0 && anzahl <= 40) {
				return spline3.splineinterpolation(ref c, durchfluss) * anzahl;
			} else {
				return 0;
			}
		}

		public double DruckverlustModul_80_30(int anzahl, double durchfluss) {
			double[] x = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500 };
			double[] y = { 0.17, 0.34, 0.6, 0.77, 1.1, 1.3, 1.5, 1.9, 2.2, 2.6, 3.1, 3.8, 4.6, 5.4, 6.1, 6.9, 7.7, 8.5, 9.4, 10.2, 11.1, 11.9, 12.8, 14, 15.1, 16.2, 17, 18.3, 19.6, 21.3 };
			double[] c = null;
			spline3.buildcubicspline(x, y, 30, 0, 0, 0, 0, ref c);
			if (anzahl > 0 && anzahl <= 40) {
				return spline3.splineinterpolation(ref c, durchfluss) * anzahl;
			} else {
				return 0;
			}
		}

		public double DefaultSpreizung(double vorlaufTemperatur) {
			double[] x = { 30.0, 32.5, 35.0, 38.0, 41.0, 44.0, 46.5, 50.0, 52.5, 55.0};
			double[] y = {  5.0,  5.0,  5.0,  6.0,  7.0,  8.0,  8.0, 10.0, 10.0, 10.0};
			double[] c = null;
			spline3.buildcubicspline(x, y, 10, 0, 0, 0, 0, ref c);
			if (vorlaufTemperatur < 35.0) {
				return 5.0;
			} else if (vorlaufTemperatur > 35.0 && vorlaufTemperatur <= 38.0) {
				return 5.0 * (vorlaufTemperatur - 38.0) / (35.0 - 38.0) + 6.0 * (vorlaufTemperatur - 35.0) / (38.0 - 35.0);
			} else if (vorlaufTemperatur > 38.0 && vorlaufTemperatur <= 41.0) {
				return 6.0 * (vorlaufTemperatur - 41.0) / (38.0 - 41.0) + 7.0 * (vorlaufTemperatur - 38.0) / (41.0 - 38.0);
			} else if (vorlaufTemperatur > 41.0 && vorlaufTemperatur <= 44.0) {
				return 7.0 * (vorlaufTemperatur - 44.0) / (41.0 - 44.0) + 8.0 * (vorlaufTemperatur - 41.0) / (44.0 - 41.0);
			} else if (vorlaufTemperatur > 44.0 && vorlaufTemperatur <= 46.5) {
				return 8.0;
			} else if (vorlaufTemperatur > 46.5 && vorlaufTemperatur <= 50) {
				return 8 * (vorlaufTemperatur - 50.0) / (46.5 - 50.0) + 10.0 * (vorlaufTemperatur - 46.5) / (50.0 - 46.5);
			} else if (vorlaufTemperatur > 50.0) {
				return 10.0;
			} else {
				return spline3.splineinterpolation(ref c, vorlaufTemperatur);
			}
		}
	}

}
