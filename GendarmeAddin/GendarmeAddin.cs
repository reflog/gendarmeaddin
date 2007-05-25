//
// Copyright (c) Eli Yukelzon - reflog@gmail.com
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
using System.IO;

using MonoDevelop.Core;
using MonoDevelop.Projects;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Pads;
using MonoDevelop.Ide.Gui.Content;
using Gtk;
using System.Collections.Generic;



namespace MonoDevelop {

    namespace GendarmeAddin {
        public enum Commands {
            TestSingleSolution,
            TestWholeSolution
        }

public class TestSingleSolutionHandler : CommandHandler {
            protected override void Update(CommandInfo info) {
                info.Enabled = IdeApp.Workbench.ActiveDocument != null;
            }

            protected override void Run() {            
                MonoDevelop.GendarmePad pad = (MonoDevelop.GendarmePad) IdeApp.Workbench.Pads[typeof(MonoDevelop.GendarmePad)].Content;
                IdeApp.Workbench.Pads[typeof(MonoDevelop.GendarmePad)].BringToFront();
                (pad.Control as MonoDevelop.GendarmeDisplay).RunTests(false);
            }
        }

public class TestWholeSolutionHandler : CommandHandler {
            protected override void Update(CommandInfo info) {
                info.Enabled = IdeApp.Workbench.ActiveDocument != null;
            }

            protected override void Run() {
                MonoDevelop.GendarmePad pad = (MonoDevelop.GendarmePad) IdeApp.Workbench.Pads[typeof(MonoDevelop.GendarmePad)].Content;
                IdeApp.Workbench.Pads[typeof(MonoDevelop.GendarmePad)].BringToFront();
                (pad.Control as MonoDevelop.GendarmeDisplay) .RunTests(true);
            }
        }

    }
}
