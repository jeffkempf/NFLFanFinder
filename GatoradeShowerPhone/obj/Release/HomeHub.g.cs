﻿

#pragma checksum "C:\Users\Jeff\Documents\Visual Studio 2013\Projects\WorkingVersionGetItDone\GatoradeShowerPhone\HomeHub.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0FFE746F5A22340E617DCBB358E59390"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GatoradeShowerPhone
{
    partial class HomeHub : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 97 "..\..\HomeHub.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.groupsButton_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 110 "..\..\HomeHub.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.gamesButton_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 123 "..\..\HomeHub.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.exitButton_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 77 "..\..\HomeHub.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.SelectedItem_Changed;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


