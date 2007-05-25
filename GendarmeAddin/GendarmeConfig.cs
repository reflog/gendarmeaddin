// CopyRight (c) Eli Yukelzon
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
using System.Collections;

using MonoDevelop.Core.Properties;
using MonoDevelop.Core;

using MonoDevelop.Core.Gui.Components;
using MonoDevelop.Components;
using MonoDevelop.Core.Gui.Dialogs;
using Gtk;

namespace MonoDevelop
{
	
	
	public partial class GendarmeConfig : Gtk.Bin
	{
	    public const string defaultFile = "/usr/lib/gendarme/rules.xml";
	    public const string defaultSet = "default";
	    
		public GendarmeConfig()
		{
			this.Build();
    		edtPath.Text = Runtime.Properties.GetProperty ("GendarmeAddIn.Path", defaultFile);
    		edtSet.Text = Runtime.Properties.GetProperty ("GendarmeAddIn.Set", defaultSet);
		}
		
		public void Store(){
    		Runtime.Properties.SetProperty ("GendarmeAddIn.Path", edtPath.Text);
    		Runtime.Properties.SetProperty ("GendarmeAddIn.Set", edtSet.Text);
    		Runtime.Properties.SaveProperties ();
		}
		
        MonoDevelop.Components.FileSelector d = new MonoDevelop.Components.FileSelector
                ("Select Gendarme Rules File", FileChooserAction.Open);
        
		protected virtual void OnBtnOpenClicked(object sender, System.EventArgs e)
		{		
		d.Filter = new FileFilter();
		d.Filter.AddMimeType("text/xml");
		if (d.Run() == (int)ResponseType.Ok)
			{
			  edtPath.Text = d.Filename;
			}
    		d.Hide();		  
		}

		
	}
	
	 public class GendarmeConfigPanel : AbstractOptionPanel
        {
                GendarmeConfig widget;
                public override void LoadPanelContents()
                {
                        widget = new GendarmeConfig ();
                        Add (widget);
                }

                public override bool StorePanelContents()
                {
                        widget.Store ();
                        return true;
                }
        }
}
