namespace Europlan.Common {
    partial class PartitionSystemForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.cmbMaximaldurchfluss = new System.Windows.Forms.ComboBox();
            this.lblMaximaldurchfluss = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAnschluss = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbAnschlussHollaender = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkFlansch = new System.Windows.Forms.CheckBox();
            this.chkEinbauschrank = new System.Windows.Forms.CheckBox();
            this.numMaxCircuits = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbCircuit = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lstError = new System.Windows.Forms.ListView();
            this.defaultColumn = new System.Windows.Forms.ColumnHeader();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCircuits)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbMaximaldurchfluss
            // 
            this.cmbMaximaldurchfluss.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMaximaldurchfluss.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMaximaldurchfluss.FormattingEnabled = true;
            this.cmbMaximaldurchfluss.Location = new System.Drawing.Point(175, 45);
            this.cmbMaximaldurchfluss.Name = "cmbMaximaldurchfluss";
            this.cmbMaximaldurchfluss.Size = new System.Drawing.Size(568, 21);
            this.cmbMaximaldurchfluss.TabIndex = 78;
            this.cmbMaximaldurchfluss.SelectedIndexChanged += new System.EventHandler(this.cmbMaximaldurchfluss_SelectedIndexChanged);
            // 
            // lblMaximaldurchfluss
            // 
            this.lblMaximaldurchfluss.Location = new System.Drawing.Point(6, 43);
            this.lblMaximaldurchfluss.Name = "lblMaximaldurchfluss";
            this.lblMaximaldurchfluss.Size = new System.Drawing.Size(163, 23);
            this.lblMaximaldurchfluss.TabIndex = 79;
            this.lblMaximaldurchfluss.Text = "Maximaldurchfluß:";
            this.lblMaximaldurchfluss.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAnschluss);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.cmbAnschlussHollaender);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.chkFlansch);
            this.groupBox1.Controls.Add(this.chkEinbauschrank);
            this.groupBox1.Controls.Add(this.numMaxCircuits);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmbCircuit);
            this.groupBox1.Controls.Add(this.lblMaximaldurchfluss);
            this.groupBox1.Controls.Add(this.cmbMaximaldurchfluss);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(749, 186);
            this.groupBox1.TabIndex = 80;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Verteilereinstellungen";
            // 
            // chkAnschluss
            // 
            this.chkAnschluss.AutoSize = true;
            this.chkAnschluss.Location = new System.Drawing.Point(175, 133);
            this.chkAnschluss.Name = "chkAnschluss";
            this.chkAnschluss.Size = new System.Drawing.Size(137, 17);
            this.chkAnschluss.TabIndex = 86;
            this.chkAnschluss.Text = "Lange Anschlussbögen";
            this.chkAnschluss.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 106);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(163, 23);
            this.label11.TabIndex = 90;
            this.label11.Text = "Zubehör:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbAnschlussHollaender
            // 
            this.cmbAnschlussHollaender.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAnschlussHollaender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAnschlussHollaender.FormattingEnabled = true;
            this.cmbAnschlussHollaender.Location = new System.Drawing.Point(175, 72);
            this.cmbAnschlussHollaender.Name = "cmbAnschlussHollaender";
            this.cmbAnschlussHollaender.Size = new System.Drawing.Size(568, 21);
            this.cmbAnschlussHollaender.TabIndex = 83;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 70);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(163, 23);
            this.label10.TabIndex = 89;
            this.label10.Text = "Anschlußholländer:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkFlansch
            // 
            this.chkFlansch.AutoSize = true;
            this.chkFlansch.Location = new System.Drawing.Point(175, 110);
            this.chkFlansch.Name = "chkFlansch";
            this.chkFlansch.Size = new System.Drawing.Size(119, 17);
            this.chkFlansch.TabIndex = 84;
            this.chkFlansch.Text = "Flanschkugelhähne";
            this.chkFlansch.UseVisualStyleBackColor = true;
            // 
            // chkEinbauschrank
            // 
            this.chkEinbauschrank.AutoSize = true;
            this.chkEinbauschrank.Location = new System.Drawing.Point(378, 110);
            this.chkEinbauschrank.Name = "chkEinbauschrank";
            this.chkEinbauschrank.Size = new System.Drawing.Size(97, 17);
            this.chkEinbauschrank.TabIndex = 85;
            this.chkEinbauschrank.Text = "Einbauschrank";
            this.chkEinbauschrank.UseVisualStyleBackColor = true;
            // 
            // numMaxCircuits
            // 
            this.numMaxCircuits.Location = new System.Drawing.Point(175, 156);
            this.numMaxCircuits.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numMaxCircuits.Name = "numMaxCircuits";
            this.numMaxCircuits.Size = new System.Drawing.Size(51, 20);
            this.numMaxCircuits.TabIndex = 87;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 23);
            this.label5.TabIndex = 88;
            this.label5.Text = "max. Heizkreise:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbCircuit
            // 
            this.cmbCircuit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCircuit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCircuit.FormattingEnabled = true;
            this.cmbCircuit.Location = new System.Drawing.Point(175, 18);
            this.cmbCircuit.Name = "cmbCircuit";
            this.cmbCircuit.Size = new System.Drawing.Size(568, 21);
            this.cmbCircuit.TabIndex = 81;
            this.cmbCircuit.SelectedIndexChanged += new System.EventHandler(this.cmbCircuit_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 23);
            this.label4.TabIndex = 82;
            this.label4.Text = "Regelkreis:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lstError
            // 
            this.lstError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstError.BackColor = System.Drawing.SystemColors.Control;
            this.lstError.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstError.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.defaultColumn});
            this.lstError.FullRowSelect = true;
            this.lstError.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstError.LabelWrap = false;
            this.lstError.Location = new System.Drawing.Point(12, 204);
            this.lstError.Name = "lstError";
            this.lstError.ShowGroups = false;
            this.lstError.Size = new System.Drawing.Size(749, 78);
            this.lstError.TabIndex = 81;
            this.lstError.UseCompatibleStateImageBehavior = false;
            this.lstError.View = System.Windows.Forms.View.Details;
            this.lstError.Visible = false;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.Location = new System.Drawing.Point(632, 288);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(129, 23);
            this.btnOk.TabIndex = 82;
            this.btnOk.Text = "System Aufteilen";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(497, 288);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(129, 23);
            this.btnCancel.TabIndex = 83;
            this.btnCancel.Text = "Abbrechen";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PartitionSystemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 323);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lstError);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PartitionSystemForm";
            this.Text = "System Aufteilen";
            this.Load += new System.EventHandler(this.PartitionSystemForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PartitionSystemForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCircuits)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbMaximaldurchfluss;
        private System.Windows.Forms.Label lblMaximaldurchfluss;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbCircuit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkAnschluss;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbAnschlussHollaender;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkFlansch;
        private System.Windows.Forms.CheckBox chkEinbauschrank;
        private System.Windows.Forms.NumericUpDown numMaxCircuits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView lstError;
        private System.Windows.Forms.ColumnHeader defaultColumn;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}