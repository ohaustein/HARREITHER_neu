using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;

namespace Europlan.Common {
	public partial class PicturePanel : UserControl {

		public delegate void LengthChangedEventHandler(object sender);
		public event LengthChangedEventHandler LengthChanged;

		private ImagePlan plan;
		private Image image = null;
		private bool moveMode = true;
		private bool roomPickerMode = false;
		private bool showRaster = false;
		private bool unsavedChanges = false;
		private bool unsavedRoomPickerChanges = false;
		Nullable<PointF> startPoint = null;
		Nullable<PointF> endPoint = null;
		private double length = 0;
		private float mouseDownX, mouseUpX, mouseDownY, mouseUpY;
		private List<PointF> roomCoordinates = new List<PointF>();
		private List<PointF> tempCoordinates = new List<PointF>();
		private bool inDesign = false;
		private bool shiftPressed = false;

		private float angle = 0;
		private float xPos = 0;
		private float yPos = 0;
		private Nullable<float> scale = null;

		public PicturePanel() {
			InitializeComponent();
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			this.MouseWheel += new MouseEventHandler(PicturePanel_MouseWheel);
		}

		public ImagePlan Plan {
			get { return plan; }
			set {
				if (value != null) {
					this.plan = value;
					if (this.plan.AbsoluteFileName != "") {
						image = Image.FromFile(this.plan.AbsoluteFileName);
					}
					this.angle = plan.Angle;
					this.xPos = plan.XPos;
					this.yPos = plan.YPos;
					this.scale = plan.Scale;
				}
			}
		}

		public bool MoveMode {
			get { return this.moveMode; }
			set { this.moveMode = value; }
		}

		public bool RoomPickerMode {
			get { return this.roomPickerMode; }
			set { this.roomPickerMode = value; }
		}

		public bool ShowRaster {
			get { return this.showRaster; }
			set { this.showRaster = value; }
		}

		public bool UnsavedChanges {
			get { return this.unsavedChanges; }
		}

		public bool UnsavedRoomPickerChanges {
			get { return this.unsavedRoomPickerChanges; }
		}

		public Nullable<PointF> StartPoint {
			get { return this.startPoint; }
			set { this.startPoint = value; }
		}

		public Nullable<PointF> EndPoint {
			get { return this.endPoint; }
			set { this.endPoint = value; }
		}

		public double Length {
			get { return this.length; }
			set { this.length = value; }
		}

		public float Angle {
			get { return angle; }
			set { 
				angle = value;
				this.Invalidate();
			}
		}

		public float XPos {
			get { return xPos; }
			set { 
				xPos = value;
				this.Invalidate();
			}
		}

		public float YPos {
			get { return yPos; }
			set { 
				yPos = value;
				this.Invalidate();
			}
		}

		public Nullable<float> Scale {
			get { return scale; }
			set { 
				scale = value;
				this.Invalidate();
			}
		}

		public List<PointF> RoomCoordinates {
			get { return this.roomCoordinates; }
			set { this.roomCoordinates = value; }
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			shiftPressed = e.Shift;
			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e) {
			shiftPressed = false;
			base.OnKeyUp(e);
		}

		private void PicturePanel_MouseClick(object sender, MouseEventArgs e) {
			Point mousePos = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
			PointF[] arr = new PointF[] { mousePos };

			Matrix X = new Matrix();
			X.Translate(((float)image.Width / 2 + this.XPos) * this.Scale.Value, ((float)image.Height / 2 + this.YPos) * this.Scale.Value);
			X.Rotate(this.Angle);
			X.Translate(-((float)image.Width / 2 + this.XPos) * this.Scale.Value, -((float)image.Height / 2 + this.YPos) * this.Scale.Value);
			X.Scale(this.Scale.Value, this.Scale.Value);
			X.Translate(this.XPos, this.YPos);
			X.Invert();
			X.TransformPoints(arr);
	
			if (RoomPickerMode) {
				PointF pos = arr[0];
				if (e.Button == MouseButtons.Left) {
					if (!shiftPressed && tempCoordinates.Count > 0) {
						pos = GetNormalizedPoint(tempCoordinates[tempCoordinates.Count - 1], pos);
					}
					if (!inDesign && roomCoordinates.Count > 0) {
						// TODO
						DialogResult result = MessageBox.Show("Wollen Sie die bereits definierte Raumgeometrie verwerfen und neu definieren?", "Verwerfen und neu definieren?", MessageBoxButtons.YesNo);
						if (result == DialogResult.No) {
							return;
						}
					}
					unsavedRoomPickerChanges = true;
					tempCoordinates.Add(pos);
					inDesign = true;
					roomCoordinates.Clear();
				} else if (e.Button == MouseButtons.Right) {
					if (!shiftPressed && tempCoordinates.Count > 0) {
						pos = GetNormalizedPoint(tempCoordinates[tempCoordinates.Count - 1], pos);
					}
					tempCoordinates.Add(pos);
					if (tempCoordinates.Count > 2) {
						// TODO
						DialogResult result = MessageBox.Show("Die definierte Fläche beträgt " + Math.Round(Europlan.Common.Plan.PolygonArea(tempCoordinates.ToArray()) / Math.Pow(this.plan.Measure.Value, 2.0), 2) + "m². Kleine Ungenauigkeiten in der Flächenberechnung können nachträglich manuell geändert werden. Wollen Sie diese Raumgeometrie übernehmen?", "Raumgeometrie übernehmen?", MessageBoxButtons.YesNo);
						if (result.Equals(DialogResult.Yes)) {
							roomCoordinates.AddRange(tempCoordinates);
							unsavedRoomPickerChanges = true;
						}
					}
					tempCoordinates.Clear();
					inDesign = false;
					//shiftPressed = false;
				}
				this.Invalidate();
			} else if (!moveMode && e.Button == MouseButtons.Left) {

				if (startPoint.HasValue && !endPoint.HasValue) {
					endPoint = arr[0];
					length = this.distance(startPoint.Value.X, startPoint.Value.Y, arr[0].X, arr[0].Y);
					//txtLength.Enabled = true;
					if (LengthChanged != null) {
						LengthChanged(this);
					}
					//startPoint = null;
				} else {
					startPoint = arr[0];
					endPoint = null;
				}
			}
		}

