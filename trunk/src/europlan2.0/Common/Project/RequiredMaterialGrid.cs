using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {

	public delegate void RequiredMaterialGridContentChangedHandler(object sender);

	public partial class RequiredMaterialGrid : UserControl {

		public event RequiredMaterialGridContentChangedHandler GridContentChanged;

		private CategoryType type;

		public RequiredMaterialGrid() {
			InitializeComponent();
			UpdateControl();
		}

		private void UpdateControl() {
			List<RequiredMaterialWrapper> wrapperList = new List<RequiredMaterialWrapper>();
			RequiredMaterialWrapper wrapper = null;
			if (Project.Instance != null) {
				foreach (Category category in Project.Instance.Config.Categories) {
					if (category.Type == this.type) {
						foreach (Material material in category.Materials) {
							//TODO: check calculated required amount, ...
							wrapper = new RequiredMaterialWrapper(material);
							wrapperList.Add(wrapper);
						}
					}
				}
			}
			requiredMaterialWrapperBindingSource.DataSource = wrapperList;
			requiredMaterialWrapperBindingSource.ResetBindings(false);
			DataGridViewGrouper grouper = new DataGridViewGrouper(dgvRequiredMaterial);
			grouper.SetGroupOn("Category");
			grouper.ShowCount = false;
		}
		
		public CategoryType CategoryType {
			get { return type; }
			set { 
				type = value;
				UpdateControl();
			}
		}

		private void dgvRequiredMaterial_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (this.GridContentChanged != null) {
				this.GridContentChanged(this);
			}
		}
	}

}
