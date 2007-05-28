//

using System;
using Gtk;
using Mono.Cecil;
using Gendarme.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Projects;
using MonoDevelop.Projects.Parser;
using MonoDevelop.Core.Gui;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Pads;
using System.Threading;

namespace MonoDevelop {
public partial class GendarmeDisplay : Gtk.Bin {

        private string config;
        private string set;
        GendarmeRunner runner;
        TreeStore store = new TreeStore(typeof(string), typeof(Violation));
        double progress_step = 0;
        double current_progress = 0;
        double previous_progress = 0;

        public GendarmeDisplay() {
            this.Build();
            prepareView();

        }

        public void ClearView(){
            store.Clear();
            lblVio.Text = "0";
            lblRules.Text = "0";
            progressbar.Fraction = 0;
        }

        void addSolution(RuleInformation ri, Violation v, TreeIter i){
                string s = Mono.Unix.Catalog.GetString("Solution");
                TreeIter i3 = store.AppendValues(i, new object [] {s, v});
                s = String.Format (ri.Solution, v.Violator)+ Environment.NewLine;
                string url = ri.Uri;
                if (url.Length > 0) {
                    url = String.Format("<u>{0}</u>",url);
                    s += "<b>"+Mono.Unix.Catalog.GetString("More info available at:")+"</b> " + Environment.NewLine + url;
                }
                store.AppendValues(i3, new object [] {s, v});
        
        }
        string escapeString(string s){
                    string news = s.Replace("‘", "&apos;");
                    news = news.Replace("\"", "&quot;");
                    news = news.Replace(">", "&gt;");
                    news = news.Replace("<", "&lt;");
                    return news;        
        }
        void addDetails(Violation v, TreeIter i){
                if(v.Messages != null && v.Messages.Count > 0) {
                    string s = Mono.Unix.Catalog.GetString("Details");
                    TreeIter i3 = store.AppendValues(i, new object [] {s, v});
                    s = String.Empty;
                    foreach (Message message in v.Messages) {
                        s += message + Environment.NewLine;
                    }
                    store.AppendValues(i3, new object [] {escapeString(s), v});
                }
        }
        
        void fillViolationsWithoutGrouping() {
            foreach(Violation v in runner.Violations) {
                RuleInformation ri = RuleInformationManager.GetRuleInformation (v.Rule);
                TreeIter i = store.AppendValues(new object [] {ri.Name, v});
                string s =  Mono.Unix.Catalog.GetString("Problem");
                TreeIter i2 = store.AppendValues(i, new object [] {s, v});
                s = String.Format (ri.Problem, v.Violator);
                store.AppendValues(i2, new object [] {escapeString(s), v});
                addDetails(v, i2);
                addSolution(ri, v, i); 
            }
        }

        void fillViolationsWithReasonGrouping() {
            Dictionary<RuleInformation, List<Violation>> vList = new Dictionary<RuleInformation, List<Violation>>();
            foreach(Violation v in runner.Violations) {
                RuleInformation ri = RuleInformationManager.GetRuleInformation (v.Rule);
                if(!vList.ContainsKey(ri)) {
                    vList[ri] = new List<Violation>();
                }
                vList[ri].Add(v);
            }
            foreach(RuleInformation ri in vList.Keys) {
                TreeIter i = store.AppendValues(new object [] {ri.Name + " ( " + vList[ri].Count + " )" , null});
                foreach(Violation v in vList[ri]) {
                    string s = String.Format (ri.Problem, v.Violator);
                    TreeIter i2 = store.AppendValues(i, new object [] {escapeString(s), v});
                    addDetails(v, i2);
                    addSolution(ri, v, i2); 
                }
            }
        }

