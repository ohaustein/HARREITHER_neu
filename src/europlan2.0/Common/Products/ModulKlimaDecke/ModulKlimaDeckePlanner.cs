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
using WW.Cad.Model.Tables;
using WW.Cad.Model.Entities;

namespace Europlan.Common {
	public partial class ModulKlimaDeckePlanner : Component, IPlanner, IProductPlanner {

		public enum KlimaDeckeMode {
			KDM_NONE,
			KDM_CONSTRUCTION,
			KDM_LAYOUT_ADD_AREA,
			KDM_PICK_MODULE,
			KDM_ADD_CONNECTION,
			KDM_DEL_CONNECTION,
            KDM_BEPLANKUNG
		}

		public ModulKlimaDeckePlanner() {
			InitializeComponent();
		}

		public ModulKlimaDeckePlanner(IContainer container) {
			container.Add(this);

			InitializeComponent();
		}

		private ModulKlimaDeckeProduct product;
		private KlimaDeckeMode mode = KlimaDeckeMode.KDM_NONE;
		private Cursor customCursor = null;
		private bool automaticOrientation = true;
		private bool automaticRows = true;
		private bool drawBeplankung = true;
		private bool highlightRoomCoordinates = true;

		private List<Point2D> newConnectionVertices = null;
		private List<Point2D> nextConnectionPoints = new List<Point2D>();

		private Nullable<NewConnectionStartData> newConnectionStartData = null;
		private List<PossibleConnectionPoint> possibleConnectionPoints = new List<PossibleConnectionPoint>();

		private List<KlimaFlaechenModulVerbindung> updateStartOnMove = new List<KlimaFlaechenModulVerbindung>();
		private List<KlimaFlaechenModulVerbindung> updateEndOnMove = new List<KlimaFlaechenModulVerbindung>();
		private List<KlimaFlaechenModulVerbindung> deleteOnMove = new List<KlimaFlaechenModulVerbindung>();
		private List<KlimaFlaechenSubAreaVerbindung> deleteSaOnMove = new List<KlimaFlaechenSubAreaVerbindung>();

		private Color[] circuitColors = new Color[] {
			Color.FromArgb(192, 0, 0),
			Color.FromArgb(0, 128, 0),
			Color.FromArgb(0, 0, 192),
			Color.FromArgb(255, 128, 0),
			Color.FromArgb(128, 128, 0),
			Color.FromArgb(0, 128, 255)
		};

		public class UpdateNewCountArgs : EventArgs {
			public int count;

			public UpdateNewCountArgs(int count) {
				this.count = count;
			}
		}

		public event EventHandler<UpdateNewCountArgs> UpdateNewCount;

		public bool AutomaticOrientation {
			get { return this.automaticOrientation; }
			set { this.automaticOrientation = value; }
		}

		public bool AutomaticRows {
			get { return this.automaticRows; }
			set { this.automaticRows = value; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModulKlimaDeckeProduct Product {
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
				this.connectionDrawer.Product = this.product;
				//this.connectionDrawer.Floor = (this.product != null && this.product.AssociatedRoom != null) ? this.product.AssociatedRoom.AssociatedFloor : null;
			}
		}

		public KlimaDeckeMode Mode {
			get { return this.mode; }
			set {
				this.mode = value;
				if (this.mode != KlimaDeckeMode.KDM_PICK_MODULE) {
					this.HighlightModules = null;
				}
				if (this.mode != KlimaDeckeMode.KDM_ADD_CONNECTION) {
					//this.possibleAnbindungspunkte = new List<GraphicalConnectionAnbindungsPunkt>();
					this.possibleConnectionPoints = new List<PossibleConnectionPoint>();
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
			if (this.mode == KlimaDeckeMode.KDM_PICK_MODULE) {
				//KeyDown(e.KeyCode, this.highlightModules);
			} else if (this.mode == KlimaDeckeMode.KDM_ADD_CONNECTION) {
				if (e.KeyCode == Keys.Escape) {
					//this.newConnectionStart = null;
					this.newConnectionStartData = null;
					this.newConnectionVertices = null;
				} else if (e.KeyCode == Keys.Back) {
					if (this.newConnectionVertices == null || this.newConnectionVertices.Count < 2) {
						//this.newConnectionStart = null;
						this.newConnectionStartData = null;
						this.newConnectionVertices = null;
					} else {
						this.newConnectionVertices.RemoveAt(this.newConnectionVertices.Count - 1);
					}
					if (this.connectedPlanPanel != null) {
						this.connectedPlanPanel.InvalidateGraphics();
					}
				}
			}
		}

		private bool KeyDown(Keys key, List<KlimaFlaechenModul> modules) {
			if (key == Keys.Delete && modules != null) {
				if (modules.Count > 0 && this.product.Connections != null && this.product.Connections.Count > 0) {
					bool ask = false;
					foreach (ModulDeckeCircuit c in this.product.PlannedCircuits) {
						bool empty = true;
						foreach (ModulDeckeSubArea sa in c.SubAreas) {
							foreach (KlimaFlaechenList row in sa.Rows) {
								foreach (KlimaFlaechenModul m in row.List) {
									if (!modules.Contains(m)) {
										empty = false;
										break;
									}
								}
								if (!empty) {
									break;
								}
							}
							if (!empty) {
								break;
							}
						}
						if (empty) {
							ask = true;
							break;
						}
					}
					if (ask) {
						if (MessageBox.Show(EuroplanRes.ModulKlimaBodenPlanner_HeizkreisLoeschenText, EuroplanRes.ModulKlimaBodenPlanner_HeizkreisLoeschenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
							return true;
						}
						this.product.Connections.Clear();
					}
				}
				List<KlimaFlaechenList> emptyRows = new List<KlimaFlaechenList>();
				List<ModulDeckeSubArea> emptySubAreas = new List<ModulDeckeSubArea>();
				List<Circuit> emptyCircuits = new List<Circuit>();
				foreach (Circuit c in this.product.PlannedCircuits) {
					ModulDeckeCircuit dc = c as ModulDeckeCircuit;
					foreach (ModulDeckeSubArea sa in dc.SubAreas) {
						foreach (KlimaFlaechenList kfl in sa.Rows) {
							foreach (KlimaFlaechenModul kfm in modules) {
								if (kfl.List.Contains(kfm)) {
									KlimaFlaechenModulVerbindung link = kfm.GetInputLink(c, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis);
									if (link != null) {
										kfl.Links.Remove(link);
									}
									link = kfm.GetOutputLink(c, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis);
									if (link != null) {
										kfl.Links.Remove(link);
									}
									KlimaFlaechenSubAreaVerbindung saLink = kfm.GetSubareaInputLink(c, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis);
									if (saLink != null) {
										dc.Links.Remove(saLink);
									}
									saLink = kfm.GetSubareaOutputLink(c, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis);
									if (saLink != null) {
										dc.Links.Remove(saLink);
									}
									kfl.List.Remove(kfm);
								}
							}
							if (kfl.List.Count == 0) {
								emptyRows.Add(kfl);
							}
						}
						foreach (KlimaFlaechenList emptyRow in emptyRows) {
							sa.Rows.Remove(emptyRow);
						}
						emptyRows.Clear();
						if (sa.Rows.Count == 0) {
							emptySubAreas.Add(sa);
						}
					}
					foreach (ModulDeckeSubArea emptySubArea in emptySubAreas) {
						dc.SubAreas.Remove(emptySubArea);
					}
					emptySubAreas.Clear();
					if (dc.SubAreas.Count == 0) {
						emptyCircuits.Add(dc);
					}
				}
				foreach (Circuit emptyCircuit in emptyCircuits) {
					this.product.PlannedCircuits.Remove(emptyCircuit);
				}
				if (this.projectChanged != null) {
					this.projectChanged(this);
				}
				this.ModuleSelected(this, new ModuleSelectedEventArgs());
				this.ConnectedPlanPanel.InvalidateGraphics();
				if (this.ListsNeedUpdate != null) {
					this.ListsNeedUpdate(this, new ListNeedsUpdateEventArgs(false));
				}
				return true;
			}
			return false;
		}

		public class ListNeedsUpdateEventArgs : EventArgs {
			public bool selectLastCircuit = false;

			public ListNeedsUpdateEventArgs() {
			}

			public ListNeedsUpdateEventArgs(bool selectLastCircuit) {
				this.selectLastCircuit = selectLastCircuit;
			}
		}
		public event EventHandler<ListNeedsUpdateEventArgs> ListsNeedUpdate;

		public delegate void AddModuleDelegate(ref double y, double start, double end, double step, ref bool left, Matrix3D invRotation, PossibleModulLane lane, Point2D borderLeftOrigin, out bool added, out KlimaFlaechenModul addedModule, out KlimaFlaechenList rowOfAddedModul, KlimaFlaechenModul lastAddedModul, KlimaFlaechenList rowOfLastAddedModul);
			//Graphics g, Matrix4D additionalTransformation, Matrix3D invRotation, double step, ref bool left, PossibleModulLane lane, Point2D borderLeftOrigin, ref double y, bool bottomUp, double start, double end);

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			this.PaintAfterPlanPannel(e.Graphics, additionalTransformation, mousePositionInPlan, mousePositionInControl, false);
		}

		public void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl, bool export) {
			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.CeilingCoordinatesToUse != null) {

				if (!export) {
					// paint anbindeleitungen and distributors
					this.connectionDrawer.Paint(g, additionalTransformation);
				}

				// generate clip for product
				GraphicsPath path = new GraphicsPath();
				List<PointF> transformedPoints = new List<PointF>();
				foreach (Point2D point in this.product.AssociatedRoom.CeilingCoordinatesToUse) {
					Point2D tmp = additionalTransformation.TransformTo2D(point);
					transformedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
				}
				path.AddPolygon(transformedPoints.ToArray());
				Region clipDisabled = new Region();
				clipDisabled.MakeInfinite();
				clipDisabled.Exclude(path);
				path.Dispose();

				// select color for painting depending depending on the background color of the plan
				Color c = Color.Black;
				if (this.ConnectedPlanPanel != null && this.ConnectedPlanPanel.ColorMode == ColorMode.CM_BLACK_BG) {
					c = Color.White;
				}
				Brush b = new SolidBrush(c);
				b = new HatchBrush(System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal, Color.FromArgb(128, c), Color.FromArgb(112, c));

				if (highlightRoomCoordinates && !export) {
					// gray out all except the room
					g.FillRegion(b, clipDisabled);
				}

				// paint construction
				if (this.product.AssociatedRoom.AssociatedPlan != null && this.product.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
					if (this.product.GraphConstruction != null) {
						this.product.GraphConstruction.Paint(g, this.Mode, this.drawBeplankung);
					}
				}

				if (this.mode != KlimaDeckeMode.KDM_CONSTRUCTION) {
					foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
						Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
						Matrix3D invRotation = rotation.GetInverse();

						double left = rotation.Transform(lane.BorderLeft.Origin).X;

						List<KlimaFlaechenModulWithRowAndCircuit> modules = this.product.GetModulesInLaneWithRowAndCircuit(lane.Nr);
						//List<KlimaFlaechenModul> modules = lane.GetModulesInThisLane(this.product);
						foreach (KlimaFlaechenModulWithRowAndCircuit mrc in modules) {
							bool highlight = false;
							if (this.highlightCircuit != null && this.highlightCircuit.ContainsModul(mrc.modul)) {
								highlight = true;
							}
							if (this.highlightSubArea != null && this.highlightSubArea.ContainsModul(mrc.modul)) {
								highlight = true;
							}
							if (this.highlightRow != null && this.highlightRow.ContainsModul(mrc.modul)) {
								highlight = true;
							}
							if (this.highlightModules != null && this.highlightModules.Contains(mrc.modul)) {
								highlight = true;
							}
							this.DrawModule(mrc.modul.ModulType, mrc.modul.Orientation, invRotation.Transform(new Point2D(left, mrc.modul.GraphPositionInLan)), additionalTransformation, g, mrc.modul.GraphBottomUp, highlight, mrc.circuit.CircuitColor, /*mrc.modul == this.hoveredModul && this.hoverInput*/ false, /*mrc.modul == this.hoveredModul && this.hoverOutput*/ false);
						}
					}
					double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					foreach (ModulDeckeCircuit circuit in this.product.PlannedCircuits) {
						if (circuit.Links != null) {
							foreach (KlimaFlaechenSubAreaVerbindung saLink in circuit.Links) {
								saLink.Draw(g, additionalTransformation, circuit.CircuitColor, measure);
							}
						}
						foreach (ModulDeckeSubArea sa in circuit.SubAreas) {
							foreach (KlimaFlaechenList row in sa.Rows) {
								if (row.Links != null) {
									foreach (KlimaFlaechenModulVerbindung link in row.Links) {
										link.Draw(g, additionalTransformation, circuit.CircuitColor, measure);
									}
								}
							}
						}
					}
				}

				if (this.layoutAddArea != null) {
					PointF[] drawArea = new PointF[this.layoutAddArea.Count];
					for (int i = 0; i < this.layoutAddArea.Count; i++) {
						Point2D tmp = additionalTransformation.TransformTo2D(this.layoutAddArea[i]);
						drawArea[i] = new PointF((float)tmp.X, (float)tmp.Y);
					}
					g.DrawPolygon(Pens.Red, drawArea);

					int count = 0;
					this.AddModulesForLayoutArea(delegate(ref double y, double start, double end, double step, ref bool left, Matrix3D invRotation, PossibleModulLane lane, Point2D borderLeftOrigin, out bool added, out KlimaFlaechenModul addedModul, out KlimaFlaechenList rowOfAddedModul, KlimaFlaechenModul lastAddedModul, KlimaFlaechenList rowOfLastAddedModul) {
						addedModul = null;
						rowOfAddedModul = null;
						added = this.TryDrawModule(g, additionalTransformation, ref y, start, end, step, ref left, this.layoutAddAreaBottomUp, invRotation, lane, borderLeftOrigin);
						if (added) {
							count++;
						}
					});
					if (this.UpdateNewCount != null) {
						this.UpdateNewCount(this, new UpdateNewCountArgs(count));
					}
				}

				// paint unused areas
				if (this.product.AssociatedRoom.CeilingUnusedAreaCoordinates != null) {
					List<PointF> unusedPoints = new List<PointF>();
					foreach (List<Point2D> unusedArea in this.product.AssociatedRoom.CeilingUnusedAreaCoordinates) {
						foreach (Point2D point in unusedArea) {
							Point2D tmp = additionalTransformation.TransformTo2D(point);
							unusedPoints.Add(new PointF((float)tmp.X, (float)tmp.Y));
						}
						PointF[] pointArray = unusedPoints.ToArray();
						g.DrawPolygon(new Pen(c), pointArray);
						g.FillPolygon(b, pointArray);
						unusedPoints.Clear();
					}
				}

				if (this.dragStartedInPlan.HasValue && this.dragEndedInPlan.HasValue) {
					Matrix3D rotation = Transformation3D.Rotate(this.Product.AssociatedRoom.AssociatedPlan.Rotation * Math.PI / 180.0);
					Point2D rotatedStart = rotation.Transform(this.dragStartedInPlan.Value);
					Point2D rotatedEnd = rotation.Transform(this.dragEndedInPlan.Value);
					Matrix3D invRotation = rotation.GetInverse();
					Polygon2D selectedPoly = new Polygon2D();
					selectedPoly.Add(this.dragStartedInPlan.Value);
					selectedPoly.Add(invRotation.Transform(new Point2D(rotatedStart.X, rotatedEnd.Y)));
					selectedPoly.Add(this.dragEndedInPlan.Value);
					selectedPoly.Add(invRotation.Transform(new Point2D(rotatedEnd.X, rotatedStart.Y)));

					PointF[] arr = new PointF[selectedPoly.Count];
					int i = 0;
					foreach (Point2D point in selectedPoly) {
						Point2D tmp = additionalTransformation.TransformTo2D(new Point3D(point, 0));
						arr[i] = new PointF((float)tmp.X, (float)tmp.Y);
						i++;
					}
					g.DrawPolygon(new Pen(Color.Red), arr);
				}

				// draw connection that is currently beeing added
				if (this.mode == KlimaDeckeMode.KDM_ADD_CONNECTION) {
					if (/*hoverAnbindungen && */this.possibleConnectionPoints != null) {
						g.ResetClip();
						Color neutralColor = this.product.AssociatedRoom.AssociatedPlan is CadPlan ? Color.White : Color.Black;
						Brush bi = new SolidBrush(Color.FromArgb(127, Color.Red));
						Brush bo = new SolidBrush(Color.FromArgb(127, Color.Blue));
						Brush bn = new SolidBrush(Color.FromArgb(127, neutralColor));
						/*if (!this.newConnectionStartAtOutput) {
							foreach (GraphicalConnectionAnbindungsPunkt anbindung in this.possibleAnbindungspunkte) {
								PointF[] points = new PointF[anbindung.Area.Count];
								for (int i = 0; i < anbindung.Area.Count; i++) {
									Point2D tmp = additionalTransformation.TransformTo2D(anbindung.Area[i]);
									points[i] = new PointF((float)tmp.X, (float)tmp.Y);
								}
								g.FillPolygon(bi, points);
								g.DrawPolygon(Pens.Red, points);
							}
						} else {
							foreach (GraphicalConnectionAnbindungsPunkt anbindung in this.possibleAnbindungspunkte) {
								PointF[] points = new PointF[anbindung.Area.Count];
								for (int i = 0; i < anbindung.Area.Count; i++) {
									Point2D tmp = additionalTransformation.TransformTo2D(anbindung.Area[i]);
									points[i] = new PointF((float)tmp.X, (float)tmp.Y);
								}
								g.FillPolygon(bo, points);
								g.DrawPolygon(Pens.Blue, points);
							}
						}*/
						foreach (PossibleConnectionPoint point in this.possibleConnectionPoints) {
							PointF[] points = new PointF[point.ConnectionArea.Count];
							for (int i = 0; i < point.ConnectionArea.Count; i++) {
								Point2D tmp = additionalTransformation.TransformTo2D(point.ConnectionArea[i]);
								points[i] = new PointF((float)tmp.X, (float)tmp.Y);
							}
							g.FillPolygon(point.Vorlauf ? bi : (point.Ruecklauf ? bo : bn), points);
							g.DrawPolygon(point.Vorlauf ? Pens.Red : (point.Ruecklauf ? Pens.Blue : new Pen(neutralColor)), points);
						}
					}
					/*if (this.possibleConnectionPoints != null) {
						foreach (PossibleConnectionPoint conn in this.possibleConnectionPoints) {

						}
					}*/
					Pen p = new Pen(Color.Green, (float)(0.021 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * additionalTransformation.M00));
					if (this.newConnectionVertices != null && this.newConnectionVertices.Count > 0) {
						Point2D newVertex2D;
						Point2D oldVertex2D = additionalTransformation.TransformTo2D(this.newConnectionVertices[0]);
						PointF newVertex;
						PointF oldVertex = new PointF((float)oldVertex2D.X, (float)oldVertex2D.Y);
						p.StartCap = System.Drawing.Drawing2D.LineCap.Flat;
						p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
						Point2D vertex;
						int countNew = this.newConnectionVertices.Count;
						int countNext = this.nextConnectionPoints.Count;
						int countSum = countNew + countNext;
						for (int i = 1; i < countSum; i++) {
							vertex = i < countNew ? this.newConnectionVertices[i] : this.nextConnectionPoints[i - countNew];
							newVertex2D = additionalTransformation.TransformTo2D(vertex);
							newVertex = new PointF((float)newVertex2D.X, (float)newVertex2D.Y);
							if (i == 2) {
								p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
							}
							if (i == countSum - 1) {
								p.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
							}
							g.DrawLine(p, oldVertex, newVertex);
							oldVertex = newVertex;
						}
					}
				}
			}
		}

