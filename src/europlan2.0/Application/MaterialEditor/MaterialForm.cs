using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;

namespace Europlan.Application {
	public partial class MaterialForm : Form {
		public MaterialForm() {
			InitializeComponent();
		}

		public Nullable<CategoryType> Filter {
			get { return this.materialEditorGrid1.Filter; }
			set { this.materialEditorGrid1.Filter = value; }
		}
	}
}