        void fillViolationsWithSolutionGrouping() {
            Dictionary<DotNetProject, List<Violation>> vList = new Dictionary<DotNetProject, List<Violation>>();
            foreach(Violation v in runner.Violations) {
                DotNetProject prj = null;
                findItem(getSource(v.Messages), out prj);
                if (prj == null) {
                    MonoDevelop.Core.Gui.Services.MessageService.ShowErrorFormatted("Cannot display results. Couldn't resolve the location of {3} {0} by it's source {3} {1} for violation {3} {2}",
                        v.Violator.ToString(),getSource(v.Messages),getMessageLine(v.Messages),Environment.NewLine);
                    return;
                }
                if(!vList.ContainsKey(prj)) {
                    vList[prj] = new List<Violation>();
                }
                vList[prj].Add(v);
            }

            foreach(DotNetProject project in vList.Keys) {
                TreeIter ir = store.AppendValues(new object [] { project.Name + " ( " + vList[project].Count + " )" , null });
                foreach(Violation v in vList[project]) {
                    RuleInformation ri = RuleInformationManager.GetRuleInformation (v.Rule);
                    TreeIter i = store.AppendValues(ir, new object [] {ri.Name, v});
                    string s =  Mono.Unix.Catalog.GetString("Problem");
                    TreeIter i2 = store.AppendValues(i, new object [] {s, v});
                    s = String.Format (ri.Problem, v.Violator);
                    store.AppendValues(i2, new object [] {escapeString(s), v});
                    addDetails(v, i2);
                    addSolution(ri, v, i); 
                }
            }
        }

        void fillViolationsWithSolutionReasonGrouping() {
            Dictionary<DotNetProject, Dictionary<RuleInformation, List<Violation>>> vList = new Dictionary<DotNetProject, Dictionary<RuleInformation, List<Violation>> >();
            foreach(Violation v in runner.Violations) {
                DotNetProject prj = null;
                findItem(getSource(v.Messages), out prj);
                if (prj == null) {
                    MonoDevelop.Core.Gui.Services.MessageService.ShowErrorFormatted("Cannot display results. Couldn't resolve the location of {3} {0} by it's source {3} {1} for violation {3} {2}",
                        v.Violator.ToString(),getSource(v.Messages),getMessageLine(v.Messages),Environment.NewLine);
                    return;
                }
                if(!vList.ContainsKey(prj)) {
                    vList[prj] = new Dictionary<RuleInformation, List<Violation>>();
                }
                RuleInformation ri = RuleInformationManager.GetRuleInformation (v.Rule);
                if(!vList[prj].ContainsKey(ri)){
                    vList[prj][ri] = new List<Violation>();
                }
                vList[prj][ri].Add(v);
            }

            foreach(DotNetProject project in vList.Keys) {
                TreeIter ir = store.AppendValues(new object [] { project.Name + " ( " + vList[project].Count + " )" , null });
                foreach(RuleInformation ri in vList[project].Keys){
                    TreeIter i = store.AppendValues(ir, new object [] {ri.Name + " ( " + vList[project][ri].Count + " )" , null});
                    foreach(Violation v in vList[project][ri]) {
                        string s = String.Format (ri.Problem, v.Violator);
                        TreeIter i2 = store.AppendValues(i, new object [] {escapeString(s), v});
                        addDetails(v, i2);
                        addSolution(ri, v, i2); 
                    }
                }
            }
        }


        void updateCounters() {
            lblRules.Text =(runner.Rules.Assembly.Count +
                            runner.Rules.Method.Count +
                            runner.Rules.Module.Count +
                            runner.Rules.Type.Count).ToString();
            lblVio.Text = (runner.Violations.Count).ToString();
        }
        void prepareView() {
            vView.Model = store;
            TreeViewColumn col = new TreeViewColumn();
            CellRendererText nameCell = new Gtk.CellRendererText ();
            col.PackStart (nameCell, true);
            vView.AppendColumn (col);
            col.SetCellDataFunc (nameCell, new Gtk.TreeCellDataFunc (RenderRow));
        }

        private void RenderRow (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter) {
            string item = (string) model.GetValue (iter, 0);
            (cell as Gtk.CellRendererText).Markup  = item;
        }




