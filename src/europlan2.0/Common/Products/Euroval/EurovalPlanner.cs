using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using WW.Math;
using WW.Math.Geometry;
using System.Xml.Serialization;
using WW.Cad.Model;
using WW.Cad.Model.Tables;
using WW.Cad.Model.Entities;

namespace Europlan.Common {
	public partial class EurovalPlanner : Component, IProductPlanner {

		public enum EurovalMode {
			EVM_NONE,
			EVM_ADD_AREA,
			EVM_ADD_RZ,
			EVM_DEL_RZ,
			EVM_ADD_RED,
			EVM_DEL_RED,
			EVM_SET_TEXT
		}

		public EurovalPlanner() {
			InitializeComponent();
		}

		public EurovalPlanner(IContainer container) {
			container.Add(this);
			InitializeComponent();
		}

		private bool unsavedChanges = false;
		private bool inDesign = false;
		private EurovalProduct product;
		private EurovalMode mode = EurovalMode.EVM_NONE;
		private Cursor customCursor = null;
		private bool highlightRoomCoordinates = true;
		private bool drawExpansionGaps = true;
		private Point2D rzStart = Point2D.Zero;
		private List<Point2D> coordsPickedSoFar = new List<Point2D>();

		public event EventHandler<EventArgs> ModeChanged;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public EurovalProduct Product {
			get { return this.product; }
			set {
				this.product = value;
				if (this.ConnectedPlanPanel != null) {
					if (this.product == null || this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedPlan == null) {
						this.ConnectedPlanPanel.Plan = null;
					} else {
						this.ConnectedPlanPanel.Plan = this.product.AssociatedRoom.AssociatedPlan;
						Room room = this.product.AssociatedRoom;
						if (room.PlanSettingX.HasValue &&
							room.PlanSettingY.HasValue &&
							room.PlanSettingScale.HasValue &&
							room.PlanSettingAngle.HasValue) {
							this.ConnectedPlanPanel.SetPlanTransformations(room.PlanSettingScale.Value, room.PlanSettingX.Value, room.PlanSettingY.Value, room.PlanSettingAngle.Value);
						}
					}
				}
				this.connectionDrawer.Floor = (this.product != null && this.product.AssociatedRoom != null) ? this.product.AssociatedRoom.AssociatedFloor : null;
			}
		}

		public EurovalMode Mode {
			get { return this.mode; }
			set {
				this.mode = value;
				if (this.mode == EurovalMode.EVM_ADD_RZ) {
					rzStart = Point2D.Zero;
				} else if (this.mode == EurovalMode.EVM_ADD_AREA) {
					if (this.product.PlannedAreaGraphical.Count > 0) {
						Reset();
					}
				}
				if (this.ModeChanged != null) {
					this.ModeChanged(this, EventArgs.Empty);
				}
			}
		}

		#region IProductPlanner Members
		private IPlanPanel connectedPlanPanel;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPlanPanel ConnectedPlanPanel {
			get { return this.connectedPlanPanel; }
			set {
				if (this.connectedPlanPanel != null) {
					this.connectedPlanPanel.KeyDown -= new KeyEventHandler(connectedPlanPanel_KeyDown);
				}
				this.connectedPlanPanel = value;
				if (this.connectedPlanPanel != null) {
					this.connectedPlanPanel.KeyDown += new KeyEventHandler(connectedPlanPanel_KeyDown);
				}
			}
		}

		private void connectedPlanPanel_KeyDown(object sender, KeyEventArgs e) {
			if (this.mode == EurovalMode.EVM_ADD_RZ || this.mode == EurovalMode.EVM_DEL_RZ || this.mode == EurovalMode.EVM_DEL_RED || this.mode == EurovalMode.EVM_SET_TEXT) {
			    if (e.KeyCode == Keys.Escape) {
					this.ConnectedPlanPanel.Mode = PlanMode.PM_MOVE;
					this.Mode = EurovalMode.EVM_NONE;
					this.connectedPlanPanel.InvalidateGraphics();
					if (this.ModeChanged != null) {
						this.ModeChanged(this, EventArgs.Empty);
					}
			    }
			} else if (this.mode == EurovalMode.EVM_ADD_AREA || this.mode == EurovalMode.EVM_ADD_RED) {
				if (e.KeyCode == Keys.Escape) {
					if (MessageBox.Show("Wollen Sie das Definieren der Fläche abbrechen?", "Abbrechen?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
						this.inDesign = false;
						this.coordsPickedSoFar.Clear();
						this.Mode = EurovalMode.EVM_NONE;
						this.ConnectedPlanPanel.Mode = PlanMode.PM_MOVE;
						this.ConnectedPlanPanel.InvalidateGraphics();
						if (this.ModeChanged != null) {
							this.ModeChanged(this, EventArgs.Empty);
						}
					}
				}
			} 
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			this.PaintAfterPlanPannel(e.Graphics, additionalTransformation, mousePositionInPlan, mousePositionInControl);
		}

		public void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.RoomCoordinates != null) {
				this.connectionDrawer.Paint(g, additionalTransformation);
				GraphicsPath path = new GraphicsPath();
				List<PointF> transformedPoints = new List<PointF>();
				foreach (Point2D point in this.product.AssociatedRoom.RoomCoordinates) {
					Point2D tmp = additionalTransformation.TransformTo2D(point);
					transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
				}
				path.AddPolygon(transformedPoints.ToArray());
				Region clipDisabled = new Region();
				clipDisabled.MakeInfinite();
				clipDisabled.Exclude(path);
				Color c = Color.Black;
				if (this.ConnectedPlanPanel != null && this.ConnectedPlanPanel.ColorMode == ColorMode.CM_BLACK_BG) {
					c = Color.White;
				}
				Brush b = new SolidBrush(c);
				b = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal, Color.FromArgb(128, c), Color.FromArgb(112, c));

				if (highlightRoomCoordinates) {
					// gray out all except the room
					g.FillRegion(b, clipDisabled);
				}


				if (this.product.PlannedAreaGraphical.Count > 2) {
					GraphicsPath fillPath = new GraphicsPath();
					fillPath.StartFigure();
					PointF[] array = new PointF[this.product.PlannedAreaGraphical.Count];
					int i = 0;
					foreach (Point2D point in this.product.PlannedAreaGraphical) {
						Point2D tmp = additionalTransformation.TransformTo2D(point);
						array[i++] = new PointF((float)tmp.X, (float)tmp.Y);
					}
					fillPath.AddPolygon(array);
					fillPath.CloseFigure();
					c = Color.FromArgb(64, Color.Red);
					b = new SolidBrush(c);
					g.FillPath(b, fillPath);
					g.DrawPath(new Pen(b), fillPath);
					fillPath.Dispose();
				}

				// paint rim
				path.Reset();
				transformedPoints.Clear();
				if (this.product.PlannedRimLength > 0) {
					path.StartFigure();
					PointF[] array = new PointF[this.product.PlannedAreaGraphical.Count];
					int i = 0;
					foreach (Point2D point in this.product.PlannedAreaGraphical) {
						Point2D tmp = additionalTransformation.TransformTo2D(point);
						array[i++] = new PointF((float)tmp.X, (float)tmp.Y);
					}
					path.AddPolygon(array);
					path.CloseFigure();
					Brush rzBrush = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.Percent30, Color.FromArgb(255, Color.Red), Color.FromArgb(0, Color.Red));
					foreach (Segment2D rimSegment in this.product.PlannedRimSegments) {
						float width = this.product.PlannedRimWidth > 0 ? this.product.PlannedRimWidth : 5.0f;
						Pen pen = new Pen(rzBrush, (float)((width * 2.0 / 100.0) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * Math.Abs(additionalTransformation.M00)));
						Region oldClip = g.Clip;
						g.Clip = new Region(path);
						Point2D start = additionalTransformation.TransformTo2D(rimSegment.Start);
						Point2D end = additionalTransformation.TransformTo2D(rimSegment.End);
						g.DrawLine(pen, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
						g.Clip = oldClip;
					}
				}

