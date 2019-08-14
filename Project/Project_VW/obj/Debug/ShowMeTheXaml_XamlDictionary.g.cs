
using System.Collections.Generic;

namespace ShowMeTheXAML
{
    public static class XamlDictionary
    {
        static XamlDictionary()
        {
            XamlResolver.Set("drawers_1", @"<smtx:XamlDisplay Key=""drawers_1"" MaxHeight=""{x:Static system:Double.MaxValue}"" Margin=""35,42,5,31"" xmlns:smtx=""clr-namespace:ShowMeTheXAML;assembly=ShowMeTheXAML"">
  <materialDesign:DrawerHost Margin=""0,0,-520,0"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" BorderThickness=""2"" BorderBrush=""{DynamicResource MaterialDesignDivider}"" Height=""527"" Width=""1040"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"">
    <materialDesign:DrawerHost.LeftDrawerContent>
      <StackPanel Margin=""16"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
        <TextBlock Margin=""4"" HorizontalAlignment=""Center"">LEFT FIELD</TextBlock>
        <Button Command=""{x:Static materialDesign:DrawerHost.CloseDrawerCommand}"" CommandParameter=""{x:Static Dock.Left}"" Margin=""4"" HorizontalAlignment=""Center"" Style=""{DynamicResource MaterialDesignFlatButton}"">
                        CLOSE THIS
                    </Button>
        <Button Command=""{x:Static materialDesign:DrawerHost.CloseDrawerCommand}"" Margin=""4"" HorizontalAlignment=""Center"" Style=""{DynamicResource MaterialDesignFlatButton}"">
                        CLOSE ALL
                    </Button>
      </StackPanel>
    </materialDesign:DrawerHost.LeftDrawerContent>
    <materialDesign:DrawerHost.TopDrawerContent>
      <StackPanel Margin=""16"" HorizontalAlignment=""Center"" Orientation=""Horizontal"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
        <TextBlock Margin=""4"" VerticalAlignment=""Center"">TOP BANANA</TextBlock>
        <Button Command=""{x:Static materialDesign:DrawerHost.CloseDrawerCommand}"" Style=""{DynamicResource MaterialDesignFlatButton}"" Margin=""4"" VerticalAlignment=""Center"">
                        CLOSE ALL
                    </Button>
        <Button Command=""{x:Static materialDesign:DrawerHost.CloseDrawerCommand}"" CommandParameter=""{x:Static Dock.Top}"" Style=""{DynamicResource MaterialDesignFlatButton}"" Margin=""4"" VerticalAlignment=""Center"">
                        CLOSE THIS
                    </Button>
      </StackPanel>
    </materialDesign:DrawerHost.TopDrawerContent>
    <materialDesign:DrawerHost.RightDrawerContent>
      <StackPanel Margin=""16"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
        <TextBlock Margin=""4"" HorizontalAlignment=""Center"">THE RIGHT STUFF</TextBlock>
        <Button Command=""{x:Static materialDesign:DrawerHost.CloseDrawerCommand}"" CommandParameter=""{x:Static Dock.Right}"" Margin=""4"" HorizontalAlignment=""Center"" Style=""{DynamicResource MaterialDesignFlatButton}"">
                        CLOSE THIS
                    </Button>
        <Button Command=""{x:Static materialDesign:DrawerHost.CloseDrawerCommand}"" Margin=""4"" HorizontalAlignment=""Center"" Style=""{DynamicResource MaterialDesignFlatButton}"">
                        CLOSE ALL
                    </Button>
      </StackPanel>
    </materialDesign:DrawerHost.RightDrawerContent>
    <materialDesign:DrawerHost.BottomDrawerContent>
      <StackPanel Margin=""16"" HorizontalAlignment=""Center"" Orientation=""Horizontal"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
        <TextBlock Margin=""4"" VerticalAlignment=""Center"">BOTTOM BRACKET</TextBlock>
        <Button Command=""{x:Static materialDesign:DrawerHost.CloseDrawerCommand}"" Style=""{DynamicResource MaterialDesignFlatButton}"" Margin=""4"" VerticalAlignment=""Center"">
                        CLOSE ALL
                    </Button>
        <Button Command=""{x:Static materialDesign:DrawerHost.CloseDrawerCommand}"" CommandParameter=""{x:Static Dock.Bottom}"" Style=""{DynamicResource MaterialDesignFlatButton}"" Margin=""4"" VerticalAlignment=""Center"">
                        CLOSE THIS
                    </Button>
      </StackPanel>
    </materialDesign:DrawerHost.BottomDrawerContent>
    <Grid MinWidth=""480"" MinHeight=""480"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
      <Grid VerticalAlignment=""Center"" HorizontalAlignment=""Center"">
        <Grid.RowDefinitions>
          <RowDefinition />
          <RowDefinition />
          <RowDefinition />
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
          <ColumnDefinition />
          <ColumnDefinition />
          <ColumnDefinition />
        </Grid.ColumnDefinitions>
        <Button Command=""{x:Static materialDesign:DrawerHost.OpenDrawerCommand}"" CommandParameter=""{x:Static Dock.Left}"" Grid.Row=""1"" Grid.Column=""0"" Margin=""4"">
          <materialDesign:PackIcon Kind=""ArrowLeft"" />
        </Button>
        <Button Command=""{x:Static materialDesign:DrawerHost.OpenDrawerCommand}"" CommandParameter=""{x:Static Dock.Top}"" Grid.Row=""0"" Grid.Column=""1"" Margin=""4"">
          <materialDesign:PackIcon Kind=""ArrowUp"" />
        </Button>
        <Button Command=""{x:Static materialDesign:DrawerHost.OpenDrawerCommand}"" CommandParameter=""{x:Static Dock.Right}"" Grid.Row=""1"" Grid.Column=""2"" Margin=""4"">
          <materialDesign:PackIcon Kind=""ArrowRight"" />
        </Button>
        <Button Command=""{x:Static materialDesign:DrawerHost.OpenDrawerCommand}"" CommandParameter=""{x:Static Dock.Bottom}"" Grid.Row=""2"" Grid.Column=""1"" Margin=""4"">
          <materialDesign:PackIcon Kind=""ArrowDown"" />
        </Button>
        <Button Command=""{x:Static materialDesign:DrawerHost.OpenDrawerCommand}"" Grid.Row=""1"" Grid.Column=""1"" Margin=""4"" Style=""{DynamicResource MaterialDesignRaisedAccentButton}"">
          <materialDesign:PackIcon Kind=""ArrowAll"" />
        </Button>
      </Grid>
    </Grid>
  </materialDesign:DrawerHost>
</smtx:XamlDisplay>");
        }
    }
}