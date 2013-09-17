using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Europlan.Common {
    public class AsyncProductCalculator {

        private static AsyncProductCalculator instance;

        private BackgroundWorker worker;

        private Stack<CalculationArguments> calcStack = new Stack<CalculationArguments>();

        private CalculationArguments currentCalculation = null;

        private AsyncProductCalculator() {
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }

        public static AsyncProductCalculator Instance {
            get {
#warning TODO semaphore
                if (instance == null) {
                    instance = new AsyncProductCalculator();
                }
                return instance;
            }
        }

        public class ProductCalculationFinishedArgs : EventArgs {
            private Product product;
            private bool calculationOk;
            private int token;

            public ProductCalculationFinishedArgs(Product product, bool calculationOk, int token) {
                this.product = product;
                this.calculationOk = calculationOk;
                this.token = token;
            }

            public Product Product {
                get { return this.product; }
            }

            public bool CalcuationOk {
                get { return this.calculationOk; }
            }

            public int Token {
                get { return this.token; }
            }
        }

        private class CalculationArguments {
            private Product product;
            private double requestedHeatLoad;
            private double requestedCoolLoad;
            private bool calculateHeat;
            private bool calculateCool;
            private bool variableSpreizung;
            private int token;

            private static int currentToken = 0;

            public CalculationArguments(Product product, double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, bool variableSpreizung) {
                this.product = product;
                this.requestedHeatLoad = requestedHeatLoad;
                this.requestedCoolLoad = requestedCoolLoad;
                this.calculateHeat = calculateHeat;
                this.calculateCool = calculateCool;
                this.variableSpreizung = variableSpreizung;
#warning TODO semaphore
                this.token = currentToken++;
            }

            public Product Product {
                get { return this.product; }
            }

            public double RequestHeatLoad {
                get { return this.requestedHeatLoad; }
            }

            public double RequestCoolLoad {
                get { return this.requestedCoolLoad; }
            }

            public bool CalculateHeat {
                get { return this.calculateHeat; }
            }

            public bool CalculateCool {
                get { return this.calculateCool; }
            }

            public bool VariableSpreizung {
                get { return this.variableSpreizung; }
            }

            public int Token {
                get { return this.token; }
            }
        }

        private event EventHandler<ProductCalculationFinishedArgs> calculationFinished;

        public event EventHandler<ProductCalculationFinishedArgs> CalculationFinished {
            add { calculationFinished += value; }
            remove { calculationFinished -= value; }
        }

        public int CalculateProduct(Product product, double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, bool variableSpreizung) {
#warning TODO semaphore
            CalculationArguments args = new CalculationArguments(product, requestedHeatLoad, requestedCoolLoad, calculateHeat, calculateCool, variableSpreizung);
            calcStack.Push(args);
            if (!worker.IsBusy) {
                worker.RunWorkerAsync();
            }
            return args.Token;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e) {
#warning TODO semaphore
            currentCalculation = calcStack.Pop();
            if (currentCalculation != null) {
                currentCalculation.Product.ConfigureProduct(currentCalculation.RequestHeatLoad, currentCalculation.RequestCoolLoad, currentCalculation.CalculateHeat, currentCalculation.CalculateCool, currentCalculation.VariableSpreizung);
            }
            e.Result = currentCalculation;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (!e.Cancelled) {
                CalculationArguments args = (CalculationArguments)e.Result;
                this.OnCalculationFinished(args.Product, true, args.Token);
            }
            if (calcStack.Count > 0) {
                worker.RunWorkerAsync();
            }
        }

        private void OnCalculationFinished(Product product, bool calculationOk, int token) {
            if (this.calculationFinished != null) {
                this.calculationFinished(this, new ProductCalculationFinishedArgs(product, calculationOk, token));
            }
        }

        public int CurrentCalculationToken {
            get { return this.currentCalculation != null ? this.currentCalculation.Token : -1; }
        }
    }
}
