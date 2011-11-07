using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Star.SettingsXpress;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using WW.Math;
using WW.Cad.Model;
using WW.Cad.IO;
using WW.Cad.Model.Tables;
using WW.Cad.Base;
using WW.Cad.Model.Entities;
using WW.Math.Geometry;

namespace Europlan.Common {

	public partial class ExportPlanForm : Form {

		public class ExportOptionTypeEnumConverter : System.ComponentModel.TypeConverter {
			private static readonly string dh_fbh = EuroplanRes.ExportOption_DH_FBH;
			private static readonly string fbh = EuroplanRes.ExportOption_FBH;
			private static readonly string dh = EuroplanRes.ExportOption_DH;
			
			private Dictionary<string, ExportOptionType> mappingFromString = new Dictionary<string, ExportOptionType>();
			private Dictionary<ExportOptionType, string> mappingToString = new Dictionary<ExportOptionType, string>();

			public ExportOptionTypeEnumConverter() {
				mappingFromString.Add(dh_fbh, ExportOptionType.DH_FBH);
				mappingFromString.Add(fbh, ExportOptionType.FBH);
				mappingFromString.Add(dh, ExportOptionType.DH);
				mappingToString.Add(ExportOptionType.DH_FBH, dh_fbh);
				mappingToString.Add(ExportOptionType.FBH, fbh);
				mappingToString.Add(ExportOptionType.DH, dh);
			}

			public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType) {
				return sourceType == typeof(string);
			}

			public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType) {
				return destinationType == typeof(string);
			}

