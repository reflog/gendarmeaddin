// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace GendarmeAddin {
    
    
    public partial class GendarmeDisplay {
        
        private Gtk.VBox vbox3;
        
        private Gtk.VPaned vpaned1;
        
        private Gtk.HBox hbox1;
        
        private Gtk.Label label1;
        
        private Gtk.Alignment alignment1;
        
        private Gtk.CheckButton chkAll;
        
        private Gtk.Button btnRun;
        
        private Gtk.Expander expander1;
        
        private Gtk.ScrolledWindow scrolledwindow1;
        
        private Gtk.TreeView vView;
        
        private Gtk.Label GtkLabel2;
        
        private Gtk.Statusbar statusbar1;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Label label4;
        
        private Gtk.Label lblVio;
        
        private Gtk.HBox hbox3;
        
        private Gtk.Label label6;
        
        private Gtk.Label lblRules;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize();
            // Widget GendarmeAddin.GendarmeDisplay
            Stetic.BinContainer.Attach(this);
            this.Events = ((Gdk.EventMask)(0));
            this.Name = "GendarmeAddin.GendarmeDisplay";
            // Container child GendarmeAddin.GendarmeDisplay.Gtk.Container+ContainerChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Events = ((Gdk.EventMask)(0));
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            // Container child vbox3.Gtk.Box+BoxChild
            this.vpaned1 = new Gtk.VPaned();
            this.vpaned1.CanFocus = true;
            this.vpaned1.Events = ((Gdk.EventMask)(0));
            this.vpaned1.Name = "vpaned1";
            this.vpaned1.Position = 34;
            // Container child vpaned1.Gtk.Paned+PanedChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Events = ((Gdk.EventMask)(0));
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Events = ((Gdk.EventMask)(0));
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Gendarme Runner");
            this.hbox1.Add(this.label1);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.hbox1[this.label1]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.alignment1 = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignment1.Events = ((Gdk.EventMask)(0));
            this.alignment1.Name = "alignment1";
            // Container child alignment1.Gtk.Container+ContainerChild
            this.chkAll = new Gtk.CheckButton();
            this.chkAll.CanFocus = true;
            this.chkAll.Events = ((Gdk.EventMask)(0));
            this.chkAll.Name = "chkAll";
            this.chkAll.Label = Mono.Unix.Catalog.GetString("Run on all items in the solution");
            this.chkAll.Active = true;
            this.chkAll.DrawIndicator = true;
            this.chkAll.UseUnderline = true;
            this.alignment1.Add(this.chkAll);
            this.hbox1.Add(this.alignment1);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox1[this.alignment1]));
            w3.Position = 1;
            // Container child hbox1.Gtk.Box+BoxChild
            this.btnRun = new Gtk.Button();
            this.btnRun.CanFocus = true;
            this.btnRun.Events = ((Gdk.EventMask)(0));
            this.btnRun.Name = "btnRun";
            this.btnRun.UseUnderline = true;
            // Container child btnRun.Gtk.Container+ContainerChild
            Gtk.Alignment w4 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            w4.Events = ((Gdk.EventMask)(0));
            w4.Name = "GtkAlignment";
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w5 = new Gtk.HBox();
            w5.Events = ((Gdk.EventMask)(0));
            w5.Name = "GtkHBox";
            w5.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w6 = new Gtk.Image();
            w6.Events = ((Gdk.EventMask)(0));
            w6.Name = "image3";
            w6.Pixbuf = Stetic.IconLoader.LoadIcon("gtk-ok", 16);
            w5.Add(w6);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w8 = new Gtk.Label();
            w8.Events = ((Gdk.EventMask)(0));
            w8.Name = "GtkLabel";
            w8.LabelProp = Mono.Unix.Catalog.GetString("Run tests");
            w8.UseUnderline = true;
            w5.Add(w8);
            w4.Add(w5);
            this.btnRun.Add(w4);
            this.hbox1.Add(this.btnRun);
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.hbox1[this.btnRun]));
            w12.Position = 2;
            w12.Expand = false;
            w12.Fill = false;
            this.vpaned1.Add(this.hbox1);
            Gtk.Paned.PanedChild w13 = ((Gtk.Paned.PanedChild)(this.vpaned1[this.hbox1]));
            w13.Resize = false;
            // Container child vpaned1.Gtk.Paned+PanedChild
            this.expander1 = new Gtk.Expander(null);
            this.expander1.CanFocus = true;
            this.expander1.Events = ((Gdk.EventMask)(0));
            this.expander1.Name = "expander1";
            this.expander1.Expanded = true;
            // Container child expander1.Gtk.Container+ContainerChild
            this.scrolledwindow1 = new Gtk.ScrolledWindow();
            this.scrolledwindow1.CanFocus = true;
            this.scrolledwindow1.Events = ((Gdk.EventMask)(0));
            this.scrolledwindow1.Name = "scrolledwindow1";
            this.scrolledwindow1.VscrollbarPolicy = ((Gtk.PolicyType)(1));
            this.scrolledwindow1.HscrollbarPolicy = ((Gtk.PolicyType)(1));
            this.scrolledwindow1.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow1.Gtk.Container+ContainerChild
            this.vView = new Gtk.TreeView();
            this.vView.CanFocus = true;
            this.vView.Events = ((Gdk.EventMask)(0));
            this.vView.Name = "vView";
            this.scrolledwindow1.Add(this.vView);
            this.expander1.Add(this.scrolledwindow1);
            this.GtkLabel2 = new Gtk.Label();
            this.GtkLabel2.Events = ((Gdk.EventMask)(0));
            this.GtkLabel2.Name = "GtkLabel2";
            this.GtkLabel2.LabelProp = Mono.Unix.Catalog.GetString("Violations:");
            this.GtkLabel2.UseUnderline = true;
            this.expander1.LabelWidget = this.GtkLabel2;
            this.vpaned1.Add(this.expander1);
            this.vbox3.Add(this.vpaned1);
            Gtk.Box.BoxChild w17 = ((Gtk.Box.BoxChild)(this.vbox3[this.vpaned1]));
            w17.Position = 0;
            // Container child vbox3.Gtk.Box+BoxChild
            this.statusbar1 = new Gtk.Statusbar();
            this.statusbar1.Events = ((Gdk.EventMask)(0));
            this.statusbar1.Name = "statusbar1";
            this.statusbar1.Spacing = 6;
            // Container child statusbar1.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Events = ((Gdk.EventMask)(0));
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.label4 = new Gtk.Label();
            this.label4.Events = ((Gdk.EventMask)(0));
            this.label4.Name = "label4";
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("<b>Violations found:</b>");
            this.label4.UseMarkup = true;
            this.hbox2.Add(this.label4);
            Gtk.Box.BoxChild w18 = ((Gtk.Box.BoxChild)(this.hbox2[this.label4]));
            w18.Position = 0;
            w18.Expand = false;
            w18.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.lblVio = new Gtk.Label();
            this.lblVio.Events = ((Gdk.EventMask)(0));
            this.lblVio.Name = "lblVio";
            this.lblVio.LabelProp = Mono.Unix.Catalog.GetString("0");
            this.hbox2.Add(this.lblVio);
            Gtk.Box.BoxChild w19 = ((Gtk.Box.BoxChild)(this.hbox2[this.lblVio]));
            w19.Position = 1;
            w19.Expand = false;
            w19.Fill = false;
            this.statusbar1.Add(this.hbox2);
            Gtk.Box.BoxChild w20 = ((Gtk.Box.BoxChild)(this.statusbar1[this.hbox2]));
            w20.Position = 1;
            w20.Expand = false;
            w20.Fill = false;
            // Container child statusbar1.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Events = ((Gdk.EventMask)(0));
            this.hbox3.Name = "hbox3";
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.label6 = new Gtk.Label();
            this.label6.Events = ((Gdk.EventMask)(0));
            this.label6.Name = "label6";
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("<b>Rules checked:</b>");
            this.label6.UseMarkup = true;
            this.hbox3.Add(this.label6);
            Gtk.Box.BoxChild w21 = ((Gtk.Box.BoxChild)(this.hbox3[this.label6]));
            w21.Position = 0;
            w21.Expand = false;
            w21.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.lblRules = new Gtk.Label();
            this.lblRules.Events = ((Gdk.EventMask)(0));
            this.lblRules.Name = "lblRules";
            this.lblRules.LabelProp = Mono.Unix.Catalog.GetString("0");
            this.hbox3.Add(this.lblRules);
            Gtk.Box.BoxChild w22 = ((Gtk.Box.BoxChild)(this.hbox3[this.lblRules]));
            w22.Position = 1;
            w22.Expand = false;
            w22.Fill = false;
            this.statusbar1.Add(this.hbox3);
            Gtk.Box.BoxChild w23 = ((Gtk.Box.BoxChild)(this.statusbar1[this.hbox3]));
            w23.Position = 2;
            w23.Expand = false;
            w23.Fill = false;
            this.vbox3.Add(this.statusbar1);
            Gtk.Box.BoxChild w24 = ((Gtk.Box.BoxChild)(this.vbox3[this.statusbar1]));
            w24.Position = 1;
            w24.Expand = false;
            w24.Fill = false;
            this.Add(this.vbox3);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.btnRun.Clicked += new System.EventHandler(this.OnBtnRunClicked);
            this.vView.ButtonPressEvent += new Gtk.ButtonPressEventHandler(this.OnVViewButtonPressEvent);
        }
    }
}
