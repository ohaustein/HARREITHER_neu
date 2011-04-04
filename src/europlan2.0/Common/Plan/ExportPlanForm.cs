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

			this.exportOptionTypeBindingSource.DataSource = Enum.GetValues(typeof(ExportOptionType));
			this.exportOptionTypeBindingSource.ResetBindings(false);		
			
			UpdateForm();
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
		}

		private void ExportPlanForm_Load(object sender, EventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ExportPlanForm"];
			this.Location = settings.GetPoint("Location", this.Location);
		}

		private void ExportPlanForm_FormClosing(object sender, FormClosingEventArgs e) {
			SettingsKey settings = SettingsFile.Settings["ExportPlanForm"];
			settings.StorePoint("Location", this.Location);
			SettingsFile.Update();
		}

		private void btnSaveAs_Click(object sender, EventArgs e) {
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.CheckPathExists = true;
			dialog.CreatePrompt = true;
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
				Graphics g = Graphics.FromImage(image);
				g.InterpolationMode = InterpolationMode.Bicubic;
				foreach (Floor floor in Project.Instance.Floors) {

					foreach (Segment2D expansionGap in floor.ExpansionGaps) {
						g.DrawLine(Pens.Blue, (float)expansionGap.Start.X, (float)expansionGap.Start.Y, (float)expansionGap.End.X, (float)expansionGap.End.Y);
					}

					if (floor.AssociatedPlanId != null && floor.AssociatedPlanId.Equals(plan.Id)) {
						foreach (Room room in floor.Rooms) {
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
											(p as ModulKlimaDeckeProduct).GraphConstruction.Planner = planner;
											(p as ModulKlimaDeckeProduct).GraphConstruction.RecalculateSchienen();
											planner.HighlightRoomCoordinates = false;
											// TODO
											// planner.DrawBeplankung = ???
											// planner.Mode = ???
											planner.PaintAfterPlanPannel(g, Matrix4D.Identity, Point2D.Zero, Point.Empty);
										} else if (p is ModulKlimaBodenProduct) {
											ModulKlimaBodenPlanner planner = new ModulKlimaBodenPlanner();
											planner.Product = p as ModulKlimaBodenProduct;
											(p as ModulKlimaBodenProduct).GraphConstruction.Planner = planner;
											(p as ModulKlimaBodenProduct).GraphConstruction.RecalculateSchienen();
											planner.HighlightRoomCoordinates = false;
											planner.DrawExpansionGaps = false;
											// TODO
											// planner.DrawBeplankung = ???
											// planner.Mode = ???
											planner.PaintAfterPlanPannel(g, Matrix4D.Identity, Point2D.Zero, Point.Empty);
										}
										//...
									}
								}

							}
						}
					}
				}
				ImageFormat format = null;
				string extension = Path.GetExtension(txtPath.Text);
				if (extension.ToLower() == ".jpg") {
					format = ImageFormat.Jpeg;
				} else if (extension.ToLower() == ".png") {
					format = ImageFormat.Png;
				} if (extension.ToLower() == ".bmp") {
					format = ImageFormat.Bmp;
				}
				image.Save(txtPath.Text, format);
				g.Dispose();
			} else if (plan is CadPlan) {
				DxfModel model = (plan as CadPlan).LoadModel(true);
				Dictionary<Type, DxfLayer> layers = new Dictionary<Type, DxfLayer>();

				DxfLayer ceilingConstructionLayer = new DxfLayer(EuroplanRes.ConstructionEditorForm_Decke);
				DxfLayer floorConstructionLayer = new DxfLayer(EuroplanRes.ConstructionEditorForm_Fussboden);
				// TODO: übersetzen
				DxfLayer beplankungLayer = new DxfLayer("Beplankung");
				DxfLayer dehnfugenLayer = new DxfLayer("Dehnfugen");

				foreach (Floor floor in Project.Instance.Floors) {

					foreach (Segment2D expansionGap in floor.ExpansionGaps) {
						DxfLine line = new DxfLine(Color.Blue, expansionGap.Start, expansionGap.End);
						line.Layer = dehnfugenLayer;
						model.Entities.Add(line);
					}

					if (floor.AssociatedPlanId != null && floor.AssociatedPlanId.Equals(plan.Id)) {
						foreach (Room room in floor.Rooms) {
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
											(p as ModulKlimaDeckeProduct).GraphConstruction.Planner = planner;
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
											(p as ModulKlimaBodenProduct).GraphConstruction.RecalculateSchienen();
											planner.HighlightRoomCoordinates = false;
											planner.DrawExpansionGaps = false;
											// TODO
											// planner.DrawBeplankung ???
											// planner.Mode = ???
											planner.DrawDxf(model, modulLayer, floorConstructionLayer);
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
				DxfWriter.Write(txtPath.Text, model);
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