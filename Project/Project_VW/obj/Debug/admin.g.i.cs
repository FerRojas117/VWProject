#pragma checksum "..\..\admin.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "D87E39AD6A3E8F56A7DDA0E9F5280FA79D0C5C1D736F86AD05ECF5621816D0D1"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using Project_VW;
using ShowMeTheXAML;
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


namespace Project_VW
{


    /// <summary>
    /// admin
    /// </summary>
    public partial class admin : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector
    {

#line default
#line hidden


#line 49 "..\..\admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GridMain;

#line default
#line hidden


#line 63 "..\..\admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button InfoUsuario;

#line default
#line hidden


#line 64 "..\..\admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NuevoUsuario;

#line default
#line hidden


#line 74 "..\..\admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GridMenu;

#line default
#line hidden


#line 88 "..\..\admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonOpenMenu;

#line default
#line hidden


#line 91 "..\..\admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonCloseMenu;

#line default
#line hidden


#line 98 "..\..\admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListViewMenu;

#line default
#line hidden


#line 99 "..\..\admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListViewItem ItemHome;

#line default
#line hidden


#line 105 "..\..\admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListViewItem ItemCreate;

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Project_VW;component/admin.xaml", System.UriKind.Relative);

#line 1 "..\..\admin.xaml"
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
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:

#line 40 "..\..\admin.xaml"
                    ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);

#line default
#line hidden
                    return;
                case 2:
                    this.Border1 = ((System.Windows.Controls.Border)(target));
                    return;
                case 3:
                    this.GridMain = ((System.Windows.Controls.Grid)(target));
                    return;
                case 4:
                    this.InfoUsuario = ((System.Windows.Controls.Button)(target));

#line 63 "..\..\admin.xaml"
                    this.InfoUsuario.Click += new System.Windows.RoutedEventHandler(this.InfoUsuario_Click);

#line default
#line hidden
                    return;
                case 5:
                    this.NuevoUsuario = ((System.Windows.Controls.Button)(target));

#line 64 "..\..\admin.xaml"
                    this.NuevoUsuario.Click += new System.Windows.RoutedEventHandler(this.AnadirUsuario_Click);

#line default
#line hidden
                    return;
                case 6:
                    this.GridMenu = ((System.Windows.Controls.Grid)(target));
                    return;
                case 7:
                    this.ButtonOpenMenu = ((System.Windows.Controls.Button)(target));

#line 88 "..\..\admin.xaml"
                    this.ButtonOpenMenu.Click += new System.Windows.RoutedEventHandler(this.ButtonOpenMenu_Click);

#line default
#line hidden
                    return;
                case 8:
                    this.ButtonCloseMenu = ((System.Windows.Controls.Button)(target));

#line 91 "..\..\admin.xaml"
                    this.ButtonCloseMenu.Click += new System.Windows.RoutedEventHandler(this.ButtonCloseMenu_Click);

#line default
#line hidden
                    return;
                case 9:
                    this.ListViewMenu = ((System.Windows.Controls.ListView)(target));

#line 98 "..\..\admin.xaml"
                    this.ListViewMenu.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListViewMenu_SelectionChanged);

#line default
#line hidden
                    return;
                case 10:
                    this.ItemHome = ((System.Windows.Controls.ListViewItem)(target));
                    return;
                case 11:
                    this.ItemCreate = ((System.Windows.Controls.ListViewItem)(target));
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

