﻿#pragma checksum "..\..\EditFunciones.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6FFCD8BB6E1706BB052CCF9E04FD37934949045F5DC6FBCEBE7077F77EFD62FB"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using Project_VW;
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


namespace Project_VW {
    
    
    /// <summary>
    /// EditFunciones
    /// </summary>
    public partial class EditFunciones : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\EditFunciones.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cb_searchFilter;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\EditFunciones.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox FirstFilter;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\EditFunciones.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox SecondFilter;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\EditFunciones.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ThirdFilter;
        
        #line default
        #line hidden
        
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
            System.Uri resourceLocater = new System.Uri("/Project_VW;component/editfunciones.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\EditFunciones.xaml"
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
            this.cb_searchFilter = ((System.Windows.Controls.ComboBox)(target));
            
            #line 22 "..\..\EditFunciones.xaml"
            this.cb_searchFilter.DropDownClosed += new System.EventHandler(this.cb_SF_DropDownClosed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.FirstFilter = ((System.Windows.Controls.ComboBox)(target));
            
            #line 23 "..\..\EditFunciones.xaml"
            this.FirstFilter.DropDownClosed += new System.EventHandler(this.cb_FF_DropDownClosed);
            
            #line default
            #line hidden
            return;
            case 3:
            this.SecondFilter = ((System.Windows.Controls.ComboBox)(target));
            
            #line 24 "..\..\EditFunciones.xaml"
            this.SecondFilter.DropDownClosed += new System.EventHandler(this.cb_SecF_DropDownClosed);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ThirdFilter = ((System.Windows.Controls.ComboBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