		void PicturePanel_MouseWheel(object sender, MouseEventArgs e) {
			unsavedChanges = true;
			Point center = this.PointToClient(this.PointToScreen(e.Location));
			AddScale(1.0f + ((float)e.Delta) / 1200.0f, center);
			this.Invalidate();
		}

		private void PicturePanel_Paint(object sender, PaintEventArgs e) {
			Point mousePos = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
			PointF[] arr = new PointF[] { mousePos };

			Graphics g = e.Graphics;
			g.FillRectangle(Brushes.White, 0, 0, this.Width, this.Height);
			if (image != null) {
				Matrix X = new Matrix();
				if (!this.Scale.HasValue) {
					float scaleX = (float)this.Width / (float)image.Width;
					float scaleY = (float)this.Height / (float)image.Height;
					float scale = Math.Min(scaleX, scaleY);
					scale = 1;
					this.Scale = scale;
				}
				X.Translate(((float)image.Width / 2 + this.XPos) * this.Scale.Value, ((float)image.Height / 2 + this.YPos) * this.Scale.Value);
				X.Rotate(this.Angle);
				X.Translate(-((float)image.Width / 2 + this.XPos) * this.Scale.Value, -((float)image.Height / 2 + this.YPos) * this.Scale.Value);
				X.Scale(this.Scale.Value, this.Scale.Value);
				X.Translate(this.XPos, this.YPos);
				g.Transform = X;

				g.DrawImage(image, 0, 0, image.Width, image.Height);
				
				Matrix m = new Matrix();
				m.Translate(((float)image.Width / 2 + this.XPos) * this.Scale.Value, ((float)image.Height / 2 + this.YPos) * this.Scale.Value);
				m.Rotate(this.Angle);
				m.Translate(-((float)image.Width / 2 + this.XPos) * this.Scale.Value, -((float)image.Height / 2 + this.YPos) * this.Scale.Value);
				m.Scale(this.Scale.Value, this.Scale.Value);
				m.Translate(this.XPos, this.YPos);
				m.Invert();
				m.TransformPoints(arr);

				g.SmoothingMode = SmoothingMode.AntiAlias;

				if (roomCoordinates.Count > 2) {
					GraphicsPath path = new GraphicsPath();
					path.StartFigure();
					PointF[] array = roomCoordinates.ToArray();
					path.AddPolygon(array);
					path.CloseFigure();
					Color c = Color.FromArgb(128, Color.Red);
					Brush b = new SolidBrush(c);
					g.FillPath(b, path);
					g.DrawPath(new Pen(b), path);
				}

				if (tempCoordinates.Count > 0 && inDesign) {
					List<PointF> points = new List<PointF>(tempCoordinates);
					PointF pos = arr[0];
					if (!shiftPressed) {
						pos = GetNormalizedPoint(points[points.Count - 1], pos);
					}
					points.Add(pos);
					//g.DrawPolygon(Pens.Black, points.ToArray());

					GraphicsPath path = new GraphicsPath();
					path.StartFigure();
					PointF[] array = points.ToArray();
					if (array.Length > 2) {
						path.AddPolygon(array);
					} else {
						path.AddLine(array[0], array[1]);
					}
					path.CloseFigure();
					Color c = Color.FromArgb(128, Color.Red);
					Brush b = new SolidBrush(c);
					g.FillPath(b, path);
					g.DrawPath(new Pen(b), path);
				}

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
					for (int i = 0; i < this.Height; i = i + 100) {
						g.DrawLine(pen, 0, i, this.Width, i);
					}
					for (int i = 0; i < this.Width; i = i + 100) {
						g.DrawLine(pen, i, 0, i, this.Height);
					}
					pen.Dispose();
				}
			}
		}

