using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace Europlan.Common
{

    public class ModulKlimaBoden20Circuit : Circuit
    {

        private List<ModulKlimaBoden20SubArea> subAreas = new List<ModulKlimaBoden20SubArea>();
        private Color circuitColor = Color.FromArgb(0, 128, 0);

        private List<KlimaFlaechenSubAreaVerbindung> verbindungen = new List<KlimaFlaechenSubAreaVerbindung>();
        private double reducedArea = 0;

        private ModulKlimaBoden20Circuit()
        {
        }

        public ModulKlimaBoden20Circuit(ModulKlimaBoden20Product product)
        {
            this.ModulKlimaBoden20Product = product;
            // add one empty default circuit
            this.subAreas.Add(new ModulKlimaBoden20SubArea());
        }

        public List<ModulKlimaBoden20SubArea> SubAreas
        {
            get { return this.subAreas; }
            set { this.subAreas = value; }
        }

        public double ReducedArea
        {
            get { return this.reducedArea; }
            set { reducedArea = value; }
        }

        private ModulKlimaBoden20Product mdProduct = null;

        [XmlIgnore]
        public ModulKlimaBoden20Product ModulKlimaBoden20Product
        {
            get
            {
                return this.PlannedProduct == null ? null : this.PlannedProduct.Product as ModulKlimaBoden20Product;
            }
            set
            {
                bool found = false;
                this.plannedProduct = null;
                this.mdProduct = null;
                foreach (Floor f in Project.Instance.Floors)
                {
                    foreach (Room r in f.Rooms)
                    {
                        foreach (PlannedProduct pp in r.PlannedProducts)
                        {
                            if (pp.Product == value)
                            {
                                this.plannedProduct = pp;
                                found = true;
                            }
                        }
                    }
                }
                if (!found)
                {
                    this.mdProduct = value;
                }
            }
        }

        private PlannedProduct plannedProduct = null;

        [XmlIgnore]
        public override PlannedProduct PlannedProduct
        {
            get
            {
                if (this.plannedProduct == null && this.mdProduct != null)
                {
                    this.ModulKlimaBoden20Product = this.mdProduct;
                }
                return this.plannedProduct;
            }
        }

        #region Area
        /// <summary>
        /// Summe der Flächen der einzelnen Module
        /// </summary>
        [XmlIgnore]
        public double CoveredArea
        {
            get
            {
                double area = 0;
                foreach (ModulKlimaBoden20SubArea subArea in subAreas)
                {
                    area += subArea.CoveredArea;
                }
                return area;
            }
        }

        [XmlIgnore]
        public double HeatArea
        {
            get
            {
                double area = 0;
                foreach (ModulKlimaBoden20SubArea subArea in subAreas)
                {
                    area += subArea.HeatArea;
                }
                return area;
            }
        }

        [XmlIgnore]
        private double HeatAreaForCalculation
        {
            get
            {
                if (this.reducedArea > this.HeatArea)
                {
                    return this.HeatArea / 2;
                }
                return this.HeatArea - this.reducedArea / 2;
            }
        }

        #endregion Area

        private double c_qHeatPerSqm;
        private double c_qCoolPerSqm;

        [XmlIgnore]
        public double C_QHeatPerSqm
        {
            get { return c_qHeatPerSqm; }
        }

        [XmlIgnore]
        public double C_QCoolPerSqm
        {
            get { return c_qCoolPerSqm; }
        }

        private double c_floorTempHeat;
        [XmlIgnore]
        public double C_FloorTempHeat
        {
            get { return this.c_floorTempHeat; }
        }

        private double c_floorTempCool;
        [XmlIgnore]
        public double C_FloorTempCool
        {
            get { return this.c_floorTempCool; }
        }

        private double c_thetaVHeat;
        private double c_thetaRHeat;

        private double c_thetaVCool;
        private double c_thetaRCool;

        [XmlIgnore]
        public override double PipeLengthWithoutConnections
        {
            get
            {
                double length = 0;
                foreach (ModulKlimaBoden20SubArea subArea in this.subAreas)
                {
                    length += subArea.EquivalentPipeLength;
                }
                return length;
            }
        }

        [XmlIgnore]
        public override double PipeLengthWithAllConnections
        {
            get { return this.PipeLengthWithoutConnections + this.vorlaufTotal + this.ruecklaufTotal; }
        }

        [XmlIgnore]
        public override double PipeLengthWithUnisolatedConnections
        {
            get { return this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated; }
        }

        [XmlIgnore]
        public double QHeat
        {
            get { return this.c_qHeatPerSqm * this.HeatAreaForCalculation; }
        }

        [XmlIgnore]
        public double QCool
        {
            get { return -this.c_qCoolPerSqm * this.HeatAreaForCalculation; }
        }

        [XmlIgnore]
        public double QFbhTotalHeat
        {
            get { return this.QHeat; }
        }

        [XmlIgnore]
        public double QFbhTotalCool
        {
            get { return this.QCool; }
        }

        public void Calculate()
        {
            EN1264 en1264 = EN1264.Instance;

            double alphaInnenHeat = 8;
            double alphaAussenHeat = 8;
            double alphaInnenCool = 8;
            double alphaAussenCool = 8;
            switch (this.ModulKlimaBoden20Product.ModulType)
            {
                case Product.ProductType.FBH:
                    alphaInnenHeat = Product.ConfigAlphaBodenHeat;
                    alphaAussenHeat = Product.ConfigAlphaDeckeHeat;
                    alphaInnenCool = Product.ConfigAlphaBodenCool;
                    alphaAussenCool = Product.ConfigAlphaDeckeCool;
                    break;

                case Product.ProductType.DH:
                    alphaInnenHeat = Product.ConfigAlphaDeckeHeat;
                    alphaAussenHeat = Product.ConfigAlphaBodenHeat;
                    alphaInnenCool = Product.ConfigAlphaDeckeCool;
                    alphaAussenCool = Product.ConfigAlphaBodenCool;
                    break;

                default:
                    alphaInnenHeat = Product.ConfigAlphaWandHeat;
                    alphaAussenHeat = Product.ConfigAlphaWandHeat;
                    alphaInnenCool = Product.ConfigAlphaWandCool;
                    alphaAussenCool = Product.ConfigAlphaWandCool;
                    break;
            }

            double atmt = ModulKlimaBoden20Product.ConfigAtmt;
            double b = ModulKlimaBoden20Product.ConfigB;
            double c = ModulKlimaBoden20Product.ConfigC;
            double alpha0 = ModulKlimaBoden20Product.ConfigAlpha0;
            double su0 = ModulKlimaBoden20Product.ConfigSu0;
            double lambdaU0 = ModulKlimaBoden20Product.ConfigLambdaU0;
            double rLambdaDecke = ModulKlimaBoden20Product.ConfigRLambdaDecke;
            double rLambdaPutz = ModulKlimaBoden20Product.ConfigRLambdaPutz;
            double rAlphaDeckeDh = 1 / alphaAussenHeat; /* Wärmeübergang Decke bei Heizung */
            double rAlphaDeckeDk = 1 / alphaAussenCool; /* Wärmeübergang Decke bei Kühlung */

            double rLambdaB = this.ModulKlimaBoden20Product.PlannedFloorConstruction == null ? 0 : this.ModulKlimaBoden20Product.PlannedFloorConstruction.RValue;           
            double rLambdaIns = this.ModulKlimaBoden20Product.PlannedInsulationConstruction == null ? 0 : this.ModulKlimaBoden20Product.PlannedInsulationConstruction.RValue;

            double su = 0.002;
            double lambdaE = 60;
            if (this.ModulKlimaBoden20Product.PlannedFloorConstruction != null)
            {
                ConstructionTypeManager ctm = ConstructionTypeManager.Instance;
                ConstructionType ctEstrichS = ctm.GetConstructionTypeById(ConstructionTypeManager.CT_STD_ESTRICH);
                ConstructionType ctEstrichU = ctm.GetConstructionTypeById(ConstructionTypeManager.CT_USER_ESTRICH);
                ConstructionType ctStahlS = ctm.GetConstructionTypeById(ConstructionTypeManager.CT_STD_STAHL);
                ConstructionType ctStahlU = ctm.GetConstructionTypeById(ConstructionTypeManager.CT_USER_STAHL);
                ConstructionType ctTrkEstrS = ctm.GetConstructionTypeById(ConstructionTypeManager.CT_STD_TRK_ESTRICH);
                ConstructionType ctTrkEstrU = ctm.GetConstructionTypeById(ConstructionTypeManager.CT_USER_TRK_ESTRICH);

                ConstructionType ct = this.ModulKlimaBoden20Product.PlannedFloorConstruction.Type;
                if (ct == ctEstrichS || ct == ctEstrichU)
                {
                    su = 0.03;
                    lambdaE = 1.2;
                }
                else if (ct == ctTrkEstrS || ct == ctTrkEstrU)
                {
                    su = 0.02;
                    lambdaE = 0.33;
                }
                else if (ct == ctStahlS || ct == ctStahlU)
                {
                    su = 0.002;
                    lambdaE = 60;
                }


            }
            double lambdaU = lambdaE;

            { // Heizlastberechnung
                double leistungsFaktor = ModulKlimaBoden20Product.ConfigLeistungsFaktorHeizen;
                double distributorVorlaufTemp;
                double distributorRuecklaufTemp;
                this.ModulKlimaBoden20Product.GetHeatFlow(out distributorVorlaufTemp, out distributorRuecklaufTemp);
                this.c_thetaVHeat = distributorVorlaufTemp;
                this.c_thetaRHeat = distributorRuecklaufTemp;
                this.c_thetaVHeat = this.c_thetaVHeat - (this.c_thetaVHeat - this.c_thetaRHeat) * this.vorlaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
                this.c_thetaRHeat = this.c_thetaRHeat + (this.c_thetaVHeat - this.c_thetaRHeat) * this.ruecklaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
                if (c_thetaVHeat.Equals(double.NaN) || c_thetaRHeat.Equals(double.NaN))
                {
                    this.c_qHeatPerSqm = 0;
                    this.c_massenstromHeat = 0;
                    this.c_druckverlustHeat = 0;
                }
                else
                {

                    double dTheta = en1264.Heizmitteluebertemperatur(this.c_thetaVHeat, this.c_thetaRHeat, this.ModulKlimaBoden20Product.AssociatedRoom.RoomHeatTemperature);

                    double au = en1264.auFlaeche(alpha0, alphaInnenHeat, su0, lambdaU0, su, lambdaE);
                    double ab = en1264.abFlaeche(b, au, atmt, rLambdaB);
                    this.c_qHeatPerSqm = en1264.WaermestromDichteFlaeche(b, ab, atmt, au, dTheta) * leistungsFaktor;

                    double qU = en1264.WaermeverlustAussen(alphaInnenHeat, rLambdaB, su, lambdaU, rAlphaDeckeDh, rLambdaIns, rLambdaDecke, rLambdaPutz, this.c_qHeatPerSqm, this.ModulKlimaBoden20Product.AssociatedRoom.RoomHeatTemperature, this.ModulKlimaBoden20Product.PlannedRoomTemperatureBelowHeat);

                    // hydraulische Berechnung
                    this.c_Qh2oHeat = (this.c_qHeatPerSqm + qU) * this.HeatAreaForCalculation;            // gesamte aufgenommene Leistung berechnen
                    //                                                                           // gesamten Druckverlust berechnen

                    foreach (ConnectionPipe cp in this.plannedProduct.Product.PlannedConnectionPipes)
                    {
                        if (this.nrOfCircuit == 0 || !cp.OnlyFirst)
                        {
                            double heatLoad;
                            double qH2o;
                            cp.CalculateHeatLoad(out heatLoad, out qH2o, this.NrOfCircuit);
                            this.c_Qh2oHeat += qH2o;
                        }
                    }
                    double totalQh2o = this.c_Qh2oHeat;
                    CircuitConnection cc = this.plannedProduct.Product.GetCircuitConnected(this.nrOfCircuit);
                    if (cc != null)
                    {
                        totalQh2o += cc.OtherCircuit.C_Qh2oHeat;
                    }
                    cc = this.plannedProduct.Product.GetCircuitInverseConnected(this.nrOfCircuit);
                    if (cc != null)
                    {
                        totalQh2o += cc.OtherCircuit.C_Qh2oHeat;
                    }

                    this.c_massenstromHeat = en1264.Massenstrom(totalQh2o, c, distributorVorlaufTemp - distributorRuecklaufTemp);
                    this.c_flussGeschwindigkeitHeat = en1264.FlussGeschwindigkeit(C_DurchflussHeat, Product.rundrohr21mmInnenA);

                    this.c_druckverlustHeat = 0;
                    foreach (ModulKlimaBoden20SubArea subArea in this.subAreas)
                    {
                        this.c_druckverlustHeat += subArea.Druckverlust(this.c_massenstromHeat);
                    }
                    foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes)
                    {
                        if (this.nrOfCircuit == 0 || !cp.OnlyFirst)
                        {
                            this.c_druckverlustHeat += cp.CalculateDruckverlust(this.c_massenstromHeat);
                        }
                    }

                    this.c_floorTempHeat = en1264.OberflaechenTemperatur(this.c_qHeatPerSqm, alphaInnenHeat, this.ModulKlimaBoden20Product.AssociatedRoom.RoomHeatTemperature);
                }
            }
            { // Kühllastberechnung
                double leistungsFaktor = ModulKlimaBoden20Product.ConfigLeistungsFaktorKuehlen;
                double distributorVorlaufTemp;
                double distributorRuecklaufTemp;
                this.ModulKlimaBoden20Product.GetCoolFlow(out distributorVorlaufTemp, out distributorRuecklaufTemp);
                this.c_thetaVCool = distributorVorlaufTemp;
                this.c_thetaRCool = distributorRuecklaufTemp;
                this.c_thetaVCool = this.c_thetaVCool - (this.c_thetaVCool - this.c_thetaRCool) * this.vorlaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
                this.c_thetaRCool = this.c_thetaRCool + (this.c_thetaVCool - this.c_thetaRCool) * this.ruecklaufNotIsolated / (this.PipeLengthWithoutConnections + this.vorlaufNotIsolated + this.ruecklaufNotIsolated);
                if (c_thetaVCool.Equals(double.NaN) || c_thetaRCool.Equals(double.NaN))
                {
                    this.c_qCoolPerSqm = 0;
                    this.c_massenstromCool = 0;
                    this.c_druckverlustCool = 0;
                }
                else
                {

                    double dTheta = en1264.Heizmitteluebertemperatur(this.c_thetaVCool, this.c_thetaRCool, this.ModulKlimaBoden20Product.AssociatedRoom.RoomCoolTemperature);

                    double au = en1264.auFlaeche(alpha0, alphaInnenCool, su0, lambdaU0, su, lambdaE);
                    double ab = en1264.abFlaeche(b, au, atmt, rLambdaB);
                    this.c_qCoolPerSqm = en1264.WaermestromDichteFlaeche(b, ab, atmt, au, dTheta) * leistungsFaktor;

                    double qU = en1264.WaermeverlustAussen(alphaInnenCool, rLambdaB, su, lambdaU, rAlphaDeckeDk, rLambdaIns, rLambdaDecke, rLambdaPutz, this.c_qCoolPerSqm, this.ModulKlimaBoden20Product.AssociatedRoom.RoomCoolTemperature, this.ModulKlimaBoden20Product.PlannedRoomTemperatureBelowCool);

                    // hydraulische Berechnung
                    this.c_Qh2oCool = (this.c_qCoolPerSqm + qU) * this.HeatAreaForCalculation;            // gesamte aufgenommene Leistung berechnen
                    //                                                                           // gesamten Druckverlust berechnen

                    foreach (ConnectionPipe cp in this.plannedProduct.Product.PlannedConnectionPipes)
                    {
                        if (this.nrOfCircuit == 0 || !cp.OnlyFirst)
                        {
                            double coolLoad;
                            double qH2o;
                            cp.CalculateCoolLoad(out coolLoad, out qH2o, this.NrOfCircuit);
                            this.c_Qh2oCool -= qH2o;
                        }
                    }
                    double totalQh2o = this.c_Qh2oCool;
                    CircuitConnection cc = this.plannedProduct.Product.GetCircuitConnected(this.nrOfCircuit);
                    if (cc != null)
                    {
                        totalQh2o += cc.OtherCircuit.C_Qh2oCool;
                    }
                    cc = this.plannedProduct.Product.GetCircuitInverseConnected(this.nrOfCircuit);
                    if (cc != null)
                    {
                        totalQh2o += cc.OtherCircuit.C_Qh2oCool;
                    }

                    this.c_massenstromCool = en1264.Massenstrom(totalQh2o, c, distributorVorlaufTemp - distributorRuecklaufTemp);
                    this.c_flussGeschwindigkeitCool = en1264.FlussGeschwindigkeit(C_DurchflussCool, Product.rundrohr21mmInnenA);

                    this.c_druckverlustCool = 0;
                    foreach (ModulKlimaBoden20SubArea subArea in this.subAreas)
                    {
                        this.c_druckverlustCool += subArea.Druckverlust(this.c_massenstromCool);
                    }
                    foreach (ConnectionPipe cp in this.PlannedProduct.Product.PlannedConnectionPipes)
                    {
                        if (this.nrOfCircuit == 0 || !cp.OnlyFirst)
                        {
                            this.c_druckverlustCool += cp.CalculateDruckverlust(this.c_massenstromCool);
                        }
                    }

                    this.c_floorTempCool = en1264.OberflaechenTemperatur(this.c_qCoolPerSqm, alphaInnenCool, this.ModulKlimaBoden20Product.AssociatedRoom.RoomCoolTemperature);
                }
            }
        }

        internal override void FinalizeLoading()
        {
            base.FinalizeLoading();
            foreach (ModulKlimaBoden20SubArea sa in this.SubAreas)
            {
                sa.FinalizeLoading();
            }
        }

        public override double CircuitArea
        {
            get { return this.CoveredArea; }
        }

        public bool ContainsModul(KlimaFlaechenModul modul)
        {
            if (this.subAreas != null)
            {
                foreach (ModulKlimaBoden20SubArea subArea in this.subAreas)
                {
                    if (subArea.ContainsModul(modul))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int CountModules()
        {
            int count = 0;
            foreach (ModulKlimaBoden20SubArea subArea in this.subAreas)
            {
                count += subArea.CountModules();
            }
            return count;
        }

        public ModulKlimaBoden20SubArea GetSubareaForModul(KlimaFlaechenModul modul, out int index)
        {
            index = 0;
            foreach (ModulKlimaBoden20SubArea sa in this.subAreas)
            {
                if (sa.ContainsModul(modul))
                {
                    return sa;
                }
                index++;
            }
            index = -1;
            return null;
        }

        [XmlIgnore]
        public Color CircuitColor
        {
            get { return this.circuitColor; }
            set { this.circuitColor = value; }
        }

        // Color cannot be serialized!!!
        // quick workaround to serialize it nevertheless
        public int CircuitColorR
        {
            get { return this.circuitColor.R; }
            set { this.circuitColor = Color.FromArgb(value, this.circuitColor.G, this.circuitColor.B); }
        }
        public int CircuitColorG
        {
            get { return this.circuitColor.G; }
            set { this.circuitColor = Color.FromArgb(this.circuitColor.R, value, this.circuitColor.B); }
        }
        public int CircuitColorB
        {
            get { return this.circuitColor.B; }
            set { this.circuitColor = Color.FromArgb(this.circuitColor.R, this.circuitColor.G, value); }
        }

        public List<KlimaFlaechenSubAreaVerbindung> Links
        {
            get { return this.verbindungen; }
            set { this.verbindungen = value; }
        }

        public List<KlimaFlaechenModul> GetAllLinkedModules(KlimaFlaechenModul referenceModul)
        {
            List<KlimaFlaechenModul> linkedModules = new List<KlimaFlaechenModul>();
            linkedModules.Add(referenceModul);
            List<KlimaFlaechenModul> nextModules = this.GetNextLinkedModules(referenceModul);
            nextModules.AddRange(this.GetPreviousLinkedModules(referenceModul));
            while (nextModules.Count > 0)
            {
                KlimaFlaechenModul nextModule = nextModules[0];
                nextModules.RemoveAt(0);
                if (!linkedModules.Contains(nextModule))
                {
                    linkedModules.Add(nextModule);
                    nextModules.AddRange(this.GetNextLinkedModules(nextModule));
                    nextModules.AddRange(this.GetPreviousLinkedModules(nextModule));
                }
            }
            return linkedModules;
        }

        public IKlimaFlaechenVerbindung GetNextLink(KlimaFlaechenModul modul)
        {
            if (this.verbindungen != null)
            {
                foreach (KlimaFlaechenSubAreaVerbindung link in this.verbindungen)
                {
                    if (link.Start.Contains(modul))
                    {
                        return link;
                    }
                }
            }

            foreach (ModulKlimaBoden20SubArea sa in this.SubAreas)
            {
                foreach (KlimaFlaechenList row in sa.Rows)
                {
                    foreach (KlimaFlaechenModulVerbindung link in row.Links)
                    {
                        if (link.Start == modul)
                        {
                            return link;
                        }
                    }
                }
            }
            return null;
        }

        public IKlimaFlaechenVerbindung GetPreviousLink(KlimaFlaechenModul modul)
        {
            if (this.verbindungen != null)
            {
                foreach (KlimaFlaechenSubAreaVerbindung link in this.verbindungen)
                {
                    if (link.End.Contains(modul))
                    {
                        return link;
                    }
                }
            }

            foreach (ModulKlimaBoden20SubArea sa in this.SubAreas)
            {
                foreach (KlimaFlaechenList row in sa.Rows)
                {
                    foreach (KlimaFlaechenModulVerbindung link in row.Links)
                    {
                        if (link.End == modul)
                        {
                            return link;
                        }
                    }
                }
            }
            return null;
        }

        private List<KlimaFlaechenModul> GetNextLinkedModules(KlimaFlaechenModul referenceModul)
        {
            List<KlimaFlaechenModul> nextModules = new List<KlimaFlaechenModul>();
            foreach (ModulKlimaBoden20SubArea sa in this.subAreas)
            {
                foreach (KlimaFlaechenList row in sa.Rows)
                {
                    if (row.Links != null)
                    {
                        foreach (KlimaFlaechenModulVerbindung link in row.Links)
                        {
                            if (link.Start == referenceModul)
                            {
                                nextModules.Add(link.End);
                            }
                        }
                    }
                }
            }

            if (this.Links != null)
            {
                foreach (KlimaFlaechenSubAreaVerbindung saLink in this.Links)
                {
                    if (saLink.Start.Contains(referenceModul))
                    {
                        nextModules.AddRange(saLink.End);
                    }
                }
            }

            return nextModules;
        }

        private List<KlimaFlaechenModul> GetPreviousLinkedModules(KlimaFlaechenModul referenceModul)
        {
            List<KlimaFlaechenModul> prevModules = new List<KlimaFlaechenModul>();
            foreach (ModulKlimaBoden20SubArea sa in this.subAreas)
            {
                foreach (KlimaFlaechenList row in sa.Rows)
                {
                    if (row.Links != null)
                    {
                        foreach (KlimaFlaechenModulVerbindung link in row.Links)
                        {
                            if (link.End == referenceModul)
                            {
                                prevModules.Add(link.Start);
                            }
                        }
                    }
                }
            }

            if (this.Links != null)
            {
                foreach (KlimaFlaechenSubAreaVerbindung saLink in this.Links)
                {
                    if (saLink.End.Contains(referenceModul))
                    {
                        prevModules.AddRange(saLink.Start);
                    }
                }
            }

            return prevModules;
        }

        public int GetDistributorConnectionIndex(bool checkVorlauf, bool checkRuecklauf)
        {
            if (this.Links != null)
            {
                foreach (KlimaFlaechenSubAreaVerbindung link in this.Links)
                {
                    if ((checkRuecklauf && link.EndConnectedToAnbindung) ||
                        (checkVorlauf && link.StartConnectedToAnbindung))
                    {
                        return link.DistributorIndex;
                    }
                }
            }
            foreach (ModulKlimaBoden20SubArea sa in this.subAreas)
            {
                foreach (KlimaFlaechenList row in sa.Rows)
                {
                    if (row.Links != null)
                    {
                        foreach (KlimaFlaechenModulVerbindung link in row.Links)
                        {
                            if ((checkRuecklauf && link.EndConnectedToAnbindung) ||
                                (checkVorlauf && link.StartConnectedToAnbindung))
                            {
                                return link.DistributorIndex;
                            }
                        }
                    }
                }
            }
            return -1;
        }

        public bool IsVorlaufConnected(KlimaFlaechenList row)
        {

            if (this.Links != null)
            {
                foreach (KlimaFlaechenSubAreaVerbindung link in this.Links)
                {
                    if (link.GetEndRows().Contains(row))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsRuecklaufConnected(KlimaFlaechenList row)
        {
            if (this.Links != null)
            {
                foreach (KlimaFlaechenSubAreaVerbindung link in this.Links)
                {
                    if (link.GetStartRows().Contains(row))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #region Graphical Materials
        public int GetRequiredWinkel(double measure)
        {
            int result = 0;
            foreach (ModulKlimaBoden20SubArea sa in this.SubAreas)
            {
                result += sa.GetRequiredWinkel();
            }
            foreach (KlimaFlaechenSubAreaVerbindung link in this.Links)
            {
                result += link.GetRequiredWinkel(measure);
            }
            return result;
        }

        public int GetRequiredTStuecke()
        {
            int result = 0;
            int linkResult;
            foreach (KlimaFlaechenSubAreaVerbindung link in this.Links)
            {
                linkResult = link.GetRequiredTStuecke();
                result += linkResult;
            }
            return result;
        }
        #endregion Graphical Materials
    }
}
