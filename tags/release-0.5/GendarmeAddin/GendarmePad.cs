// Copyright (C) Eli Yukelzon
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
using System.Threading;
using Gtk;
using Gdk;

using MonoDevelop.Core;
using MonoDevelop.Projects;
using MonoDevelop.Core.Gui;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Pads;

namespace MonoDevelop {


public class GendarmePad: IPadContent {
        GendarmeDisplay widget;

        public GendarmePad() {
            widget = new GendarmeDisplay();
            IdeApp.ProjectOperations.CombineOpened += (CombineEventHandler) MonoDevelop.Core.Gui.Services.DispatchService.GuiDispatch (new CombineEventHandler (OnCombineUpdate));
            IdeApp.ProjectOperations.CombineClosed += (CombineEventHandler) MonoDevelop.Core.Gui.Services.DispatchService.GuiDispatch (new CombineEventHandler (OnCombineUpdate));
            widget.ShowAll();
        }
        
        protected virtual void OnCombineUpdate (object sender, CombineEventArgs e)                                                                     
                {                                                                                                                                            
                        widget.ClearView();                                                                                    
                }                                                                                                                                            
                                                                                                                                                             
                
        void IPadContent.Initialize (IPadWindow window) {
            window.Title = GettextCatalog.GetString ("Gendarme Test results");
            window.Icon = "md-combine-icon";
        }

        public void Dispose () {}

        public Gtk.Widget Control {
            get {
                return widget;
            }
        }
    
        public void RedrawContent () {}

       


    }
}
