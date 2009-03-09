namespace Europlan.Application {
	partial class MainForm {
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
			this.components = new System.ComponentModel.Container();
			this.UpdateApplication = new Janus.Windows.UI.CommandBars.UICommand("UpdateApplication");
			this.Info = new Janus.Windows.UI.CommandBars.UICommand("Info");
			this.Beenden = new Janus.Windows.UI.CommandBars.UICommand("Beenden");
			this.Update1 = new Janus.Windows.UI.CommandBars.UICommand("UpdateApplication");
			this.Info1 = new Janus.Windows.UI.CommandBars.UICommand("Info");
			this.Hilfe = new Janus.Windows.UI.CommandBars.UICommand("Hilfe");
			this.Beenden1 = new Janus.Windows.UI.CommandBars.UICommand("Beenden");
			this.Datei = new Janus.Windows.UI.CommandBars.UICommand("Datei");
			this.BottomRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
			this.RightRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
			this.LeftRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
			this.uiCommandBar1 = new Janus.Windows.UI.CommandBars.UICommandBar();
			this.Hilfe1 = new Janus.Windows.UI.CommandBars.UICommand("Hilfe");
			this.Datei1 = new Janus.Windows.UI.CommandBars.UICommand("Datei");
			this.TopRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
			this.uiCommandManager1 = new Janus.Windows.UI.CommandBars.UICommandManager(this.components);
			this.TopRebar2 = new Janus.Windows.UI.CommandBars.UIRebar();
			this.Datei2 = new Janus.Windows.UI.CommandBars.UICommand("Datei");
			this.Hilfe2 = new Janus.Windows.UI.CommandBars.UICommand("Hilfe");
			((System.ComponentModel.ISupportInitialize)(this.BottomRebar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.RightRebar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LeftRebar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiCommandBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TopRebar1)).BeginInit();
			this.TopRebar1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiCommandManager1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TopRebar2)).BeginInit();
			this.SuspendLayout();
			// 
			// UpdateApplication
			// 
			this.UpdateApplication.Key = "UpdateApplication";
			this.UpdateApplication.Name = "UpdateApplication";
			this.UpdateApplication.Text = "&Auf Aktualisierungen prüfen...";
			this.UpdateApplication.Click += new Janus.Windows.UI.CommandBars.CommandEventHandler(this.Update_Click);
			// 
			// Info
			// 
			this.Info.Key = "Info";
			this.Info.Name = "Info";
			this.Info.Shortcut = System.Windows.Forms.Shortcut.F1;
			this.Info.Text = "&Info";
			// 
			// Beenden
			// 
			this.Beenden.Key = "Beenden";
			this.Beenden.Name = "Beenden";
			this.Beenden.Shortcut = System.Windows.Forms.Shortcut.AltF4;
			this.Beenden.Text = "&Beenden";
			this.Beenden.Click += new Janus.Windows.UI.CommandBars.CommandEventHandler(this.Beenden_Click);
			// 
			// Update1
			// 
			this.Update1.Key = "UpdateApplication";
			this.Update1.Name = "Update1";
			// 
			// Info1
			// 
			this.Info1.Key = "Info";
			this.Info1.Name = "Info1";
			// 
			// Hilfe
			// 
			this.Hilfe.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.Info1,
            this.Update1});
			this.Hilfe.Key = "Hilfe";
			this.Hilfe.Name = "Hilfe";
			this.Hilfe.Text = "&Hilfe";
			// 
			// Beenden1
			// 
			this.Beenden1.Key = "Beenden";
			this.Beenden1.Name = "Beenden1";
			// 
			// Datei
			// 
			this.Datei.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.Beenden1});
			this.Datei.Key = "Datei";
			this.Datei.Name = "Datei";
			this.Datei.Text = "&Datei";
			// 
			// BottomRebar1
			// 
			this.BottomRebar1.CommandManager = this.uiCommandManager1;
			this.BottomRebar1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.BottomRebar1.Location = new System.Drawing.Point(0, 489);
			this.BottomRebar1.Name = "BottomRebar1";
			this.BottomRebar1.Size = new System.Drawing.Size(884, 0);
			this.BottomRebar1.TabIndex = 3;
			// 
			// RightRebar1
			// 
			this.RightRebar1.CommandManager = this.uiCommandManager1;
			this.RightRebar1.Dock = System.Windows.Forms.DockStyle.Right;
			this.RightRebar1.Location = new System.Drawing.Point(884, 24);
			this.RightRebar1.Name = "RightRebar1";
			this.RightRebar1.Size = new System.Drawing.Size(0, 465);
			this.RightRebar1.TabIndex = 2;
			// 
			// LeftRebar1
			// 
			this.LeftRebar1.CommandManager = this.uiCommandManager1;
			this.LeftRebar1.Dock = System.Windows.Forms.DockStyle.Left;
			this.LeftRebar1.Location = new System.Drawing.Point(0, 24);
			this.LeftRebar1.Name = "LeftRebar1";
			this.LeftRebar1.Size = new System.Drawing.Size(0, 465);
			this.LeftRebar1.TabIndex = 1;
			// 
			// uiCommandBar1
			// 
			this.uiCommandBar1.AllowClose = Janus.Windows.UI.InheritableBoolean.False;
			this.uiCommandBar1.AllowCustomize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiCommandBar1.CommandBarType = Janus.Windows.UI.CommandBars.CommandBarType.Menu;
			this.uiCommandBar1.CommandManager = this.uiCommandManager1;
			this.uiCommandBar1.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.Datei2,
            this.Hilfe2});
			this.uiCommandBar1.Key = "MainMenu";
			this.uiCommandBar1.Location = new System.Drawing.Point(0, 0);
			this.uiCommandBar1.LockCommandBar = Janus.Windows.UI.InheritableBoolean.True;
			this.uiCommandBar1.Name = "uiCommandBar1";
			this.uiCommandBar1.RowIndex = 0;
			this.uiCommandBar1.Size = new System.Drawing.Size(884, 24);
			this.uiCommandBar1.TabIndex = 0;
			// 
			// Hilfe1
			// 
			this.Hilfe1.Key = "Hilfe";
			this.Hilfe1.Name = "Hilfe1";
			// 
			// Datei1
			// 
			this.Datei1.Key = "Datei";
			this.Datei1.Name = "Datei1";
			// 
			// TopRebar1
			// 
			this.TopRebar1.CommandBars.AddRange(new Janus.Windows.UI.CommandBars.UICommandBar[] {
            this.uiCommandBar1});
			this.TopRebar1.CommandManager = this.uiCommandManager1;
			this.TopRebar1.Controls.Add(this.uiCommandBar1);
			this.TopRebar1.Dock = System.Windows.Forms.DockStyle.Top;
			this.TopRebar1.Location = new System.Drawing.Point(0, 0);
			this.TopRebar1.Name = "TopRebar1";
			this.TopRebar1.Size = new System.Drawing.Size(884, 24);
			this.TopRebar1.TabIndex = 0;
			// 
			// uiCommandManager1
			// 
			this.uiCommandManager1.BottomRebar = this.BottomRebar1;
			this.uiCommandManager1.CommandBars.AddRange(new Janus.Windows.UI.CommandBars.UICommandBar[] {
            this.uiCommandBar1});
			this.uiCommandManager1.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.Datei,
            this.Hilfe,
            this.Beenden,
            this.Info,
            this.UpdateApplication});
			this.uiCommandManager1.ContainerControl = this;
			this.uiCommandManager1.Id = new System.Guid("258c0936-b1d7-4cfd-8826-c2b1d8d758e3");
			this.uiCommandManager1.LeftRebar = this.LeftRebar1;
			this.uiCommandManager1.RightRebar = this.RightRebar1;
			this.uiCommandManager1.TopRebar = this.TopRebar1;
			// 
			// TopRebar2
			// 
			this.TopRebar2.CommandManager = this.uiCommandManager1;
			this.TopRebar2.Dock = System.Windows.Forms.DockStyle.Top;
			this.TopRebar2.Location = new System.Drawing.Point(0, 0);
			this.TopRebar2.Name = "TopRebar2";
			this.TopRebar2.Size = new System.Drawing.Size(0, 0);
			this.TopRebar2.TabIndex = 0;
			// 
			// Datei2
			// 
			this.Datei2.Key = "Datei";
			this.Datei2.Name = "Datei2";
			// 
			// Hilfe2
			// 
			this.Hilfe2.Key = "Hilfe";
			this.Hilfe2.Name = "Hilfe2";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 489);
			this.Controls.Add(this.LeftRebar1);
			this.Controls.Add(this.RightRebar1);
			this.Controls.Add(this.TopRebar1);
			this.Controls.Add(this.BottomRebar1);
			this.Name = "MainForm";
			this.Text = "Europlan";
			((System.ComponentModel.ISupportInitialize)(this.BottomRebar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.RightRebar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LeftRebar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiCommandBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TopRebar1)).EndInit();
			this.TopRebar1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiCommandManager1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TopRebar2)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Janus.Windows.UI.CommandBars.UICommand UpdateApplication;
		private Janus.Windows.UI.CommandBars.UICommand Info;
		private Janus.Windows.UI.CommandBars.UICommand Beenden;
		private Janus.Windows.UI.CommandBars.UICommand Update1;
		private Janus.Windows.UI.CommandBars.UICommand Info1;
		private Janus.Windows.UI.CommandBars.UICommand Hilfe;
		private Janus.Windows.UI.CommandBars.UICommand Beenden1;
		private Janus.Windows.UI.CommandBars.UICommand Datei;
		private Janus.Windows.UI.CommandBars.UIRebar BottomRebar1;
		private Janus.Windows.UI.CommandBars.UICommandManager uiCommandManager1;
		private Janus.Windows.UI.CommandBars.UICommandBar uiCommandBar1;
		private Janus.Windows.UI.CommandBars.UIRebar LeftRebar1;
		private Janus.Windows.UI.CommandBars.UIRebar RightRebar1;
		private Janus.Windows.UI.CommandBars.UIRebar TopRebar1;
		private Janus.Windows.UI.CommandBars.UICommand Hilfe1;
		private Janus.Windows.UI.CommandBars.UICommand Datei1;
		private Janus.Windows.UI.CommandBars.UIRebar TopRebar2;
		private Janus.Windows.UI.CommandBars.UICommand Datei2;
		private Janus.Windows.UI.CommandBars.UICommand Hilfe2;

	}
}

