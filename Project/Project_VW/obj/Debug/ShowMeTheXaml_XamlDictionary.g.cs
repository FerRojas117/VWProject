
using System.Collections.Generic;

namespace ShowMeTheXAML
{
    public static class XamlDictionary
    {
        static XamlDictionary()
        {
            XamlResolver.Set("shadow_15", @"<smtx:XamlDisplay Grid.Column=""1"" Key=""shadow_15"" Margin=""16 16 0 16"" xmlns:smtx=""clr-namespace:ShowMeTheXAML;assembly=ShowMeTheXAML"">
  <materialDesign:Card materialDesign:ShadowAssist.ShadowDepth=""Depth5"" Padding=""32"" HorizontalAlignment=""Left"" Width=""429"" Margin=""0,0,-209,0"" Height=""365"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"">
    <Grid xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
      <Grid.RowDefinitions>
        <RowDefinition Height=""60 px""></RowDefinition>
        <RowDefinition></RowDefinition>
      </Grid.RowDefinitions>
      <materialDesign:Card Grid.Row=""0"" materialDesign:ShadowAssist.ShadowDepth=""Depth5"" Margin=""10,0,0,0"" Background=""#FF673BB7"">
        <Label Grid.Row=""0"" Content=""Selecciona Sistemas para relacionar
 el vehículo con estos"" FontSize=""18"" HorizontalContentAlignment=""Center"" Background=""#FF5C3D9C"" Foreground=""#DDFFFFFF""></Label>
      </materialDesign:Card>
      <Label x:Name=""noSystems"" Grid.Row=""1"" Margin=""49,92,57,77"" Content=""No hay sistemas 
disponibles por el momento"" HorizontalContentAlignment=""Center"" FontSize=""20"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""></Label>
      <StackPanel x:Name=""stackSystems"" Grid.Row=""1"" Orientation=""Vertical"" Margin=""0 20 0 0"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""></StackPanel>
    </Grid>
  </materialDesign:Card>
</smtx:XamlDisplay>");
        }
    }
}