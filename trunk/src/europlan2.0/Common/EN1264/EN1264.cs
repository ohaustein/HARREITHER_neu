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

		public double WaermedurchgangsKoeffizient(double B, double potenzProdukt) {
			return B * potenzProdukt;
		}

		public double WaermestromDichte(double K, double heizmittelUebertemperatur) {
			return K * heizmittelUebertemperatur;
		}

		public double WaemeverlustUnten(double alpha, double RlambdaB, double sU, double lambdaU, double RalphaDecke, double RlambdaIns, double RlambdaDecke, double RlambdaPutz, double q, double innenTemperatur, double untenTemperatur) {
			double Ro = (1.0 / alpha) + RlambdaB + (sU / lambdaU);
			double Ru = RlambdaIns + RlambdaDecke + RlambdaPutz + RalphaDecke;
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

		public double Druckverlust(double leistung, double c, double spreizung, double rohrInnenQuerschnitt, double dichte, double rohrInnenDurchmesser, double viskositaet, double k, double rohrLaenge) {
			double durchfluss = Durchfluss(leistung, c, spreizung);
			double flussGeschwindigkeit = FlussGeschwindigkeit(durchfluss, rohrInnenQuerschnitt, dichte);
			double reynoldsZahl = ReynoldsZahl(flussGeschwindigkeit, rohrInnenDurchmesser, viskositaet);
			double lambda = WiderstandsBeiwert(reynoldsZahl, k, rohrInnenDurchmesser);
			return (lambda * (rohrLaenge / rohrInnenDurchmesser) * dichte * (Math.Pow(flussGeschwindigkeit, 2) / 2)) / 100;
		}
	}

}
