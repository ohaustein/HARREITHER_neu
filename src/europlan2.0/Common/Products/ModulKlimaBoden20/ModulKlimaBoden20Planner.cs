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
using WW.Cad.Model.Tables;
using WW.Cad.Model.Entities;

namespace Europlan.Common {
	public partial class ModulKlimaBoden20Planner : Component, IProductPlanner {

        private struct NewConnectionStartData
        {
            private KlimaFlaechenModul startModul;
            private KlimaFlaechenList rowOfStartModul;
            private ModulKlimaBoden20SubArea subAreaOfStartModul;
            private bool startAtOutput;

            private KlimaFlaechenSubAreaVerbindung startVerbindung;
            private ModulKlimaBoden20SubArea verbindungStartSubArea;
            private ModulKlimaBoden20SubArea verbindungEndSubArea;
            private List<KlimaFlaechenList> openRowsInStartSubArea;
            private List<KlimaFlaechenList> openRowsInEndSubArea;

            private ModulKlimaBoden20Circuit circuitOfStart;
            private int distributorIndex;
            private List<int> ignoreDistributorIndices;

            private GraphicalConnectionAnbindungsPunkt startAnbindung;

            public NewConnectionStartData(KlimaFlaechenModul startModul, bool startAtOutput, ModulKlimaBoden20Product product)
            {
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
                foreach (ModulKlimaBoden20Circuit c in product.PlannedCircuits)
                {
                    foreach (ModulKlimaBoden20SubArea sa in c.SubAreas)
                    {
                        foreach (KlimaFlaechenList row in sa.Rows)
                        {
                            if (row.List.Contains(startModul))
                            {
                                rowOfStartModul = row;
                                subAreaOfStartModul = sa;
                                circuitOfStart = c;
                                break;
                            }
                        }
                        if (rowOfStartModul != null)
                        {
                            break;
                        }
                    }
                    if (rowOfStartModul != null)
                    {
                        break;
                    }
                }

                this.distributorIndex = this.circuitOfStart.GetDistributorConnectionIndex(true, true);
                this.ignoreDistributorIndices = new List<int>();
                foreach (ModulKlimaBoden20Circuit c in product.PlannedCircuits)
                {
                    int index;
                    if (startAtOutput)
                    {
                        index = c.GetDistributorConnectionIndex(this.distributorIndex < 0, true);
                    }
                    else
                    {
                        index = c.GetDistributorConnectionIndex(true, this.distributorIndex < 0);
                    }
                    if (index >= 0)
                    {
                        this.ignoreDistributorIndices.Add(index);
                    }
                }
            }

            public NewConnectionStartData(GraphicalConnectionAnbindungsPunkt anbindung, bool vorlauf, ModulKlimaBoden20Product product)
            {
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

                foreach (ModulKlimaBoden20Circuit c in product.PlannedCircuits)
                {
                    if (c.GetDistributorConnectionIndex(!vorlauf, vorlauf) == this.distributorIndex)
                    {
                        circuitOfStart = c;
                    }
                }
            }

            public NewConnectionStartData(KlimaFlaechenSubAreaVerbindung startVerbindung, ModulKlimaBoden20Product product)
            {
                this.startModul = null;
                this.rowOfStartModul = null;
                this.subAreaOfStartModul = null;
                this.startAnbindung = null;
                this.startAtOutput = false;

                this.startVerbindung = startVerbindung;

                KlimaFlaechenModul verbindungStartModul = null;
                KlimaFlaechenModul verbindungEndModul = null;
                if (startVerbindung.Start != null && startVerbindung.Start.Count > 0)
                {
                    verbindungStartModul = startVerbindung.Start[0];
                }
                if (startVerbindung.End != null && startVerbindung.End.Count > 0)
                {
                    verbindungEndModul = startVerbindung.End[0];
                }

                this.circuitOfStart = null;
                foreach (ModulKlimaBoden20Circuit c in product.PlannedCircuits)
                {
                    if (c.Links.Contains(startVerbindung))
                    {
                        this.circuitOfStart = c;
                        break;
                    }
                }

                this.verbindungStartSubArea = null;
                this.verbindungEndSubArea = null;
                this.openRowsInStartSubArea = new List<KlimaFlaechenList>();
                this.openRowsInEndSubArea = new List<KlimaFlaechenList>();
                foreach (ModulKlimaBoden20SubArea sa in this.circuitOfStart.SubAreas)
                {
                    foreach (KlimaFlaechenList row in sa.Rows)
                    {
                        if (verbindungStartModul != null && row.List.Contains(verbindungStartModul))
                        {
                            this.verbindungStartSubArea = sa;
                        }
                        if (verbindungEndModul != null && row.List.Contains(verbindungEndModul))
                        {
                            this.verbindungEndSubArea = sa;
                        }
                        if ((verbindungStartModul == null || this.verbindungStartSubArea != null) && (verbindungEndModul == null || this.verbindungEndSubArea != null))
                        {
                            break;
                        }
                    }
                    if ((verbindungStartModul == null || this.verbindungStartSubArea != null) && (verbindungEndModul == null || this.verbindungEndSubArea != null))
                    {
                        break;
                    }
                }

                if (this.verbindungStartSubArea != null)
                {
                    this.openRowsInStartSubArea.AddRange(this.verbindungStartSubArea.Rows);
                    foreach (KlimaFlaechenList r in startVerbindung.GetStartRows())
                    {
                        if (r != null && this.openRowsInStartSubArea.Contains(r))
                        {
                            this.openRowsInStartSubArea.Remove(r);
                        }
                    }
                }
                if (this.verbindungEndSubArea != null)
                {
                    this.openRowsInEndSubArea.AddRange(this.verbindungEndSubArea.Rows);
                    foreach (KlimaFlaechenList r in startVerbindung.GetEndRows())
                    {
                        if (r != null && this.openRowsInEndSubArea.Contains(r))
                        {
                            this.openRowsInEndSubArea.Remove(r);
                        }
                    }
                }

                this.distributorIndex = this.circuitOfStart.GetDistributorConnectionIndex(true, true);
                this.ignoreDistributorIndices = new List<int>();
                foreach (ModulKlimaBoden20Circuit c in product.PlannedCircuits)
                {
                    int index;
                    if (startAtOutput)
                    {
                        index = c.GetDistributorConnectionIndex(this.distributorIndex < 0, true);
                    }
                    else
                    {
                        index = c.GetDistributorConnectionIndex(true, this.distributorIndex < 0);
                    }
                    if (index >= 0)
                    {
                        this.ignoreDistributorIndices.Add(index);
                    }
                }
            }

            public bool StartsAtModul
            {
                get { return this.startModul != null; }
            }

            public bool StartsAtVerbindung
            {
                get { return this.startVerbindung != null; }
            }

            public bool StartsAtAnbindung
            {
                get { return this.startAnbindung != null; }
            }

            public KlimaFlaechenModul StartModul
            {
                get { return this.startModul; }
            }

            public KlimaFlaechenList RowOfStartModul
            {
                get { return this.rowOfStartModul; }
            }

            public ModulKlimaBoden20SubArea SubAreaOfStartModul
            {
                get { return this.subAreaOfStartModul; }
            }

            public bool StartAtOutput
            {
                get { return this.startAtOutput; }
            }



            public KlimaFlaechenSubAreaVerbindung StartVerbindung
            {
                get { return this.startVerbindung; }
            }

            public ModulKlimaBoden20SubArea VerbindungStartSubArea
            {
                get { return this.verbindungStartSubArea; }
            }

            public ModulKlimaBoden20SubArea VerbindungEndSubArea
            {
                get { return this.verbindungEndSubArea; }
            }

            public List<KlimaFlaechenList> OpenRowsInStartSubArea
            {
                get { return this.openRowsInStartSubArea; }
            }

            public List<KlimaFlaechenList> OpenRowsInEndSubArea
            {
                get { return this.openRowsInEndSubArea; }
            }


            public ModulKlimaBoden20Circuit CircuitOfStart
            {
                get { return this.circuitOfStart; }
            }

            public int DistributorIndex
            {
                get { return this.distributorIndex; }
            }

            public List<int> IgnoreDistributorIndices
            {
                get { return this.ignoreDistributorIndices; }
            }

            public GraphicalConnectionAnbindungsPunkt StartAnbindung
            {
                get { return this.startAnbindung; }
            }
        }




		public enum VerlegungsAbstand {
			VA_DICHT = 0,
			VA_MODULIEREND = 1,
			VA_MODULIEREND_X2 = 2
		}

		private Color[] circuitColors = new Color[] {
			Color.FromArgb(192, 0, 0),
			Color.FromArgb(0, 128, 0),
			Color.FromArgb(0, 0, 192),
			Color.FromArgb(255, 128, 0),
			Color.FromArgb(128, 128, 0),
			Color.FromArgb(0, 128, 255)
		};

		public enum KlimaBodenMode {
			KDM_NONE,
			KDM_CONSTRUCTION,
			KDM_LAYOUT_ADD_AREA,
			KDM_LAYOUT_ADD_AREA_FINISH,
			KDM_LAYOUT_ADD_AREA_PICK_REFERENCE,
			KDM_PICK_MODULE,
			KDM_ADD_CONNECTIONS,
			KDM_DEL_CONNECTION
		}

		public class UpdateNewCountArgs : EventArgs {
			public int count;

			public UpdateNewCountArgs(int count) {
				this.count = count;
			}
		}

		public event EventHandler<UpdateNewCountArgs> UpdateNewCount;

		public delegate void AddModuleDelegate(double x, double y, int rowNr, double rotation, out bool added, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, bool bottomUp, out KlimaFlaechenModul addedModul, out KlimaFlaechenList rowOfAddedModul, KlimaFlaechenModul lastAddedModul, KlimaFlaechenList rowOfLastAddedModul, out ModulKlimaBoden20Circuit circuitOfModul, bool fits);

		public ModulKlimaBoden20Planner() {
			InitializeComponent();
		}

		public ModulKlimaBoden20Planner(IContainer container) {
			container.Add(this);


			InitializeComponent();
		}

		private ModulKlimaBoden20Product product;
		private KlimaBodenMode mode = KlimaBodenMode.KDM_NONE;
		private Cursor customCursor = null;
		private bool highlightRoomCoordinates = true;
		private bool drawExpansionGaps = true;
		private double newModulesRotation = 0.0;
		private VerlegungsAbstand newModulesXDicht = VerlegungsAbstand.VA_MODULIEREND;
		private VerlegungsAbstand newModulesYDicht = VerlegungsAbstand.VA_MODULIEREND;
		private double newModulesOffsetX = 0.0;
		private double newModulesOffsetY = 0.0;
		private KlimaFlaechenModul.ModulOrientationEnum newModulesStartingOrientation = KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
		private static double modulierendDistance = 0.1;

		public event EventHandler<EventArgs> ModeChanged;
        public event EventHandler<ListNeedsUpdateEventArgs> ListsNeedUpdate;

        private NewConnectionStartData? newConnectionStartData = null;

        public class ListNeedsUpdateEventArgs : EventArgs
        {
            public bool selectLastCircuit = false;

            public ListNeedsUpdateEventArgs()
            {
            }

            public ListNeedsUpdateEventArgs(bool selectLastCircuit)
            {
                this.selectLastCircuit = selectLastCircuit;
            }
        }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModulKlimaBoden20Product Product {
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
			}
		}

        private Cursor cursorBeforePickReference = Cursors.Cross;