        string GetAttribute (XmlElement xel, string name, string defaultValue) {
            XmlAttribute xa = xel.Attributes [name];
            if (xa == null)
                return defaultValue;
            return xa.Value;
        }

        bool LoadConfiguration () {
            XmlDocument doc = new XmlDocument ();
            bool result = false;
            try {
                doc.Load (config);
                if (doc.DocumentElement.Name != "gendarme")
                    return false;
                foreach (XmlElement ruleset in doc.DocumentElement.SelectNodes("ruleset")) {
                    if (ruleset.Attributes["name"].Value != set)
                            continue;
                    foreach (XmlElement assembly in ruleset.SelectNodes("rules")) {
                        string include = GetAttribute (assembly, "include", "*");
                        string exclude = GetAttribute (assembly, "exclude", String.Empty);
                        string from =  System.IO.Path.Combine(System.IO.Path.GetDirectoryName (config),
                                                              GetAttribute (assembly, "from", String.Empty));
                        int n = runner.LoadRulesFromAssembly (from, include, exclude);
                        result = (result || (n > 0));
                    }
                }
            } catch (Exception e) {
                MonoDevelop.Core.Gui.Services.MessageService.ShowErrorFormatted("Error reading rules" + Environment.NewLine + "Details: " + e);
                return false;
            }

            return result;
        }

        void fillViolations(MonoDevelop.GendarmeGroupingType mode) {
           MonoDevelop.Core.Gui.Services.DispatchService.GuiDispatch (new StatefulMessageHandler (delegate {
                vView.Model = null;
              }
           ), null);

            switch(mode) {
            case MonoDevelop.GendarmeGroupingType.None:
                fillViolationsWithoutGrouping();
                break;
            case MonoDevelop.GendarmeGroupingType.Solution:
                fillViolationsWithSolutionGrouping();
                break;
            case MonoDevelop.GendarmeGroupingType.SolutionAndReason:
                fillViolationsWithSolutionReasonGrouping();
                break;
            case MonoDevelop.GendarmeGroupingType.Reason:
                fillViolationsWithReasonGrouping();
                break;
            }
           MonoDevelop.Core.Gui.Services.DispatchService.GuiDispatch (new StatefulMessageHandler (delegate {
                vView.Model = store; 
              }
           ), null);
        }
        
        
        public void RunTests(object item) {
            store.Clear();
            progressbar.Fraction = 0;            
            Thread thread = new Thread(OnDoWork);
            thread.IsBackground = true;
            thread.Start(item);            
        }
        
        private void SetProgress(double number)
       { 
           MonoDevelop.Core.Gui.Services.DispatchService.GuiDispatch (new StatefulMessageHandler (delegate {
                 progressbar.Fraction = number;
                 updateCounters();
              }
           ), null);
       }
        public void OnProgressChanged(int progress){
            double addp = (double)progress / progress_step;
            double savep = previous_progress; 
            previous_progress = addp;
            current_progress +=  addp - savep;
            SetProgress(current_progress);
        }        
       
        void OnDoWork (object item){
            runner = new GendarmeRunner();
            runner.ProgressChanged += new GendarmeRunner.ProgressChangedHandler(OnProgressChanged);
            config = Runtime.Properties.GetProperty ("GendarmeAddIn.Path", MonoDevelop.GendarmeConfig.defaultFile);
            set = Runtime.Properties.GetProperty ("GendarmeAddIn.Set", MonoDevelop.GendarmeConfig.defaultSet);
            LoadConfiguration ();
            current_progress = 0;
            if(item is Combine) {
                if((item as Combine).GetAllEntries (typeof(DotNetProject)).Count>0){
                    progress_step = (double) ((item as Combine).GetAllEntries (typeof(DotNetProject)).Count)*100;
                foreach(DotNetProject entry in (item as Combine).GetAllEntries (typeof(DotNetProject))) {
                    previous_progress = 0;
                    if(System.IO.File.Exists(entry.GetOutputFileName ())) {
                        AssemblyDefinition ass = AssemblyFactory.GetAssembly(entry.GetOutputFileName ());
                        runner.ProcessWithProgress(ass);
                    }
                }
                }
            } else if(item is DotNetProject) {
                progress_step = 100;
                if(System.IO.File.Exists((item as DotNetProject).GetOutputFileName ())) {
                    AssemblyDefinition ass = AssemblyFactory.GetAssembly( (item as DotNetProject).GetOutputFileName ());
                    runner.ProcessWithProgress(ass);
                }

            } else  return;
                
                SetProgress(1);
                fillViolations((MonoDevelop.GendarmeGroupingType)Runtime.Properties.GetProperty ("GendarmeAddIn.GroupMode", (int)GendarmeGroupingType.None));
        }
        
