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
		private bool unsavedChanges = false;
		private bool showRaster = false;
		private bool moveMode = true;
		Nullable<Point> startPoint = null;
		Nullable<Point> endPoint = null;
		private double length = 0;

		public ImagePlanOptionsForm(ImagePlan plan) {
			InitializeComponent();
			this.SetLanguage();
			this.plan = plan;
			this.MouseWheel += new MouseEventHandler(ImagePlanOptionsForm_MouseWheel);
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.ImagePlanOptionsForm_Titel; //"Optionen";
			this.lblLength.Text = EuroplanRes.PlanOptionsForm_Leange; //"Länge:"
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
			Point mousePos = picturePanel.PointToClient(new Point(MousePosition.X, MousePosition.Y));
			Point[] arr = new Point[] { mousePos };

			Graphics g = e.Graphics;
			g.FillRectangle(Brushes.White, 0, 0, picturePanel.Width, picturePanel.Height);
			if (image != null) {
				Matrix X = new Matrix();
				if (!plan.Scale.HasValue) {
					float scaleX = (float)picturePanel.Width / (float)image.Width;
					float scaleY = (float)picturePanel.Height / (float)image.Height;
					float scale = Math.Min(scaleX, scaleY);
					scale = 1;
					plan.Scale = scale;
				}
				X.Translate(((float)image.Width / 2 + plan.XPos) * plan.Scale.Value, ((float)image.Height / 2 + plan.YPos) * plan.Scale.Value);
				X.Rotate(plan.Angle);
				X.Translate(-((float)image.Width / 2 + plan.XPos) * plan.Scale.Value, -((float)image.Height / 2 + plan.YPos) * plan.Scale.Value);
				X.Scale(plan.Scale.Value, plan.Scale.Value);
				X.Translate(plan.XPos, plan.YPos);
				g.Transform = X;

				g.DrawImage(image, 0, 0, image.Width, image.Height);

				Matrix m = new Matrix();
				m.Translate(((float)image.Width / 2 + plan.XPos) * plan.Scale.Value, ((float)image.Height / 2 + plan.YPos) * plan.Scale.Value);
				m.Rotate(plan.Angle);
				m.Translate(-((float)image.Width / 2 + plan.XPos) * plan.Scale.Value, -((float)image.Height / 2 + plan.YPos) * plan.Scale.Value);
				m.Scale(plan.Scale.Value, plan.Scale.Value);
				m.Translate(plan.XPos, plan.YPos);
				m.Invert();
				m.TransformPoints(arr);

				if (!moveMode && startPoint.HasValue) {
					if (endPoint.HasValue) {
						g.DrawLine(Pens.Red, startPoint.Value, endPoint.Value);
					} else {
						g.DrawLine(Pens.Red, startPoint.Value, arr[0]);
					}
				}

				X = new Matrix();
				g.Transform = X;
			
				if (showRaster) {
					Pen pen = Pens.DarkGray.Clone() as Pen;
					//pen.DashStyle = DashStyle.Dash;
					for (int i = 0; i < picturePanel.Height; i = i + 100) {
						g.DrawLine(pen, 0, i, picturePanel.Width, i);
					}
					for (int i = 0; i < picturePanel.Width; i = i + 100) {
						g.DrawLine(pen, i, 0, i, picturePanel.Height);
					}
					pen.Dispose();
				}
			}
		}

		private void btnRotateLeft_Click(object sender, EventArgs e) {
			unsavedChanges = true;
			plan.Angle -= 5;
			picturePanel.Invalidate();
		}
		
		private void btnRotateLeftSmall_Click(object sender, EventArgs e) {
			unsavedChanges = true;
			plan.Angle -= 0.1f;
			picturePanel.Invalidate();
		}

		private void btnRotateRight_Click(object sender, EventArgs e) {
			unsavedChanges = true;
			plan.Angle += 5;
			picturePanel.Invalidate();
		}

		private void btnRotateRightSmall_Click(object sender, EventArgs e) {
			unsavedChanges = true;
			plan.Angle += 0.1f;
			picturePanel.Invalidate();
		}

		void ImagePlanOptionsForm_MouseWheel(object sender, MouseEventArgs e) {
			unsavedChanges = true;
			Point center = picturePanel.PointToClient(this.PointToScreen(e.Location));
			AddScale(1.0f + ((float)e.Delta) / 1200.0f, center);
			picturePanel.Invalidate();
		}

		private void AddScale(double addedScale, Nullable<PointF> center) {
			if (plan.Scale.Value * addedScale < 0.01) {
				addedScale = 0.01 / plan.Scale.Value;
			}
			if (plan.Scale.Value * addedScale > 10000.0) {
				addedScale = 10000.0 / plan.Scale.Value;
			}
			double oldScale = plan.Scale.Value;
			double newScale = oldScale * addedScale;
			plan.Scale = (float) newScale;
			double centerX = center.HasValue ? center.Value.X : picturePanel.ClientSize.Width / 2.0;
			double centerY = center.HasValue ? center.Value.Y : picturePanel.ClientSize.Height / 2.0;
			plan.XPos = (float)((centerX - (centerX - plan.XPos * oldScale) * addedScale) / newScale);
			plan.YPos = (float)((centerY - (centerY - plan.YPos * oldScale) * addedScale) / newScale);
		}

		private void picturePanel_MouseDown(object sender, MouseEventArgs e) {
			if ((moveMode && e.Button == MouseButtons.Left) || (!moveMode && e.Button == MouseButtons.Middle)) {
				Point mousePos = picturePanel.PointToClient(new Point(MousePosition.X, MousePosition.Y));
				Point[] arr = new Point[] { mousePos };

				Matrix X = new Matrix();
				X.Scale(plan.Scale.Value, plan.Scale.Value);
				X.Translate(plan.XPos, plan.YPos);
				X.Invert();
				X.TransformPoints(arr);

				mouseDownX = arr[0].X;
				mouseDownY = arr[0].Y;
			}
			if (!moveMode && e.Button == MouseButtons.Middle) {
				picturePanel.Cursor = Cursors.Hand;
			}
		}

		private void picturePanel_MouseUp(object sender, MouseEventArgs e) {
			if (!moveMode && e.Button == MouseButtons.Middle) {
				picturePanel.Cursor = Cursors.Cross;
			}
		}

		private void picturePanel_MouseMove(object sender, MouseEventArgs e) {
			if ((moveMode && e.Button == MouseButtons.Left) || (!moveMode && e.Button == MouseButtons.Middle)) {
				unsavedChanges = true;
				Point mousePos = picturePanel.PointToClient(new Point(MousePosition.X, MousePosition.Y));
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
			} else if (!moveMode && startPoint.HasValue && !endPoint.HasValue) {
				picturePanel.Invalidate();
			}
		}
		
		public bool UnsavedChanges {
			get { return unsavedChanges; }
		}

		private void btnRaster_Click(object sender, EventArgs e) {
			showRaster = !showRaster;
			btnRaster.Checked = showRaster;
			if (showRaster) {
				btnRaster.Text = EuroplanRes.ImagePlanOptionsForm_RasterAus;
			} else {
				btnRaster.Text = EuroplanRes.ImagePlanOptionsForm_RasterEin;
			}
			picturePanel.Invalidate();
		}

		private void picturePanel_Resize(object sender, EventArgs e) {
			picturePanel.Invalidate();
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			unsavedChanges = true;
			//plan.Scale *= 1.1f;
			AddScale(1.1, null);
			picturePanel.Invalidate();
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			unsavedChanges = true;
			//plan.Scale *= 0.9f;
			AddScale(0.9, null);
			picturePanel.Invalidate();
		}

		private double distance(int x1, int y1, int x2, int y2) {
			double result = 0;
			double part1 = Math.Pow((x2 - x1), 2);
			double part2 = Math.Pow((y2 - y1), 2);
			double underRadical = part1 + part2;
			result = Math.Sqrt(underRadical);
			return result;
		}

		private void btnMove_Click(object sender, EventArgs e) {
			picturePanel.Cursor = Cursors.Hand;
			btnMove.Checked = true;
			btnDistance.Checked = false;
			txtLength.Visible = false;
			lblLength.Visible = false;
			btnSetLength.Visible = false;
			moveMode = true;
			startPoint = null;
			endPoint = null;
			picturePanel.Invalidate();
		}

		private void btnDistance_Click(object sender, EventArgs e) {
			picturePanel.Cursor = Cursors.Cross;
			btnMove.Checked = false;
			btnDistance.Checked = true;
			//txtLength.Visible = true;
			//lblLength.Visible = true;
		    //txtLength.Enabled = plan.Measure.HasValue;
			//txtLength.Text = "";
			moveMode = false;
		}

		private void picturePanel_MouseClick(object sender, MouseEventArgs e) {
			if (!moveMode && e.Button == MouseButtons.Left) {
				Point mousePos = picturePanel.PointToClient(new Point(MousePosition.X, MousePosition.Y));
				Point[] arr = new Point[] { mousePos };

				Matrix X = new Matrix();
				X.Translate(((float)image.Width / 2 + plan.XPos) * plan.Scale.Value, ((float)image.Height / 2 + plan.YPos) * plan.Scale.Value);
				X.Rotate(plan.Angle);
				X.Translate(-((float)image.Width / 2 + plan.XPos) * plan.Scale.Value, -((float)image.Height / 2 + plan.YPos) * plan.Scale.Value);
				X.Scale(plan.Scale.Value, plan.Scale.Value);
				X.Translate(plan.XPos, plan.YPos);
				X.Invert();
				X.TransformPoints(arr);
				if (startPoint.HasValue && !endPoint.HasValue) {
					endPoint = arr[0];
					length = this.distance(startPoint.Value.X, startPoint.Value.Y, arr[0].X, arr[0].Y);
					//txtLength.Enabled = true;
					if (plan.Measure.HasValue) {
						txtLength.Text = "" + (length / plan.Measure.Value).ToString("0.00") + EuroplanRes.Unit_Meter;
					} else {
						txtLength.Text = "???";
					}
					lblLength.Visible = true;
					txtLength.Visible = true;
					btnSetLength.Visible = true;
					//startPoint = null;
				} else {
					startPoint = arr[0];
					endPoint = null;
				}
			}
		}

		private void txtLength_TextChanged(object sender, EventArgs e) {
			double len = 0;
			if (Double.TryParse(txtLength.Text, out len)) {
				unsavedChanges = true;
				plan.Measure = (float)(length / len);
			}
		}

		private void btnSetLength_Click(object sender, EventArgs e) {
			Nullable<double> length = plan.Measure.HasValue ? this.length / this.plan.Measure.Value : (Nullable<double>)null;
			PlanSetMeasureForm psmf = new PlanSetMeasureForm(length);
			if (psmf.ShowDialog() == DialogResult.OK) {
				if (length != psmf.Length) {
					this.unsavedChanges = true;
					plan.Measure = (float)(this.length / psmf.Length);
					this.txtLength.Text = (this.length / this.plan.Measure.Value).ToString("0.00") + EuroplanRes.Unit_Meter;
				}
			}
		}

	}
}