using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Europlan.Common;

namespace Europlan.UnitTest {

	[TestFixture] // telling NUnit that this class contains test functions 
	public class EN1264Test {

		EN1264 norm = null;

		[SetUp]
		public void SetUp() {
			norm = EN1264.Instance;
		}

		[TearDown]
		public void TearDown() {
		}

		[Test]
		public void TestHeizmitteluebertemperatur() {
			double result = norm.Heizmitteluebertemperatur(35, 30, 20);
			Assert.AreEqual(12.33151731, Math.Round(result, 8));
			result = norm.Heizmitteluebertemperatur(17, 20, 25);
			Assert.AreEqual(-6.38292944, Math.Round(result, 8));
			result = norm.Heizmitteluebertemperatur(15, 17, 26);
			Assert.AreEqual(-9.966577309, Math.Round(result, 9));
		}

		[Test]
		public void TestAB() {
			double result = norm.ab(EN1264.alpha0, EN1264.alphaHeizen, EN1264.sU0, EN1264.lambdaU0, EN1264.lambdaE, 0.1);
			Assert.AreEqual(0.597987928, Math.Round(result, 9));
			result = norm.ab(EN1264.alpha0, EN1264.alphaKuehlen, EN1264.sU0, EN1264.lambdaU0, EN1264.lambdaE, 0.1);
			Assert.AreEqual(0.472265004, Math.Round(result, 9));
		}

		[Test]
		public void TestABFlaeche() {
			double result = norm.abFlaeche(6.5, 1.485465233, 1.06, 0.01);
			Assert.AreEqual(0.90715409, Math.Round(result, 8));
			result = norm.abFlaeche(6.5, 1.111028037, 1.06, 0);
			Assert.AreEqual(1, Math.Round(result, 0));
		}

		[Test]
		public void TestAT() {
			double result = norm.at(0);
			Assert.AreEqual(1.230, Math.Round(result, 3));
			result = norm.at(0.05);
			Assert.AreEqual(1.188, Math.Round(result, 3));
			result = norm.at(0.10);
			Assert.AreEqual(1.156, Math.Round(result, 3));
			result = norm.at(0.125);
			Assert.AreEqual(1.144, Math.Round(result, 3));
			result = norm.at(0.15);
			Assert.AreEqual(1.134, Math.Round(result, 3));
			result = norm.at(0.20);
			Assert.AreEqual(1.122, Math.Round(result, 3));
		}

		[Test]
		public void TestMT() {
			double result = norm.mt(0.2);
			Assert.AreEqual(-1.6667, Math.Round(result, 4));
		}

		[Test]
		public void TestAU() {
			double result = norm.au(0.20, 0.10);
			Assert.AreEqual(1.0315, result);
			result = norm.au(0.15, 0.05);
			Assert.AreEqual(1.046, result);
			result = norm.au(0.225, 0.10);
			Assert.AreEqual(1.0295, result);
		}

		[Test]
		public void TestAUFlaeche() {
			double result = norm.auFlaeche(EN1264.alpha0, EN1264.alphaHeizen, 0.045, 1.0, 0.002, 60);
			Assert.AreEqual(1.485465233, Math.Round(result, 9));
			result = norm.auFlaeche(EN1264.alpha0, EN1264.alphaHeizen, 0.045, 1.0, 0.01, 0.32);
			Assert.AreEqual(1.111028037, Math.Round(result, 9));
		}

		[Test]
		public void TestMU() {
			double result = norm.mu(0.035);
			Assert.AreEqual(1, Math.Round(result, 4));
		}

		[Test]
		public void TestAD() {
			double result = norm.ad(0.20, 0.10);
			Assert.AreEqual(1.035, result);
			result = norm.ad(0.15, 0.05);
			Assert.AreEqual(1.034, result);
			result = norm.ad(0.225, 0.10);
			Assert.AreEqual(1.038, result);
		}
		
		[Test]
		public void TestMD() {
			double result = norm.md(0.0206505);
			Assert.AreEqual(0.162625, Math.Round(result, 6));
		}

