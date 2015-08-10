using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace Europlan.Common {

	public class EN1264 {

		private static EN1264 instance = null;
		private static readonly object padlock = new object();
		private static readonly ILog log = LogManager.GetLogger(typeof(EN1264));

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

		public const double KVSValue = 1.7;

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

		public double WaermestromDichteRegister(double heizmittelTemperatur, double raumTemperatur, double[][] standardTabelle, double faktor, bool compact) {
			double q;
			if (compact) {
				double[] y = new double[5];
				double x0;
				double x1;
				int x0i;
				int x1i;
				if (heizmittelTemperatur < 32.5) {
					x0 = raumTemperatur;
					x1 = 32.5;
					x0i = -1;
					x1i = 0;
				} else if (heizmittelTemperatur < 35.0) {
					x0 = 32.5;
					x1 = 35.0;
					x0i = 0;
					x1i = 1;
				} else if (heizmittelTemperatur < 37.5) {
					x0 = 35.0;
					x1 = 37.5;
					x0i = 1;
					x1i = 2;
				} else if (heizmittelTemperatur < 40.0) {
					x0 = 37.5;
					x1 = 40.0;
					x0i = 2;
					x1i = 3;
				} else if (heizmittelTemperatur < 42.5) {
					x0 = 40.0;
					x1 = 42.5;
					x0i = 3;
					x1i = 4;
				} else if (heizmittelTemperatur < 45.0) {
					x0 = 42.5;
					x1 = 45.0;
					x0i = 4;
					x1i = 5;
				} else {
					x0 = raumTemperatur;
					x1 = 45.0;
					x0i = -1;
					x1i = 5;
				}
				double y0;
				double y1;
				double x = heizmittelTemperatur;
				double[] tmp = { 15.0, 18.0, 20.0, 22.0, 24.0 };
				for (int i = 0; i < 5; i++) {
					if (x0i < 0) {
						x0 = tmp[i];
					}
					if (x1i < 0) {
						x1 = tmp[i];
					}
					y0 = x0i < 0 ? 0 : standardTabelle[i][x0i];
					y1 = x1i < 0 ? 0 : standardTabelle[i][x1i];
					y[i] = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				}
				if (raumTemperatur < 15.0) {
					x0 = 15;
					x1 = heizmittelTemperatur;
					x0i = 0;
					x1i = -1;
				} else if (raumTemperatur < 18.0) {
					x0 = 15;
					x1 = 18;
					x0i = 0;
					x1i = 1;
				} else if (raumTemperatur < 20.0) {
					x0 = 18;
					x1 = 20;
					x0i = 1;
					x1i = 2;
				} else if (raumTemperatur < 22.0) {
					x0 = 20;
					x1 = 22;
					x0i = 2;
					x1i = 3;
				} else if (raumTemperatur < 24.0) {
					x0 = 22;
					x1 = 24;
					x0i = 3;
					x1i = 4;
				} else {
					x0 = 24;
					x1 = heizmittelTemperatur;
					x0i = 4;
					x1i = -1;
				}
				y0 = x0i < 0 ? 0 : y[x0i];
				y1 = x1i < 0 ? 0 : y[x1i];
				x = raumTemperatur;
				q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
			} else {
				double[] y = new double[5];
				double x0;
				double x1;
				int x0i;
				int x1i;
				if (heizmittelTemperatur < 30.0) {
					x0 = raumTemperatur;
					x1 = 30.0;
					x0i = -1;
					x1i = 0;
				} else if (heizmittelTemperatur < 32.5) {
					x0 = 30.0;
					x1 = 32.5;
					x0i = 0;
					x1i = 1;
				} else if (heizmittelTemperatur < 35.0) {
					x0 = 32.5;
					x1 = 35.0;
					x0i = 1;
					x1i = 2;
				} else if (heizmittelTemperatur < 37.5) {
					x0 = 35.0;
					x1 = 37.5;
					x0i = 2;
					x1i = 3;
				} else if (heizmittelTemperatur < 40.0) {
					x0 = 37.5;
					x1 = 40.0;
					x0i = 3;
					x1i = 4;
				} else if (heizmittelTemperatur < 42.5) {
					x0 = 40.0;
					x1 = 42.5;
					x0i = 4;
					x1i = 5;
				} else if (heizmittelTemperatur < 45.0) {
					x0 = 42.5;
					x1 = 45.0;
					x0i = 5;
					x1i = 6;
				} else if (heizmittelTemperatur < 47.5) {
					x0 = 45.0;
					x1 = 47.5;
					x0i = 6;
					x1i = 7;
				} else if (heizmittelTemperatur < 50.0) {
					x0 = 47.5;
					x1 = 50.0;
					x0i = 7;
					x1i = 8;
				} else {
					x0 = raumTemperatur;
					x1 = 50.0;
					x0i = -1;
					x1i = 8;
				}
				double y0;
				double y1;
				double x = heizmittelTemperatur;
				double[] tmp = { 15.0, 18.0, 20.0, 22.0, 24.0 };
				for (int i = 0; i < 5; i++) {
					if (x0i < 0) {
						x0 = tmp[i];
					}
					if (x1i < 0) {
						x1 = tmp[i];
					}
					y0 = x0i < 0 ? 0 : standardTabelle[i][x0i];
					y1 = x1i < 0 ? 0 : standardTabelle[i][x1i];
					y[i] = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				}
				if (raumTemperatur < 15.0) {
					x0 = 15;
					x1 = heizmittelTemperatur;
					x0i = 0;
					x1i = -1;
				} else if (raumTemperatur < 18.0) {
					x0 = 15;
					x1 = 18;
					x0i = 0;
					x1i = 1;
				} else if (raumTemperatur < 20.0) {
					x0 = 18;
					x1 = 20;
					x0i = 1;
					x1i = 2;
				} else if (raumTemperatur < 22.0) {
					x0 = 20;
					x1 = 22;
					x0i = 2;
					x1i = 3;
				} else if (raumTemperatur < 24.0) {
					x0 = 22;
					x1 = 24;
					x0i = 3;
					x1i = 4;
				} else {
					x0 = 24;
					x1 = heizmittelTemperatur;
					x0i = 4;
					x1i = -1;
				}
				y0 = x0i < 0 ? 0 : y[x0i];
				y1 = x1i < 0 ? 0 : y[x1i];
				x = raumTemperatur;
				q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
			}
			return q < 0 ? 0 : q * faktor;
		}

		public double KaeltestromDichteRegister(double kuehlmittelTemperatur, double raumTemperatur, double[] standardTabelle, double faktor) {
			if (kuehlmittelTemperatur >= raumTemperatur) {
				return 0;
			}

			double d = raumTemperatur - kuehlmittelTemperatur;

			double x0;
			double x1;
			int x0i;
			int x1i;
			if (d < 2.0) {
				x0 = 0.0;
				x1 = 2.0;
				x0i = 0;
				x1i = 1;
			} else if (d < 3.0) {
				x0 = 2.0;
				x1 = 3.0;
				x0i = 1;
				x1i = 2;
			} else if (d < 4.0) {
				x0 = 3.0;
				x1 = 4.0;
				x0i = 2;
				x1i = 3;
			} else if (d < 5.0) {
				x0 = 4.0;
				x1 = 5.0;
				x0i = 3;
				x1i = 4;
			} else if (d < 6.0) {
				x0 = 5.0;
				x1 = 6.0;
				x0i = 4;
				x1i = 5;
			} else if (d < 7.0) {
				x0 = 6.0;
				x1 = 7.0;
				x0i = 5;
				x1i = 6;
			} else if (d < 9.0) {
				x0 = 7.0;
				x1 = 9.0;
				x0i = 6;
				x1i = 7;
			} else {
				x0 = 0.0;
				x1 = 9.0;
				x0i = 0;
				x1i = 7;
			}
			double y0 = x0i < 0 ? 0 : standardTabelle[x0i];
			double y1 = x1i < 0 ? 0 : standardTabelle[x1i];
			double x = d;
			double q = y0 + (y1 - y0) / (x1 - x0) * (d - x0);

			if (q < 0) {
				q = 0;
			}

			return -q * faktor;
		}

		public double KaeltestromDichteRegister(double kuehlmittelTemperatur, double raumTemperatur, double[][] standardTabelle, double faktor) {
			if (raumTemperatur <= kuehlmittelTemperatur) {
				return 0;
			}

			double x0;
			double x1;
			double y0;
			double y1;
			double x;
			double[] y = new double[4];
			double q;
			double[] tmp = { 16.0, 18.0, 20.0, 22.0 };
			if (kuehlmittelTemperatur < 16.0) {
				for (int i = 0; i < 4; i++) {
					// Berechnung mit 16° und maximaler Kühlmitteltemperatur
					x = kuehlmittelTemperatur;
					x0 = tmp[0];
					x1 = tmp[standardTabelle[i].Length - 1];
					y0 = standardTabelle[i][0];
					y1 = standardTabelle[i][standardTabelle[i].Length - 1];
					y[i] = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				}
				if (raumTemperatur < 18.0) {
					x = raumTemperatur;
					x0 = kuehlmittelTemperatur;
					x1 = 25;
					y0 = 0;
					y1 = y[3];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else if (raumTemperatur < 20.0) {
					x = raumTemperatur;
					x0 = 18.0;
					x1 = 20.0;
					y0 = y[0];
					y1 = y[1];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else if (raumTemperatur < 22.0) {
					x = raumTemperatur;
					x0 = 20.0;
					x1 = 22.0;
					y0 = y[1];
					y1 = y[2];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else if (raumTemperatur < 25.0) {
					x = raumTemperatur;
					x0 = 22.0;
					x1 = 25.0;
					y0 = y[2];
					y1 = y[3];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else {
					x = raumTemperatur;
					x0 = kuehlmittelTemperatur;
					x1 = 25.0;
					y0 = 0;
					y1 = y[3];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				}
			} else if (kuehlmittelTemperatur < 18.0) {
				for (int i = 0; i < 4; i++) {
					x = kuehlmittelTemperatur;
					x0 = tmp[0];
					x1 = tmp[1];
					y0 = standardTabelle[i][0];
					y1 = standardTabelle[i][1];
					y[i] = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				}
				if (raumTemperatur < 18.0) {
					x = raumTemperatur;
					x0 = kuehlmittelTemperatur;
					x1 = 18;
					y0 = 0;
					y1 = y[0];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else if (raumTemperatur < 20.0) {
					x = raumTemperatur;
					x0 = 18.0;
					x1 = 20.0;
					y0 = y[0];
					y1 = y[1];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else if (raumTemperatur < 22.0) {
					x = raumTemperatur;
					x0 = 20.0;
					x1 = 22.0;
					y0 = y[1];
					y1 = y[2];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else if (raumTemperatur < 25.0) {
					x = raumTemperatur;
					x0 = 22.0;
					x1 = 25.0;
					y0 = y[2];
					y1 = y[3];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else {
					x = raumTemperatur;
					x0 = kuehlmittelTemperatur;
					x1 = 25.0;
					y0 = 0;
					y1 = y[3];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				}
			} else if (kuehlmittelTemperatur < 20.0) {
				for (int i = 1; i < 4; i++) {
					x = kuehlmittelTemperatur;
					x0 = tmp[1];
					x1 = tmp[2];
					y0 = standardTabelle[i][1];
					y1 = standardTabelle[i][2];
					y[i] = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				}
				if (raumTemperatur < 20.0) {
					x = raumTemperatur;
					x0 = kuehlmittelTemperatur;
					x1 = 20;
					y0 = 0;
					y1 = y[1];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else if (raumTemperatur < 22.0) {
					x = raumTemperatur;
					x0 = 20.0;
					x1 = 22.0;
					y0 = y[1];
					y1 = y[2];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else if (raumTemperatur < 25.0) {
					x = raumTemperatur;
					x0 = 22.0;
					x1 = 25.0;
					y0 = y[2];
					y1 = y[3];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else {
					x = raumTemperatur;
					x0 = kuehlmittelTemperatur;
					x1 = 25.0;
					y0 = 0;
					y1 = y[3];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				}
			} else if (kuehlmittelTemperatur < 22.0) {
				for (int i = 2; i < 4; i++) {
					x = kuehlmittelTemperatur;
					x0 = tmp[2];
					x1 = tmp[3];
					y0 = standardTabelle[i][2];
					y1 = standardTabelle[i][3];
					y[i] = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				}
				if (raumTemperatur < 22.0) {
					x = raumTemperatur;
					x0 = kuehlmittelTemperatur;
					x1 = 22;
					y0 = 0;
					y1 = y[2];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else if (raumTemperatur < 25.0) {
					x = raumTemperatur;
					x0 = 22.0;
					x1 = 25.0;
					y0 = y[2];
					y1 = y[3];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else {
					x = raumTemperatur;
					x0 = kuehlmittelTemperatur;
					x1 = 25.0;
					y0 = 0;
					y1 = y[3];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				}
			} else {
				for (int i = 3; i < 4; i++) {
					x = kuehlmittelTemperatur;
					x0 = tmp[3];
					x1 = 25.0;
					y0 = standardTabelle[i][3];
					y1 = 0;
					y[i] = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				}
				if (raumTemperatur < 25.0) {
					x = raumTemperatur;
					x0 = kuehlmittelTemperatur;
					x1 = 25;
					y0 = 0;
					y1 = y[3];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				} else {
					x = raumTemperatur;
					x0 = kuehlmittelTemperatur;
					x1 = 25.0;
					y0 = 0;
					y1 = y[3];
					q = y0 + (y1 - y0) / (x1 - x0) * (x - x0);
				}
			}

			if (q < 0) {
				q = 0;
			}
			return -q * faktor;
		}

		public double HithermBeplankungsFaktor(double[] rWerte, double[] faktoren, double rWert) {
			double[] c = null;
			spline3.buildcubicspline(rWerte, faktoren, rWerte.Length < faktoren.Length ? rWerte.Length : faktoren.Length, 0, 0, 0, 0, ref c);
			double rtn = spline3.splineinterpolation(ref c, rWert);
			if (rtn > 1) {
				rtn = 1;
			}
			if (rtn < 0) {
				rtn = 0;
			}
			return rtn;
		}

		public double OberflaechenTemperatur(double waermestrom, double alpha, double raumTemperatur) {
			return (waermestrom / alpha) + raumTemperatur;
		}

		public double TaupunktTemperatur(double luftFeuchte, double raumTemperatur) {
			return (Math.Pow(luftFeuchte, 0.12468828) * (raumTemperatur + 109.8)) - 109.8;
		}

		public double WaermeverlustAussen(double alpha, double RlambdaB, double sU, double lambdaU, double RalphaDecke, double RlambdaIns, double RlambdaDecke, double RlambdaPutz, double q, double innenTemperatur, double untenTemperatur) {
			double Ru = RlambdaIns + RlambdaDecke + RlambdaPutz + RalphaDecke;
			return WaermeverlustAussen(alpha, RlambdaB, sU, lambdaU, Ru, q, innenTemperatur, untenTemperatur);
		}

		public double WaermeverlustAussen(double alpha, double RlambdaB, double sU, double lambdaU, double Ru, double q, double innenTemperatur, double untenTemperatur) {
			double Ro = (1.0 / alpha) + RlambdaB + (sU / lambdaU);
			double qu = (1.0 / Ru) * ((Ro * q) + innenTemperatur - untenTemperatur);
			return qu;
		}

		public double WaermeverlustAussen(double q, double rI, double rA, double innenTemperatur, double aussenTemperatur) {
			double qA = (1.0 / rA) * ((rI * q) + innenTemperatur - aussenTemperatur);
			return qA;
		}

		public double WaermeverlustAussen(double q, double alphaInnen, double rConstrInnen, double alphaAussen, double rDaemmungAussen, double temperaturInnen, double temperaturAussen) {
			double rI = 1.0 / alphaInnen + rConstrInnen;
			double rA = 1.0 / alphaAussen + rDaemmungAussen;
			return WaermeverlustAussen(q, rI, rA, temperaturInnen, temperaturAussen);
		}

		public double Massenstrom(double leistung, double c, double spreizung) {
			return leistung / (c * (1000.0 / 3600.0) * spreizung);
		}

		public double FlussGeschwindigkeit(double durchfluss, double rohrInnenQuerschnitt) {
			return FlussGeschwindigkeit(durchfluss, rohrInnenQuerschnitt, 1000);
		}

		public double FlussGeschwindigkeit(double massenstrom, double rohrInnenQuerschnitt, double dichte) {
			return massenstrom / (rohrInnenQuerschnitt * (dichte * 3600.0));
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

		public double DruckverlustRohr(double massenstrom, double rohrInnenQuerschnitt, double dichte, double rohrInnenDurchmesser, double viskositaet, double k, double rohrLaenge) {
			double flussGeschwindigkeit = FlussGeschwindigkeit(massenstrom, rohrInnenQuerschnitt, dichte);
			double reynoldsZahl = ReynoldsZahl(flussGeschwindigkeit, rohrInnenDurchmesser, viskositaet);
			double lambda = WiderstandsBeiwert(reynoldsZahl, k, rohrInnenDurchmesser);
			return (lambda * (rohrLaenge / rohrInnenDurchmesser) * dichte * (Math.Pow(flussGeschwindigkeit, 2) / 2)) / 100;
		}

		public double DruckverlustRohr(double leistung, double c, double spreizung, double rohrInnenQuerschnitt, double dichte, double rohrInnenDurchmesser, double viskositaet, double k, double rohrLaenge) {
			double massenstrom = Massenstrom(leistung, c, spreizung);
			return DruckverlustRohr(massenstrom, rohrInnenQuerschnitt, dichte, rohrInnenDurchmesser, viskositaet, k, rohrLaenge);
		}

		public double DruckverlustModul_100_40(int anzahl, double massenstrom) {
			double[] x = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500 };
			double[] y = ModulKlimaBodenProduct.ConfigDruckverlustModul_100_40;
			double[] c = null;
			spline3.buildcubicspline(x, y, 30, 0, 0, 0, 0, ref c);
			if (anzahl > 0 && anzahl <= 40) {
				return spline3.splineinterpolation(ref c, massenstrom) * anzahl;
			} else {
				return 0;
			}
		}

		public double DruckverlustModul_120_30(int anzahl, double massenstrom) {
			double[] x = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500 };
			double[] y = ModulKlimaDeckeProduct.ConfigDruckverlustModul_120_30;
			double[] c = null;
			spline3.buildcubicspline(x, y, 30, 0, 0, 0, 0, ref c);
			if (anzahl > 0 && anzahl <= 40) {
				return spline3.splineinterpolation(ref c, massenstrom) * anzahl;
			} else {
				return 0;
			}
		}

		public double DruckverlustModul_100_30(int anzahl, double massenstrom) {
			double[] x = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500 };
			double[] y = ModulKlimaDeckeProduct.ConfigDruckverlustModul_100_30;
			double[] c = null;
			spline3.buildcubicspline(x, y, 30, 0, 0, 0, 0, ref c);
			if (anzahl > 0 && anzahl <= 40) {
				return spline3.splineinterpolation(ref c, massenstrom) * anzahl;
			} else {
				return 0;
			}
		}

		public double DruckverlustModul_80_30(int anzahl, double massenstrom) {
			double[] x = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500 };
			double[] y = ModulKlimaDeckeProduct.ConfigDruckverlustModul_80_30;
			double[] c = null;
			spline3.buildcubicspline(x, y, 30, 0, 0, 0, 0, ref c);
			if (anzahl > 0 && anzahl <= 40) {
				return spline3.splineinterpolation(ref c, massenstrom) * anzahl;
			} else {
				return 0;
			}
		}

		public double DruckverlustModul_100_40_20(int anzahl, double massenstrom) {
			double[] x = { 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400};
			double[] y = ModulKlimaBoden20Product.ConfigDruckverlustModul_100_40_20;
			double[] c = null;
			spline3.buildcubicspline(x, y, 24, 0, 0, 0, 0, ref c);
			if (anzahl > 0 && anzahl <= 40) {
				return spline3.splineinterpolation(ref c, massenstrom) * anzahl;
			} else {
				return 0;
			}
		}

		/// <summary>
		/// Berechnet den Druckversult eines Hitherm Registers. Der Durchfluss/Massenstrom muss dieser Methode in kg/h
		/// uebergeben werden und nicht in l/h wie in der Tabelle im Hitherm Produktkatalog!
		/// </summary>
		public double DruckverlustRegister(HithermRegister.HithermRegisterTypeEnum type, int width, double massenstrom) {
			double[] x1;
			double[] x2;
			double[][] y;

			x1 = new double[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400 };

			switch (type) {
				case HithermRegister.HithermRegisterTypeEnum.HIT_50_5:
					x2 = new double[] { 25, 50, 75, 100, 125, 150 };
					y = HithermProduct.ConfigDruckverlustHIT_50_5;
					break;

				case HithermRegister.HithermRegisterTypeEnum.HIT_50_10:
					x2 = new double[] { 50, 100, 150, 200, 250, 300 };
					y = HithermProduct.ConfigDruckverlustHIT_50_10;
					break;

				case HithermRegister.HithermRegisterTypeEnum.HIT_100_5:
					x2 = new double[] { 25, 50, 75, 100, 125, 150 };
					y = HithermProduct.ConfigDruckverlustHIT_100_5;
					break;

				case HithermRegister.HithermRegisterTypeEnum.HIT_100_10:
					x2 = new double[] { 50, 100, 150, 200, 250, 300 };
					y = HithermProduct.ConfigDruckverlustHIT_100_10;
					break;

				case HithermRegister.HithermRegisterTypeEnum.HIT_150_5:
					x2 = new double[] { 25, 50, 75, 100, 125, 150 };
					y = HithermProduct.ConfigDruckverlustHIT_150_5;
					break;

				case HithermRegister.HithermRegisterTypeEnum.HIT_150_10:
					x2 = new double[] { 50, 100, 150, 200, 250, 300 };
					y = HithermProduct.ConfigDruckverlustHIT_150_10;
					break;

				case HithermRegister.HithermRegisterTypeEnum.HIT_200_5:
					x2 = new double[] { 25, 50, 75, 100, 125, 150 };
					y = HithermProduct.ConfigDruckverlustHIT_200_5;
					break;

				case HithermRegister.HithermRegisterTypeEnum.HIT_200_10:
					x2 = new double[] { 50, 100, 150, 200, 250, 300 };
					y = HithermProduct.ConfigDruckverlustHIT_200_10;
					break;

				case HithermRegister.HithermRegisterTypeEnum.HIT_250_5:
					x2 = new double[] { 25, 50, 75, 100, 125, 150 };
					y = HithermProduct.ConfigDruckverlustHIT_250_5;
					break;

				case HithermRegister.HithermRegisterTypeEnum.HIT_250_10:
					x2 = new double[] { 50, 100, 150, 200, 250, 300 };
					y = HithermProduct.ConfigDruckverlustHIT_250_10;
					break;

				case HithermRegister.HithermRegisterTypeEnum.HIT_300_5:
					x2 = new double[] { 25, 50, 75, 100, 125, 150 };
					return 0;

				case HithermRegister.HithermRegisterTypeEnum.HIT_300_10:
					x2 = new double[] { 50, 100, 150, 200, 250, 300 };
					return 0;

                default:
					return 0;
			}

			double[] y2 = new double[y.Length];

			for (int i = 0; i < y.Length; i++) {
				double[] ct = null;
				spline3.buildcubicspline(x1, y[i], x1.Length, 0, 0, 0, 0, ref ct);
				y2[i] = spline3.splineinterpolation(ref ct, massenstrom);
				if (y2[i] < 0) {
					y2[i] = 0;
				}
			}

			double[] c = null;
			spline3.buildcubicspline(x2, y2, x2.Length, 0, 0, 0, 0, ref c);
			double rtn = spline3.splineinterpolation(ref c, width);
			if (rtn < 0.1) {
				rtn = 0.1;
			}
			return rtn;
		}

		/// <summary>
		/// Berechnet den Druckversult eines Hitherm Registers. Der Durchfluss/Massenstrom muss dieser Methode in kg/h
		/// uebergeben werden und nicht in l/h wie in der Tabelle im Hitherm Produktkatalog!
		/// </summary>
		public double DruckverlustRegister(HithermCompactRegister.HithermCompactRegisterTypeEnum type, double massenstrom) {
			switch (type) {
				case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Std:
				case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_620_Ds:
					return this.DruckverlustRegister(HithermRegister.HithermRegisterTypeEnum.HIT_50_5, 50, massenstrom);

				case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Std:
				case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Ds:
				case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1000_Par:
					return this.DruckverlustRegister(HithermRegister.HithermRegisterTypeEnum.HIT_100_5, 50, massenstrom);

				case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Std:
				case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Ds:
				case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_1500_Par:
					return this.DruckverlustRegister(HithermRegister.HithermRegisterTypeEnum.HIT_150_5, 50, massenstrom);

				case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Std:
				case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Ds:
				case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2000_Par:
					return this.DruckverlustRegister(HithermRegister.HithermRegisterTypeEnum.HIT_200_5, 50, massenstrom);

				case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Std:
				case HithermCompactRegister.HithermCompactRegisterTypeEnum.HITC_2500_Ds:
					return this.DruckverlustRegister(HithermRegister.HithermRegisterTypeEnum.HIT_250_5, 50, massenstrom);

				default:
					return 0;
			}
		}

		public double DefaultSpreizung(double vorlaufTemperatur) {
			if (vorlaufTemperatur <= 35.0) {
				return 5.0;
			} else if (vorlaufTemperatur <= 38.0) {
				return 5.0 * (vorlaufTemperatur - 38.0) / (35.0 - 38.0) + 6.0 * (vorlaufTemperatur - 35.0) / (38.0 - 35.0);
			} else if (vorlaufTemperatur <= 41.0) {
				return 6.0 * (vorlaufTemperatur - 41.0) / (38.0 - 41.0) + 7.0 * (vorlaufTemperatur - 38.0) / (41.0 - 38.0);
			} else if (vorlaufTemperatur <= 44.0) {
				return 7.0 * (vorlaufTemperatur - 44.0) / (41.0 - 44.0) + 8.0 * (vorlaufTemperatur - 41.0) / (44.0 - 41.0);
			} else if (vorlaufTemperatur <= 46.5) {
				return 8.0;
			} else if (vorlaufTemperatur <= 50) {
				return 8 * (vorlaufTemperatur - 50.0) / (46.5 - 50.0) + 10.0 * (vorlaufTemperatur - 46.5) / (50.0 - 46.5);
			} else {
				return 10.0;
			}
		}
	}

}