				path.Reset();
				transformedPoints.Clear();
				if (coordsPickedSoFar.Count > 0 && inDesign) {
					List<Point2D> border = new List<Point2D>();
					if (this.Mode == EurovalMode.EVM_ADD_AREA) {
						border = this.product.AssociatedRoom.RoomCoordinates;
					} else {
						border = this.product.PlannedAreaGraphical;
					}

					List<Point2D> points = new List<Point2D>(coordsPickedSoFar);
					Point2D pos = new Point2D((float)mousePositionInPlan.X, (float)mousePositionInPlan.Y);
					if (GetSnapPoint(border, pos) != Point2D.Zero) {
						pos = GetSnapPoint(border, pos);
						foreach (Point2D point in border) {
							Segment2D line = new Segment2D(point, pos);
							if (line.GetLength() < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
								pos = point;
								break;
							}
						}
					} else {
						if ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT) {
							if (points.Count == 1) {
								pos = GetNormalizedPoint(points[0], null, pos);
							} else if (points.Count == 2) {
								pos = GetNormalizedPoint(points[points.Count - 1], points[0], pos);
							} else {
								pos = GetNormalizedPoint(points[points.Count - 1], points[0], pos, points[0]);
							}
						}
					}

					points.Add(pos);

					path.StartFigure();
					PointF[] array = new PointF[points.Count];
					int i = 0;
					foreach (Point2D point in points) {
						Point2D tmp = additionalTransformation.TransformTo2D(point);
						array[i++] = new PointF((float)tmp.X, (float)tmp.Y);
					}
					if (array.Length > 2) {
						//path.AddLines(array);
						path.AddPolygon(array);
					} else {
						path.AddLine(array[0], array[1]);
					}
					path.CloseFigure();
					b = null;
					if (this.Mode == EurovalMode.EVM_ADD_AREA) {
						c = Color.FromArgb(64, Color.Red);
						b = new SolidBrush(c);
						g.FillPath(b, path);
						g.DrawPath(new Pen(b), path);
					} else if (this.Mode == EurovalMode.EVM_ADD_RED) {
						c = Color.Black;
						if (this.ConnectedPlanPanel != null && this.ConnectedPlanPanel.ColorMode == ColorMode.CM_BLACK_BG) {
							c = Color.White;
						}
						b = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal, Color.FromArgb(128, c), Color.FromArgb(112, c));
						g.FillPath(b, path);
						b = new SolidBrush(c);
						g.DrawPath(new Pen(b), path);
					}
				}

				// paint product
				//path.Reset();
				//transformedPoints.Clear();
				//g.FillPath(new SolidBrush(Color.FromArgb(64, Color.Red)), path);

				if (this.mode == EurovalMode.EVM_ADD_RZ) {
					Point2D rzPoint = GetSnapPoint(this.product.PlannedAreaGraphical, mousePositionInPlan);

					if (rzPoint != Point2D.Zero) {
						foreach (Point2D point in this.product.PlannedAreaGraphical) {
							Segment2D line = new Segment2D(point, rzPoint);
							if (line.GetLength() < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
								rzPoint = point;
								break;
							}
						}
						Point2D p = additionalTransformation.TransformTo2D(rzPoint);
						float size = (float)(this.product.AssociatedRoom.AssociatedPlan.Measure * 0.10 * Math.Abs(additionalTransformation.M00));
						Pen pen = new Pen(Color.Blue, 2);
						g.DrawLine(pen, (float)p.X - size, (float)p.Y - size, (float)p.X + size, (float)p.Y + size);
						g.DrawLine(pen, (float)p.X - size, (float)p.Y + size, (float)p.X + size, (float)p.Y - size);
					}

					if (rzStart != Point2D.Zero) {
						List<Segment2D> rzSegments = GetSegments(this.product.PlannedAreaGraphical, rzStart);
						double dist = double.MaxValue;
						Segment2D closest = new Segment2D();
						foreach (Segment2D rzSegment in rzSegments) {
							if (rzSegment.GetDistance(mousePositionInPlan) < dist) {
								dist = rzSegment.GetDistance(mousePositionInPlan);
								closest = rzSegment;
							}
						}
						Point2D tempEnd = closest.GetClosestPoint(mousePositionInPlan);
						Pen pen = new Pen(Color.Blue, 2);
						Point2D start = additionalTransformation.TransformTo2D(rzStart);
						Point2D end = additionalTransformation.TransformTo2D(tempEnd);
						g.DrawLine(pen, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
					}
				}

				if (this.product.AssociatedRoom.RoomUnusedAreaCoordinates != null) {
					List<PointF> unusedPoints = new List<PointF>();
					foreach (List<Point2D> unusedArea in this.product.AssociatedRoom.RoomUnusedAreaCoordinates) {
						foreach (Point2D point in unusedArea) {
							Point2D tmp = additionalTransformation.TransformTo2D(point);
							unusedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
						}
						PointF[] pointArray = unusedPoints.ToArray();
						c = Color.Black;
						if (this.ConnectedPlanPanel != null && this.ConnectedPlanPanel.ColorMode == ColorMode.CM_BLACK_BG) {
							c = Color.White;
						}
						b = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal, Color.FromArgb(198, c), Color.FromArgb(182, c));
						g.DrawPolygon(new Pen(c), pointArray);
						g.FillPolygon(b, pointArray);
						unusedPoints.Clear();
					}
				}

				if (this.product.PlannedReducedAreas.Count > 0) {
					List<PointF> reducedPoints = new List<PointF>();
					foreach (List<Point2D> reducedArea in this.product.PlannedReducedAreas) {
						foreach (Point2D point in reducedArea) {
							Point2D tmp = additionalTransformation.TransformTo2D(point);
							reducedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
						}
						PointF[] pointArray = reducedPoints.ToArray();
						c = Color.Black;
						if (this.ConnectedPlanPanel != null && this.ConnectedPlanPanel.ColorMode == ColorMode.CM_BLACK_BG) {
							c = Color.White;
						}
						b = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal, Color.FromArgb(128, c), Color.FromArgb(112, c));
						g.DrawPolygon(new Pen(c), pointArray);
						g.FillPolygon(b, pointArray);
						reducedPoints.Clear();
					}
				}

				if (drawExpansionGaps) {
					foreach (Segment2D expansionGap in this.Product.AssociatedRoom.AssociatedFloor.ExpansionGaps) {
						Point2D start = additionalTransformation.TransformTo2D(expansionGap.Start);
						Point2D end = additionalTransformation.TransformTo2D(expansionGap.End);
						g.DrawLine(Pens.Blue, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
					}
				}

				if (this.product.TextBoxPosition == Point2D.Zero && product.PlannedAreaGraphical.Count > 0) {
					Polygon2D polygon = new Polygon2D(product.PlannedAreaGraphical);
					if (polygon.GetCentroid().HasValue) {
						product.TextBoxPosition = polygon.GetCentroid().Value;
					}
				}

				if (this.product.TextBoxPosition != Point2D.Zero) {
					Font font = new Font("Arial", 5.0f / g.DpiX * Math.Abs((float)additionalTransformation.M22) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value);
					float maxWidth = 0;
					float maxHeight = 0;

					string productName = Project.Instance.GetPlannedProduct(product).InternalName;
					string az = "--";
					if (product.PlannedLayDistance.HasValue) {
						switch (product.PlannedLayDistance) {
							case EurovalProduct.EurovalLayDistance.EV5:
								az = EuroplanRes.EurovalProduct_EV5; //"EV5";
								break;
							case EurovalProduct.EurovalLayDistance.EV10:
								az = EuroplanRes.EurovalProduct_EV10; //"EV10";
								break;
							case EurovalProduct.EurovalLayDistance.EV15:
								az = EuroplanRes.EurovalProduct_EV15; //"EV15";
								break;
							case EurovalProduct.EurovalLayDistance.EV20:
								az = EuroplanRes.EurovalProduct_EV20; //"EV20";
								break;
							case EurovalProduct.EurovalLayDistance.EV25:
								az = EuroplanRes.EurovalProduct_EV25; //"EV25";
								break;
							case EurovalProduct.EurovalLayDistance.EV30:
								az = EuroplanRes.EurovalProduct_EV30; //"EV30";
								break;
							case EurovalProduct.EurovalLayDistance.EV35:
								az = EuroplanRes.EurovalProduct_EV35; //"EV35";
								break;
							default:
								az = "--";
								break;
						}
					}

					string rz = "--";
					if (product.PlannedRimType.HasValue) {
						switch (product.PlannedRimLayDistance) {
							case EurovalProduct.EurovalLayDistance.EV5:
								rz = EuroplanRes.EurovalProduct_EV5 + "/" + product.PlannedRimWidth.ToString(); //"EV5";
								break;
							case EurovalProduct.EurovalLayDistance.EV10:
								rz = EuroplanRes.EurovalProduct_EV10 + "/" + product.PlannedRimWidth.ToString(); //"EV10";
								break;
							case EurovalProduct.EurovalLayDistance.EV15:
								rz = EuroplanRes.EurovalProduct_EV15 + "/" + product.PlannedRimWidth.ToString(); //"EV15";
								break;
							case EurovalProduct.EurovalLayDistance.EV20:
								rz = EuroplanRes.EurovalProduct_EV20 + "/" + product.PlannedRimWidth.ToString(); //"EV20";
								break;
							case EurovalProduct.EurovalLayDistance.EV25:
								rz = EuroplanRes.EurovalProduct_EV25 + "/" + product.PlannedRimWidth.ToString(); //"EV25";
								break;
							case EurovalProduct.EurovalLayDistance.EV30:
								rz = EuroplanRes.EurovalProduct_EV30 + "/" + product.PlannedRimWidth.ToString(); //"EV30";
								break;
							case EurovalProduct.EurovalLayDistance.EV35:
								rz = EuroplanRes.EurovalProduct_EV35 + "/" + product.PlannedRimWidth.ToString(); //"EV35";
								break;
							default:
								rz = "--";
								break;
						}
					}

					maxWidth = Math.Max(maxWidth, g.MeasureString("Name: ", font).Width);
					maxWidth = Math.Max(maxWidth, g.MeasureString("AZ: ", font).Width);
					maxWidth = Math.Max(maxWidth, g.MeasureString("RZ: ", font).Width);
					maxWidth = Math.Max(maxWidth, g.MeasureString("HK: ", font).Width);
					maxHeight = Math.Max(maxHeight, g.MeasureString("Name: ", font).Height);
					maxHeight = Math.Max(maxHeight, g.MeasureString("AZ: ", font).Height);
					maxHeight = Math.Max(maxHeight, g.MeasureString("RZ: ", font).Height);
					maxHeight = Math.Max(maxHeight, g.MeasureString("HK: ", font).Height);

					maxWidth = Math.Max(maxWidth, g.MeasureString(productName, font).Width);
					maxWidth = Math.Max(maxWidth, g.MeasureString(az, font).Width);
					maxWidth = Math.Max(maxWidth, g.MeasureString(rz, font).Width);
					maxWidth = Math.Max(maxWidth, g.MeasureString(product.PlannedCircuitCount.ToString(), font).Width);
					maxHeight = Math.Max(maxHeight, g.MeasureString(productName, font).Height);
					maxHeight = Math.Max(maxHeight, g.MeasureString(az, font).Height);
					maxHeight = Math.Max(maxHeight, g.MeasureString(rz, font).Height);
					maxHeight = Math.Max(maxHeight, g.MeasureString(product.PlannedCircuitCount.ToString(), font).Height);

					Pen p;
					if (this.connectedPlanPanel != null && this.connectedPlanPanel.ColorMode == ColorMode.CM_BLACK_BG) {
						p = new Pen(Color.White);
					} else {
						p = new Pen(Color.Black);
					}					
					float border = 2.0f / g.DpiX * Math.Abs((float)additionalTransformation.M22) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

					Point2D pos = Point2D.Zero;
					if (this.Mode == EurovalMode.EVM_SET_TEXT) {
						pos = mousePositionInPlan;
					} else {
						pos = product.TextBoxPosition;
					}

					PaintTextBox("Name: ", font, pos, maxWidth, 0, maxHeight, 0, border, p, g, additionalTransformation);
					PaintTextBox("AZ: ", font, pos, maxWidth, 0, maxHeight, 1, border, p, g, additionalTransformation);
					PaintTextBox("RZ: ", font, pos, maxWidth, 0, maxHeight, 2, border, p, g, additionalTransformation);
					PaintTextBox("HK: ", font, pos, maxWidth, 0, maxHeight, 3, border, p, g, additionalTransformation);

					PaintTextBox(productName, font, pos, maxWidth, 1, maxHeight, 0, border, p, g, additionalTransformation);
					PaintTextBox(az, font, pos, maxWidth, 1, maxHeight, 1, border, p, g, additionalTransformation);
					PaintTextBox(rz, font, pos, maxWidth, 1, maxHeight, 2, border, p, g, additionalTransformation);
					PaintTextBox(product.PlannedCircuitCount.ToString(), font, pos, maxWidth, 1, maxHeight, 3, border, p, g, additionalTransformation);
				}

				path.Dispose();
			}
		}

		private void PaintTextBox(string text, Font font, Point2D start, float width, int xFactor, float height, int yFactor, float border, Pen pen, Graphics g, Matrix4D additionalTransformation) {
			Point2D topleft2D = additionalTransformation.TransformTo2D(start);

			PointF topleft = new PointF((float)topleft2D.X + (xFactor * (width + (2 * border))), (float)topleft2D.Y + (yFactor * (height + (2 * border))));
			PointF topRight = new PointF((float)topleft.X + width + (2 * border), (float)topleft.Y);
			PointF bottomRight = new PointF((float)topleft.X + width + (2 * border), (float)topleft.Y + height + (2 * border));
			PointF bottomLeft = new PointF((float)topleft.X, (float)topleft.Y + height + (2 * border));
			PointF stringPos = new PointF((float)topleft.X + border, (float)topleft.Y + border);

			g.DrawLine(pen, topleft, topRight);
			g.DrawLine(pen, topRight, bottomRight);
			g.DrawLine(pen, bottomRight, bottomLeft);
			g.DrawLine(pen, bottomLeft, topleft);
			g.DrawString(text, font, new SolidBrush(pen.Color), stringPos);
		}

		private Point2D GetSnapPoint(List<Point2D> border, Point2D mousePosition) {
			double distance = Double.MaxValue;
			Point2D snapPoint = Point2D.Zero;
			Point2D prevPoint = Point2D.Zero;
			Segment2D line = new Segment2D();
			foreach (Point2D point in border) {
				if (prevPoint != Point2D.Zero) {
					line = new Segment2D(prevPoint, point);
					if (line.GetDistance(mousePosition) < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
						if (line.GetDistance(mousePosition) < distance) {
							distance = line.GetDistance(mousePosition);
							snapPoint = line.GetClosestPoint(mousePosition);
						}
					}
				}
				prevPoint = point;
			}
			if (snapPoint == Point2D.Zero) {
				line = new Segment2D(prevPoint, border[0]);
				if (line.GetDistance(mousePosition) < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
					if (line.GetDistance(mousePosition) < distance) {
						distance = line.GetDistance(mousePosition);
						snapPoint = line.GetClosestPoint(mousePosition);
					}
				}
			}
			return snapPoint;
		}

		private List<Segment2D> GetSegments(List<Point2D> border, Point2D referencePoint) {
			double distance = Double.MaxValue;
			
			Point2D rzPoint = Point2D.Zero;
			Point2D prevPoint = Point2D.Zero;
			List<Segment2D> list = new List<Segment2D>();

			Segment2D line = new Segment2D();
			foreach (Point2D point in border) {
				if (prevPoint != Point2D.Zero) {
					line = new Segment2D(prevPoint, point);
					if (line.GetDistance(referencePoint) < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
						if (line.GetDistance(referencePoint) < distance) {
							list.Add(line);
						}
					}
				}
				prevPoint = point;
			}

			line = new Segment2D(prevPoint, border[0]);
			if (line.GetDistance(referencePoint) < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
				if (line.GetDistance(referencePoint) < distance) {
					list.Add(line);
				}
			}

			return list;
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode != EurovalMode.EVM_ADD_AREA && this.Mode != EurovalMode.EVM_ADD_RED && this.Mode != EurovalMode.EVM_SET_TEXT && button == MouseButtons.Right) {
				rzStart = Point2D.Zero;
				this.Mode = EurovalMode.EVM_NONE;
				this.connectedPlanPanel.InvalidateGraphics();
				return true;
			}

			if (this.Mode == EurovalMode.EVM_ADD_AREA || this.Mode == EurovalMode.EVM_ADD_RED) {
				List<Point2D> border = new List<Point2D>();
				if (this.Mode == EurovalMode.EVM_ADD_AREA) {
					border = this.product.AssociatedRoom.RoomCoordinates;
				} else {
					border = this.product.PlannedAreaGraphical;
				}
				PointF pos = new PointF((float)planPoint.X, (float)planPoint.Y);

				Point2D normalizedPoint = planPoint;
				bool pick = (button == MouseButtons.Left) || (button == MouseButtons.Right);
				bool finishPick = button == MouseButtons.Right;
				bool addFinishinigPick = true;
				bool snapFound = false;
				if (coordsPickedSoFar.Count > 0 && (this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT) {
					if (coordsPickedSoFar.Count == 1) {
						if (GetSnapPoint(border, normalizedPoint) != Point2D.Zero) {
							normalizedPoint = GetSnapPoint(border, normalizedPoint);
							snapFound = true;
						} else {
							normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[0], null, normalizedPoint);
						}
					} else if (coordsPickedSoFar.Count == 2) {
						if (GetSnapPoint(border, normalizedPoint) != Point2D.Zero) {
							normalizedPoint = GetSnapPoint(border, normalizedPoint);
							snapFound = true;
						} else {
							normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], coordsPickedSoFar[0], normalizedPoint);
						}
					} else {
						bool isStart;
						if (GetSnapPoint(border, normalizedPoint) != Point2D.Zero) {
							normalizedPoint = GetSnapPoint(border, normalizedPoint);
							snapFound = true;
							foreach (Point2D point in border) {
								Segment2D line = new Segment2D(point, normalizedPoint);
								if (line.GetLength() < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
									normalizedPoint = point;
									break;
								}
							}
							isStart = coordsPickedSoFar.Count > 0 && normalizedPoint == coordsPickedSoFar[0];
						} else {
							normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], coordsPickedSoFar[0], normalizedPoint, coordsPickedSoFar[0], out isStart);
						}
						addFinishinigPick = finishPick;
						finishPick = finishPick || (pick && isStart);
					}
				} else {
					if (GetSnapPoint(border, normalizedPoint) != Point2D.Zero) {
						normalizedPoint = GetSnapPoint(border, normalizedPoint);
						snapFound = true;
					}
				}

				if (snapFound) {
					foreach (Point2D point in border) {
						Segment2D line = new Segment2D(point, normalizedPoint);
						if (line.GetLength() < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
							normalizedPoint = point;
							break;
						}
					}
				}

				if (!this.AreaIsValid(border, normalizedPoint)) {
					return false;
				}

				if (pick && !finishPick) {
					if (!inDesign && coordsPickedSoFar.Count == 0 && this.product.PlannedAreaGraphical.Count > 0) {
						if (!Reset()) {
							return false;
						}
					}

					unsavedChanges = true;
					coordsPickedSoFar.Add(normalizedPoint);
					this.SimplifyPolygon(coordsPickedSoFar, false);
					inDesign = true;
				} else if (finishPick) {
					if (addFinishinigPick) {
						coordsPickedSoFar.Add(normalizedPoint);
					}
					this.SimplifyPolygon(coordsPickedSoFar, true);
					if (coordsPickedSoFar.Count > 2) {
						if (this.Mode == EurovalMode.EVM_ADD_AREA) {
							double area = Math.Round(Math.Abs(new Polygon2D(coordsPickedSoFar).GetArea()) / Math.Pow(this.ConnectedPlanPanel.Plan.Measure.Value, 2.0), 2);
							DialogResult result = MessageBox.Show("Die definierte Fläche beträgt " + area + "m². Wollen Sie diese Fläche übernehmen?", "Fläche übernehmen?", MessageBoxButtons.YesNo);
							if (result.Equals(DialogResult.Yes)) {
								Polygon2D room = new Polygon2D(this.product.AssociatedRoom.RoomCoordinates);
								if (room.IsClockwise()) {
									room.Reverse();
								}
								Polygon2D prod = new Polygon2D(coordsPickedSoFar);
								if (prod.IsClockwise()) {
									prod.Reverse();
								}
								List<Polygon2D> list1 = new List<Polygon2D>();
								list1.Add(room);
								List<Polygon2D> list2 = new List<Polygon2D>();
								list2.Add(prod);

								IList<Polygon2D> clippedPolygons = Polygon2D.GetIntersection(list1, list2);
								if (clippedPolygons.Count > 0) {
									this.product.PlannedAreaGraphical.AddRange(clippedPolygons[0]);
									this.product.PlannedFloorArea = (float)Math.Round(Math.Abs(clippedPolygons[0].GetArea()) / Math.Pow(this.ConnectedPlanPanel.Plan.Measure.Value, 2.0), 2);
									list1.Clear();
									list1.Add(clippedPolygons[0]);
									list2.Clear();
									if (this.product.AssociatedRoom.RoomUnusedAreaCoordinates != null) {
										foreach (List<Point2D> unusedArea in this.product.AssociatedRoom.RoomUnusedAreaCoordinates) {
											Polygon2D unusedAreaPolygon = new Polygon2D(unusedArea);
											if (unusedAreaPolygon.IsClockwise()) {
												unusedAreaPolygon.Reverse();
											}
											list2.Add(unusedAreaPolygon);
										}
										clippedPolygons = Polygon2D.GetIntersection(list1, list2);
										area = 0;
										foreach (Polygon2D clippedPolygon in clippedPolygons) {
											area += Math.Round(Math.Abs(clippedPolygon.GetArea()) / Math.Pow(this.ConnectedPlanPanel.Plan.Measure.Value, 2.0), 2);
										}
										this.product.PlannedAreaUnheated = (float)area;
									}
								}								
								unsavedChanges = true;
								this.ConnectedPlanPanel.Mode = PlanMode.PM_MOVE;
								this.Mode = EurovalMode.EVM_NONE;
								if (ModeChanged != null) {
									this.ModeChanged(this, EventArgs.Empty);
								}
							}
						} else if (this.Mode == EurovalMode.EVM_ADD_RED) {
							Polygon2D prod = new Polygon2D(this.product.PlannedAreaGraphical);
							if (prod.IsClockwise()) {
								prod.Reverse();
							}
							Polygon2D reduced = new Polygon2D(coordsPickedSoFar);
							if (reduced.IsClockwise()) {
								reduced.Reverse();
							}
							List<Polygon2D> list1 = new List<Polygon2D>();
							list1.Add(prod);
							List<Polygon2D> list2 = new List<Polygon2D>();
							list2.Add(reduced);

							try {
								IList<Polygon2D> clippedPolygons = Polygon2D.GetIntersection(list1, list2);
								if (clippedPolygons.Count > 0) {
									list1.Clear();
									if (clippedPolygons[0].IsClockwise()) {
										clippedPolygons[0].Reverse();
									}
									list1.Add(clippedPolygons[0]);
									list2.Clear();
									foreach (List<Point2D> redArea in this.product.PlannedReducedAreas) {
										Polygon2D poly = new Polygon2D(redArea);
										if (poly.IsClockwise()) {
											poly.Reverse();
										}
										list2.Add(poly);
									}
									if (list2.Count > 0) {
										try {
											IList<Polygon2D> clippedPolygons2 = Polygon2D.GetDifference(list1, list2);
											foreach (Polygon2D poly in clippedPolygons2) {
												this.product.PlannedReducedAreas.Add(new List<Point2D>(poly));
											}
										} catch (Exception /*ex*/) {
											this.product.PlannedReducedAreas.Add(new List<Point2D>(clippedPolygons[0]));
										}
									} else {
										this.product.PlannedReducedAreas.Add(new List<Point2D>(clippedPolygons[0]));
									}

									double area = 0;
									foreach (List<Point2D> reducedArea in this.product.PlannedReducedAreas) {
										Polygon2D poly = new Polygon2D(reducedArea);
										area += Math.Round(Math.Abs(poly.GetArea()) / Math.Pow(this.ConnectedPlanPanel.Plan.Measure.Value, 2.0), 2);
									}

									this.product.PlannedAreaReduced = (float)area;
								}
							} catch (Exception /*ex*/) {
								return true;
							}					

							unsavedChanges = true;
							//this.ConnectedPlanPanel.Mode = PlanMode.PM_MOVE;
							//this.Mode = EurovalMode.EVM_NONE;
							//if (ModeChanged != null) {
							//    this.ModeChanged(this, EventArgs.Empty);
							//}
						}
					}
					coordsPickedSoFar.Clear();
					inDesign = false;
				}
				return true;
			}

			if (this.Mode == EurovalMode.EVM_ADD_RZ && button == MouseButtons.Left) {
				if (rzStart == Point2D.Zero) {
					rzStart = GetSnapPoint(this.product.PlannedAreaGraphical, planPoint);
					if (rzStart != Point2D.Zero) {
						foreach (Point2D point in this.product.PlannedAreaGraphical) {
							Segment2D line = new Segment2D(point, rzStart);
							if (line.GetLength() < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
								rzStart = point;
								break;
							}
						}
					}
				} else {
					Point2D rzEnd = GetSnapPoint(this.product.PlannedAreaGraphical, planPoint);
					if (rzEnd != Point2D.Zero) {
						foreach (Point2D point in this.product.PlannedAreaGraphical) {
							Segment2D line = new Segment2D(point, rzEnd);
							if (line.GetLength() < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
								rzEnd = point;
								break;
							}
						}

						List<Segment2D> rzSegments = GetSegments(this.product.PlannedAreaGraphical, rzEnd);
						foreach (Segment2D rzSegment in rzSegments) {
							if (GetSegments(this.product.PlannedAreaGraphical, rzStart).Contains(rzSegment)) {
								this.product.PlannedRimSegments.Add(new Segment2D(rzStart, rzEnd));

								float sum = 0;
								foreach (Segment2D segment in this.product.PlannedRimSegments) {
									sum += (float)segment.GetLength() / this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
								}
								this.product.PlannedRimLength = sum;

								rzStart = Point2D.Zero;

								if (this.ProjectChanged != null) {
									this.ProjectChanged(this);
								}
								return true;
							}
						}
					}
				}
			} else if (this.Mode == EurovalMode.EVM_DEL_RZ && button == MouseButtons.Left) {
				double distance = double.MaxValue;
				Segment2D toDelete = new Segment2D();
				foreach (Segment2D rzSegment in this.product.PlannedRimSegments) {
					if (rzSegment.GetDistance(planPoint) < distance) {
						distance = rzSegment.GetDistance(planPoint);
						toDelete = rzSegment;
					}
				}
				if (this.product.PlannedRimSegments.Contains(toDelete)) {
					this.product.PlannedRimSegments.Remove(toDelete);
				}

				float sum = 0;
				foreach (Segment2D segment in this.product.PlannedRimSegments) {
					sum += (float)segment.GetLength() / this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				}
				this.product.PlannedRimLength = sum;
				if (this.ProjectChanged != null) {
					this.ProjectChanged(this);
				}
				return true;
			} else if (this.Mode == EurovalMode.EVM_DEL_RED) {
				if (button == MouseButtons.Left) {
					List<Point2D> areaToDelete = null;
					foreach (List<Point2D> reducedArea in this.product.PlannedReducedAreas) {
						Polygon2D reducedPoly = new Polygon2D(reducedArea);
						if (reducedPoly.IsInside(planPoint)) {
							areaToDelete = reducedArea;
							break;
						}
					}
					if (areaToDelete != null) {
						this.product.PlannedReducedAreas.Remove(areaToDelete);
						double area = 0;
						foreach (List<Point2D> reducedArea in this.product.PlannedReducedAreas) {
							Polygon2D poly = new Polygon2D(reducedArea);
							area += Math.Round(Math.Abs(poly.GetArea()) / Math.Pow(this.ConnectedPlanPanel.Plan.Measure.Value, 2.0), 2);
						}
						this.product.PlannedAreaReduced = (float)area;
						return true;
					}
				}
			} else if (this.Mode == EurovalMode.EVM_SET_TEXT && button == MouseButtons.Left) {
				this.product.TextBoxPosition = planPoint;
				unsavedChanges = true;
				this.ConnectedPlanPanel.Mode = PlanMode.PM_MOVE;
				this.Mode = EurovalMode.EVM_NONE;
				if (ModeChanged != null) {
					this.ModeChanged(this, EventArgs.Empty);
				}
				return true;
			}
			return false;
		}

		private bool Reset() {
			if (this.Mode == EurovalMode.EVM_ADD_AREA) {
				DialogResult result;
				if (this.product.Connections != null && this.product.Connections.Count > 0) {
					result = MessageBox.Show("Wollen Sie die bereits definierte Fläche und die bestehenden Anbindeleitungen verwerfen und neu definieren?", "Verwerfen und neu definieren?", MessageBoxButtons.YesNo);
				} else {
					result = MessageBox.Show("Wollen Sie die bereits definierte Fläche verwerfen und neu definieren?", "Verwerfen und neu definieren?", MessageBoxButtons.YesNo);
				}
				if (result == DialogResult.No) {
					return false;
				}
				if (this.product.Connections != null) {
					this.product.Connections.Clear();
				}
				this.product.PlannedAreaGraphical.Clear();
				this.product.PlannedRimSegments.Clear();
				this.product.PlannedReducedAreas.Clear();
				this.product.PlannedRimLength = 0;
				this.product.PlannedRimCorners = 0;
				this.connectedPlanPanel.InvalidateGraphics();
				return true;
			} else if (this.Mode == EurovalMode.EVM_ADD_RED) {
				return true;
			}
			return true;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			if (this.Mode == EurovalMode.EVM_ADD_RZ || this.Mode == EurovalMode.EVM_SET_TEXT) {
				return true;
			} else if (this.Mode == EurovalMode.EVM_DEL_RED) {
				bool ok = false;
				foreach (List<Point2D> reducedArea in product.PlannedReducedAreas) {
					Polygon2D polygon = new Polygon2D(reducedArea);
					if (polygon.IsInside(planPoint)) {
						ok = true;
					}
				}
				if (ok) {
					this.ConnectedPlanPanel.PlanCursor = Cursors.Hand;
				} else {
					this.ConnectedPlanPanel.PlanCursor = Cursors.No;
				}
				return false;
			} else if (this.Mode == EurovalMode.EVM_ADD_AREA || this.Mode == EurovalMode.EVM_ADD_RED) {
				List<Point2D> border = new List<Point2D>();
				if (this.Mode == EurovalMode.EVM_ADD_AREA) {
					border = this.product.AssociatedRoom.RoomCoordinates;
				} else {
					border = this.product.PlannedAreaGraphical;
				}
				
				Point2D normalizedPoint = planPoint;
				bool isStart = false;
				if (coordsPickedSoFar.Count == 0) {
					bool snapFound = false;
					if (GetSnapPoint(border, planPoint) != Point2D.Zero) {
						normalizedPoint = GetSnapPoint(border, planPoint);
						snapFound = true;
					}
					foreach (Point2D point in border) {
						Segment2D line = new Segment2D(point, normalizedPoint);
						if (line.GetLength() < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
							normalizedPoint = point;
							snapFound = true;
							break;
						}
					}
				} else if (GetSnapPoint(border, planPoint) != Point2D.Zero) {
					normalizedPoint = GetSnapPoint(border, planPoint);
					foreach (Point2D point in border) {
						Segment2D line = new Segment2D(point, normalizedPoint);
						if (line.GetLength() < (this.product.AssociatedRoom.AssociatedPlan.Measure * 0.1)) {
							normalizedPoint = point;
							break;
						}
					}
					isStart = coordsPickedSoFar.Count > 0 && normalizedPoint == coordsPickedSoFar[0];
				} else {
					if ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT) {
						if (coordsPickedSoFar.Count == 1) {
							normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[0], null, planPoint);
						} else if (coordsPickedSoFar.Count == 2) {
							normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], coordsPickedSoFar[0], planPoint);
						} else {
							normalizedPoint = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], coordsPickedSoFar[0], planPoint, coordsPickedSoFar[0], out isStart);
						}
					}
				}
				if (AreaIsValid(border, normalizedPoint)) {
					this.ConnectedPlanPanel.PlanCursor = isStart ? Cursors.Hand : Cursors.Cross;
				} else {
					this.ConnectedPlanPanel.PlanCursor = Cursors.No;
				}
				return inDesign;
			}
			return false;
		}


		private bool AreaIsValid(List<Point2D> border, Point2D normalizedPoint) {
			Polygon2D polygon = new Polygon2D(border);
			if (!polygon.IsInside(normalizedPoint)) {
				// the current point is not inside the area
				List<Segment2D> segments = new List<Segment2D>();
				Polygon2D.GetSegments(polygon, segments);
				foreach (Segment2D segment in segments) {
					if (segment.GetDistance(normalizedPoint) == 0) {
						return true;
					}
				}
				return false;
			}
			return true;
		}

		//private bool Intersects(Polygon2D polygon, Segment2D line) {
		//    List<Segment2D> segments = new List<Segment2D>();
		//    Polygon2D.GetSegments(polygon, segments);
		//    foreach (Segment2D segment in segments) {
		//        if (Segment2D.Intersects(segment, line)) {
		//            if (segment.GetDistance(line.Start) == 0 || segment.GetDistance(line.End) == 0) {
		//                if (!polygon.IsInside(line.GetCenter())) {
		//                    return true;
		//                }
		//            } else {
		//                return true;
		//            }
		//        } else {
		//            string test = "";
		//        }
		//    }
		//    return false;
		//}


		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			return false;
		}

		private bool AllPointsInside(Polygon2D polygon, IEnumerable<Point2D> points) {
			bool inside = true;
			foreach (Point2D point in points) {
				if (!polygon.IsInside(point)) {
					inside = false;
					break;
				}
			}
			return inside;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			return false;
		}

		internal bool ShiftPressed {
			get { return (Control.ModifierKeys & (Keys.Shift | Keys.ShiftKey | Keys.LShiftKey | Keys.RShiftKey)) != Keys.None; }
		}

		public Cursor CustomCursor {
			get { return this.customCursor; }
		}

		public bool PlannerKeyPress(Keys key) {
			//return KeyDown(key);
			return false;
		}

		private Point2D GetNormalizedPoint(Point2D basePoint1, Nullable<Point2D> basePoint2, Point2D currentPoint, Nullable<Point2D> startPoint) {
			bool tmp;
			return this.GetNormalizedPoint(basePoint1, basePoint2, currentPoint, startPoint, out tmp);
		}

		private Point2D GetNormalizedPoint(Point2D basePoint1, Nullable<Point2D> basePoint2, Point2D currentPoint) {
			bool tmp;
			return this.GetNormalizedPoint(basePoint1, basePoint2, currentPoint, null, out tmp);
		}

		private bool onlyHorizAndVert = false;
		private double angleSnapDist = Math.Tan(10.0 / 180.0 * Math.PI);
		private double startPointSnapSqDist = 100;

		private Point2D GetNormalizedPoint(Point2D basePoint1_, Nullable<Point2D> basePoint2_, Point2D currentPoint_, Nullable<Point2D> startPoint_, out bool isStartPoint) {
			Matrix3D rotationMatrix = Transformation3D.Rotate(this.ConnectedPlanPanel.Plan.Rotation / 180 * Math.PI);
			Point2D basePoint1 = rotationMatrix.Transform(basePoint1_);
			Nullable<Point2D> basePoint2 = basePoint2_.HasValue ? rotationMatrix.Transform(basePoint2_.Value) : basePoint2_;
			Point2D currentPoint = rotationMatrix.Transform(currentPoint_);
			Nullable<Point2D> startPoint = startPoint_.HasValue ? rotationMatrix.Transform(startPoint_.Value) : startPoint_;
			isStartPoint = false;
			if (startPoint.HasValue) {
				double scale = this.ConnectedPlanPanel.ScaleForCalculation;
				double xDistStart = (startPoint.Value.X - currentPoint.X) * scale;
				double yDistStart = (startPoint.Value.Y - currentPoint.Y) * scale;
				if (xDistStart * xDistStart + yDistStart * yDistStart < startPointSnapSqDist) {
					isStartPoint = true;
					return startPoint_.Value;
				}
			}
			if (ConnectedPlanPanel.SupportsSnap) {
				return currentPoint;
			}

			double xDistance1 = Math.Abs(basePoint1.X - currentPoint.X);
			double yDistance1 = Math.Abs(basePoint1.Y - currentPoint.Y);
			double xDistInMeter1 = (currentPoint.X - basePoint1.X) / this.ConnectedPlanPanel.Plan.Measure.Value;
			double yDistInMeter1 = (currentPoint.Y - basePoint1.Y) / this.ConnectedPlanPanel.Plan.Measure.Value;
			double xDistInMeterRounded1 = Math.Round(xDistInMeter1, 1);
			double yDistInMeterRounded1 = Math.Round(yDistInMeter1, 1);
			Point2D p;
			if (onlyHorizAndVert) {
				if (xDistance1 < yDistance1) {
					p = new Point2D(basePoint1.X, basePoint1.Y + ((float)yDistInMeterRounded1 * this.ConnectedPlanPanel.Plan.Measure.Value));
				} else {
					p = new Point2D(basePoint1.X + ((float)xDistInMeterRounded1 * this.ConnectedPlanPanel.Plan.Measure.Value), basePoint1.Y);
				}

			} else {
				if (basePoint2.HasValue) {
					double xDistance2 = Math.Abs(basePoint2.Value.X - currentPoint.X);
					double yDistance2 = Math.Abs(basePoint2.Value.Y - currentPoint.Y);
					double xDistInMeter2 = (currentPoint.X - basePoint2.Value.X) / this.ConnectedPlanPanel.Plan.Measure.Value;
					double yDistInMeter2 = (currentPoint.Y - basePoint2.Value.Y) / this.ConnectedPlanPanel.Plan.Measure.Value;
					double xDistInMeterRounded2 = Math.Round(xDistInMeter2, 1);
					double yDistInMeterRounded2 = Math.Round(yDistInMeter2, 1);

					double newX = currentPoint.X;
					double newY = currentPoint.Y;
					if (Math.Abs(xDistInMeter1 - xDistInMeterRounded1) <= Math.Abs(xDistInMeter2 - xDistInMeterRounded2)) {
						newX = basePoint1.X + xDistInMeterRounded1 * this.ConnectedPlanPanel.Plan.Measure.Value;
					} else {
						newX = basePoint2.Value.X + xDistInMeterRounded2 * this.ConnectedPlanPanel.Plan.Measure.Value;
					}
					if (Math.Abs(yDistInMeter1 - yDistInMeterRounded1) <= Math.Abs(yDistInMeter2 - yDistInMeterRounded2)) {
						newY = basePoint1.Y + yDistInMeterRounded1 * this.ConnectedPlanPanel.Plan.Measure.Value;
					} else {
						newY = basePoint2.Value.Y + yDistInMeterRounded2 * this.ConnectedPlanPanel.Plan.Measure.Value;
					}

					xDistance1 = Math.Abs(basePoint1.X - newX);
					yDistance1 = Math.Abs(basePoint1.Y - newY);
					xDistance2 = Math.Abs(basePoint2.Value.X - newX);
					yDistance2 = Math.Abs(basePoint2.Value.Y - newY);

					double xTan1 = xDistance1 / yDistance1;
					double xTan2 = xDistance2 / yDistance2;
					double yTan1 = yDistance1 / xDistance1;
					double yTan2 = yDistance2 / xDistance2;
					if (xTan1 < xTan2) {
						if (xTan1 < angleSnapDist) {
							newX = basePoint1.X;
						}
					} else {
						if (xTan2 < angleSnapDist) {
							newX = basePoint2.Value.X;
						}
					}
					if (yTan1 < yTan2) {
						if (yTan1 < angleSnapDist) {
							newY = basePoint1.Y;
						}
					} else {
						if (yTan2 < angleSnapDist) {
							newY = basePoint2.Value.Y;
						}
					}
					p = new Point2D(newX, newY);
				} else {

					double newX = basePoint1.X + xDistInMeterRounded1 * this.ConnectedPlanPanel.Plan.Measure.Value;
					double newY = basePoint1.Y + yDistInMeterRounded1 * this.ConnectedPlanPanel.Plan.Measure.Value;
					xDistance1 = Math.Abs(basePoint1.X - newX);
					yDistance1 = Math.Abs(basePoint1.Y - newY);

					if (xDistance1 / yDistance1 <= angleSnapDist) {
						newX = basePoint1.X;
					} else if (yDistance1 / xDistance1 <= angleSnapDist) {
						newY = basePoint1.Y;
					}
					p = new Point2D(newX, newY);
				}
			}
			rotationMatrix = Transformation3D.Rotate(-this.ConnectedPlanPanel.Plan.Rotation / 180 * Math.PI);
			p = rotationMatrix.Transform(p);
			return p;
		}

		private double minSqDist = 0.00000001; // (0.1mm)

		private void SimplifyPolygon(List<Point2D> polygon, bool closed) {
			if (polygon.Count < 3) {
				return;
			}
			Point2D prev;
			Point2D cur;
			Point2D next;
			Line2D line;
			double measureSq = this.ConnectedPlanPanel.Plan.Measure.Value * this.ConnectedPlanPanel.Plan.Measure.Value;
			for (int i = 1; i < polygon.Count - 1; i++) {
				prev = polygon[i - 1];
				cur = polygon[i];
				next = polygon[i + 1];
				line = new Line2D(prev, next - prev);
				if (line.GetDistanceSquared(cur) / measureSq < minSqDist) {
					polygon.Remove(cur);
					SimplifyPolygon(polygon, closed);
					return;
				}
			}
			if (closed) {
				prev = polygon[polygon.Count - 2];
				cur = polygon[polygon.Count - 1];
				next = polygon[0];
				line = new Line2D(prev, next - prev);
				if (line.GetDistanceSquared(cur) / measureSq < minSqDist) {
					polygon.Remove(cur);
					SimplifyPolygon(polygon, closed);
					return;
				}
				prev = polygon[polygon.Count - 1];
				cur = polygon[0];
				next = polygon[1];
				line = new Line2D(prev, next - prev);
				if (line.GetDistanceSquared(cur) / measureSq < minSqDist) {
					polygon.Remove(cur);
					SimplifyPolygon(polygon, closed);
					return;
				}
			}
		}

		#endregion

		public event ProjectChangedHandler ProjectChanged;

		internal void DrawDxf(DxfModel model, DxfLayer layer) {
			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.RoomCoordinates != null) {
				Color c = Color.Red;
				double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				List<Polygon2D> clip = new List<Polygon2D>();

				if (this.product.PlannedReducedAreas.Count > 0) {
					Color gray = Color.Gray;
					foreach (List<Point2D> reducedArea in this.product.PlannedReducedAreas) {
						Polygon2D polygon = new Polygon2D(reducedArea);
						clip.Add(polygon);
						DxfPolyline2D polyLine = new DxfPolyline2D(gray, polygon.ToArray());
						polyLine.Closed = true;
						polyLine.Layer = layer;
						model.Entities.Add(polyLine);

						DxfHatch hatch = new DxfHatch();
						hatch.Color = gray;
						DxfHatch.BoundaryPath boundaryPath = new DxfHatch.BoundaryPath();
						boundaryPath.Type = BoundaryPathType.Polyline;
						boundaryPath.PolylineData = new DxfHatch.BoundaryPath.Polyline(polygon.ToArray());
						boundaryPath.PolylineData.Closed = true;
						hatch.BoundaryPaths.Add(boundaryPath);

						hatch.Pattern = new DxfPattern();
						DxfPattern.Line patternLine = new DxfPattern.Line();
						patternLine.Angle = Math.PI / 4d;
						patternLine.Offset = new Vector2D(0.11d * measure, -0.11d * measure);
						hatch.Pattern.Lines.Add(patternLine);

						hatch.Layer = layer;
						model.Entities.Add(hatch);
					}
				}

				if (this.product.AssociatedRoom.RoomUnusedAreaCoordinates != null) {
					Color gray = Color.Red;
					foreach (List<Point2D> unusedArea in this.product.AssociatedRoom.RoomUnusedAreaCoordinates) {
						Polygon2D polygon = new Polygon2D(unusedArea);
						clip.Add(polygon);
						DxfPolyline2D polyLine = new DxfPolyline2D(gray, polygon.ToArray());
						polyLine.Closed = true;
						polyLine.Layer = layer;
						model.Entities.Add(polyLine);
					}
				}

				if (this.product.PlannedAreaGraphical.Count > 2) {
					Point2D[] polygon = this.product.PlannedAreaGraphical.ToArray();

					DxfPolyline2D polyLine = new DxfPolyline2D(c, polygon);
					polyLine.Closed = true;
					polyLine.Layer = layer;
					model.Entities.Add(polyLine);

					DxfHatch hatch = new DxfHatch();
					hatch.Color = c;
					DxfHatch.BoundaryPath boundaryPath = new DxfHatch.BoundaryPath();
					boundaryPath.Type = BoundaryPathType.Polyline;
					boundaryPath.PolylineData = new DxfHatch.BoundaryPath.Polyline(polygon);
					boundaryPath.PolylineData.Closed = true;
					hatch.BoundaryPaths.Add(boundaryPath);

					foreach (Polygon2D poly in clip) {
						if (!poly.IsClockwise()) {
							poly.Reverse();
						}
						boundaryPath = new DxfHatch.BoundaryPath();
						boundaryPath.Type = BoundaryPathType.Polyline;
						boundaryPath.PolylineData = new DxfHatch.BoundaryPath.Polyline(poly);
						boundaryPath.PolylineData.Closed = true;
						hatch.BoundaryPaths.Add(boundaryPath);
					}
										
					hatch.Pattern = new DxfPattern();
					DxfPattern.Line patternLine = new DxfPattern.Line();
					patternLine.Angle = Math.PI / 4d;
					patternLine.Offset = new Vector2D(0.3d * measure, -0.3d * measure);
					hatch.Pattern.Lines.Add(patternLine);
					//patternLine = new DxfPattern.Line();
					//patternLine.Angle = 3d * Math.PI / 4d;
					//patternLine.Offset = new Vector2D(0.02 * measure, 0.02d * measure);
					//hatch.Pattern.Lines.Add(patternLine);

					hatch.Layer = layer;
					model.Entities.Add(hatch);
				}

				// paint rim
				if (this.product.PlannedRimLength > 0) {
					Polygon2D product = new Polygon2D(this.product.PlannedAreaGraphical);
					List<Polygon2D> list1 = new List<Polygon2D>();
					if (product.IsClockwise()) {
						product.Reverse();
					}


					Brush rzBrush = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.Percent30, Color.FromArgb(255, Color.Red), Color.FromArgb(0, Color.Red));
					foreach (Segment2D rimSegment in this.product.PlannedRimSegments) {
						list1.Clear();
						list1.Add(product);
						List<Polygon2D> list2 = new List<Polygon2D>();
						float width = this.product.PlannedRimWidth > 0 ? this.product.PlannedRimWidth : 5.0f;
						width = width / 100 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
						Vector2D vector = rimSegment.GetDelta();
						vector.Normalize();
						Vector2D norm1 = new Vector2D(-vector.Y, vector.X);
						Vector2D norm2 = new Vector2D(vector.Y, -vector.X);
						norm1 *= width;
						norm2 *= width;
						Point2D p1 = rimSegment.Start + norm1;
						Point2D p2 = rimSegment.Start + norm2;
						Point2D p3 = rimSegment.End + norm2;
						Point2D p4 = rimSegment.End + norm1;

						Polygon2D rim = new Polygon2D();
						rim.Add(p1);
						rim.Add(p2);
						rim.Add(p3);
						rim.Add(p4);
						if (rim.IsClockwise()) {
							rim.Reverse();
						}
						list2.Add(rim);

						IList<Polygon2D> clippedPolygons = Polygon2D.GetIntersection(list1, list2);
						foreach (Polygon2D polygon in clippedPolygons) {
							//DxfPolyline2D polyLine = new DxfPolyline2D(c, polygon);
							//polyLine.Closed = true;
							//polyLine.Layer = layer;
							//model.Entities.Add(polyLine);
							if (polygon.IsClockwise()) {
								polygon.Reverse();
							}

							DxfHatch hatch = new DxfHatch();
							hatch.Color = c;
							DxfHatch.BoundaryPath boundaryPath = new DxfHatch.BoundaryPath();
							boundaryPath.Type = BoundaryPathType.Polyline;
							boundaryPath.PolylineData = new DxfHatch.BoundaryPath.Polyline(polygon);
							boundaryPath.PolylineData.Closed = true;
							hatch.BoundaryPaths.Add(boundaryPath);
							

							foreach (Polygon2D poly in clip) {
								if (poly.IsClockwise()) {
									poly.Reverse();
								}
								list1 = new List<Polygon2D>();
								list1.Add(polygon);
								list2 = new List<Polygon2D>();
								list2.Add(poly);
								IList<Polygon2D> clipped = Polygon2D.GetIntersection(list1, list2);
								if (clipped.Count > 0) {
									foreach (Polygon2D p in clipped) {
										boundaryPath = new DxfHatch.BoundaryPath();
										boundaryPath.Type = BoundaryPathType.Polyline;
										boundaryPath.PolylineData = new DxfHatch.BoundaryPath.Polyline(p);
										boundaryPath.PolylineData.Closed = true;
										hatch.BoundaryPaths.Add(boundaryPath);
									}
								}
							}

							hatch.Pattern = new DxfPattern();
							DxfPattern.Line patternLine = new DxfPattern.Line();
							patternLine.Angle = 3d * Math.PI / 4d;
							patternLine.Offset = new Vector2D(0.15d * measure, 0.15d * measure);
							hatch.Pattern.Lines.Add(patternLine);
							hatch.Layer = layer;
							model.Entities.Add(hatch);
						}
					}

				}

				if (this.product.TextBoxPosition == Point2D.Zero && this.product.PlannedAreaGraphical.Count > 0) {
					Polygon2D polygon = new Polygon2D(product.PlannedAreaGraphical);
					if (polygon.GetCentroid().HasValue) {
						product.TextBoxPosition = polygon.GetCentroid().Value;
					}
				}

				if (this.product.TextBoxPosition != Point2D.Zero) {
					double maxWidth = 0;
					double maxHeight = 0;

					string productName = Project.Instance.GetPlannedProduct(product).InternalName;
					string az = "--";
					if (product.PlannedLayDistance.HasValue) {
						switch (product.PlannedLayDistance) {
							case EurovalProduct.EurovalLayDistance.EV5:
								az = EuroplanRes.EurovalProduct_EV5; //"EV5";
								break;
							case EurovalProduct.EurovalLayDistance.EV10:
								az = EuroplanRes.EurovalProduct_EV10; //"EV10";
								break;
							case EurovalProduct.EurovalLayDistance.EV15:
								az = EuroplanRes.EurovalProduct_EV15; //"EV15";
								break;
							case EurovalProduct.EurovalLayDistance.EV20:
								az = EuroplanRes.EurovalProduct_EV20; //"EV20";
								break;
							case EurovalProduct.EurovalLayDistance.EV25:
								az = EuroplanRes.EurovalProduct_EV25; //"EV25";
								break;
							case EurovalProduct.EurovalLayDistance.EV30:
								az = EuroplanRes.EurovalProduct_EV30; //"EV30";
								break;
							case EurovalProduct.EurovalLayDistance.EV35:
								az = EuroplanRes.EurovalProduct_EV35; //"EV35";
								break;
							default:
								az = "--";
								break;
						}
					}

					string rz = "--";
					if (product.PlannedRimType.HasValue) {
						switch (product.PlannedRimLayDistance) {
							case EurovalProduct.EurovalLayDistance.EV5:
								rz = EuroplanRes.EurovalProduct_EV5 + "/" + product.PlannedRimWidth.ToString(); //"EV5";
								break;
							case EurovalProduct.EurovalLayDistance.EV10:
								rz = EuroplanRes.EurovalProduct_EV10 + "/" + product.PlannedRimWidth.ToString(); //"EV10";
								break;
							case EurovalProduct.EurovalLayDistance.EV15:
								rz = EuroplanRes.EurovalProduct_EV15 + "/" + product.PlannedRimWidth.ToString(); //"EV15";
								break;
							case EurovalProduct.EurovalLayDistance.EV20:
								rz = EuroplanRes.EurovalProduct_EV20 + "/" + product.PlannedRimWidth.ToString(); //"EV20";
								break;
							case EurovalProduct.EurovalLayDistance.EV25:
								rz = EuroplanRes.EurovalProduct_EV25 + "/" + product.PlannedRimWidth.ToString(); //"EV25";
								break;
							case EurovalProduct.EurovalLayDistance.EV30:
								rz = EuroplanRes.EurovalProduct_EV30 + "/" + product.PlannedRimWidth.ToString(); //"EV30";
								break;
							case EurovalProduct.EurovalLayDistance.EV35:
								rz = EuroplanRes.EurovalProduct_EV35 + "/" + product.PlannedRimWidth.ToString(); //"EV35";
								break;
							default:
								rz = "--";
								break;
						}
					}

					if (!model.TextStyles.Contains("HarreitherStyle")) {
						DxfTextStyle textStyle = new DxfTextStyle("HarreitherStyle", "Arial.ttf");
						model.TextStyles.Add(textStyle);
					}
					Point2D pos = product.TextBoxPosition;
					
					DxfText text = new DxfText("Name: ", (Point3D)pos, 0.05f * this.product.AssociatedRoom.AssociatedPlan.Measure.Value);
					text.Style = model.TextStyles["HarreitherStyle"];
					maxWidth = Math.Max(maxWidth, text.BoxWidth);
					maxHeight = Math.Max(maxHeight, text.BoxHeight);
					text.Text = "AZ: ";
					maxWidth = Math.Max(maxWidth, text.BoxWidth);
					maxHeight = Math.Max(maxHeight, text.BoxHeight); 
					text.Text = "RZ: ";
					maxWidth = Math.Max(maxWidth, text.BoxWidth);
					maxHeight = Math.Max(maxHeight, text.BoxHeight);
					text.Text = "HK: ";
					maxWidth = Math.Max(maxWidth, text.BoxWidth);
					maxHeight = Math.Max(maxHeight, text.BoxHeight);

					text.Text = productName;
					maxWidth = Math.Max(maxWidth, text.BoxWidth);
					maxHeight = Math.Max(maxHeight, text.BoxHeight);
					text.Text = az;
					maxWidth = Math.Max(maxWidth, text.BoxWidth);
					maxHeight = Math.Max(maxHeight, text.BoxHeight);
					text.Text = rz;
					maxWidth = Math.Max(maxWidth, text.BoxWidth);
					maxHeight = Math.Max(maxHeight, text.BoxHeight);
					text.Text = product.PlannedCircuitCount.ToString();
					maxWidth = Math.Max(maxWidth, text.BoxWidth);
					maxHeight = Math.Max(maxHeight, text.BoxHeight);

					Color color;
					if (this.connectedPlanPanel != null && this.connectedPlanPanel.ColorMode == ColorMode.CM_BLACK_BG) {
						color = Color.White;
					} else {
						color = Color.Black;
					}
					float border = 0.02f * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

					PaintDxfTextBox("Name: ", "HarreitherStyle", pos, maxWidth, 0, maxHeight, 0, border, color, model, layer);
					PaintDxfTextBox("AZ: ", "HarreitherStyle", pos, maxWidth, 0, maxHeight, -1, border, color, model, layer);
					PaintDxfTextBox("RZ: ", "HarreitherStyle", pos, maxWidth, 0, maxHeight, -2, border, color, model, layer);
					PaintDxfTextBox("HK: ", "HarreitherStyle", pos, maxWidth, 0, maxHeight, -3, border, color, model, layer);

					PaintDxfTextBox(productName, "HarreitherStyle", pos, maxWidth, 1, maxHeight, 0, border, color, model, layer);
					PaintDxfTextBox(az, "HarreitherStyle", pos, maxWidth, 1, maxHeight, -1, border, color, model, layer);
					PaintDxfTextBox(rz, "HarreitherStyle", pos, maxWidth, 1, maxHeight, -2, border, color, model, layer);
					PaintDxfTextBox(product.PlannedCircuitCount.ToString(), "HarreitherStyle", pos, maxWidth, 1, maxHeight, -3, border, color, model, layer);
				}


			//    if (this.product.AssociatedRoom.AssociatedPlan != null && this.product.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
			//        if (this.product.GraphConstruction != null) {
			//            this.product.GraphConstruction.PaintDxf(model, floorConstructionLayer);
			//        }
			//    }

			//    if (this.mode != KlimaBodenMode.KDM_CONSTRUCTION) {
			//        Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
			//        Matrix3D invRotation = rotation.GetInverse();

			//        List<KlimaFlaechenModul> selectedModules = this.GetAllSelectedModules();
			//        foreach (ModulBodenCircuit circuit in this.product.PlannedCircuits) {
			//            foreach (KlimaFlaechenModul modul in circuit.Row.List) {
			//                this.DrawDxfModule(modul.ModulType, modul.Orientation, invRotation.Transform(new Point2D(modul.GraphPosX, modul.GraphPosY)), additionalTransformation, model, modulLayer, modul.GraphBottomUp, circuit.CircuitColor);
			//            }
			//        }
			//    }
			}
		}

		private void PaintDxfTextBox(string dxfText, string fontStyle, Point2D start, double width, int xFactor, double height, int yFactor, float border, Color c, DxfModel model, DxfLayer layer) {
			Point2D topleft2D = start;

			Point2D topleft = new Point2D(topleft2D.X + (xFactor * (width + (2 * border))), topleft2D.Y + (yFactor * (height + (2 * border))));
			Point2D topRight = new Point2D(topleft.X + width + (2 * border), topleft.Y);
			Point2D bottomRight = new Point2D(topleft.X + width + (2 * border), topleft.Y + height + (2 * border));
			Point2D bottomLeft = new Point2D(topleft.X, topleft.Y + height + (2 * border));
			Point2D stringPos = new Point2D(topleft.X + border, topleft.Y + border);

			DxfLine line = new DxfLine(c, topleft, topRight);
			line.Layer = layer;
			model.Entities.Add(line);
			line = new DxfLine(c, topRight, bottomRight);
			line.Layer = layer;
			model.Entities.Add(line);
			line = new DxfLine(c, bottomRight, bottomLeft);
			line.Layer = layer;
			model.Entities.Add(line);
			line = new DxfLine(c, bottomLeft, topleft);
			line.Layer = layer;
			model.Entities.Add(line);

			DxfText text = new DxfText(dxfText, (Point3D)stringPos, 0.05f * this.product.AssociatedRoom.AssociatedPlan.Measure.Value);
			text.Style = model.TextStyles[fontStyle];
			text.Layer = layer;
			text.Color = c;
			model.Entities.Add(text);
		}

		[DefaultValue(true)]
		public bool HighlightRoomCoordinates {
			get { return this.highlightRoomCoordinates; }
			set { this.highlightRoomCoordinates = value; }
		}

		[DefaultValue(true)]
		public bool DrawExpansionGaps {
			get { return this.drawExpansionGaps; }
			set { this.drawExpansionGaps = value; }
		}
	}
}