		[Test]
		public void TestPotenzproduktFussboden() {
			double result = norm.PotenzProduktFussboden(EN1264.alpha0, EN1264.alphaHeizen, EN1264.sU0, EN1264.lambdaU0, EN1264.lambdaE, 0.1, 0.2, EN1264.sU, 0.0206505);
			Assert.AreEqual(0.48714876, Math.Round(result, 8));
		}

		[Test]
		public void TestPotenzproduktFussbodenGeometrie() {
			double result = norm.PotenzProduktFussbodenGeometrie(EN1264.alpha0, EN1264.alphaHeizen, EN1264.sU0, EN1264.lambdaU0, EN1264.lambdaE, 0.1, 0.2, EN1264.sU, 0.0206505, 1.1034);
			Assert.AreEqual(0.537519939, Math.Round(result, 9));
			result = norm.PotenzProduktFussbodenGeometrie(EN1264.alpha0, EN1264.alphaKuehlen, EN1264.sU0, EN1264.lambdaU0, EN1264.lambdaE, 0.1, 0.2, EN1264.sU, 0.0206505, 1.1034);
			Assert.AreEqual(0.42451, Math.Round(result, 5));
		}

		[Test]
		public void TestPotenzproduktFussbodenGeometrieUndWandteilungsFaktor() {
			double result = norm.PotenzProduktFussbodenGeometrieUndWandteilungsFaktor(EN1264.alpha0, 8, EN1264.sU0, EN1264.lambdaU0, 1, 0, 0.05, 0.015, 0.01, 1, 1, 2);
			Assert.AreEqual(1.02570862, Math.Round(result, 9));
		}

		[Test]
		public void TestSystemabhaengigerKoeffizient() {
			double result = norm.SystemabhaengigerKoeffizient(6.7, EN1264.alpha0, EN1264.alphaHeizen, EN1264.sU0, EN1264.lambdaU0, EN1264.lambdaE, 0.1, 0.2, EN1264.sU, 0.0206505, EN1264.sr, EN1264.sr0, EN1264.lambdaR, EN1264.lambdaR0);
			Assert.AreEqual(6.286301313, Math.Round(result, 9));
		}

		[Test]
		public void TestSystemabhaengigerKoeffizientGeometrie() {
			double result = norm.SystemabhaengigerKoeffizientGeometrie(6.7, EN1264.alpha0, EN1264.alphaHeizen, EN1264.sU0, EN1264.lambdaU0, EN1264.lambdaE, 0.1, 0.2, EN1264.sU, 0.0206505, 1.1034, EN1264.sr, EN1264.sr0, EN1264.lambdaR, EN1264.lambdaR0);
			Assert.AreEqual(6.246420765, Math.Round(result, 9));
			result = norm.SystemabhaengigerKoeffizientGeometrie(6.7, EN1264.alpha0, EN1264.alphaKuehlen, EN1264.sU0, EN1264.lambdaU0, EN1264.lambdaE, 0.1, 0.2, EN1264.sU, 0.0206505, 1.1034, EN1264.sr, EN1264.sr0, EN1264.lambdaR, EN1264.lambdaR0);
			Assert.AreEqual(6.33661057, Math.Round(result, 8));
		}

		[Test]
		public void TestSystemabhaengigerKoeffizientGeometrieUndWandteilungsFaktor() {
			double result = norm.SystemabhaengigerKoeffizientGeometrieUndWandteilungsFaktor(6.7, EN1264.alpha0, 8, EN1264.sU0, EN1264.lambdaU0, 1, 0, 0.05, 0.015, 0.01, 1, 1, 2, 0.0015, EN1264.sr0, EN1264.lambdaR, EN1264.lambdaR0);
			Assert.AreEqual(6.635436076, Math.Round(result, 9));
		}

		[Test]
		public void TestWaermedurchgangsKoeffizientRohr() {
			double result = norm.WaermedurchgangsKoeffizientRohr(6.5, 0.5);
			Assert.AreEqual(3.25, result);
		}

		[Test]
		public void TestWaermestromDichteRohr() {
			double result = norm.WaermestromDichteRohr(6.5, 0.5);
			Assert.AreEqual(3.25, result);
		}