			public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
				if (value is string) {
					if (mappingFromString.ContainsKey((string)value)) {
						return mappingFromString[(string)value];
					}
				}
				return base.ConvertFrom(context, culture, value);
			}

			public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
				if (value is ExportOptionType && destinationType == typeof(string)) {
					if (mappingToString.ContainsKey((ExportOptionType)value)) {
						return mappingToString[(ExportOptionType)value];
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		[System.ComponentModel.TypeConverter(typeof(ExportOptionTypeEnumConverter))]
		public enum ExportOptionType {
			DH_FBH,
			FBH,
			DH,
		}


		private Plan plan;

		public ExportPlanForm(Plan plan) {
			InitializeComponent();
			this.SetLanguage();
			this.plan = plan;
		}

		private void UpdateForm() {
			btnExport.Enabled = false;
			if (txtPath.Text != "") {
				string dir = Path.GetDirectoryName(txtPath.Text);
				if (Directory.Exists(dir)) {
					btnExport.Enabled = true;
				}
			}
		}

		private void SetLanguage() {
			this.Text = EuroplanRes.ExportPlanForm_Titel; //"Plan exportieren";
			this.btnCancel.Text = EuroplanRes.ExportPlanForm_Abbrechen; //"Abbrechen";
			this.btnExport.Text = EuroplanRes.ExportPlanForm_Exportieren; //"Exportieren";
			this.lblExportOption.Text = EuroplanRes.ExportPlanForm_Exportumfang; //"Exportumfang";
			this.lblFileName.Text = EuroplanRes.ExportPlanForm_Dateipfad; //"Dateipfad";
			this.chkExportWallNumbers.Text = EuroplanRes.ExportPlanForm_Wandnummerierung;
		}

		private void ExportPlanForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ExportPlanForm"];
			this.Location = settings.GetPoint("Location", this.Location);

			bool graphicalWallFromPlan = false;
			bool floorIsPlanned = false;
			bool ceilingIsPlanned = false;

			foreach (Floor floor in Project.Instance.Floors) {
				if (floor.AssociatedPlanId != null && floor.AssociatedPlanId.Equals(plan.Id)) {
					foreach (Room room in floor.Rooms) {
						if (!graphicalWallFromPlan) {
							if (room.Walls != null && room.Walls.Count > 0) {
								foreach (GraphicalWall wall in room.Walls) {
									if (wall.PlanStartPoint.HasValue && wall.PlanEndPoint.HasValue) {
										graphicalWallFromPlan = true;
										break;
									}
								}
							}
						}
						if (!floorIsPlanned || !ceilingIsPlanned) {
							foreach (PlannedProduct pp in room.PlannedProducts) {
								if (pp.Product.Type == Product.ProductType.DH) {
									ceilingIsPlanned = true;
									if (floorIsPlanned) {
										break;
									}
								} else if (pp.Product.Type == Product.ProductType.FBH) {
									floorIsPlanned = true;
									if (ceilingIsPlanned) {
										break;
									}
								}
							}
						}
					}
				}
			}
			if (graphicalWallFromPlan) {
				chkExportWallNumbers.Enabled = true;
				chkExportWallNumbers.Checked = true;
			}
			IList<ExportOptionType> list = new List<ExportOptionType>();
			if (floorIsPlanned && ceilingIsPlanned) {
				list.Add(ExportOptionType.DH_FBH);
			}
			if (floorIsPlanned) {
				list.Add(ExportOptionType.FBH);
			}
			if (ceilingIsPlanned) {
				list.Add(ExportOptionType.DH);
			}
			if (list.Count == 0) {
				MessageBox.Show(EuroplanRes.ExportPlanForm_KeineProdukteText);
				this.Close();
			}

			this.exportOptionTypeBindingSource.DataSource = list;
			this.exportOptionTypeBindingSource.ResetBindings(false);

			UpdateForm();
		}

		private void ExportPlanForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ExportPlanForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}

		private void btnSaveAs_Click(object sender, EventArgs e) {
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.CheckPathExists = true;
			dialog.CreatePrompt = false;
			dialog.OverwritePrompt = true;
			dialog.InitialDirectory = Path.GetDirectoryName(Project.Instance.ProjectFileName);
			if (this.plan is CadPlan) {
				dialog.DefaultExt = "dxf";
				dialog.Filter = "DXF|*.dxf";
			} else if (this.plan is ImagePlan) {
				dialog.DefaultExt = Path.GetExtension(this.plan.AbsoluteFileName);
				dialog.Filter = dialog.DefaultExt + "|*." + dialog.DefaultExt;
			}
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK) {
				txtPath.Text = dialog.FileName;
			}
			dialog.Dispose();
			UpdateForm();
		}

		private void btnExport_Click(object sender, EventArgs e) {
			if (plan is ImagePlan) {
				Image image = Image.FromFile(plan.AbsoluteFileName);
				Bitmap b;
				Graphics g;
				if (plan.Measure.Value < 200) {
					float factor = 200.0f / plan.Measure.Value;
					if (image.Width * factor * image.Height * factor > 10000 * 5000) {
						//factor = (float)Math.Sqrt(10000.0f * 5000.0f / image.Width / image.Height);
					}
					try {
						b = new Bitmap((int)(image.Width * factor), (int)(image.Height * factor));
					} catch {
						MessageBox.Show(EuroplanRes.ExportPlanForm_BildFehlerText, EuroplanRes.ExportPlanForm_BildFehlerTitel, MessageBoxButtons.OK, MessageBoxIcon.Error);
						this.Close();
						return;
					}
					g = Graphics.FromImage(b);
					g.InterpolationMode = InterpolationMode.Bicubic;
					g.DrawImage(image, 0, 0, image.Width * factor, image.Height * factor);
					Matrix m = new Matrix();
					m.Scale(factor, factor);
					g.Transform = m;

				} else {
					b = new Bitmap(image);
					g = Graphics.FromImage(b);
					g.InterpolationMode = InterpolationMode.Bicubic;
				}
				foreach (Floor floor in Project.Instance.Floors) {
					if (floor.AssociatedPlanId != null && floor.AssociatedPlanId.Equals(plan.Id)) {
						foreach (Segment2D expansionGap in floor.ExpansionGaps) {
							g.DrawLine(Pens.Blue, (float)expansionGap.Start.X, (float)expansionGap.Start.Y, (float)expansionGap.End.X, (float)expansionGap.End.Y);
						}
						foreach (Room room in floor.Rooms) {
							if (chkExportWallNumbers.Enabled && chkExportWallNumbers.Checked) {
								float size = (float)(plan.Measure * 0.1 );
								Pen p = new Pen(Color.Black, size);
								float fontSize = 8.0f / g.DpiX * plan.Measure.Value;
								Font font = new Font("Arial", fontSize);
								StringFormat stringFormat = new StringFormat();
								stringFormat.Alignment = StringAlignment.Center;
								stringFormat.LineAlignment = StringAlignment.Center;

								foreach (GraphicalWall wall in room.Walls) {
									Point2D start = wall.PlanStartPoint.Value;
									Point2D end = wall.PlanEndPoint.Value;
									Vector2D normVector = wall.PlanEndPoint.Value - wall.PlanStartPoint.Value;
									normVector.Normalize();
									normVector = new Vector2D(normVector.Y, -normVector.X);
									Segment2D segment = new Segment2D(wall.PlanStartPoint.Value, wall.PlanEndPoint.Value);
									Point2D numberStart = segment.GetCenter() + (normVector * (plan.Measure.Value * 0.15));
									g.FillEllipse(Brushes.White, (float)(numberStart.X - fontSize), (float)(numberStart.Y - fontSize), fontSize * 2.0f, fontSize * 2.0f);
									//g.DrawEllipse(Pens.Black, (float)(numberStart.X - fontSize), (float)(numberStart.Y - fontSize), fontSize * 2.0f, fontSize * 2.0f);
									g.DrawString("" + (room.Walls.IndexOf(wall) + 1), font, new SolidBrush(p.Color), (float)numberStart.X, (float)numberStart.Y, stringFormat);
								}
							}
							foreach (PlannedProduct pp in room.PlannedProducts) {
								if ((cmbExportOption.SelectedValue.Equals(ExportOptionType.DH_FBH) &&
									(pp.Product.Type == Product.ProductType.DH || pp.Product.Type == Product.ProductType.FBH))
									|| (cmbExportOption.SelectedValue.Equals(ExportOptionType.FBH) && pp.Product.Type == Product.ProductType.FBH)
									|| (cmbExportOption.SelectedValue.Equals(ExportOptionType.DH) && pp.Product.Type == Product.ProductType.DH)) {
									Product p = pp.Product;
									if (p.GraphicalMode.HasValue && p.GraphicalMode.Value) {
										if (p is ModulKlimaDeckeProduct) {
											ModulKlimaDeckePlanner planner = new ModulKlimaDeckePlanner();
											planner.Product = p as ModulKlimaDeckeProduct;
											//(p as ModulKlimaDeckeProduct).GraphConstruction.Planner = planner;
											(p as ModulKlimaDeckeProduct).GraphConstruction.RecalculateSchienen();
											planner.HighlightRoomCoordinates = false;
											// TODO
											// planner.DrawBeplankung = ???
											// planner.Mode = ???
											planner.PaintAfterPlanPannel(g, Matrix4D.Identity, Point2D.Zero, Point.Empty, true);
										} else if (p is ModulKlimaBodenProduct) {
											ModulKlimaBodenPlanner planner = new ModulKlimaBodenPlanner();
											planner.Product = p as ModulKlimaBodenProduct;
											(p as ModulKlimaBodenProduct).GraphConstruction.Planner = planner;
											(p as ModulKlimaBodenProduct).GraphConstruction.RecalculateStaffeln();
											planner.HighlightRoomCoordinates = false;
											planner.DrawExpansionGaps = false;
											// TODO
											// planner.DrawBeplankung = ???
											// planner.Mode = ???
											planner.PaintAfterPlanPannel(g, Matrix4D.Identity, Point2D.Zero, Point.Empty, true);
										} else if (p is EurovalProduct) {
											EurovalPlanner planner = new EurovalPlanner();
											planner.Product = p as EurovalProduct;
											planner.HighlightRoomCoordinates = false;
											planner.DrawExpansionGaps = false;
											planner.PaintAfterPlanPannel(g, Matrix4D.Identity, Point2D.Zero, Point.Empty, true);
										} else if (p is EcothermProduct) {
											EcothermPlanner planner = new EcothermPlanner();
											planner.Product = p as EcothermProduct;
											planner.HighlightRoomCoordinates = false;
											planner.DrawExpansionGaps = false;
											planner.PaintAfterPlanPannel(g, Matrix4D.Identity, Point2D.Zero, Point.Empty, true);
										}
										//...
									}
								}

							}
						}
						ConnectionDrawer connectionDrawer = new ConnectionDrawer();
						connectionDrawer.Floor = floor;
						connectionDrawer.PlanCeiling = cmbExportOption.SelectedValue.Equals(ExportOptionType.DH_FBH) || cmbExportOption.SelectedValue.Equals(ExportOptionType.DH);
						connectionDrawer.PlanFloor = cmbExportOption.SelectedValue.Equals(ExportOptionType.DH_FBH) || cmbExportOption.SelectedValue.Equals(ExportOptionType.FBH);
						connectionDrawer.Paint(g, Matrix4D.Identity);
					}
				}
				Font warningFont = new Font("Arial", 10);
				SizeF warningSize = g.MeasureString(EuroplanRes.PlanExport_Hinweis, warningFont);
				g.DrawString(EuroplanRes.PlanExport_Hinweis, warningFont, Brushes.Black, new PointF(5, image.Height - 5 - warningSize.Height));
				
				ImageFormat format = null;
				string extension = Path.GetExtension(txtPath.Text);
				if (extension.ToLower() == ".jpg") {
					format = ImageFormat.Jpeg;
				} else if (extension.ToLower() == ".png") {
					format = ImageFormat.Png;
				} if (extension.ToLower() == ".bmp") {
					format = ImageFormat.Bmp;
				}
				b.Save(txtPath.Text, format);
				g.Dispose();
			} else if (plan is CadPlan) {
				DxfModel model = (plan as CadPlan).LoadModel(true);
				Dictionary<Type, DxfLayer> layers = new Dictionary<Type, DxfLayer>();

				DxfLayer ceilingConstructionLayer = new DxfLayer(EuroplanRes.ConstructionEditorForm_Decke);
				DxfLayer floorConstructionLayer = new DxfLayer(EuroplanRes.ConstructionEditorForm_Fussboden);
				// TODO: übersetzen
				DxfLayer beplankungLayer = new DxfLayer(EuroplanRes.ExportPlanForm_LayerBeplankung);
				DxfLayer dehnfugenLayer = new DxfLayer(EuroplanRes.ExportPlanForm_LayerDehnfugen);
				DxfLayer wandLayer = new DxfLayer(EuroplanRes.ExportPlanForm_LayerWandnumerierungen);
				DxfLayer distributorLayer = new DxfLayer(EuroplanRes.ExportPlanForm_LayerVerteiler);
				DxfLayer anbindeLayer = new DxfLayer(EuroplanRes.ExportPlanForm_LayerAnbindeleitungen);

				foreach (Floor floor in Project.Instance.Floors) {
					if (floor.AssociatedPlanId != null && floor.AssociatedPlanId.Equals(plan.Id)) {
						foreach (Segment2D expansionGap in floor.ExpansionGaps) {
							DxfLine line = new DxfLine(Color.Blue, expansionGap.Start, expansionGap.End);
							line.Layer = dehnfugenLayer;
							model.Entities.Add(line);
						}
						ConnectionDrawer connectionDrawer = new ConnectionDrawer();
						connectionDrawer.Floor = floor;
						connectionDrawer.PlanCeiling = cmbExportOption.SelectedValue.Equals(ExportOptionType.DH_FBH) || cmbExportOption.SelectedValue.Equals(ExportOptionType.DH);
						connectionDrawer.PlanFloor = cmbExportOption.SelectedValue.Equals(ExportOptionType.DH_FBH) || cmbExportOption.SelectedValue.Equals(ExportOptionType.FBH);
						connectionDrawer.DrawDxf(model, distributorLayer, anbindeLayer);

						foreach (Room room in floor.Rooms) {
							if (chkExportWallNumbers.Enabled && chkExportWallNumbers.Checked) {
								if (!model.TextStyles.Contains("HarreitherStyle")) {
									DxfTextStyle textStyle = new DxfTextStyle("HarreitherStyle", "Arial.ttf");
									model.TextStyles.Add(textStyle);
								}

								foreach (GraphicalWall wall in room.Walls) {
									Point2D start = wall.PlanStartPoint.Value;
									Point2D end = wall.PlanEndPoint.Value;
									Vector2D normVector = wall.PlanEndPoint.Value - wall.PlanStartPoint.Value;
									normVector.Normalize();
									normVector = new Vector2D(normVector.Y, -normVector.X);
									Segment2D segment = new Segment2D(wall.PlanStartPoint.Value, wall.PlanEndPoint.Value);
									Point2D numberStart = segment.GetCenter() + (normVector * (plan.Measure.Value * 0.15));
									DxfText text = new DxfText("" + (room.Walls.IndexOf(wall) + 1), (Point3D)numberStart, 0.08f * plan.Measure.Value);
									text.Style = model.TextStyles["HarreitherStyle"];
									text.HorizontalAlignment = TextHorizontalAlignment.Center;
									text.VerticalAlignment = TextVerticalAlignment.Middle;
									text.Layer = wandLayer;
									text.Color = Color.White;
									model.Entities.Add(text);
								}
							}

							foreach (PlannedProduct pp in room.PlannedProducts) {
								if ((cmbExportOption.SelectedValue.Equals(ExportOptionType.DH_FBH) &&
									(pp.Product.Type == Product.ProductType.DH || pp.Product.Type == Product.ProductType.FBH))
									|| (cmbExportOption.SelectedValue.Equals(ExportOptionType.FBH) && pp.Product.Type == Product.ProductType.FBH)
									|| (cmbExportOption.SelectedValue.Equals(ExportOptionType.DH) && pp.Product.Type == Product.ProductType.DH)) {
									Product p = pp.Product;
									if (p.GraphicalMode.HasValue && p.GraphicalMode.Value) {
										if (p is ModulKlimaDeckeProduct) {
											DxfLayer modulLayer = GetOrCreateDxfLayer(layers, typeof(ModulKlimaDeckeProduct), model);
											ModulKlimaDeckePlanner planner = new ModulKlimaDeckePlanner();
											planner.Product = p as ModulKlimaDeckeProduct;
											//(p as ModulKlimaDeckeProduct).GraphConstruction.Planner = planner;
											(p as ModulKlimaDeckeProduct).GraphConstruction.RecalculateSchienen();
											planner.HighlightRoomCoordinates = false;
											// TODO
											// planner.DrawBeplankung ???
											// planner.Mode = ???
											planner.DrawDxf(model, modulLayer, ceilingConstructionLayer, beplankungLayer);
										} else if (p is ModulKlimaBodenProduct) {
											DxfLayer modulLayer = GetOrCreateDxfLayer(layers, typeof(ModulKlimaBodenProduct), model);
											ModulKlimaBodenPlanner planner = new ModulKlimaBodenPlanner();
											planner.Product = p as ModulKlimaBodenProduct;
											(p as ModulKlimaBodenProduct).GraphConstruction.Planner = planner;
											(p as ModulKlimaBodenProduct).GraphConstruction.RecalculateStaffeln();
											planner.HighlightRoomCoordinates = false;
											planner.DrawExpansionGaps = false;
											// TODO
											// planner.DrawBeplankung ???
											// planner.Mode = ???
											planner.DrawDxf(model, modulLayer, floorConstructionLayer);
										} else if (p is EurovalProduct) {
											DxfLayer layer = GetOrCreateDxfLayer(layers, typeof(EurovalProduct), model);
											EurovalPlanner planner = new EurovalPlanner();
											planner.Product = p as EurovalProduct;
											planner.HighlightRoomCoordinates = false;
											planner.DrawExpansionGaps = false;
											planner.DrawDxf(model, layer);
										} else if (p is EcothermProduct) {
											DxfLayer layer = GetOrCreateDxfLayer(layers, typeof(EcothermProduct), model);
											EcothermPlanner planner = new EcothermPlanner();
											planner.Product = p as EcothermProduct;
											planner.HighlightRoomCoordinates = false;
											planner.DrawExpansionGaps = false;
											planner.DrawDxf(model, layer);
										}
										//.....
									}
								}
							}
						}
					}
				}
				foreach (DxfLayer layer in layers.Values) {
					model.Layers.Add(layer);
				}
				try {
					DxfWriter.Write(txtPath.Text, model);
				} catch (Exception ex) {
					Console.Out.WriteLine(ex.StackTrace);
				}
			}
			this.Close();
		}

		private DxfLayer GetOrCreateDxfLayer(Dictionary<Type, DxfLayer> layers, Type key, DxfModel model) {
			if (layers.ContainsKey(key)) {
				return layers[key];
			} else {
				object[] attributes = key.GetCustomAttributes(typeof(ProductNameAttribute), true);

				string name = "";
				if (attributes.Length > 0) {
					name = (attributes[0] as ProductNameAttribute).FullName;
				} else {
					name = key.Name;
				}
				name = name.Replace(' ', '_');
				name = name.Replace("®", "");

				foreach (DxfLayer l in model.Layers) {
					if (l.Name == name) {
						return l;
					}
				}

				DxfLayer layer = new DxfLayer(name);
				layers.Add(key, layer);
				return layer;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			this.Close();
		}

	}
}