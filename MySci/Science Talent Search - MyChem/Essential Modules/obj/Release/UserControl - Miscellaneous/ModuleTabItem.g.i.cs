﻿#pragma checksum "..\..\..\UserControl - Miscellaneous\ModuleTabItem.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "17A86CB4E43BAD8D58AA0A05B79D8FD3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MyChem_Program {
    
    
    /// <summary>
    /// ModuleTabItem
    /// </summary>
    public partial class ModuleTabItem : System.Windows.Controls.TabItem, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Essential Modules;component/usercontrol%20-%20miscellaneous/moduletabitem.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControl - Miscellaneous\ModuleTabItem.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 6 "..\..\..\UserControl - Miscellaneous\ModuleTabItem.xaml"
            ((MyChem_Program.ModuleTabItem)(target)).Loaded += new System.Windows.RoutedEventHandler(this.TabItem_Loaded);
            
            #line default
            #line hidden
            
            #line 6 "..\..\..\UserControl - Miscellaneous\ModuleTabItem.xaml"
            ((MyChem_Program.ModuleTabItem)(target)).DragLeave += new System.Windows.DragEventHandler(this.TabItem_DragLeave);
            
            #line default
            #line hidden
            
            #line 6 "..\..\..\UserControl - Miscellaneous\ModuleTabItem.xaml"
            ((MyChem_Program.ModuleTabItem)(target)).QueryContinueDrag += new System.Windows.QueryContinueDragEventHandler(this.TabItem_QueryContinueDrag);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 2:
            
            #line 17 "..\..\..\UserControl - Miscellaneous\ModuleTabItem.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.mnuRename_Click);
            
            #line default
            #line hidden
            break;
            case 3:
            
            #line 18 "..\..\..\UserControl - Miscellaneous\ModuleTabItem.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.mnuUndock_Click);
            
            #line default
            #line hidden
            break;
            case 4:
            
            #line 19 "..\..\..\UserControl - Miscellaneous\ModuleTabItem.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.mnuClose_Click);
            
            #line default
            #line hidden
            break;
            case 5:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.MouseLeftButtonDownEvent;
            
            #line 24 "..\..\..\UserControl - Miscellaneous\ModuleTabItem.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.Panel_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.MouseLeftButtonDownEvent;
            
            #line 25 "..\..\..\UserControl - Miscellaneous\ModuleTabItem.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.Panel_MouseLeftButtonUp);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}
