using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Europlan.Common {
    public class AsyncProductCalculator {

        private static AsyncProductCalculator instance;
        private static readonly object instanceLock = new object();

        private BackgroundWorker worker;

        //private Stack<CalculationArguments> calcStack = new Stack<CalculationArguments>();
        private Queue<CalculationArguments> calcQueue = new Queue<CalculationArguments>();

        private CalculationArguments currentCalculation = null;

        private AsyncProductCalculator() {
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = false;
            worker.WorkerReportsProgress = false;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }

        public static AsyncProductCalculator Instance {
            get {
                lock (instanceLock) {
                    if (instance == null) {
                        instance = new AsyncProductCalculator();
                    }
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
            private static object tokenLock = new object();

            public CalculationArguments(Product product, double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, bool variableSpreizung) {
                this.product = product;
                this.requestedHeatLoad = requestedHeatLoad;
                this.requestedCoolLoad = requestedCoolLoad;
                this.calculateHeat = calculateHeat;
                this.calculateCool = calculateCool;
                this.variableSpreizung = variableSpreizung;
                lock (tokenLock) {
                    this.token = currentToken++;
                }
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

        private class CalculationResult {
            private Product product;
            private int token;
            private bool calculationOk;

            public CalculationResult(Product product, int token, bool calculationOk) {
                this.product = product;
                this.token = token;
                this.calculationOk = calculationOk;
            }

            public Product Product {
                get { return this.product; }
            }

            public int Token {
                get { return this.token; }
            }

            public bool CalculationOk {
                get { return this.calculationOk; }
            }
        }

        private event EventHandler<ProductCalculationFinishedArgs> calculationFinished;

        public event EventHandler<ProductCalculationFinishedArgs> CalculationFinished {
            add { calculationFinished += value; }
            remove { calculationFinished -= value; }
        }

        public int CalculateProduct(Product product, double requestedHeatLoad, double requestedCoolLoad, bool calculateHeat, bool calculateCool, bool variableSpreizung) {
            CalculationArguments args = new CalculationArguments(product, requestedHeatLoad, requestedCoolLoad, calculateHeat, calculateCool, variableSpreizung);
            lock (calcQueue) {
                System.Console.WriteLine("adding product to calculate: " + product.FullName + " (" + args.Token + ")");
                calcQueue.Enqueue(args);
                if (!worker.IsBusy) {
                    worker.RunWorkerAsync();
                }
            }
            return args.Token;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e) {
            lock (calcQueue) {
                if (calcQueue.Count == 0) {
                    return;
                }
                currentCalculation = calcQueue.Dequeue();
                // if multiple consecutive calculations for the same product are on the stack skip all but the last one
                while (calcQueue.Count > 0 && calcQueue.Peek().Product == currentCalculation.Product) {
                    currentCalculation = calcQueue.Dequeue();
                }
            }

            lock (currentCalculation.Product.CalculationLock) {
                System.Console.WriteLine("calculating product: " + currentCalculation.Product.FullName + " (" + currentCalculation.Token + ")");
                e.Result = new CalculationResult(currentCalculation.Product, currentCalculation.Token, currentCalculation.Product.ConfigureProduct(currentCalculation.RequestHeatLoad, currentCalculation.RequestCoolLoad, currentCalculation.CalculateHeat, currentCalculation.CalculateCool, currentCalculation.VariableSpreizung));
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (!e.Cancelled) {
                CalculationResult result = (CalculationResult)e.Result;
                this.OnCalculationFinished(result.Product, result.CalculationOk, result.Token);
            }
            lock (calcQueue) {
                System.Console.WriteLine("finished calculation for product: " + ((CalculationResult)e.Result).Product.FullName + " (" + ((CalculationResult)e.Result).Token + ")");
                if (calcQueue.Count > 0) {
                    worker = new BackgroundWorker();
                    worker.WorkerSupportsCancellation = false;
                    worker.WorkerReportsProgress = false;
                    worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                    worker.RunWorkerAsync();
                }
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
