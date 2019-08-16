﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project_VW
{
    /// <summary>
    /// Lógica de interacción para admin.xaml
    /// </summary>
    public partial class admin : Page
    {
        private Visibility visibility;
        public Visibility Visibility
        {
            get
            {
                return visibility;
            }
            set
            {
                visibility = value;

                OnPropertyChanged("Visibility");
            }
        }
        public admin()
        {
            InitializeComponent();
            Visibility = Visibility.Visible;
            // DataContext explains WPF in which object WPF has to check the binding path. Here Vis is in "this" then:
            DataContext = this;
            if (SesionUsuario.getUserTipo() == 2)
                NuevoUsuario.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void AnadirUsuario_Click(object sender, RoutedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();
            usc = new NuevoUsuario();
            GridMain.Children.Add(usc);
        }

        private void InfoUsuario_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("UID: " + SesionUsuario.getUserID() +
                     ", UN: " + SesionUsuario.getUser() +
                     ", UT: " + SesionUsuario.getUserTipo());
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemHome":
                    usc = new UserControl1_test();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemCreate":
                    usc = new UserControl2_test();
                    GridMain.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }

    }
}
