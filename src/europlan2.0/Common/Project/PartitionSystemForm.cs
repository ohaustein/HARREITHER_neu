using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Europlan.Common {
    public partial class PartitionSystemForm : Form {

        private PlannedProduct plannedOriginalProduct = null;
        private JumbovalProduct jvOriginalProduct = null;
        private ModulKlimaDeckeProduct mkdOriginalProduct = null;

        private ProductConnection originalProductConnection = null;

        private Distributor newDistributor = new Distributor();

        private List<Product> newProducts = new List<Product>();

        public PartitionSystemForm(PlannedProduct product, RegulatorCircuit regulatorCircuit) {
            InitializeComponent();

            this.SetLanguage();

            this.SetDefaultValues();

            this.DialogResult = DialogResult.Cancel;

            if (product == null) {
                return;
            }
            if (product.Product is JumbovalProduct) {
                this.jvOriginalProduct = (JumbovalProduct)product.Product;
                this.plannedOriginalProduct = product;
                this.originalProductConnection = this.jvOriginalProduct.PlannedConnection;
                this.newDistributor.MaxCircuits = Int32.MaxValue;

                this.jvOriginalProduct.PlannedConnection = new ProductConnection(newDistributor);
                this.jvOriginalProduct.PartitionCalculation = true;
            } else if (product.Product is ModulKlimaDeckeProduct) {
                this.mkdOriginalProduct = (ModulKlimaDeckeProduct)product.Product;
                this.plannedOriginalProduct = product;
                this.originalProductConnection = this.mkdOriginalProduct.PlannedConnection;
                this.newDistributor.MaxCircuits = Int32.MaxValue;

                this.mkdOriginalProduct.PlannedConnection = new ProductConnection(newDistributor);
            }

            this.cmbCircuit.SelectedItem = regulatorCircuit;
        }

        private void SetLanguage() {
            this.label4.Text = EuroplanRes.DistributorPanel_Regelkreis; //"Regelkreis:"
            this.label5.Text = EuroplanRes.DistributorPanel_MaximaleHeizkreise; //"max. Heizkreise:"
            this.chkEinbauschrank.Text = EuroplanRes.DistributorPanel_Einbauschrank; //"Einbauschrank"
            this.chkFlansch.Text = EuroplanRes.DistributorPanel_Flanschkugelhaehne; //"Flanschkugelhähne"
            this.label10.Text = EuroplanRes.DistributorPanel_Anschlusshollaender; //"Anschlußholländer:"
            this.label11.Text = EuroplanRes.DistributorPanel_Zubehoer; //"Zubehör:"
            this.chkAnschluss.Text = EuroplanRes.DistributorPanel_LangeAnschlussboegen; //"Lange Anschlussbögen"
            this.lblMaximaldurchfluss.Text = EuroplanRes.DistributorPanel_Maximaldurchfluss; //"Maximaldurchfluß:"
            this.groupBox1.Text = ""; // TODO: groupBox1.Text übersetzen

            this.cmbMaximaldurchfluss.Items.Clear();
            foreach (Distributor.DistributorTypeEnum item in Enum.GetValues(typeof(Distributor.DistributorTypeEnum))) {
                this.cmbMaximaldurchfluss.Items.Add(item);
            }
            this.cmbAnschlussHollaender.Items.Clear();
            foreach (Distributor.AnschlussHollaenderEnum item in Enum.GetValues(typeof(Distributor.AnschlussHollaenderEnum))) {
                this.cmbAnschlussHollaender.Items.Add(item);
            }
            this.cmbCircuit.Items.Clear();
            foreach (RegulatorCircuit circuit in Project.Instance.RegulatorCircuits) {
                this.cmbCircuit.Items.Add(circuit);
            }
        }

        private void SetDefaultValues() {
            this.cmbMaximaldurchfluss.SelectedItem = Distributor.DistributorTypeEnum.DT_480;
            this.cmbAnschlussHollaender.SelectedItem = Distributor.AnschlussHollaenderEnum.Kein;
            this.cmbCircuit.SelectedIndex = 0;
            this.numMaxCircuits.Value = 8;
            this.chkFlansch.Checked = false;
            this.chkEinbauschrank.Checked = false;
            this.chkAnschluss.Checked = false;
        }


        private void PartitionSystemForm_Load(object sender, EventArgs e) {
            if (this.jvOriginalProduct != null) {
                this.RecalculateSystem();
            } else if (this.mkdOriginalProduct != null) {
                this.RecalculateSystem();
            } else {
                this.Close();
            }
        }

        public void UpdateControl() {
            Distributor.DistributorTypeEnum distributorType = (Distributor.DistributorTypeEnum)this.cmbMaximaldurchfluss.SelectedItem;
            this.numMaxCircuits.Maximum = distributorType == Distributor.DistributorTypeEnum.DT_480 ? 8 : 12;
        }

        private void cmbMaximaldurchfluss_SelectedIndexChanged(object sender, EventArgs e) {
            Distributor.DistributorTypeEnum distributorType = (Distributor.DistributorTypeEnum)this.cmbMaximaldurchfluss.SelectedItem;
            if (distributorType == Distributor.DistributorTypeEnum.DT_480) {
                numMaxCircuits.Maximum = 8;
            } else {
                numMaxCircuits.Maximum = 12;
            }
            this.RecalculateSystem();
        }

        private void RecalculateSystem() {
            if (this.cmbAnschlussHollaender.SelectedItem == null || this.cmbMaximaldurchfluss.SelectedItem == null || this.cmbCircuit.SelectedItem == null || this.plannedOriginalProduct == null || this.plannedOriginalProduct.Product == null) {
                return;
            }
            this.newDistributor.AnschlussHollaender = (Distributor.AnschlussHollaenderEnum)this.cmbAnschlussHollaender.SelectedItem;
            this.newDistributor.DistributorType = (Distributor.DistributorTypeEnum)this.cmbMaximaldurchfluss.SelectedItem;
            this.newDistributor.RegulatorCircuit = (RegulatorCircuit)this.cmbCircuit.SelectedItem;
            this.newDistributor.EinbauSchrank = this.chkEinbauschrank.Checked;
            this.newDistributor.FlanschKugelHaehne = this.chkFlansch.Checked;
            this.newDistributor.LangeAnschlussboegen = this.chkAnschluss.Checked;

            this.plannedOriginalProduct.ConfigureProduct(true);

            this.lstError.Items.Clear();
            foreach (string message in this.plannedOriginalProduct.Product.ErrorMessageArray) {
                ListViewItem item = new ListViewItem(message);
                item.ForeColor = Color.Red;
                this.lstError.Items.Add(item);
            }
            if (this.lstError.Items.Count > 0) {
                this.lstError.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                //int height = this.lstError.Items[this.lstError.Items.Count - 1].Position.Y + this.lstError.Items[this.lstError.Items.Count - 1].Bounds.Height + 20;
                //this.lstError.Height = height;
                this.lstError.Visible = true;
            } else {
                this.lstError.Visible = false;
            }
        }

        private void PartitionSystemForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (this.DialogResult == DialogResult.Cancel) {
                // revert system
                if (this.jvOriginalProduct != null) {
                    this.jvOriginalProduct.PlannedConnection = this.originalProductConnection;
                    this.jvOriginalProduct.PartitionCalculation = false;
                    this.plannedOriginalProduct.ConfigureProduct(true);
                } else if (this.mkdOriginalProduct != null) {
                    this.mkdOriginalProduct.PlannedConnection = this.originalProductConnection;
                    this.plannedOriginalProduct.ConfigureProduct(true);
                }
            } else {
                // partition system

                List<Product> newProducts = new List<Product>();
                Product originalProduct = null;

                if (this.jvOriginalProduct != null) {
                    originalProduct = this.jvOriginalProduct;

                    int circuitsLeft = this.jvOriginalProduct.PlannedCircuitCount;
                    int originalCircuits = circuitsLeft;
                    double originalArea = this.jvOriginalProduct.PlannedFloorArea;
                    double originalHeatLoad = this.plannedOriginalProduct.RequestedHeatLoad;
                    double originalCoolLoad = this.plannedOriginalProduct.RequestedCoolLoad;
                    double originalReducedArea = this.jvOriginalProduct.PlannedAreaReduced;
                    double originalUnheatedArea = this.jvOriginalProduct.PlannedAreaUnheated;
                    double originalRimLength = this.jvOriginalProduct.PlannedRimLength;

                    Room room = this.jvOriginalProduct.AssociatedRoom;
                    bool calculateHeat = this.plannedOriginalProduct.CalculateHeat;
                    bool calculateCool = this.plannedOriginalProduct.CalculateCool;
                    Construction floorConstruction = this.jvOriginalProduct.PlannedFloorConstruction;
                    Construction insulationConstruction = this.jvOriginalProduct.PlannedInsulationConstruction;
                    Product.CalculateModeEnum calculateMode = this.jvOriginalProduct.CalculateMode;
                    RegulatorCircuit regCirc = this.cmbCircuit.SelectedItem as RegulatorCircuit;

                    int circuitsPerSystem = (int) this.numMaxCircuits.Value;

                    int newSystemCount = (int)Math.Ceiling(((double)circuitsLeft) / ((double)circuitsPerSystem));

                    int rimCorners = this.jvOriginalProduct.PlannedRimCorners / newSystemCount;
                    int rimCornersRest = this.jvOriginalProduct.PlannedRimCorners % newSystemCount;

                    Nullable<JumbovalProduct.JumbovalLayDistance> layDistance = this.jvOriginalProduct.RequestedLayDistance;
                    Nullable<JumbovalProduct.JumbovalRimType> rimType = this.jvOriginalProduct.RequestedRimType;
                    double estrichueberdeckung = this.jvOriginalProduct.Estrichueberdeckung;
                    double schienenabstand = this.jvOriginalProduct.Schienenabstand;
                    float roomTempBelowHeat = this.jvOriginalProduct.PlannedRoomTemperatureBelowHeat;
                    float roomTempBelowCool = this.jvOriginalProduct.PlannedRoomTemperatureBelowCool;

                    string distributorNameTemplate = EuroplanRes.PartitionSystemForm_NewDistributorName.Replace("%SYSTEM%", this.jvOriginalProduct.Name).Replace("%RAUMNAME%", room.Name).Replace("%TOTAL%", newSystemCount.ToString());

                    int i = 1;

                    int indexInRoom = room.PlannedProducts.IndexOf(this.plannedOriginalProduct);

                    circuitsLeft -= ConfigureNewJumbovalSystem(this.plannedOriginalProduct, this.jvOriginalProduct, room, calculateHeat, calculateCool, Math.Min(circuitsLeft, circuitsPerSystem), originalCircuits, floorConstruction, insulationConstruction, calculateMode, originalArea, originalUnheatedArea, originalReducedArea, originalRimLength, rimCorners + (rimCornersRest > 0 ? 1 : 0), originalHeatLoad, originalCoolLoad, layDistance, rimType, estrichueberdeckung, schienenabstand, roomTempBelowHeat, roomTempBelowCool, Distributor.NewDistributorId, distributorNameTemplate.Replace("%NUMBER%", i.ToString()), regCirc, indexInRoom);
                    

                    while (circuitsLeft > 0) {
                        i++;
                        indexInRoom++;
                        JumbovalProduct newProduct = new JumbovalProduct();
                        newProducts.Add(newProduct);
                        circuitsLeft -= ConfigureNewJumbovalSystem(null, newProduct, room, calculateHeat, calculateCool, Math.Min(circuitsLeft, circuitsPerSystem), originalCircuits, floorConstruction, insulationConstruction, calculateMode, originalArea, originalUnheatedArea, originalReducedArea, originalRimLength, rimCorners + (rimCornersRest >= i ? 1 : 0), originalHeatLoad, originalCoolLoad, layDistance, rimType, estrichueberdeckung, schienenabstand, roomTempBelowHeat, roomTempBelowCool, Distributor.NewDistributorId, distributorNameTemplate.Replace("%NUMBER%", i.ToString()), regCirc, indexInRoom);
                    }
                } else if (this.mkdOriginalProduct != null) {
                    originalProduct = this.mkdOriginalProduct;

                    int circuitsLeft = this.mkdOriginalProduct.PlannedCircuitCount;
                    int originalCircuits = circuitsLeft;
                    double originalArea = this.mkdOriginalProduct.PlannedCeilingArea;
                    double originalHeatLoad = this.plannedOriginalProduct.RequestedHeatLoad;
                    double originalCoolLoad = this.plannedOriginalProduct.RequestedCoolLoad;
                    double originalPlannedHeatLoad = this.plannedOriginalProduct.PlannedHeatLoad;
                    double originalPlannedCoolLoad = this.plannedOriginalProduct.PlannedCoolLoad;
                    double originalUnheatedArea = this.mkdOriginalProduct.PlannedAreaUnheated;

                    Room room = this.mkdOriginalProduct.AssociatedRoom;
                    bool calculateHeat = this.plannedOriginalProduct.CalculateHeat;
                    bool calculateCool = this.plannedOriginalProduct.CalculateCool;
                    Construction ceilingConstruction = this.mkdOriginalProduct.PlannedCeilingConstruction;
                    Construction insulationConstruction = this.mkdOriginalProduct.PlannedInsulationConstruction;
                    Product.CalculateModeEnum calculateMode = this.mkdOriginalProduct.CalculateMode;
                    RegulatorCircuit regCirc = this.cmbCircuit.SelectedItem as RegulatorCircuit;

                    int circuitsPerSystem = (int)this.numMaxCircuits.Value;

                    float roomTempBelowHeat = this.mkdOriginalProduct.PlannedRoomTemperatureBelowHeat;
                    float roomTempBelowCool = this.mkdOriginalProduct.PlannedRoomTemperatureBelowCool;

                    int newSystemCount = (int)Math.Ceiling(((double)circuitsLeft) / ((double)circuitsPerSystem));
                    string distributorNameTemplate = EuroplanRes.PartitionSystemForm_NewDistributorName.Replace("%SYSTEM%", this.mkdOriginalProduct.Name).Replace("%RAUMNAME%", room.Name).Replace("%TOTAL%", newSystemCount.ToString());

                    int i = 1;

                    double originalCoveredArea = this.mkdOriginalProduct.CoveredArea;

                    Queue<ModulDeckeCircuit> circuits = new Queue<ModulDeckeCircuit>();
                    foreach (ModulDeckeCircuit c in this.mkdOriginalProduct.PlannedCircuits) {
                        circuits.Enqueue(c);
                    }
                    this.mkdOriginalProduct.PlannedCircuits.Clear();
                    int indexInRoom = room.PlannedProducts.IndexOf(this.plannedOriginalProduct);

                    circuitsLeft -= ConfigureNewModulKlimaDeckeSystem(this.plannedOriginalProduct, this.mkdOriginalProduct, room, calculateHeat, calculateCool, Math.Min(circuitsLeft, circuitsPerSystem), originalCircuits, ceilingConstruction, insulationConstruction, calculateMode, originalArea, originalUnheatedArea, originalCoveredArea, originalHeatLoad, originalCoolLoad, originalPlannedHeatLoad, originalPlannedCoolLoad, circuits, roomTempBelowHeat, roomTempBelowCool, Distributor.NewDistributorId, distributorNameTemplate.Replace("%NUMBER%", i.ToString()), regCirc, indexInRoom);


                    while (circuitsLeft > 0) {
                        i++;
                        indexInRoom++;
                        ModulKlimaDeckeProduct newProduct = new ModulKlimaDeckeProduct();
                        newProducts.Add(newProduct);
                        circuitsLeft -= ConfigureNewModulKlimaDeckeSystem(null, newProduct, room, calculateHeat, calculateCool, Math.Min(circuitsLeft, circuitsPerSystem), originalCircuits, ceilingConstruction, insulationConstruction, calculateMode, originalArea, originalUnheatedArea, originalCoveredArea, originalHeatLoad, originalCoolLoad, originalPlannedHeatLoad, originalPlannedCoolLoad, circuits, roomTempBelowHeat, roomTempBelowCool, Distributor.NewDistributorId, distributorNameTemplate.Replace("%NUMBER%", i.ToString()), regCirc, indexInRoom);
                    }
                }
                foreach (Product p in newProducts) {
                    foreach (ConnectionPipe c in originalProduct.PlannedConnectionPipes) {
                        if (!c.OnlyFirst) {
                            ConnectionPipe newC = new ConnectionPipe();
                            newC.ConnectionThrough = c.ConnectionThrough;
                            newC.Insulation = c.Insulation;
                            newC.PipeType = c.PipeType;
                            newC.Print = c.Print;
                            newC.Room = c.Room;
                            newC.Ruecklauf = c.Ruecklauf;
                            newC.Verlegeart = c.Verlegeart;
                            newC.Vorlauf = c.Vorlauf;
                            p.PlannedConnectionPipes.Add(newC);
                        }
                    }
                }
            }
        }

        private int ConfigureNewJumbovalSystem(PlannedProduct originalPp, JumbovalProduct p, Room room, bool calculateHeat, bool calculateCool, int circuits, int originalCircuits,
                                             Construction floorConstruction, Construction insulationConstruction, Product.CalculateModeEnum calculatMode,
                                             double originalFloorArea, double originalUnheatedArea, double originalReducedArea,
                                             double originalRimLength, int rimCorners,
                                             double originalHeatLoad, double originalCoolLoad,
                                             Nullable<JumbovalProduct.JumbovalLayDistance> layDistance, Nullable<JumbovalProduct.JumbovalRimType> rimType,
                                             double estrichueberdeckung, double schienenabstand,
                                             float roomTempBelowHeat, float roomTempBelowCool, 
                                             String distributorId, String distributorName, RegulatorCircuit regulatorCircuit, int indexInRoom) {
            Floor f = room.AssociatedFloor;

            p.InitializeNewProduct();
            p.UsedForQuickDimensioning = false;
            p.AssociatedRoom = room;
            PlannedProduct pp = originalPp;
            if (pp == null) {
                pp = new PlannedProduct(p);
            }
            pp.CalculateHeat = calculateHeat;
            pp.CalculateCool = calculateCool;
            p.RequestedCircuits = circuits;
            p.PlannedFloorArea = (float)(originalFloorArea / originalCircuits * circuits);
            pp.CoverHeatLoad = false;
            pp.CoverCoolLoad = false;
            pp.RequestedHeatLoad = originalHeatLoad / originalCircuits * circuits;
            pp.RequestedCoolLoad = originalHeatLoad / originalCircuits * circuits;
            p.PlannedAreaUnheated = (float)(originalUnheatedArea / originalCircuits * circuits);
            p.PlannedAreaReduced = (float)(originalReducedArea / originalCircuits * circuits);
            p.PlannedRimLength = (float)(originalRimLength / originalCircuits * circuits);
            p.PlannedRimCorners = rimCorners;

            p.RequestedLayDistance = layDistance;
            p.RequestedRimType = rimType;
            p.Estrichueberdeckung = estrichueberdeckung;
            p.Schienenabstand = schienenabstand;
            p.PlannedRoomTemperatureBelowHeat = roomTempBelowHeat;
            p.PlannedRoomTemperatureBelowCool = roomTempBelowCool;

            Distributor d = new Distributor();
            d.Id = distributorId;
            d.Name = distributorName;
            d.RegulatorCircuit = regulatorCircuit;
            d.DistributorType = (Distributor.DistributorTypeEnum)this.cmbMaximaldurchfluss.SelectedItem;
            d.AnschlussHollaender = (Distributor.AnschlussHollaenderEnum)this.cmbAnschlussHollaender.SelectedItem;
            d.FlanschKugelHaehne = this.chkFlansch.Checked;
            d.EinbauSchrank = this.chkEinbauschrank.Checked;
            d.LangeAnschlussboegen = this.chkAnschluss.Checked;
            d.MaxCircuits = (int)this.numMaxCircuits.Value;
            f.Distributors.Add(d);

            p.PlannedConnection = new ProductConnection(d);

            p.PlannedFloorConstruction = floorConstruction;
            p.PlannedInsulationConstruction = insulationConstruction;

            pp.Product.CalculateMode = calculatMode;

            if (originalPp == null) {
                room.PlannedProducts.Insert(indexInRoom, pp);
                //room.PlannedProducts.Add(pp);
            }

            pp.ConfigureProduct(false);

            return circuits;
        }

        private int ConfigureNewModulKlimaDeckeSystem(PlannedProduct originalPp, ModulKlimaDeckeProduct p, Room room, bool calculateHeat, bool calculateCool, int circuits, int originalCircuits,
                                             Construction ceilingConstruction, Construction insulationConstruction, Product.CalculateModeEnum calculatMode,
                                             double originalCeilingArea, double originalUnheatedArea, double originalCoveredArea, 
                                             double originalHeatLoad, double originalCoolLoad, double originalPlannedHeatLoad, double originalPlannedCoolLoad,
                                             Queue<ModulDeckeCircuit> circuitsLeft,
                                             float roomTempBelowHeat, float roomTempBelowCool,
                                             String distributorId, String distributorName, RegulatorCircuit regulatorCircuit, int indexInRoom) {
            Floor f = room.AssociatedFloor;

            p.InitializeNewProduct();
            p.UsedForQuickDimensioning = false;
            p.AssociatedRoom = room;
            PlannedProduct pp = originalPp;
            if (pp == null) {
                pp = new PlannedProduct(p);
            }
            pp.CalculateHeat = calculateHeat;
            pp.CalculateCool = calculateCool;
            p.PlannedCircuits.Clear();
            double coveredArea = 0;
            double coveredHeatLoad = 0;
            double coveredCoolLoad = 0;
            for (int i = 0; i < circuits; i++) {
                ModulDeckeCircuit c = circuitsLeft.Dequeue();
                c.ModulKlimaDeckeProduct = p;
                p.PlannedCircuits.Add(c);
                coveredArea += c.CoveredArea;
                coveredHeatLoad += c.QHeat;
                coveredCoolLoad += c.QCool;
            }
            //p.RequestedCircuits = circuits;
            //p.PlannedCeilingArea = (float)p.CoveredArea;
            p.PlannedCeilingArea = (float)(originalCeilingArea * coveredArea / originalCoveredArea);
            pp.CoverHeatLoad = false;
            pp.CoverCoolLoad = false;
            pp.RequestedHeatLoad = (originalPlannedHeatLoad == 0) ? 0 : (originalHeatLoad * coveredHeatLoad / originalPlannedHeatLoad);
            pp.RequestedCoolLoad = (originalPlannedCoolLoad == 0) ? 0 : (originalCoolLoad * coveredCoolLoad / originalPlannedCoolLoad);
            p.PlannedAreaUnheated = (float)(originalUnheatedArea * coveredArea / originalCoveredArea);

            p.PlannedRoomTemperatureBelowHeat = roomTempBelowHeat;
            p.PlannedRoomTemperatureBelowCool = roomTempBelowCool;

            Distributor d = new Distributor();
            d.Id = distributorId;
            d.Name = distributorName;
            d.RegulatorCircuit = regulatorCircuit;
            d.DistributorType = (Distributor.DistributorTypeEnum)this.cmbMaximaldurchfluss.SelectedItem;
            d.AnschlussHollaender = (Distributor.AnschlussHollaenderEnum)this.cmbAnschlussHollaender.SelectedItem;
            d.FlanschKugelHaehne = this.chkFlansch.Checked;
            d.EinbauSchrank = this.chkEinbauschrank.Checked;
            d.LangeAnschlussboegen = this.chkAnschluss.Checked;
            d.MaxCircuits = (int)this.numMaxCircuits.Value;
            f.Distributors.Add(d);

            p.PlannedConnection = new ProductConnection(d);

            p.PlannedCeilingConstruction = ceilingConstruction;
            p.PlannedInsulationConstruction = insulationConstruction;

            pp.Product.CalculateMode = calculatMode;

            if (originalPp == null) {
                room.PlannedProducts.Insert(indexInRoom, pp);
                //room.PlannedProducts.Add(pp);
            }

            pp.ConfigureProduct(false);

            return circuits;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmbCircuit_SelectedIndexChanged(object sender, EventArgs e) {
            this.RecalculateSystem();
        }
    }
}