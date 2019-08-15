
using System.Collections.Generic;

namespace ShowMeTheXAML
{
    public static class XamlDictionary
    {
        static XamlDictionary()
        {
            XamlResolver.Set("menus_1", @"<smtx:XamlDisplay Key=""menus_1"" DockPanel.Dock=""Top"" Margin=""5 5 0 5"" xmlns:smtx=""clr-namespace:ShowMeTheXAML;assembly=ShowMeTheXAML"">
  <Menu IsMainMenu=""True"" Height=""52"" Margin=""0,0,-537,0"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
    <MenuItem Header=""_File"">
      <MenuItem Header=""Save"">
        <MenuItem.Icon>
          <materialDesign:PackIcon Kind=""ContentSave"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
        </MenuItem.Icon>
      </MenuItem>
      <MenuItem Header=""Save As.."" />
      <MenuItem Header=""Exit"" InputGestureText=""Ctrl+E"">
        <MenuItem.Icon>
          <materialDesign:PackIcon Kind=""ExitToApp"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
        </MenuItem.Icon>
      </MenuItem>
      <Separator />
      <MenuItem Header=""Excellent"" IsCheckable=""True"" IsChecked=""True"" />
      <MenuItem Header=""Rubbish"" IsCheckable=""True"" />
      <MenuItem Header=""Dig Deeper"" InputGestureText=""Ctrl+D"">
        <MenuItem Header=""Enlightenment?"" IsCheckable=""True"" />
        <MenuItem Header=""Disappointment"" IsCheckable=""True"" />
      </MenuItem>
      <MenuItem Header=""Look Deeper"" InputGestureText=""Ctrl+D"">
        <MenuItem Header=""Plain"" />
        <MenuItem Header=""Ice Cream"" />
      </MenuItem>
    </MenuItem>
    <MenuItem Header=""_Edit"">
      <MenuItem Header=""_Cut"" Command=""Cut"">
        <MenuItem.Icon>
          <materialDesign:PackIcon Kind=""ContentCut"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
        </MenuItem.Icon>
      </MenuItem>
      <MenuItem Header=""_Copy"" Command=""Copy"">
        <MenuItem.Icon>
          <materialDesign:PackIcon Kind=""ContentCopy"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
        </MenuItem.Icon>
      </MenuItem>
      <MenuItem Header=""_Paste"" Command=""Paste"">
        <MenuItem.Icon>
          <materialDesign:PackIcon Kind=""ContentPaste"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
        </MenuItem.Icon>
      </MenuItem>
    </MenuItem>
  </Menu>
</smtx:XamlDisplay>");
XamlResolver.Set("menus_2", @"<smtx:XamlDisplay Key=""menus_2"" DockPanel.Dock=""Top"" Margin=""5 0 0 0"" xmlns:smtx=""clr-namespace:ShowMeTheXAML;assembly=ShowMeTheXAML"">
  <ToolBarTray Margin=""0,0,-537,2"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
    <ToolBar Style=""{DynamicResource MaterialDesignToolBar}"" ClipToBounds=""False"" Margin=""0,0,-555,-13"">
      <Button ToolTip=""Follow me on Twitter"">
        <materialDesign:PackIcon Kind=""TwitterBox"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
      </Button>
      <Button ToolTip=""Save"">
        <materialDesign:PackIcon Kind=""ContentSave"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
      </Button>
      <Separator />
      <Button Command=""Cut"" ToolTip=""Cut"" ToolBar.OverflowMode=""AsNeeded"">
        <materialDesign:PackIcon Kind=""ContentCut"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
      </Button>
      <Button Command=""Copy"" ToolTip=""Copy that stuff"" ToolBar.OverflowMode=""AsNeeded"">
        <materialDesign:PackIcon Kind=""ContentCopy"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
      </Button>
      <Separator />
      <Button Command=""Paste"" ToolTip=""Paste some stuff"" ToolBar.OverflowMode=""AsNeeded"">
        <materialDesign:PackIcon Kind=""ContentPaste"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
      </Button>
      <!-- when badging in a toolbar, make sure the parent ToolBar.ClipToBounds=""False"", and
                     manually apply the button style -->
      <materialDesign:Badged ToolBar.OverflowMode=""AsNeeded"" Badge=""{materialDesign:PackIcon Alert}"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"">
        <Button ToolTip=""Badge it up!"" Style=""{StaticResource {x:Static ToolBar.ButtonStyleKey}}"">
          <materialDesign:PackIcon Kind=""AirplaneTakeoff"" />
        </Button>
      </materialDesign:Badged>
      <Separator />
      <ListBox>
        <ListBoxItem ToolTip=""This is a lonley toggle with TextBlock instead of icon"">
          <TextBlock>W</TextBlock>
        </ListBoxItem>
      </ListBox>
      <Separator />
      <ListBox SelectedIndex=""0"">
        <ListBox.ToolTip>
          <StackPanel>
            <TextBlock Text=""MaterialDesignToolToggleFlatListBox"" />
            <TextBlock Text=""Exclusive selection"" />
            <TextBlock Text=""ListBoxAssist.IsToggle allows more natural toggle behaviour"" />
          </StackPanel>
        </ListBox.ToolTip>
        <ListBoxItem>
          <materialDesign:PackIcon Kind=""FormatAlignLeft"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
        </ListBoxItem>
        <ListBoxItem>
          <materialDesign:PackIcon Kind=""FormatAlignCenter"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
        </ListBoxItem>
        <ListBoxItem>
          <materialDesign:PackIcon Kind=""FormatAlignRight"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
        </ListBoxItem>
        <ListBoxItem>
          <materialDesign:PackIcon Kind=""FormatAlignJustify"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
        </ListBoxItem>
      </ListBox>
      <Separator />
      <ListBox SelectionMode=""Extended"">
        <ListBox.ToolTip>
          <StackPanel>
            <TextBlock Text=""MaterialDesignToolToggleListBox"" />
            <TextBlock Text=""Multiple selection"" />
            <TextBlock Text=""ListBoxAssist.IsToggle allows more natural toggle behaviour"" />
          </StackPanel>
        </ListBox.ToolTip>
        <ListBoxItem>
          <materialDesign:PackIcon Kind=""FormatBold"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
        </ListBoxItem>
        <ListBoxItem>
          <materialDesign:PackIcon Kind=""FormatItalic"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
        </ListBoxItem>
        <ListBoxItem x:Name=""UnderlineCheckbox"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
          <materialDesign:PackIcon Kind=""FormatUnderline"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
        </ListBoxItem>
      </ListBox>
      <Separator />
      <Label Content=""Font size:"" VerticalAlignment=""Center"" />
      <ComboBox>
        <ComboBoxItem Content=""10"" />
        <ComboBoxItem IsSelected=""True"" Content=""12"" />
        <ComboBoxItem Content=""14"" />
        <ComboBoxItem Content=""16"" />
      </ComboBox>
      <CheckBox>
                        Check
                    </CheckBox>
      <Button ToolTip=""Take a nap"" ToolBar.OverflowMode=""Always"">
        <materialDesign:PackIcon Kind=""Hotel"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
      </Button>
      <RadioButton GroupName=""XXX"" Content=""Radio"" />
      <RadioButton GroupName=""XXX"" Content=""Ga Ga"" />
      <Separator />
      <RadioButton GroupName=""YYY"" Style=""{StaticResource MaterialDesignToolRadioButton}"">
        <materialDesign:PackIcon Kind=""Radio"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
      </RadioButton>
      <RadioButton GroupName=""YYY"" Style=""{StaticResource MaterialDesignToolRadioButton}"">
        <materialDesign:PackIcon Kind=""EmoticonPoop"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"" />
      </RadioButton>
      <Separator />
      <ToggleButton />
    </ToolBar>
  </ToolBarTray>
</smtx:XamlDisplay>");
        }
    }
}