		[Test]
		public void TestWaermestromDichteFlaeche() {
			double result = norm.WaermestromDichteFlaeche(6.5, 0.90715409, 1.06, 1.485465233, 14.79782077);
			Assert.AreEqual(137.39, Math.Round(result, 2));
			result = norm.WaermestromDichteFlaeche(6.5, 1, 1.06, 1.111028037, -9.966577309);
			Assert.AreEqual(-76.29, Math.Round(result, 2));
		}

		[Test]
		public void TestOberflaechenTemperatur() {
			double result = norm.OberflaechenTemperatur(40.0, 10.8, 25);
			Assert.AreEqual(28.7037, Math.Round(result, 4));
		}	

		[Test]
		public void TestTaupunktTemperatur() {
			double result = norm.TaupunktTemperatur(0.5, 26);
			Assert.AreEqual(14.76, Math.Round(result, 2));
		}		

		[Test]
		public void TestWaermeverlustUnten() {
			double result = norm.WaermeverlustAussen(EN1264.alphaHeizen, 0.1, EN1264.sU, EN1264.lambdaU, EN1264.rAlphaDecke, 1.25, 0.11, 0.015, 37.76, 20, 20);
			Assert.AreEqual(5.42, Math.Round(result, 2));
			result = norm.WaermeverlustAussen(8, 0, 0.015, 1.2, EN1264.rAlphaDecke, 1.25, 0.11, 0.02, 100.71, 20, -18);
			Assert.AreEqual(33.45, Math.Round(result, 2));
			result = norm.WaermeverlustAussen(EN1264.alpha0, 0.1, 0.035, 1.2, EN1264.rAlphaDecke, 1.25, 0.11, 0.02, -17.17, 25, 25);
			Assert.AreEqual(-2.46, Math.Round(result, 2));
		}

		[Test]
		public void TestDurchfluss() {
			double result = norm.Massenstrom(854.0, EN1264.c, 5.0);
			Assert.AreEqual(146.7, Math.Round(result, 1));
		}

		[Test]
		public void TestFlussgeschwindigkeit() {
			double result = norm.FlussGeschwindigkeit(146.7, 0.000183783, 1000);
			Assert.AreEqual(0.222, Math.Round(result, 3));
		}

		[Test]
		public void TestReynoldszahl() {
			double result = norm.ReynoldsZahl(0.221803557, 0.0153, 0.00000101);
			Assert.AreEqual(3360, Math.Round(result, 0));
		}

		[Test]
		public void TestWiderstandsBeiwert() {
			double result = norm.WiderstandsBeiwert(64, 1, 1);
			Assert.AreEqual(1, result);
			result = norm.WiderstandsBeiwert(128, 1, 1);
			Assert.AreEqual(0.5, result);
			result = norm.WiderstandsBeiwert(64, 10, 10);
			Assert.AreEqual(1, result);
			result = norm.WiderstandsBeiwert(3360.0, EN1264.k, 0.0153);
			Assert.AreEqual(0.042275, Math.Round(result, 6));
		}

		[Test]
		public void TestDruckverlustRohr() {
			double result = norm.DruckverlustRohr(854.0, EN1264.c, 5, 0.000183783, EN1264.dichte, 0.0153, EN1264.viskositaet, EN1264.k, 80.0);
			Assert.AreEqual(54.37, Math.Round(result, 2));
		}

		[Test]
		public void TestDruckverlustModul() {
			double result = norm.DruckverlustModul_100_40(1, 100);
			Assert.AreEqual(3, Math.Round(result, 2));
			result = norm.DruckverlustModul_100_40(15, 100);
			Assert.AreEqual(45, Math.Round(result, 2));
		}

		[Test]
		public void TestDefaultSpreizung() {
			Assert.AreEqual(5.0, norm.DefaultSpreizung(25.9));
			Assert.AreEqual(5.0, norm.DefaultSpreizung(30.0));
			Assert.AreEqual(5.0, norm.DefaultSpreizung(32.5));
			Assert.AreEqual(5.0, norm.DefaultSpreizung(35.0));
			Assert.AreEqual(6.0, norm.DefaultSpreizung(38.0));
			Assert.AreEqual(6.7, Math.Round(norm.DefaultSpreizung(40.0), 1));
			Assert.AreEqual(7.0, norm.DefaultSpreizung(41.0));
			Assert.AreEqual(8.0, norm.DefaultSpreizung(44.0));
			Assert.AreEqual(8.0, norm.DefaultSpreizung(45.5));
			Assert.AreEqual(8.0, norm.DefaultSpreizung(46.5));
			Assert.AreEqual(10.0, norm.DefaultSpreizung(50.0));
			Assert.AreEqual(10.0, norm.DefaultSpreizung(52.5));
			Assert.AreEqual(10.0, norm.DefaultSpreizung(55.0));
			Assert.AreEqual(10.0, norm.DefaultSpreizung(59.7));
		}