        string getMessageLine(MessageCollection ms) {
            if (ms != null && ms.Count > 0) {
                foreach(Message m in ms) {
                    return m.Text;
                }
            }
            return String.Empty;
        }
        string getSource(MessageCollection ms) {
            if (ms != null && ms.Count > 0) {
                foreach(Message m in ms) {
                    return m.Location.ToString();
                }
            }
            return String.Empty;
        }

        ILanguageItem findItemMethod(IClass cls, string type, string method){
         foreach(IClass clspart in cls.Parts) {         
             if (clspart.FullyQualifiedName == type) {
                 if (method != String.Empty) {
                     foreach (IMethod met in cls.Methods) {
                         if ((met.Name == method) ||
                                 (met.IsConstructor  && method == ".ctor")
                            ) {
                             return met;
                         }
                     }
                     
                     if(method.Length>4 && (method.StartsWith("set_") ||method.StartsWith("get_"))){
                         foreach(IProperty prop in cls.Properties){
                             if (prop.CanGet && (("get_"+prop.Name) == method) ||
                                 prop.CanSet && (("set_"+prop.Name) == method) )
                                     return prop;
                         }
                     }
                 }
                 return clspart;
            }
            foreach(IClass subclass in cls.InnerClasses){
                    ILanguageItem metret = findItemMethod(subclass, type, method);
                    if (metret != null) return metret;            
            }
         }    
         return null;
        }
        
        ILanguageItem findItem(string path, out DotNetProject targetProject) {
            string type = String.Empty;
            string method = String.Empty;
            int mpos = path.IndexOf("::");
            if(mpos!=-1) {
                type = path.Substring(0,mpos);
                method = path.Substring(mpos+2,path.IndexOf(":", mpos+2)-mpos-2);
            } else {
                type = path.Substring(0,path.IndexOf(":"));
            }
            type = type.Replace("/",".");
            int anonpos = type.IndexOf("<>c__CompilerGenerated");
            if (anonpos != -1){
                type = type.Substring(0, anonpos-1);
                method = String.Empty;
            }
            foreach(DotNetProject entry in IdeApp.ProjectOperations.CurrentOpenCombine.GetAllEntries (typeof(DotNetProject))) {
                targetProject = entry;
                foreach(ProjectFile project in entry.ProjectFiles) {
                    IParserContext ctx = IdeApp.ProjectOperations.ParserDatabase.GetProjectParserContext (project.Project);
                    foreach (IClass cls in ctx.GetProjectContents ()) {
                            ILanguageItem metret = findItemMethod(cls, type, method);
                            if (metret != null) 
                                return metret;
                     }
                 }
             }

            targetProject = null;
            return null;
        }

        void openWindow(AssemblyDefinition a, string path) {
            DotNetProject prj = null;
            ILanguageItem item = findItem( path, out prj);
            if (item != null)
                IdeApp.ProjectOperations.JumpToDeclaration (item);
            else
                MonoDevelop.Core.Gui.Services.MessageService.ShowErrorFormatted("The source for this violation was not found");
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
                            object ob = store.GetValue(iter, 1);
                            if (ob == null) return;
                            Violation v = (Violation)ob;

                            string s = Mono.Unix.Catalog.GetString("Go to problem source:");
                            string s2 = getSource(v.Messages);
                            if (s2 != String.Empty) {
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
