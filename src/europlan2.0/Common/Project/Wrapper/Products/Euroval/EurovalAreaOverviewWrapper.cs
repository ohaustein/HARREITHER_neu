using System;
using System.Collections.Generic;
using System.Text;

namespace Europlan.Common {

	public class EurovalAreaOverviewWrapper {

		private string layDistance;
		private double azArea;
		private double rzArea;
		private double connectingArea;

		public string LayDistance {
			get { return layDistance; }
			set { layDistance = value; }
		}

		public double TotalArea {
			get { return azArea + rzArea + connectingArea; }
		}

		public double AzArea {
			get { return azArea; }
			set { azArea = value; }
		}

		public double RzArea {
			get { return rzArea; }
			set { rzArea = value; }
		}
		
		public double ConnectingArea {
			get { return connectingArea; }
			set { connectingArea = value; }
		}

		/// <summary>
		/// Validiert die Flächenaufstellung auf mögliche Fehler.
		/// </summary>
		/// <returns>Liste von Fehlermeldungen. Leer, wenn keine Fehler gefunden wurden.</returns>
		public List<string> ValidateFlaechenaufstellung() {
			List<string> errors = new List<string>();

			// Prüfung: LayDistance darf nicht leer oder null sein
			if (string.IsNullOrWhiteSpace(layDistance)) {
				errors.Add("Verlegeabstand (LayDistance) ist nicht gesetzt.");
			}

			// Prüfung: Flächen dürfen nicht negativ sein
			if (azArea < 0) {
				errors.Add("Aufenthaltszonenfläche (AzArea) darf nicht negativ sein: " + azArea);
			}

			if (rzArea < 0) {
				errors.Add("Randzonenfläche (RzArea) darf nicht negativ sein: " + rzArea);
			}

			if (connectingArea < 0) {
				errors.Add("Verbindungsfläche (ConnectingArea) darf nicht negativ sein: " + connectingArea);
			}

			// Prüfung: TotalArea muss konsistent sein
			double calculatedTotal = azArea + rzArea + connectingArea;
			double tolerance = 0.001; // Toleranz für Rundungsfehler
			if (Math.Abs(TotalArea - calculatedTotal) > tolerance) {
				errors.Add("Gesamtfläche (TotalArea) stimmt nicht mit der Summe der Einzelflächen überein. Erwartet: " + calculatedTotal + ", Tatsächlich: " + TotalArea);
			}

			return errors;
		}

	}

}
