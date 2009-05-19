using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;

namespace Europlan.AdminApplication {
	public partial class ConstructionEditorForm : Form {
		public ConstructionEditorForm(Construction construction) {
			InitializeComponent();
			this.InitializeForm(construction);
			this.FormClosing += new FormClosingEventHandler(ConstructionEditorForm_FormClosing);
		}

		private void ConstructionEditorForm_FormClosing(object sender, FormClosingEventArgs e) {
			this.constructionEditor.ClearSelection();
		}

		private void InitializeForm(Construction construction) {
			this.constructionEditor.Construction = construction;
			switch (this.constructionEditor.ConstructionScope) {
				case ConstructionScopeEnum.FloorConstruction:
					this.lblType.Text = "Fuﬂbodenkonstruktion";
					break;

				case ConstructionScopeEnum.InsulationConstruction:
					this.lblType.Text = "W‰rmed‰mmkonstruktion";
					break;

				case ConstructionScopeEnum.CeilingConstruction:
					this.lblType.Text = "Deckenkonstruktion";
					break;

				default:
					this.lblType.Text = "Unbekannte Konstruktion";
					break;
			}
		}

		protected override void OnClosing(CancelEventArgs e) {

			base.OnClosing(e);
		}
	}
}