using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;

namespace Europlan.AdminApplication.ContructionEditor {
	public partial class MaterialEditorGrid : UserControl {

		private MaterialTypeEnum filter = MaterialTypeEnum.All;
		private MaterialListWrapper wrapper;
		
		public MaterialEditorGrid() {
			InitializeComponent();
			this.wrapper = new MaterialListWrapper();
			this.materialsWrapperBindingSource.DataSource = this.wrapper;
			this.materialsWrapperBindingSource.ResetBindings(false);
		}
	}
}