		[Test]
		public void TestWaermestromDichteRegister() {
			double[][] table = {new double[] {105,120,140,155,175,190,210,225,240},
								new double[] { 85,100,120,135,155,170,185,205,220},
								new double[] { 70, 85,105,120,140,155,175,190,210},
								new double[] { 55, 70, 90,105,125,140,160,175,195},
								new double[] { 45, 60, 80, 95,115,130,145,165,180}};
			double result = norm.WaermestromDichteRegister(35.0, 18.0, table, 1.0, false);
			Assert.AreEqual(120.0, result);
			result = norm.WaermestromDichteRegister(35.0, 18.0, table, 1.1, false);
			Assert.AreEqual(132.0, result);
			result = norm.WaermestromDichteRegister(50.0, 19.0, table, 1.0, false);
			Assert.AreEqual(215.0, Math.Round(result, 0));
		}

		[Test]
		public void TestWaermestromdichteRegisterStetig() {
			double[][] table = {new double[] {105,120,140,155,175,190,210,225,240},
								new double[] { 85,100,120,135,155,170,185,205,220},
								new double[] { 70, 85,105,120,140,155,175,190,210},
								new double[] { 55, 70, 90,105,125,140,160,175,195},
								new double[] { 45, 60, 80, 95,115,130,145,165,180}};

			for (double ti = 5.0; ti < 30.0; ti += 0.2) {
				bool first = true;
				double oldVal = 0;
				for (double thm = ti; thm < 60.0; thm += 0.2) {
					double value = norm.WaermestromDichteRegister(thm, ti, table, 1, false);
					if (first) {
						first = false;
					} else {
						Assert.IsTrue(oldVal < value, "Heizleistung bei ti=" + ti.ToString() + "°C, tHm1=" + (thm - 0.2).ToString() + "°C, tHm2=" + thm.ToString() + "°C nicht stetig steigend");
					}
					oldVal = value;
				}
			}
		}

		[Test]
		public void TestWaermestromdichteCompactRegisterStetig() {
			double[][] regHeizleistung2000Par = {
				//  tHm (°C)  32.5  35.0  37.5  40.0  42.5  45.0
				new double[] { 160,  180,  205,  225,  250,  275}, // ti=15°C
				new double[] { 130,  155,  175,  200,  220,  245}, // ti=18°C
				new double[] { 115,  135,  160,  180,  205,  225}, // ti=20°C
				new double[] {  95,  120,  140,  165,  185,  210}, // ti=22°C
				new double[] {  75,  100,  125,  145,  170,  190}  // ti=24°C
			};

			for (double ti = 5.0; ti < 30.0; ti += 0.2) {
				bool first = true;
				double oldVal = 0;
				for (double thm = ti; thm < 60.0; thm += 0.2) {
					double value = norm.WaermestromDichteRegister(thm, ti, regHeizleistung2000Par, 1, true);
					if (first) {
						first = false;
					} else {
						Assert.IsTrue(oldVal < value, "Heizleistung bei ti=" + ti.ToString() + "°C, tHm1=" + (thm - 0.2).ToString() + "°C, tHm2=" + thm.ToString() + "°C nicht stetig steigend");
					}
					oldVal = value;
				}
			}
		}

