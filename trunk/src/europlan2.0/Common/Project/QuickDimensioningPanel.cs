using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
	public partial class QuickDimensioningPanel : UserControl, IEditorUserControl {

		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private Dictionary<Floor, QuickDimensioningFloorGrid> grids = new Dictionary<Floor, QuickDimensioningFloorGrid>();

		public QuickDimensioningPanel() {
			InitializeComponent();
		}

		public void UpdateControl() {
			//Project.Instance.Config.

			this.grids.Clear();

			List<TabPage> pagesToRemove = new List<TabPage>();
			foreach (TabPage page in this.tabQuickDimensioning.TabPages) {
				if (page != this.pageSettings) {
					pagesToRemove.Add(page);
				}
			}
			foreach (TabPage page in pagesToRemove) {
				this.tabQuickDimensioning.TabPages.Remove(page);
			}

			foreach (Floor floor in Project.Instance.Floors) {
				TabPage page = new TabPage(floor.Name);
				page.UseVisualStyleBackColor = true;
				QuickDimensioningFloorGrid grid = new QuickDimensioningFloorGrid();
				grid.Floor = floor;
				grid.Euroval = this.EurovalHeating;
				page.Controls.Add(grid);
				grid.Dock = DockStyle.Fill;
				this.tabQuickDimensioning.TabPages.Add(page);
				this.grids.Add(floor, grid);
			}
		}

		public bool AllowLeave() {
			return true;
		}

		public bool EurovalHeating {
			get { return this.cbEurovalHeat.Checked; }
			set { this.cbEurovalHeat.Checked = value; }
		}

		public bool HithermHeating {
			get { return this.cbHithermHeat.Checked; }
			set { this.cbHithermHeat.Checked = value; }
		}

		public bool HithermCompactHeating {
			get { return this.cbHithermCompactHeat.Checked; }
			set { this.cbHithermCompactHeat.Checked = value; }
		}

		public bool ModulKlimaBodenHeating {
			get { return this.cbModulKlimaBodenHeat.Checked; }
			set { this.cbModulKlimaBodenHeat.Checked = value; }
		}

		public bool ModulKlimaDeckeHeating {
			get { return this.cbModulKlimaDeckeHeat.Checked; }
			set { this.cbModulKlimaDeckeHeat.Checked = value; }
		}

		public bool ModulKlimaDeckeCooling {
			get { return this.cbModulKlimaDeckeCool.Checked; }
			set { this.cbModulKlimaDeckeCool.Checked = value; }
		}

		private void cbEurovalHeat_CheckedChanged(object sender, EventArgs e) {
			foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
				grid.Euroval = this.EurovalHeating;
			}
		}

		private void cbHithermHeat_CheckedChanged(object sender, EventArgs e) {
			foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
				grid.Hitherm = this.HithermHeating;
			}
		}

		private void cbHithermCompactHeat_CheckedChanged(object sender, EventArgs e) {
			foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
				grid.HithermCompact = this.HithermCompactHeating;
			}
		}

		private void cbModulKlimaBodenHeat_CheckedChanged(object sender, EventArgs e) {
			foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
				grid.ModulKlimaBoden = this.ModulKlimaBodenHeating;
			}
		}

		private void cbModulKlimaDeckeHeat_CheckedChanged(object sender, EventArgs e) {
			foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
				grid.ModulKlimaDecke = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
			}
		}

		private void cbModulKlimaDeckeCool_CheckedChanged(object sender, EventArgs e) {
			foreach (QuickDimensioningFloorGrid grid in this.grids.Values) {
				grid.ModulKlimaDecke = this.ModulKlimaDeckeHeating || this.ModulKlimaDeckeCooling;
				grid.Cooling = this.ModulKlimaDeckeCooling;
			}
		}
	}
}