		public void AddScale(double addedScale, Nullable<PointF> center) {
			if (this.Scale.Value * addedScale < 0.01) {
				addedScale = 0.01 / this.Scale.Value;
			}
			if (this.Scale.Value * addedScale > 10000.0) {
				addedScale = 10000.0 / this.Scale.Value;
			}
			double oldScale = this.Scale.Value;
			double newScale = oldScale * addedScale;
			this.Scale = (float)newScale;
			double centerX = center.HasValue ? center.Value.X : this.ClientSize.Width / 2.0;
			double centerY = center.HasValue ? center.Value.Y : this.ClientSize.Height / 2.0;
			this.XPos = (float)((centerX - (centerX - this.XPos * oldScale) * addedScale) / newScale);
			this.YPos = (float)((centerY - (centerY - this.YPos * oldScale) * addedScale) / newScale);
		}

		private double distance(double x1, double y1, double x2, double y2) {
			double result = 0;
			double part1 = Math.Pow((x2 - x1), 2);
			double part2 = Math.Pow((y2 - y1), 2);
			double underRadical = part1 + part2;
			result = Math.Sqrt(underRadical);
			return result;
		}

		private void PicturePanel_MouseDown(object sender, MouseEventArgs e) {
			if ((moveMode && e.Button == MouseButtons.Left) || ((!moveMode || roomPickerMode) && e.Button == MouseButtons.Middle)) {
				Point mousePos = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
				PointF[] arr = new PointF[] { mousePos };

				Matrix X = new Matrix();
				X.Scale(this.Scale.Value, this.Scale.Value);
				X.Translate(this.XPos, this.YPos);
				X.Invert();
				X.TransformPoints(arr);

				mouseDownX = arr[0].X;
				mouseDownY = arr[0].Y;
			}
			if ((!moveMode || roomPickerMode) && e.Button == MouseButtons.Middle) {
				this.Cursor = Cursors.Hand;
			}
		}

		private void PicturePanel_MouseUp(object sender, MouseEventArgs e) {
			if ((!moveMode || roomPickerMode) && e.Button == MouseButtons.Middle) {
				this.Cursor = Cursors.Cross;
			}
		}

		private void PicturePanel_MouseMove(object sender, MouseEventArgs e) {
			if ((moveMode && e.Button == MouseButtons.Left) || ((!moveMode || roomPickerMode) && e.Button == MouseButtons.Middle)) {
				unsavedChanges = true;
				Point mousePos = this.PointToClient(new Point(MousePosition.X, MousePosition.Y));
				PointF[] arr = new PointF[] { mousePos };

				Matrix X = new Matrix();
				X.Scale(this.Scale.Value, this.Scale.Value);
				X.Translate(this.XPos, this.YPos);
				X.Invert();
				X.TransformPoints(arr);

				mouseUpX = arr[0].X;
				mouseUpY = arr[0].Y;

				this.XPos += mouseUpX - mouseDownX;
				this.YPos += mouseUpY - mouseDownY;
			} else if (!moveMode && startPoint.HasValue && !endPoint.HasValue) {
				
			}
			this.Invalidate();
		}

		private void PicturePanel_Resize(object sender, EventArgs e) {
			this.Invalidate();
		}


		internal void ApplyChangesToPlan() {
			this.plan.Scale = this.Scale;
			this.plan.XPos = this.XPos;
			this.plan.YPos = this.YPos;
			this.plan.Angle = this.Angle;
		}

		private PointF GetNormalizedPoint(PointF basePoint, PointF currentPoint) {
			float xDistance = Math.Abs(basePoint.X - currentPoint.X);
			float yDistance = Math.Abs(basePoint.Y - currentPoint.Y);
			PointF p;
			if (xDistance < yDistance) {
				double distanceInMeter = (currentPoint.Y - basePoint.Y) / this.Plan.Measure.Value;
				distanceInMeter = Math.Round(distanceInMeter, 1);
				p = new PointF(basePoint.X, basePoint.Y + ((float)distanceInMeter * this.Plan.Measure.Value));
			} else {
				double distanceInMeter = (currentPoint.X - basePoint.X) / this.Plan.Measure.Value;
				distanceInMeter = Math.Round(distanceInMeter, 1);
				p = new PointF(basePoint.X + ((float)distanceInMeter * this.Plan.Measure.Value), basePoint.Y);
			}

			return p;
		}
	}

}
