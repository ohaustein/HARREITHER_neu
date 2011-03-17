using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {

	public partial class FloorSummaryPanel : UserControl, IEditorUserControl {
		
		public event ProjectStructureChangedHandler ProjectStructureChanged;
		public event ProjectChangedHandler ProjectChanged;
		public event TreeSelectionRequestedHandler TreeSelectionRequested;

		private Floor floor;
		private bool updateControlOngoing = false;
		
		public FloorSummaryPanel() {
			InitializeComponent();

			this.SetLanguage();

			//this.gridRooms.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
			this.gridRooms.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
			this.gridRooms.CellPainting += new DataGridViewCellPaintingEventHandler(gridRooms_CellPainting);
			this.gridRooms.Paint += new PaintEventHandler(gridRooms_Paint);
			this.gridRooms.ColumnWidthChanged += new DataGridViewColumnEventHandler(gridRooms_ColumnWidthChanged);
		}

		private void SetLanguage() {
			this.btnWhatIsNext.Text = EuroplanRes.General_WieGehtsWeiter; //"Wie geht\'s weiter?";

			this.btnAddDistributor.Text = EuroplanRes.FloorSummaryPanel_VerteilerAnlegen; //"Verteiler anlegen";
			this.label1.Text = EuroplanRes.FloorSummaryPanel_Geschossdaten; //"Geschoßdaten:";
			this.btnRemoveDistributor.Text = EuroplanRes.FloorSummaryPanel_VerteilerLoeschen; //"Verteiler löschen";
			this.idDataGridViewTextBoxColumn.HeaderText = EuroplanRes.FloorSummaryPanel_Nummer; //"Nr.\n";
			this.idDataGridViewTextBoxColumn.ToolTipText = EuroplanRes.FloorSummaryPanel_NummerLang; //"eindeutige Raumnummer";
			this.nameDataGridViewTextBoxColumn.HeaderText = EuroplanRes.FloorSummaryPanel_Bezeichnung; //"Bezeichnung\n";
			this.nameDataGridViewTextBoxColumn.ToolTipText = EuroplanRes.FloorSummaryPanel_BezeichnungLang; //"Bezeichnung des Raumes";
			this.Area.HeaderText = EuroplanRes.FloorSummaryPanel_Flaeche; //"A\n(m²)";
			this.Area.ToolTipText = EuroplanRes.FloorSummaryPanel_FlaecheLang; //"Raumfläche";
			this.RoomTemperature.HeaderText = EuroplanRes.FloorSummaryPanel_RaumtemperaturHeiz; //"Ti\n(°C)";
			this.RoomTemperature.ToolTipText = EuroplanRes.FloorSummaryPanel_RaumtemperaturHeizLang; //"Norminnentemperatur laut Wärmebedarfsberechnung";
			this.HeatLoad.HeaderText = EuroplanRes.FloorSummaryPanel_Waermebedarf; //"QN\n(W)";
			this.HeatLoad.ToolTipText = EuroplanRes.FloorSummaryPanel_WaermebedarfLang; //"Normwärmebedarf laut Wärmebedarfsrechnung";
			this.FloorHeatingLoss.HeaderText = EuroplanRes.FloorSummaryPanel_Fussbodentransmissionen; //"QFB\n(W)";
			this.FloorHeatingLoss.ToolTipText = EuroplanRes.FloorSummaryPanel_FussbodentransmissionenLang; //"Im Wärmebedarf enthaltene Fußbodentransmissionen";
			this.AdditionalHeatLoad.HeaderText = EuroplanRes.FloorSummaryPanel_Fremdwaermeleistung; //"QFr\n(W)";
			this.AdditionalHeatLoad.ToolTipText = EuroplanRes.FloorSummaryPanel_FremdwaermeleistungLang; //"Zusätzliche Fremdwärmeleistung";
			this.RoomCoolTemperature.HeaderText = EuroplanRes.FloorSummaryPanel_RaumtemperaturKuehl; //"Ti\n(°C)";
			this.RoomCoolTemperature.ToolTipText = EuroplanRes.FloorSummaryPanel_RaumtemperaturKuehlLang; //"Gewünschte Rauminnentemperatur bei Kühlung";
			this.RoomRelativeHumidity.HeaderText = EuroplanRes.FloorSummaryPanel_Luftfeuchtigkeit; //"RF\n(%)";
			this.RoomRelativeHumidity.ToolTipText = EuroplanRes.FloorSummaryPanel_LuftfeuchtigkeitLang; //"Relative Luftfeuchtigkeit für Kühlung";
			this.CoolLoad.HeaderText = EuroplanRes.FloorSummaryPanel_Kuehlleistung; //"QKühl\n(W)";
			this.CoolLoad.ToolTipText = EuroplanRes.FloorSummaryPanel_KuehlleistungLang; //"Erforderliche Külleistung laut Kühllastberechnung";
			this.colView.HeaderText = EuroplanRes.FloorSummaryPanel_Bearbeiten; //"Bearbeiten\n";
			this.IsNassraum.HeaderText = EuroplanRes.FloorSummaryPanel_Nassraum;
			this.chkAssignPlan.Text = EuroplanRes.FloorSummaryPanel_PlanVorhanden;
		}

		public void UpdateControl(bool resetUserInterface) {
			updateControlOngoing = true;
			if (this.Tag != null) {
				this.floor = this.Tag as Floor;
				this.floorRoomsSource.DataSource = this.floor.Rooms;
			}
			List<DataGridViewColumn> selectedCols = null;
			Room selectedRoom = null;
			if (this.gridRooms.SelectedRows.Count > 0) {
				selectedRoom = this.gridRooms.SelectedRows[0].DataBoundItem as Room;
			} else if (this.gridRooms.SelectedCells.Count > 0) {
				selectedCols = new List<DataGridViewColumn>();
				foreach (DataGridViewCell cell in this.gridRooms.SelectedCells) {
					if (selectedRoom == null) {
						selectedRoom = cell.OwningRow.DataBoundItem as Room;
					}
					if (selectedRoom != null && selectedRoom == cell.OwningRow.DataBoundItem) {
						selectedCols.Add(cell.OwningColumn);
					}
				}
			}

			this.lblFloorName.Text = floor.Name;
			this.floorRoomsSource.ResetBindings(false);
			this.btnRemoveDistributor.Enabled = floor.Distributors.Count > 0;

			if (selectedRoom != null) {
				foreach (DataGridViewRow row in this.gridRooms.Rows) {
					if (row.DataBoundItem == selectedRoom) {
						if (selectedCols == null) {
							row.Selected = true;
						} else {
							foreach (DataGridViewColumn col in selectedCols) {
								if (row.Cells[col.Index].Visible) {
									row.Cells[col.Index].Selected = true;
								}
							}
						}
					}
				}
			}
			RoomCoolTemperature.Visible = Project.Instance.CalculateCoolLoad;
			RoomRelativeHumidity.Visible = Project.Instance.CalculateCoolLoad;
			CoolLoad.Visible = Project.Instance.CalculateCoolLoad;

			planBindingSource.DataSource = null;
			if (Project.Instance.ImportedPlans.Count > 0) {
				chkAssignPlan.Enabled = true;
				if (floor.AssociatedPlanId != null) {
					planBindingSource.DataSource = Project.Instance.ImportedPlans;
					cmbPlans.Enabled = true;
					chkAssignPlan.Checked = true;
					foreach (Plan plan in Project.Instance.ImportedPlans) {
						if (plan.Id == floor.AssociatedPlanId) {
							cmbPlans.SelectedItem = plan;
						}
					}
				} else {
					chkAssignPlan.Checked = false;
					cmbPlans.Enabled = false;
				}
			} else {
				chkAssignPlan.Enabled = false;
				cmbPlans.Enabled = false;
			}
			updateControlOngoing = false;
		}

		public bool AllowLeave() {
			return true;
		}

		private void gridRooms_CellClick(object sender, DataGridViewCellEventArgs e) {
			//this.gridRooms.EditMode = (e.ColumnIndex == -1 ? DataGridViewEditMode.EditOnKeystroke : DataGridViewEditMode.EditOnEnter);
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.gridRooms.Columns.Count &&
					this.gridRooms.Columns[e.ColumnIndex] == this.colView &&
					e.RowIndex >= 0 && e.RowIndex < this.gridRooms.Rows.Count) {
				Room r = this.gridRooms.Rows[e.RowIndex].DataBoundItem as Room;
				if (r != null) {
					if (TreeSelectionRequested != null) {
						TreeSelectionRequested(this, r);
					}
				}
			}

		}

		private void gridRooms_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
			if (e.RowIndex >= 0 && e.RowIndex < this.gridRooms.Rows.Count &&
					this.gridRooms.Rows[e.RowIndex].DataBoundItem == null) {
				e.PaintCells(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border | DataGridViewPaintParts.ErrorIcon | DataGridViewPaintParts.Focus | DataGridViewPaintParts.SelectionBackground);
				e.PaintHeader(DataGridViewPaintParts.All);
				e.Handled = true;
			}
		}

		private void gridRooms_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.gridRooms.Columns.Count &&
					e.RowIndex >= 0 && e.RowIndex < this.gridRooms.Rows.Count) {
				if (this.gridRooms.Columns[e.ColumnIndex] == this.Area) {
					Room r = this.gridRooms.Rows[e.RowIndex].DataBoundItem as Room;
					if (r != null && oldArea.HasValue) {
						double floorArea = 0;
						double ceilingArea = 0;
						foreach (PlannedProduct pp in r.PlannedProducts) {
							floorArea += pp.Product.PlannedFloorArea;
							ceilingArea += pp.Product.PlannedCeilingArea;
						}
						if (floorArea > r.Area || ceilingArea > r.Area) {
							string message = EuroplanRes.FloorSummaryPanel_RaumflaecheZuKleinText;
							message = message.Replace("%SYSTEME%", Math.Round((floorArea > ceilingArea ? floorArea : ceilingArea), 1).ToString());
							message = message.Replace("%RAUM%", Math.Round(r.Area, 1).ToString());
							if (MessageBox.Show(message, EuroplanRes.FloorSummaryPanel_RaumflaecheZuKleinTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
								foreach (PlannedProduct pp in r.PlannedProducts) {
									if (floorArea > r.Area && pp.Product.PlannedFloorArea > 0) {
										pp.Product.PlannedFloorArea = (float)(pp.Product.PlannedFloorArea * r.Area / floorArea);
									}
									if (ceilingArea > r.Area && pp.Product.PlannedCeilingArea > 0) {
										pp.Product.PlannedCeilingArea = (float)(pp.Product.PlannedCeilingArea * r.Area / ceilingArea);
									}
								}
								foreach (PlannedProduct pp in r.PlannedProducts) {
									List<PlannedProduct> connectedProducts = this.floor.FindConnectedProduct(pp);
									foreach (PlannedProduct connectedProduct in connectedProducts) {
										connectedProduct.Product.ConfigureProduct(connectedProduct.RequestedHeatLoad, connectedProduct.RequestedCoolLoad, connectedProduct.CalculateHeat, connectedProduct.CalculateCool, false);
									}
									pp.Product.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, false);
									foreach (PlannedProduct connectedProduct in connectedProducts) {
										connectedProduct.Product.ConfigureProduct(connectedProduct.RequestedHeatLoad, connectedProduct.RequestedCoolLoad, connectedProduct.CalculateHeat, connectedProduct.CalculateCool, false);
									}
									PlannedProduct inverseConnectedProduct = null;
									if (pp.Product.PlannedConnection != null && pp.Product.PlannedConnection.ConnectionType == ProductConnection.ConnectionTypeEnum.OTHER_PRODUCT) {
										inverseConnectedProduct = pp.Product.PlannedConnection.OtherProduct;
									}
									if (inverseConnectedProduct != null) {
										inverseConnectedProduct.Product.ConfigureProduct(inverseConnectedProduct.RequestedHeatLoad, inverseConnectedProduct.RequestedCoolLoad, inverseConnectedProduct.CalculateHeat, inverseConnectedProduct.CalculateCool, false);
										pp.Product.ConfigureProduct(pp.RequestedHeatLoad, pp.RequestedCoolLoad, pp.CalculateHeat, pp.CalculateCool, false);
									}
								}
							} else {
								this.gridRooms.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = oldArea.Value;
							}
						}
						oldArea = null;
					}
				}
				if (this.gridRooms.Columns[e.ColumnIndex] == this.nameDataGridViewTextBoxColumn && ProjectStructureChanged != null) {
					ProjectStructureChanged(this);
				}
				if (ProjectChanged != null) {
					ProjectChanged(this);
				}
			}
		}

		private void gridRooms_UserDeletedRow(object sender, DataGridViewRowEventArgs e) {
			gridRooms.AllowUserToAddRows = true;
			if (ProjectStructureChanged != null) {
				ProjectStructureChanged(this);
			}
		}

		private void gridRooms_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
			gridRooms.AllowUserToAddRows = false;
		}

		private void btnAddDistributor_Click(object sender, EventArgs e) {
			if (Project.Instance != null && Project.Instance.RegulatorCircuits.Count > 0) {
				NewDistributorForm form = new NewDistributorForm();
				DialogResult result = form.ShowDialog();
				if (result == DialogResult.OK) {
					if (form.Distributor != null) {
						this.floor.Distributors.Add(form.Distributor);
						if (ProjectStructureChanged != null) {
							ProjectStructureChanged(this);
						}
					}
				}
				form.Dispose();
			} else {
				MessageBox.Show(EuroplanRes.FloorSummaryPanel_KeinRegelkreisText, EuroplanRes.FloorSummaryPanel_KeinRegelkreisTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void btnRemoveDistributor_Click(object sender, EventArgs e) {
			if (Project.Instance != null) {
				SelectDistributorForm form = new SelectDistributorForm(floor.Distributors);
				if (form.ShowDialog() == DialogResult.OK) {
					Distributor toDelete = form.SelectedDistributor;
					if (toDelete != null) {
						bool usedForQuickDimensioning = false;
						foreach (Floor f in Project.Instance.Floors) {
							foreach (Room r in f.Rooms) {
								foreach (Product p in r.UsedProductsForQuickDimensioning) {
									if (p.QuickDimensioningConnectedDistributors.ContainsKey(toDelete.Id)) {
										usedForQuickDimensioning = true;
									}
								}
							}
						}
						if (usedForQuickDimensioning) {
							if (MessageBox.Show(EuroplanRes.FloorSummaryPanel_VerteilerLoeschenText, EuroplanRes.FloorSummaryPanel_VerteilerLoeschenTitel, MessageBoxButtons.YesNo) == DialogResult.Yes) {
								foreach (Floor f in Project.Instance.Floors) {
									foreach (Room r in f.Rooms) {
										foreach (Product p in r.UsedProductsForQuickDimensioning) {
											if (p.QuickDimensioningConnectedDistributors.ContainsKey(toDelete.Id)) {
												p.QuickDimensioningConnectedDistributors.Remove(toDelete.Id);
											}
										}
									}
								}
								floor.Distributors.Remove(toDelete);
								if (ProjectStructureChanged != null) {
									ProjectStructureChanged(this);
								}
							}
						} else {
							List<PlannedProduct> connectedProducts = toDelete.PlannedConnectedProducts;
							foreach (PlannedProduct pp in connectedProducts) {
								pp.Product.PlannedConnection = null;
							}
							foreach (PlannedProduct pp in connectedProducts) {
								pp.ConfigureProduct(false);
							}
							floor.Distributors.Remove(toDelete);
							if (ProjectStructureChanged != null) {
								ProjectStructureChanged(this);
							}
						}
					}
				}
				form.Dispose();
			}
		}


		void gridRooms_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e) {
			if (e.Column.DisplayIndex >= 3 && e.Column.DisplayIndex <= 6) {
				this.gridRooms.InvalidateCell(3, -1);
				this.gridRooms.InvalidateCell(4, -1);
				this.gridRooms.InvalidateCell(5, -1);
				this.gridRooms.InvalidateCell(6, -1);
			}
			if (e.Column.DisplayIndex >= 7 && e.Column.DisplayIndex <= 9) {
				this.gridRooms.InvalidateCell(7, -1);
				this.gridRooms.InvalidateCell(8, -1);
				this.gridRooms.InvalidateCell(9, -1);
			}
		}

		void gridRooms_Paint(object sender, PaintEventArgs e) {
			Rectangle r1 = this.gridRooms.GetCellDisplayRectangle(4, -1, true); //get the column header cell
			Rectangle r2 = this.gridRooms.GetCellDisplayRectangle(5, -1, true); //get the column header cell
			Rectangle r3 = this.gridRooms.GetCellDisplayRectangle(6, -1, true); //get the column header cell
			Rectangle r4 = this.gridRooms.GetCellDisplayRectangle(7, -1, true); //get the column header cell

			r1.X += 1;
			r1.Y += 1;
			r1.Width = r1.Width + r2.Width + r3.Width + r4.Width - 4;
			r1.Height = r1.Height / 2 - 2;
			//e.Graphics.FillRectangle(new SolidBrush(this.gridRooms.ColumnHeadersDefaultCellStyle.BackColor), r1);
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			
			e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			//e.Graphics.DrawLine(new Pen(SystemColors.ControlDark), new Point(r1.X, r1.Y + 2), new Point(r1.X + r1.Width, r1.Y + 2));
			//e.Graphics.DrawLine(new Pen(SystemColors.ControlDark), new Point(r1.X, r1.Y + r1.Height - 7), new Point(r1.X + r1.Width, r1.Y + r1.Height - 7));
			e.Graphics.DrawString(EuroplanRes.FloorSummaryPanel_Heizbetrieb,
				this.gridRooms.ColumnHeadersDefaultCellStyle.Font,
				new SolidBrush(this.gridRooms.ColumnHeadersDefaultCellStyle.ForeColor),
				r1,
				format);
			//e.Graphics.DrawLine(Pens.Black, new Point(r1.X, r1.Y + r1.Height - 5), new Point(r1.X + r1.Width, r1.Y + r1.Height - 5));
			//e.Graphics.DrawLine(Pens.Black, new Point(r1.X, r1.Y), new Point(r1.X, r1.Y + (r1.Height * 2)));
			//e.Graphics.DrawLine(Pens.Black, new Point(r1.X + r1.Width, r1.Y), new Point(r1.X + r1.Width, r1.Y + (r1.Height * 2)));

			r1 = this.gridRooms.GetCellDisplayRectangle(8, -1, true); //get the column header cell
			r2 = this.gridRooms.GetCellDisplayRectangle(9, -1, true); //get the column header cell
			r3 = this.gridRooms.GetCellDisplayRectangle(10, -1, true); //get the column header cell

			r1.X += 1;
			r1.Y += 1;
			r1.Width = r1.Width + r2.Width + r3.Width - 4;
			r1.Height = r1.Height / 2 - 2;
			//e.Graphics.FillRectangle(new SolidBrush(this.gridRooms.ColumnHeadersDefaultCellStyle.BackColor), r1);
			format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			e.Graphics.DrawString(EuroplanRes.FloorSummaryPanel_Kuehlbetrieb,
				this.gridRooms.ColumnHeadersDefaultCellStyle.Font,
				new SolidBrush(this.gridRooms.ColumnHeadersDefaultCellStyle.ForeColor),
				r1,
				format);
			//e.Graphics.DrawLine(Pens.Black, new Point(r1.X, r1.Y + r1.Height - 5), new Point(r1.X + r1.Width, r1.Y + r1.Height - 5));
			//e.Graphics.DrawLine(Pens.Black, new Point(r1.X, r1.Y), new Point(r1.X, r1.Y + (r1.Height * 2)));
			//e.Graphics.DrawLine(Pens.Black, new Point(r1.X + r1.Width, r1.Y), new Point(r1.X + r1.Width, r1.Y + (r1.Height * 2)));

		}

		void gridRooms_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
			if (e.RowIndex == -1 && e.ColumnIndex > -1) {
				e.PaintBackground(e.CellBounds, false);

				Rectangle r2 = e.CellBounds;
				r2.Y += e.CellBounds.Height / 2;
				r2.Height = e.CellBounds.Height / 2;
				e.PaintContent(r2);
				e.Handled = true;
			}
		}

		private void btnWhatIsNext_Click(object sender, EventArgs e) {
			MessageBox.Show(EuroplanRes.FloorSummaryPanel_WieGehtsWeiterText, EuroplanRes.FloorSummaryPanel_WieGehtsWeiterTitel);
		}

		Nullable<float> oldArea = null;
		private void gridRooms_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {
			this.oldArea = null;
			if (e.ColumnIndex >= 0 && e.ColumnIndex < this.gridRooms.Columns.Count &&
					e.RowIndex >= 0 && e.RowIndex < this.gridRooms.Rows.Count) {
				if (this.gridRooms.Columns[e.ColumnIndex] == this.Area) {
					this.oldArea = (this.gridRooms.Rows[e.RowIndex].DataBoundItem as Room).Area;
				}
			}
		}

		private void chkAssignPlan_CheckedChanged(object sender, EventArgs e) {
			if (!updateControlOngoing) {
				if (chkAssignPlan.Checked) {
					cmbPlans.Enabled = true;

					planBindingSource.DataSource = Project.Instance.ImportedPlans;

					if (floor.AssociatedPlanId != null) {
						foreach (Plan plan in Project.Instance.ImportedPlans) {
							if (plan.Id == floor.AssociatedPlanId) {
								cmbPlans.SelectedItem = plan;
								break;
							}
						}
					}
				} else {
					bool roomCoordinatesAvailable = false;
					foreach (Room room in floor.Rooms) {
						if (room.RoomCoordinates.Count > 0) {
							roomCoordinatesAvailable = true;
							break;
						}
					}
					if (roomCoordinatesAvailable) {
						DialogResult result = MessageBox.Show(EuroplanRes.FloorSummaryPanel_RemovePlanText, EuroplanRes.FloorSummaryPanel_RemovePlanCaption, MessageBoxButtons.YesNo);
						if (result == DialogResult.No) {
							UpdateControl(false);
							return;
						} else {
							foreach (Room room in floor.Rooms) {
								if (room.RoomCoordinates.Count > 0) {
									room.RoomCoordinates.Clear();
									room.RoomUnusedAreaCoordinates.Clear();
									room.CeilingCoordinates.Clear();
									room.CeilingUnusedAreaCoordinates.Clear();
									foreach (PlannedProduct pp in room.PlannedProducts) {
										if (pp.Product.GraphicalMode.HasValue && pp.Product.GraphicalMode.Value) {
											pp.Product.GraphicalMode = false;
										}
									}
								}
								room.PlanSettingX = null;
								room.PlanSettingY = null;
								room.PlanSettingScale = null;
								room.PlanSettingAngle = null;
							}
						}
					}

					floor.AssociatedPlanId = null;
					cmbPlans.Enabled = false;
				}
				if (ProjectChanged != null) {
					ProjectChanged(this);
				}
			}
		}

		private void cmbPlans_SelectedValueChanged(object sender, EventArgs e) {
			if (!updateControlOngoing) {
				if (cmbPlans.SelectedItem != null) {
					bool planAlreadyUsed = false;
					foreach (Room room in floor.Rooms) {
						if (room.RoomCoordinates.Count > 0) {
							planAlreadyUsed = true;
							break;
						}
					}
					if (!planAlreadyUsed) {
						foreach (Distributor d in floor.Distributors) {
							if (d.GraphicalRepresentations.Count > 0) {
								planAlreadyUsed = true;
								break;
							}
						}
					}
					if (planAlreadyUsed) {
						DialogResult result = MessageBox.Show(EuroplanRes.FloorSummaryPanel_ChangePlanText, EuroplanRes.FloorSummaryPanel_ChangePlanCaption, MessageBoxButtons.YesNo);
						if (result == DialogResult.No) {
							UpdateControl(false);
							return;
						} else {
							foreach (Room room in floor.Rooms) {
								if (room.RoomCoordinates.Count > 0) {
									room.RoomCoordinates.Clear();
									room.RoomUnusedAreaCoordinates.Clear();
									room.CeilingCoordinates.Clear();
									room.CeilingUnusedAreaCoordinates.Clear();
									foreach (PlannedProduct pp in room.PlannedProducts) {
										if (pp.Product.GraphicalMode.HasValue && pp.Product.GraphicalMode.Value) {
											pp.Product.GraphicalMode = false;
										}
									}
								}
								room.PlanSettingX = null;
								room.PlanSettingY = null;
								room.PlanSettingScale = null;
								room.PlanSettingAngle = null;
							}
							foreach (Distributor d in floor.Distributors) {
								for (int i = 0; i < d.GraphicalRepresentations.Count; i++) {
									if (d.GraphicalRepresentations[i].floorId == floor.Id) {
										d.GraphicalRepresentations.RemoveAt(i);
										i--;
									}
								}
							}
						}
					}
					floor.AssociatedPlanId = (cmbPlans.SelectedItem as Plan).Id;
					if (ProjectChanged != null) {
						ProjectChanged(this);
					}
				}
			}
		}

	}
}
