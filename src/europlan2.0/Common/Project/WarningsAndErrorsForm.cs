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
			FillListView();
		}

		private void FillListView() {
			lstErrors.Items.Clear();
			string text = "";
			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct plannedProduct in room.PlannedProducts) {
						foreach (string warning in plannedProduct.Product.ErrorMessageArray) {
							text = plannedProduct.InternalName + " in " + room.Id + " (" + room.Name + "): " + warning;
							lstErrors.Items.Add(text);
						}

					}
				}
			}
			foreach (string notification in Project.Instance.NotificationMessageArray) {
				lstErrors.Items.Add(notification);
			}
			foreach (Floor floor in Project.Instance.Floors) {
				foreach (Room room in floor.Rooms) {
					foreach (PlannedProduct plannedProduct in room.PlannedProducts) {
						foreach (string notification in plannedProduct.Product.NotificationMessageArray) {
							text = plannedProduct.InternalName + " in " + room.Id + " (" + room.Name + "): " + notification;
							lstErrors.Items.Add(text);
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