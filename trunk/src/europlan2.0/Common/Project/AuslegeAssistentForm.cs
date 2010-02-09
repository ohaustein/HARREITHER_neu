using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;

namespace Europlan.Common {
	public partial class AuslegeAssistentForm : Form {

		private bool resizing = false;
		private bool checkStateUpdating = false;
		private TreeNode rootNode = null;
		private Licensing.License license = Licensing.LicenseManager.Instance.License;

		public AuslegeAssistentForm() {
			InitializeComponent();

			
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEuroval)) {
				chkEuroval.Text = new EurovalProduct().FullName;
			} else {
				chkEuroval.Checked = false;
				chkEuroval.Enabled = false;
			}
			if (license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEcotherm)) {
				chkEcotherm.Text = new EcothermProduct().FullName;
			} else {
				chkEcotherm.Checked = false;
				chkEcotherm.Enabled = false;
			}
			
			ConfigureTree();
		}

		private void ConfigureTree() {
			treeProducts.Nodes.Clear();

			rootNode = new TreeNode("Projekt");
			treeProducts.Nodes.Add(rootNode);

			Random r = new Random(DateTime.Now.Millisecond);

			foreach (Floor f in Project.Instance.Floors) {
				foreach (Distributor d in f.Distributors) {
					TreeNode distributorNode = new TreeNode(d.Id + ": " + d.Name);
					rootNode.Nodes.Add(distributorNode);
					foreach (PlannedProduct pp in d.PlannedConnectedProducts) {
						if ((pp.Product is EurovalProduct && license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEuroval) || 
							(pp.Product is EcothermProduct&& license.IsModuleEnabled(Licensing.AbstractLicensedModule.ProdEcotherm)))) {
							AuslegeNode node = new AuslegeNode(pp.InternalName + " in " + pp.Product.AssociatedRoom.Id + " (" + pp.Product.AssociatedRoom.Name + ")");
							distributorNode.Nodes.Add(node);

							// TODO - calculation...
							int[] azValues = new int[21];
							int[] rzValues = new int[21];
							for (int i = 0; i < azValues.Length; i++ ) {
								azValues[i] = r.Next(0, 7);
								rzValues[i] = r.Next(0, 3);
							}
							node.PlannedProduct = pp;
							node.AzValues = azValues;
							node.RzValues = rzValues;
						}
					}
				}
			}
			rootNode.Checked = true;
			treeProducts.ExpandAll();
		}


		private void graphicsPanel_Paint(object sender, PaintEventArgs e) {
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			DrawChart(e.Graphics, e.ClipRectangle);
			DrawProductLines(e.Graphics, e.ClipRectangle);
		}

		private void DrawChart(Graphics g, Rectangle rectangle) {
			if (!resizing) {
				Pen dashPen = new Pen(Brushes.Gray);
				dashPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
				int temperature;
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Center;

				//Randzone
				int x0 = rectangle.X + 5;
				int y0Rz = rectangle.Y + 5;
				int leftOffset = 45;
				int rightOffset = 45;
				int topOffset = 30;
				int bottomOffset = 35;
				int rzGridHeight = (rectangle.Height / 3) - topOffset - bottomOffset;
				int gridWidth = rectangle.Width - leftOffset - rightOffset;
				g.DrawString("Randzone", graphicsPanel.Font, Brushes.Black, x0, y0Rz);

				int count = 2;
				int layDistance = 5;
				for (int i = 0; i <= count; i++) {
					g.DrawLine(Pens.Black, x0 + leftOffset - 2, y0Rz + topOffset + (i * rzGridHeight / count), x0 + leftOffset + gridWidth + 2, y0Rz + topOffset + +(i * rzGridHeight / count));
					g.DrawString("EV" + layDistance.ToString(), graphicsPanel.Font, Brushes.Black, x0 + 5, y0Rz + topOffset + (i * rzGridHeight / count) - graphicsPanel.Font.SizeInPoints / 2);
					layDistance += 5;
				}
				// 30°C
				g.DrawLine(Pens.Black, x0 + leftOffset, y0Rz + topOffset - 2, x0 + leftOffset, y0Rz + topOffset + rzGridHeight + 2);
				// 50°C
				g.DrawLine(Pens.Black, x0 + leftOffset + gridWidth, y0Rz + topOffset - 2, x0 + leftOffset + gridWidth, y0Rz + topOffset + rzGridHeight + 2);

				g.DrawString("Tv\n°C", graphicsPanel.Font, Brushes.Black, x0 + leftOffset + gridWidth + 10, y0Rz + topOffset + rzGridHeight - 10);

				temperature = 30;
				for (int i = 0; i <= 20; i++) {
					if (i % 2 == 0) {
						g.DrawString(temperature.ToString(), graphicsPanel.Font, Brushes.Black, x0 + leftOffset + (i * gridWidth / 20), y0Rz + topOffset + rzGridHeight + 10, stringFormat);
					}
					if (i > 0 && i < 20) {
						if (i % 2 == 0) {
							g.DrawLine(dashPen, x0 + leftOffset + (i * gridWidth / 20), y0Rz + topOffset - 2, x0 + leftOffset + (i * gridWidth / 20), y0Rz + topOffset + rzGridHeight + 2);
						} else {
							g.DrawLine(Pens.Black, x0 + leftOffset + (i * gridWidth / 20), y0Rz + topOffset + rzGridHeight - 2, x0 + leftOffset + (i * gridWidth / 20), y0Rz + topOffset + rzGridHeight + 2);
						}
					}
					temperature++;
				}


				//Aufenthaltszone
				int y0Az = y0Rz + 5 + rectangle.Height / 3;
				int azGridHeight = (rectangle.Height / 3) * 2 - topOffset - bottomOffset;
				g.DrawString("Aufenthaltszone", graphicsPanel.Font, Brushes.Black, x0, y0Az);

				count = 6;
				layDistance = 5;
				for (int i = 0; i <= count; i++) {
					g.DrawLine(Pens.Black, x0 + leftOffset - 2, y0Az + topOffset + (i * azGridHeight / count), x0 + leftOffset + gridWidth + 2, y0Az + topOffset + +(i * azGridHeight / count));
					g.DrawString("EV" + layDistance.ToString(), graphicsPanel.Font, Brushes.Black, x0 + 5, y0Az + topOffset + (i * azGridHeight / count) - graphicsPanel.Font.SizeInPoints / 2);
					layDistance += 5;
				}
				// 30°C
				g.DrawLine(Pens.Black, x0 + leftOffset, y0Az + topOffset - 2, x0 + leftOffset, y0Az + topOffset + azGridHeight + 2);
				// 50°C
				g.DrawLine(Pens.Black, x0 + leftOffset + gridWidth, y0Az + topOffset - 2, x0 + leftOffset + gridWidth, y0Az + topOffset + azGridHeight + 2);

				g.DrawString("Tv\n°C", graphicsPanel.Font, Brushes.Black, x0 + leftOffset + gridWidth + 10, y0Az + topOffset + azGridHeight - 10);

				temperature = 30;
				for (int i = 0; i <= 20; i++) {
					if (i % 2 == 0) {
						g.DrawString(temperature.ToString(), graphicsPanel.Font, Brushes.Black, x0 + leftOffset + (i * gridWidth / 20), y0Az + topOffset + azGridHeight + 10, stringFormat);
					}
					if (i > 0 && i < 20) {
						if (i % 2 == 0) {
							g.DrawLine(dashPen, x0 + leftOffset + (i * gridWidth / 20), y0Az + topOffset - 2, x0 + leftOffset + (i * gridWidth / 20), y0Az + topOffset + azGridHeight + 2);
						} else {
							g.DrawLine(Pens.Black, x0 + leftOffset + (i * gridWidth / 20), y0Az + topOffset + azGridHeight - 2, x0 + leftOffset + (i * gridWidth / 20), y0Az + topOffset + azGridHeight + 2);
						}
					}
					temperature++;
				}
			}
		}


		private void DrawProductLines(Graphics g, Rectangle rectangle) {
			if (!resizing) {
				Pen smallPen = new Pen(Brushes.Red);
				Pen boldPen = new Pen(Brushes.Red);
				boldPen.Width = (float)2.5;

				int x0 = rectangle.X + 5;
				int y0Rz = rectangle.Y + 5;
				int leftOffset = 45;
				int rightOffset = 45;
				int topOffset = 30;
				int bottomOffset = 35;
				int rzGridHeight = (rectangle.Height / 3) - topOffset - bottomOffset;
				int gridWidth = rectangle.Width - leftOffset - rightOffset;

				float radius = (float)2.5;

				foreach (TreeNode tn in rootNode.Nodes) {
					Pen defaultPen;
					bool distributorSelected = false;
					if (treeProducts.SelectedNode == tn) {
						defaultPen = boldPen;
						distributorSelected = true;
					} else {
						defaultPen = smallPen;
					}
					foreach (AuslegeNode an in tn.Nodes) {
						Pen pen;
						if ((an.PlannedProduct.Product is EurovalProduct && chkEuroval.Checked) ||
							(an.PlannedProduct.Product is EcothermProduct && chkEcotherm.Checked)) {
							if (an.Checked || distributorSelected || treeProducts.SelectedNode == an) {
								if (treeProducts.SelectedNode == an) {
									pen = boldPen;
								} else {
									pen = defaultPen;
								}

								// Randzone
								if (an.RzValues != null) {
									PointF prevPoint = PointF.Empty;
									for (int i = 0; i < an.RzValues.Length; i++) {
										PointF p = new PointF(x0 + leftOffset + (i * gridWidth / 20), y0Rz + topOffset + rzGridHeight / 2 * an.RzValues[i]);
										// draw circle
										g.DrawEllipse(pen, p.X - radius, p.Y - radius, 2 * radius, 2 * radius);
										// draw line
										if (!prevPoint.IsEmpty) {
											g.DrawLine(pen, prevPoint, p);
										}
										prevPoint = p;
									}
								}

								// Aufenthaltszone
								int y0Az = y0Rz + 5 + rectangle.Height / 3;
								int azGridHeight = (rectangle.Height / 3) * 2 - topOffset - bottomOffset;

								if (an.AzValues != null) {
									PointF prevPoint = PointF.Empty;
									for (int i = 0; i < an.AzValues.Length; i++) {
										PointF p = new PointF(x0 + leftOffset + (i * gridWidth / 20), y0Az + topOffset + azGridHeight / 6 * an.AzValues[i]);
										// draw circle
										g.DrawEllipse(pen, p.X - radius, p.Y - radius, 2 * radius, 2 * radius);
										// draw line
										if (!prevPoint.IsEmpty) {
											g.DrawLine(pen, prevPoint, p);
										}
										prevPoint = p;
									}
								}
							}
						}
					}
				}
			}
		}

		private void AuslegeAssistentForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["AuslegeAssistentForm"];
			this.Location = settings.GetPoint("Location", this.Location);
			this.Size = settings.GetSize("Size", this.Size);
			this.splitContainer1.SplitterDistance = settings.GetSetting("SplitterDistance", this.splitContainer1.SplitterDistance);
		}

		private void AuslegeAssistentForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["AuslegeAssistentForm"];
			settings.StorePoint("Location", this.Location);
			settings.StoreSize("Size", this.Size);
			settings.StoreSetting("SplitterDistance", this.splitContainer1.SplitterDistance);
			SettingsFile.Update();
		}

		private void AuslegeAssistentForm_ResizeBegin(object sender, EventArgs e) {
			resizing = true;
		}

		private void AuslegeAssistentForm_ResizeEnd(object sender, EventArgs e) {
			resizing = false;
			graphicsPanel.Invalidate();
		}

		private void treeProducts_AfterCheck(object sender, TreeViewEventArgs e) {
			if (e.Node is AuslegeNode) {
				if (!checkStateUpdating) {
					if (!e.Node.Checked) {
						checkStateUpdating = true;
						e.Node.Parent.Checked = false;
						e.Node.Parent.Parent.Checked = false;
						checkStateUpdating = false;
					}
					graphicsPanel.Invalidate();
				}
			} else {
				if (!checkStateUpdating) {
					checkStateUpdating = true;
					if (!e.Node.Checked && e.Node.Parent != null) {
						e.Node.Parent.Checked = false;
					}
					foreach (TreeNode node in e.Node.Nodes) {
						node.Checked = e.Node.Checked;
						if (node.Nodes.Count > 0) {
							foreach (TreeNode n in node.Nodes) {
								n.Checked = e.Node.Checked;
							}
						}
					}
					checkStateUpdating = false;
					graphicsPanel.Invalidate();
				}
			}
		}

		private void treeProducts_AfterSelect(object sender, TreeViewEventArgs e) {
			graphicsPanel.Invalidate();
		}

		private void chk_CheckStateChanged(object sender, EventArgs e) {
			graphicsPanel.Invalidate();
		}



	}
}