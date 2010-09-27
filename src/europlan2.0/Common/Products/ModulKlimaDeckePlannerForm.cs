using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common.Products {
	public partial class ModulKlimaDeckePlannerForm : Form {

		public ModulKlimaDeckePlannerForm(ModulKlimaDeckeProduct product) {
			InitializeComponent();
			if (product.AssociatedRoom.AssociatedPlan is CadPlan) {
				CadPanel panel = new CadPanel();
				panel.Dock = DockStyle.Fill;
				this.Controls.Add(panel);
				panel.ProductPlanner = this.roomPicker1;
				panel.Mode = PlanMode.PM_PLANNER_CLICK;
			} else if (product.AssociatedRoom.AssociatedPlan is ImagePlan) {
				ImagePanel panel = new ImagePanel();
				panel.Dock = DockStyle.Fill;
				this.Controls.Add(panel);
				panel.ProductPlanner = this.roomPicker1;
				panel.Mode = PlanMode.PM_PLANNER_CLICK;
			}
			this.roomPicker1.Room = product.AssociatedRoom;
			this.modulKlimaBodenPlanner1.Product = product;
		}
	}
}