		[Test]
		public void TestKaeltestromdichteCompactRegister() {
			double[][] regKuehlleistungProQm = {
				//  tKm (°C)  16.0  18.0  20.0  22.0
				new double[] {12.5,  0.0            }, // ti=18°C
				new double[] {24.0, 12.5,  0.0      }, // ti=20°C
				new double[] {39.0, 25.0, 12.5,  0.0}, // ti=22°C
				new double[] {58.0, 44.0, 32.0, 19.5}  // ti=25°C
			};


			Assert.AreEqual(0.0, norm.KaeltestromDichteRegister(16.0, 16.0, regKuehlleistungProQm, 1));
			Assert.AreEqual(-12.5 / 2.0, norm.KaeltestromDichteRegister(16.0, 17.0, regKuehlleistungProQm, 1));
			Assert.AreEqual(-25.0, norm.KaeltestromDichteRegister(16.0, 18.0, regKuehlleistungProQm, 2));
			Assert.AreEqual(0.0, norm.KaeltestromDichteRegister(20.0, 19.0, regKuehlleistungProQm, 1));
			Assert.AreEqual(-38.4, norm.KaeltestromDichteRegister(20.0, 26.0, regKuehlleistungProQm, 1));
			Assert.AreEqual(-19.5, norm.KaeltestromDichteRegister(22.0, 25.0, regKuehlleistungProQm, 1));
			Assert.AreEqual(-116.0, norm.KaeltestromDichteRegister(16.0, 34.0, regKuehlleistungProQm, 1));
			Assert.AreEqual(-36.0, norm.KaeltestromDichteRegister(14.0, 20.0, regKuehlleistungProQm, 1));
			Assert.AreEqual(-25.125, norm.KaeltestromDichteRegister(17.0, 21.0, regKuehlleistungProQm, 1));
		}

		[Test]
		public void TestKaeltestromdichteCompactRegister0GradDiff() {
			double[][] regKuehlleistungProQm = {
				//  tKm (°C)  16.0  18.0  20.0  22.0
				new double[] {12.5,  0.0            }, // ti=18°C
				new double[] {24.0, 12.5,  0.0      }, // ti=20°C
				new double[] {39.0, 25.0, 12.5,  0.0}, // ti=22°C
				new double[] {58.0, 44.0, 32.0, 19.5}  // ti=25°C
			};

			for (double i = 5.0; i < 30.0; i += 0.2) {
				double value = norm.KaeltestromDichteRegister(i, i, regKuehlleistungProQm, 1);
				Assert.AreEqual(0.0, value, "Kühlleistung für tKm=" + i.ToString() + "°C, ti=" + (i + 2.0).ToString() + "°C ist nicht 0 (" + (-value).ToString() + "W/m²)");
			}
		}

		[Test]
		public void TestKaeltestromdichteCompactRegister2GradDiff() {
			double[][] regKuehlleistungProQm = {
				//  tKm (°C)  16.0  18.0  20.0  22.0
				new double[] {12.5,  0.0            }, // ti=18°C
				new double[] {24.0, 12.5,  0.0      }, // ti=20°C
				new double[] {39.0, 25.0, 12.5,  0.0}, // ti=22°C
				new double[] {58.0, 44.0, 32.0, 19.5}  // ti=25°C
			};

			for (double i = 5.0; i < 30.0; i += 0.2) {
				double value = norm.KaeltestromDichteRegister(i, i + 2.0, regKuehlleistungProQm, 1);
				Assert.IsTrue(value > -13.1, "Kühlleistung für tKm=" + i.ToString() + "°C, ti=" + (i + 2.0).ToString() + "°C zu groß (" + (-value).ToString() + "W/m² > 13.1W/m²)");
				Assert.IsTrue(value < -12.0, "Kühlleistung für tKm=" + i.ToString() + "°C, ti=" + (i + 2.0).ToString() + "°C zu klein (" + (-value).ToString() + "W/m² < 12.0W/m²)");
			}
		}

