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
		private float mouseDownX, mouseUpX, mouseDownY, mouseUpY;

		public ImagePlanOptionsForm(ImagePlan plan) {
			InitializeComponent();
			this.SetLanguage();
			this.plan = plan;
			this.MouseWheel += new MouseEventHandler(ImagePlanOptionsForm_MouseWheel);
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
				if (!plan.Scale.HasValue) {
					float scaleX = (float)picturePanel.Width / (float)image.Width;
					float scaleY = (float)picturePanel.Height / (float)image.Height;
					float scale = Math.Min(scaleX, scaleY);
					plan.Scale = scale;
				}
				X.Translate(((float)image.Width / 2 + plan.XPos) * plan.Scale.Value, ((float)image.Height / 2 + plan.YPos) * plan.Scale.Value);
				X.Rotate(plan.Angle);
				X.Translate(-((float)image.Width / 2 + plan.XPos) * plan.Scale.Value, -((float)image.Height / 2 + plan.YPos) * plan.Scale.Value);
				X.Scale(plan.Scale.Value, plan.Scale.Value);
				X.Translate(plan.XPos, plan.YPos);
				g.Transform = X;

				g.DrawImage(image, 0, 0, image.Width, image.Height);
			}
		}

		private void btnRotateLeft_Click(object sender, EventArgs e) {
			plan.Angle -= 10;
			picturePanel.Invalidate();
		}

		private void btnRotateRight_Click(object sender, EventArgs e) {
			plan.Angle += 10;
			picturePanel.Invalidate();
		}

		void ImagePlanOptionsForm_MouseWheel(object sender, MouseEventArgs e) {
			plan.Scale += ((float)e.Delta) / 1200;
			plan.Scale = plan.Scale < 0.1f ? 0.1f : plan.Scale;
			picturePanel.Invalidate();
		}

		private void picturePanel_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				Point mousePos = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
				Point[] arr = new Point[] { mousePos };

				Matrix X = new Matrix();
				X.Scale(plan.Scale.Value, plan.Scale.Value);
				X.Translate(plan.XPos, plan.YPos);
				X.Invert();
				X.TransformPoints(arr);

				mouseDownX = arr[0].X;
				mouseDownY = arr[0].Y;
			}
		}

		private void picturePanel_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				Point mousePos = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
				Point[] arr = new Point[] { mousePos };

				Matrix X = new Matrix();
				X.Scale(plan.Scale.Value, plan.Scale.Value);
				X.Translate(plan.XPos, plan.YPos);
				X.Invert();
				X.TransformPoints(arr);

				mouseUpX = arr[0].X;
				mouseUpY = arr[0].Y;

				plan.XPos += mouseUpX - mouseDownX;
				plan.YPos += mouseUpY - mouseDownY;
				picturePanel.Invalidate();
			}
		}
		
	}
}