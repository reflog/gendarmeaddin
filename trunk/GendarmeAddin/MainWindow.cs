//

using System;
using Gtk;
using Mono.Cecil;
using Gendarme.Framework;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Xml;
using System.IO;

namespace gtk {


public partial class MainWindow : Gtk.Window {
        private const string config = "/usr/lib/gendarme/rules.xml";
        private const string set = "default";
        Runner runner = new MinimalRunner();
        TreeStore store = new TreeStore(typeof(string), typeof(Violation));


        void fillViolations() {
            foreach(Violation v in runner.Violations) {
                RuleInformation ri = RuleInformationManager.GetRuleInformation (v.Rule);
                TreeIter i = store.AppendValues(new object [] {ri.Name, v});
                string s =  Mono.Unix.Catalog.GetString("Problem");
                TreeIter i2 = store.AppendValues(i, new object [] {s, v});
                s = String.Format (ri.Problem, v.Violator);
                store.AppendValues(i2, new object [] {s, v});
                TreeIter i3;
                if(v.Messages != null && v.Messages.Count > 0) {
                    s = Mono.Unix.Catalog.GetString("Details");
                    i3 = store.AppendValues(i2, new object [] {s, v});
                    s = String.Empty;
                    foreach (Message message in v.Messages) {
                        s += message + Environment.NewLine;
                    }
                    store.AppendValues(i3, new object [] {s, v});
                }
                s = Mono.Unix.Catalog.GetString("Solution");
                i3 = store.AppendValues(i, new object [] {s, v});
                s = String.Format (ri.Solution, v.Violator)+ Environment.NewLine;
                string url = ri.Uri;
                if (url.Length > 0) {
                    url = String.Format("<u>{0}</u>",url);
                    s += "<b>"+Mono.Unix.Catalog.GetString("More info available at:")+"</b> " + Environment.NewLine + url;
                }
                store.AppendValues(i3, new object [] {s, v});
            }
            lblRules.Text =(runner.Rules.Assembly.Count +
                            runner.Rules.Method.Count +
                            runner.Rules.Module.Count +
                            runner.Rules.Type.Count).ToString();
            lblVio.Text = (runner.Violations.Count).ToString();
        }

        void prepareView() {
            store.Clear();
            vView.Model = store;
            TreeViewColumn col = new TreeViewColumn();
            CellRendererText nameCell = new Gtk.CellRendererText ();
            nameCell.WrapWidth = 300;
            col.PackStart (nameCell, true);
            vView.AppendColumn (col);
            col.SetCellDataFunc (nameCell, new Gtk.TreeCellDataFunc (RenderRow));
        }

        private void RenderRow (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter) {
            string item = (string) model.GetValue (iter, 0);
            (cell as Gtk.CellRendererText).Markup  = item;
        }


public MainWindow() :
        base(Gtk.WindowType.Toplevel) {
            this.Build();
            LoadConfiguration ();
        }


        string GetAttribute (XmlElement xel, string name, string defaultValue) {
            XmlAttribute xa = xel.Attributes [name];
            if (xa == null)
                return defaultValue;
            return xa.Value;
        }


        bool LoadConfiguration () {
            XmlDocument doc = new XmlDocument ();
            doc.Load (config);
            if (doc.DocumentElement.Name != "gendarme")
                return false;

            bool result = false;
            foreach (XmlElement ruleset in doc.DocumentElement.SelectNodes("ruleset")) {
                if (ruleset.Attributes["name"].Value != set)
                        continue;
                foreach (XmlElement assembly in ruleset.SelectNodes("rules")) {
                    string include = GetAttribute (assembly, "include", "*");
                    string exclude = GetAttribute (assembly, "exclude", String.Empty);
                    string from =  "/usr/lib/gendarme/"+GetAttribute (assembly, "from", String.Empty);
                    try {
                        int n = runner.LoadRulesFromAssembly (from, include, exclude);
                        result = (result || (n > 0));
                    } catch (Exception e) {
                        Console.WriteLine ("Error reading rules{1}Details: {0}", e, Environment.NewLine);
                        return false;
                    }
                }
            }
            return result;
        }

        protected virtual void OnDeleteEvent(object o, Gtk.DeleteEventArgs args) {
            Application.Quit();
        }

        protected virtual void OnBtnRunClicked(object sender, System.EventArgs e) {
            runner.Process(AssemblyFactory.GetAssembly("/home/reflog/xmmssharp/xmms-sharp/bin/Debug/xmms-sharp.dll"));
            prepareView();
            fillViolations();
        }

        string getSource(MessageCollection ms) {
            if (ms != null && ms.Count == 0)return "";

            foreach(Message m in ms) {
                return m.Location.ToString();
            }
            return "";
        }
        
        void openWindow(AssemblyDefinition a, string path){
            string type = "";
            string method = "";
            int mpos = path.IndexOf("::");
            if(mpos!=-1){                
                type = path.Substring(0,mpos);
                method = path.Substring(mpos+2,path.IndexOf(":", mpos+2)-mpos-2);
            }else{
                type = path.Substring(0,path.IndexOf(":"));
            }
        }

        [GLib.ConnectBefore]
        protected virtual void OnVViewButtonPressEvent(object o, Gtk.ButtonPressEventArgs args) {
            if(args.Event.Button == 3 && args.Event.Type == Gdk.EventType.ButtonPress) {
                TreeSelection selection = vView.Selection;
                if (selection.GetSelectedRows().Length  == 1) {
                    TreePath path = TreePath.NewFirst();
                    if(vView.GetPathAtPos((int)args.Event.X, (int)args.Event.Y, out path)) {
                        TreeIter iter = TreeIter.Zero;
                        if(store.GetIter(out iter, path)) {
                            Violation v = (Violation)store.GetValue(iter, 1);

                            string s = Mono.Unix.Catalog.GetString("Go to problem source:");
                            string s2 = getSource(v.Messages);
                            if (s2 != "") {
                                Menu m = new Menu();
                                MenuItem mi = new MenuItem(s+Environment.NewLine+s2);

                                mi.Activated +=  delegate(object sender, EventArgs a) {
                                                     openWindow(v.Assembly, s2);
                                                 };
                                m.Append(mi);
                                m.ShowAll();
                                m.Popup();
                            }
                        }
                    }
                }
            }
        }

    }
}