		public KlimaBodenMode Mode {
			get { return this.mode; }
			set {
                if (this.mode != KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE && value == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
                    this.cursorBeforePickReference = this.customCursor;
                    this.customCursor = Cursors.Arrow;
                    if (this.ConnectedPlanPanel != null) {
                        this.ConnectedPlanPanel.PlanCursor = this.customCursor;
                    }
                } else if (this.mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE && value != KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
                    this.customCursor = this.cursorBeforePickReference;
                    if (this.ConnectedPlanPanel != null) {
                        this.ConnectedPlanPanel.PlanCursor = this.customCursor;
                    }
                }
				this.mode = value;
				if (this.mode != KlimaBodenMode.KDM_PICK_MODULE && this.HighlightModules != null) {
					this.HighlightModules = null;
				}
				if (this.mode != KlimaBodenMode.KDM_LAYOUT_ADD_AREA && this.mode != KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH && this.mode != KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
					this.layoutAddArea = null;
					if (this.connectedPlanPanel != null) {
						this.connectedPlanPanel.InvalidateGraphics();
					}
				}
				if (this.mode != KlimaBodenMode.KDM_ADD_CONNECTIONS) {
                    this.possibleConnectionPoints = new List<KlimaFlaechenModul.PossibleConnectionPoint>();
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
			if (this.mode == KlimaBodenMode.KDM_PICK_MODULE) {
				// nothing to do here
			} else if (this.mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH || this.mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
				if (e.KeyCode == Keys.Escape) {
					this.layoutAddArea = null;
					this.Mode = KlimaBodenMode.KDM_LAYOUT_ADD_AREA;
					this.connectedPlanPanel.InvalidateGraphics();
				}
			} else if (this.mode == KlimaBodenMode.KDM_ADD_CONNECTIONS) {
				if (e.KeyCode == Keys.Escape) {
                    this.newConnectionStartData = null;
					this.newConnectionVertices = null;
				} else if (e.KeyCode == Keys.Back) {
					if (this.newConnectionVertices == null || this.newConnectionVertices.Count < 2) {
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
					foreach (ModulKlimaBoden20Circuit c in this.product.PlannedCircuits) {
						bool empty = true;
						foreach (KlimaFlaechenModul m in c.GetAllModules()) {
							if (!modules.Contains(m)) {
								empty = false;
								break;
							}
						}
						if (empty) {
							ask = true;
							break;
						}
					}
					if (ask) {
						if (MessageBox.Show(EuroplanRes.ModulKlimaBoden20Planner_HeizkreisLoeschenText, EuroplanRes.ModulKlimaBoden20Planner_HeizkreisLoeschenTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
							return true;
						}
						this.product.Connections.Clear();
					}
				}
                List<KlimaFlaechenList> emptyRows = new List<KlimaFlaechenList>();
                List<ModulKlimaBoden20SubArea> emptySubAreas = new List<ModulKlimaBoden20SubArea>();
                List<Circuit> emptyCircuits = new List<Circuit>();
                foreach (Circuit c in this.product.PlannedCircuits)
                {
                    ModulKlimaBoden20Circuit dc = c as ModulKlimaBoden20Circuit;
                    foreach (ModulKlimaBoden20SubArea sa in dc.SubAreas)
                    {
                        foreach (KlimaFlaechenList kfl in sa.Rows)
                        {
                            foreach (KlimaFlaechenModul kfm in modules)
                            {
                                if (kfl.List.Contains(kfm))
                                {
                                    KlimaFlaechenModulVerbindung link = kfm.GetInputLink(c, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis);
                                    if (link != null)
                                    {
                                        kfl.Links.Remove(link);
                                    }
                                    link = kfm.GetOutputLink(c, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis);
                                    if (link != null)
                                    {
                                        kfl.Links.Remove(link);
                                    }
                                    KlimaFlaechenSubAreaVerbindung saLink = kfm.GetSubareaInputLink(c, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis);
                                    if (saLink != null)
                                    {
                                        dc.Links.Remove(saLink);
                                    }
                                    saLink = kfm.GetSubareaOutputLink(c, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis);
                                    if (saLink != null)
                                    {
                                        dc.Links.Remove(saLink);
                                    }
                                    kfl.List.Remove(kfm);
                                }
                            }
                            if (kfl.List.Count == 0)
                            {
                                emptyRows.Add(kfl);
                            }
                        }
                        foreach (KlimaFlaechenList emptyRow in emptyRows)
                        {
                            sa.Rows.Remove(emptyRow);
                        }
                        emptyRows.Clear();
                        if (sa.Rows.Count == 0)
                        {
                            emptySubAreas.Add(sa);
                        }
                    }
                    foreach (ModulKlimaBoden20SubArea emptySubArea in emptySubAreas)
                    {
                        dc.SubAreas.Remove(emptySubArea);
                    }
                    emptySubAreas.Clear();
                    if (dc.SubAreas.Count == 0)
                    {
                        emptyCircuits.Add(dc);
                    }
                }
                foreach (Circuit emptyCircuit in emptyCircuits)
                {
                    this.product.PlannedCircuits.Remove(emptyCircuit);
                }
                if (this.projectChanged != null)
                {
                    this.projectChanged(this);
                }
                this.ModuleSelected(this, new ModuleSelectedEventArgs());
                this.ConnectedPlanPanel.InvalidateGraphics();
                if (this.ListsNeedUpdate != null)
                {
                    this.ListsNeedUpdate(this, new ListNeedsUpdateEventArgs(false));
                }
                return true;
            }
            return false;
		}

		public void PaintAfterPlanPannel(System.Windows.Forms.PaintEventArgs e, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl) {
			this.PaintAfterPlanPannel(e.Graphics, additionalTransformation, mousePositionInPlan, mousePositionInControl, false);
		}

		public void PaintAfterPlanPannel(Graphics g, Matrix4D additionalTransformation, Point2D mousePositionInPlan, Point mousePositionInControl, bool export) {
			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.RoomCoordinates != null) {

				if (!export) {
					// paint anbindeleitungen and distributors
					this.connectionDrawer.Paint(g, additionalTransformation);
				}

                bool cadPlan = false;
                if (this.Product.AssociatedRoom.AssociatedFloor != null && this.Product.AssociatedRoom.AssociatedFloor.AssociatedPlan is CadPlan) {
                    cadPlan = true;
                }

				// generate clip for product
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
						this.product.GraphConstruction.Paint(g, this.Mode);
					}
				}

				if (this.layoutAddArea != null) {
					// calculate and draw rectangle that is currently selected for adding modules
					PointF[] drawArea = new PointF[this.layoutAddArea.Count];
					for (int i = 0; i < this.layoutAddArea.Count; i++) {
						Point2D tmp = additionalTransformation.TransformTo2D(this.layoutAddArea[i]);
						drawArea[i] = new PointF((float)tmp.X, (float)tmp.Y);
					}
					g.DrawPolygon(Pens.Red, drawArea);

					// draw modules that are currently being added
					int count = 0;
                    this.AddModulesForLayoutArea(delegate(double x, double y, int rowNr, double rotation, out bool added, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, bool bottomUp, out KlimaFlaechenModul addedModul, out KlimaFlaechenList rowOfAddedModul, KlimaFlaechenModul lastAddedModul, KlimaFlaechenList rowOfLastAddedModul, out ModulKlimaBoden20Circuit circuitOfModul, bool fits)
                    {
						added = this.TryDrawModule(g, additionalTransformation, x, y, rotation, orientation, bottomUp, fits ? Color.Green : Color.FromArgb(63, Color.Red), cadPlan);
						if (fits) {
							count++;
						}
						addedModul = null;
                        rowOfAddedModul = null;
						circuitOfModul = null;
					}, true);
					if (this.UpdateNewCount != null) {
						this.UpdateNewCount(this, new UpdateNewCountArgs(count));
					}
				}

				// paint unused areas of room
				if (this.product.AssociatedRoom.RoomUnusedAreaCoordinates != null) {
					List<PointF> unusedPoints = new List<PointF>();
					foreach (List<Point2D> unusedArea in this.product.AssociatedRoom.RoomUnusedAreaCoordinates) {
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

				// paint expansion gaps
				if (drawExpansionGaps) {
					foreach (Segment2D expansionGap in this.Product.AssociatedRoom.AssociatedFloor.ExpansionGaps) {
						Point2D start = additionalTransformation.TransformTo2D(expansionGap.Start);
						Point2D end = additionalTransformation.TransformTo2D(expansionGap.End);
						g.DrawLine(Pens.Blue, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
					}
				}

				// draw rectangle for module selection
				if (this.dragStartedInPlan.HasValue && this.dragEndedInPlan.HasValue) {
					Matrix transform = g.Transform;
					g.Transform = new Matrix();
					g.DrawPolygon(new Pen(Color.Red), new Point[] { this.dragStartedInControl.Value, new Point(this.dragStartedInControl.Value.X, this.dragEndedInControl.Value.Y), this.dragEndedInControl.Value, new Point(this.dragEndedInControl.Value.X, this.dragStartedInControl.Value.Y) });
					g.Transform = transform;
				}

				// paint modules
				List<KlimaFlaechenModul> selectedModules = this.GetAllSelectedModules();
				foreach (ModulKlimaBoden20Circuit circuit in this.product.PlannedCircuits) {

                    foreach (KlimaFlaechenModul modul in circuit.GetAllModules())
                    {
                        this.DrawModule(modul.ModulType, modul.Orientation, new Point2D(modul.GraphPosX, modul.GraphPosY), modul.GraphRotation, additionalTransformation, g, modul.GraphBottomUp, selectedModules.Contains(modul), circuit.CircuitColor, modul == this.hoveredModul && this.hoverInput, modul == this.hoveredModul && this.hoverOutput, cadPlan);
                    }

					if (circuit.Links != null) {
						foreach (KlimaFlaechenSubAreaVerbindung link in circuit.Links)
                        {
							link.Draw(g, additionalTransformation, circuit.CircuitColor, this.product.AssociatedRoom.AssociatedPlan.Measure.Value);
						}

                        foreach (ModulKlimaBoden20SubArea subarea in circuit.SubAreas)
                        {
                            foreach (KlimaFlaechenList row in subarea.Rows) 
                            {
                                foreach (KlimaFlaechenModulVerbindung link in row.Links)
                                {
                                    link.Draw(g, additionalTransformation, circuit.CircuitColor, this.product.AssociatedRoom.AssociatedPlan.Measure.Value);
                                }
                            }
                        }
					}
				}

				if (this.mode == KlimaBodenMode.KDM_ADD_CONNECTIONS) {
                    if (this.possibleConnectionPoints != null)
                    {
                        g.ResetClip();
                        Color neutralColor = this.product.AssociatedRoom.AssociatedPlan is CadPlan ? Color.White : Color.Black;
                        Brush bi = new SolidBrush(Color.FromArgb(127, Color.Red));
                        Brush bo = new SolidBrush(Color.FromArgb(127, Color.Blue));
                        Brush bn = new SolidBrush(Color.FromArgb(127, neutralColor));
                        foreach (KlimaFlaechenModul.PossibleConnectionPoint point in this.possibleConnectionPoints)
                        {
                            PointF[] points = new PointF[point.ConnectionArea.Count];
                            for (int i = 0; i < point.ConnectionArea.Count; i++)
                            {
                                Point2D tmp = additionalTransformation.TransformTo2D(point.ConnectionArea[i]);
                                points[i] = new PointF((float)tmp.X, (float)tmp.Y);
                            }
                            g.FillPolygon(point.Vorlauf ? bi : (point.Ruecklauf ? bo : bn), points);
                            g.DrawPolygon(point.Vorlauf ? Pens.Red : (point.Ruecklauf ? Pens.Blue : new Pen(neutralColor)), points);
                        }
                    }
                    Pen p = new Pen(Color.Green, (float)(0.021 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * additionalTransformation.M00));
                    if (this.newConnectionVertices != null && this.newConnectionVertices.Count > 0)
                    {
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
                        for (int i = 1; i < countSum; i++)
                        {
                            vertex = i < countNew ? this.newConnectionVertices[i] : this.nextConnectionPoints[i - countNew];
                            newVertex2D = additionalTransformation.TransformTo2D(vertex);
                            newVertex = new PointF((float)newVertex2D.X, (float)newVertex2D.Y);
                            if (i == 2)
                            {
                                p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                            }
                            if (i == countSum - 1)
                            {
                                p.EndCap = System.Drawing.Drawing2D.LineCap.Flat;
                            }
                            g.DrawLine(p, oldVertex, newVertex);
                            oldVertex = newVertex;
                        }
                    }
				}
			}
		}

		private List<Point2D> newConnectionVertices = null;
        private List<KlimaFlaechenModul.PossibleConnectionPoint> possibleConnectionPoints = new List<KlimaFlaechenModul.PossibleConnectionPoint>();

		public bool PlannerClick(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			bool redraw = false;
			if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_PICK_REFERENCE) {
				Dictionary<KlimaFlaechenModul, Polygon2D> moduleAreas = this.GetModuleAreas();
				KlimaFlaechenModul pickedModul = null;
				foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> kvp in moduleAreas) {
					if (kvp.Value.IsInside(planPoint)) {
						pickedModul = kvp.Key;
						break;
					}
				}

				if (pickedModul != null) {
					Matrix3D rotation = Transformation3D.Rotate(-this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);
					Matrix3D invRotation = rotation.GetInverse();

                    double top = double.MaxValue;
					double bottom = double.MinValue;
					double left = double.MaxValue;
					double right = double.MinValue;
					foreach (Point2D point in this.layoutAddArea) {
						Point2D rotatedPoint = invRotation.Transform(point);
						if (rotatedPoint.X < left) {
							left = rotatedPoint.X;
						}
						if (rotatedPoint.X > right) {
							right = rotatedPoint.X;
						}
						if (rotatedPoint.Y < top) {
							top = rotatedPoint.Y;
						}
						if (rotatedPoint.Y > bottom) {
							bottom = rotatedPoint.Y;
						}
					}
					Point2D referencePoint = invRotation.Transform(new Point2D(pickedModul.GraphPosX, pickedModul.GraphPosY));

                    Point2D rotatedTopLeft = invRotation.Transform(this.layoutAddArea[0]);
                    Point2D rotatedBottomLeft = invRotation.Transform(this.layoutAddArea[1]);
                    Point2D rotatedBottomRight = invRotation.Transform(this.layoutAddArea[2]);
                    Point2D rotatedTopRight = invRotation.Transform(this.layoutAddArea[3]);

                    bool rightToLeft = rotatedTopLeft.X > rotatedTopRight.X;
                    bool bottomUp = rotatedTopLeft.Y > rotatedBottomLeft.Y;

					double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

					double stepX = width;
					stepX += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * (int)this.newModulesXDicht;
					double stepY = height;
					stepY += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * (int)this.newModulesYDicht;

					double refLeft = referencePoint.X;
					while (refLeft - stepX > left) {
						refLeft -= stepX;
					}
					while (refLeft < left) {
						refLeft += stepX;
					}

					double refTop = referencePoint.Y;
					while (refTop - stepY > top) {
						refTop -= stepY;
					}
					while (refTop < top) {
						refTop += stepY;
					}

                    this.NewModulesOffsetX = refLeft - (rightToLeft ? right + modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * (int)this.newModulesXDicht : left);
                    this.NewModulesOffsetY = refTop - (bottomUp ? bottom + modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * (int)this.newModulesYDicht : top);

					this.Mode = KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH;
					if (this.connectedPlanPanel != null) {
						this.connectedPlanPanel.InvalidateGraphics();
					}
					if (this.ModeChanged != null) {
						this.ModeChanged(this, EventArgs.Empty);
					}
				}
			} else if (this.mode == KlimaBodenMode.KDM_ADD_CONNECTIONS) {
                if (this.newConnectionStartData == null)
                {
                    foreach (KlimaFlaechenModul.PossibleConnectionPoint connection in this.possibleConnectionPoints)
                    {
                        if (connection.ConnectionArea.IsInside(planPoint))
                        {
                            switch (connection.ConnectionType)
                            {
                                case KlimaFlaechenModul.PossibleConnectionPointType.CONNECTION_MODULE:
                                    this.newConnectionStartData = new NewConnectionStartData(connection.Modul, connection.Ruecklauf, this.product);
                                    this.newConnectionVertices = new List<Point2D>();
                                    this.newConnectionVertices.Add(connection.ConnectionPoint);
                                    break;

                                case KlimaFlaechenModul.PossibleConnectionPointType.CONNECTION_SUBAREA:
                                    this.newConnectionStartData = new NewConnectionStartData(connection.Verbindung, this.product);
                                    this.newConnectionVertices = new List<Point2D>();
                                    this.newConnectionVertices.Add(connection.ConnectionPoint);
                                    break;

                                case KlimaFlaechenModul.PossibleConnectionPointType.CONNECTION_PRODUCT:
                                    this.newConnectionStartData = new NewConnectionStartData(connection.ProductConnection, connection.Vorlauf, this.product);
                                    this.newConnectionVertices = new List<Point2D>();
                                    this.newConnectionVertices.Add(connection.ConnectionPoint);
                                    break;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    this.newConnectionVertices.AddRange(this.nextConnectionPoints);
                    foreach (KlimaFlaechenModul.PossibleConnectionPoint connection in this.possibleConnectionPoints)
                    {
                        if (connection.ConnectionArea.IsInside(planPoint))
                        {
                            int tmp;
                            switch (connection.ConnectionType)
                            {
                                case KlimaFlaechenModul.PossibleConnectionPointType.CONNECTION_MODULE:
                                    if (this.newConnectionStartData.Value.StartsAtModul)
                                    {
                                        ModulKlimaBoden20SubArea sa = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(connection.Modul, out tmp);
                                        KlimaFlaechenList row = sa.GetRowForModul(connection.Modul, out tmp);
                                        if (row != this.newConnectionStartData.Value.RowOfStartModul)
                                        {
                                            if (this.newConnectionStartData.Value.CircuitOfStart.Links == null)
                                            {
                                                this.newConnectionStartData.Value.CircuitOfStart.Links = new List<KlimaFlaechenSubAreaVerbindung>();
                                            }
                                            if (this.newConnectionStartData.Value.StartAtOutput)
                                            {
                                                if (connection.Ruecklauf)
                                                {
                                                    this.newConnectionStartData.Value.CircuitOfStart.Links.Add(new KlimaFlaechenSubAreaVerbindung(new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul, connection.Modul }, new KlimaFlaechenModul[] { }, new List<Point2D>[] { this.newConnectionVertices }, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product)));
                                                }
                                                else
                                                {
                                                    this.newConnectionStartData.Value.CircuitOfStart.Links.Add(new KlimaFlaechenSubAreaVerbindung(new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul }, new KlimaFlaechenModul[] { connection.Modul }, new List<Point2D>[] { this.newConnectionVertices }, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product)));
                                                }
                                            }
                                            else
                                            {
                                                this.newConnectionVertices.Reverse();
                                                if (connection.Vorlauf)
                                                {
                                                    this.newConnectionStartData.Value.CircuitOfStart.Links.Add(new KlimaFlaechenSubAreaVerbindung(new KlimaFlaechenModul[] { }, new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul, connection.Modul }, new List<Point2D>[] { this.newConnectionVertices }, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product)));
                                                }
                                                else
                                                {
                                                    this.newConnectionStartData.Value.CircuitOfStart.Links.Add(new KlimaFlaechenSubAreaVerbindung(new KlimaFlaechenModul[] { connection.Modul }, new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul }, new List<Point2D>[] { this.newConnectionVertices }, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product)));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (this.newConnectionStartData.Value.RowOfStartModul.Links == null)
                                            {
                                                this.newConnectionStartData.Value.RowOfStartModul.Links = new List<KlimaFlaechenModulVerbindung>();
                                            }
                                            if (this.newConnectionStartData.Value.StartAtOutput)
                                            {
                                                this.newConnectionStartData.Value.RowOfStartModul.Links.Add(new KlimaFlaechenModulVerbindung(this.newConnectionStartData.Value.StartModul, connection.Modul, this.newConnectionVertices, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product)));
                                            }
                                            else
                                            {
                                                this.newConnectionVertices.Reverse();
                                                this.newConnectionStartData.Value.RowOfStartModul.Links.Add(new KlimaFlaechenModulVerbindung(connection.Modul, this.newConnectionStartData.Value.StartModul, this.newConnectionVertices, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product)));
                                            }
                                        }
                                        this.newConnectionStartData = null;
                                        this.newConnectionVertices = null;
                                    }
                                    else if (this.newConnectionStartData.Value.StartsAtVerbindung)
                                    {
                                        if (connection.Vorlauf)
                                        {
                                            this.newConnectionStartData.Value.StartVerbindung.End.Add(connection.Modul);
                                            this.newConnectionVertices.Reverse();
                                            this.newConnectionStartData.Value.StartVerbindung.Vertices.Add(this.newConnectionVertices);
                                            this.newConnectionStartData = null;
                                            this.newConnectionVertices = null;
                                        }
                                        else if (connection.Ruecklauf)
                                        {
                                            this.newConnectionStartData.Value.StartVerbindung.Start.Add(connection.Modul);
                                            this.newConnectionStartData.Value.StartVerbindung.Vertices.Add(this.newConnectionVertices);
                                            this.newConnectionStartData = null;
                                            this.newConnectionVertices = null;
                                        }
                                    }
                                    else if (this.newConnectionStartData.Value.StartsAtAnbindung)
                                    {
                                        if (this.newConnectionStartData.Value.StartAnbindung.NewProductConnection != null)
                                        {
                                            if (this.product.Connections == null)
                                            {
                                                this.product.Connections = new List<GraphicalProductConnection>();
                                            }
                                            this.product.Connections.Add(this.newConnectionStartData.Value.StartAnbindung.NewProductConnection);
                                        }
                                        ModulKlimaBoden20Circuit c = this.product.GetCircuitForModul(connection.Modul, out tmp);
                                        if (c.Links == null)
                                        {
                                            c.Links = new List<KlimaFlaechenSubAreaVerbindung>();
                                        }

                                        if (!this.newConnectionStartData.Value.StartAtOutput)
                                        {
                                            c.Links.Add(new KlimaFlaechenSubAreaVerbindung(new List<KlimaFlaechenModul>(new KlimaFlaechenModul[] { connection.Modul }), null, new List<Point2D>[] { this.newConnectionVertices }, c, Project.Instance.GetPlannedProduct(this.product), this.newConnectionStartData.Value.StartAnbindung.Index));
                                        }
                                        else
                                        {
                                            c.Links.Add(new KlimaFlaechenSubAreaVerbindung(null, new List<KlimaFlaechenModul>(new KlimaFlaechenModul[] { connection.Modul }), new List<Point2D>[] { this.newConnectionVertices }, c, Project.Instance.GetPlannedProduct(this.product), this.newConnectionStartData.Value.StartAnbindung.Index));
                                        }
                                        this.newConnectionStartData = null;
                                        this.newConnectionVertices = null;
                                    }
                                    break;

                                case KlimaFlaechenModul.PossibleConnectionPointType.CONNECTION_SUBAREA:
                                    if (this.newConnectionStartData.Value.StartsAtModul)
                                    {
                                        if (this.newConnectionStartData.Value.StartAtOutput)
                                        {
                                            connection.Verbindung.Start.Add(this.newConnectionStartData.Value.StartModul);
                                            connection.Verbindung.Vertices.Add(this.newConnectionVertices);
                                        }
                                        else
                                        {
                                            connection.Verbindung.End.Add(this.newConnectionStartData.Value.StartModul);
                                            this.newConnectionVertices.Reverse();
                                            connection.Verbindung.Vertices.Add(this.newConnectionVertices);
                                        }
                                        this.newConnectionStartData = null;
                                        this.newConnectionVertices = null;
                                    }
                                    else if (this.newConnectionStartData.Value.StartsAtVerbindung)
                                    {
                                        if (connection.Verbindung.Start != null)
                                        {
                                            this.newConnectionStartData.Value.StartVerbindung.Start.AddRange(connection.Verbindung.Start);
                                        }
                                        if (connection.Verbindung.End != null)
                                        {
                                            this.newConnectionStartData.Value.StartVerbindung.End.AddRange(connection.Verbindung.End);
                                        }
                                        this.newConnectionStartData.Value.StartVerbindung.Vertices.AddRange(connection.Verbindung.Vertices);
                                        this.newConnectionStartData.Value.StartVerbindung.Vertices.Add(this.newConnectionVertices);
                                        this.newConnectionStartData.Value.CircuitOfStart.Links.Remove(connection.Verbindung);
                                        this.newConnectionStartData = null;
                                        this.newConnectionVertices = null;
                                    }
                                    else if (this.newConnectionStartData.Value.StartsAtAnbindung)
                                    {
                                        if (this.newConnectionStartData.Value.StartAnbindung.NewProductConnection != null)
                                        {
                                            if (this.product.Connections == null)
                                            {
                                                this.product.Connections = new List<GraphicalProductConnection>();
                                            }
                                            this.product.Connections.Add(this.newConnectionStartData.Value.StartAnbindung.NewProductConnection);
                                        }

                                        if (connection.Verbindung.Start == null || connection.Verbindung.Start.Count == 0)
                                        {
                                            connection.Verbindung.DistributorIndex = this.newConnectionStartData.Value.StartAnbindung.Index;
                                            connection.Verbindung.Vertices.Add(this.newConnectionVertices);
                                            this.newConnectionStartData = null;
                                            this.newConnectionVertices = null;
                                        }
                                        else if (connection.Verbindung.End == null || connection.Verbindung.End.Count == 0)
                                        {
                                            connection.Verbindung.DistributorIndex = this.newConnectionStartData.Value.StartAnbindung.Index;
                                            connection.Verbindung.Vertices.Add(this.newConnectionVertices);
                                            this.newConnectionStartData = null;
                                            this.newConnectionVertices = null;
                                        }
                                    }
                                    break;

                                case KlimaFlaechenModul.PossibleConnectionPointType.CONNECTION_PRODUCT:
                                    if (connection.ProductConnection.NewProductConnection != null)
                                    {
                                        if (this.product.Connections == null)
                                        {
                                            this.product.Connections = new List<GraphicalProductConnection>();
                                        }
                                        this.product.Connections.Add(connection.ProductConnection.NewProductConnection);
                                    }
                                    if (this.newConnectionStartData.Value.CircuitOfStart.Links == null)
                                    {
                                        this.newConnectionStartData.Value.CircuitOfStart.Links = new List<KlimaFlaechenSubAreaVerbindung>();
                                    }
                                    if (this.newConnectionStartData.Value.StartsAtModul)
                                    {
                                        if (this.newConnectionStartData.Value.StartAtOutput)
                                        {
                                            this.newConnectionStartData.Value.CircuitOfStart.Links.Add(new KlimaFlaechenSubAreaVerbindung(new List<KlimaFlaechenModul>(new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul }), null, new List<Point2D>[] { this.newConnectionVertices }, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product), connection.ProductConnection.Index));
                                        }
                                        else
                                        {
                                            this.newConnectionStartData.Value.CircuitOfStart.Links.Add(new KlimaFlaechenSubAreaVerbindung(null, new List<KlimaFlaechenModul>(new KlimaFlaechenModul[] { this.newConnectionStartData.Value.StartModul }), new List<Point2D>[] { this.newConnectionVertices }, this.newConnectionStartData.Value.CircuitOfStart, Project.Instance.GetPlannedProduct(this.product), connection.ProductConnection.Index));
                                        }
                                        this.newConnectionStartData = null;
                                        this.newConnectionVertices = null;
                                    }
                                    else if (this.newConnectionStartData.Value.StartsAtVerbindung)
                                    {
                                        if (this.newConnectionStartData.Value.StartVerbindung.Start == null || this.newConnectionStartData.Value.StartVerbindung.Start.Count == 0)
                                        {
                                            this.newConnectionStartData.Value.StartVerbindung.DistributorIndex = connection.ProductConnection.Index;
                                            this.newConnectionStartData.Value.StartVerbindung.Vertices.Add(this.newConnectionVertices);
                                            this.newConnectionStartData = null;
                                            this.newConnectionVertices = null;
                                        }
                                        else if (this.newConnectionStartData.Value.StartVerbindung.End == null || this.newConnectionStartData.Value.StartVerbindung.End.Count == 0)
                                        {
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
			} else if (this.Mode == KlimaBodenMode.KDM_DEL_CONNECTION) {
                double bestDist = double.MaxValue;
                KlimaFlaechenModulVerbindung bestLink = null;
                KlimaFlaechenList bestRow = null;
                KlimaFlaechenSubAreaVerbindung bestSaLink = null;
                ModulKlimaBoden20Circuit bestCircuit = null;
                double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
                foreach (ModulKlimaBoden20Circuit circuit in this.product.PlannedCircuits)
                {
                    if (circuit.Links != null)
                    {
                        foreach (KlimaFlaechenSubAreaVerbindung saLink in circuit.Links)
                        {
                            double dist = saLink.GetDistance(planPoint);
                            if (dist < bestDist && dist <= measure * 0.025)
                            {
                                bestDist = dist;
                                bestSaLink = saLink;
                                bestCircuit = circuit;
                                bestLink = null;
                                bestRow = null;
                            }
                        }
                    }
                    foreach (ModulKlimaBoden20SubArea sa in circuit.SubAreas)
                    {
                        foreach (KlimaFlaechenList row in sa.Rows)
                        {
                            if (row.Links != null)
                            {
                                foreach (KlimaFlaechenModulVerbindung link in row.Links)
                                {
                                    double dist = link.GetDistance(planPoint);
                                    if (dist < bestDist && dist <= measure * 0.025)
                                    {
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
                if (bestLink != null)
                {
                    bestRow.Links.Remove(bestLink);
                    redraw = true;
                }
                if (bestSaLink != null)
                {
                    bestCircuit.Links.Remove(bestSaLink);
                    redraw = true;
                }
                if (bestSaLink != null)
                {
                    bool stillConnected = false;
                    foreach (ModulKlimaBoden20Circuit c in this.product.PlannedCircuits)
                    {
                        if (c.Links != null)
                        {
                            foreach (KlimaFlaechenSubAreaVerbindung link in c.Links)
                            {
                                if (link.StartConnectedToAnbindung || link.EndConnectedToAnbindung)
                                {
                                    stillConnected = true;
                                    break;
                                }
                            }
                        }
                        if (stillConnected)
                        {
                            break;
                        }
                        foreach (ModulKlimaBoden20SubArea sa in c.SubAreas)
                        {
                            foreach (KlimaFlaechenList row in sa.Rows)
                            {
                                if (row.Links != null)
                                {
                                    foreach (KlimaFlaechenModulVerbindung link in row.Links)
                                    {
                                        if (link.StartConnectedToAnbindung || link.EndConnectedToAnbindung)
                                        {
                                            stillConnected = true;
                                            break;
                                        }
                                    }
                                }
                                if (stillConnected)
                                {
                                    break;
                                }
                            }
                            if (stillConnected)
                            {
                                break;
                            }
                        }
                        if (stillConnected)
                        {
                            break;
                        }
                    }
                    if (!stillConnected)
                    {
                        List<GraphicalProductConnection> connectionsToDelete = new List<GraphicalProductConnection>();
                        foreach (GraphicalProductConnection conn in this.product.Connections)
                        {
                            if (conn.Automatic)
                            {
                                connectionsToDelete.Add(conn);
                            }
                        }
                        foreach (GraphicalProductConnection conn in connectionsToDelete)
                        {
                            this.product.Connections.Remove(conn);
                        }
                    }
                }
			}
			return redraw;
		}     

        private Point2D GetNextConnectionVertex(Point2D mousePoint, out bool horizontal)
        {
            if (this.newConnectionVertices == null || this.newConnectionVertices.Count == 0)
            {
                horizontal = true;
                return mousePoint;
            }

            double rotation = this.product.GraphConstruction.Rotation;

            Point2D lastVertex = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
            Matrix3D transformation = Matrix3D.Identity;
            transformation = transformation * Transformation3D.Rotate(-rotation * Math.PI / 180.0);
            transformation = transformation * Transformation3D.Translation(-lastVertex.X, -lastVertex.Y);

            Point2D transformedMousePoint = transformation.Transform(mousePoint);
            if (Math.Abs(transformedMousePoint.X) < Math.Abs(transformedMousePoint.Y))
            {
                transformedMousePoint.X = 0;
                horizontal = false;
            }
            else
            {
                transformedMousePoint.Y = 0;
                horizontal = true;
            }

            return transformation.GetInverse().Transform(transformedMousePoint);
        }


        private List<Point2D> GetNextConnectionVerticesInclConnectionPoints(Point2D mousePoint, out Nullable<KlimaFlaechenModul.PossibleConnectionPoint> endConnectionPoint)
        {
            List<Point2D> nextConnectionPoints = new List<Point2D>();
            endConnectionPoint = null;

            foreach (KlimaFlaechenModul.PossibleConnectionPoint conn in this.possibleConnectionPoints)
            {
                if (conn.ConnectionArea.IsInside(mousePoint))
                {
                    endConnectionPoint = conn;
                }
            }

            if (endConnectionPoint != null)
            {
                if (this.newConnectionVertices.Count > 1)
                {
                    Point2D p1 = this.newConnectionVertices[this.newConnectionVertices.Count - 2];
                    Point2D p2 = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
                    Line2D line1 = new Line2D(p1, p1 - p2);
                    Line2D line2 = new Line2D(endConnectionPoint.Value.ConnectionPoint, new Vector2D(line1.Direction.Y, -line1.Direction.X));
                    Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
                    if (intersection.HasValue)
                    {
                        nextConnectionPoints.Add(intersection.Value);
                    }
                }
                nextConnectionPoints.Add(endConnectionPoint.Value.ConnectionPoint);
            }
            else
            {
                bool horizontal;
                nextConnectionPoints.Add(this.GetNextConnectionVertex(mousePoint, out horizontal));
            }

            return nextConnectionPoints;
        }

        

		private KlimaFlaechenModul hoveredModul = null;
		private bool hoverInput = false;
		private bool hoverOutput = false;
		private List<Point2D> nextConnectionPoints = new List<Point2D>();

        private List<KlimaFlaechenModul.PossibleConnectionPoint> GetPossibleConnectionPoints(Point2D mousePosition)
        {
            List<KlimaFlaechenModul.PossibleConnectionPoint> result = new List<KlimaFlaechenModul.PossibleConnectionPoint>();
            int tmp;
            if (this.newConnectionStartData != null)
            {
                if (this.newConnectionStartData.Value.StartsAtModul)
                {
                    // add connections to distributor
                    List<GraphicalConnectionAnbindungsPunkt> productConnections;
                    if (!this.newConnectionStartData.Value.StartAtOutput)
                    {
                        productConnections = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, true, this.newConnectionStartData.Value.DistributorIndex, this.newConnectionStartData.Value.IgnoreDistributorIndices, mousePosition);
                    }
                    else
                    {
                        productConnections = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, false, this.newConnectionStartData.Value.DistributorIndex, this.newConnectionStartData.Value.IgnoreDistributorIndices, mousePosition);
                    }
                    foreach (GraphicalConnectionAnbindungsPunkt conn in productConnections)
                    {
                        result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(conn, !this.newConnectionStartData.Value.StartAtOutput, this.newConnectionStartData.Value.StartAtOutput));
                    }

                    // add connections to modules
                    Dictionary<KlimaFlaechenModul, Polygon2D> areas = this.GetModuleAreas();
                    double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
                    bool invertYAxis = this.product.AssociatedRoom.AssociatedPlan.InvertYAxis;
                    foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> area in areas)
                    {
                        if (area.Value.IsInside(mousePosition))
                        {
                            if (this.newConnectionStartData.Value.StartModul == area.Key)
                            {
                                break;
                            }
                            ModulKlimaBoden20Circuit c = this.product.GetCircuitForModul(area.Key, out tmp);
                            ModulKlimaBoden20SubArea sa = c.GetSubareaForModul(area.Key, out tmp);
                            KlimaFlaechenList row = sa.GetRowForModul(area.Key, out tmp);
                            if (c == this.newConnectionStartData.Value.CircuitOfStart)
                            {
                                if (sa != this.newConnectionStartData.Value.SubAreaOfStartModul || row == this.newConnectionStartData.Value.RowOfStartModul)
                                {
                                    // allow connections from output to input or from input to output
                                    if (this.newConnectionStartData.Value.StartAtOutput)
                                    {
                                        if (area.Key.IsInputOpen(c, invertYAxis))
                                        {
                                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(area.Key.GetInputConnection(measure, invertYAxis, product), area.Key.GetInputConnectionArea(measure, invertYAxis, this.product), area.Key, true, false));
                                        }
                                    }
                                    else
                                    {
                                        if (area.Key.IsOutputOpen(c, invertYAxis))
                                        {
                                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(area.Key.GetOutputConnection(measure, invertYAxis, product), area.Key.GetOutputConnectionArea(measure, invertYAxis, this.product), area.Key, false, true));
                                        }
                                    }
                                }
                                else
                                {
                                    // allow connections from input to input or from output to output
                                    if (this.newConnectionStartData.Value.StartAtOutput)
                                    {
                                        if (area.Key.IsOutputOpen(c, invertYAxis))
                                        {
                                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(area.Key.GetOutputConnection(measure, invertYAxis, product), area.Key.GetOutputConnectionArea(measure, invertYAxis, this.product), area.Key, false, true));
                                        }
                                    }
                                    else
                                    {
                                        if (area.Key.IsInputOpen(c, invertYAxis))
                                        {
                                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(area.Key.GetInputConnection(measure, invertYAxis, product), area.Key.GetInputConnectionArea(measure, invertYAxis, this.product), area.Key, true, false));
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
                    if (this.newConnectionStartData.Value.CircuitOfStart.Links != null)
                    {
                        double dist;
                        double bestDist = double.MaxValue;
                        Nullable<Point2D> point;
                        foreach (KlimaFlaechenSubAreaVerbindung saLink in this.newConnectionStartData.Value.CircuitOfStart.Links)
                        {
                            point = saLink.GetClosestPoint(mousePosition, out dist);
                            dist = dist / measure;
                            if (dist <= 0.05 && dist < bestDist)
                            {
                                // check if this connection is allowed
                                bool ok = false;
                                if (this.newConnectionStartData.Value.StartAtOutput)
                                {
                                    ModulSubArea subArea = null;
                                    List<ModulSubArea> subAreas = saLink.GetStartSubAreas();
                                    if (subAreas != null && subAreas.Count > 0)
                                    {
                                        subArea = subAreas[0];
                                    }

                                    ModulKlimaBoden20SubArea startSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartModul, out tmp);
                                    KlimaFlaechenList startRow = startSubArea.GetRowForModul(this.newConnectionStartData.Value.StartModul, out tmp);
                                    if ((subArea == null || subArea == startSubArea) && !saLink.GetStartRows().Contains(startRow) && !saLink.StartConnectedToAnbindung)
                                    {
                                        ok = true;
                                    }
                                }
                                else
                                {
                                    ModulSubArea subArea = null;
                                    List<ModulSubArea> subAreas = saLink.GetEndSubAreas();
                                    if (subAreas != null && subAreas.Count > 0)
                                    {
                                        subArea = subAreas[0];
                                    }

                                    ModulKlimaBoden20SubArea startSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartModul, out tmp);
                                    KlimaFlaechenList startRow = startSubArea.GetRowForModul(this.newConnectionStartData.Value.StartModul, out tmp);
                                    if ((subArea == null || subArea == startSubArea) && !saLink.GetStartRows().Contains(startRow) && !saLink.EndConnectedToAnbindung)
                                    {
                                        ok = true;
                                    }
                                }
                                if (ok)
                                {
                                    bestDist = dist;
                                    saLinkPoint = point;
                                    verbindung = saLink;
                                }
                            }
                        }
                    }
                    if (saLinkPoint != null)
                    {
                        bool addRealEndPoint = true;
                        if (this.newConnectionVertices.Count > 1)
                        {
                            Point2D p1 = this.newConnectionVertices[this.newConnectionVertices.Count - 2];
                            Point2D p2 = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
                            Line2D line1 = new Line2D(p1, p1 - p2);
                            Line2D line2 = new Line2D(saLinkPoint.Value, new Vector2D(line1.Direction.Y, -line1.Direction.X));
                            Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
                            if (intersection.HasValue)
                            {
                                double dist;
                                verbindung.GetClosestPoint(intersection.Value, out dist);
                                dist = dist * measure;
                                if (dist < 0.0000001)
                                {
                                    result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
                                    addRealEndPoint = false;
                                }
                            }
                        }
                        else if (this.newConnectionVertices.Count > 0)
                        {
                            bool tmpH;
                            Point2D nextPoint = this.GetNextConnectionVertex(mousePosition, out tmpH);
                            Line2D l = new Line2D(this.newConnectionVertices[0], this.newConnectionVertices[0] - nextPoint);
                            Nullable<Point2D> oldPoint = null;
                            Segment2D seg;
                            foreach (List<Point2D> list in verbindung.Vertices)
                            {
                                oldPoint = null;
                                foreach (Point2D p in list)
                                {
                                    if (oldPoint.HasValue)
                                    {
                                        seg = new Segment2D(oldPoint.Value, p);
                                        Nullable<Point2D> intersection = Line2D.GetIntersection(l, seg);
                                        if (intersection.HasValue)
                                        {
                                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
                                            addRealEndPoint = false;
                                            break;
                                        }
                                    }
                                    oldPoint = p;
                                }
                                if (!addRealEndPoint)
                                {
                                    break;
                                }
                            }
                        }
                        if (addRealEndPoint)
                        {
                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(saLinkPoint.Value, verbindung, measure, false, false));
                        }
                    }
                }
                else if (this.newConnectionStartData.Value.StartsAtVerbindung)
                {
                    // add connections to distributor
                    List<GraphicalConnectionAnbindungsPunkt> productConnections = null;
                    bool getVorlauf = this.newConnectionStartData.Value.StartVerbindung.Start == null || this.newConnectionStartData.Value.StartVerbindung.Start.Count == 0;
                    bool getRuecklauf = this.newConnectionStartData.Value.StartVerbindung.End == null || this.newConnectionStartData.Value.StartVerbindung.End.Count == 0;
                    if (getVorlauf)
                    {
                        productConnections = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, true, this.newConnectionStartData.Value.DistributorIndex, this.newConnectionStartData.Value.IgnoreDistributorIndices, mousePosition);
                    }
                    else if (getRuecklauf)
                    {
                        productConnections = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, false, this.newConnectionStartData.Value.DistributorIndex, this.newConnectionStartData.Value.IgnoreDistributorIndices, mousePosition);
                    }
                    if (productConnections != null)
                    {
                        foreach (GraphicalConnectionAnbindungsPunkt conn in productConnections)
                        {
                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(conn, getVorlauf, getRuecklauf));
                        }
                    }

                    // add connections to modules
                    Dictionary<KlimaFlaechenModul, Polygon2D> areas = this.GetModuleAreas();
                    double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
                    bool invertYAxis = this.product.AssociatedRoom.AssociatedPlan.InvertYAxis;
                    foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> area in areas)
                    {
                        if (area.Value.IsInside(mousePosition))
                        {
                            if (this.newConnectionStartData.Value.StartModul == area.Key)
                            {
                                break;
                            }
                            ModulKlimaBoden20Circuit c = this.product.GetCircuitForModul(area.Key, out tmp);
                            ModulKlimaBoden20SubArea sa = c.GetSubareaForModul(area.Key, out tmp);
                            KlimaFlaechenList row = sa.GetRowForModul(area.Key, out tmp);
                            if (c == this.newConnectionStartData.Value.CircuitOfStart)
                            {
                                ModulKlimaBoden20SubArea verbindungStartSubArea = null;
                                ModulKlimaBoden20SubArea verbindungEndSubArea = null;
                                if (this.newConnectionStartData.Value.StartVerbindung.Start != null && this.newConnectionStartData.Value.StartVerbindung.Start.Count > 0)
                                {
                                    verbindungStartSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartVerbindung.Start[0], out tmp);
                                }
                                if (this.newConnectionStartData.Value.StartVerbindung.End != null && this.newConnectionStartData.Value.StartVerbindung.End.Count > 0)
                                {
                                    verbindungEndSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartVerbindung.End[0], out tmp);
                                }
                                if (sa != null)
                                {
                                    if (sa == verbindungStartSubArea)
                                    {
                                        if (area.Key.IsOutputOpen(c, invertYAxis))
                                        {
                                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(area.Key.GetOutputConnection(measure, invertYAxis, this.product), area.Key.GetOutputConnectionArea(measure, invertYAxis, this.product), area.Key, false, true));
                                        }
                                    }
                                    else if (sa == verbindungEndSubArea)
                                    {
                                        if (area.Key.IsInputOpen(c, invertYAxis))
                                        {
                                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(area.Key.GetInputConnection(measure, invertYAxis, this.product), area.Key.GetInputConnectionArea(measure, invertYAxis, this.product), area.Key, true, false));
                                        }
                                    }
                                    if (verbindungStartSubArea == null && sa != verbindungEndSubArea)
                                    {
                                        if (area.Key.IsOutputOpen(c, invertYAxis))
                                        {
                                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(area.Key.GetOutputConnection(measure, invertYAxis, this.product), area.Key.GetOutputConnectionArea(measure, invertYAxis, this.product), area.Key, false, true));
                                        }
                                    }
                                    else if (verbindungEndSubArea == null && sa != verbindungStartSubArea)
                                    {
                                        if (area.Key.IsInputOpen(c, invertYAxis))
                                        {
                                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(area.Key.GetInputConnection(measure, invertYAxis, this.product), area.Key.GetInputConnectionArea(measure, invertYAxis, this.product), area.Key, true, false));
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
                    if (this.newConnectionStartData.Value.CircuitOfStart.Links != null)
                    {
                        double dist;
                        double bestDist = double.MaxValue;
                        Nullable<Point2D> point;
                        foreach (KlimaFlaechenSubAreaVerbindung saLink in this.newConnectionStartData.Value.CircuitOfStart.Links)
                        {
                            point = saLink.GetClosestPoint(mousePosition, out dist);
                            dist = dist / measure;
                            if (dist <= 0.05 && dist < bestDist && this.newConnectionStartData.Value.StartVerbindung != saLink)
                            {
                                // check if this connection is allowed
                                bool ok = false;
                                ModulKlimaBoden20SubArea verbindungStartSubArea = null;
                                ModulKlimaBoden20SubArea verbindungEndSubArea = null;
                                if (this.newConnectionStartData.Value.StartVerbindung.Start != null && this.newConnectionStartData.Value.StartVerbindung.Start.Count > 0)
                                {
                                    verbindungStartSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartVerbindung.Start[0], out tmp);
                                }
                                if (this.newConnectionStartData.Value.StartVerbindung.End != null && this.newConnectionStartData.Value.StartVerbindung.End.Count > 0)
                                {
                                    verbindungEndSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(this.newConnectionStartData.Value.StartVerbindung.End[0], out tmp);
                                }

                                ModulKlimaBoden20SubArea saLinkStartSubArea = null;
                                ModulKlimaBoden20SubArea saLinkEndSubArea = null;
                                if (saLink.Start != null && saLink.Start.Count > 0)
                                {
                                    saLinkStartSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(saLink.Start[0], out tmp);
                                }
                                if (saLink.End != null && saLink.End.Count > 0)
                                {
                                    verbindungEndSubArea = this.newConnectionStartData.Value.CircuitOfStart.GetSubareaForModul(saLink.End[0], out tmp);
                                }

                                if ((verbindungStartSubArea == null || saLinkStartSubArea == null || verbindungStartSubArea == saLinkStartSubArea) && (verbindungEndSubArea == null || saLinkEndSubArea == null || verbindungEndSubArea == saLinkEndSubArea))
                                {
                                    ok = true;
                                }
                                if (ok)
                                {
                                    bestDist = dist;
                                    saLinkPoint = point;
                                    verbindung = saLink;
                                }
                            }
                        }
                    }
                    if (saLinkPoint != null)
                    {
                        bool addRealEndPoint = true;
                        if (this.newConnectionVertices.Count > 1)
                        {
                            Point2D p1 = this.newConnectionVertices[this.newConnectionVertices.Count - 2];
                            Point2D p2 = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
                            Line2D line1 = new Line2D(p1, p1 - p2);
                            Line2D line2 = new Line2D(saLinkPoint.Value, new Vector2D(line1.Direction.Y, -line1.Direction.X));
                            Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
                            if (intersection.HasValue)
                            {
                                double dist;
                                verbindung.GetClosestPoint(intersection.Value, out dist);
                                dist = dist * measure;
                                if (dist < 0.0000001)
                                {
                                    result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
                                    addRealEndPoint = false;
                                }
                            }
                        }
                        else if (this.newConnectionVertices.Count > 0)
                        {
                            bool tmpH;
                            Point2D nextPoint = this.GetNextConnectionVertex(mousePosition, out tmpH);
                            Line2D l = new Line2D(this.newConnectionVertices[0], this.newConnectionVertices[0] - nextPoint);
                            Nullable<Point2D> oldPoint = null;
                            Segment2D seg;
                            foreach (List<Point2D> list in verbindung.Vertices)
                            {
                                oldPoint = null;
                                foreach (Point2D p in list)
                                {
                                    if (oldPoint.HasValue)
                                    {
                                        seg = new Segment2D(oldPoint.Value, p);
                                        Nullable<Point2D> intersection = Line2D.GetIntersection(l, seg);
                                        if (intersection.HasValue)
                                        {
                                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
                                            addRealEndPoint = false;
                                            break;
                                        }
                                    }
                                    oldPoint = p;
                                }
                                if (!addRealEndPoint)
                                {
                                    break;
                                }
                            }
                        }
                        if (addRealEndPoint)
                        {
                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(saLinkPoint.Value, verbindung, measure, false, false));
                        }
                    }
                }
                else if (this.newConnectionStartData.Value.StartsAtAnbindung)
                {

                    // add connections to modules
                    Dictionary<KlimaFlaechenModul, Polygon2D> areas = this.GetModuleAreas();
                    double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
                    bool invertYAxis = this.product.AssociatedRoom.AssociatedPlan.InvertYAxis;
                    foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> area in areas)
                    {
                        if (area.Value.IsInside(mousePosition))
                        {

                            ModulKlimaBoden20Circuit c = this.product.GetCircuitForModul(area.Key, out tmp);
                            if (this.newConnectionStartData.Value.CircuitOfStart != null && c != this.newConnectionStartData.Value.CircuitOfStart)
                            {
                                break;
                            }
                            if (c.GetDistributorConnectionIndex(this.newConnectionStartData.Value.StartAtOutput, !this.newConnectionStartData.Value.StartAtOutput) >= 0)
                            {
                                break;
                            }
                            int di = c.GetDistributorConnectionIndex(true, true);
                            if (di >= 0 && this.newConnectionStartData.Value.DistributorIndex != c.GetDistributorConnectionIndex(true, true))
                            {
                                break;
                            }

                            if (!this.newConnectionStartData.Value.StartAtOutput && area.Key.IsOutputOpen(c, invertYAxis))
                            {
                                result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(area.Key.GetOutputConnection(measure, invertYAxis, this.product), area.Key.GetOutputConnectionArea(measure, invertYAxis, this.product), area.Key, false, true));
                            }
                            if (this.newConnectionStartData.Value.StartAtOutput && area.Key.IsInputOpen(c, invertYAxis))
                            {
                                result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(area.Key.GetInputConnection(measure, invertYAxis, this.product), area.Key.GetInputConnectionArea(measure, invertYAxis, this.product), area.Key, true, false));
                            }
                            break;
                        }
                    }

                    List<ModulKlimaBoden20Circuit> circuitsToUse = new List<ModulKlimaBoden20Circuit>();
                    if (this.newConnectionStartData.Value.CircuitOfStart != null)
                    {
                        circuitsToUse.Add(this.newConnectionStartData.Value.CircuitOfStart);
                    }
                    else
                    {
                        foreach (ModulKlimaBoden20Circuit c in this.product.PlannedCircuits)
                        {
                            int di = c.GetDistributorConnectionIndex(true, true);
                            if (di < 0 || di == this.newConnectionStartData.Value.DistributorIndex)
                            {
                                circuitsToUse.Add(c);
                            }
                        }
                    }
                    double dist;
                    double bestDist = double.MaxValue;
                    Nullable<Point2D> point, saLinkPoint = null;
                    KlimaFlaechenSubAreaVerbindung verbindung = null;
                    foreach (ModulKlimaBoden20Circuit c in circuitsToUse)
                    {
                        foreach (KlimaFlaechenSubAreaVerbindung saLink in c.Links)
                        {
                            point = saLink.GetClosestPoint(mousePosition, out dist);
                            dist = dist / measure;
                            if (dist <= 0.05 && dist < bestDist)
                            {
                                bestDist = dist;
                                saLinkPoint = point;
                                verbindung = saLink;
                            }
                        }
                    }

                    if (saLinkPoint != null)
                    {
                        bool addRealEndPoint = true;
                        if (this.newConnectionVertices.Count > 1)
                        {
                            Point2D p1 = this.newConnectionVertices[this.newConnectionVertices.Count - 2];
                            Point2D p2 = this.newConnectionVertices[this.newConnectionVertices.Count - 1];
                            Line2D line1 = new Line2D(p1, p1 - p2);
                            Line2D line2 = new Line2D(saLinkPoint.Value, new Vector2D(line1.Direction.Y, -line1.Direction.X));
                            Nullable<Point2D> intersection = Line2D.GetIntersection(line1, line2);
                            if (intersection.HasValue)
                            {
                                verbindung.GetClosestPoint(intersection.Value, out dist);
                                dist = dist * measure;
                                if (dist < 0.0000001)
                                {
                                    result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
                                    addRealEndPoint = false;
                                }
                            }
                        }
                        else if (this.newConnectionVertices.Count > 0)
                        {
                            bool tmpH;
                            Point2D nextPoint = this.GetNextConnectionVertex(mousePosition, out tmpH);
                            Line2D l = new Line2D(this.newConnectionVertices[0], this.newConnectionVertices[0] - nextPoint);
                            Nullable<Point2D> oldPoint = null;
                            Segment2D seg;
                            foreach (List<Point2D> list in verbindung.Vertices)
                            {
                                oldPoint = null;
                                foreach (Point2D p in list)
                                {
                                    if (oldPoint.HasValue)
                                    {
                                        seg = new Segment2D(oldPoint.Value, p);
                                        Nullable<Point2D> intersection = Line2D.GetIntersection(l, seg);
                                        if (intersection.HasValue)
                                        {
                                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(intersection.Value, verbindung, measure, false, false));
                                            addRealEndPoint = false;
                                            break;
                                        }
                                    }
                                    oldPoint = p;
                                }
                                if (!addRealEndPoint)
                                {
                                    break;
                                }
                            }
                        }
                        if (addRealEndPoint)
                        {
                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(saLinkPoint.Value, verbindung, measure, false, false));
                        }
                    }
                }
            }
            else
            {
                // add connections to distributor
                List<int> ignoreVlDistributorIndices = new List<int>();
                List<int> ignoreRlDistributorIndices = new List<int>();
                foreach (ModulKlimaBoden20Circuit c in product.PlannedCircuits)
                {
                    int index;
                    index = c.GetDistributorConnectionIndex(true, false);
                    if (index >= 0)
                    {
                        ignoreVlDistributorIndices.Add(index);
                    }
                    index = c.GetDistributorConnectionIndex(false, true);
                    if (index >= 0)
                    {
                        ignoreRlDistributorIndices.Add(index);
                    }
                }

                List<GraphicalConnectionAnbindungsPunkt> productConnections;
                productConnections = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, true, -1, ignoreVlDistributorIndices, mousePosition);
                foreach (GraphicalConnectionAnbindungsPunkt conn in productConnections)
                {
                    result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(conn, true, false));
                }
                productConnections = this.product.GetAnbindungsPunkte(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, false, -1, ignoreRlDistributorIndices, mousePosition);
                foreach (GraphicalConnectionAnbindungsPunkt conn in productConnections)
                {
                    result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(conn, false, true));
                }

                // add connections to modules
                Dictionary<KlimaFlaechenModul, Polygon2D> areas = this.GetModuleAreas();
                double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
                bool invertYAxis = this.product.AssociatedRoom.AssociatedPlan.InvertYAxis;
                foreach (KeyValuePair<KlimaFlaechenModul, Polygon2D> area in areas)
                {
                    if (area.Value.IsInside(mousePosition))
                    {
                        ModulKlimaBoden20Circuit c = this.product.GetCircuitForModul(area.Key, out tmp);
                        ModulKlimaBoden20SubArea sa = c.GetSubareaForModul(area.Key, out tmp);
                        KlimaFlaechenList row = sa.GetRowForModul(area.Key, out tmp);
                        if (area.Key.IsInputOpen(c, invertYAxis))
                        {
                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(area.Key.GetInputConnection(measure, invertYAxis, product), area.Key.GetInputConnectionArea(measure, invertYAxis, this.product), area.Key, true, false));
                        }
                        if (area.Key.IsOutputOpen(c, invertYAxis))
                        {
                            result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(area.Key.GetOutputConnection(measure, invertYAxis, product), area.Key.GetOutputConnectionArea(measure, invertYAxis, this.product), area.Key, false, true));
                        }
                        break;
                    }
                }

                // add connections to subarea-connections
                Nullable<Point2D> saLinkPoint = null;
                KlimaFlaechenSubAreaVerbindung verbindung = null;
                foreach (ModulKlimaBoden20Circuit c in this.product.PlannedCircuits)
                {
                    double dist;
                    double bestDist = double.MaxValue;
                    Nullable<Point2D> point;
                    if (c.Links != null)
                    {
                        foreach (KlimaFlaechenSubAreaVerbindung saLink in c.Links)
                        {
                            point = saLink.GetClosestPoint(mousePosition, out dist);
                            dist = dist / measure;
                            if (dist <= 0.05 && dist < bestDist)
                            {
                                // check if this connection is allowed
                                bestDist = dist;
                                saLinkPoint = point;
                                verbindung = saLink;
                            }
                        }
                    }
                }
                if (saLinkPoint != null)
                {
                    result.Add(new KlimaFlaechenModul.PossibleConnectionPoint(saLinkPoint.Value, verbindung, measure, false, false));
                }
            }


            return result;
        }

		public bool PlannerMouseMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
            bool redraw = false;
			if (this.Mode == KlimaBodenMode.KDM_CONSTRUCTION) {
				if (this.product.GraphConstruction != null) {
					if (this.product.GraphConstruction.HitTest(planPoint, pointInControl)) {
						this.customCursor = this.product.GraphConstruction.PickCursor;
						this.ConnectedPlanPanel.PlanCursor = this.product.GraphConstruction.PickCursor;
					} else {
						this.customCursor = Cursors.Default;
						this.ConnectedPlanPanel.PlanCursor = Cursors.Default;
					}
				}
			} else if (this.Mode == KlimaBodenMode.KDM_ADD_CONNECTIONS) {

                redraw = true;

                this.possibleConnectionPoints = this.GetPossibleConnectionPoints(planPoint);

                if (this.newConnectionStartData != null)
                {
                    Nullable<KlimaFlaechenModul.PossibleConnectionPoint> tmpConnectionPoint;
                    this.nextConnectionPoints = this.GetNextConnectionVerticesInclConnectionPoints(planPoint, out tmpConnectionPoint);
                }
                else
                {
                    this.nextConnectionPoints = new List<Point2D>();
                }
                if (this.ConnectedPlanPanel != null)
                {
                    this.ConnectedPlanPanel.InvalidateGraphics();
                }
			}
			return redraw;
		}

		private Point2D layoutAddAreaStart;
		private PointF layoutAddAreaStartScreen;
		private Polygon2D layoutAddArea = null;
		private bool layoutAddAreaBottomUp = false;

		private Nullable<Point> dragStartedInControl;
		private Nullable<Point2D> dragStartedInPlan;
		private Nullable<Point> dragEndedInControl;
		private Nullable<Point2D> dragEndedInPlan;

		private Point2D lastPlanPoint;
		private Point lastPointInControl;

		public bool PlannerDragStart(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == KlimaBodenMode.KDM_CONSTRUCTION) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					this.product.GraphConstruction.StartDrag(planPoint, pointInControl);
				}
			} else if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA) {
				this.newModulesOffsetX = 0;
				this.newModulesOffsetY = 0;
				this.layoutAddAreaStart = planPoint;
				this.layoutAddAreaStartScreen = pointInControl;
			} else if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH) {
				this.lastPlanPoint = planPoint;
				this.lastPointInControl = pointInControl;
			} else if (this.Mode == KlimaBodenMode.KDM_PICK_MODULE) {
				if (!this.ShiftPressed) {
					this.dragStartedInControl = pointInControl;
					this.dragStartedInPlan = planPoint;
				} else {
					this.dragStartedInControl = pointInControl;
					this.dragStartedInPlan = planPoint;
				}
			}
			return false;
		}

		public bool PlannerDragMove(WW.Math.Point2D planPoint, System.Drawing.Point pointInControl, MouseButtons button) {
			if (this.Mode == KlimaBodenMode.KDM_CONSTRUCTION) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					this.product.GraphConstruction.MoveDrag(planPoint, pointInControl);
					if (this.projectChanged != null) {
						this.projectChanged(this);
					}
					return true;
				}
			} else if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA) {
				if (button == MouseButtons.Left && this.product.GraphConstruction != null) {
					Matrix3D matrix = Transformation3D.Rotate(this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);
					Point2D rotatedP1 = matrix.Transform(layoutAddAreaStart);
					Point2D rotatedP3 = matrix.Transform(planPoint);
					this.layoutAddAreaBottomUp = (rotatedP1.Y > rotatedP3.Y);
					Point2D rotatedP2 = new Point2D(rotatedP1.X, rotatedP3.Y);
					Point2D rotatedP4 = new Point2D(rotatedP3.X, rotatedP1.Y);
					matrix = matrix.GetInverse();
					layoutAddArea = new Polygon2D();
					layoutAddArea.Add(layoutAddAreaStart);
					layoutAddArea.Add(matrix.Transform(rotatedP2));
					layoutAddArea.Add(planPoint);
					layoutAddArea.Add(matrix.Transform(rotatedP4));
					return true;
				}
			} else if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH && button == MouseButtons.Left) {
				Vector2D vector = planPoint - this.lastPlanPoint;
				Matrix3D rotate = Transformation3D.Rotate(this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);
				vector = rotate.Transform(vector);
				this.NewModulesOffsetX += vector.X;
				this.NewModulesOffsetY += vector.Y;
				this.lastPlanPoint = planPoint;
				this.lastPointInControl = pointInControl;
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			} else if (this.Mode == KlimaBodenMode.KDM_PICK_MODULE && button == MouseButtons.Left) {
				this.dragEndedInControl = pointInControl;
				this.dragEndedInPlan = planPoint;
				this.connectedPlanPanel.InvalidateGraphics();
			}
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
			if (this.Mode == KlimaBodenMode.KDM_LAYOUT_ADD_AREA) {
				this.Mode = KlimaBodenMode.KDM_LAYOUT_ADD_AREA_FINISH;
				if (this.ModeChanged != null) {
					this.ModeChanged(this, EventArgs.Empty);
				}
			} else if (this.mode == KlimaBodenMode.KDM_PICK_MODULE) {
				if (this.dragStartedInPlan.HasValue) {
					dragEndedInPlan = planPoint;

					Matrix3D rotation = Transformation3D.Rotate(this.product.AssociatedRoom.AssociatedPlan.Rotation * Math.PI / 180.0);
					Matrix3D invRotation = rotation.GetInverse();

					Point2D p1 = rotation.Transform(dragStartedInPlan.Value);
					Point2D p3 = rotation.Transform(dragEndedInPlan.Value);
					Point2D p2 = new Point2D(p1.X, p3.Y);
					Point2D p4 = new Point2D(p3.X, p1.Y);

					Polygon2D selection = new Polygon2D(new Point2D[] { dragStartedInPlan.Value, invRotation.Transform(p2), dragEndedInPlan.Value, invRotation.Transform(p4) });

					List<KlimaFlaechenModul> selectedModules;
					if (this.ShiftPressed && this.HighlightModules != null && this.HighlightModules.Count > 0) {
						selectedModules = new List<KlimaFlaechenModul>(this.HighlightModules);
					} else {
						selectedModules = new List<KlimaFlaechenModul>();
					}

					foreach (ModulKlimaBoden20Circuit c in this.product.PlannedCircuits) {
						foreach (KlimaFlaechenModul modul in c.GetAllModules()) {
							Polygon2D modulArea = GetModuleArea(modul);
							if (AllPointsInside(selection, modulArea)) {
								selectedModules.Add(modul);
							}
							if (AllPointsInside(modulArea, selection)) {
								selectedModules.Add(modul);
							}
						}
					}

					if (selectedModules.Count > 0) {
						this.HighlightModules = selectedModules;
					} else {
						this.HighlightModules = null;
					}

					this.dragStartedInControl = null;
					this.dragStartedInPlan = null;
					this.dragEndedInControl = null;
					this.dragEndedInPlan = null;
					if (this.ConnectedPlanPanel != null) {
						this.ConnectedPlanPanel.InvalidateGraphics();
					}
					if (this.ModuleSelected != null) {
						this.ModuleSelected(this, new ModuleSelectedEventArgs());
					}
					return true;
				}
			}
			return false;
		}

		internal bool ShiftPressed {
			get { return (Control.ModifierKeys & (Keys.Shift | Keys.ShiftKey | Keys.LShiftKey | Keys.RShiftKey)) != Keys.None; }
		}

		public Cursor CustomCursor {
			get { return this.customCursor; }
		}

		public bool PlannerKeyPress(Keys key) {
			return KeyDown(key, this.GetAllSelectedModules());
		}

		#endregion

        private event ProjectChangedHandler projectChanged;
        public event ProjectChangedHandler ProjectChanged {
            add { this.projectChanged += value; }
            remove { this.projectChanged -= value; }
        }

        public class ModuleSelectedEventArgs : EventArgs
        {
            public KlimaFlaechenModul modul;
            public List<KlimaFlaechenModul> modules;

            public ModuleSelectedEventArgs()
            {
                this.modul = null;
                this.modules = null;
            }

            public ModuleSelectedEventArgs(KlimaFlaechenModul modul)
            {
                this.modul = modul;
                this.modules = null;
            }

            public ModuleSelectedEventArgs(List<KlimaFlaechenModul> modules)
            {
                this.modules = modules;
                this.modul = null;
            }
        }

        public event EventHandler<ModuleSelectedEventArgs> ModuleSelected;

		[XmlIgnore]
		public double NewModulesRotation {
			get { return newModulesRotation; }
			set { newModulesRotation = value; }
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double NewModulesRotationInclPlanRotation {
			get {
				return -this.newModulesRotation + this.product.AssociatedRoom.AssociatedPlan.Rotation;
			}
		}

		private double NewModulesStepX {
			get {
				double stepX;
				if (this.product.GraphConstruction is ModulKlimaBoden20ConstructionStaffeln) {
					stepX = (this.product.GraphConstruction as ModulKlimaBoden20ConstructionStaffeln).StaffelnAchsabstand * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				} else {
					stepX = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
					stepX += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * (int)this.newModulesXDicht;
				}
				return stepX;
			}
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double NewModulesOffsetX {
			get { return newModulesOffsetX; }
			set {
				double stepX = this.NewModulesStepX;

				newModulesOffsetX = value;
				while (newModulesOffsetX < 0) {
					newModulesOffsetX += stepX;
				}
				while (newModulesOffsetX >= stepX) {
					newModulesOffsetX -= stepX;
				}
			}
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double NewModulesOffsetY {
			get { return newModulesOffsetY; }
			set {
				double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
				double stepY = height;
				stepY += modulierendDistance * this.product.AssociatedRoom.AssociatedPlan.Measure.Value * (int)this.newModulesYDicht;

				newModulesOffsetY = value;
				while (newModulesOffsetY < 0) {
					newModulesOffsetY += stepY;
				}
				while (newModulesOffsetY >= stepY) {
					newModulesOffsetY -= stepY;
				}
			}
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public VerlegungsAbstand NewModulesXDicht {
			get { return this.newModulesXDicht; }
			set {
				this.newModulesXDicht = value;
				if (this.connectedPlanPanel != null && this.layoutAddArea != null) {
					this.connectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public VerlegungsAbstand NewModulesYDicht {
			get { return this.newModulesYDicht; }
			set {
				this.newModulesYDicht = value;
				if (this.connectedPlanPanel != null && this.layoutAddArea != null) {
					this.connectedPlanPanel.InvalidateGraphics();
				}
			}
		}

		[XmlIgnore]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public KlimaFlaechenModul.ModulOrientationEnum NewModulesStartingOrientation {
			get { return newModulesStartingOrientation; }
			set { newModulesStartingOrientation = value; }
		}

        private bool TryDrawModule(Graphics g, Matrix4D additionalTransformation, double x, double y, double rotation, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, bool bottomUp, Color color, bool cadPlan) {
			this.DrawModule(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20, orientation, new Point2D(x, y), rotation, additionalTransformation, g, bottomUp, true, color, false, false, cadPlan);
			return true;
		}

        private bool TryAddModule(bool bottomUp, double x, double y, double rotation, ModulKlimaBoden20Circuit circuit, ModulKlimaBoden20SubArea subArea, KlimaFlaechenList row, KlimaFlaechenModul.ModulOrientationEnum? orientation, bool onlyAddToExistingHks, out KlimaFlaechenModul addedModul, out KlimaFlaechenList rowOfAddedModul, KlimaFlaechenModul lastAddedModul, KlimaFlaechenList rowOfLastAddedModul)
        {
			KlimaFlaechenModul modul = new KlimaFlaechenModul();
            addedModul = null;
            rowOfAddedModul = null;
			modul.ModulType = KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20;
			modul.GraphPosX = x;
			modul.GraphPosY = y;
			modul.GraphRotation = rotation;
			if (orientation.HasValue) {
				modul.Orientation = orientation.Value;
			}
			modul.GraphBottomUp = bottomUp;

            KlimaFlaechenList usedRow = null;
            if (row != null)
            {
                usedRow = row;
            }
            if (usedRow != null || !onlyAddToExistingHks) {
				if (usedRow == null) {
					usedRow = new KlimaFlaechenList();
					subArea.Rows.Add(usedRow);					
				}
                usedRow.List.Add(modul);
				//modulesAdded.Add(modul);
				addedModul = modul;
				rowOfAddedModul = usedRow;
				
				if (addedModul != lastAddedModul && rowOfAddedModul == rowOfLastAddedModul) {
					Point2D output1 = lastAddedModul.GetOutputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);
					Point2D input1 = addedModul.GetInputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);

					Point2D output2 = addedModul.GetOutputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);
					Point2D input2 = lastAddedModul.GetInputConnection(this.product.AssociatedRoom.AssociatedPlan.Measure.Value, this.product.AssociatedRoom.AssociatedPlan.InvertYAxis, this.product);

					if ((output1 - input1).GetLength() <= (output2 - input2).GetLength()) {
						if (rowOfAddedModul.Links == null) {
							rowOfAddedModul.Links = new List<KlimaFlaechenModulVerbindung>();
						}
						rowOfAddedModul.Links.Add(new KlimaFlaechenModulVerbindung(lastAddedModul, addedModul, new Point2D[] { output1, input1 }, circuit, Project.Instance.GetPlannedProduct(this.product)));
					} else {
						if (rowOfAddedModul.Links == null) {
							rowOfAddedModul.Links = new List<KlimaFlaechenModulVerbindung>();
						}
						rowOfAddedModul.Links.Add(new KlimaFlaechenModulVerbindung(addedModul, lastAddedModul, new Point2D[] { output2, input2 }, circuit, Project.Instance.GetPlannedProduct(this.product)));
					}
				}
				return true;
			} else {
				return false;
			}
		}

		private int AddModulesForLayoutArea(AddModuleDelegate doIt, bool addIncomplete) {
			if (this.layoutAddArea == null || this.layoutAddArea.Count != 4) {
				return 0;
			}
			Matrix3D rotation = Transformation3D.Rotate(-this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);
			Matrix3D invRotation = rotation.GetInverse();

			double top = double.MaxValue;
			double bottom = double.MinValue;
			double left = double.MaxValue;
			double right = double.MinValue;
			foreach (Point2D point in this.layoutAddArea) {
				Point2D rotatedPoint = invRotation.Transform(point);
				if (rotatedPoint.X < left) {
					left = rotatedPoint.X;
				}
				if (rotatedPoint.X > right) {
					right = rotatedPoint.X;
				}
				if (rotatedPoint.Y < top) {
					top = rotatedPoint.Y;
				}
				if (rotatedPoint.Y > bottom) {
					bottom = rotatedPoint.Y;
				}
			}

			Point2D rotatedTopLeft = invRotation.Transform(this.layoutAddArea[0]);
			Point2D rotatedBottomLeft = invRotation.Transform(this.layoutAddArea[1]);
			Point2D rotatedBottomRight = invRotation.Transform(this.layoutAddArea[2]);
			Point2D rotatedTopRight = invRotation.Transform(this.layoutAddArea[3]);

			bool rightToLeft = rotatedTopLeft.X > rotatedTopRight.X;
			bool bottomUp = rotatedTopLeft.Y > rotatedBottomLeft.Y;
			left = Math.Min(rotatedTopLeft.X, rotatedTopRight.X);
			right = Math.Max(rotatedTopLeft.X, rotatedTopRight.X);
			top = Math.Min(rotatedTopLeft.Y, rotatedBottomLeft.Y);
			bottom = Math.Max(rotatedTopLeft.Y, rotatedBottomLeft.Y);

			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;

            bool invertYAxis = this.product.AssociatedRoom.AssociatedPlan.InvertYAxis;

			double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * measure;
			double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * measure;

			double stepX = this.NewModulesStepX;
			double stepY = height;
			stepY += modulierendDistance * measure * (int)this.newModulesYDicht;

			right -= width;
			bottom -= height;

			bool added = false;
			List<Polygon2D> roomList = new List<Polygon2D>();
			Polygon2D room = new Polygon2D(this.product.AssociatedRoom.RoomCoordinates);
			if (room.IsClockwise()) {
				room.Reverse();
			}
			roomList.Add(room);

			List<List<Polygon2D>> unusedList = new List<List<Polygon2D>>();
			foreach (List<Point2D> unused in this.product.AssociatedRoom.RoomUnusedAreaCoordinates) {
				Polygon2D newUnused = new Polygon2D(unused);
				if (newUnused.IsClockwise()) {
					newUnused.Reverse();
				}
				List<Polygon2D> newUnusedList = new List<Polygon2D>();
				newUnusedList.Add(newUnused);
				unusedList.Add(newUnusedList);
			}

			foreach (List<Point2D> staffelPoints in this.product.GraphConstruction.Staffeln) {
				Polygon2D staffel = new Polygon2D(staffelPoints);
				if (staffel.IsClockwise()) {
					staffel.Reverse();
				}
				List<Polygon2D> newUnusedList = new List<Polygon2D>();
				newUnusedList.Add(staffel);
				unusedList.Add(newUnusedList);
			}

			foreach (Polygon2D module in this.GetModuleAreas().Values) {
				if (module.IsClockwise()) {
					module.Reverse();
				}
				List<Polygon2D> newUnusedList = new List<Polygon2D>();
				newUnusedList.Add(module);
				unusedList.Add(newUnusedList);
			}

			Matrix4D xyRotation = Transformation4D.RotateZ(-this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);
			bool orientationLeft = this.newModulesStartingOrientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;

			double startY = bottomUp ? bottom - (newModulesOffsetY > 0 ? stepY - newModulesOffsetY : 0) : top + newModulesOffsetY;
			double endY = bottomUp ? top : bottom;
			double incY = bottomUp ? -stepY : stepY;
			double startX = rightToLeft ? right - (newModulesOffsetX > 0 ? stepX - newModulesOffsetX : 0) : left + newModulesOffsetX;
			double endX = rightToLeft ? left : right;
			double incX = rightToLeft ? -stepX : stepX;

            int rowNr = 0;
			for (double x = startX; x >= left && x <= right; x += incX) {
				orientationLeft = this.newModulesStartingOrientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT; ;
				KlimaFlaechenModul lastAddedModul = null;
				KlimaFlaechenModul addedModul = null;
                KlimaFlaechenList rowOfLastAddedModul = null;
                KlimaFlaechenList rowOfAddedModul = null;
				ModulKlimaBoden20Circuit circuitOfModul = null;
				for (double y = startY; y >= top && y <= bottom; y += incY) {
					Point3D xy = xyRotation.Transform(new Point3D(x, y, 0));
					Matrix4D moduleTransformation = Matrix4D.Identity;
					moduleTransformation = moduleTransformation * Transformation4D.Translation(xy.X, xy.Y, 0);
					moduleTransformation = moduleTransformation * Transformation4D.RotateZ(-this.NewModulesRotationInclPlanRotation * Math.PI / 180.0);

					Point2D topLeft2D = moduleTransformation.TransformTo2D(new Point2D(0, 0));
					Point2D topRight2D = moduleTransformation.TransformTo2D(new Point2D(width, 0));
					Point2D bottomRight2D = moduleTransformation.TransformTo2D(new Point2D(width, height));
					Point2D bottomLeft2D = moduleTransformation.TransformTo2D(new Point2D(0, height));

					Polygon2D newModule = new Polygon2D(new Point2D[] { topLeft2D, topRight2D, bottomRight2D, bottomLeft2D });
					if (newModule.IsClockwise()) {
						newModule.Reverse();
					}
					List<Polygon2D> newModuleList = new List<Polygon2D>();
					newModuleList.Add(newModule);

					bool fits = true;
					// check if module is inside room coordinates
					try {
						List<Polygon2D> difference = Polygon2D.GetDifference(newModuleList, roomList);
						if (difference != null && difference.Count > 0) {
							fits = false;
						}
					} catch (Exception e) {
						Console.WriteLine(e);
					}

					// check if module intersects unused area or another module
					bool intersectionFound = false;
					foreach (List<Polygon2D> unused in unusedList) {
                        List<Polygon2D> intersection = null;
                        try {
                            intersection = Polygon2D.GetIntersection(newModuleList, unused);
                        } catch {
                            // nothing to do
                        }
						if (intersection != null && intersection.Count > 0) {
							intersectionFound = true;
							break;
						}
					}
					if (intersectionFound) {
						fits = false;
					}

					if (addIncomplete || fits) {
						bool thisModuleBottomUp = bottomUp;
						KlimaFlaechenModul.ModulOrientationEnum thisModuleOrientation = (orientationLeft) ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
						doIt(xy.X, xy.Y, rowNr, -this.NewModulesRotationInclPlanRotation, out added, thisModuleOrientation, thisModuleBottomUp, out addedModul, out rowOfAddedModul, lastAddedModul, rowOfLastAddedModul, out circuitOfModul, fits);
                        if (added)
                        {
                            lastAddedModul = addedModul;                           
                            rowOfLastAddedModul = rowOfAddedModul;
                        }
						if (fits) {
							orientationLeft = !orientationLeft;
						}
					}
				}
                if (rowOfLastAddedModul != null)
                {
                    rowNr++;
                }
			}			

			return 1;
		}

		private Dictionary<KlimaFlaechenModul, Polygon2D> GetModuleAreas() {
			Dictionary<KlimaFlaechenModul, Polygon2D> moduleAreas = new Dictionary<KlimaFlaechenModul, Polygon2D>();
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * measure;
			double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * measure;
			foreach (ModulKlimaBoden20Circuit c in this.product.PlannedCircuits) {
				foreach (KlimaFlaechenModul modul in c.GetAllModules()) {
					Matrix3D transformation = Matrix3D.Identity;
					transformation = transformation * Transformation3D.Translation(modul.GraphPosX, modul.GraphPosY);
					transformation = transformation * Transformation3D.Rotate(modul.GraphRotation * Math.PI / 180.0);
					Polygon2D modulArea = new Polygon2D();
					modulArea.Add(transformation.Transform(new Point2D(0, 0)));
					modulArea.Add(transformation.Transform(new Point2D(width, 0)));
					modulArea.Add(transformation.Transform(new Point2D(width, height)));
					modulArea.Add(transformation.Transform(new Point2D(0, height)));
					moduleAreas.Add(modul, modulArea);
				}
			}

			return moduleAreas;
		}

		private Dictionary<KlimaFlaechenModul, Polygon2D> GetModuleInputs() {
			Dictionary<KlimaFlaechenModul, Polygon2D> inputAreas = new Dictionary<KlimaFlaechenModul, Polygon2D>();
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * measure;
			double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * measure;
			Point2D input12D, input22D, input32D, input42D;

			foreach (ModulKlimaBoden20Circuit c in this.product.PlannedCircuits) {
				foreach (KlimaFlaechenModul modul in c.GetAllModules()) {
                    KlimaFlaechenModul.ModulOrientationEnum orientationToUse = modul.Orientation;
                    if (this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) {
                        orientationToUse = orientationToUse == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
                    }
					Matrix3D transformation = Matrix3D.Identity;
					transformation = transformation * Transformation3D.Translation(modul.GraphPosX, modul.GraphPosY);
					transformation = transformation * Transformation3D.Rotate(modul.GraphRotation * Math.PI / 180.0);

                    if (this.product.AssociatedRoom.AssociatedPlan.InvertYAxis == modul.GraphBottomUp) {
                        if (orientationToUse == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
							input12D = transformation.Transform(new Point2D(width, height));
							input22D = transformation.Transform(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
							inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
                        } else if (orientationToUse == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
							input12D = transformation.Transform(new Point2D(0, height));
							input22D = transformation.Transform(new Point2D(0, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
							inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
						}
					} else {
                        if (orientationToUse == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
							input12D = transformation.Transform(new Point2D(0, 0));
							input22D = transformation.Transform(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
							inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
                        } else if (orientationToUse == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
							input12D = transformation.Transform(new Point2D(width, 0));
							input22D = transformation.Transform(new Point2D(width, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							input42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
							inputAreas.Add(modul, new Polygon2D(new Point2D[] { input12D, input22D, input32D, input42D }));
						}
					}
				}
			}

			return inputAreas;
		}

		private Dictionary<KlimaFlaechenModul, Polygon2D> GetModuleOutputs() {
			Dictionary<KlimaFlaechenModul, Polygon2D> outputAreas = new Dictionary<KlimaFlaechenModul, Polygon2D>();
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * measure;
			double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * measure;
			Point2D output12D, output22D, output32D, output42D;

			foreach (ModulKlimaBoden20Circuit c in this.product.PlannedCircuits) {
				foreach (KlimaFlaechenModul modul in c.GetAllModules()) {
                    KlimaFlaechenModul.ModulOrientationEnum orientationToUse = modul.Orientation;
                    if (this.product.AssociatedRoom.AssociatedPlan.InvertYAxis) {
                        orientationToUse = orientationToUse == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT ? KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT : KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
                    }
                    Matrix3D transformation = Matrix3D.Identity;
					transformation = transformation * Transformation3D.Translation(modul.GraphPosX, modul.GraphPosY);
					transformation = transformation * Transformation3D.Rotate(modul.GraphRotation * Math.PI / 180.0);

                    if (this.product.AssociatedRoom.AssociatedPlan.InvertYAxis == modul.GraphBottomUp) {
                        if (orientationToUse == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
							output12D = transformation.Transform(new Point2D(0, 0));
							output22D = transformation.Transform(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
							outputAreas.Add(modul, new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }));
                        } else if (orientationToUse == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
							output12D = transformation.Transform(new Point2D(width, 0));
							output22D = transformation.Transform(new Point2D(width, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
							outputAreas.Add(modul, new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }));
						}
					} else {
                        if (orientationToUse == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
							output12D = transformation.Transform(new Point2D(width, height));
							output22D = transformation.Transform(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output32D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output42D = transformation.Transform(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
							outputAreas.Add(modul, new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }));
                        } else if (orientationToUse == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
							output12D = transformation.Transform(new Point2D(0, height));
							output22D = transformation.Transform(new Point2D(0, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output32D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
							output42D = transformation.Transform(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
							outputAreas.Add(modul, new Polygon2D(new Point2D[] { output12D, output22D, output32D, output42D }));
						}
					}
				}
			}

			return outputAreas;
		}

		private Polygon2D GetModuleArea(KlimaFlaechenModul modul) {
			double measure = this.product.AssociatedRoom.AssociatedPlan.Measure.Value;
			double height = KlimaFlaechenModul.GetModuleHeight(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * measure;
			double width = KlimaFlaechenModul.GetModuleWidth(KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20) * measure;
			Matrix3D transformation = Matrix3D.Identity;
			transformation = transformation * Transformation3D.Translation(modul.GraphPosX, modul.GraphPosY);
			transformation = transformation * Transformation3D.Rotate(modul.GraphRotation * Math.PI / 180.0);
			Polygon2D modulArea = new Polygon2D();
			modulArea.Add(transformation.Transform(new Point2D(0, 0)));
			modulArea.Add(transformation.Transform(new Point2D(width, 0)));
			modulArea.Add(transformation.Transform(new Point2D(width, height)));
			modulArea.Add(transformation.Transform(new Point2D(0, height)));
			return modulArea;
		}

        public void DrawModule(KlimaFlaechenModul.ModulTypeEnum type, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, Point2D position, double rotation, Matrix4D additionalTransformation, Graphics g, bool bottomUp, bool highlight, Color circuitColor, bool highlightInput, bool highlightOutput, bool cadPlan) {
			if (this.product == null || this.product.GraphConstruction == null ||
				this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedPlan == null ||
				this.product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}

            Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientationForDrawing = orientation;
            if (cadPlan) {
                if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
                    orientationForDrawing = KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT;
                } else if (orientation == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
                    orientationForDrawing = KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT;
                }
            }

			additionalTransformation = additionalTransformation * Transformation4D.Translation(position.X, position.Y, 0);
			additionalTransformation = additionalTransformation * Transformation4D.RotateZ(rotation * Math.PI / 180.0);

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

			if (this.product.AssociatedRoom.AssociatedPlan.InvertYAxis == bottomUp) {
				directionTop12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionTop22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.10 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionTop32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, 0));

				directionBottom12D = additionalTransformation.TransformTo2D(new Point2D(width / 2 - 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom22D = additionalTransformation.TransformTo2D(new Point2D(width / 2 + 0.05 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
				directionBottom32D = additionalTransformation.TransformTo2D(new Point2D(width / 2, height - 0.2 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));

				if (highlightInput || highlightOutput) {
                    if (orientationForDrawing == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
						output12D = topLeft2D;
						output22D = additionalTransformation.TransformTo2D(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output32D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output42D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
						input12D = bottomRight2D;
						input22D = additionalTransformation.TransformTo2D(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input32D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input42D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
                    } else if (orientationForDrawing == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
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
                    if (orientationForDrawing == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
						input12D = topLeft2D;
						input22D = additionalTransformation.TransformTo2D(new Point2D(0, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input32D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						input42D = additionalTransformation.TransformTo2D(new Point2D(0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, 0));
						output12D = bottomRight2D;
						output22D = additionalTransformation.TransformTo2D(new Point2D(width, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output32D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value));
						output42D = additionalTransformation.TransformTo2D(new Point2D(width - 0.1 * this.product.AssociatedRoom.AssociatedPlan.Measure.Value, height));
                    } else if (orientationForDrawing == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
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
            if (orientationForDrawing == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_RIGHT) {
				g.FillPolygon(b, new PointF[] { topLeft, topRight, bottomRight, bottomLeft });
				if (type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60 || type == KlimaFlaechenModul.ModulTypeEnum.MODUL_60_60B) {
					g.DrawLines(p, new PointF[] { topRight, bottomRight, bottomLeft, topLeft, topRight, middle, bottomRight });
				} else {
					g.DrawLines(p, new PointF[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft, topRight });
				}
            } else if (orientationForDrawing == KlimaFlaechenModul.ModulOrientationEnum.ORIENTATION_LEFT) {
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

            if (orientationForDrawing != null) {
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
					g.FillPolygon(bInput, new PointF[] { input1, input2, input3, input4 });
					g.DrawPolygon(pInput, new PointF[] { input1, input2, input3, input4 });
				}
				if (highlightOutput) {
					PointF output1 = new PointF((float)output12D.X, (float)output12D.Y);
					PointF output2 = new PointF((float)output22D.X, (float)output22D.Y);
					PointF output3 = new PointF((float)output32D.X, (float)output32D.Y);
					PointF output4 = new PointF((float)output42D.X, (float)output42D.Y);

					Brush bOutput = new SolidBrush(Color.FromArgb(127, Color.Blue));
					Pen pOutput = new Pen(Color.Blue);
					g.FillPolygon(bOutput, new PointF[] { output1, output2, output3, output4 });
					g.DrawPolygon(pOutput, new PointF[] { output1, output2, output3, output4 });
				}
			}

			GraphicsPath path = new GraphicsPath();

			Matrix oldTransform = g.Transform;
			Matrix newTransform = g.Transform.Clone();
			g.Transform = new Matrix();

			if (this.product.AssociatedRoom.AssociatedPlan is CadPlan) {
				newTransform.RotateAt(-(float)(rotation), bottomLeft);
			} else {
				newTransform.RotateAt((float)(rotation), topLeft);
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
                case KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20:
                    moduleString = EuroplanRes.KlimaFlaechenModul_100_40_20_Short;
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
				g.DrawString(moduleString, new Font("Arial", 5.0f / g.DpiX * Math.Abs((float)additionalTransformation.M22) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value), new SolidBrush(Color.FromArgb(circuitColor.A, c)), bottomLeft);
				g.Transform = oldTransform;
			} else {
				g.DrawString(moduleString, new Font("Arial", 5.0f / g.DpiX * Math.Abs((float)additionalTransformation.M22) * this.product.AssociatedRoom.AssociatedPlan.Measure.Value), new SolidBrush(Color.FromArgb(circuitColor.A, c)), topLeft);
				g.Transform = oldTransform;
			}
		}

		private void DrawDxfModule(KlimaFlaechenModul.ModulTypeEnum type, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, Point2D position, Matrix4D additionalTransformation, WW.Cad.Model.DxfModel model, DxfLayer layer, bool bottomUp, Color circuitColor, double rotation) {
			if (this.product == null || this.product.GraphConstruction == null ||
						this.product.AssociatedRoom == null || this.product.AssociatedRoom.AssociatedPlan == null ||
						this.product.AssociatedRoom.AssociatedPlan.Measure == null) {
				return;
			}

			additionalTransformation = additionalTransformation * Transformation4D.Translation(position.X, position.Y, 0);
			additionalTransformation = additionalTransformation * Transformation4D.RotateZ(rotation * Math.PI / 180.0);

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
                case KlimaFlaechenModul.ModulTypeEnum.MODUL_100_40_20:
                    moduleString = EuroplanRes.KlimaFlaechenModul_100_40_20_Short;
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
			text.Style = model.TextStyles["HarreitherStyle"];
			text.Layer = layer;
			text.Color = c;
			text.Rotation = rotation / 180.0 * Math.PI;
			model.Entities.Add(text);
		}

		internal void DrawDxf(WW.Cad.Model.DxfModel model, DxfLayer modulLayer, DxfLayer floorConstructionLayer) {
			Matrix4D additionalTransformation = Matrix4D.Identity;

			if (this.product != null && this.product.AssociatedRoom != null && this.product.AssociatedRoom.RoomCoordinates != null) {
				if (this.product.AssociatedRoom.AssociatedPlan != null && this.product.AssociatedRoom.AssociatedPlan.Measure.HasValue) {
					if (this.product.GraphConstruction != null) {
						this.product.GraphConstruction.PaintDxf(model, floorConstructionLayer);
					}
				}

				if (this.mode != KlimaBodenMode.KDM_CONSTRUCTION) {
					Matrix3D rotation = Transformation3D.Rotate(-this.product.GraphConstruction.Rotation * Math.PI / 180.0);
					Matrix3D invRotation = rotation.GetInverse();

					List<KlimaFlaechenModul> selectedModules = this.GetAllSelectedModules();
					foreach (ModulBodenCircuit circuit in this.product.PlannedCircuits) {
						foreach (KlimaFlaechenModul modul in circuit.Row.List) {
							this.DrawDxfModule(modul.ModulType, modul.Orientation, invRotation.Transform(new Point2D(modul.GraphPosX, modul.GraphPosY)), additionalTransformation, model, modulLayer, modul.GraphBottomUp, circuit.CircuitColor, modul.GraphRotation);
						}
					}
				}

				foreach (ModulBodenCircuit c in this.product.PlannedCircuits) {
					if (c.Links != null) {
						foreach (KlimaFlaechenModulVerbindung link in c.Links) {
							link.DrawDxf(model, modulLayer, c.CircuitColor);
						}
					}
				}

				if (this.product.AssociatedRoom.RoomUnusedAreaCoordinates != null) {
					EntityColor gray = EntityColor.CreateFromRgb(Color.Gray.ToArgb());
					foreach (List<Point2D> unusedArea in this.product.AssociatedRoom.RoomUnusedAreaCoordinates) {
						Polygon2D polygon = new Polygon2D(unusedArea);
						DxfPolyline2D polyLine = new DxfPolyline2D(gray, polygon.ToArray());
						polyLine.Closed = true;
						polyLine.Layer = modulLayer;
						model.Entities.Add(polyLine);
					}
				}
			}
		}

		public ModulKlimaBoden20Circuit ConfirmNewModules() {
            ModulKlimaBoden20SubArea newSubArea = null;
            ModulKlimaBoden20Circuit newCircuit = null;
            ModulKlimaBoden20SubArea oldSubArea = null;
            KlimaFlaechenList oldRow = null;
            ModulKlimaBoden20Circuit circuitToAdd = this.highlightCircuit;
            bool onlyAddToExisting = false;
            if (this.highlightCircuit == null && highlightSubArea == null && highlightRow == null)
            {
                if (this.product.Connections != null && this.product.Connections.Count > 0)
                {
                    if (MessageBox.Show(EuroplanRes.ModulKlimaBoden20Planner_AnbindeleitungLoeschen, EuroplanRes.ModulKlimaBoden20Planner_NeuerHeizkreisTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        onlyAddToExisting = true;                       
                    }
                    else
                    {
                        this.product.Connections.Clear();
                    }
                }
                newCircuit = new ModulKlimaBoden20Circuit(this.Product);
                newCircuit.CircuitColor = this.GetNewCircuitColor();
                newSubArea = newCircuit.SubAreas[0];
                newSubArea.Rows.Clear();
                circuitToAdd = newCircuit;
            }
            else if (this.highlightSubArea == null && highlightRow == null)
            {
                newSubArea = new ModulKlimaBoden20SubArea();
                newSubArea.Rows.Clear();
            }
            else if (this.highlightRow == null)
            {
                oldSubArea = this.highlightSubArea;
            }
            else
            {
                oldRow = this.highlightRow;
                int tmp;
                KlimaFlaechenModul mTmp = oldRow.List[0];
                oldSubArea = this.product.GetCircuitForModul(mTmp, out tmp).GetSubareaForModul(mTmp, out tmp);              
            }

            List<KlimaFlaechenModul> modulesAdded = new List<KlimaFlaechenModul>();

            KlimaFlaechenList rowOfFirstAddedModul = oldRow;

            AddModuleDelegate addModuleDelegate = delegate(double x, double y, int rowNr, double rotation, out bool added, Nullable<KlimaFlaechenModul.ModulOrientationEnum> orientation, bool bottomUp, out KlimaFlaechenModul addedModul, out KlimaFlaechenList rowOfAddedModul, KlimaFlaechenModul lastAddedModul, KlimaFlaechenList rowOfLastAddedModul, out ModulKlimaBoden20Circuit circuitOfModul, bool fits)
            {
                ModulKlimaBoden20SubArea sa = (newSubArea != null ? newSubArea : oldSubArea);
                KlimaFlaechenList r = rowOfFirstAddedModul;
                if (rowOfFirstAddedModul != null)
                {
                    int lastRowIndex = sa.Rows.IndexOf(rowOfFirstAddedModul);
                    if (lastRowIndex >= 0 && lastRowIndex + rowNr < sa.Rows.Count)
                    {
                        r = sa.Rows[lastRowIndex + rowNr];
                    }
                    else
                    {
                        r = null;
                    }
                }
                added = this.TryAddModule(this.layoutAddAreaBottomUp, x, y, rotation, circuitToAdd, sa, r, orientation, onlyAddToExisting, out addedModul, out rowOfAddedModul, lastAddedModul, rowOfLastAddedModul);
                if (rowOfFirstAddedModul == null)
                {
                    rowOfFirstAddedModul = rowOfAddedModul;
                }
                circuitOfModul = circuitToAdd;
            };

            int count = this.AddModulesForLayoutArea(addModuleDelegate, false);

            if (this.UpdateNewCount != null)
            {
                this.UpdateNewCount(this, new UpdateNewCountArgs(0));
            }


            if (count > 0)
            {
                if (!this.product.ContainsModules && newCircuit != null)
                {
                    this.product.PlannedCircuits.Clear();
                }
                if (newCircuit != null && newCircuit.CountModules() > 0)
                {
                    this.product.PlannedCircuits.Add(newCircuit);
                }
                else if (newSubArea != null && newSubArea.CountModules() > 0)
                {
                    this.highlightCircuit.SubAreas.Add(newSubArea);
                }
                if (this.projectChanged != null)
                {
                    this.projectChanged(this);
                }
                if (this.ListsNeedUpdate != null)
                {
                    this.ListsNeedUpdate(this, new ListNeedsUpdateEventArgs(newCircuit != null && newCircuit.CountModules() > 0));
                }
            }
            this.layoutAddArea = null;
            this.Mode = KlimaBodenMode.KDM_LAYOUT_ADD_AREA;
            return count > 0 ? newCircuit : null;
		}

		private ModulKlimaBoden20Circuit highlightCircuit = null;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ModulKlimaBoden20Circuit HighlightCircuit {
			get { return this.highlightCircuit; }
			set {
				this.highlightCircuit = value;
				this.highlightModules = null;
                this.highlightSubArea = null;
                this.highlightRow = null;
				if (this.ConnectedPlanPanel != null) {
					this.ConnectedPlanPanel.InvalidateGraphics();
				}
			}
		}

        private ModulKlimaBoden20SubArea highlightSubArea = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ModulKlimaBoden20SubArea HighlightSubArea
        {
            get { return this.highlightSubArea; }
            set
            {
                this.highlightSubArea = value;
                this.highlightCircuit = null;
                this.highlightRow = null;
                this.highlightModules = null;
                if (this.ConnectedPlanPanel != null)
                {
                    this.ConnectedPlanPanel.InvalidateGraphics();
                }
            }
        }

        private KlimaFlaechenList highlightRow = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public KlimaFlaechenList HighlightRow
        {
            get { return this.highlightRow; }
            set
            {
                this.highlightRow = value;
                this.highlightCircuit = null;
                this.highlightSubArea = null;
                this.highlightModules = null;
                if (this.ConnectedPlanPanel != null)
                {
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

        public List<KlimaFlaechenModul> GetAllSelectedModules()
        {
            List<KlimaFlaechenModul> modules = new List<KlimaFlaechenModul>();
            if (this.HighlightCircuit != null)
            {
                foreach (ModulKlimaBoden20SubArea subArea in this.HighlightCircuit.SubAreas)
                {
                    foreach (KlimaFlaechenList row in subArea.Rows)
                    {
                        foreach (KlimaFlaechenModul modul in row.List)
                        {
                            modules.Add(modul);
                        }
                    }
                }
            }
            else if (this.HighlightSubArea != null)
            {
                foreach (KlimaFlaechenList row in this.HighlightSubArea.Rows)
                {
                    foreach (KlimaFlaechenModul modul in row.List)
                    {
                        modules.Add(modul);
                    }
                }
            }
            else if (this.HighlightRow != null)
            {
                foreach (KlimaFlaechenModul modul in this.HighlightRow.List)
                {
                    modules.Add(modul);
                }
            }
            else if (this.HighlightModules != null)
            {
                foreach (KlimaFlaechenModul modul in this.HighlightModules)
                {
                    modules.Add(modul);
                }
            }
            return modules;
        }

		public bool ContainsNotConfirmedModules {
			get {
				return this.layoutAddArea != null;
			}
		}

		private Color GetNewCircuitColor() {
			Dictionary<Color, int> dict = new Dictionary<Color, int>();
			foreach (Color c in this.circuitColors) {
				dict.Add(c, 0);
			}
			foreach (ModulKlimaBoden20Circuit circuit in this.product.PlannedCircuits) {
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

        public bool ShowPlanBackground {
            get { return Europlan.Common.Product.ShowPlanInBackground; }
        }
	}
}
