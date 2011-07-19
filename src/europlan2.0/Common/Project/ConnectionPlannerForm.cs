using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using WW.Math.Geometry;

namespace Europlan.Common {
	public partial class ConnectionPlannerForm : Form {

		//private Floor floor;
		//private Dictionary<Distributor, Distributor.GraphicalRepresentation> distributors = new Dictionary<Distributor, Distributor.GraphicalRepresentation>();

		private bool changed = false;

		public ConnectionPlannerForm(Floor floor) {
			InitializeComponent();

			this.connectionPlanner.Floor = floor;

			/*foreach (Distributor distributor in floor.GetAllAvailableDistributors()) {
				foreach (Distributor.GraphicalRepresentation rep in distributor.GraphicalRepresentations) {
					if (rep.floorId == floor.Id) {
						this.distributors.Add(distributor, rep);
					}
				}
			}*/

			this.UpdateToolbar(null);
		}

		private void UpdateControls() {
		}

		private void btnZoomOut_Click(object sender, EventArgs e) {
			this.planPanel.AddScale(0.9, null);
		}

		private void btnZoomIn_Click(object sender, EventArgs e) {
			this.planPanel.AddScale(1.1, null);
		}

		private void btnMove_Click(object sender, EventArgs e) {
			if (!btnMove.Checked) {
				this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.CM_NONE;
				this.planPanel.Mode = PlanMode.PM_MOVE;
				this.UpdateButtons();
			}
		}

		private void UpdateButtons() {
			if (this.planPanel.Mode == PlanMode.PM_MOVE) {
				this.btnMove.Checked = true;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnPickConnection.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_ADD_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConnections.Checked = true;
				this.btnDeleteConnection.Checked = false;
				this.btnPickConnection.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_DEL_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = true;
				this.btnPickConnection.Checked = false;
			} else if ((this.planPanel.Mode == PlanMode.PM_PLANNER_CLICK || this.planPanel.Mode == PlanMode.PM_PLANNER_DRAG) && this.connectionPlanner.Mode == ConnectionPlanner.ConnectionMode.KDM_SELECT_CONNECTION) {
				this.btnMove.Checked = false;
				this.btnConnections.Checked = false;
				this.btnDeleteConnection.Checked = false;
				this.btnPickConnection.Checked = true;
			} else {
				this.btnMove.Checked = false;
			}
		}

		private TabPage previousTab = null;

		private void UpdateToolbar(TabPage tabPage) {
		}

		private void ModulKlimaBodenPlannerForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ConnectionPlannerForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
		}

