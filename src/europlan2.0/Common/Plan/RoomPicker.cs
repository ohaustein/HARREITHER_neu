using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using WW.Math;

namespace Europlan.Common {
	public partial class RoomPicker : Component, IProductPlanner {

		public enum RoomPickerMode {
			RPM_PICK_ROOM,
			RPM_PICK_UNUSED,
			RPM_DEL_UNUSED
		}

		public RoomPicker() {
			InitializeComponent();
		}

		public RoomPicker(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		private Room room;
		private RoomPickerMode mode = RoomPickerMode.RPM_PICK_ROOM;
		private List<PointF> roomCoordinates = new List<PointF>();
		private List<List<PointF>> unusedCoordinates = new List<List<PointF>>();
		private List<PointF> coordsPickedSoFar = new List<PointF>();
		private bool inDesign = false;
		private bool unsavedRoomPickerChanges = false;
		private PointF mousePosInPlan;

		public Room Room {
			get { return this.room; }
			set {
				this.room = value;
				if (this.ConnectedPlanPanel != null) {
					if (this.room == null || this.room.AssociatedPlan == null) {
						this.ConnectedPlanPanel.Plan = null;
					} else {
						this.ConnectedPlanPanel.Plan = this.room.AssociatedPlan;
					}
				}
			}
		}

		public RoomPickerMode Mode {
			get { return this.mode; }
			set { this.mode = value; }
		}

		#region IProductPlanner Members
		private IPlanPanel connectedPlanPanel;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPlanPanel ConnectedPlanPanel {
			get { return this.connectedPlanPanel; }
			set { this.connectedPlanPanel = value; }
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation) {
			Graphics g = e.Graphics;
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
				path.Dispose();
			}

			if (unusedCoordinates.Count > 0) {
				foreach (List<PointF> unusedArea in unusedCoordinates) {
					GraphicsPath path = new GraphicsPath();
					path.StartFigure();
					PointF[] array = unusedArea.ToArray();
					path.AddPolygon(array);
					path.CloseFigure();
					Color c = Color.FromArgb(0, Color.Red);
					Color c2 = Color.FromArgb(128, Color.White);
					Brush b = new HatchBrush(HatchStyle.BackwardDiagonal, c2, c);
					g.FillPath(b, path);
					b = new SolidBrush(c2);
					g.DrawPath(new Pen(b), path);
					path.Dispose();
				}
			}

			if (coordsPickedSoFar.Count > 0 && inDesign) {
				List<PointF> points = new List<PointF>(coordsPickedSoFar);
				PointF pos = mousePosInPlan;
				if ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT) {
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
				Brush b = null;
				if (this.Mode == RoomPickerMode.RPM_PICK_ROOM) {
					Color c = Color.FromArgb(128, Color.Red);
					b = new SolidBrush(c);
					g.FillPath(b, path);
					g.DrawPath(new Pen(b), path);
				} else if (this.Mode == RoomPickerMode.RPM_PICK_UNUSED) {
					Color c = Color.FromArgb(0, Color.Red);
					Color c2 = Color.FromArgb(128, Color.White);
					b = new HatchBrush(HatchStyle.BackwardDiagonal, c2, c);
					g.FillPath(b, path);
					b = new SolidBrush(c2);
					g.DrawPath(new Pen(b), path);
				}
				path.Dispose();
			}
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.PointF screenPoint, MouseButtons button) {
			// TODO
			if (this.Mode == RoomPickerMode.RPM_PICK_ROOM) {
				PointF pos = screenPoint;

				if (this.Mode == RoomPickerMode.RPM_PICK_UNUSED) {
					GraphicsPath path = new GraphicsPath();
					path.StartFigure();
					PointF[] array = roomCoordinates.ToArray();
					path.AddPolygon(array);
					path.CloseFigure();
					if (!path.IsVisible(pos)) {
						path.Dispose();
						return false;
					} else {
						if (unusedCoordinates.Count > 0) {
							foreach (List<PointF> unusedArea in unusedCoordinates) {
								path.Dispose();
								path = new GraphicsPath();
								path.StartFigure();
								array = unusedArea.ToArray();
								path.AddPolygon(array);
								path.CloseFigure();
								if (path.IsVisible(pos)) {
									path.Dispose();
									return false;
								}
								path.Dispose();
								if (coordsPickedSoFar.Count > 0 && inDesign) {
									PointF prev = PointF.Empty;
									PointF mouse = pos;
									PointF start = coordsPickedSoFar[coordsPickedSoFar.Count - 1];
									if ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT) {
										mouse = GetNormalizedPoint(start, pos);
									}
									if (coordsPickedSoFar.Count >= 2) {
										List<PointF> temp = new List<PointF>(coordsPickedSoFar);
										temp.Add(mouse);
										path.Dispose();
										path = new GraphicsPath();
										path.StartFigure();
										array = temp.ToArray();
										path.AddPolygon(array);
										path.CloseFigure();
									}
									foreach (PointF curr in unusedArea) {
										if (coordsPickedSoFar.Count >= 2 && path.IsVisible(curr)) {
											path.Dispose();
											return false;
										}
										path.Dispose();
										if (prev != PointF.Empty) {
											if (IsIntersecting(mouse, start, prev, curr)) {
												return false;
											}
											if (IsIntersecting(mouse, coordsPickedSoFar[0], prev, curr)) {
												return false;
											}
										}
										prev = curr;
									}

									prev = PointF.Empty;
									foreach (PointF curr in roomCoordinates) {
										if (prev != PointF.Empty) {
											if (IsIntersecting(mouse, start, prev, curr)) {
												return false;
											}
											if (IsIntersecting(mouse, coordsPickedSoFar[0], prev, curr)) {
												return false;
											}
										}
										prev = curr;
									}

								}
							}
						}
					}
				}

				if (button == MouseButtons.Left) {
					if ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT && coordsPickedSoFar.Count > 0) {
						pos = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], pos);
					}
					if (!inDesign && this.Mode == RoomPickerMode.RPM_PICK_ROOM && roomCoordinates.Count > 0) {
						// TODO
						DialogResult result = MessageBox.Show("Wollen Sie die bereits definierte Raumgeometrie verwerfen und neu definieren?", "Verwerfen und neu definieren?", MessageBoxButtons.YesNo);
						if (result == DialogResult.No) {
							return false;
						}
					}
					unsavedRoomPickerChanges = true;
					coordsPickedSoFar.Add(pos);
					inDesign = true;
					if (this.Mode == RoomPickerMode.RPM_PICK_ROOM) {
						roomCoordinates.Clear();
					}
				} else if (button == MouseButtons.Right) {
					if ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT && coordsPickedSoFar.Count > 0) {
						pos = GetNormalizedPoint(coordsPickedSoFar[coordsPickedSoFar.Count - 1], pos);
					}
					coordsPickedSoFar.Add(pos);
					if (coordsPickedSoFar.Count > 2) {
						if (this.Mode == RoomPickerMode.RPM_PICK_ROOM) {
							// TODO
							DialogResult result = MessageBox.Show("Die definierte Fläche beträgt " + Math.Round(Europlan.Common.Plan.PolygonArea(coordsPickedSoFar.ToArray()) / Math.Pow(this.ConnectedPlanPanel.Plan.Measure.Value, 2.0), 2) + "m². Kleine Ungenauigkeiten in der Flächenberechnung können nachträglich manuell geändert werden. Wollen Sie diese Raumgeometrie übernehmen?", "Raumgeometrie übernehmen?", MessageBoxButtons.YesNo);
							if (result.Equals(DialogResult.Yes)) {
								roomCoordinates.AddRange(coordsPickedSoFar);
								unsavedRoomPickerChanges = true;
							}
						} else if (this.Mode == RoomPickerMode.RPM_PICK_UNUSED) {
							unusedCoordinates.Add(new List<PointF>(coordsPickedSoFar));
							unsavedRoomPickerChanges = true;
						}
					}
					coordsPickedSoFar.Clear();
					inDesign = false;
				}
				return true;
			}
			return false;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.PointF screenPoint, MouseButtons button) {
			if (this.Mode == RoomPickerMode.RPM_PICK_UNUSED) {
				GraphicsPath path = new GraphicsPath();
				path.StartFigure();
				PointF[] array = roomCoordinates.ToArray();
				path.AddPolygon(array);
				path.CloseFigure();
				if (path.IsVisible(new PointF((float)planPoint.X, (float)planPoint.Y))) {
					path.Dispose();
					this.ConnectedPlanPanel.Cursor = Cursors.Cross;
					if (unusedCoordinates.Count > 0) {
						foreach (List<PointF> unusedArea in unusedCoordinates) {
							path = new GraphicsPath();
							path.StartFigure();
							array = unusedArea.ToArray();
							path.AddPolygon(array);
							path.CloseFigure();
							if (path.IsVisible(new PointF((float)planPoint.X, (float)planPoint.Y))) {
								this.ConnectedPlanPanel.Cursor = Cursors.No;
								path.Dispose();
								break;
							} else {
								path.Dispose();
								if (coordsPickedSoFar.Count > 0 && inDesign) {
									PointF prev = PointF.Empty;
									PointF mouse = new PointF((float)planPoint.X, (float)planPoint.Y);
									PointF start = coordsPickedSoFar[coordsPickedSoFar.Count - 1];
									if ((this.ConnectedPlanPanel.ModifierKey & ModifierKey.MK_SHIFT) != ModifierKey.MK_SHIFT) {
										mouse = GetNormalizedPoint(start, new PointF((float)planPoint.X, (float)planPoint.Y) );
									}
									if (coordsPickedSoFar.Count >= 2) {
										List<PointF> temp = new List<PointF>(coordsPickedSoFar);
										temp.Add(mouse);
										path = new GraphicsPath();
										path.StartFigure();
										array = temp.ToArray();
										path.AddPolygon(array);
										path.CloseFigure();
									}
									foreach (PointF curr in unusedArea) {
										if (coordsPickedSoFar.Count >= 2 && path.IsVisible(curr)) {
											this.ConnectedPlanPanel.Cursor = Cursors.No;
											break;
										}
										if (prev != PointF.Empty) {
											if (IsIntersecting(mouse, start, prev, curr)) {
												this.ConnectedPlanPanel.Cursor = Cursors.No;
												break;
											}
											if (IsIntersecting(mouse, coordsPickedSoFar[0], prev, curr)) {
												this.ConnectedPlanPanel.Cursor = Cursors.No;
												break;
											}
										}
										prev = curr;
									}

									if (this.ConnectedPlanPanel.Cursor == Cursors.No) {
										break;
									}
									path.Dispose();
									prev = PointF.Empty;
									foreach (PointF curr in roomCoordinates) {
										if (prev != PointF.Empty) {
											if (IsIntersecting(mouse, start, prev, curr)) {
												this.ConnectedPlanPanel.Cursor = Cursors.No;
												break;
											}
											if (IsIntersecting(mouse, coordsPickedSoFar[0], prev, curr)) {
												this.ConnectedPlanPanel.Cursor = Cursors.No;
												break;
											}
										}
										prev = curr;
									}

									if (this.ConnectedPlanPanel.Cursor == Cursors.No) {
										break;
									}
								}
							}
						}
					}
				} else {
					this.ConnectedPlanPanel.Cursor = Cursors.No;
				}
				return true;
			} else if (this.Mode == RoomPickerMode.RPM_DEL_UNUSED) {
				this.ConnectedPlanPanel.Cursor = Cursors.No;
				foreach (List<PointF> unusedArea in unusedCoordinates) {
					GraphicsPath path = new GraphicsPath();
					path.StartFigure();
					PointF[] array = unusedArea.ToArray();
					path.AddPolygon(array);
					path.CloseFigure();
					if (path.IsVisible(new PointF((float)planPoint.X, (float)planPoint.Y))) {
						this.ConnectedPlanPanel.Cursor = Cursors.Hand;
						path.Dispose();
						break;
					}
					path.Dispose();
				}
			} else if (this.Mode == RoomPickerMode.RPM_PICK_ROOM) {
				return true;
			}
			return false;
		}

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.PointF screenPoint, MouseButtons button) {
			// TODO
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.PointF screenPoint, WW.Math.Point2D lastPlanPoint, System.Drawing.PointF lastScreenPoint, MouseButtons button) {
			// TODO
			return false;
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.PointF screenPoint, MouseButtons button) {
			// TODO
			return false;
		}
		#endregion

		private PointF GetNormalizedPoint(PointF basePoint, PointF currentPoint) {
			float xDistance = Math.Abs(basePoint.X - currentPoint.X);
			float yDistance = Math.Abs(basePoint.Y - currentPoint.Y);
			PointF p;
			if (xDistance < yDistance) {
				double distanceInMeter = (currentPoint.Y - basePoint.Y) / this.ConnectedPlanPanel.Plan.Measure.Value;
				distanceInMeter = Math.Round(distanceInMeter, 1);
				p = new PointF(basePoint.X, basePoint.Y + ((float)distanceInMeter * this.ConnectedPlanPanel.Plan.Measure.Value));
			} else {
				double distanceInMeter = (currentPoint.X - basePoint.X) / this.ConnectedPlanPanel.Plan.Measure.Value;
				distanceInMeter = Math.Round(distanceInMeter, 1);
				p = new PointF(basePoint.X + ((float)distanceInMeter * this.ConnectedPlanPanel.Plan.Measure.Value), basePoint.Y);
			}

			return p;
		}

		private bool IsIntersecting(PointF p1, PointF p2, PointF p3, PointF p4) {
			float x1, x2, x3, x4, y1, y2, y3, y4;
			float ua, ub, ud;
			//float x, y;
			x1 = p1.X; x2 = p2.X; x3 = p3.X; x4 = p4.X;
			y1 = p1.Y; y2 = p2.Y; y3 = p3.Y; y4 = p4.Y;
			ud = ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
			if (ud != 0) {
				ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ud;
				ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ud;
				if (IsBetween(ua, 0, 1) && IsBetween(ub, 0, 1)) {
					return true;
					//    x = x1 + ua * (x2 - x1);
					//    y = y1 + ua * (y2 - y1);
				}
			}
			return false;
		}

		private bool IsBetween(float value, float min, float max) {
			if (value >= min && value <= max) return true;
			return false;
		}
	}
}