		[Test]
		public void TestKaeltestromdichteCompactRegister5GradDiff() {
			double[][] regKuehlleistungProQm = {
				//  tKm (°C)  16.0  18.0  20.0  22.0
				new double[] {12.5,  0.0            }, // ti=18°C
				new double[] {24.0, 12.5,  0.0      }, // ti=20°C
				new double[] {39.0, 25.0, 12.5,  0.0}, // ti=22°C
				new double[] {58.0, 44.0, 32.0, 19.5}  // ti=25°C
			};

			for (double i = 5.0; i < 30.0; i += 0.2) {
				double value = norm.KaeltestromDichteRegister(i, i + 5.0, regKuehlleistungProQm, 1);
				Assert.IsTrue(value > -33.5, "Kühlleistung für tKm=" + i.ToString() + "°C, ti=" + (i + 5.0).ToString() + "°C zu groß (" + (-value).ToString() + "W/m² > 33.5W/m²)");
				Assert.IsTrue(value < -30.0, "Kühlleistung für tKm=" + i.ToString() + "°C, ti=" + (i + 5.0).ToString() + "°C zu klein (" + (-value).ToString() + "W/m² < 30.0W/m²)");
			}
		}

		[Test]
		public void TestKaeltestromdichteCompactRegisterStetig() {
			double[][] regKuehlleistungProQm = {
				//  tKm (°C)  16.0  18.0  20.0  22.0
				new double[] {12.5,  0.0            }, // ti=18°C
				new double[] {24.0, 12.5,  0.0      }, // ti=20°C
				new double[] {39.0, 25.0, 12.5,  0.0}, // ti=22°C
				new double[] {58.0, 44.0, 32.0, 19.5}  // ti=25°C
			};

			for (double ti = 5.0; ti < 30.0; ti += 0.2) {
				bool first = true;
				double oldVal = 0;
				for (double tkm = 5.0; tkm < ti; tkm += 0.2) {
					double value = norm.KaeltestromDichteRegister(tkm, ti, regKuehlleistungProQm, 1);
					if (first) {
						first = false;
					} else {
						Assert.IsTrue(oldVal < value, "Kühlleistung bei ti=" + ti.ToString() + "°C, tKm1=" + (tkm - 0.2).ToString() + "°C, tKm2=" + tkm.ToString() + "°C nicht stetig fallend");
					}
					oldVal = value;
				}
			}
		}

		[Test]
		public void TestKaeltestromdichteStdRegister() {
			double[] stdRegKuehlleistung = { 0, 9, 14, 18, 24, 29, 32, 43 };

			Assert.AreEqual(-86.0, norm.KaeltestromDichteRegister(5.0, 23.0 , stdRegKuehlleistung, 1));
			Assert.AreEqual(0.0, norm.KaeltestromDichteRegister(18.0, 18.0, stdRegKuehlleistung, 1));
			Assert.AreEqual(-43.5, norm.KaeltestromDichteRegister(16.0, 22.0, stdRegKuehlleistung, 1.5));
		}

		[Test]
		public void TestKaeltestromdichteStdRegisterStetig() {
			double[] stdRegKuehlleistung = { 0, 9, 14, 18, 24, 29, 32, 43 };

			for (double ti = 5.0; ti < 30.0; ti += 0.2) {
				bool first = true;
				double oldVal = 0;
				for (double tkm = 5.0; tkm < ti; tkm += 0.2) {
					double value = norm.KaeltestromDichteRegister(tkm, ti, stdRegKuehlleistung, 1);
					if (first) {
						first = false;
					} else {
						Assert.IsTrue(oldVal < value, "Kühlleistung bei ti=" + ti.ToString() + "°C, tKm1=" + (tkm - 0.2).ToString() + "°C, tKm2=" + tkm.ToString() + "°C nicht stetig fallend");
					}
					oldVal = value;
				}
			}
		}

		[Test]
		public void TestKaeltestromdichteHlRegisterStetig() {
			double[] hlRegKuehlleistung = { 0, 13, 20, 25, 33, 40, 45, 60 };

			for (double ti = 5.0; ti < 30.0; ti += 0.2) {
				bool first = true;
				double oldVal = 0;
				for (double tkm = 5.0; tkm < ti; tkm += 0.2) {
					double value = norm.KaeltestromDichteRegister(tkm, ti, hlRegKuehlleistung, 1);
					if (first) {
						first = false;
					} else {
						Assert.IsTrue(oldVal < value, "Kühlleistung bei ti=" + ti.ToString() + "°C, tKm1=" + (tkm - 0.2).ToString() + "°C, tKm2=" + tkm.ToString() + "°C nicht stetig fallend");
					}
					oldVal = value;
				}
			}
		}
	}
}
