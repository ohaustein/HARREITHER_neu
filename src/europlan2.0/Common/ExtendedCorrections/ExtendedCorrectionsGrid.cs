using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Europlan.Common {
	public partial class ExtendedCorrectionsGrid : UserControl {

		private EurovalProduct evProduct = null;
		private EcothermProduct ecProduct = null;

		private ExtendedCorrections sumRow = null;

		public event EventHandler CorrectionsEnabledChanged;
		public event EventHandler CorrectionsChanged;

		public ExtendedCorrectionsGrid() {
			InitializeComponent();

			this.SetLanguage();
			this.gridExtendedCorrections.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
			this.gridExtendedCorrections.CellPainting += new DataGridViewCellPaintingEventHandler(gridExtendedCorrections_CellPainting);
			this.gridExtendedCorrections.Paint += new PaintEventHandler(gridExtendedCorrections_Paint);
			this.gridExtendedCorrections.ColumnWidthChanged += new DataGridViewColumnEventHandler(gridExtendedCorrections_ColumnWidthChanged);
		}

		private void SetLanguage() {
			this.rbExtendedCorrections.Text = EuroplanRes.ExtendedCorrectionsGrid_Aktivieren; //"erweiterte Korrekturen aktivieren"
			this.rbStandardCorrections.Text = EuroplanRes.ExtendedCorrectionsGrid_Deaktivieren; //"nur Standardkorrekturen verwenden (keine erweiterten Korrekturen)"
			this.CircuitNr.HeaderText = EuroplanRes.ExtendedCorrectionsGrid_HeizkreisCol; //"Heiz-\nkreis\nNr."
			this.correctAreaDataGridViewCheckBoxColumn.HeaderText = EuroplanRes.ExtendedCorrectionsGrid_FlaecheAktivieren; //"Vor-\ngabe"
			this.areaValueDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ExtendedCorrectionsGrid_FlaecheWert; //"m²\n"
			this.areaPercentageDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ExtendedCorrectionsGrid_FlaecheProzent; //"%\n"
			this.correctRimDataGridViewCheckBoxColumn.HeaderText = EuroplanRes.ExtendedCorrectionsGrid_RandzoneAktivieren; //"Vor-\ngabe"
			this.rimLengthValueDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ExtendedCorrectionsGrid_RandzoneWert; //"m\n"
			this.rimPercentageDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ExtendedCorrectionsGrid_RandzoneProzent; //"%\n"
			this.rimCornersValueDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ExtendedCorrectionsGrid_RandzoneEcken; //"Anzahl\nEcken"
			this.correctConnectionsDataGridViewCheckBoxColumn.HeaderText = EuroplanRes.ExtendedCorrectionsGrid_AnbindungAktivieren; //"Vor-\ngabe"
			this.connectionsPercentageDataGridViewTextBoxColumn.HeaderText = EuroplanRes.ExtendedCorrectionsGrid_AnbindungProzent; //"Fläche\n%"
		}

		public Product Product {
			get {
				if (this.evProduct != null) {
					return this.evProduct;
				} else if (this.ecProduct != null) {
					return this.ecProduct;
				} else {
					return null;
				}
			}
			set {
				this.evProduct = value as EurovalProduct;
				this.ecProduct = value as EcothermProduct;
				if (this.evProduct != null) {
					this.sumRow = new ExtendedCorrections(this.evProduct);
				} else {
					this.sumRow = new ExtendedCorrections(this.ecProduct);
				}
				this.UpdateControl(true, true);
			}
		}

		private bool CorrectionsEnabled {
			get {
				if (this.evProduct != null) {
					return evProduct.PlannedCorrections;
				} else if (this.ecProduct != null) {
					return this.ecProduct.PlannedCorrections;
				} else {
					return false;
				}
			}
			set {
				if (this.evProduct != null) {
					this.evProduct.PlannedCorrections = value;
				} else if (this.ecProduct != null) {
					this.ecProduct.PlannedCorrections = value;
				}
			}
		}

		private List<ExtendedCorrections> CorrectionsList {
			get {
				if (this.evProduct != null) {
					if (this.evProduct.PlannedCorrections) {
						List<ExtendedCorrections> list = new List<ExtendedCorrections>(this.evProduct.PlannedCorrectionList);
						list.Add(this.sumRow);
						return list;
					}
				} else if (this.ecProduct != null) {
					if (this.ecProduct.PlannedCorrections) {
						List<ExtendedCorrections> list = new List<ExtendedCorrections>(this.ecProduct.PlannedCorrectionList);
						list.Add(this.sumRow);
						return list;
					}
				}
				return new List<ExtendedCorrections>();
			}
		}

		private int ignoreRadio = 0;
		private int ignoreList = 0;

		public void UpdateControl(bool updateRadio, bool updateList) {
			ignoreRadio++;
			ignoreList++;

			if (this.evProduct != null) {
				this.areaValueDataGridViewTextBoxColumn.MaxValue = (decimal)this.evProduct.PlannedFloorArea;
				this.rimLengthValueDataGridViewTextBoxColumn.MaxValue = (decimal)this.evProduct.PlannedRimLength;

				this.correctAreaDataGridViewCheckBoxColumn.ReadOnly = this.evProduct.PlannedFloorArea == 0;
				this.correctAreaDataGridViewCheckBoxColumn.DefaultCellStyle.BackColor = this.correctAreaDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
				this.areaValueDataGridViewTextBoxColumn.ReadOnly = this.correctAreaDataGridViewCheckBoxColumn.ReadOnly;
				this.areaValueDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = this.correctAreaDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
				this.areaPercentageDataGridViewTextBoxColumn.ReadOnly = this.correctAreaDataGridViewCheckBoxColumn.ReadOnly;
				this.areaPercentageDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = this.correctAreaDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;

				this.correctRimDataGridViewCheckBoxColumn.ReadOnly = this.evProduct.PlannedRimLength == 0;
				this.correctRimDataGridViewCheckBoxColumn.DefaultCellStyle.BackColor = this.correctRimDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
				this.rimLengthValueDataGridViewTextBoxColumn.ReadOnly = this.correctRimDataGridViewCheckBoxColumn.ReadOnly;
				this.rimLengthValueDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = this.correctRimDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
				this.rimPercentageDataGridViewTextBoxColumn.ReadOnly = this.correctRimDataGridViewCheckBoxColumn.ReadOnly;
				this.rimPercentageDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = this.correctRimDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
				this.rimCornersValueDataGridViewTextBoxColumn.ReadOnly = this.correctRimDataGridViewCheckBoxColumn.ReadOnly;
				this.rimCornersValueDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = this.correctRimDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;

				this.correctConnectionsDataGridViewCheckBoxColumn.ReadOnly = this.evProduct.PlannedRemoveArea == 0;
				this.correctConnectionsDataGridViewCheckBoxColumn.DefaultCellStyle.BackColor = this.correctConnectionsDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
				this.connectionsPercentageDataGridViewTextBoxColumn.ReadOnly = this.correctConnectionsDataGridViewCheckBoxColumn.ReadOnly;
				this.connectionsPercentageDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = this.correctConnectionsDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
			} else if (this.ecProduct != null) {
				this.areaValueDataGridViewTextBoxColumn.MaxValue = (decimal)this.ecProduct.PlannedFloorArea;
				this.rimLengthValueDataGridViewTextBoxColumn.MaxValue = (decimal)this.ecProduct.PlannedRimLength;

				this.correctAreaDataGridViewCheckBoxColumn.ReadOnly = this.ecProduct.PlannedFloorArea == 0;
				this.correctAreaDataGridViewCheckBoxColumn.DefaultCellStyle.BackColor = this.correctAreaDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
				this.areaValueDataGridViewTextBoxColumn.ReadOnly = this.correctAreaDataGridViewCheckBoxColumn.ReadOnly;
				this.areaValueDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = this.correctAreaDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
				this.areaPercentageDataGridViewTextBoxColumn.ReadOnly = this.correctAreaDataGridViewCheckBoxColumn.ReadOnly;
				this.areaPercentageDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = this.correctAreaDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;

				this.correctRimDataGridViewCheckBoxColumn.ReadOnly = this.ecProduct.PlannedRimLength == 0;
				this.correctRimDataGridViewCheckBoxColumn.DefaultCellStyle.BackColor = this.correctRimDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
				this.rimLengthValueDataGridViewTextBoxColumn.ReadOnly = this.correctRimDataGridViewCheckBoxColumn.ReadOnly;
				this.rimLengthValueDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = this.correctRimDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
				this.rimPercentageDataGridViewTextBoxColumn.ReadOnly = this.correctRimDataGridViewCheckBoxColumn.ReadOnly;
				this.rimPercentageDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = this.correctRimDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
				this.rimCornersValueDataGridViewTextBoxColumn.ReadOnly = this.correctRimDataGridViewCheckBoxColumn.ReadOnly;
				this.rimCornersValueDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = this.correctRimDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;

				this.correctConnectionsDataGridViewCheckBoxColumn.ReadOnly = this.ecProduct.PlannedRemoveArea == 0;
				this.correctConnectionsDataGridViewCheckBoxColumn.DefaultCellStyle.BackColor = this.correctConnectionsDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
				this.connectionsPercentageDataGridViewTextBoxColumn.ReadOnly = this.correctConnectionsDataGridViewCheckBoxColumn.ReadOnly;
				this.connectionsPercentageDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = this.correctConnectionsDataGridViewCheckBoxColumn.ReadOnly ? SystemColors.Control : this.gridExtendedCorrections.DefaultCellStyle.BackColor;
			}

			if (updateRadio) {
				if (this.CorrectionsEnabled) {
					this.rbExtendedCorrections.Checked = true;
				} else {
					this.rbStandardCorrections.Checked = true;
				}
			}

			if (updateList) {
				this.extendedCorrectionsBindingSource.DataSource = this.CorrectionsList;
				this.extendedCorrectionsBindingSource.ResetBindings(false);
				this.gridExtendedCorrections.Enabled = this.CorrectionsEnabled;
			}

			ignoreRadio--;
			ignoreList--;
		}

		private void rbExtendedCorrections_CheckedChanged(object sender, EventArgs e) {
			if (ignoreRadio == 0) {
				this.CorrectionsEnabled = rbExtendedCorrections.Checked;
				this.UpdateControl(false, true);
				if (this.CorrectionsEnabledChanged != null) {
					this.CorrectionsEnabledChanged(this, EventArgs.Empty);
				}
			}
		}

		#region Grid Headers
		private void gridExtendedCorrections_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
			if (e.RowIndex == -1 && e.ColumnIndex > -1) {
				e.PaintBackground(e.CellBounds, false);

				Rectangle r2 = e.CellBounds;
				r2.Y += e.CellBounds.Height / 2;
				r2.Height = e.CellBounds.Height / 2;
				e.PaintContent(r2);
				e.Handled = true;
			}
		}

		private void gridExtendedCorrections_Paint(object sender, PaintEventArgs e) {
			Rectangle r1 = this.gridExtendedCorrections.GetCellDisplayRectangle(this.correctAreaDataGridViewCheckBoxColumn.Index, -1, true); //get the column header cell
			Rectangle r2 = this.gridExtendedCorrections.GetCellDisplayRectangle(this.areaValueDataGridViewTextBoxColumn.Index, -1, true); //get the column header cell
			Rectangle r3 = this.gridExtendedCorrections.GetCellDisplayRectangle(this.areaPercentageDataGridViewTextBoxColumn.Index, -1, true); //get the column header cell

			r1.X += 1;
			r1.Y += 1;
			r1.Width = r1.Width + r2.Width + r3.Width - 4;
			r1.Height = 26;
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;

			e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			e.Graphics.DrawString(EuroplanRes.ExtendedCorrectionsGrid_Flaeche /*"Anteil Gesamtfläche"*/,
				this.gridExtendedCorrections.ColumnHeadersDefaultCellStyle.Font,
				new SolidBrush(this.gridExtendedCorrections.ColumnHeadersDefaultCellStyle.ForeColor),
				r1,
				format);

			r1 = this.gridExtendedCorrections.GetCellDisplayRectangle(this.correctRimDataGridViewCheckBoxColumn.Index, -1, true); //get the column header cell
			r2 = this.gridExtendedCorrections.GetCellDisplayRectangle(this.rimLengthValueDataGridViewTextBoxColumn.Index, -1, true); //get the column header cell
			r3 = this.gridExtendedCorrections.GetCellDisplayRectangle(this.rimPercentageDataGridViewTextBoxColumn.Index, -1, true); //get the column header cell
			Rectangle r4 = this.gridExtendedCorrections.GetCellDisplayRectangle(this.rimCornersValueDataGridViewTextBoxColumn.Index, -1, true); //get the column header cell

			r1.X += 1;
			r1.Y += 1;
			r1.Width = r1.Width + r2.Width + r3.Width + r4.Width - 4;
			r1.Height = 26;
			format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			e.Graphics.DrawString(EuroplanRes.ExtendedCorrectionsGrid_Randzone /*"Anteil Randzone"*/,
				this.gridExtendedCorrections.ColumnHeadersDefaultCellStyle.Font,
				new SolidBrush(this.gridExtendedCorrections.ColumnHeadersDefaultCellStyle.ForeColor),
				r1,
				format);

			r1 = this.gridExtendedCorrections.GetCellDisplayRectangle(this.correctConnectionsDataGridViewCheckBoxColumn.Index, -1, true); //get the column header cell
			r2 = this.gridExtendedCorrections.GetCellDisplayRectangle(this.connectionsPercentageDataGridViewTextBoxColumn.Index, -1, true); //get the column header cell

			r1.X += 1;
			r1.Y += 1;
			r1.Width = r1.Width + r2.Width - 4;
			r1.Height = 26;
			format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;
			e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark), new Rectangle(r1.X + 4, r1.Y + 4, r1.Width - 8, r1.Height - 9));
			e.Graphics.DrawString(EuroplanRes.ExtendedCorrectionsGrid_Anbindung /*""Anbindeleitungen"*/,
				this.gridExtendedCorrections.ColumnHeadersDefaultCellStyle.Font,
				new SolidBrush(this.gridExtendedCorrections.ColumnHeadersDefaultCellStyle.ForeColor),
				r1,
				format);
		}

		private void gridExtendedCorrections_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e) {
			if (e.Column.Index == this.correctAreaDataGridViewCheckBoxColumn.Index ||
				e.Column.Index == this.areaValueDataGridViewTextBoxColumn.Index ||
				e.Column.Index == this.areaPercentageDataGridViewTextBoxColumn.Index) {
				this.gridExtendedCorrections.InvalidateCell(this.correctAreaDataGridViewCheckBoxColumn.Index, -1);
				this.gridExtendedCorrections.InvalidateCell(this.areaValueDataGridViewTextBoxColumn.Index, -1);
				this.gridExtendedCorrections.InvalidateCell(this.areaValueDataGridViewTextBoxColumn.Index, -1);
			}
			if (e.Column.Index == this.correctRimDataGridViewCheckBoxColumn.Index ||
				e.Column.Index == this.rimLengthValueDataGridViewTextBoxColumn.Index ||
				e.Column.Index == this.rimPercentageDataGridViewTextBoxColumn.Index ||
				e.Column.Index == this.rimCornersValueDataGridViewTextBoxColumn.Index) {
				this.gridExtendedCorrections.InvalidateCell(this.correctRimDataGridViewCheckBoxColumn.Index, -1);
				this.gridExtendedCorrections.InvalidateCell(this.rimLengthValueDataGridViewTextBoxColumn.Index, -1);
				this.gridExtendedCorrections.InvalidateCell(this.rimPercentageDataGridViewTextBoxColumn.Index, -1);
				this.gridExtendedCorrections.InvalidateCell(this.rimCornersValueDataGridViewTextBoxColumn.Index, -1);
			}
			if (e.Column.Index == this.correctConnectionsDataGridViewCheckBoxColumn.Index ||
				e.Column.Index == this.connectionsPercentageDataGridViewTextBoxColumn.Index) {
				this.gridExtendedCorrections.InvalidateCell(this.correctConnectionsDataGridViewCheckBoxColumn.Index, -1);
				this.gridExtendedCorrections.InvalidateCell(this.connectionsPercentageDataGridViewTextBoxColumn.Index, -1);
			}
		}
		#endregion Grid Headers

		private void gridExtendedCorrections_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
			for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++) {
				DataGridViewRow row = this.gridExtendedCorrections.Rows[i];
				ExtendedCorrections ec = row.DataBoundItem as ExtendedCorrections;

				if (ec.Sum) {
					row.ReadOnly = true;
					foreach (DataGridViewColumn col in this.gridExtendedCorrections.Columns) {
						row.Cells[col.Index].Style.BackColor = SystemColors.Control;
					}
				} else {
					foreach (DataGridViewColumn col in this.gridExtendedCorrections.Columns) {
						row.Cells[col.Index].Style.BackColor = row.DefaultCellStyle.BackColor;
					}

					DataGridViewCell cell = row.Cells[this.areaValueDataGridViewTextBoxColumn.Index];
					cell.Style.BackColor = (ec.CorrectArea ? row.DefaultCellStyle.BackColor : SystemColors.Control);
					cell.ReadOnly = !ec.CorrectArea;

					cell = row.Cells[this.areaPercentageDataGridViewTextBoxColumn.Index];
					cell.Style.BackColor = (ec.CorrectArea ? row.DefaultCellStyle.BackColor : SystemColors.Control);
					cell.ReadOnly = !ec.CorrectArea;

					cell = row.Cells[this.rimLengthValueDataGridViewTextBoxColumn.Index];
					cell.Style.BackColor = (ec.CorrectRim ? row.DefaultCellStyle.BackColor : SystemColors.Control);
					cell.ReadOnly = !ec.CorrectRim;

					cell = row.Cells[this.rimPercentageDataGridViewTextBoxColumn.Index];
					cell.Style.BackColor = (ec.CorrectRim ? row.DefaultCellStyle.BackColor : SystemColors.Control);
					cell.ReadOnly = !ec.CorrectRim;

					cell = row.Cells[this.rimCornersValueDataGridViewTextBoxColumn.Index];
					cell.Style.BackColor = (ec.CorrectRim ? row.DefaultCellStyle.BackColor : SystemColors.Control);
					cell.ReadOnly = !ec.CorrectRim;

					cell = row.Cells[this.connectionsPercentageDataGridViewTextBoxColumn.Index];
					cell.Style.BackColor = (ec.CorrectConnections ? row.DefaultCellStyle.BackColor : SystemColors.Control);
					cell.ReadOnly = !ec.CorrectConnections;
				}
			}
		}

		private void gridExtendedCorrections_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (e.RowIndex >= 0) {
				DataGridViewRow row = this.gridExtendedCorrections.Rows[e.RowIndex];
				ExtendedCorrections ec = row.DataBoundItem as ExtendedCorrections;
				DataGridViewCell cell;
				if (e.ColumnIndex == this.correctAreaDataGridViewCheckBoxColumn.Index) {
					cell = row.Cells[this.areaValueDataGridViewTextBoxColumn.Index];
					cell.Style.BackColor = (ec.CorrectArea ? row.DefaultCellStyle.BackColor : SystemColors.Control);
					cell.ReadOnly = !ec.CorrectArea;

					cell = row.Cells[this.areaPercentageDataGridViewTextBoxColumn.Index];
					cell.Style.BackColor = (ec.CorrectArea ? row.DefaultCellStyle.BackColor : SystemColors.Control);
					cell.ReadOnly = !ec.CorrectArea;
				} else if (e.ColumnIndex == this.correctRimDataGridViewCheckBoxColumn.Index) {
					cell = row.Cells[this.rimLengthValueDataGridViewTextBoxColumn.Index];
					cell.Style.BackColor = (ec.CorrectRim ? row.DefaultCellStyle.BackColor : SystemColors.Control);
					cell.ReadOnly = !ec.CorrectRim;

					cell = row.Cells[this.rimPercentageDataGridViewTextBoxColumn.Index];
					cell.Style.BackColor = (ec.CorrectRim ? row.DefaultCellStyle.BackColor : SystemColors.Control);
					cell.ReadOnly = !ec.CorrectRim;

					cell = row.Cells[this.rimCornersValueDataGridViewTextBoxColumn.Index];
					cell.Style.BackColor = (ec.CorrectRim ? row.DefaultCellStyle.BackColor : SystemColors.Control);
					cell.ReadOnly = !ec.CorrectRim;
				} else if (e.ColumnIndex == this.correctConnectionsDataGridViewCheckBoxColumn.Index) {
					cell = row.Cells[this.connectionsPercentageDataGridViewTextBoxColumn.Index];
					cell.Style.BackColor = (ec.CorrectConnections ? row.DefaultCellStyle.BackColor : SystemColors.Control);
					cell.ReadOnly = !ec.CorrectConnections;
				}
				if (this.CorrectionsChanged != null) {
					this.CorrectionsChanged(this, EventArgs.Empty);
				}
			}
			foreach (DataGridViewRow row in this.gridExtendedCorrections.Rows) {
				this.gridExtendedCorrections.InvalidateRow(row.Index);
			}
		}

		private void gridExtendedCorrections_CurrentCellDirtyStateChanged(object sender, EventArgs e) {
			if (this.gridExtendedCorrections.CurrentCell.ColumnIndex == this.correctAreaDataGridViewCheckBoxColumn.Index ||
				this.gridExtendedCorrections.CurrentCell.ColumnIndex == this.correctRimDataGridViewCheckBoxColumn.Index ||
				this.gridExtendedCorrections.CurrentCell.ColumnIndex == this.correctConnectionsDataGridViewCheckBoxColumn.Index) {
				this.gridExtendedCorrections.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		}

		private void gridExtendedCorrections_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e) {
			if (e.RowIndex >= 0 && e.RowIndex < this.gridExtendedCorrections.Rows.Count && this.gridExtendedCorrections.Rows[e.RowIndex].DataBoundItem is ExtendedCorrections) {
				ExtendedCorrections ec = this.gridExtendedCorrections.Rows[e.RowIndex].DataBoundItem as ExtendedCorrections;
				if (ec.Sum &&
					e.ColumnIndex != this.areaValueDataGridViewTextBoxColumn.Index &&
					e.ColumnIndex != this.areaPercentageDataGridViewTextBoxColumn.Index &&
					e.ColumnIndex != this.rimLengthValueDataGridViewTextBoxColumn.Index &&
					e.ColumnIndex != this.rimPercentageDataGridViewTextBoxColumn.Index &&
					e.ColumnIndex != this.rimCornersValueDataGridViewTextBoxColumn.Index &&
					e.ColumnIndex != this.connectionsPercentageDataGridViewTextBoxColumn.Index) {

					e.Paint(e.ClipBounds, e.PaintParts & (~DataGridViewPaintParts.ContentForeground));
					e.Handled = true;
				} else if (this.correctAreaDataGridViewCheckBoxColumn.ReadOnly &&
					(e.ColumnIndex == this.correctAreaDataGridViewCheckBoxColumn.Index ||
					e.ColumnIndex == this.areaValueDataGridViewTextBoxColumn.Index ||
					e.ColumnIndex == this.areaPercentageDataGridViewTextBoxColumn.Index)) {

					e.Paint(e.ClipBounds, e.PaintParts & (~DataGridViewPaintParts.ContentForeground));
					e.Handled = true;
				} else if (this.correctRimDataGridViewCheckBoxColumn.ReadOnly && 
					(e.ColumnIndex == this.correctRimDataGridViewCheckBoxColumn.Index ||
					e.ColumnIndex == this.rimLengthValueDataGridViewTextBoxColumn.Index ||
					e.ColumnIndex == this.rimPercentageDataGridViewTextBoxColumn.Index ||
					e.ColumnIndex == this.rimCornersValueDataGridViewTextBoxColumn.Index)) {

					e.Paint(e.ClipBounds, e.PaintParts & (~DataGridViewPaintParts.ContentForeground));
					e.Handled = true;
				} else if (this.correctConnectionsDataGridViewCheckBoxColumn.ReadOnly &&
					(e.ColumnIndex == this.correctConnectionsDataGridViewCheckBoxColumn.Index ||
					e.ColumnIndex == this.connectionsPercentageDataGridViewTextBoxColumn.Index)) {

					e.Paint(e.ClipBounds, e.PaintParts & (~DataGridViewPaintParts.ContentForeground));
					e.Handled = true;
				}
			}
		}

		public bool AllowLeave() {
			if (this.sumRow != null && this.CorrectionsEnabled) {
				if (Math.Round(this.sumRow.AreaPercentage, 1) != 100.0) {
					MessageBox.Show(EuroplanRes.ExtendedCorrectionsGrid_FehlerFlaeche /*"Die Summe der Anteile an der Gesamtfläche muss 100% der Gesamtfläche ergeben"*/, 
						EuroplanRes.ExtendedCorrectionsGrid_Fehler /*"Bitte korrigieren Sie die Eingabe"*/,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				if (Math.Round(this.sumRow.RimPercentage, 1) != 100.0) {
					MessageBox.Show(EuroplanRes.ExtendedCorrectionsGrid_FehlerRandzone /*"Die Summe der Anteile an der Randzone muss 100% der Gesamtfläche ergeben"*/,
						EuroplanRes.ExtendedCorrectionsGrid_Fehler /*"Bitte korrigieren Sie die Eingabe"*/,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				if ((this.evProduct != null && this.sumRow.RimCornersValue != this.evProduct.PlannedRimCorners) ||
					(this.ecProduct != null && this.sumRow.RimCornersValue != this.ecProduct.PlannedRimCorners)) {
					MessageBox.Show(EuroplanRes.ExtendedCorrectionsGrid_FehlerEcken /*"Die Summe der Ecken der Randzone an der Gesamtfläche muss 100% die gesamte Anzahl an vorgegebenen Ecken ergeben"*/,
						EuroplanRes.ExtendedCorrectionsGrid_Fehler /*"Bitte korrigieren Sie die Eingabe"*/,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				if (Math.Round(this.sumRow.ConnectionsPercentage, 1) != 100.0) {
					MessageBox.Show(EuroplanRes.ExtendedCorrectionsGrid_FehlerAnbindung /*"Die Summe der Anteile an der Fläche der Anbindeleitung muss 100% der gesamten Fläche der Anbindeleitung ergeben"*/,
						EuroplanRes.ExtendedCorrectionsGrid_Fehler /*"Bitte korrigieren Sie die Eingabe"*/,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
			}
			return true;
		}
	}
}
