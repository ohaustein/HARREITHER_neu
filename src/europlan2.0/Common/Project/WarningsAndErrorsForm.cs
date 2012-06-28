using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class WarningsAndErrorsForm : Form {

		public WarningsAndErrorsForm() {
			InitializeComponent();

			this.SetLanguage();

			FillListView();
		}

		private void SetLanguage() {
			this.btnClose.Text = EuroplanRes.General_Schliessen; //"&Schlieﬂen";
			this.Text = EuroplanRes.WarningsAndErrorsForm_Titel; //"Warnungen und Fehler";
		}

		private void FillListView() {
			lstErrors.Items.Clear();
			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct plannedProduct in room.PlannedProducts) {
						foreach (string warning in plannedProduct.Product.ErrorMessageArray) {
							string message = EuroplanRes.WarningsAndErrorsForm_Warnung;
							message = message.Replace("%SYSTEM%", plannedProduct.InternalName);
							message = message.Replace("%RAUMID%", room.Id);
							message = message.Replace("%RAUMNAME%", room.Name);
							message = message.Replace("%WARNUNG%", warning);
							lstErrors.Items.Add(message, "error.png");
						}

					}
				}
			}
			foreach (string notification in Project.Instance.NotificationMessageArray) {
				lstErrors.Items.Add(notification, "warning.png");
			}
			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Distributor dist in floor.Distributors) {
					foreach (string error in dist.ErrorMessageArray) {
						string message = EuroplanRes.WarningsAndErrorsForm_VerteilerWarnung;
						message = message.Replace("%VERTEILERID%", dist.Id);
						message = message.Replace("%VERTEILERNAME%", dist.Name);
						message = message.Replace("%GESCHOSS%", floor.Name);
						message = message.Replace("%WARNUNG%", error);
						lstErrors.Items.Add(message, "warning.png");
						//lstErrors.Items.Add(error);
					}
				}
			}
			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct plannedProduct in room.PlannedProducts) {
						foreach (string notification in plannedProduct.Product.NotificationMessageArray) {
							string message = EuroplanRes.WarningsAndErrorsForm_Warnung;
							message = message.Replace("%SYSTEM%", plannedProduct.InternalName);
							message = message.Replace("%RAUMID%", room.Id);
							message = message.Replace("%RAUMNAME%", room.Name);
							message = message.Replace("%HINWEIS%", notification);
							lstErrors.Items.Add(message, "warning.png");
						}
					}
				}
			}
			lstErrors.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void WarningsAndErrorsForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["WarningsAndErrorsForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void WarningsAndErrorsForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["WarningsAndErrorsForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

	}
}