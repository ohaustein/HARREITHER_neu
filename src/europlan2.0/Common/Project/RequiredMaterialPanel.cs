using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class RequiredMaterialPanel : UserControl, IEditorUserControl {


		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		
		public RequiredMaterialPanel() {
			InitializeComponent();

			this.SetLanguage();

			UpdateControl(true);
		}

		private void SetLanguage() {
			this.label1.Text = EuroplanRes.RequiredMaterialPanel_Materialbedarf; //"Materialbedarf";
			this.tabFloor.Text = EuroplanRes.RequiredMaterialPanel_Fussboden; //"Fuﬂboden";
			this.tabWall.Text = EuroplanRes.RequiredMaterialPanel_Wand; //"Wand";
			this.tabCeiling.Text = EuroplanRes.RequiredMaterialPanel_Decke; //"Decke";
			this.tabDistributor.Text = EuroplanRes.RequiredMaterialPanel_Verteiler; //"Verteiler";
			this.tabInsulation.Text = EuroplanRes.RequiredMaterialPanel_Daemmung; //"D‰mmung";
			this.tabGeneral.Text = EuroplanRes.RequiredMaterialPanel_Allgemein; //"Allgemein";
		}

		public void UpdateControl(bool resetUserInterface) {
			Project.Instance.CalculateRequiredMaterial();
			requiredMaterialGridFloor.UpdateControl(true);
			requiredMaterialGridWall.UpdateControl(true);
			requiredMaterialGridCeiling.UpdateControl(true);
			requiredMaterialGridDistributor.UpdateControl(true);
			requiredMaterialGridInsulation.UpdateControl(true);
			requiredMaterialGridGeneral.UpdateControl(true);
		}

		public bool AllowLeave() {
			return true;
		}

		private void requiredMaterialGrid_GridContentChanged(object sender) {
			if (this.ProjectChanged != null) {
				this.ProjectChanged(this);
			}
		}

	}
}
