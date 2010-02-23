using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Europlan.Common;
using System.Threading;

namespace Europlan.Common {
	public partial class ConstructionEditorForm : Form {
		private System.Resources.ResourceManager resources = EuroplanRes.ResourceManager;

		public ConstructionEditorForm(Construction construction) {
			InitializeComponent();
			this.InitializeForm(construction);
			this.FormClosing += new FormClosingEventHandler(ConstructionEditorForm_FormClosing);
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.ConstructionEditorForm_Titel; //"Konstruktion";
		}

		private void ConstructionEditorForm_FormClosing(object sender, FormClosingEventArgs e) {
			this.constructionEditor.ClearSelection();
		}

		private void InitializeForm(Construction construction) {
			this.constructionEditor.Construction = construction;
			switch (this.constructionEditor.ConstructionScope) {
				case ConstructionScopeEnum.FloorConstruction:
					this.lblType.Text = EuroplanRes.ConstructionEditorForm_Fussboden; //"Fuﬂbodenkonstruktion";
					break;

				case ConstructionScopeEnum.InsulationConstruction:
					this.lblType.Text = EuroplanRes.ConstructionEditorForm_Daemm; //"W‰rmed‰mmkonstruktion";
					break;

				case ConstructionScopeEnum.CeilingConstruction:
					this.lblType.Text = EuroplanRes.ConstructionEditorForm_Decke; //"Deckenkonstruktion";
					break;

				default:
					this.lblType.Text = EuroplanRes.ConstructionEditorForm_Unbekannt; //"Unbekannte Konstruktion";
					break;
			}
		}

		protected override void OnClosing(CancelEventArgs e) {

			base.OnClosing(e);
		}

		private void ConstructionEditorForm_FormClosing_1(object sender, FormClosingEventArgs e) {
			this.constructionEditor.Cleanup();
		}

		public bool ReadOnly {
			set { this.constructionEditor.ReadOnly = value; }
			get { return this.constructionEditor.ReadOnly; }
		}
	}
}