		private int AddModulesForLayoutArea(AddModuleDelegate doIt) {
			Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
			Matrix3D invRotation = rotation.GetInverse();

			bool added = false;
			int count = 0;
			// TODO enable moduleHeightTolerance to avoid problems with rounding
			double moduleHeightTolerance = 0.0001 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			//double moduleHeightTolerance = 0;

			KlimaFlaechenModul lastAddedModul = null;
			KlimaFlaechenModul addedModul = null;
			KlimaFlaechenList rowOfLastAddedModul = null;
			KlimaFlaechenList rowOfAddedModul = null;
			if (alignRectangle) {
				// rectangle aligned to schienen
				Segment2D topSeg = new Segment2D(rotation.Transform(this.layoutAddArea[0]), rotation.Transform(this.layoutAddArea[3]));
				double start = topSeg.Start.Y;
				double end = rotation.Transform(this.layoutAddArea[1]).Y;
				double step = KlimaFlaechenModul.GetModuleHeight(this.moduleTypeToAdd) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				bool left;
				int newRows = 0;
				foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
					int laneCount = 0;
					left = this.startingOrientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
					Point2D borderLeftOrigin = rotation.Transform(lane.BorderLeft.Origin);
					borderLeftOrigin.Y = topSeg.Start.Y;
					Point2D borderRighOrigin = rotation.Transform(lane.BorderRight.Origin);
					borderLeftOrigin.Y = topSeg.Start.Y;
					if (Line2D.Intersects(new Line2D(borderLeftOrigin, new Vector2D(0, 1)), topSeg) && Line2D.Intersects(new Line2D(borderRighOrigin, new Vector2D(0, 1)), topSeg)) {
						if (this.layoutAddAreaBottomUp) {
							for (double y = start - step; y > end; y -= step) {
								doIt(ref y, start, end, step - moduleHeightTolerance, ref left, invRotation, lane, borderLeftOrigin, out added, out addedModul, out rowOfAddedModul, lastAddedModul, rowOfLastAddedModul);
								if (added) {
									lastAddedModul = addedModul;
									rowOfLastAddedModul = rowOfAddedModul;
									if (laneCount == 0) {
										newRows++;
									}
									count++;
									laneCount++;
									if (laneCount >= ModulKlimaDeckeProduct.ConfigMaxModulesInRow) {
										break;
									}
								}
							}
						} else {
							for (double y = start; y < end - step; y += step) {
								doIt(ref y, start, end, step - moduleHeightTolerance, ref left, invRotation, lane, borderLeftOrigin, out added, out addedModul, out rowOfAddedModul, lastAddedModul, rowOfLastAddedModul);
								if (added) {
									lastAddedModul = addedModul;
									rowOfLastAddedModul = rowOfAddedModul;
									if (laneCount == 0) {
										newRows++;
									}
									count++;
									laneCount++;
									if (laneCount >= ModulKlimaDeckeProduct.ConfigMaxModulesInRow) {
										break;
									}
								}
							}
						}
					}
					if (newRows >= ModulKlimaDeckeProduct.ConfigMaxModulesInParallel) {
						break;
					}
				}
			} else {
				// rectangle not aligned to schienen
				// the optimalLayout flag is ignored in this mode as this will always be layouted optimal
				Polygon2D rotatedLayoutAddArea = new Polygon2D();
				foreach (Point2D p in this.layoutAddArea) {
					rotatedLayoutAddArea.Add(rotation.Transform(p));
				}

				double step = KlimaFlaechenModul.GetModuleHeight(this.moduleTypeToAdd) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				bool left;
				foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
					left = this.startingOrientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
					Nullable<Point2D> topLeftIntersection = null;
					Nullable<Point2D> bottomLeftIntersection = null;
					
					Point2D borderLeftOrigin = rotation.Transform(lane.BorderLeft.Origin);

					if (this.GetIntersections(out topLeftIntersection, out bottomLeftIntersection, new Line2D(borderLeftOrigin, new Vector2D(0, 1)), rotatedLayoutAddArea /*, out bottomUpLeft*/)) {
						Point2D borderRightOrigin = rotation.Transform(lane.BorderRight.Origin);

						Nullable<Point2D> topRightIntersection = null;
						Nullable<Point2D> bottomRightIntersection = null;
						if (this.GetIntersections(out topRightIntersection, out bottomRightIntersection, new Line2D(borderRightOrigin, new Vector2D(0, 1)), rotatedLayoutAddArea /*, out bottomUpRight*/)) {
							double top = Math.Max(topLeftIntersection.Value.Y, topRightIntersection.Value.Y);
							double bottom = Math.Min(bottomLeftIntersection.Value.Y, bottomRightIntersection.Value.Y);
							if (top > bottom) {
								continue;
							}
							if (this.layoutAddAreaBottomUp) {
								for (double y = bottom - step; y > top; y -= step) {
									doIt(ref y, top, bottom, step - moduleHeightTolerance, ref left, invRotation, lane, borderLeftOrigin, out added, out addedModul, out rowOfAddedModul, lastAddedModul, rowOfLastAddedModul);
									if (added) {
										lastAddedModul = addedModul;
										rowOfLastAddedModul = rowOfAddedModul;
										count++;
									}
								}
							} else {
								for (double y = top; y < bottom - step; y += step) {
									doIt(ref y, top, bottom, step - moduleHeightTolerance, ref left, invRotation, lane, borderLeftOrigin, out added, out addedModul, out rowOfAddedModul, lastAddedModul, rowOfLastAddedModul);
									if (added) {
										lastAddedModul = addedModul;
										rowOfLastAddedModul = rowOfAddedModul;
										count++;
									}
								}
							}
						}

					}
				}
			}
			return count;
		}

		/// <summary>
		/// This method only is guaranteed to work if the passed poylgon really is a rectangle. this will not be checked inside this method
		/// </summary>
		/// <param name="topIntersection"></param>
		/// <param name="bottomIntersection"></param>
		/// <param name="line"></param>
		/// <param name="polygon"></param>
		private bool GetIntersections(out Nullable<Point2D> topIntersection, out Nullable<Point2D> bottomIntersection, Line2D line, Polygon2D  rectangle/*Point2D p1, Point2D p2, Point2D p3, Point2D p4, out bool bottomUp*/) {
			List<Segment2D> segments = new List<Segment2D>();
			Polygon2D.GetSegments(rectangle, segments);
			topIntersection = null;
			bottomIntersection = null;
			foreach (Segment2D segment in segments) {
				Nullable<Point2D> intersection = Line2D.GetIntersection(line, segment);
				if (intersection.HasValue && (!topIntersection.HasValue || intersection.Value.Y < topIntersection.Value.Y)) {
					topIntersection = intersection;
				}
				if (intersection.HasValue && (!bottomIntersection.HasValue ||intersection.Value.Y > bottomIntersection.Value.Y)) {
					bottomIntersection = intersection;
				}
			}
			if (!topIntersection.HasValue || !bottomIntersection.HasValue || topIntersection.Equals(bottomIntersection)) {
				topIntersection = null;
				bottomIntersection = null;
			}

			return topIntersection != null;
		}

		private ModulDeckeCircuit highlightCircuit = null;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModulDeckeCircuit HighlightCircuit {
			get { return this.highlightCircuit; }
			set {
				this.highlightCircuit = value;
				this.highlightSubArea = null;
				this.highlightRow = null;
				this.highlightModules = null;
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		private ModulDeckeSubArea highlightSubArea = null;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModulDeckeSubArea HighlightSubArea {
			get { return this.highlightSubArea; }
			set {
				this.highlightSubArea = value;
				this.highlightCircuit = null;
				this.highlightRow = null;
				this.highlightModules = null;
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		private KlimaFlaechenList highlightRow = null;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public KlimaFlaechenList HighlightRow {
			get { return this.highlightRow; }
			set {
				this.highlightRow = value;
				this.highlightCircuit = null;
				this.highlightSubArea = null;
				this.highlightModules = null;
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		private List<KlimaFlaechenModul> highlightModules = null;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public List<KlimaFlaechenModul> HighlightModules {
			get { return this.highlightModules; }
			set {
				this.highlightRow = null;
				this.highlightCircuit = null;
				this.highlightSubArea = null;
				this.highlightModules = value;
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		public List<KlimaFlaechenModul> GetAllSelectedModules() {
			List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
			if (this.HighlightCircuit != null) {
				foreach (ModulDeckeSubArea subArea in this.HighlightCircuit.SubAreas) {
					foreach (KlimaFlaechenList row in subArea.Rows) {
						foreach (KlimaFlaechenModul modul in row.List) {
							modules.Add(modul);
						}
					}
				}
			} else if (this.HighlightSubArea != null) {
				foreach (KlimaFlaechenList row in this.HighlightSubArea.Rows) {
					foreach (KlimaFlaechenModul modul in row.List) {
						modules.Add(modul);
					}
				}
			} else if (this.HighlightRow != null) {
				foreach (KlimaFlaechenModul modul in this.HighlightRow.List) {
					modules.Add(modul);
				}
			} else if (this.HighlightModules != null) {
				foreach (KlimaFlaechenModul modul in this.HighlightModules) {
					modules.Add(modul);
				}
			}
			return modules;
		}

		private bool TryDrawModule(Graphics g, Matrix4D additionalTransformation, ref double y, double start, double end, double step, ref bool left, bool bottomUp, Matrix3D invRotation, PossibleModulLane lane, Point2D borderLeftOrigin) {
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			foreach (FreeModulLaneArea area in lane.GetFreeAreas(this.product, null)) {
				if (!this.alignRectangle || this.optimalLayout) {
					Nullable<double> bestStart = area.BestStart(y, y + step, bottomUp, measure * 0.0001);
					if (bestStart.HasValue) {
						y = bestStart.Value;
						double upperBorder = Math.Min(start, end);
						double lowerBorder = Math.Max(start, end);
						if (bestStart.Value >= upperBorder && bestStart.Value + step <= lowerBorder) {
							Point2D tmp = invRotation.Transform(new Point2D(borderLeftOrigin.X, y));
							this.DrawModule(this.moduleTypeToAdd, null /*left ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT*/, tmp, additionalTransformation, g, false, true, Color.FromArgb(0, 240, 0), false, false);
							left = !left;
							return true;
						}
						break;
					}
				} else {
					if (area.Fits(y, y + step)) {
						Point2D tmp = invRotation.Transform(new Point2D(borderLeftOrigin.X, y));
						this.DrawModule(this.moduleTypeToAdd, null /*left ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT*/, tmp, additionalTransformation, g, false, true, Color.FromArgb(0, 240, 0), false, false);
						left = !left;
						return true;
					}
				}
			}
			return false;
		}

		private bool TryAddModule(Matrix4D additionalTransformation, ref double y, double start, double end, double step, ref bool left, bool bottomUp, Matrix3D invRotation, PossibleModulLane lane, Point2D borderLeftOrigin, Dictionary<PossibleModulLane, KlimaFlaechenList> laneToRowMapping, ModulDeckeSubArea subArea, KlimaFlaechenList row, List<KlimaFlaechenModul> modulesAdded, bool tryToFindRow, bool onlyAddToExistingHks, out KlimaFlaechenModul addedModul, out KlimaFlaechenList rowOfAddedModul, KlimaFlaechenModul lastAddedModul, KlimaFlaechenList rowOfLastAddedModul, ModulDeckeCircuit circuitToAdd) {
			bool invertDirection = false;
			if (this.connectedPlanPanel != null && this.connectedPlanPanel.Plan != null) {
				if (!this.connectedPlanPanel.Plan.InvertYAxis) {
					invertDirection = true;
					//bottomUp = !bottomUp;
				}
			}
			addedModul = lastAddedModul;
			rowOfAddedModul = rowOfLastAddedModul;
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			foreach (FreeModulLaneArea area in lane.GetFreeAreas(this.product, null)) {
				if (!this.alignRectangle || this.optimalLayout) {
					//if ((area.Bottom - area.Top) / this.product.AssociatedRoom.AssociatedPlan.Measure.Value >= KlimaFlaechenModul.GetModuleHeight(moduleTypeToAdd)) {
						Nullable<double> bestStart = area.BestStart(y, y + step, bottomUp, measure * 0.0001);
						if (bestStart.HasValue) {
							y = bestStart.Value;
							double upperBorder = Math.Min(start, end);
							double lowerBorder = Math.Max(start, end);
							if (bestStart.Value >= upperBorder && bestStart.Value + step <= lowerBorder) {
								KlimaFlaechenModul modul = new KlimaFlaechenModul(moduleTypeToAdd, left ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
								modul.GraphLane = lane.Nr;
								modul.GraphPositionInLan = bestStart.Value;
								modul.GraphBottomUp = invertDirection ? !bottomUp : bottomUp;
								KlimaFlaechenList usedRow = null;
								if (tryToFindRow) {
									List<KlimaFlaechenModulWithRowAndCircuit> modulesInLane = this.product.GetModulesInLaneWithRowAndCircuit(lane.Nr);
									//modulesInLane.Sort(new KlimaFlaechenModuleComparer(true));
									double bestPosBefore = double.MinValue;
									double bestPosAfter = double.MaxValue;
									foreach (KlimaFlaechenModulWithRowAndCircuit modulInLane in modulesInLane) {
										if (modulInLane.modul.GraphPositionInLan < modul.GraphPositionInLan && modulInLane.modul.GraphPositionInLan > bestPosBefore) {
											bestPosBefore = modulInLane.modul.GraphPositionInLan;
											usedRow = modulInLane.row;
											circuitToAdd = modulInLane.circuit;
										}
										if (usedRow == null && modulInLane.modul.GraphPositionInLan > modul.GraphPositionInLan && modulInLane.modul.GraphPositionInLan < bestPosAfter) {
											bestPosAfter = modulInLane.modul.GraphPositionInLan;
											usedRow = modulInLane.row;
											circuitToAdd = modulInLane.circuit;
										}
									}
								} else if (row != null) {
									usedRow = row;
								}
								if (usedRow != null || !onlyAddToExistingHks) {
									if (usedRow == null) {
										if (laneToRowMapping.ContainsKey(lane)) {
											usedRow = laneToRowMapping[lane];
										} else {
											usedRow = new KlimaFlaechenList();
											subArea.Rows.Add(usedRow);
											laneToRowMapping.Add(lane, usedRow);
										}
									}
									usedRow.List.Add(modul);
									modulesAdded.Add(modul);
									addedModul = modul;
									rowOfAddedModul = usedRow;
									if (moduleTypeToAdd != KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 && moduleTypeToAdd != KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) {
										left = !left;
									}
									if (addedModul != lastAddedModul && rowOfAddedModul == rowOfLastAddedModul) {
										Point2D output1 = lastAddedModul.GetOutputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);
										Point2D input1 = addedModul.GetInputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);

										Point2D output2 = addedModul.GetOutputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);
										Point2D input2 = lastAddedModul.GetInputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);

										if ((output1 - input1).GetLength() <= (output2 - input2).GetLength()) {
											if (rowOfAddedModul.Links == null) {
												rowOfAddedModul.Links = new List<KlimaFlaechenModulVerbindung>();
											}
											rowOfAddedModul.Links.Add(new KlimaFlaechenModulVerbindung(lastAddedModul, addedModul, new Point2D[] { output1, input1 }, circuitToAdd, Project.Instance.GetPlannedProduct(this.product)));
										} else {
											if (rowOfAddedModul.Links == null) {
												rowOfAddedModul.Links = new List<KlimaFlaechenModulVerbindung>();
											}
											rowOfAddedModul.Links.Add(new KlimaFlaechenModulVerbindung(addedModul, lastAddedModul, new Point2D[] { output2, input2 }, circuitToAdd, Project.Instance.GetPlannedProduct(this.product)));
										}
									}
									return true;
								} else {
									return false;
								}
							}
							break;
						}
					//}
				} else {
					if (area.Fits(y, y + step)) {
						KlimaFlaechenModul modul = new KlimaFlaechenModul(moduleTypeToAdd, left ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
						modul.GraphLane = lane.Nr;
						modul.GraphPositionInLan = y;
						modul.GraphBottomUp = invertDirection ? !bottomUp : bottomUp;
						KlimaFlaechenList usedRow;
						if (row != null) {
							usedRow = row;
						} else if (laneToRowMapping.ContainsKey(lane)) {
							usedRow = laneToRowMapping[lane];
						} else {
							usedRow = new KlimaFlaechenList();
							subArea.Rows.Add(usedRow);
							laneToRowMapping.Add(lane, usedRow);
						}
						usedRow.List.Add(modul);
						addedModul = modul;
						rowOfAddedModul = usedRow;

						if (moduleTypeToAdd != KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 && moduleTypeToAdd != KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) {
							left = !left;
						}
						if (addedModul != lastAddedModul && rowOfAddedModul == rowOfLastAddedModul) {
							int tmp;
							ModulDeckeCircuit circuit = this.product.GetCircuitForModul(addedModul, out tmp);

							Point2D output1 = lastAddedModul.GetOutputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);
							Point2D input1 = addedModul.GetInputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);

							Point2D output2 = addedModul.GetOutputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);
							Point2D input2 = lastAddedModul.GetInputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);

							if ((output1 - input1).GetLength() <= (output2 - input2).GetLength()) {
								rowOfAddedModul.Links.Add(new KlimaFlaechenModulVerbindung(lastAddedModul, addedModul, new Point2D[] { output1, input1 }, circuit, Project.Instance.GetPlannedProduct(this.product)));
							} else {
								rowOfAddedModul.Links.Add(new KlimaFlaechenModulVerbindung(addedModul, lastAddedModul, new Point2D[] { output2, input2 }, circuit, Project.Instance.GetPlannedProduct(this.product)));
							}
						}
						return true;
					}
				}
			}
			return false;
		}

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			bool redraw = false;
			if (this.mode == KlimaDeckeMode.KDM_PICK_MODULE) {
				if (this.product == null || this.product.AssociatedRoom == null ||
					this.product.AssociatedRoom.AssociatedPlan == null || this.product.AssociatedRoom.AssociatedPlan.Measure == null) {
					return false;
				}
				Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
				Point2D rotatedPoint = rotation.Transform(planPoint);
				double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				bool moduleFound = false;
				foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
					Point2D left = rotation.Transform(lane.BorderLeft.Origin);
					Point2D right = rotation.Transform(lane.BorderRight.Origin);
					if (rotatedPoint.X >= left.X && rotatedPoint.X <= right.X) {
						List<KlimaFlaechenModul> modules = this.product.GetModulesInLane(lane.Nr);
						foreach (KlimaFlaechenModul module in modules) {
							if (rotatedPoint.Y >= module.GraphPositionInLan && rotatedPoint.Y <= module.GraphPositionInLan + KlimaFlaechenModul.GetModuleHeight(module.ModulType) * measure) {
								moduleFound = true;
								if (this.ShiftPressed) {
									if (this.HighlightModules == null) {
										this.HighlightModules = new List<KlimaFlaechenModul>();
									}
									if (this.HighlightModules.Contains(module)) {
										this.HighlightModules.Remove(module);
									} else {
										this.HighlightModules.Add(module);
									}
									//this.ConnectedPlanPanel.InvalidateGraphics();
									if (this.ModuleSelected != null) {
										this.ModuleSelected(this, new ModuleSelectedEventArgs(this.HighlightModules));
									}
								} else {
									//List<KlimaFlaechenModul> highlightedModules = new List<KlimaFlaechenModul>();
									//highlightedModules.Add(module);
									//this.HighlightModules = highlightedModules;
									if (this.HighlightModules == null) {
										this.HighlightModules = new List<KlimaFlaechenModul>();
									} else {
										this.HighlightModules.Clear();
									}
									this.HighlightModules.Add(module);
									if (this.ModuleSelected != null) {
										this.ModuleSelected(this, new ModuleSelectedEventArgs(module));
									}
								}
							}
						}
					}
				}
				if (!moduleFound && !this.ShiftPressed) {
					this.HighlightModules = null;
					if (this.ModuleSelected != null) {
						this.ModuleSelected(this, new ModuleSelectedEventArgs());
					}
					return true;
				} else if (moduleFound) {
					return true;
				}
			} else if (this.mode == KlimaDeckeMode.KDM_ADD_CONNECTION) {
				if (this.newConnectionStartData == null) {
					foreach (PossibleConnectionPoint connection in this.possibleConnectionPoints) {
						if (connection.ConnectionArea.IsInside(planPoint)) {
							switch (connection.ConnectionType) {
								case PossibleConnectionPointType.CONNECTION_MODULE:
									this.newConnectionStartData = new NewConnectionStartData(connection.Modul, connection.Ruecklauf, this.product);
									this.newConnectionVertices = new List<Point2D>();
									this.newConnectionVertices.Add(connection.ConnectionPoint);
									break;

								case PossibleConnectionPointType.CONNECTION_SUBAREA:
									this.newConnectionStartData = new NewConnectionStartData(connection.Verbindung, this.product);
									this.newConnectionVertices = new List<Point2D>();
									this.newConnectionVertices.Add(connection.ConnectionPoint);
									break;

								case PossibleConnectionPointType.CONNECTION_PRODUCT:
									this.newConnectionStartData = new NewConnectionStartData(connection.ProductConnection, connection.Vorlauf, this.product);
									this.newConnectionVertices = new List<Point2D>();
									this.newConnectionVertices.Add(connection.ConnectionPoint);
									//this.newConnectionStartData = new NewConnectionStartData(
									break;
							}
							break;
							/*if (this.newConnectionStartData.Value.StartAtOutput) {
								this.newConnectionVertices.Add(this.newConnectionStartData.Value.StartModul.GetOutputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product));
							} else {
								this.newConnectionVertices.Add(this.newConnectionStartData.Value.StartModul.GetInputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product));
							}*/
						}
					}
				} else {
					this.newConnectionVertices.AddRange(this.nextConnectionPoints);
					foreach (PossibleConnectionPoint connection in this.possibleConnectionPoints) {
						if (connection.ConnectionArea.IsInside(planPoint)) {
							int tmp;
							switch (connection.ConnectionType) {
								case PossibleConnectionPointType.CONNECTION_MODULE:
									if (this.newConnectionStartData.Value.StartsAtModul) {
										ModulDeckeSubArea sa = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(connection.Modul, out tmp);
										KlimaFlaechenList row = sa.GetRowForModul(connection.Modul, out tmp);
										if (row != this.newConnectionStartData.Value.RowOfStartModul) {
											if (this.newConnectionStartData.Value.CircuitOfStart.Links == null) {
												this.newConnectionStartData.Value.CircuitOfStart.Links = new List<KlimaFlaechenSubAreaVerbindung>();
											}
											if (this.newConnectionStartData.Value.StartAtOutput) {
												if (connection.Ruecklauf) {
													this.newConnectionStartData.Value.CircuitOfStart.Links.Add(new KlimaFlaechenSubAreaVerbindung(new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul, connection.Modul }, new KlimaFlaechenModul[] { }, new List<Point2D>[] { this.newConnectionVertices }, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product)));
												} else {
													this.newConnectionStartData.Value.CircuitOfStart.Links.Add(new KlimaFlaechenSubAreaVerbindung(new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul }, new KlimaFlaechenModul[] { connection.Modul }, new List<Point2D>[] { this.newConnectionVertices }, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product)));
												}
											} else {
												this.newConnectionVertices.Reverse();
												if (connection.Vorlauf) {
													this.newConnectionStartData.Value.CircuitOfStart.Links.Add(new KlimaFlaechenSubAreaVerbindung(new KlimaFlaechenModul[] { }, new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul, connection.Modul }, new List<Point2D>[] { this.newConnectionVertices }, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product)));
												} else {
													this.newConnectionStartData.Value.CircuitOfStart.Links.Add(new KlimaFlaechenSubAreaVerbindung(new KlimaFlaechenModul[] { connection.Modul }, new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul }, new List<Point2D>[] { this.newConnectionVertices }, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product)));
												}
											}
										} else {
											if (this.newConnectionStartData.Value.RowOfStartModul.Links == null) {
												this.newConnectionStartData.Value.RowOfStartModul.Links = new List<KlimaFlaechenModulVerbindung>();
											}
											if (this.newConnectionStartData.Value.StartAtOutput) {
												this.newConnectionStartData.Value.RowOfStartModul.Links.Add(new KlimaFlaechenModulVerbindung(this.newConnectionStartData.Value.StartModul, connection.Modul, this.newConnectionVertices, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product)));
											} else {
												this.newConnectionVertices.Reverse();
												this.newConnectionStartData.Value.RowOfStartModul.Links.Add(new KlimaFlaechenModulVerbindung(connection.Modul, this.newConnectionStartData.Value.StartModul, this.newConnectionVertices, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product)));
											}
										}
										this.newConnectionStartData = null;
										this.newConnectionVertices = null;
									} else if (this.newConnectionStartData.Value.StartsAtVerbindung) {
										if (connection.Vorlauf) {
											this.newConnectionStartData.Value.StartVerbindung.End.Add(connection.Modul);
											this.newConnectionVertices.Reverse();
											this.newConnectionStartData.Value.StartVerbindung.Vertices.Add(this.newConnectionVertices);
											this.newConnectionStartData = null;
											this.newConnectionVertices = null;
										} else if (connection.Ruecklauf) {
											this.newConnectionStartData.Value.StartVerbindung.Start.Add(connection.Modul);
											this.newConnectionStartData.Value.StartVerbindung.Vertices.Add(this.newConnectionVertices);
											this.newConnectionStartData = null;
											this.newConnectionVertices = null;
										}
									} else if (this.newConnectionStartData.Value.StartsAtAnbindung) {
										if (this.newConnectionStartData.Value.StartAnbindung.NewProductConnection != null) {
											if (this.product.Connections == null) {
												this.product.Connections = new List<GraphicalProductConnection>();
											}
											this.product.Connections.Add(this.newConnectionStartData.Value.StartAnbindung.NewProductConnection);
										}
										ModulDeckeCircuit c = this.product.GetCircuitForModul(connection.Modul, out tmp);
										if (c.Links == null) {
											c.Links = new List<KlimaFlaechenSubAreaVerbindung>();
										}

										if (!this.newConnectionStartData.Value.StartAtOutput) {
											c.Links.Add(new KlimaFlaechenSubAreaVerbindung(new List<KlimaFlaechenModul>(new KlimaFlaechenModul[] { connection.Modul }), null, new List<Point2D>[] { this.newConnectionVertices }, c, Project.Instance.GetPlannedProduct(this.product), this.newConnectionStartData.Value.StartAnbindung.Index));
										} else {
											c.Links.Add(new KlimaFlaechenSubAreaVerbindung(null, new List<KlimaFlaechenModul>(new KlimaFlaechenModul[] { connection.Modul }), new List<Point2D>[] { this.newConnectionVertices }, c, Project.Instance.GetPlannedProduct(this.product), this.newConnectionStartData.Value.StartAnbindung.Index));
										}
										this.newConnectionStartData = null;
										this.newConnectionVertices = null;
									}
									break;

								case PossibleConnectionPointType.CONNECTION_SUBAREA:
									if (this.newConnectionStartData.Value.StartsAtModul) {
										if (this.newConnectionStartData.Value.StartAtOutput) {
											connection.Verbindung.Start.Add(this.newConnectionStartData.Value.StartModul);
											connection.Verbindung.Vertices.Add(this.newConnectionVertices);
										} else {
											connection.Verbindung.End.Add(this.newConnectionStartData.Value.StartModul);
											this.newConnectionVertices.Reverse();
											connection.Verbindung.Vertices.Add(this.newConnectionVertices);
										}
										this.newConnectionStartData = null;
										this.newConnectionVertices = null;
									} else if (this.newConnectionStartData.Value.StartsAtVerbindung) {
										if (connection.Verbindung.Start != null) {
											this.newConnectionStartData.Value.StartVerbindung.Start.AddRange(connection.Verbindung.Start);
										}
										if (connection.Verbindung.End != null) {
											this.newConnectionStartData.Value.StartVerbindung.End.AddRange(connection.Verbindung.End);
										}
										this.newConnectionStartData.Value.StartVerbindung.Vertices.AddRange(connection.Verbindung.Vertices);
										this.newConnectionStartData.Value.StartVerbindung.Vertices.Add(this.newConnectionVertices);
										this.newConnectionStartData.Value.CircuitOfStart.Links.Remove(connection.Verbindung);
										this.newConnectionStartData = null;
										this.newConnectionVertices = null;
									} else if (this.newConnectionStartData.Value.StartsAtAnbindung) {
										if (this.newConnectionStartData.Value.StartAnbindung.NewProductConnection != null) {
											if (this.product.Connections == null) {
												this.product.Connections = new List<GraphicalProductConnection>();
											}
											this.product.Connections.Add(this.newConnectionStartData.Value.StartAnbindung.NewProductConnection);
										}

										if (connection.Verbindung.Start == null || connection.Verbindung.Start.Count == 0) {
											connection.Verbindung.DistributorIndex = this.newConnectionStartData.Value.StartAnbindung.Index;
											connection.Verbindung.Vertices.Add(this.newConnectionVertices);
											this.newConnectionStartData = null;
											this.newConnectionVertices = null;
										} else if (connection.Verbindung.End == null || connection.Verbindung.End.Count == 0) {
											connection.Verbindung.DistributorIndex = this.newConnectionStartData.Value.StartAnbindung.Index;
											connection.Verbindung.Vertices.Add(this.newConnectionVertices);
											this.newConnectionStartData = null;
											this.newConnectionVertices = null;
										}
									}
									break;

								case PossibleConnectionPointType.CONNECTION_PRODUCT:
									//connection.ProductConnection

									if (connection.ProductConnection.NewProductConnection != null) {
										if (this.product.Connections == null) {
											this.product.Connections = new List<GraphicalProductConnection>();
										}
										this.product.Connections.Add(connection.ProductConnection.NewProductConnection);
									}
									if (this.newConnectionStartData.Value.CircuitOfStart.Links == null) {
										this.newConnectionStartData.Value.CircuitOfStart.Links = new List<KlimaFlaechenSubAreaVerbindung>();
									}
									if (this.newConnectionStartData.Value.StartsAtModul) {
										//if (connection.Vorlauf) {
										if (this.newConnectionStartData.Value.StartAtOutput) {
											this.newConnectionStartData.Value.CircuitOfStart.Links.Add(new KlimaFlaechenSubAreaVerbindung(new List<KlimaFlaechenModul>(new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul }), null, new List<Point2D>[] { this.newConnectionVertices }, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product), connection.ProductConnection.Index));
										} else {
											this.newConnectionStartData.Value.CircuitOfStart.Links.Add(new KlimaFlaechenSubAreaVerbindung(null, new List<KlimaFlaechenModul>(new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul }), new List<Point2D>[] { this.newConnectionVertices }, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product), connection.ProductConnection.Index));
										}
										this.newConnectionStartData = null;
										this.newConnectionVertices = null;
									} else if (this.newConnectionStartData.Value.StartsAtVerbindung) {
										if (this.newConnectionStartData.Value.StartVerbindung.Start == null || this.newConnectionStartData.Value.StartVerbindung.Start.Count == 0) {
											this.newConnectionStartData.Value.StartVerbindung.DistributorIndex = connection.ProductConnection.Index;
											this.newConnectionStartData.Value.StartVerbindung.Vertices.Add(this.newConnectionVertices);
											this.newConnectionStartData = null;
											this.newConnectionVertices = null;
										} else if (this.newConnectionStartData.Value.StartVerbindung.End == null || this.newConnectionStartData.Value.StartVerbindung.End.Count == 0) {
											this.newConnectionStartData.Value.StartVerbindung.DistributorIndex = connection.ProductConnection.Index;
											this.newConnectionStartData.Value.StartVerbindung.Vertices.Add(this.newConnectionVertices);
											this.newConnectionStartData = null;
											this.newConnectionVertices = null;
										}
									}
									break;
							}
							break;
						}
					}
				}
				/*//if (this.newConnectionStart == null) {
				if (this.newConnectionStartData == null) {
					foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> input in this.GetModuleInputs(null)) {
						if (input.Value.IsInside(planPoint)) {
							int tmp;
							ModulDeckeCircuit circuit = this.product.GetCircuitForModul(input.Key, out tmp);
							if (input.Key.GetInputLink(circuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null) {
								this.newConnectionStartData = new NewConnectionStartData(input.Key, false, this.product);
								//this.newConnectionStart = input.Key;
								//this.newConnectionStartAtOutput = false;
								//this.newConnectionCircuit = circuit;
								//this.newConnectionCircuitDistributorIndex = this.newConnectionCircuit.GetDistributorConnectionIndex(true, true);
								//this.newConnectionIgnoreDistributorIndices = new List<int>();
								//foreach (ModulDeckeCircuit c in this.product.PlannedCircuits) {
								//	int index = c.GetDistributorConnectionIndex(true, this.newConnectionCircuitDistributorIndex < 0);
								//	if (index >= 0) {
								//		this.newConnectionIgnoreDistributorIndices.Add(index);
								//	}
								//}
							}
							break;
						}
					}
					//if (this.newConnectionStart == null) {
					if (this.newConnectionStartData == null) {
						foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> output in this.GetModuleOutputs(null)) {
							if (output.Value.IsInside(planPoint)) {
								int tmp;
								ModulDeckeCircuit circuit = this.product.GetCircuitForModul(output.Key, out tmp);
								if (output.Key.GetOutputLink(circuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null) {
									this.newConnectionStartData = new NewConnectionStartData(output.Key, true, this.product);
									//this.newConnectionStart = output.Key;
									//this.newConnectionStartAtOutput = true;
									//this.newConnectionCircuit = circuit;
									//this.newConnectionCircuitDistributorIndex = this.newConnectionCircuit.GetDistributorConnectionIndex(true, true);
									//this.newConnectionIgnoreDistributorIndices = new List<int>();
									//foreach (ModulDeckeCircuit c in this.product.PlannedCircuits) {
									//	int index = c.GetDistributorConnectionIndex(this.newConnectionCircuitDistributorIndex < 0, true);
									//	if (index >= 0) {
									//		this.newConnectionIgnoreDistributorIndices.Add(index);
									//	}
									//}
								}
								break;
							}
						}
					}
					//if (this.newConnectionStart != null) {
					if (this.newConnectionStartData != null) {
						this.newConnectionVertices = new List<Point2D>();
						//if (this.newConnectionStartAtOutput) {
						if (this.newConnectionStartData.Value.StartAtOutput) {
							this.newConnectionVertices.Add(this.newConnectionStartData.Value.StartModul.GetOutputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product));
						} else {
							this.newConnectionVertices.Add(this.newConnectionStartData.Value.StartModul.GetInputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product));
						}
					}
				} else {
					KlimaFlaechenModul endModul;
					KlimaFlaechenSubAreaVerbindung endVerbindung;
					GraphicalConnectionAnbindungsPunkt endAnbindung;
					bool rowConnection;
					this.newConnectionVertices.AddRange(this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out endModul, out endVerbindung, out endAnbindung, out rowConnection));
					this.nextConnectionPoints.Clear();
					if (endModul != null) {
						if (rowConnection) {
							int tmp;
							//ModulDeckeCircuit startCircuit = this.product.GetCircuitForModul(this.newConnectionStart, out tmp);
							//ModulDeckeSubArea startSubArea = startCircuit.GetSubareaForModul(this.newConnectionStart, out tmp);
							//KlimaFlaechenList startRow = startSubArea.GetRowForModul(this.newConnectionStart, out tmp);
							ModulDeckeCircuit endCircuit = this.product.GetCircuitForModul(endModul, out tmp);
							ModulDeckeSubArea endSubArea = startCircuit.GetSubareaForModul(endModul, out tmp);
							KlimaFlaechenList endRow = startSubArea.GetRowForModul(endModul, out tmp);
							if (startCircuit == endCircuit && startSubArea == endSubArea && startRow != endRow) {
								if (startCircuit.Links == null) {
									startCircuit.Links = new List<KlimaFlaechenSubAreaVerbindung>();
								}
								//if (this.newConnectionStartAtOutput) {
								if (this.newConnectionStartData.Value.StartAtOutput) {
									startCircuit.Links.Add(new KlimaFlaechenSubAreaVerbindung(new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul, endModul }, new KlimaFlaechenModul[] { }, new List<Point2D>[] { this.newConnectionVertices }, startCircuit, Project.Instance.GetPlannedProduct(this.product)));
								} else {
									startCircuit.Links.Add(new KlimaFlaechenSubAreaVerbindung(new KlimaFlaechenModul[] { }, new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul, endModul }, new List<Point2D>[] { this.newConnectionVertices }, startCircuit, Project.Instance.GetPlannedProduct(this.product)));
								}
								this.newConnectionVertices = null;
								//this.newConnectionStart = null;
								this.newConnectionStartData = null;
							}
						} else {
							int tmp;
							//ModulDeckeCircuit startCircuit = this.product.GetCircuitForModul(this.newConnectionStart, out tmp);
							//ModulDeckeSubArea startSubArea = startCircuit.GetSubareaForModul(this.newConnectionStart, out tmp);
							//KlimaFlaechenList startRow = startSubArea.GetRowForModul(this.newConnectionStart, out tmp);
							ModulDeckeCircuit endCircuit = this.product.GetCircuitForModul(endModul, out tmp);
							ModulDeckeSubArea endSubArea = startCircuit.GetSubareaForModul(endModul, out tmp);
							KlimaFlaechenList endRow = startSubArea.GetRowForModul(endModul, out tmp);
							if (startCircuit == endCircuit) {
								if (startRow == endRow) {
									if (startRow.Links == null) {
										startRow.Links = new List<KlimaFlaechenModulVerbindung>();
									}
									//if (this.newConnectionStartAtOutput) {
									if (this.newConnectionStartData.Value.StartAtOutput) {
										startRow.Links.Add(new KlimaFlaechenModulVerbindung(this.newConnectionStartData.Value.StartModul, endModul, this.newConnectionVertices, startCircuit, Project.Instance.GetPlannedProduct(this.product)));
									} else {
										startRow.Links.Add(new KlimaFlaechenModulVerbindung(endModul, this.newConnectionStartData.Value.StartModul, this.newConnectionVertices, startCircuit, Project.Instance.GetPlannedProduct(this.product)));
									}
									this.newConnectionVertices = null;
									//this.newConnectionStart = null;
									this.newConnectionStartData = null;
								} else {
									if (startCircuit.Links == null) {
										startCircuit.Links = new List<KlimaFlaechenSubAreaVerbindung>();
									}
									//if (this.newConnectionStartAtOutput) {
									if (this.newConnectionStartData.Value.StartAtOutput) {
										startCircuit.Links.Add(new KlimaFlaechenSubAreaVerbindung(new KlimaFlaechenModul[] { this.newConnectionStart }, new KlimaFlaechenModul[] { endModul }, new List<Point2D>[] { this.newConnectionVertices }, startCircuit, Project.Instance.GetPlannedProduct(this.product)));
									} else {
										startCircuit.Links.Add(new KlimaFlaechenSubAreaVerbindung(new KlimaFlaechenModul[] { endModul }, new KlimaFlaechenModul[] { this.newConnectionStart }, new List<Point2D>[] { this.newConnectionVertices }, startCircuit, Project.Instance.GetPlannedProduct(this.product)));
									}
									this.newConnectionVertices = null;
									//this.newConnectionStart = null;
									this.newConnectionStartData = null;
								}
							}
						}
					} else if (endVerbindung != null) {
						int tmp;
						ModulDeckeCircuit startCircuit = this.product.GetCircuitForModul(this.newConnectionStart, out tmp);
						ModulDeckeSubArea startSubArea = startCircuit.GetSubareaForModul(this.newConnectionStart, out tmp);
						KlimaFlaechenList startRow = startSubArea.GetRowForModul(this.newConnectionStart, out tmp);
						ModulDeckeCircuit endCircuit = endVerbindung.Circuit as ModulDeckeCircuit;
						if (startCircuit == endCircuit) {
							if (this.newConnectionStartAtOutput) {
								endVerbindung.Start.Add(this.newConnectionStart);
								endVerbindung.Vertices.Add(this.newConnectionVertices);
							} else {
								endVerbindung.End.Add(this.newConnectionStart);
								endVerbindung.Vertices.Add(this.newConnectionVertices);
							}
						}
						this.newConnectionVertices = null;
						this.newConnectionStart = null;
					} else if (endAnbindung != null) {
						int tmp;
						if (endAnbindung.NewProductConnection != null) {
							if (this.product.Connections == null) {
								this.product.Connections = new List<GraphicalProductConnection>();
							}
							this.product.Connections.Add(endAnbindung.NewProductConnection);
						}
						ModulDeckeCircuit c = this.product.GetCircuitForModul(this.newConnectionStart, out tmp);
						if (c.Links == null) {
							c.Links = new List<KlimaFlaechenSubAreaVerbindung>();
						}
						if (this.newConnectionStartAtOutput) {
							c.Links.Add(new KlimaFlaechenSubAreaVerbindung(new List<KlimaFlaechenModul>(new KlimaFlaechenModul[] { this.newConnectionStart }), null, new List<Point2D>[] { this.newConnectionVertices }, c, Project.Instance.GetPlannedProduct(this.product), endAnbindung.Index));
						} else {
							c.Links.Add(new KlimaFlaechenSubAreaVerbindung(null, new List<KlimaFlaechenModul>(new KlimaFlaechenModul[] { this.newConnectionStart }), new List<Point2D>[] { this.newConnectionVertices }, c, Project.Instance.GetPlannedProduct(this.product), endAnbindung.Index));
						}
						this.newConnectionVertices = null;
						this.newConnectionStart = null;
					}
					redraw = true;
				}*/
			} else if (this.Mode == KlimaDeckeMode.KDM_DEL_CONNECTION) {
				double bestDist = double.MaxValue;
				KlimaFlaechenModulVerbindung bestLink = null;
				KlimaFlaechenList bestRow = null;
				KlimaFlaechenSubAreaVerbindung bestSaLink = null;
				ModulDeckeCircuit bestCircuit = null;
				double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				foreach (ModulDeckeCircuit circuit in this.product.PlannedCircuits) {
					if (circuit.Links != null) {
						foreach (KlimaFlaechenSubAreaVerbindung saLink in circuit.Links) {
							double dist = saLink.GetDistance(planPoint);
							if (dist < bestDist && dist <= measure * 0.025) {
								bestDist = dist;
								bestSaLink = saLink;
								bestCircuit = circuit;
								bestLink = null;
								bestRow = null;
							}
						}
					}
					foreach (ModulDeckeSubArea sa in circuit.SubAreas) {
						foreach (KlimaFlaechenList row in sa.Rows) {
							if (row.Links != null) {
								foreach (KlimaFlaechenModulVerbindung link in row.Links) {
									double dist = link.GetDistance(planPoint);
									if (dist < bestDist && dist <= measure * 0.025) {
										bestDist = dist;
										bestLink = link;
										bestRow = row;
										bestSaLink = null;
										bestCircuit = null;
									}
								}
							}
						}
					}
				}
				if (bestLink != null) {
					bestRow.Links.Remove(bestLink);
					redraw = true;
				}
				if (bestSaLink != null) {
					bestCircuit.Links.Remove(bestSaLink);
					redraw = true;
				}
				if (bestSaLink != null) {
					bool stillConnected = false;
					foreach (ModulDeckeCircuit c in this.product.PlannedCircuits) {
						if (c.Links != null) {
							foreach (KlimaFlaechenSubAreaVerbindung link in c.Links) {
								if (link.StartConnectedToAnbindung || link.EndConnectedToAnbindung) {
									stillConnected = true;
									break;
								}
							}
						}
						if (stillConnected) {
							break;
						}
						foreach (ModulDeckeSubArea sa in c.SubAreas) {
							foreach (KlimaFlaechenList row in sa.Rows) {
								if (row.Links != null) {
									foreach (KlimaFlaechenModulVerbindung link in row.Links) {
										if (link.StartConnectedToAnbindung || link.EndConnectedToAnbindung) {
											stillConnected = true;
											break;
										}
									}
								}
								if (stillConnected) {
									break;
								}
							}
							if (stillConnected) {
								break;
							}
						}
						if (stillConnected) {
							break;
						}
					}
					if (!stillConnected) {
						List<GraphicalProductConnection> connectionsToDelete = new List<GraphicalProductConnection>();
						foreach (GraphicalProductConnection conn in this.product.Connections) {
							if (conn.Automatic) {
								connectionsToDelete.Add(conn);
							}
						}
						foreach (GraphicalProductConnection conn in connectionsToDelete) {
							this.product.Connections.Remove(conn);
						}
					}
				}
			}
			return redraw;
		}

		public class ModuleSelectedEventArgs : EventArgs {
			public KlimaFlaechenModul modul;
			public List<KlimaFlaechenModul> modules;

			public ModuleSelectedEventArgs() {
				this.modul = null;
				this.modules = null;
			}

			public ModuleSelectedEventArgs(KlimaFlaechenModul modul) {
				this.modul = modul;
				this.modules = null;
			}

			public ModuleSelectedEventArgs(List<KlimaFlaechenModul> modules) {
				this.modules = modules;
				this.modul = null;
			}
		}

		public event EventHandler<ModuleSelectedEventArgs> ModuleSelected;

		/*private bool AllowInput(KlimaFlaechenModul modul) {
			int tmp;
			if (modul == null) {
				return false;
			}
			ModulDeckeCircuit currentCircuit = this.product.GetCircuitForModul(modul, out tmp);
			if (this.newConnectionStart == null) {
				return modul.GetInputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null && modul.GetSubareaInputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null;
			}
			ModulDeckeCircuit startCircuit = this.product.GetCircuitForModul(this.newConnectionStart, out tmp);
			if (currentCircuit != startCircuit) {
				return false;
			}

			ModulDeckeSubArea currentSubArea = currentCircuit.GetSubareaForModul(modul, out tmp);
			KlimaFlaechenList currentRow = currentSubArea.GetRowForModul(modul, out tmp);
			ModulDeckeSubArea startSubArea = currentCircuit.GetSubareaForModul(this.newConnectionStart, out tmp);
			KlimaFlaechenList startRow = currentSubArea.GetRowForModul(this.newConnectionStart, out tmp);

			if (currentRow == startRow || currentSubArea != startSubArea) {
				return this.newConnectionStartAtOutput && modul.GetInputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null && modul.GetSubareaInputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null;
			} else if (currentSubArea == startSubArea) {
				return !this.newConnectionStartAtOutput && modul.GetInputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null && modul.GetSubareaInputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null;
			} else {
				return false;
				//return !this.newConnectionStartAtOutput && modul.GetInputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null && modul.GetSubareaInputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null;
			}
		}

		private bool AllowOutput(KlimaFlaechenModul modul) {
			int tmp;
			if (modul == null) {
				return false;
			}
			ModulDeckeCircuit currentCircuit = this.product.GetCircuitForModul(modul, out tmp);
			if (this.newConnectionStart == null) {
				return modul.GetOutputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null && modul.GetSubareaOutputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null;
			}
			ModulDeckeCircuit startCircuit = this.product.GetCircuitForModul(this.newConnectionStart, out tmp);
			if (currentCircuit != startCircuit) {
				return false;
			}

			ModulDeckeSubArea currentSubArea = currentCircuit.GetSubareaForModul(modul, out tmp);
			KlimaFlaechenList currentRow = currentSubArea.GetRowForModul(modul, out tmp);
			ModulDeckeSubArea startSubArea = currentCircuit.GetSubareaForModul(this.newConnectionStart, out tmp);
			KlimaFlaechenList startRow = currentSubArea.GetRowForModul(this.newConnectionStart, out tmp);

			if (currentRow == startRow || currentSubArea != startSubArea) {
				return !this.newConnectionStartAtOutput && modul.GetOutputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null && modul.GetSubareaOutputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null;
			} else if (currentSubArea == startSubArea) {
				return this.newConnectionStartAtOutput && modul.GetOutputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null && modul.GetSubareaOutputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null;
			} else {
				return false;
				//return this.newConnectionStartAtOutput && modul.GetOutputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null && modul.GetSubareaOutputLink(currentCircuit, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) == null;
			}
		}*/

		private enum PossibleConnectionPointType {
			CONNECTION_PRODUCT,
			CONNECTION_MODULE,
			CONNECTION_SUBAREA
		}

		private struct PossibleConnectionPoint {
			private PossibleConnectionPointType connectionType;
			private GraphicalConnectionAnbindungsPunkt productConnection;
			private Nullable<Point2D> connectionPoint;
			private Polygon2D connectionArea;
			private bool vorlauf;
			private bool ruecklauf;

			private KlimaFlaechenModul modul;
			private KlimaFlaechenSubAreaVerbindung verbindung;

			public PossibleConnectionPoint(GraphicalConnectionAnbindungsPunkt productConnection, bool vorlauf, bool ruecklauf) {
				this.connectionType = PossibleConnectionPointType.CONNECTION_PRODUCT;
				this.productConnection = productConnection;
				this.connectionPoint = null;
				this.connectionArea = null;
				this.modul = null;
				this.verbindung = null;
				this.vorlauf = vorlauf && !ruecklauf;
				this.ruecklauf = ruecklauf && !vorlauf;
			}

			public PossibleConnectionPoint(Point2D connectionPoint, Polygon2D connectionArea, KlimaFlaechenModul modul, bool vorlauf, bool ruecklauf) {
				this.connectionType = PossibleConnectionPointType.CONNECTION_MODULE;
				this.connectionPoint = connectionPoint;
				this.connectionArea = connectionArea;
				this.modul = modul;
				this.verbindung = null;
				this.productConnection = null;
				this.vorlauf = vorlauf && !ruecklauf;
				this.ruecklauf = ruecklauf && !vorlauf;
			}

			public PossibleConnectionPoint(Point2D connectionPoint, KlimaFlaechenSubAreaVerbindung verbindung, double measure, bool vorlauf, bool ruecklauf) {
				this.connectionType = PossibleConnectionPointType.CONNECTION_SUBAREA;
				this.connectionPoint = connectionPoint;
				double connSize = 0.05 * measure;
				this.connectionArea = new Polygon2D(new Point2D[] { connectionPoint + new Vector2D(connSize, connSize), connectionPoint + new Vector2D(connSize, -connSize), connectionPoint + new Vector2D(-connSize, -connSize), connectionPoint + new Vector2D(-connSize, connSize) });
				this.verbindung = verbindung;
				this.modul = null;
				this.productConnection = null;
				this.vorlauf = vorlauf && !ruecklauf;
				this.ruecklauf = ruecklauf && !vorlauf;
			}

			public GraphicalConnectionAnbindungsPunkt ProductConnection {
				get { return this.productConnection; }
			}

			public PossibleConnectionPointType ConnectionType {
				get { return this.connectionType; }
			}

			public Point2D ConnectionPoint {
				get {
					switch (this.connectionType) {
						case PossibleConnectionPointType.CONNECTION_PRODUCT:
							return this.productConnection.Point;
							
						case PossibleConnectionPointType.CONNECTION_SUBAREA:
						case PossibleConnectionPointType.CONNECTION_MODULE:
							return this.connectionPoint.Value;

						default:
							throw new Exception();
					}
				}
			}

			public Polygon2D ConnectionArea {
				get {
					switch (this.connectionType) {
						case PossibleConnectionPointType.CONNECTION_PRODUCT:
							return this.productConnection.Area;

						case PossibleConnectionPointType.CONNECTION_SUBAREA:
						case PossibleConnectionPointType.CONNECTION_MODULE:
							return this.connectionArea;

						default:
							throw new Exception();
					}
				}
			}

			public bool Vorlauf {
				get { return this.vorlauf; }
			}

			public bool Ruecklauf {
				get { return this.ruecklauf; }
			}

			public KlimaFlaechenModul Modul {
				get { return this.modul; }
			}

			public KlimaFlaechenSubAreaVerbindung Verbindung {
				get { return this.verbindung; }
			}
		}

		private struct NewConnectionStartData {
			private KlimaFlaechenModul startModul;
			private KlimaFlaechenList rowOfStartModul;
			private ModulDeckeSubArea subAreaOfStartModul;
			private bool startAtOutput;

			private KlimaFlaechenSubAreaVerbindung startVerbindung;
			private ModulDeckeSubArea verbindungStartSubArea;
			private ModulDeckeSubArea verbindungEndSubArea;
			private List<KlimaFlaechenList> openRowsInStartSubArea;
			private List<KlimaFlaechenList> openRowsInEndSubArea;

			private ModulDeckeCircuit circuitOfStart;
			private int distributorIndex;
			private List<int> ignoreDistributorIndices;

			private GraphicalConnectionAnbindungsPunkt startAnbindung;

			public NewConnectionStartData(KlimaFlaechenModul startModul, bool startAtOutput, ModulKlimaDeckeProduct product) {
				this.startVerbindung = null;
				this.verbindungStartSubArea = null;
				this.verbindungEndSubArea = null;
				this.openRowsInStartSubArea = null;
				this.openRowsInEndSubArea = null;
				this.startAnbindung = null;

				this.startModul = startModul;
				this.startAtOutput = startAtOutput;
				rowOfStartModul = null;
				subAreaOfStartModul = null;
				circuitOfStart = null;
				foreach (ModulDeckeCircuit c in product.PlannedCircuits) {
					foreach (ModulDeckeSubArea sa in c.SubAreas) {
						foreach (KlimaFlaechenList row in sa.Rows) {
							if (row.List.Contains(startModul)) {
								rowOfStartModul = row;
								subAreaOfStartModul = sa;
								circuitOfStart = c;
								break;
							}
						}
						if (rowOfStartModul != null) {
							break;
						}
					}
					if (rowOfStartModul != null) {
						break;
					}
				}

				this.distributorIndex = this.circuitOfStart.GetDistributorConnectionIndex(true, true);
				this.ignoreDistributorIndices = new List<int>();
				foreach (ModulDeckeCircuit c in product.PlannedCircuits) {
					int index;
					if (startAtOutput) {
						index = c.GetDistributorConnectionIndex(this.distributorIndex < 0, true);
					} else {
						index = c.GetDistributorConnectionIndex(true, this.distributorIndex < 0);
					}
					if (index >= 0) {
						this.ignoreDistributorIndices.Add(index);
					}
				}
			}

			/*public NewConnectionStartData(int distributorIndex, bool startAtOutput, ModulKlimaDeckeProduct product) {
				this.distributorIndex = distributorIndex;
				this.startAtOutput = startAtOutput;
				if (startAtOutput) {
					foreach (ModulDeckeCircuit c in product.PlannedCircuits) {
						foreach (KlimaFlaechenSubAreaVerbindung link in c.Links) {
							if (link.DistributorIndex == distributorIndex
						}
					}
				} else {
				}
			}*/

			public NewConnectionStartData(GraphicalConnectionAnbindungsPunkt anbindung, bool vorlauf, ModulKlimaDeckeProduct product) {
				this.startModul = null;
				this.rowOfStartModul = null;
				this.subAreaOfStartModul = null;
				this.startVerbindung = null;
				this.verbindungStartSubArea = null;
				this.verbindungEndSubArea = null;
				this.openRowsInStartSubArea = null;
				this.openRowsInEndSubArea = null;
				this.ignoreDistributorIndices = new List<int>();

				this.startAtOutput = vorlauf;
				this.startAnbindung = anbindung;
				this.distributorIndex = anbindung.Index;
				this.circuitOfStart = null;

				foreach (ModulDeckeCircuit c in product.PlannedCircuits) {
					if (c.GetDistributorConnectionIndex(!vorlauf, vorlauf) == this.distributorIndex) {
						circuitOfStart = c;
					}
				}
			}

			public NewConnectionStartData(KlimaFlaechenSubAreaVerbindung startVerbindung, ModulKlimaDeckeProduct product) {
				this.startModul = null;
				this.rowOfStartModul = null;
				this.subAreaOfStartModul = null;
				this.startAnbindung = null;
				this.startAtOutput = false;

				this.startVerbindung = startVerbindung;

				KlimaFlaechenModul verbindungStartModul = null;
				KlimaFlaechenModul verbindungEndModul = null;
				if (startVerbindung.Start != null && startVerbindung.Start.Count > 0) {
					verbindungStartModul = startVerbindung.Start[0];
				}
				if (startVerbindung.End != null && startVerbindung.End.Count > 0) {
					verbindungEndModul = startVerbindung.End[0];
				}

				this.circuitOfStart = null;
				foreach (ModulDeckeCircuit c in product.PlannedCircuits) {
					if (c.Links.Contains(startVerbindung)) {
						this.circuitOfStart = c;
						break;
					}
				}

				this.verbindungStartSubArea = null;
				this.verbindungEndSubArea = null;
				this.openRowsInStartSubArea = new List<KlimaFlaechenList>();
				this.openRowsInEndSubArea = new List<KlimaFlaechenList>();
				foreach (ModulDeckeSubArea sa in this.circuitOfStart.SubAreas) {
					foreach (KlimaFlaechenList row in sa.Rows) {
						if (verbindungStartModul != null && row.List.Contains(verbindungStartModul)) {
							this.verbindungStartSubArea = sa;
						}
						if (verbindungEndModul != null && row.List.Contains(verbindungEndModul)) {
							this.verbindungEndSubArea = sa;
						}
						if ((verbindungStartModul == null || this.verbindungStartSubArea != null) && (verbindungEndModul == null || this.verbindungEndSubArea != null)) {
							break;
						}
					}
					if ((verbindungStartModul == null || this.verbindungStartSubArea != null) && (verbindungEndModul == null || this.verbindungEndSubArea != null)) {
						break;
					}
				}

				if (this.verbindungStartSubArea != null) {
					this.openRowsInStartSubArea.AddRange(this.verbindungStartSubArea.Rows);
					foreach (KlimaFlaechenList r in startVerbindung.GetStartRows()) {
						if (r != null && this.openRowsInStartSubArea.Contains(r)) {
							this.openRowsInStartSubArea.Remove(r);
						}
					}
				}
				if (this.verbindungEndSubArea != null) {
					this.openRowsInEndSubArea.AddRange(this.verbindungEndSubArea.Rows);
					foreach (KlimaFlaechenList r in startVerbindung.GetEndRows()) {
						if (r != null && this.openRowsInEndSubArea.Contains(r)) {
							this.openRowsInEndSubArea.Remove(r);
						}
					}
				}

				this.distributorIndex = this.circuitOfStart.GetDistributorConnectionIndex(true, true);
				this.ignoreDistributorIndices = new List<int>();
				foreach (ModulDeckeCircuit c in product.PlannedCircuits) {
					int index;
					if (startAtOutput) {
						index = c.GetDistributorConnectionIndex(this.distributorIndex < 0, true);
					} else {
						index = c.GetDistributorConnectionIndex(true, this.distributorIndex < 0);
					}
					if (index >= 0) {
						this.ignoreDistributorIndices.Add(index);
					}
				}
			}

			public bool StartsAtModul {
				get { return this.startModul != null; }
			}

			public bool StartsAtVerbindung {
				get { return this.startVerbindung != null; }
			}

			public bool StartsAtAnbindung {
				get { return this.startAnbindung != null; }
			}

			public KlimaFlaechenModul StartModul {
				get { return this.startModul; }
			}

			public KlimaFlaechenList RowOfStartModul {
				get { return this.rowOfStartModul; }
			}

			public ModulDeckeSubArea SubAreaOfStartModul {
				get { return this.subAreaOfStartModul; }
			}

			public bool StartAtOutput {
				get { return this.startAtOutput; }
			}



			public KlimaFlaechenSubAreaVerbindung StartVerbindung {
				get { return this.startVerbindung; }
			}

			public ModulDeckeSubArea VerbindungStartSubArea {
				get { return this.verbindungStartSubArea; }
			}

			public ModulDeckeSubArea VerbindungEndSubArea {
				get { return this.verbindungEndSubArea; }
			}

			public List<KlimaFlaechenList> OpenRowsInStartSubArea {
				get { return this.openRowsInStartSubArea; }
			}

			public List<KlimaFlaechenList> OpenRowsInEndSubArea {
				get { return this.openRowsInEndSubArea; }
			}


			public ModulDeckeCircuit CircuitOfStart {
				get { return this.circuitOfStart; }
			}

			public int DistributorIndex {
				get { return this.distributorIndex; }
			}

			public List<int> IgnoreDistributorIndices {
				get { return this.ignoreDistributorIndices; }
			}

			public GraphicalConnectionAnbindungsPunkt StartAnbindung {
				get { return this.startAnbindung; }
			}
		}

		private List<PossibleConnectionPoint> GetPossibleConnectionPoints(Point2D mousePosition) {
			List<PossibleConnectionPoint> result = new List<PossibleConnectionPoint>();
			int tmp;
			if (this.newConnectionStartData != null) {
				if (this.newConnectionStartData.Value.StartsAtModul) {
					// add connections to distributor
					List<GraphicalConnectionAnbindungsPunkt> productConnections;
					if (!this.newConnectionStartData.Value.StartAtOutput) {
						productConnections = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, true, this.newConnectionStartData.Value.DistributorIndex, this.newConnectionStartData.Value.IgnoreDistributorIndices, mousePosition);
					} else {
						productConnections = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, false, this.newConnectionStartData.Value.DistributorIndex, this.newConnectionStartData.Value.IgnoreDistributorIndices, mousePosition);
					}
					foreach (GraphicalConnectionAnbindungsPunkt conn in productConnections) {
						result.Add(new PossibleConnectionPoint(conn, !this.newConnectionStartData.Value.StartAtOutput, this.newConnectionStartData.Value.StartAtOutput));
					}

					// add connections to modules
					Dictionary<KlimaFlaechenModul, Polygon2D> areas = this.GetModuleAreas();
					double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					bool invertYAxis = this.product.AssociatedRoom.AssociatedPlan.InvertYAxis;
					foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> area in areas) {
						if (area.Value.IsInside(mousePosition)) {
							if (this.newConnectionStartData.Value.StartModul == area.Key) {
								break;
							}
							ModulDeckeCircuit c = this.product.GetCircuitForModul(area.Key, out tmp);
							ModulDeckeSubArea sa = c.GetSubareaForModul(area.Key, out tmp);
							KlimaFlaechenList row = sa.GetRowForModul(area.Key, out tmp);
							if (c == this.newConnectionStartData.Value.CircuitOfStart) {
								if (sa != this.newConnectionStartData.Value.SubAreaOfStartModul || row == this.newConnectionStartData.Value.RowOfStartModul) {
									// allow connections from output to input or from input to output
									if (this.newConnectionStartData.Value.StartAtOutput) {
										if (area.Key.IsInputOpen(c, invertYAxis)) {
											result.Add(new PossibleConnectionPoint(area.Key.GetInputConnection(measure, invertYAxis, product), area.Key.GetInputConnectionArea(measure, invertYAxis, this.product), area.Key, true, false));
										}
									} else {
										if (area.Key.IsOutputOpen(c, invertYAxis)) {
											result.Add(new PossibleConnectionPoint(area.Key.GetOutputConnection(measure, invertYAxis, product), area.Key.GetOutputConnectionArea(measure, invertYAxis, this.product), area.Key, false, true));
										}
									}
								} else {
									// allow connections from input to input or from output to output
									if (this.newConnectionStartData.Value.StartAtOutput) {
										if (area.Key.IsOutputOpen(c, invertYAxis)) {
											result.Add(new PossibleConnectionPoint(area.Key.GetOutputConnection(measure, invertYAxis, product), area.Key.GetOutputConnectionArea(measure, invertYAxis, this.product), area.Key, false, true));
										}
									} else {
										if (area.Key.IsInputOpen(c, invertYAxis)) {
											result.Add(new PossibleConnectionPoint(area.Key.GetInputConnection(measure, invertYAxis, product), area.Key.GetInputConnectionArea(measure, invertYAxis, this.product), area.Key, true, false));
										}
									}
								}
							}
							break;
						}
					}

					// add connections to subarea-connections
					Nullable<Point2D> saLinkPoint = null;
					KlimaFlaechenSubAreaVerbindung verbindung = null;
					if (this.newConnectionStartData.Value.CircuitOfStart.Links != null) {
						double dist;
						double bestDist = double.MaxValue;
						Nullable<Point2D> point;
						foreach (KlimaFlaechenSubAreaVerbindung saLink in this.newConnectionStartData.Value.CircuitOfStart.Links) {
							point = saLink.GetClosestPoint(mousePosition, out dist);
							dist = dist / measure;
							if (dist <= 0.05 && dist < bestDist) {
								// check if this connection is allowed
								bool ok = false;
								if (this.newConnectionStartData.Value.StartAtOutput) {
									ModulDeckeSubArea subArea = null;
									List<ModulDeckeSubArea> subAreas = saLink.GetStartSubAreas();
									if (subAreas != null && subAreas.Count > 0) {
										subArea = subAreas[0];
									}

									//int tmp;
									ModulDeckeSubArea startSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartModul, out tmp);
									KlimaFlaechenList startRow = startSubArea.GetRowForModul(this.newConnectionStartData.Value.StartModul, out tmp);
									if ((subArea == null || subArea == startSubArea) && !saLink.GetStartRows().Contains(startRow) && !saLink.StartConnectedToAnbindung) {
										ok = true;
									}
									//if (saLink.EndRows != null && saLink.EndRows.Count > 0 && saLink.EndRows[0].
								} else {
									ModulDeckeSubArea subArea = null;
									List<ModulDeckeSubArea> subAreas = saLink.GetEndSubAreas();
									if (subAreas != null && subAreas.Count > 0) {
										subArea = subAreas[0];
									}

									//int tmp;
									ModulDeckeSubArea startSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartModul, out tmp);
									KlimaFlaechenList startRow = startSubArea.GetRowForModul(this.newConnectionStartData.Value.StartModul, out tmp);
									if ((subArea == null || subArea == startSubArea) && !saLink.GetStartRows().Contains(startRow) && !saLink.EndConnectedToAnbindung) {
										ok = true;
									}
								}
								if (ok) {
									bestDist = dist;
									saLinkPoint = point;
									verbindung = saLink;
								}
							}
						}
					}
					if (saLinkPoint != null) {
						bool addRealEndPoint = true;
						if (this.newConnectionVertices.Count > 1) {
							Point2D p1 = this.newConnectionVertices[this.newConnectionVertices.Count - 2];
							Point2D p2 = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
							Line2D line1 = new Line2D(p1, p1 - p2);
							Line2D line2 = new Line2D(saLinkPoint.Value, new Vector2D(line1.Direction.Y, -line1.Direction.X));
							Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
							if (intersection.HasValue) {
								double dist;
								verbindung.GetClosestPoint(intersection.Value, out dist);
								dist = dist * measure;
								if (dist < 0.0000001) {
									result.Add(new PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
									addRealEndPoint = false;
								}
							}
						} else if (this.newConnectionVertices.Count > 0) {
							bool tmpH;
							Point2D nextPoint = this.GetNextConnectionVertex(mousePosition, out tmpH);
							Line2D l = new Line2D(this.newConnectionVertices[0], this.newConnectionVertices[0] - nextPoint);
							Nullable<Point2D> oldPoint = null;
							Segment2D seg;
							foreach (List<Point2D> list in verbindung.Vertices) {
								oldPoint = null;
								foreach (Point2D p in list) {
									if (oldPoint.HasValue) {
										seg = new Segment2D(oldPoint.Value, p);
										Nullable<Point2D> intersection = Line2D.GetIntersection(l, seg);
										if (intersection.HasValue) {
											result.Add(new PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
											addRealEndPoint = false;
											break;
										}
									}
									oldPoint = p;
								}
								if (!addRealEndPoint) {
									break;
								}
							}
						}
						if (addRealEndPoint) {
							result.Add(new PossibleConnectionPoint(saLinkPoint.Value, verbindung, measure, false, false));
						}
					}
				} else if (this.newConnectionStartData.Value.StartsAtVerbindung) {
					// add connections to distributor
					List<GraphicalConnectionAnbindungsPunkt> productConnections = null;
					bool getVorlauf = this.newConnectionStartData.Value.StartVerbindung.Start == null || this.newConnectionStartData.Value.StartVerbindung.Start.Count == 0;
					bool getRuecklauf = this.newConnectionStartData.Value.StartVerbindung.End == null || this.newConnectionStartData.Value.StartVerbindung.End.Count == 0;
					if (getVorlauf) {
						productConnections = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, true, this.newConnectionStartData.Value.DistributorIndex, this.newConnectionStartData.Value.IgnoreDistributorIndices, mousePosition);
					} else if (getRuecklauf) {
						productConnections = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, false, this.newConnectionStartData.Value.DistributorIndex, this.newConnectionStartData.Value.IgnoreDistributorIndices, mousePosition);
					}
					if (productConnections != null) {
						foreach (GraphicalConnectionAnbindungsPunkt conn in productConnections) {
							result.Add(new PossibleConnectionPoint(conn, getVorlauf, getRuecklauf));
						}
					}

					// add connections to modules
					Dictionary<KlimaFlaechenModul, Polygon2D> areas = this.GetModuleAreas();
					double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					bool invertYAxis = this.product.AssociatedRoom.AssociatedPlan.InvertYAxis;
					foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> area in areas) {
						if (area.Value.IsInside(mousePosition)) {
							if (this.newConnectionStartData.Value.StartModul == area.Key) {
								break;
							}
							ModulDeckeCircuit c = this.product.GetCircuitForModul(area.Key, out tmp);
							ModulDeckeSubArea sa = c.GetSubareaForModul(area.Key, out tmp);
							KlimaFlaechenList row = sa.GetRowForModul(area.Key, out tmp);
							if (c == this.newConnectionStartData.Value.CircuitOfStart) {
								ModulDeckeSubArea verbindungStartSubArea = null;
								ModulDeckeSubArea verbindungEndSubArea = null;
								if (this.newConnectionStartData.Value.StartVerbindung.Start != null && this.newConnectionStartData.Value.StartVerbindung.Start.Count > 0) {
									verbindungStartSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartVerbindung.Start[0], out tmp);
								}
								if (this.newConnectionStartData.Value.StartVerbindung.End != null && this.newConnectionStartData.Value.StartVerbindung.End.Count > 0) {
									verbindungEndSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartVerbindung.End[0], out tmp);
								}
								if (sa != null) {
									if (sa == verbindungStartSubArea) {
										if (area.Key.IsOutputOpen(c, invertYAxis)) {
											result.Add(new PossibleConnectionPoint(area.Key.GetOutputConnection(measure, invertYAxis, this.product), area.Key.GetOutputConnectionArea(measure, invertYAxis, this.product), area.Key, false, true));
										}
									} else if (sa == verbindungEndSubArea) {
										if (area.Key.IsInputOpen(c, invertYAxis)) {
											result.Add(new PossibleConnectionPoint(area.Key.GetInputConnection(measure, invertYAxis, this.product), area.Key.GetInputConnectionArea(measure, invertYAxis, this.product), area.Key, true, false));
										}
									}
									if (verbindungStartSubArea == null && sa != verbindungEndSubArea) {
										if (area.Key.IsOutputOpen(c, invertYAxis)) {
											result.Add(new PossibleConnectionPoint(area.Key.GetOutputConnection(measure, invertYAxis, this.product), area.Key.GetOutputConnectionArea(measure, invertYAxis, this.product), area.Key, false, true));
										}
									} else if (verbindungEndSubArea == null && sa != verbindungStartSubArea) {
										if (area.Key.IsInputOpen(c, invertYAxis)) {
											result.Add(new PossibleConnectionPoint(area.Key.GetInputConnection(measure, invertYAxis, this.product), area.Key.GetInputConnectionArea(measure, invertYAxis, this.product), area.Key, true, false));
										}
									}
								}
							}
							break;
						}
					}

					// add connections to subarea-connections
					Nullable<Point2D> saLinkPoint = null;
					KlimaFlaechenSubAreaVerbindung verbindung = null;
					if (this.newConnectionStartData.Value.CircuitOfStart.Links != null) {
						double dist;
						double bestDist = double.MaxValue;
						Nullable<Point2D> point;
						foreach (KlimaFlaechenSubAreaVerbindung saLink in this.newConnectionStartData.Value.CircuitOfStart.Links) {
							point = saLink.GetClosestPoint(mousePosition, out dist);
							dist = dist / measure;
							if (dist <= 0.05 && dist < bestDist && this.newConnectionStartData.Value.StartVerbindung != saLink) {
								// check if this connection is allowed
								bool ok = false;
								ModulDeckeSubArea verbindungStartSubArea = null;
								ModulDeckeSubArea verbindungEndSubArea = null;
								if (this.newConnectionStartData.Value.StartVerbindung.Start != null && this.newConnectionStartData.Value.StartVerbindung.Start.Count > 0) {
									verbindungStartSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartVerbindung.Start[0], out tmp);
								}
								if (this.newConnectionStartData.Value.StartVerbindung.End != null && this.newConnectionStartData.Value.StartVerbindung.End.Count > 0) {
									verbindungEndSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartVerbindung.End[0], out tmp);
								}

								ModulDeckeSubArea saLinkStartSubArea = null;
								ModulDeckeSubArea saLinkEndSubArea = null;
								if (saLink.Start != null && saLink.Start.Count > 0) {
									saLinkStartSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(saLink.Start[0], out tmp);
								}
								if (saLink.End != null && saLink.End.Count > 0) {
									verbindungEndSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(saLink.End[0], out tmp);
								}

								if ((verbindungStartSubArea == null || saLinkStartSubArea == null || verbindungStartSubArea == saLinkStartSubArea) && (verbindungEndSubArea == null || saLinkEndSubArea == null || verbindungEndSubArea == saLinkEndSubArea)) {
									ok = true;
								}
								if (ok) {
									bestDist = dist;
									saLinkPoint = point;
									verbindung = saLink;
								}
							}
						}
					}
					if (saLinkPoint != null) {
						bool addRealEndPoint = true;
						if (this.newConnectionVertices.Count > 1) {
							Point2D p1 = this.newConnectionVertices[this.newConnectionVertices.Count - 2];
							Point2D p2 = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
							Line2D line1 = new Line2D(p1, p1 - p2);
							Line2D line2 = new Line2D(saLinkPoint.Value, new Vector2D(line1.Direction.Y, -line1.Direction.X));
							Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
							if (intersection.HasValue) {
								double dist;
								verbindung.GetClosestPoint(intersection.Value, out dist);
								dist = dist * measure;
								if (dist < 0.0000001) {
									result.Add(new PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
									addRealEndPoint = false;
								}
							}
						} else if (this.newConnectionVertices.Count > 0) {
							bool tmpH;
							Point2D nextPoint = this.GetNextConnectionVertex(mousePosition, out tmpH);
							Line2D l = new Line2D(this.newConnectionVertices[0], this.newConnectionVertices[0] - nextPoint);
							Nullable<Point2D> oldPoint = null;
							Segment2D seg;
							foreach (List<Point2D> list in verbindung.Vertices) {
								oldPoint = null;
								foreach (Point2D p in list) {
									if (oldPoint.HasValue) {
										seg = new Segment2D(oldPoint.Value, p);
										Nullable<Point2D> intersection = Line2D.GetIntersection(l, seg);
										if (intersection.HasValue) {
											result.Add(new PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
											addRealEndPoint = false;
											break;
										}
									}
									oldPoint = p;
								}
								if (!addRealEndPoint) {
									break;
								}
							}
						}
						if (addRealEndPoint) {
							result.Add(new PossibleConnectionPoint(saLinkPoint.Value, verbindung, measure, false, false));
						}
					}
				} else if (this.newConnectionStartData.Value.StartsAtAnbindung) {

					// add connections to modules
					Dictionary<KlimaFlaechenModul, Polygon2D> areas = this.GetModuleAreas();
					double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					bool invertYAxis = this.product.AssociatedRoom.AssociatedPlan.InvertYAxis;
					foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> area in areas) {
						if (area.Value.IsInside(mousePosition)) {
							
							ModulDeckeCircuit c = this.product.GetCircuitForModul(area.Key, out tmp);
							if (this.newConnectionStartData.Value.CircuitOfStart != null && c != this.newConnectionStartData.Value.CircuitOfStart) {
								break;
							}
							if (c.GetDistributorConnectionIndex(this.newConnectionStartData.Value.StartAtOutput, !this.newConnectionStartData.Value.StartAtOutput) >= 0) {
								break;
							}
							int di = c.GetDistributorConnectionIndex(true, true);
							if (di >= 0 && this.newConnectionStartData.Value.DistributorIndex != c.GetDistributorConnectionIndex(true, true)) {
								break;
							}

							if (!this.newConnectionStartData.Value.StartAtOutput && area.Key.IsOutputOpen(c, invertYAxis)) {
								result.Add(new PossibleConnectionPoint(area.Key.GetOutputConnection(measure, invertYAxis, this.product), area.Key.GetOutputConnectionArea(measure, invertYAxis, this.product), area.Key, false, true));
							}
							if (this.newConnectionStartData.Value.StartAtOutput && area.Key.IsInputOpen(c, invertYAxis)) {
								result.Add(new PossibleConnectionPoint(area.Key.GetInputConnection(measure, invertYAxis, this.product), area.Key.GetInputConnectionArea(measure, invertYAxis, this.product), area.Key, true, false));
							}
							break;
						}
					}

					List<ModulDeckeCircuit> circuitsToUse = new List<ModulDeckeCircuit>();
					if (this.newConnectionStartData.Value.CircuitOfStart != null) {
						circuitsToUse.Add(this.newConnectionStartData.Value.CircuitOfStart);
					} else {
						foreach (ModulDeckeCircuit c in this.product.PlannedCircuits) {
							int di = c.GetDistributorConnectionIndex(true, true);
							if (di < 0 || di == this.newConnectionStartData.Value.DistributorIndex) {
								circuitsToUse.Add(c);
							}
						}
					}
					double dist;
					double bestDist = double.MaxValue;
					Nullable<Point2D> point, saLinkPoint = null;
					KlimaFlaechenSubAreaVerbindung verbindung = null;
					foreach (ModulDeckeCircuit c in circuitsToUse) {
						foreach (KlimaFlaechenSubAreaVerbindung saLink in c.Links) {
							point = saLink.GetClosestPoint(mousePosition, out dist);
							dist = dist / measure;
							if (dist <= 0.05 && dist < bestDist) {
								bestDist = dist;
								saLinkPoint = point;
								verbindung = saLink;
							}
						}
					}

					if (saLinkPoint != null) {
						bool addRealEndPoint = true;
						if (this.newConnectionVertices.Count > 1) {
							Point2D p1 = this.newConnectionVertices[this.newConnectionVertices.Count - 2];
							Point2D p2 = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
							Line2D line1 = new Line2D(p1, p1 - p2);
							Line2D line2 = new Line2D(saLinkPoint.Value, new Vector2D(line1.Direction.Y, -line1.Direction.X));
							Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
							if (intersection.HasValue) {
								verbindung.GetClosestPoint(intersection.Value, out dist);
								dist = dist * measure;
								if (dist < 0.0000001) {
									result.Add(new PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
									addRealEndPoint = false;
								}
							}
						} else if (this.newConnectionVertices.Count > 0) {
							bool tmpH;
							Point2D nextPoint = this.GetNextConnectionVertex(mousePosition, out tmpH);
							Line2D l = new Line2D(this.newConnectionVertices[0], this.newConnectionVertices[0] - nextPoint);
							Nullable<Point2D> oldPoint = null;
							Segment2D seg;
							foreach (List<Point2D> list in verbindung.Vertices) {
								oldPoint = null;
								foreach (Point2D p in list) {
									if (oldPoint.HasValue) {
										seg = new Segment2D(oldPoint.Value, p);
										Nullable<Point2D> intersection = Line2D.GetIntersection(l, seg);
										if (intersection.HasValue) {
											result.Add(new PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
											addRealEndPoint = false;
											break;
										}
									}
									oldPoint = p;
								}
								if (!addRealEndPoint) {
									break;
								}
							}
						}
						if (addRealEndPoint) {
							result.Add(new PossibleConnectionPoint(saLinkPoint.Value, verbindung, measure, false, false));
						}
					}

					// add connections to subarea-connections
					/*Nullable<Point2D> saLinkPoint = null;
					KlimaFlaechenSubAreaVerbindung verbindung = null;
					if (this.newConnectionStartData.Value.CircuitOfStart.Links != null) {
						double dist;
						double bestDist = double.MaxValue;
						Nullable<Point2D> point;
						foreach (KlimaFlaechenSubAreaVerbindung saLink in this.newConnectionStartData.Value.CircuitOfStart.Links) {
							point = saLink.GetClosestPoint(mousePosition, out dist);
							dist = dist / measure;
							if (dist <= 0.05 && dist < bestDist && this.newConnectionStartData.Value.StartVerbindung != saLink) {
								// check if this connection is allowed
								bool ok = false;
								ModulDeckeSubArea verbindungStartSubArea = null;
								ModulDeckeSubArea verbindungEndSubArea = null;
								if (this.newConnectionStartData.Value.StartVerbindung.Start != null && this.newConnectionStartData.Value.StartVerbindung.Start.Count > 0) {
									verbindungStartSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartVerbindung.Start[0], out tmp);
								}
								if (this.newConnectionStartData.Value.StartVerbindung.End != null && this.newConnectionStartData.Value.StartVerbindung.End.Count > 0) {
									verbindungEndSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartVerbindung.End[0], out tmp);
								}

								ModulDeckeSubArea saLinkStartSubArea = null;
								ModulDeckeSubArea saLinkEndSubArea = null;
								if (saLink.Start != null && saLink.Start.Count > 0) {
									saLinkStartSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(saLink.Start[0], out tmp);
								}
								if (saLink.End != null && saLink.End.Count > 0) {
									verbindungEndSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(saLink.End[0], out tmp);
								}

								if ((verbindungStartSubArea == null || saLinkStartSubArea == null || verbindungStartSubArea == saLinkStartSubArea) && (verbindungEndSubArea == null || saLinkEndSubArea == null || verbindungEndSubArea == saLinkEndSubArea)) {
									ok = true;
								}
								if (ok) {
									bestDist = dist;
									saLinkPoint = point;
									verbindung = saLink;
								}
							}
						}
					}
					if (saLinkPoint != null) {
						bool addRealEndPoint = true;
						if (this.newConnectionVertices.Count > 1) {
							Point2D p1 = this.newConnectionVertices[this.newConnectionVertices.Count - 2];
							Point2D p2 = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
							Line2D line1 = new Line2D(p1, p1 - p2);
							Line2D line2 = new Line2D(saLinkPoint.Value, new Vector2D(line1.Direction.Y, -line1.Direction.X));
							Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
							if (intersection.HasValue) {
								double dist;
								verbindung.GetClosestPoint(intersection.Value, out dist);
								dist = dist * measure;
								if (dist < 0.0000001) {
									result.Add(new PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
									addRealEndPoint = false;
								}
							}
						} else if (this.newConnectionVertices.Count > 0) {
							bool tmpH;
							Point2D nextPoint = this.GetNextConnectionVertex(mousePosition, out tmpH);
							Line2D l = new Line2D(this.newConnectionVertices[0], this.newConnectionVertices[0] - nextPoint);
							Nullable<Point2D> oldPoint = null;
							Segment2D seg;
							foreach (List<Point2D> list in verbindung.Vertices) {
								oldPoint = null;
								foreach (Point2D p in list) {
									if (oldPoint.HasValue) {
										seg = new Segment2D(oldPoint.Value, p);
										Nullable<Point2D> intersection = Line2D.GetIntersection(l, seg);
										if (intersection.HasValue) {
											result.Add(new PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
											addRealEndPoint = false;
											break;
										}
									}
									oldPoint = p;
								}
								if (!addRealEndPoint) {
									break;
								}
							}
						}
						if (addRealEndPoint) {
							result.Add(new PossibleConnectionPoint(saLinkPoint.Value, verbindung, measure, false, false));
						}
					}*/

				}
			} else {
				// add connections to distributor
				List<int> ignoreVlDistributorIndices = new List<int>();
				List<int> ignoreRlDistributorIndices = new List<int>();
				foreach (ModulDeckeCircuit c in product.PlannedCircuits) {
					int index;
					index = c.GetDistributorConnectionIndex(true, false);
					if (index >= 0) {
						ignoreVlDistributorIndices.Add(index);
					}
					index = c.GetDistributorConnectionIndex(false, true);
					if (index >= 0) {
						ignoreRlDistributorIndices.Add(index);
					}
				}

				List<GraphicalConnectionAnbindungsPunkt> productConnections;
				productConnections = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, true, -1, ignoreVlDistributorIndices, mousePosition);
				foreach (GraphicalConnectionAnbindungsPunkt conn in productConnections) {
					result.Add(new PossibleConnectionPoint(conn, true, false));
				}
				productConnections = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, false, -1, ignoreRlDistributorIndices, mousePosition);
				foreach (GraphicalConnectionAnbindungsPunkt conn in productConnections) {
					result.Add(new PossibleConnectionPoint(conn, false, true));
				}

				// add connections to modules
				Dictionary<KlimaFlaechenModul, Polygon2D> areas = this.GetModuleAreas();
				//KlimaFlaechenModul modul = null;
				double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				bool invertYAxis = this.product.AssociatedRoom.AssociatedPlan.InvertYAxis;
				foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> area in areas) {
					if (area.Value.IsInside(mousePosition)) {
						ModulDeckeCircuit c = this.product.GetCircuitForModul(area.Key, out tmp);
						ModulDeckeSubArea sa = c.GetSubareaForModul(area.Key, out tmp);
						KlimaFlaechenList row = sa.GetRowForModul(area.Key, out tmp);
						if (area.Key.IsInputOpen(c, invertYAxis)) {
							result.Add(new PossibleConnectionPoint(area.Key.GetInputConnection(measure, invertYAxis, product), area.Key.GetInputConnectionArea(measure, invertYAxis, this.product), area.Key, true, false));
						}
						if (area.Key.IsOutputOpen(c, invertYAxis)) {
							result.Add(new PossibleConnectionPoint(area.Key.GetOutputConnection(measure, invertYAxis, product), area.Key.GetOutputConnectionArea(measure, invertYAxis, this.product), area.Key, false, true));
						}
						break;
					}
				}

				// add connections to subarea-connections
				Nullable<Point2D> saLinkPoint = null;
				KlimaFlaechenSubAreaVerbindung verbindung = null;
				//if (this.newConnectionStartData.Value.CircuitOfStart.Links != null) {
				foreach (ModulDeckeCircuit c in this.product.PlannedCircuits) {
					double dist;
					double bestDist = double.MaxValue;
					Nullable<Point2D> point;
					if (c.Links != null) {
						foreach (KlimaFlaechenSubAreaVerbindung saLink in c.Links) {
							point = saLink.GetClosestPoint(mousePosition, out dist);
							dist = dist / measure;
							if (dist <= 0.05 && dist < bestDist) {
								// check if this connection is allowed
								bestDist = dist;
								saLinkPoint = point;
								verbindung = saLink;
							}
						}
					}
				}
				if (saLinkPoint != null) {
					result.Add(new PossibleConnectionPoint(saLinkPoint.Value, verbindung, measure, false, false));
				}
			}


			return result;
		}

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			bool redraw = false;
			if (this.Mode == KlimaDeckeMode.KDM_CONSTRUCTION) {
				if (this.product.GraphConstruction != null) {
					if (this.product.GraphConstruction.HitTest(planPoint, pointInControl)) {
						this.customCursor = this.product.GraphConstruction.PickCursor;
						this.ConnectedPlanPanel.PlanCursor = this.product.GraphConstruction.PickCursor;
					} else {
						this.customCursor = Cursors.Default;
						this.ConnectedPlanPanel.PlanCursor = Cursors.Default;
					}
				}
			} else if (this.Mode == KlimaDeckeMode.KDM_ADD_CONNECTION) {
				redraw = true;

				this.possibleConnectionPoints = this.GetPossibleConnectionPoints(planPoint);

				if (this.newConnectionStartData != null) {
					//KlimaFlaechenModul tmpModul;
					//KlimaFlaechenSubAreaVerbindung tmpVerbindung;
					//GraphicalConnectionAnbindungsPunkt tmpAnbindung;
					//bool tmpRowConnection;
					Nullable<PossibleConnectionPoint> tmpConnectionPoint;
					//this.nextConnectionPoints = this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out tmpModul, out tmpVerbindung, out tmpAnbindung, out tmpRowConnection);
					this.nextConnectionPoints = this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out tmpConnectionPoint);
				} else {
					this.nextConnectionPoints = new List<Point2D>();
				}

				/*if (this.newConnectionStart != null) {
					if (!this.newConnectionStartAtOutput) {
						this.possibleAnbindungspunkte = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, true, this.newConnectionCircuitDistributorIndex, this.newConnectionIgnoreDistributorIndices, planPoint);
					} else {
						this.possibleAnbindungspunkte = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, false, this.newConnectionCircuitDistributorIndex, this.newConnectionIgnoreDistributorIndices, planPoint);
					}
				}

				Dictionary<KlimaFlaechenModul, Polygon2D> areas = this.GetModuleAreas();
				KlimaFlaechenModul modul = null;
				foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> area in areas) {
					if (area.Value.IsInside(planPoint)) {
						modul = area.Key;
						break;
					}
				}
				int tmp;
				ModulDeckeCircuit currentCircuit = this.product.GetCircuitForModul(modul, out tmp);
				if (modul != null && (this.newConnectionStart == null || currentCircuit == this.product.GetCircuitForModul(this.newConnectionStart, out tmp))) {
					List<KlimaFlaechenModul> connectedModules;
					if (this.newConnectionStart != null) {
						connectedModules = currentCircuit.GetAllLinkedModules(this.newConnectionStart);
					} else {
						connectedModules = new List<KlimaFlaechenModul>();
					}
					if (!connectedModules.Contains(modul)) {
						this.hoveredModul = modul;
						this.hoverInput = this.AllowInput(modul);
						this.hoverOutput = this.AllowOutput(modul);
					} else {
						this.hoveredModul = null;
						this.hoverInput = false;
						this.hoverOutput = false;
					}
				} else {
					this.hoveredModul = null;
					this.hoverInput = false;
					this.hoverOutput = false;
				}

				this.hoverAnbindungen = this.newConnectionStart != null;
				
				if (this.newConnectionStart != null) {
					KlimaFlaechenModul tmpModul;
					KlimaFlaechenSubAreaVerbindung tmpVerbindung;
					GraphicalConnectionAnbindungsPunkt tmpAnbindung;
					bool tmpRowConnection;
					this.nextConnectionPoints = this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out tmpModul, out tmpVerbindung, out tmpAnbindung, out tmpRowConnection);
				} else {
					this.nextConnectionPoints = new List<Point2D>();
				}*/
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
			return redraw;
		}

		private Dictionary<KlimaFlaechenModul, Polygon2D> GetModuleAreas() {
			Dictionary<KlimaFlaechenModul, Polygon2D> moduleAreas = new Dictionary<KlimaFlaechenModul, Polygon2D>();
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double height;
			double width;
			Matrix3D laneRotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
			Matrix3D moduleRotation = Transformation3D.Rotate(this.product.GraphConstruction.Rotation * Math.PI / 180.0);
			foreach (ModulDeckeCircuit c in this.product.PlannedCircuits) {
				foreach (ModulDeckeSubArea sa in c.SubAreas) {
					foreach (KlimaFlaechenList row in sa.Rows) {
						foreach (KlimaFlaechenModul modul in row.List) {
							height = KlimaFlaechenModul.GetModuleHeight(modul.ModulType) * measure;
							width = KlimaFlaechenModul.GetModuleWidth(modul.ModulType) * measure;
							double x = laneRotation.Transform(this.product.GraphConstruction.PossibleLanes[modul.GraphLane].BorderLeft.Origin).X;
							double y = modul.GraphPositionInLan;
							Matrix3D transformation = moduleRotation * Transformation3D.Translation(x, y);
							Polygon2D modulArea = new Polygon2D();
							modulArea.Add(transformation.Transform(new Point2D(0, 0)));
							modulArea.Add(transformation.Transform(new Point2D(width, 0)));
							modulArea.Add(transformation.Transform(new Point2D(width, height)));
							modulArea.Add(transformation.Transform(new Point2D(0, height)));
							moduleAreas.Add(modul, modulArea);
						}
					}
				}
			}

			return moduleAreas;
		}

		private Point2D layoutAddAreaStart;
		private PointF layoutAddAreaStartScreen;
		private Polygon2D layoutAddArea = null;
		private bool layoutAddAreaBottomUp = false;

		private Nullable<Point> dragStartedInControl;
		private Nullable<Point2D> dragStartedInPlan;
		private Nullable<Point> dragEndedInControl;
		private Nullable<Point2D> dragEndedInPlan;
		//private bool dragIsPick;
		private Dictionary<KlimaFlaechenModul, double> oldModulPositions;
		private bool moveModules = false;

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == KlimaDeckeMode.KDM_CONSTRUCTION) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					this.product.GraphConstruction.StartDrag(planPoint, pointInControl);
				}
			} else if (this.Mode == KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
				this.layoutAddAreaStart = planPoint;
				this.layoutAddAreaStartScreen = pointInControl;
			} else if (this.Mode == KlimaDeckeMode.KDM_PICK_MODULE) {
				if (!this.ShiftPressed) {
					Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
					Point2D rotatedPoint = rotation.Transform(planPoint);
					double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					KlimaFlaechenModul pickedModul = null;
					foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
						Point2D left = rotation.Transform(lane.BorderLeft.Origin);
						Point2D right = rotation.Transform(lane.BorderRight.Origin);
						if (rotatedPoint.X >= left.X && rotatedPoint.X <= right.X) {
							List<KlimaFlaechenModul> modules = this.product.GetModulesInLane(lane.Nr);
							foreach (KlimaFlaechenModul module in modules) {
								if (rotatedPoint.Y >= module.GraphPositionInLan && rotatedPoint.Y <= module.GraphPositionInLan + KlimaFlaechenModul.GetModuleHeight(module.ModulType) * measure) {
									pickedModul = module;
									break;
								}
							}
							if (pickedModul != null) {
								break;
							}
						}
					}
					this.moveModules = this.GetAllSelectedModules().Contains(pickedModul);
					this.dragStartedInControl = pointInControl;
					this.dragStartedInPlan = planPoint;
					if (this.moveModules) {
						oldModulPositions = new Dictionary<KlimaFlaechenModul, double>();
						foreach (KlimaFlaechenModul modul in this.GetAllSelectedModules()) {
							oldModulPositions.Add(modul, modul.GraphPositionInLan);
						}
					}
					//dragIsPick = true;
				} else {
					this.moveModules = false;
					this.dragStartedInControl = pointInControl;
					this.dragStartedInPlan = planPoint;
					//dragIsPick = true;
				}
			}
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == KlimaDeckeMode.KDM_CONSTRUCTION) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					this.product.GraphConstruction.MoveDrag(planPoint, pointInControl);
					if (this.projectChanged != null) {
						this.projectChanged(this);
					}
					return true;
				}
			} else if (this.Mode == KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					if (alignRectangle) {
						Matrix3D matrix = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
						Point2D rotatedP1 = matrix.Transform(layoutAddAreaStart);
						Point2D rotatedP3 = matrix.Transform(planPoint);
						this.layoutAddAreaBottomUp = (rotatedP1.Y > rotatedP3.Y);
						Point2D rotatedP2 = new Point2D(rotatedP1.X, rotatedP3.Y);
						Point2D rotatedP4 = new Point2D(rotatedP3.X, rotatedP1.Y);
						matrix = matrix.GetInverse();
						//Point2D p2 = matrix.Transform(rotatedP2);
						//Point2D p4 = matrix.Transform(rotatedP4);
						layoutAddArea = new Polygon2D();
						layoutAddArea.Add(layoutAddAreaStart);
						layoutAddArea.Add(matrix.Transform(rotatedP2));
						layoutAddArea.Add(planPoint);
						layoutAddArea.Add(matrix.Transform(rotatedP4));
						return true;
					} else {
						Matrix3D matrix = Transformation3D.Rotate(this.product.AssociatedRoom.AssociatedPlan.Rotation * Math.PI / 180.0);
						Point2D rotatedP1 = matrix.Transform(layoutAddAreaStart);
						Point2D rotatedP3 = matrix.Transform(planPoint);
						//this.layoutAddAreaBottomUp = this.layoutAddAreaStartScreen.X < pointInControl.X;
						if (this.product.GraphConstruction.RotationRelativeToPlan <= 45 || this.product.GraphConstruction.RotationRelativeToPlan > 135) {
							if (this.product.GraphConstruction.RotationRelativeToPlan > 135) {
								this.layoutAddAreaBottomUp = this.layoutAddAreaStartScreen.Y < pointInControl.Y;
							} else {
								this.layoutAddAreaBottomUp = this.layoutAddAreaStartScreen.Y > pointInControl.Y;
							}
						} else {
							this.layoutAddAreaBottomUp = this.layoutAddAreaStartScreen.X < pointInControl.X;
						}
						Point2D rotatedP2 = new Point2D(rotatedP1.X, rotatedP3.Y);
						Point2D rotatedP4 = new Point2D(rotatedP3.X, rotatedP1.Y);
						matrix = matrix.GetInverse();
						//Point2D p2 = matrix.Transform(rotatedP2);
						//Point2D p4 = matrix.Transform(rotatedP4);
						layoutAddArea = new Polygon2D();
						layoutAddArea.Add(layoutAddAreaStart);
						layoutAddArea.Add(matrix.Transform(rotatedP2));
						layoutAddArea.Add(planPoint);
						layoutAddArea.Add(matrix.Transform(rotatedP4));
						return true;

						/*layoutAddArea = new Polygon2D();
						layoutAddArea.Add(layoutAddAreaStart);
						layoutAddArea.Add(new Point2D(layoutAddAreaStart.X, planPoint.Y));
						layoutAddArea.Add(planPoint);
						layoutAddArea.Add(new Point2D(planPoint.X, layoutAddAreaStart.Y));
						return true;*/
					}
				}
			} else if (this.Mode == KlimaDeckeMode.KDM_PICK_MODULE && button == MouseButtons.Left) {
				/*int deltaX = this.dragStartedInControl.X - pointInControl.X;
				int deltaY = this.dragStartedInControl.Y - pointInControl.Y;*/
				Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);

				/*if (deltaX * deltaX + deltaY * deltaY > 25) {
					dragIsPick = false;
				}*/
				if (/*!dragIsPick && */moveModules) {
					Point2D rotatedStartPoint = rotation.Transform(this.dragStartedInPlan.Value);
					Point2D rotatedCurPoint = rotation.Transform(planPoint);
					double delta = rotatedCurPoint.Y - rotatedStartPoint.Y;
					double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					// TODO move modules
					Dictionary<int, List<KlimaFlaechenModul>> modulesPerLane = new Dictionary<int, List<KlimaFlaechenModul>>();
					foreach (KlimaFlaechenModul modul in this.GetAllSelectedModules()) {
						if (!modulesPerLane.ContainsKey(modul.GraphLane)) {
							modulesPerLane.Add(modul.GraphLane, new List<KlimaFlaechenModul>());
						}
						modulesPerLane[modul.GraphLane].Add(modul);
					}
					//Dictionary<KlimaFlaechenModul, double> move = new Dictionary<KlimaFlaechenModul, double>();
					foreach (KeyValuePair<int, List<KlimaFlaechenModul>> kvp in modulesPerLane) {
						if (kvp.Value.Count > 0) {
							KlimaFlaechenModul firstModul = kvp.Value[0];
							bool bottomUp = firstModul.GraphPositionInLan < this.oldModulPositions[firstModul] + delta;
							kvp.Value.Sort(new KlimaFlaechenModuleComparer(!bottomUp));
							PossibleModulLane lane = this.product.GraphConstruction.PossibleLanes[kvp.Key];
							foreach (KlimaFlaechenModul modul in kvp.Value) {
								Nullable<double> bestMove = lane.BestMovePossible(modul, this.oldModulPositions[modul] + delta, measure, this.product, bottomUp);
								if (bestMove.HasValue) {
									modul.GraphPositionInLan = bestMove.Value;
									//move.Add(modul, bestMove.Value - this.oldModulPositions[modul]);
								}
							}
						}
					}

					foreach (KlimaFlaechenModulVerbindung link in this.updateStartOnMove) {
						link.Vertices[0] = link.Start.GetOutputConnection(measure, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);
					}
					foreach (KlimaFlaechenModulVerbindung link in this.updateEndOnMove) {
						link.Vertices[1] = link.End.GetInputConnection(measure, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);
					}
					foreach (KlimaFlaechenModulVerbindung link in this.deleteOnMove) {
						(link.Circuit as ModulDeckeCircuit).SubAreas[link.EndSubarea].Rows[link.EndRow].Links.Remove(link);
					}
					this.deleteOnMove.Clear();
					foreach (KlimaFlaechenSubAreaVerbindung link in this.deleteSaOnMove) {
						(link.Circuit as ModulDeckeCircuit).Links.Remove(link);
					}
					this.deleteSaOnMove.Clear();
					
					if (this.projectChanged != null) {
						this.projectChanged(this);
					}
					this.connectedPlanPanel.InvalidateGraphics();
				} else {
					this.dragEndedInControl = pointInControl;
					this.dragEndedInPlan = planPoint;
					this.connectedPlanPanel.InvalidateGraphics();
				}
			}
			return false;
		}

		public class KlimaFlaechenModuleComparer : IComparer<KlimaFlaechenModul> {
			private bool ascending;

			public KlimaFlaechenModuleComparer(bool ascending) {
				this.ascending = ascending;
			}

			#region IComparer<KlimaFlaechenModul> Members
			public int Compare(KlimaFlaechenModul x, KlimaFlaechenModul y) {
				int result;
				if (ascending) {
					result = x.GraphLane.CompareTo(y.GraphLane);
					if (result == 0) {
						result = x.GraphPositionInLan.CompareTo(y.GraphPositionInLan);
					}
				} else {
					result = y.GraphLane.CompareTo(x.GraphLane);
					if (result == 0) {
						result = y.GraphPositionInLan.CompareTo(x.GraphPositionInLan);
					}
				}
				return result;
			}
			#endregion
		}

		public class KlimaFlaechenModuleWithRowAndCircuitComparer : IComparer<KlimaFlaechenModulWithRowAndCircuit> {
			private bool ascending;

			public KlimaFlaechenModuleWithRowAndCircuitComparer(bool ascending) {
				this.ascending = ascending;
			}

			#region IComparer<KlimaFlaechenModulWithRowAndCircuit> Members
			public int Compare(KlimaFlaechenModulWithRowAndCircuit x, KlimaFlaechenModulWithRowAndCircuit y) {
				int result;
				if (ascending) {
					result = x.modul.GraphLane.CompareTo(y.modul.GraphLane);
					if (result == 0) {
						result = x.modul.GraphPositionInLan.CompareTo(y.modul.GraphPositionInLan);
					}
				} else {
					result = y.modul.GraphLane.CompareTo(x.modul.GraphLane);
					if (result == 0) {
						result = y.modul.GraphPositionInLan.CompareTo(x.modul.GraphPositionInLan);
					}
				}
				return result;
			}
			#endregion
		}

		internal bool ShiftPressed {
			get { return (Control.ModifierKeys & (Keys.Shift | Keys.ShiftKey | Keys.LShiftKey | Keys.RShiftKey)) != Keys.None; }
		}

		private Random random = new Random((int)DateTime.Now.Ticks);

		private Color GetNewCircuitColor() {
			Dictionary<Color, int> dict = new Dictionary<Color, int>();
			foreach (Color c in this.circuitColors) {
				dict.Add(c, 0);
			}
			foreach (ModulDeckeCircuit circuit in this.product.PlannedCircuits) {
				if (dict.ContainsKey(circuit.CircuitColor)) {
					dict[circuit.CircuitColor]++;
				}
			}
			int minValue = Int32.MaxValue;
			Color newColor = this.circuitColors[0];
			foreach (KeyValuePair<Color, int> kvp in dict) {
				if (kvp.Value < minValue) {
					newColor = kvp.Key;
					minValue = kvp.Value;
				}
			}
			return newColor;
			//return Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
		}

		public bool PlannerDragEnd(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			// TODO
			if (this.mode == KlimaDeckeMode.KDM_LAYOUT_ADD_AREA) {
				if (this.layoutAddArea != null) {
					ModulDeckeSubArea newSubArea = null;
					ModulDeckeCircuit newCircuit = null;
					ModulDeckeSubArea oldSubArea = null;
					KlimaFlaechenList oldRow = null;
					ModulDeckeCircuit circuitToAdd = this.highlightCircuit;
					bool onlyAddToExisting = false;
					if (this.highlightCircuit == null && highlightSubArea == null && highlightRow == null) {
						if (this.product.Connections != null && this.product.Connections.Count > 0) {
							if (MessageBox.Show(EuroplanRes.ModulKlimaBodenPlanner_AnbindeleitungLoeschen, EuroplanRes.ModulKlimaBodenPlanner_NeuerHeizkreisTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
								//this.layoutAddArea = null;
								//return true;
								onlyAddToExisting = true;
							} else {
								this.product.Connections.Clear();
							}
						}
						newCircuit = new ModulDeckeCircuit(this.Product);
						newCircuit.CircuitColor = this.GetNewCircuitColor();
						newSubArea = newCircuit.SubAreas[0];
						newSubArea.Rows.Clear();
						circuitToAdd = newCircuit;
					} else if (this.highlightSubArea == null && highlightRow == null) {
						newSubArea = new ModulDeckeSubArea();
						newSubArea.Rows.Clear();
					} else if (this.highlightRow == null) {
						oldSubArea = this.highlightSubArea;
					} else {
						oldRow = this.highlightRow;
					}
					Dictionary<PossibleModulLane, KlimaFlaechenList> laneToRowMapping = new Dictionary<PossibleModulLane, KlimaFlaechenList>();
					List<KlimaFlaechenModul> modulesAdded = new List<KlimaFlaechenModul>();
					int count = this.AddModulesForLayoutArea(delegate(ref double y, double start, double end, double step, ref bool left, Matrix3D invRotation, PossibleModulLane lane, Point2D borderLeftOrigin, out bool added, out KlimaFlaechenModul addedModul, out KlimaFlaechenList rowOfAddedModul, KlimaFlaechenModul lastAddedModul, KlimaFlaechenList rowOfLastAddedModul) {
						added = this.TryAddModule(this.ConnectedPlanPanel.PlanTransformation, ref y, start, end, step, ref left, this.layoutAddAreaBottomUp, invRotation, lane, borderLeftOrigin, laneToRowMapping, (newSubArea != null ? newSubArea : oldSubArea), oldRow, modulesAdded, this.automaticRows, onlyAddToExisting, out addedModul, out rowOfAddedModul, lastAddedModul, rowOfLastAddedModul, circuitToAdd);
					});
					if (this.UpdateNewCount != null) {
						this.UpdateNewCount(this, new UpdateNewCountArgs(0));
					}

					if (count > 0) {
						if (!this.product.ContainsModules && newCircuit != null) {
							this.product.PlannedCircuits.Clear();
						}
						if (newCircuit != null && newCircuit.CountModules() > 0) {
							this.product.PlannedCircuits.Add(newCircuit);
						} else if (newSubArea != null && newSubArea.CountModules() > 0) {
							this.highlightCircuit.SubAreas.Add(newSubArea);
						}
						if (this.automaticOrientation) {
							foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
								List<KlimaFlaechenModulWithRowAndCircuit> modules = this.product.GetModulesInLaneWithRowAndCircuit(lane.Nr);
								modules.Sort(new KlimaFlaechenModuleWithRowAndCircuitComparer(false));
								Nullable<bool> left = null;
								bool lastDiagonal = true;
								KlimaFlaechenList lastRow = null;
								foreach (KlimaFlaechenModulWithRowAndCircuit moduleWithRow in modules) {
									if (left.HasValue && lastRow == moduleWithRow.row && modulesAdded.Contains(moduleWithRow.modul)) {
										//moduleWithRow.modul.Orientation = (left.Value == moduleWithRow.modul.DiagonalDurchstroemt) ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
										bool trueLeft = left.Value;
										if (lastDiagonal != moduleWithRow.modul.DiagonalDurchstroemt) {
											trueLeft = !trueLeft;
										}
										if (!moduleWithRow.modul.DiagonalDurchstroemt && moduleWithRow.modul.GraphBottomUp) {
											trueLeft = !trueLeft;
										}
										moduleWithRow.modul.Orientation = trueLeft ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
										if (moduleWithRow.modul.DiagonalDurchstroemt) {
											left = !left;
										}
										lastDiagonal = moduleWithRow.modul.DiagonalDurchstroemt;
									} else {
										lastRow = moduleWithRow.row;
										lastDiagonal = moduleWithRow.modul.DiagonalDurchstroemt;
										if (moduleWithRow.modul.DiagonalDurchstroemt) {
											left = moduleWithRow.modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
										} else {
											if (!moduleWithRow.modul.GraphBottomUp) {
												left = moduleWithRow.modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
											} else {
												left = moduleWithRow.modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
											}
										}
									}
								}
								modules.Sort(new KlimaFlaechenModuleWithRowAndCircuitComparer(true));
								left = null;
								lastRow = null;
								foreach (KlimaFlaechenModulWithRowAndCircuit moduleWithRow in modules) {
									if (left.HasValue && lastRow == moduleWithRow.row && modulesAdded.Contains(moduleWithRow.modul)) {
										//moduleWithRow.modul.Orientation = (left.Value == moduleWithRow.modul.DiagonalDurchstroemt) ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
										bool trueLeft = left.Value;
										if (lastDiagonal != moduleWithRow.modul.DiagonalDurchstroemt) {
											trueLeft = !trueLeft;
										}
										if (!moduleWithRow.modul.DiagonalDurchstroemt && !moduleWithRow.modul.GraphBottomUp) {
											trueLeft = !trueLeft;
										}
										moduleWithRow.modul.Orientation = trueLeft ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
										if (moduleWithRow.modul.DiagonalDurchstroemt) {
											left = !left;
										}
										lastDiagonal = moduleWithRow.modul.DiagonalDurchstroemt;
									} else {
										lastRow = moduleWithRow.row;
										lastDiagonal = moduleWithRow.modul.DiagonalDurchstroemt;
										if (moduleWithRow.modul.DiagonalDurchstroemt) {
											left = moduleWithRow.modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
										} else {
											if (moduleWithRow.modul.GraphBottomUp) {
												left = moduleWithRow.modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
											} else {
												left = moduleWithRow.modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
											}
										}
									}
								}
							}
						}
						/*foreach (ModulDeckeCircuit circuit in this.product.PlannedCircuits) {
							foreach (ModulDeckeSubArea subArea in circuit.SubAreas) {
								foreach (KlimaFlaechenList row in subArea.Rows) {
									if (row.List.Count > 1) {
										row.List.Sort(new KlimaFlaechenModuleComparer(true));
										bool left = (row.List[0].Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
										foreach (KlimaFlaechenModul modul in row.List) {
											modul.Orientation = (left ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT);
											left = !left;
										}
									}
								}
							}
						}*/
						if (this.projectChanged != null) {
							this.projectChanged(this);
						}
						if (this.ListsNeedUpdate != null) {
							this.ListsNeedUpdate(this, new ListNeedsUpdateEventArgs(newCircuit != null && newCircuit.CountModules() > 0));
						}
					}

					this.layoutAddArea = null;
					return true;
				}
			} else if (this.mode == KlimaDeckeMode.KDM_PICK_MODULE) {
				if (!this.moveModules) {
					if (this.product == null || this.product.AssociatedRoom == null ||
						this.product.AssociatedRoom.AssociatedPlan == null || this.product.AssociatedRoom.AssociatedPlan.Measure == null) {
						this.dragStartedInPlan = null;
						this.dragStartedInControl = null;
						this.dragEndedInPlan = null;
						this.dragEndedInControl = null;
						return false;
					}

					this.dragEndedInControl = pointInControl;
					this.dragEndedInPlan = planPoint;

					Matrix3D rotation = Transformation3D.Rotate(this.Product.AssociatedRoom.AssociatedPlan.Rotation * Math.PI / 180.0);
					Point2D rotatedStart = rotation.Transform(this.dragStartedInPlan.Value);
					Point2D rotatedEnd = rotation.Transform(this.dragEndedInPlan.Value);
					Matrix3D invRotation = rotation.GetInverse();
					Polygon2D selectedPoly = new Polygon2D();
					selectedPoly.Add(this.dragStartedInPlan.Value);
					selectedPoly.Add(invRotation.Transform(new Point2D(rotatedStart.X, rotatedEnd.Y)));
					selectedPoly.Add(this.dragEndedInPlan.Value);
					selectedPoly.Add(invRotation.Transform(new Point2D(rotatedEnd.X, rotatedStart.Y)));
					List<KlimaFlaechenModul> modules = GetModuleInPoly(selectedPoly);
					if (this.ShiftPressed) {
						if (this.HighlightModules == null) {
							this.HighlightModules = this.GetAllSelectedModules();
						}
						if (modules.Count == 1) {
							if (this.HighlightModules.Contains(modules[0])) {
								this.HighlightModules.Remove(modules[0]);
							} else {
								this.HighlightModules.Add(modules[0]);
							}
						} else {
							foreach (KlimaFlaechenModul modul in modules) {
								if (!this.HighlightModules.Contains(modul)) {
									this.HighlightModules.Add(modul);
								}
							}
						}
					} else {
						this.HighlightModules = modules;
					}

					this.updateStartOnMove.Clear();
					this.updateEndOnMove.Clear();
					this.deleteOnMove.Clear();
					this.deleteSaOnMove.Clear();
					foreach (ModulDeckeCircuit c in this.product.PlannedCircuits) {
						foreach (ModulDeckeSubArea sa in c.SubAreas) {
							foreach (KlimaFlaechenList row in sa.Rows) {
								foreach (KlimaFlaechenModulVerbindung link in row.Links) {
									if (link.Vertices.Count == 2) {
										if (this.HighlightModules.Contains(link.Start)) {
											this.updateStartOnMove.Add(link);
										}
										if (this.HighlightModules.Contains(link.End)) {
											this.updateEndOnMove.Add(link);
										}
									} else {
										this.deleteOnMove.Add(link);
									}
								}
							}
						}
						foreach (KlimaFlaechenSubAreaVerbindung link in c.Links) {
							bool deleted = false;
							foreach (KlimaFlaechenModul m in link.Start) {
								if (this.HighlightModules.Contains(m)) {
									this.deleteSaOnMove.Add(link);
									deleted = true;
									break;
								}
							}
							if (!deleted) {
								foreach (KlimaFlaechenModul m in link.End) {
									if (this.HighlightModules.Contains(m)) {
										this.deleteSaOnMove.Add(link);
										break;
									}
								}
							}
						}
					}

					this.dragStartedInPlan = null;
					this.dragStartedInControl = null;
					this.dragEndedInPlan = null;
					this.dragEndedInControl = null;

					this.ConnectedPlanPanel.InvalidateGraphics();
					this.ModuleSelected(this, new ModuleSelectedEventArgs(modules));

					/*KlimaFlaechenModul module = GetModuleAtPoint(planPoint);
					if (module != null) {
						if (this.ShiftPressed) {
							if (this.HighlightModules == null) {
								this.HighlightModules = this.GetAllSelectedModules();
							}
							if (this.HighlightModules.Contains(module)) {
								this.HighlightModules.Remove(module);
							} else {
								this.HighlightModules.Add(module);
							}
							if (this.ModuleSelected != null) {
								this.ModuleSelected(this, new ModuleSelectedEventArgs(this.HighlightModules));
							}
						} else {
							if (this.HighlightModules == null) {
								this.HighlightModules = new List<KlimaFlaechenModul>();
							} else {
								this.HighlightModules.Clear();
							}
							this.HighlightModules.Add(module);
							if (this.ModuleSelected != null) {
								this.ModuleSelected(this, new ModuleSelectedEventArgs(module));
							}
						}
					}

					if (module == null && !this.ShiftPressed) {
						this.HighlightModules = null;
						if (this.ModuleSelected != null) {
							this.ModuleSelected(this, new ModuleSelectedEventArgs());
						}
						return true;
					} else if (module != null) {
						return true;
					}*/
				}
			}
			return false;
		}

		private KlimaFlaechenModul GetModuleAtPoint(Point2D planPoint) {
			Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
			Point2D rotatedPoint = rotation.Transform(planPoint);
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
				Point2D left = rotation.Transform(lane.BorderLeft.Origin);
				Point2D right = rotation.Transform(lane.BorderRight.Origin);
				if (rotatedPoint.X >= left.X && rotatedPoint.X <= right.X) {
					List<KlimaFlaechenModul> modules = this.product.GetModulesInLane(lane.Nr);
					foreach (KlimaFlaechenModul module in modules) {
						if (rotatedPoint.Y >= module.GraphPositionInLan && rotatedPoint.Y <= module.GraphPositionInLan + KlimaFlaechenModul.GetModuleHeight(module.ModulType) * measure) {
							return module;
						}
					}
				}
			}
			return null;
		}

		private List<KlimaFlaechenModul> GetModuleInPoly(Polygon2D poly) {
			Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
			//Matrix3D invRotation = rotation.GetInverse();
			Polygon2D rotatedPoly = new Polygon2D();
			foreach (Point2D point in poly) {
				rotatedPoly.Add(rotation.Transform(point));
			}
			/*Point2D topLeft = new Point2D(startPoint.X < endPoint.X ? startPoint.X : endPoint.X, startPoint.Y < endPoint.Y ? startPoint.Y : endPoint.Y);
			Point2D bottomRight = new Point2D(startPoint.X >= endPoint.X ? startPoint.X : endPoint.X, startPoint.Y >= endPoint.Y ? startPoint.Y : endPoint.Y);
			Point2D rotatedTopLeft = rotation.Transform(topLeft);
			Point2D rotatedBottomRight = rotation.Transform(bottomRight);*/
			double height;
			double width;

			List<KlimaFlaechenModul> pickedModules = new List<KlimaFlaechenModul>();

			foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
				double left = rotation.Transform(lane.BorderLeft.Origin).X;

				List<KlimaFlaechenModul> modules = lane.GetModulesInThisLane(this.product);
				foreach (KlimaFlaechenModul modul in modules) {
					height = KlimaFlaechenModul.GetModuleHeight(modul.ModulType) * this.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
					width = KlimaFlaechenModul.GetModuleWidth(modul.ModulType) * this.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
					Polygon2D modulPoly = new Polygon2D(new Point2D[] { new Point2D(left, modul.GraphPositionInLan), new Point2D(left, modul.GraphPositionInLan + height), new Point2D(left + width, modul.GraphPositionInLan + height), new Point2D(left + width, modul.GraphPositionInLan) });

					if (rotatedPoly.IsInside(new Point2D(left, modul.GraphPositionInLan)) &&
						rotatedPoly.IsInside(new Point2D(left, modul.GraphPositionInLan + height)) &&
						rotatedPoly.IsInside(new Point2D(left + width, modul.GraphPositionInLan)) &&
						rotatedPoly.IsInside(new Point2D(left + width, modul.GraphPositionInLan + height))) {
						pickedModules.Add(modul);
						continue;
					}
					if (modulPoly.IsInside(rotatedPoly[0]) && modulPoly.IsInside(rotatedPoly[1]) && modulPoly.IsInside(rotatedPoly[2]) && modulPoly.IsInside(rotatedPoly[3])) {
						pickedModules.Add(modul);
						continue;
					}
					/*if (rotatedPoly.IsInside(new Point2D(left, modul.GraphPositionInLan + height))) {
						pickedModules.Add(modul);
						continue;
					}
					if (rotatedPoly.IsInside(new Point2D(left + width, modul.GraphPositionInLan))) {
						pickedModules.Add(modul);
						continue;
					}
					if (rotatedPoly.IsInside(new Point2D(left + width, modul.GraphPositionInLan + height))) {
						pickedModules.Add(modul);
						continue;
					}*/
				}
			}
			return pickedModules;

			/*foreach (ModulDeckeCircuit circuit in this.Product.PlannedCircuits) {
				foreach (ModulDeckeSubArea subArea in circuit) {
					foreach (KlimaFlaechenList row in subArea.Rows) {
						foreach (KlimaFlaechenModul modul in row.List) {
							height = KlimaFlaechenModul.GetModuleHeight(modul.ModulType) * this.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
							width = KlimaFlaechenModul.GetModuleWidth(modul.ModulType) * this.Product.AssociatedRoom.AssociatedPlan.Measure.Value;
						}
					}
				}
			}
			throw new Exception("The method or operation is not implemented.");*/
		}

		public Cursor CustomCursor {
			get { return this.customCursor; }
		}

		public bool PlannerKeyPress(Keys key) {
			return KeyDown(key, this.GetAllSelectedModules());
		}

		public void DrawModule(KlimaFlaechenModul.ModulTypeEnum type, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, Point2D position, Matrix4D additionalTransformation, Graphics g, bool bottomUp, bool highlight, Color circuitColor, bool highlightInput, bool highlightOutput) {
			if (this.product == null || this.product.GraphConstruction == null ||
				this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedPlan == null ||
				this.product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}
			additionalTransformation = additionalTransformation * Transformation4D.Translation(position.X, position.Y, 0);
			additionalTransformation = additionalTransformation * Transformation4D.RotateZ(this.product.GraphConstruction.Rotation * Math.PI / 180.0);

			double height = KlimaFlaechenModul.GetModuleHeight(type) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double width = KlimaFlaechenModul.GetModuleWidth(type) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

			Point2D topLeft2D = additionalTransformation.TransformTo2D(new Point2D(0, 0));
			Point2D topRight2D = additionalTransformation.TransformTo2D(new Point2D(width, 0));
			Point2D bottomRight2D = additionalTransformation.TransformTo2D(new Point2D(width, height));
			Point2D bottomLeft2D = additionalTransformation.TransformTo2D(new Point2D(0, height));

			Point2D input12D = Point2D.Zero;
			Point2D input22D = Point2D.Zero;
			Point2D input32D = Point2D.Zero;
			Point2D input42D = Point2D.Zero;
			Point2D output12D = Point2D.Zero;
			Point2D output22D = Point2D.Zero;
			Point2D output32D = Point2D.Zero;
			Point2D output42D = Point2D.Zero;

			Point2D directionTop12D;
			Point2D directionTop22D;
			Point2D directionTop32D;
			Point2D directionBottom12D;
			Point2D directionBottom22D;
			Point2D directionBottom32D;

			/*if (this.product.AssociatedRoom.AssociatedPlan is CadPlan) {
				if (bottomUp) {
					directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0));

					directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				} else {
					directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
					directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
					directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

					directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				}
			} else {
				if (bottomUp) {
					directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.20 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.20 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

					directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
					directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
					directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				} else {
					directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0.20 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

					directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
					directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height));
				}
			}*/
			if (this.product.AssociatedRoom.AssociatedPlan.InvertYAxis == bottomUp) {
				directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0));

				directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

				if (highlightInput || highlightOutput) {
					if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
						output12D = topLeft2D;
						output22D = additionalTransformation.TransformTo2D(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output32D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output42D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
						input12D = bottomRight2D;
						input22D = additionalTransformation.TransformTo2D(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input32D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input42D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
					} else if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
						output12D = topRight2D;
						output22D = additionalTransformation.TransformTo2D(new Point2D(width, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output32D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output42D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
						input12D = bottomLeft2D;
						input22D = additionalTransformation.TransformTo2D(new Point2D(0, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input32D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input42D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
					}
				}
			} else {
				directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
				directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
				directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

				directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

				if (highlightInput || highlightOutput) {
					if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
						input12D = topLeft2D;
						input22D = additionalTransformation.TransformTo2D(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input32D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input42D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
						output12D = bottomRight2D;
						output22D = additionalTransformation.TransformTo2D(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output32D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output42D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
					} else if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
						input12D = topRight2D;
						input22D = additionalTransformation.TransformTo2D(new Point2D(width, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input32D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input42D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
						output12D = bottomLeft2D;
						output22D = additionalTransformation.TransformTo2D(new Point2D(0, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output32D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output42D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
					}
				}
			}

			PointF topLeft = new PointF((float)topLeft2D.X, (float)topLeft2D.Y);
			PointF topRight = new PointF((float)topRight2D.X, (float)topRight2D.Y);
			PointF bottomRight = new PointF((float)bottomRight2D.X, (float)bottomRight2D.Y);
			PointF bottomLeft = new PointF((float)bottomLeft2D.X, (float)bottomLeft2D.Y);
			PointF middle = new PointF((topLeft.X + bottomRight.X) / 2, (topLeft.Y + bottomRight.Y) / 2);

			PointF directionTop1 = new PointF((float)directionTop12D.X, (float)directionTop12D.Y);
			PointF directionTop2 = new PointF((float)directionTop22D.X, (float)directionTop22D.Y);
			PointF directionTop3 = new PointF((float)directionTop32D.X, (float)directionTop32D.Y);

			PointF directionBottom1 = new PointF((float)directionBottom12D.X, (float)directionBottom12D.Y);
			PointF directionBottom2 = new PointF((float)directionBottom22D.X, (float)directionBottom22D.Y);
			PointF directionBottom3 = new PointF((float)directionBottom32D.X, (float)directionBottom32D.Y);

			Color c;
			if (highlight) {
				int cr = Math.Min((int)(circuitColor.R * 1.5) + 32, 255);
				int cg = Math.Min((int)(circuitColor.G * 1.5) + 32, 255);
				int cb = Math.Min((int)(circuitColor.B * 1.5) + 32, 255);
				c = Color.FromArgb(circuitColor.A / 2, cr, cg, cb);
			} else {
				c = Color.FromArgb(circuitColor.A / 2, circuitColor);
			}

			Pen p = new Pen(c);
			if (highlight) {
				p.Width = 1.5f;
			}
			Brush b = new SolidBrush(Color.FromArgb(c.A / 2, c));
			if ((type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 || type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) && bottomUp) {
				orientation = (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
			}
			if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
				g.FillPolygon(b, new PointF[] { topLeft, topRight, bottomRight, bottomLeft });
				if (type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 || type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) {
					g.DrawLines(p, new PointF[] { topRight, bottomRight, bottomLeft, topLeft, topRight, middle, bottomRight });
				} else {
					g.DrawLines(p, new PointF[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft, topRight });
				}
			} else if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
				g.FillPolygon(b, new PointF[] { topLeft, topRight, bottomRight, bottomLeft });
				if (type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 || type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) {
					g.DrawLines(p, new PointF[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft, middle, topLeft });
				} else {
					g.DrawLines(p, new PointF[] { topLeft, topRight, bottomRight, bottomLeft, topLeft, bottomRight });
				}
			} else {
				g.FillPolygon(b, new PointF[] { topLeft, topRight, bottomRight, bottomLeft });
				g.DrawLines(p, new PointF[] { topLeft, topRight, bottomRight, bottomLeft, topLeft });
			}

			if ((type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 || type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) && bottomUp) {
				orientation = (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT);
			}
			if (orientation != null) {
				if (highlight) {
					g.FillPolygon(b, new PointF[] { directionTop1, directionTop2, directionTop3 });
					g.FillPolygon(b, new PointF[] { directionBottom1, directionBottom2, directionBottom3 });
				} else {
					g.DrawPolygon(p, new PointF[] { directionTop1, directionTop2, directionTop3 });
					g.DrawPolygon(p, new PointF[] { directionBottom1, directionBottom2, directionBottom3 });
				}
				if (highlightInput) {
					PointF input1 = new PointF((float)input12D.X, (float)input12D.Y);
					PointF input2 = new PointF((float)input22D.X, (float)input22D.Y);
					PointF input3 = new PointF((float)input32D.X, (float)input32D.Y);
					PointF input4 = new PointF((float)input42D.X, (float)input42D.Y);

					Brush bInput = new SolidBrush(Color.FromArgb(127, Color.Red));
					Pen pInput = new Pen(Color.Red);
					//if (this.newConnectionStart == null || this.newConnectionStartAtOutput) {
						g.FillPolygon(bInput, new PointF[] { input1, input2, input3, input4 });
						g.DrawPolygon(pInput, new PointF[] { input1, input2, input3, input4 });
					//}
				}
				if (highlightOutput) {
					PointF output1 = new PointF((float)output12D.X, (float)output12D.Y);
					PointF output2 = new PointF((float)output22D.X, (float)output22D.Y);
					PointF output3 = new PointF((float)output32D.X, (float)output32D.Y);
					PointF output4 = new PointF((float)output42D.X, (float)output42D.Y);

					Brush bOutput = new SolidBrush(Color.FromArgb(127, Color.Blue));
					Pen pOutput = new Pen(Color.Blue);
					//if (this.newConnectionStart == null || !this.newConnectionStartAtOutput) {
						g.FillPolygon(bOutput, new PointF[] { output1, output2, output3, output4 });
						g.DrawPolygon(pOutput, new PointF[] { output1, output2, output3, output4 });
					//}
				}
			}

			GraphicsPath path = new GraphicsPath();

			Matrix oldTransform = g.Transform;
			Matrix newTransform = g.Transform.Clone();
			g.Transform = new Matrix();
			
			if (this.product.AssociatedRoom.AssociatedPlan is CadPlan) {
				newTransform.RotateAt(-(float)(this.product.GraphConstruction.Rotation), bottomLeft);
			} else {
				newTransform.RotateAt((float)(this.product.GraphConstruction.Rotation), topLeft);
			}
			g.Transform = newTransform;

			string moduleString = "";
			switch (type) {
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30:
					moduleString = EuroplanRes.KlimaFlaechenModul_100_30_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40:
					moduleString = EuroplanRes.KlimaFlaechenModul_100_40_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30:
					moduleString = EuroplanRes.KlimaFlaechenModul_120_30_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60B_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60C_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60D_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30:
					moduleString = EuroplanRes.KlimaFlaechenModul_80_30_Short;
					break;
			}

			if (this.product.AssociatedRoom.AssociatedPlan is CadPlan) {
				g.DrawString(moduleString, new Font("Arial", 5.0f / g.DpiX * Math.Abs((float)additionalTransformation.M22) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value), new SolidBrush(Color.FromArgb(255, c)), bottomLeft);
				g.Transform = oldTransform;
			} else {
				g.DrawString(moduleString, new Font("Arial", 5.0f / g.DpiX * Math.Abs((float)additionalTransformation.M22) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value), new SolidBrush(Color.FromArgb(255, c)), topLeft);
				g.Transform = oldTransform;
			}
		}
		#endregion

		#region layoutAddArea
		private KlimaFlaechenModul.ModulTypeEnum moduleTypeToAdd = KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30;
		private KlimaFlaechenModul.ModulOrientationEnum startingOrientation = KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
		private bool optimalLayout = true;
		private bool alignRectangle = true;

		public KlimaFlaechenModul.ModulTypeEnum ModuleTypeToAdd {
			get { return this.moduleTypeToAdd; }
			set { this.moduleTypeToAdd = value; }
		}

		public KlimaFlaechenModul.ModulOrientationEnum StartingOrientation {
			get { return this.startingOrientation; }
			set { this.startingOrientation = value; }
		}

		public bool OptimalLayout {
			get { return this.optimalLayout; }
			set { this.optimalLayout = value; }
		}

		public bool AlignRectangle {
			get { return this.alignRectangle; }
			set { this.alignRectangle = value; }
		}
		#endregion layoutAddArea

        private event ProjectChangedHandler projectChanged;
        public event ProjectChangedHandler ProjectChanged {
            add { this.projectChanged += value; }
            remove { this.projectChanged -= value; }
        }

		[DefaultValue(true)]
		public bool DrawBeplankung {
			get { return this.drawBeplankung; }
			set { this.drawBeplankung = value; }
		}

		[DefaultValue(true)]
		public bool HighlightRoomCoordinates {
			get { return this.highlightRoomCoordinates; }
			set { this.highlightRoomCoordinates = value; }
		}


		internal void DrawDxf(WW.Cad.Model.DxfModel model, DxfLayer modulLayer, DxfLayer constructionLayer, DxfLayer beplankungLayer) {
			Matrix4D additionalTransformation = Matrix4D.Identity;
						
			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.CeilingCoordinatesToUse != null) {
				if (this.product.AssociatedRoom.AssociatedPlan != null && this.product.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
					if (this.product.GraphConstruction != null) {
						this.product.GraphConstruction.PaintDxf(model, constructionLayer, beplankungLayer, this.drawBeplankung);
					}
				}
				
				if (this.mode != KlimaDeckeMode.KDM_CONSTRUCTION) {
					foreach (PossibleModulLane lane in this.product.GraphConstruction.PossibleLanes) {
						Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
						Matrix3D invRotation = rotation.GetInverse();

						double left = rotation.Transform(lane.BorderLeft.Origin).X;

						List<KlimaFlaechenModulWithRowAndCircuit> modules = this.product.GetModulesInLaneWithRowAndCircuit(lane.Nr);
						foreach (KlimaFlaechenModulWithRowAndCircuit mrc in modules) {
							this.DrawDxfModule(mrc.modul.ModulType, mrc.modul.Orientation, invRotation.Transform(new Point2D(left, mrc.modul.GraphPositionInLan)), additionalTransformation, model, modulLayer, mrc.modul.GraphBottomUp, mrc.circuit.CircuitColor);
						}
					}
				}

				foreach (ModulDeckeCircuit c in this.product.PlannedCircuits) {
					if (c.Links != null) {
						foreach (KlimaFlaechenSubAreaVerbindung link in c.Links) {
							link.DrawDxf(model, modulLayer, c.CircuitColor);
						}
					}
					foreach (ModulDeckeSubArea sa in c.SubAreas) {
						foreach (KlimaFlaechenList row in sa.Rows) {
							if (row.Links != null) {
								foreach (KlimaFlaechenModulVerbindung link in row.Links) {
									link.DrawDxf(model, modulLayer, c.CircuitColor);
								}
							}
						}
					}
				}

				if (this.product.AssociatedRoom.CeilingUnusedAreaCoordinates != null) {
					EntityColor gray = EntityColor.CreateFromRgb(Color.Gray.ToArgb());
					foreach (List<Point2D> unusedArea in this.product.AssociatedRoom.CeilingUnusedAreaCoordinates) {
						Polygon2D polygon = new Polygon2D(unusedArea);
						DxfPolyline2D polyLine = new DxfPolyline2D(gray, polygon.ToArray());
						polyLine.Closed = true;
						polyLine.Layer = modulLayer;
						model.Entities.Add(polyLine);
					}
				}
			}		
		}

		private void DrawDxfModule(KlimaFlaechenModul.ModulTypeEnum type, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, Point2D position, Matrix4D additionalTransformation, WW.Cad.Model.DxfModel model, DxfLayer layer, bool bottomUp, Color circuitColor) {
			if (this.product == null || this.product.GraphConstruction == null ||
						this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedPlan == null ||
						this.product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}
			
			additionalTransformation = additionalTransformation * Transformation4D.Translation(position.X, position.Y, 0);
			additionalTransformation = additionalTransformation * Transformation4D.RotateZ(this.product.GraphConstruction.Rotation * Math.PI / 180.0);

			double height = KlimaFlaechenModul.GetModuleHeight(type) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double width = KlimaFlaechenModul.GetModuleWidth(type) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

			Point2D topLeft2D = additionalTransformation.TransformTo2D(new Point2D(0, 0));
			Point2D topRight2D = additionalTransformation.TransformTo2D(new Point2D(width, 0));
			Point2D bottomRight2D = additionalTransformation.TransformTo2D(new Point2D(width, height));
			Point2D bottomLeft2D = additionalTransformation.TransformTo2D(new Point2D(0, height));
			Point2D middle2D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height / 2));
			Point2D textStart = additionalTransformation.TransformTo2D(new Point2D(0.01 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.06 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

			Point2D directionTop12D;
			Point2D directionTop22D;
			Point2D directionTop32D;
			Point2D directionBottom12D;
			Point2D directionBottom22D;
			Point2D directionBottom32D;

			if (bottomUp) {
				directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0));

				directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
			} else {
				directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
				directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
				directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

				directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
			}

			EntityColor c = EntityColor.CreateFromRgb(circuitColor.ToArgb());
			
			Point2D[] polygon = null;
			if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
				polygon = new Point2D[] { topLeft2D, topRight2D, bottomRight2D, bottomLeft2D };
				if (type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 || type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) {
					DxfLine line = new DxfLine(c, bottomLeft2D, middle2D);
					line.Layer = layer;
					model.Entities.Add(line);
					line = new DxfLine(c, middle2D, topLeft2D);
					line.Layer = layer;
					model.Entities.Add(line);
				} else {
					DxfLine line = new DxfLine(c, topRight2D, bottomLeft2D);
					line.Layer = layer;
					model.Entities.Add(line);
				}
			} else if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
				polygon = new Point2D[] { topLeft2D, topRight2D, bottomRight2D, bottomLeft2D };
				if (type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 || type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) {
					DxfLine line = new DxfLine(c, bottomRight2D, middle2D);
					line.Layer = layer;
					model.Entities.Add(line);
					line = new DxfLine(c, middle2D, topRight2D);
					line.Layer = layer;
					model.Entities.Add(line);
				} else {
					DxfLine line = new DxfLine(c, topLeft2D, bottomRight2D);
					line.Layer = layer;
					model.Entities.Add(line);
				}
			} else {
				polygon = new Point2D[] { topLeft2D, topRight2D, bottomRight2D, bottomLeft2D };
			}

			DxfPolyline2D polyLine = new DxfPolyline2D(c, polygon);
			polyLine.Closed = true;
			polyLine.Layer = layer;
			model.Entities.Add(polyLine);

			if (orientation != null) {
				polygon = new Point2D[] { directionTop12D, directionTop22D, directionTop32D };
				polyLine = new DxfPolyline2D(c, polygon);
				polyLine.Closed = true;
				polyLine.Layer = layer;
				model.Entities.Add(polyLine);
				polygon = new Point2D[] { directionBottom12D, directionBottom22D, directionBottom32D };
				polyLine = new DxfPolyline2D(c, polygon);
				polyLine.Closed = true;
				polyLine.Layer = layer;
				model.Entities.Add(polyLine);
			}

			string moduleString = "";
			switch (type) {
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_100_30:
					moduleString = EuroplanRes.KlimaFlaechenModul_100_30_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40:
					moduleString = EuroplanRes.KlimaFlaechenModul_100_40_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_120_30:
					moduleString = EuroplanRes.KlimaFlaechenModul_120_30_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60B_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60C:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60C_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60D:
					moduleString = EuroplanRes.KlimaFlaechenModul_60_60D_Short;
					break;
				case KlimaFlaechenModul.ModulTypeEnum.MODUL_80_30:
					moduleString = EuroplanRes.KlimaFlaechenModul_80_30_Short;
					break;
			}

			if (!model.TextStyles.Contains("HarreitherStyle")) {
				DxfTextStyle textStyle = new DxfTextStyle("HarreitherStyle", "Arial.ttf");
				model.TextStyles.Add(textStyle);
			}
			DxfText text = new DxfText(moduleString, (Point3D)textStart, 0.05f * this.product.AssociatedRoom.AssociatedPlan.Measure.Value);
			//text3.Thickness = 0.4d;
			text.Style = model.TextStyles["HarreitherStyle"];
			text.Layer = layer;
			text.Color = c;
			text.Rotation = this.product.GraphConstruction.Rotation / 180.0 * Math.PI;
			model.Entities.Add(text);		
		}

		private Dictionary<KlimaFlaechenModul, Polygon2D> GetModuleInputs(ModulDeckeSubArea subArea) {
			Dictionary<KlimaFlaechenModul, Polygon2D> inputAreas = new Dictionary<KlimaFlaechenModul, Polygon2D>();
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double height;
			double width;
			Point2D input12D, input22D, input32D, input42D;

			Matrix3D laneRotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
			Matrix3D moduleRotation = Transformation3D.Rotate(this.product.GraphConstruction.Rotation * Math.PI / 180.0);
			List<ModulDeckeSubArea> subAreas = new List<ModulDeckeSubArea>();
			if (subArea != null) {
				subAreas.Add(subArea);
			} else {
				foreach (ModulDeckeCircuit c in this.product.PlannedCircuits) {
					subAreas.AddRange(c.SubAreas);
				}
			}
			//foreach (ModulDeckeCircuit c in this.product.PlannedCircuits) {
				//foreach (ModulDeckeSubArea sa in c.SubAreas) {
				foreach (ModulDeckeSubArea sa in subAreas) {
					foreach (KlimaFlaechenList row in sa.Rows) {
						foreach (KlimaFlaechenModul modul in row.List) {
							double x = laneRotation.Transform(this.product.GraphConstruction.PossibleLanes[modul.GraphLane].BorderLeft.Origin).X;
							double y = modul.GraphPositionInLan;
							Matrix3D transformation = moduleRotation * Transformation3D.Translation(x, y);
							height = KlimaFlaechenModul.GetModuleHeight(modul.ModulType) * measure;
							width = KlimaFlaechenModul.GetModuleWidth(modul.ModulType) * measure;

							if (this.product.AssociatedRoom.AssociatedPlan.InvertYAxis == modul.GraphBottomUp) {
								if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
									/*output12D = topLeft2D;
									output22D = transformation.Transform(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));*/
									input12D = transformation.Transform(new Point2D(width, height));
									input22D = transformation.Transform(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									input32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									input42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
									inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
								} else if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
									/*output12D = topRight2D;
									output22D = transformation.Transform(new Point2D(width, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));*/
									input12D = transformation.Transform(new Point2D(0, height));
									input22D = transformation.Transform(new Point2D(0, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									input32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									input42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
									inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
								}
							} else {
								if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
									input12D = transformation.Transform(new Point2D(0, 0));
									input22D = transformation.Transform(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									input32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									input42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
									inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
									/*output12D = bottomRight2D;
									output22D = transformation.Transform(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));*/
								} else if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
									input12D = transformation.Transform(new Point2D(width, 0));
									input22D = transformation.Transform(new Point2D(width, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									input32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									input42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
									inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
									/*output12D = bottomLeft2D;
									output22D = transformation.Transform(new Point2D(0, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));*/
								}
							}

							/*Polygon2D modulArea = new Polygon2D();
							modulArea.Add(transformation.Transform(new Point2D(0, 0)));
							modulArea.Add(transformation.Transform(new Point2D(width, 0)));
							modulArea.Add(transformation.Transform(new Point2D(width, height)));
							modulArea.Add(transformation.Transform(new Point2D(0, height)));
							moduleAreas.Add(modul, modulArea);*/
						}
					}
				}
			//}

			return inputAreas;
		}

		private Dictionary<KlimaFlaechenModul, Polygon2D> GetModuleOutputs(ModulDeckeSubArea subArea) {
			Dictionary<KlimaFlaechenModul, Polygon2D> outputAreas = new Dictionary<KlimaFlaechenModul, Polygon2D>();
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double height, width;
			Point2D output12D, output22D, output32D, output42D;

			Matrix3D laneRotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
			Matrix3D moduleRotation = Transformation3D.Rotate(this.product.GraphConstruction.Rotation * Math.PI / 180.0);
			List<ModulDeckeSubArea> subAreas = new List<ModulDeckeSubArea>();
			if (subArea != null) {
				subAreas.Add(subArea);
			} else {
				foreach (ModulDeckeCircuit c in this.product.PlannedCircuits) {
					subAreas.AddRange(c.SubAreas);
				}
			}
			//foreach (ModulDeckeCircuit c in this.product.PlannedCircuits) {
				//foreach (ModulDeckeSubArea sa in c.SubAreas) {
				foreach (ModulDeckeSubArea sa in subAreas) {
					foreach (KlimaFlaechenList row in sa.Rows) {
						foreach (KlimaFlaechenModul modul in row.List) {
							double x = laneRotation.Transform(this.product.GraphConstruction.PossibleLanes[modul.GraphLane].BorderLeft.Origin).X;
							double y = modul.GraphPositionInLan;
							Matrix3D transformation = moduleRotation * Transformation3D.Translation(x, y);
							height = KlimaFlaechenModul.GetModuleHeight(modul.ModulType) * measure;
							width = KlimaFlaechenModul.GetModuleWidth(modul.ModulType) * measure;

							if (this.product.AssociatedRoom.AssociatedPlan.InvertYAxis == modul.GraphBottomUp) {
								if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
									output12D = transformation.Transform(new Point2D(0, 0));
									output22D = transformation.Transform(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
									outputAreas.Add(modul, new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }));
								} else if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
									output12D = transformation.Transform(new Point2D(width, 0));
									output22D = transformation.Transform(new Point2D(width, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
									outputAreas.Add(modul, new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }));
								}
							} else {
								if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
									output12D = transformation.Transform(new Point2D(width, height));
									output22D = transformation.Transform(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
									outputAreas.Add(modul, new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }));
								} else if (modul.Orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
									output12D = transformation.Transform(new Point2D(0, height));
									output22D = transformation.Transform(new Point2D(0, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
									output42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
									outputAreas.Add(modul, new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }));
								}
							}

							/*Polygon2D modulArea = new Polygon2D();
							modulArea.Add(transformation.Transform(new Point2D(0, 0)));
							modulArea.Add(transformation.Transform(new Point2D(width, 0)));
							modulArea.Add(transformation.Transform(new Point2D(width, height)));
							modulArea.Add(transformation.Transform(new Point2D(0, height)));
							moduleAreas.Add(modul, modulArea);*/
						}
					}
				}
			//}

			return outputAreas;
		}

		//private List<Point2D> GetNextConnectionVerticesInclConnectionPoints(Point2D mousePoint, out KlimaFlaechenModul endModule, out KlimaFlaechenSubAreaVerbindung endVerbindung, out GraphicalConnectionAnbindungsPunkt endAnbindung, out bool rowConnection) {
		private List<Point2D> GetNextConnectionVerticesInclConnectionPoints(Point2D mousePoint, out Nullable<PossibleConnectionPoint> endConnectionPoint) {
			List<Point2D> nextConnectionPoints = new List<Point2D>();
			endConnectionPoint = null;
			/*endModule = null;
			endVerbindung = null;
			endAnbindung = null;
			rowConnection = false;*/

			foreach (PossibleConnectionPoint conn in this.possibleConnectionPoints) {
				if (conn.ConnectionArea.IsInside(mousePoint)) {
					endConnectionPoint = conn;
				}
			}

			if (endConnectionPoint != null) {
				//Point2D connectionPoint = this.newConnectionStartAtOutput != rowConnection ? endModule.GetInputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product) : endModule.GetOutputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);
				if (this.newConnectionVertices.Count > 1) {
					Point2D p1 = this.newConnectionVertices[this.newConnectionVertices.Count - 2];
					Point2D p2 = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
					Line2D line1 = new Line2D(p1, p1 - p2);
					Line2D line2 = new Line2D(endConnectionPoint.Value.ConnectionPoint, new Vector2D(line1.Direction.Y, -line1.Direction.X));
					Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
					if (intersection.HasValue) {
						nextConnectionPoints.Add(intersection.Value);
					}
				}
				nextConnectionPoints.Add(endConnectionPoint.Value.ConnectionPoint);
			} else {
				bool horizontal;
				nextConnectionPoints.Add(this.GetNextConnectionVertex(mousePoint, out horizontal));
			}

			/*int tmp;
			//if (this.newConnectionStartAtOutput) {
			if (this.newConnectionStartData.Value.StartAtOutput) {
				foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> input in this.GetModuleInputs(null)) {
					if (input.Value.IsInside(mousePoint)) {
						if (this.product.GetCircuitForModul(input.Key, out tmp) == this.product.GetCircuitForModul(this.newConnectionStart, out tmp) && this.AllowInput(input.Key)) {
							endModule = input.Key;
						}
						break;
					}
				}
				if (endModule == null) {
					//ModulDeckeSubArea subArea = this.newConnectionCircuit.GetSubareaForModul(this.newConnectionStart, out tmp);
					ModulDeckeSubArea subArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartModul, out tmp);
					foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> output in this.GetModuleOutputs(subArea)) {
						if (output.Value.IsInside(mousePoint)) {
							//if (!this.newConnectionCircuit.IsRuecklaufConnected(subArea.GetRowForModul(output.Key, out tmp))) {
							if (!this.newConnectionStartData.Value.CircuitOfStart.IsRuecklaufConnected(subArea.GetRowForModul(output.Key, out tmp))) {
								endModule = output.Key;
								rowConnection = true;
							}
							break;
						}
					}
				}
				if (endModule == null) {
					foreach (GraphicalConnectionAnbindungsPunkt output in this.possibleAnbindungspunkte) {
						if (output.Area.IsInside(mousePoint)) {
							endAnbindung = output;
							break;
						}
					}
				}
			} else {
				foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> output in this.GetModuleOutputs(null)) {
					if (output.Value.IsInside(mousePoint)) {
						if (this.product.GetCircuitForModul(output.Key, out tmp) == this.product.GetCircuitForModul(this.newConnectionStart, out tmp) && this.AllowOutput(output.Key)) {
							endModule = output.Key;
						}
						break;
					}
				}
				if (endModule == null) {
					ModulDeckeSubArea subArea = this.newConnectionCircuit.GetSubareaForModul(this.newConnectionStart, out tmp);
					foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> input in this.GetModuleInputs(subArea)) {
						if (input.Value.IsInside(mousePoint)) {
							if (!this.newConnectionCircuit.IsVorlaufConnected(subArea.GetRowForModul(input.Key, out tmp))) {
								endModule = input.Key;
								rowConnection = true;
							}
							break;
						}
					}
				}
				if (endModule == null) {
					foreach (GraphicalConnectionAnbindungsPunkt input in this.possibleAnbindungspunkte) {
						if (input.Area.IsInside(mousePoint)) {
							endAnbindung = input;
							break;
						}
					}
				}
			}
			//ModulDeckeCircuit circuit = this.product.GetCircuitForModul(this.newConnectionStart, out tmp);
			ModulDeckeCircuit circuit = this.newConnectionCircuit;
			if (!rowConnection && circuit.GetAllLinkedModules(this.newConnectionStart).Contains(endModule)) {
				endModule = null;
			}
			if (endModule != null) {
				Point2D connectionPoint = this.newConnectionStartAtOutput != rowConnection ? endModule.GetInputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product) : endModule.GetOutputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);
				if (this.newConnectionVertices.Count > 1) {
					Point2D p1 = this.newConnectionVertices[this.newConnectionVertices.Count - 2];
					Point2D p2 = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
					Line2D line1 = new Line2D(p1, p1 - p2);
					Line2D line2 = new Line2D(connectionPoint, new Vector2D(line1.Direction.Y, -line1.Direction.X));
					Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
					if (intersection.HasValue) {
						nextConnectionPoints.Add(intersection.Value);
					}
				}
				nextConnectionPoints.Add(connectionPoint);
			} else if (endAnbindung != null) {
				Point2D connectionPoint = endAnbindung.Point;
				if (this.newConnectionVertices.Count > 1) {
					Point2D p1 = this.newConnectionVertices[this.newConnectionVertices.Count - 2];
					Point2D p2 = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
					Line2D line1 = new Line2D(p1, p1 - p2);
					Line2D line2 = new Line2D(connectionPoint, new Vector2D(line1.Direction.Y, -line1.Direction.X));
					Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
					if (intersection.HasValue) {
						nextConnectionPoints.Add(intersection.Value);
					}
				}
				nextConnectionPoints.Add(connectionPoint);
			} else {
				Nullable<Point2D> saLinkPoint = null;
				double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				if (circuit.Links != null) {
					double dist;
					double bestDist = double.MaxValue;
					Nullable<Point2D> point;
					foreach (KlimaFlaechenSubAreaVerbindung saLink in circuit.Links) {
						point = saLink.GetClosestPoint(mousePoint, out dist);
						dist = dist / measure;
						if (dist <= 0.05 && dist < bestDist) {
							// check if this connection is allowed
							bool ok = false;
							if (this.newConnectionStartAtOutput) {
								List<ModulDeckeSubArea> subAreas = saLink.GetStartSubAreas();
								//int tmp;
								ModulDeckeSubArea startSubArea = this.newConnectionCircuit.GetSubareaForModul(this.newConnectionStart, out tmp);
								KlimaFlaechenList startRow = startSubArea.GetRowForModul(this.newConnectionStart, out tmp);
								if (subAreas != null && subAreas.Count > 0 && subAreas[0] == startSubArea && !saLink.GetStartRows().Contains(startRow)) {
									ok = true;
								}
								//if (saLink.EndRows != null && saLink.EndRows.Count > 0 && saLink.EndRows[0].
							} else {
								List<ModulDeckeSubArea> subAreas = saLink.GetEndSubAreas();
								//int tmp;
								ModulDeckeSubArea startSubArea = this.newConnectionCircuit.GetSubareaForModul(this.newConnectionStart, out tmp);
								KlimaFlaechenList startRow = startSubArea.GetRowForModul(this.newConnectionStart, out tmp);
								if (subAreas != null && subAreas.Count > 0 && subAreas[0] == startSubArea && !saLink.GetStartRows().Contains(startRow)) {
									ok = true;
								}
							}
							if (ok) {
								bestDist = dist;
								saLinkPoint = point;
								endVerbindung = saLink;
							}
						}
					}
				}
				if (saLinkPoint == null) {
					bool horizontal;
					nextConnectionPoints.Add(this.GetNextConnectionVertex(mousePoint, out horizontal));
				} else {
					bool addRealEndPoint = true;
					if (this.newConnectionVertices.Count > 1) {
						Point2D p1 = this.newConnectionVertices[this.newConnectionVertices.Count - 2];
						Point2D p2 = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
						Line2D line1 = new Line2D(p1, p1 - p2);
						Line2D line2 = new Line2D(saLinkPoint.Value, new Vector2D(line1.Direction.Y, -line1.Direction.X));
						Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
						if (intersection.HasValue) {
							nextConnectionPoints.Add(intersection.Value);
							double dist;
							endVerbindung.GetClosestPoint(intersection.Value, out dist);
							dist = dist * measure;
							if (dist < 0.0000001) {
								addRealEndPoint = false;
							}
						}
					}
					if (addRealEndPoint) {
						nextConnectionPoints.Add(saLinkPoint.Value);
					}
				}
			}*/
			return nextConnectionPoints;
		}

		private Point2D GetNextConnectionVertex(Point2D mousePoint, out bool horizontal) {
			if (this.newConnectionVertices == null || this.newConnectionVertices.Count == 0) {
				horizontal = true;
				return mousePoint;
			}

			double rotation = this.product.GraphConstruction.Rotation;

			Point2D lastVertex = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
			Matrix3D transformation = Matrix3D.Identity;
			transformation = transformation * Transformation3D.Rotate(-rotation * Math.PI / 180.0);
			transformation = transformation * Transformation3D.Translation(-lastVertex.X, -lastVertex.Y);

			Point2D transformedMousePoint = transformation.Transform(mousePoint);
			if (Math.Abs(transformedMousePoint.X) < Math.Abs(transformedMousePoint.Y)) {
				transformedMousePoint.X = 0;
				horizontal = false;
			} else {
				transformedMousePoint.Y = 0;
				horizontal = true;
			}

			return transformation.GetInverse().Transform(transformedMousePoint);
		}

		public bool MoveRow(KlimaFlaechenList row) {
			SelectMoveTargetForm form = new SelectMoveTargetForm(this.product, true);
			bool move = form.ShowDialog() == DialogResult.OK;
			if (move) {
				this.product.MoveRow(row, form.SelectedSubarea, form.SelectedCircuit, this.GetNewCircuitColor());
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
			form.Dispose();
			return move;
		}

		public bool MoveSubarea(ModulDeckeSubArea subArea) {
			SelectMoveTargetForm form = new SelectMoveTargetForm(this.product, false);
			bool move = form.ShowDialog() == DialogResult.OK;
			if (move) {
				this.product.MoveSubarea(subArea, form.SelectedCircuit, this.GetNewCircuitColor());
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
			form.Dispose();
			return move;
		}

        public bool ShowPlanBackground {
            get { return Europlan.Common.Product.ShowPlanInBackground; }
        }
    }
}
