using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class RequiredMaterialPanel : UserControl, IEditorUserControl {
        private event ProjectStructureChangedHandler projectStructureChanged;
        public event ProjectStructureChangedHandler ProjectStructureChanged {
            add { this.projectStructureChanged += value; }
            remove { this.projectStructureChanged -= value; }
        }
        private event ProjectChangedHandler projectChanged;
        public event ProjectChangedHandler ProjectChanged {
            add { this.projectChanged += value; }
            remove { this.projectChanged -= value; }
        }
        private event TreeSelectionRequestedHandler treeSelectionRequested;
        public event TreeSelectionRequestedHandler TreeSelectionRequested {
            add { this.treeSelectionRequested += value; }
            remove { this.treeSelectionRequested -= value; }
        }

		
		public RequiredMaterialPanel() {
			InitializeComponent();

			// visual studio always deletes these lines of code from the designer.cs, so just do it here...
			this.requiredMaterialGridFloor.CategoryType = Europlan.Common.CategoryType.Floor;
			this.requiredMaterialGridWall.CategoryType = Europlan.Common.CategoryType.Wall;
			this.requiredMaterialGridCeiling.CategoryType = Europlan.Common.CategoryType.Ceiling;
			this.requiredMaterialGridDistributor.CategoryType = Europlan.Common.CategoryType.Distributor;
			this.requiredMaterialGridInsulation.CategoryType = Europlan.Common.CategoryType.Insulation;
			this.requiredMaterialGridGeneral.CategoryType = Europlan.Common.CategoryType.General;

			this.SetLanguage();

			UpdateControl(true);
		}

		private void SetLanguage() {
			this.label1.Text = EuroplanRes.RequiredMaterialPanel_Materialbedarf; //"Materialbedarf"
			this.tabFloor.Text = EuroplanRes.RequiredMaterialPanel_Fussboden; //"Fuﬂboden"
			this.tabWall.Text = EuroplanRes.RequiredMaterialPanel_Wand; //"Wand"
			this.tabCeiling.Text = EuroplanRes.RequiredMaterialPanel_Decke; //"Decke"
			this.tabDistributor.Text = EuroplanRes.RequiredMaterialPanel_Verteiler; //"Verteiler"
			this.tabInsulation.Text = EuroplanRes.RequiredMaterialPanel_Daemmung; //"D‰mmung"
			this.tabGeneral.Text = EuroplanRes.RequiredMaterialPanel_Allgemein; //"Allgemein
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
			if (this.projectChanged != null) {
				this.projectChanged(this);
			}
		}

	}
}
