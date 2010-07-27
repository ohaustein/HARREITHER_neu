using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.Drawing.Drawing2D;

namespace Europlan.Common {

	public partial class ImagePlanOptionsForm : Form {

		private ImagePlan plan;
		private Image image = null;

		public ImagePlanOptionsForm(ImagePlan plan) {
			InitializeComponent();
			this.SetLanguage();
			this.plan = plan;
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.ImagePlanOptionsForm_Titel; //"Raumtypen";
		}



		private void ImagePlanOptionsForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ImagePlanOptionsForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();

		}

		private void ImagePlanOptionsForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ImagePlanOptionsForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);

			if (plan.AbsoluteFileName != "") {
				image = Image.FromFile(plan.AbsoluteFileName);
			}
		}

		private void picturePanel_Paint(object sender, PaintEventArgs e) {
			Graphics g = e.Graphics;
			if (image != null) {
				Matrix X = new Matrix();
				if (plan.Scale.HasValue) {
					X.Scale(plan.Scale.Value, plan.Scale.Value);
				} else {
					float scaleX = (float)picturePanel.Width / (float)image.Width;
					float scaleY = (float)picturePanel.Height / (float)image.Height;
					float scale = Math.Min(scaleX, scaleY);
					X.Scale(scale, scale);
					plan.Scale = scale;
				}
				g.Transform = X;
				g.DrawImage(image, 0, 0, image.Width, image.Height);
			}
		}

	}
}