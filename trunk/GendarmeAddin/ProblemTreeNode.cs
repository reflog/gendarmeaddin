using System.Collections.Generic;
using System;
using Gendarme.Framework;
using Gtk;
using MonoDevelop.Projects;

namespace MonoDevelop.GendarmeAddin
{
    
    public interface INodeAppendable { 
        void Append(TreeStore store);
        void AppendExpanded(TreeStore store);
        bool WasExpanded();
    }
    
    public class ProblemTreeNode : INodeAppendable
    {
        Violation vio;
        protected void deleteDummyNode(TreeStore store, TreeIter parent){
            TreeIter i = TreeIter.Zero;
            store.IterNthChild(out i, parent, 0);
            store.Remove(ref i);
        }
        protected  string escapeString(string s){
                    string news = s.Replace("‘", "&apos;");
                    news = news.Replace("\"", "&quot;");
                    news = news.Replace(">", "&gt;");
                    news = news.Replace("<", "&lt;");
                    return news;        
        }
        protected bool was_expanded = false;
        public bool WasExpanded() { return was_expanded; }
        
        public ProblemTreeNode() {
        }
        public ProblemTreeNode(Violation v)
        {
            vio = v;
        }
        
        protected void addSolution(TreeStore store, RuleInformation ri, Violation v, TreeIter i){
                string s = Mono.Unix.Catalog.GetString("Solution");
                TreeIter i3 = store.AppendValues(i, new object [] {s, v, null});
                s = String.Format (ri.Solution, v.Violator)+ Environment.NewLine;
                string url = ri.Uri;
                if (url.Length > 0) {
                    url = String.Format("<u>{0}</u>",url);
                    s += "<b>"+Mono.Unix.Catalog.GetString("More info available at:")+"</b> " + Environment.NewLine + url;
                }
                store.AppendValues(i3, new object [] {s, v, null});
        
        }
        
        protected void addDetails(TreeStore store, Violation v, TreeIter i){
                if(v.Messages != null && v.Messages.Count > 0) {
                    string s = Mono.Unix.Catalog.GetString("Details");
                    TreeIter i3 = store.AppendValues(i, new object [] {s, v, null});
                    s = String.Empty;
                    foreach (Message message in v.Messages) {
                        s += message + Environment.NewLine;
                    }
                    store.AppendValues(i3, new object [] {escapeString(s), v, null});
                }
        }
        virtual public void AppendExpanded(TreeStore store){
        
        }
        
        virtual public void Append(TreeStore store){
                RuleInformation ri = RuleInformationManager.GetRuleInformation (vio.Rule);
                
                TreeIter i = store.AppendValues(new object [] {ri.Name, vio, null});
                string s =  Mono.Unix.Catalog.GetString("Problem");
                TreeIter i2 = store.AppendValues(i, new object [] {s, vio, null});
                s = String.Format (ri.Problem, vio.Violator);
                store.AppendValues(i2, new object [] {escapeString(s), vio});
                addDetails(store, vio, i2);
                addSolution(store, ri, vio, i);
        }
    }
    public class ReasonTreeNode : ProblemTreeNode {
        RuleInformation ri;
        List<Violation> vi;
        
        public ReasonTreeNode(RuleInformation _r, List<Violation> _v){
            ri = _r;
            vi = _v;
        }
        TreeIter i;
        
        override public void AppendExpanded(TreeStore store){
        if(!WasExpanded()){
                was_expanded = true;
                deleteDummyNode(store, i);
                foreach(Violation v in vi) {
                    string s = String.Format (ri.Problem, v.Violator);
                    TreeIter i2 = store.AppendValues(i, new object [] {escapeString(s), v});
                    addDetails(store, v, i2);
                    addSolution(store, ri, v, i2); 
                }               
        }
        }
        override public void Append(TreeStore store){
                i = store.AppendValues(new object [] {ri.Name + " ( " + vi.Count + " )" , null, this});
                store.AppendValues(i, new object [] {});
        }
    }
    
        public class SolutionTreeNode : ProblemTreeNode {
        DotNetProject project;
        List<Violation> vi;
        
        public SolutionTreeNode(DotNetProject _r, List<Violation> _v){
            project = _r;
            vi = _v;
        }
        override public void AppendExpanded(TreeStore store){
        if(!WasExpanded()){
                was_expanded = true;
                deleteDummyNode(store, ir);
                foreach(Violation v in vi) {
                    RuleInformation ri = RuleInformationManager.GetRuleInformation (v.Rule);
                    TreeIter i = store.AppendValues(ir, new object [] {ri.Name, v});
                    string s =  Mono.Unix.Catalog.GetString("Problem");
                    TreeIter i2 = store.AppendValues(i, new object [] {s, v});
                    s = String.Format (ri.Problem, v.Violator);
                    store.AppendValues(i2, new object [] {escapeString(s), v});
                    addDetails(store, v, i2);
                    addSolution(store, ri, v, i); 
                }
            }
        }
        TreeIter ir;
        override public void Append(TreeStore store){
                ir = store.AppendValues(new object [] { project.Name + " ( " + vi.Count + " )" , null, this });
                store.AppendValues(ir, new object [] {});
        }
        }

        public class SolutionReasonTreeNode : ProblemTreeNode {
        DotNetProject project;
        Dictionary<RuleInformation, List<Violation>> vi;
        
        public SolutionReasonTreeNode(DotNetProject _r, Dictionary<RuleInformation, List<Violation>> _v){
            project = _r;
            vi = _v;
        }
        override public void AppendExpanded(TreeStore store){
        if(!WasExpanded()){
                was_expanded = true;
                deleteDummyNode(store, ir);                
                foreach(RuleInformation ri in vi.Keys){
                    TreeIter i = store.AppendValues(ir, new object [] {ri.Name + " ( " + vi[ri].Count + " )" , null});
                    foreach(Violation v in vi[ri]) {
                        string s = String.Format (ri.Problem, v.Violator);
                        TreeIter i2 = store.AppendValues(i, new object [] {escapeString(s), v});
                        addDetails(store, v, i2);
                        addSolution(store, ri, v, i2); 
                    }
                }
            }
        }
        TreeIter ir;
        override public void Append(TreeStore store){
                ir = store.AppendValues(new object [] { project.Name + " ( " + vi.Count + " )" , null, this });
                store.AppendValues(ir, new object [] {});
        }
        }

        
}