		private void ModulKlimaBodenPlannerForm_FormClosing(object sender, FormClosingEventArgs e) {
			/*double measure = this.connectionPlanner.Floor.AssociatedPlan.Measure.Value;
			// Update connectionpipes in products to match the graphical connections
			foreach (Room room in this.connectionPlanner.Floor.Rooms) {
				foreach (PlannedProduct pp in room.PlannedProducts) {
					double restFirstCircuitVl = 0;
					double restFirstCircuitRl = 0;
					double restOtherCircuitVl = 0;
					double restOtherCircuitRl = 0;
					int restOtherCircuitCountVl = 0;
					int restOtherCircuitCountRl = 0;
					Dictionary<PlannedProduct, double> firstCircuitVl = new Dictionary<PlannedProduct, double>();
					Dictionary<PlannedProduct, double> otherCircuitVl = new Dictionary<PlannedProduct, double>();
					Dictionary<PlannedProduct, int> otherCircuitCountVl = new Dictionary<PlannedProduct, int>();
					Dictionary<PlannedProduct, double> firstCircuitRl = new Dictionary<PlannedProduct, double>();
					Dictionary<PlannedProduct, double> otherCircuitRl = new Dictionary<PlannedProduct, double>();
					Dictionary<PlannedProduct, int> otherCircuitCountRl = new Dictionary<PlannedProduct, int>();
					Dictionary<PlannedProduct, Room> rooms = new Dictionary<PlannedProduct, Room>();
					foreach (GraphicalProductConnection connection in pp.Product.Connections) {
						//foreach (int circuitIndex in connection.ProductCircuitIndices) {
							*//*if (false && circuitIndex == 0) { // TODO
								if (connection.Vorlauf) {
									restFirstCircuitVl = connection.GetLength(1); // do not use measure here as unmeasured lengths will be subtracted
								} else {
									restFirstCircuitRl = connection.GetLength(1);
								}
							} else {
								if (connection.Vorlauf) {
									restOtherCircuitVl += connection.GetLength(1);
									restOtherCircuitCountVl++;
								} else {
									restOtherCircuitRl += connection.GetLength(1);
									restOtherCircuitCountRl++;
								}
							}
							foreach (Room room2 in this.connectionPlanner.Floor.Rooms) {
								foreach (PlannedProduct pp2 in room2.PlannedProducts) {
									rooms[pp2] = room2;
									if (pp != pp2 && pp2.Product.Type == connection.ConnectionType && pp2.Product.GraphicalArea != null) {
										double length = connection.GetPartInsidePolygon(pp2.Product.GraphicalArea);
										if (length > 0) {
											if (false && circuitIndex == 0) { // TODO
												if (connection.Vorlauf) {
													firstCircuitVl[pp2] = length;
													restFirstCircuitVl -= length;
												} else {
													firstCircuitRl[pp2] = length;
													restFirstCircuitRl -= length;
												}
											} else {
												if (connection.Vorlauf) {
													if (otherCircuitVl.ContainsKey(pp2)) {
														otherCircuitVl[pp2] += length;
														otherCircuitCountVl[pp2]++;
													} else {
														otherCircuitVl[pp2] = length;
														otherCircuitCountVl[pp2] = 1;
													}
													restOtherCircuitVl -= length;
												} else {
													if (otherCircuitRl.ContainsKey(pp2)) {
														otherCircuitRl[pp2] += length;
														otherCircuitCountRl[pp2]++;
													} else {
														otherCircuitRl[pp2] = length;
														otherCircuitCountRl[pp2] = 1;
													}
													restOtherCircuitRl -= length;
												}
											}
										}
									}
								}
							}*//*
						//}
					}
					if (pp.Product.Connections != null && pp.Product.Connections.Count > 0) {
						pp.Product.PlannedConnectionPipes.Clear();
						// add connections for first circuit
						foreach (KeyValuePair<PlannedProduct, double> kvp in firstCircuitVl) {
							ConnectionPipe pipe = new ConnectionPipe();
							pipe.ConnectionThrough = kvp.Key;
							pipe.Room = rooms[kvp.Key];
							pipe.Vorlauf = kvp.Value / measure;
							pipe.OnlyFirst = true;
							if (firstCircuitRl.ContainsKey(kvp.Key)) {
								pipe.Ruecklauf = firstCircuitRl[kvp.Key] / measure;
								firstCircuitRl.Remove(kvp.Key);
							}
							pp.Product.PlannedConnectionPipes.Add(pipe);
						}
						foreach (KeyValuePair<PlannedProduct, double> kvp in firstCircuitRl) {
							ConnectionPipe pipe = new ConnectionPipe();
							pipe.ConnectionThrough = kvp.Key;
							pipe.Room = rooms[kvp.Key];
							pipe.Ruecklauf = kvp.Value / measure;
							pipe.OnlyFirst = true;
							pp.Product.PlannedConnectionPipes.Add(pipe);
						}
						if (restFirstCircuitVl > 0 || restFirstCircuitRl > 0) {
							ConnectionPipe pipe = new ConnectionPipe();
							pipe.Vorlauf = restFirstCircuitVl / measure;
							pipe.Ruecklauf = restFirstCircuitRl / measure;
							pipe.OnlyFirst = true;
							pp.Product.PlannedConnectionPipes.Add(pipe);
						}

						// add connections for all circuit
						foreach (KeyValuePair<PlannedProduct, double> kvp in otherCircuitVl) {
							ConnectionPipe pipe = new ConnectionPipe();
							pipe.ConnectionThrough = kvp.Key;
							pipe.Room = rooms[kvp.Key];
							pipe.Vorlauf = kvp.Value / otherCircuitCountVl[kvp.Key] / measure;
							pipe.OnlyFirst = false;
							if (firstCircuitRl.ContainsKey(kvp.Key)) {
								pipe.Ruecklauf = firstCircuitRl[kvp.Key] / otherCircuitCountRl[kvp.Key] / measure;
								firstCircuitRl.Remove(kvp.Key);
							}
							pp.Product.PlannedConnectionPipes.Add(pipe);
						}
						foreach (KeyValuePair<PlannedProduct, double> kvp in otherCircuitRl) {
							ConnectionPipe pipe = new ConnectionPipe();
							pipe.ConnectionThrough = kvp.Key;
							pipe.Room = rooms[kvp.Key];
							pipe.Ruecklauf = kvp.Value / otherCircuitCountRl[kvp.Key] / measure;
							pipe.OnlyFirst = false;
							pp.Product.PlannedConnectionPipes.Add(pipe);
						}
						if (restOtherCircuitVl > 0 || restOtherCircuitRl > 0) {
							ConnectionPipe pipe = new ConnectionPipe();
							pipe.Vorlauf = restOtherCircuitVl / restOtherCircuitCountVl / measure;
							pipe.Ruecklauf = restOtherCircuitRl / restOtherCircuitCountRl / measure;
							pipe.OnlyFirst = false;
							pp.Product.PlannedConnectionPipes.Add(pipe);
						}
					}
				}
			}*/
			this.connectionPlanner.ReGenerateConnectionPipes();

			SettingsKey settings = SettingsFile.Settings["ConnectionPlannerForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			SettingsFile.Update();
		}

		public bool Changed {
			get {
				// TODO remove this when the cahnged flag is properly implemented
				return true;
				return this.changed; 
			}
		}

		private int ignoreRandfries = 0;
		private void numRandfries_ValueChanged(object sender, EventArgs e) {
			if (ignoreRandfries == 0) {
				this.changed = true;
				this.planPanel.InvalidateGraphics();
			}
		}

		private void btnConnections_Click(object sender, EventArgs e) {
			this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.KDM_ADD_CONNECTION;
			this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
			this.UpdateButtons();
		}

		private void btnDeleteConnection_Click(object sender, EventArgs e) {
			this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.KDM_DEL_CONNECTION;
			this.planPanel.Mode = PlanMode.PM_PLANNER_CLICK;
			this.UpdateButtons();
		}

		private void btnPickConnection_Click(object sender, EventArgs e) {
			this.connectionPlanner.Mode = ConnectionPlanner.ConnectionMode.KDM_SELECT_CONNECTION;
			this.planPanel.Mode = PlanMode.PM_PLANNER_DRAG;
			this.UpdateButtons();
		}

		private void btnBoden_Click(object sender, EventArgs e) {
			this.btnBoden.Checked = true;
			this.btnDecke.Checked = false;
			this.connectionPlanner.PlanFloor = true;
		}

		private void btnDecke_Click(object sender, EventArgs e) {
			this.btnBoden.Checked = false;
			this.btnDecke.Checked = true;
			this.connectionPlanner.PlanCeiling = true;
		}

		private void btnCircuits_Click(object sender, EventArgs e) {
			if (!this.btnFirstCircuit.Checked && !this.btnOtherCircuits.Checked) {
				if (sender == this.btnFirstCircuit) {
					this.btnOtherCircuits.Checked = true;
				} else {
					this.btnFirstCircuit.Checked = true;
				}
			}
			this.connectionPlanner.AddFirstCircuit = this.btnFirstCircuit.Checked;
			this.connectionPlanner.AddOtherCircuits = this.btnOtherCircuits.Checked;
		}

	}
}