using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SQLite;
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
    /// Lógica de interacción para Administracion.xaml
    /// </summary>
    public partial class Administracion : UserControl
    {
        DB db;
        List<Item2Edit> items;
        List<CheckBoxPairsSistemas> cbpairs;
        List<Item2EditAuto> restore;
        List<Item2Edit> itemsRestore;
        int numRegisters = 0;
        int whichItem = 0; // 1 = cars  2 = functions  3 = systems

        // queries to get items
        string qry_getAutos = "SELECT ID, modelo FROM autos WHERE isActive = 1";
        string qry_getSistemas = "SELECT ID, nombre FROM sistema WHERE isActive = 1";
        string qry_getFunciones = "SELECT ID, Funktion FROM funktion WHERE isActive = 1";
        // EN of queries
        public Administracion()
        {
            InitializeComponent();
            db = new DB();
            items = new List<Item2Edit>();
            restore = new List<Item2EditAuto>();
            itemsRestore = new List<Item2Edit>();
            cbpairs = new List<CheckBoxPairsSistemas>();
            cbpairs.Add(new CheckBoxPairsSistemas("Autos", "Autos" ));
            cbpairs.Add(new CheckBoxPairsSistemas("Funciones", "Funciones"));
            cbpairs.Add(new CheckBoxPairsSistemas("Sistemas", "Sistemas"));          
            edit.DisplayMemberPath = "nombre";
            edit.SelectedValuePath = "ID";
            edit.ItemsSource = cbpairs;
        }

        private void buscar_DropDownClosed(object sender, EventArgs e)
        {
            // clear childs of stackpanel with systems
            if (edit.SelectedValue == null)
                return;
            

            string selectedEdition = edit.SelectedValue.ToString();
            switch(selectedEdition)
            {
                case "Autos":                 
                    whichItem = 1;
                    fillItems2Edit(qry_getAutos, whichItem);
                    break;
                case "Funciones":
                    whichItem = 2;
                    fillItems2Edit(qry_getFunciones, whichItem);
                    break;
                case "Sistemas":
                    whichItem = 3;
                    fillItems2Edit(qry_getSistemas, whichItem);
                    break;
            }
        }

        private void itemsCB_DropDownClosed(object sender, EventArgs e)
        {
            if (ItemsCB.SelectedValue == null)
                return;
            nombreItem.Text = "";
            string chosenItem = ItemsCB.Text;
            nombreItem.Text = chosenItem;
        }

        public void fillItems2Edit(string getItems, int autoOrSystem)
        {
            RestoreStack.Children.Clear();
            items.Clear();
            restore.Clear();
            itemsRestore.Clear();
            ItemsCB.ItemsSource = null;
            nombreItem.Text = "";
            string qry_countItems = "SELECT COUNT(ID) AS count ";
            string qry_RestoreItems = "SELECT ";
            
            db.openConn();
            if(autoOrSystem == 1)
            {
                using (db.setComm(getItems))
                {
                    db.setReader();
                    while (db.getReader().Read())
                    {
                        items.Add(
                            new Item2Edit
                            {
                                ID = Convert.ToString(db.getReader()["ID"]),
                                nombre = Convert.ToString(db.getReader()["modelo"])
                            }
                        );
                    }
                }
                //  if car was selected, get elements that are notactive
                qry_RestoreItems += "ID, modelo FROM autos WHERE isActive = 0";
                qry_countItems += "FROM autos WHERE isActive = 0";

                using (db.setComm(qry_countItems))
                {
                    db.setReader();
                    while (db.getReader().Read())
                    {
                        numRegisters = Convert.ToInt32(db.getReader()["count"]);
                    }
                }

                if (numRegisters <= 0)
                {
                    noItems.Visibility = Visibility.Visible;
                }
                else
                {
                    noItems.Visibility = Visibility.Collapsed;
                    using (db.setComm(qry_RestoreItems))
                    {
                        db.setReader();
                        while (db.getReader().Read())
                        {
                            restore.Add(
                                new Item2EditAuto
                                {
                                    ID = Convert.ToString(db.getReader()["ID"]),
                                    modelo = Convert.ToString(db.getReader()["modelo"])
                                }
                            );
                        }

                    }
                    // fill stackpanel with elements that can be restored
                    foreach (Item2EditAuto i2ea in restore)
                    {
                        CheckBox cb = new CheckBox();
                        cb.Name = "_" + i2ea.ID;
                        cb.Content = i2ea.modelo;
                        RestoreStack.Children.Add(cb);
                    }
                }
            }
            else
            {
                /*
                using (db.setComm(getItems))
                {
                    db.setReader();
                    while (db.getReader().Read())
                    {
                        items.Add(
                            new Item2Edit
                            {
                                ID = Convert.ToString(db.getReader()["ID"]),
                                nombre = Convert.ToString(db.getReader()["nombre"])
                            }
                        );
                    }
                }
                */
                // same for funktion and system, get elements and append them to stack

                if(whichItem == 2)
                {
                    qry_RestoreItems += "ID, Funktion FROM funktion WHERE isActive = 0";
                    qry_countItems += "FROM funktion WHERE isActive = 0";

                    using (db.setComm(getItems))
                    {
                        db.setReader();
                        while (db.getReader().Read())
                        {
                            items.Add(
                                new Item2Edit
                                {
                                    ID = Convert.ToString(db.getReader()["ID"]),
                                    nombre = Convert.ToString(db.getReader()["Funktion"])
                                }
                            );
                        }
                    }
                }
                                 
                else
                {
                    qry_RestoreItems += "ID, nombre FROM sistema WHERE isActive = 0";
                    qry_countItems += "FROM sistema WHERE isActive = 0";

                    using (db.setComm(getItems))
                    {
                        db.setReader();
                        while (db.getReader().Read())
                        {
                            items.Add(
                                new Item2Edit
                                {
                                    ID = Convert.ToString(db.getReader()["ID"]),
                                    nombre = Convert.ToString(db.getReader()["nombre"])
                                }
                            );
                        }
                    }
                }

                using (db.setComm(qry_countItems))
                {
                    db.setReader();
                    while (db.getReader().Read())
                    {
                        numRegisters = Convert.ToInt32(db.getReader()["count"]);
                    }
                }

                if (numRegisters == 0)
                {
                    noItems.Visibility = Visibility.Visible;
                }
                else
                {
                    noItems.Visibility = Visibility.Collapsed;
                    using (db.setComm(qry_RestoreItems))
                    {
                        db.setReader();
                        while (db.getReader().Read())
                        {
                            if(whichItem == 2)
                            {
                                itemsRestore.Add(
                                new Item2Edit
                                {
                                    ID = Convert.ToString(db.getReader()["ID"]),
                                    nombre = Convert.ToString(db.getReader()["Funktion"])
                                }
                            );
                            }
                            else
                            {
                                itemsRestore.Add(
                                new Item2Edit
                                {
                                    ID = Convert.ToString(db.getReader()["ID"]),
                                    nombre = Convert.ToString(db.getReader()["nombre"])
                                }
                            );
                            }                          
                        }
                    }
                    // fill stackpanel with elements that can be restored
                    foreach (Item2Edit i2e in itemsRestore)
                    {
                        CheckBox cb = new CheckBox();
                        cb.Name = "_" + i2e.ID;
                        cb.Content = i2e.nombre;
                        RestoreStack.Children.Add(cb);
                    }
                }
            }            
            db.closeConn();

            ItemsCB.DisplayMemberPath = "nombre";
            ItemsCB.SelectedValuePath = "ID";
            ItemsCB.ItemsSource = items;      
        }

        #region METHODS OF BUTTONS SAVE, DELETE, RESTORE
        private void Save(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(nombreItem.Text))
            {
                MessageBox.Show("Escribe al menos una letra para editar.");
                return;
            }
            if(ItemsCB.SelectedValue == null)
            {
                MessageBox.Show("Escoge un item para editar.");
                return;
            }

            string updateItems = "UPDATE ";
            
            switch(whichItem)
            {
                case 1: //cars
                    updateItems += "autos SET modelo = '" + nombreItem.Text + "' WHERE ID = ";
                    updateItems += ItemsCB.SelectedValue.ToString();
                    db.openConn();
                    using (db.setComm(updateItems))
                    {
                        db.getComm().ExecuteNonQuery();
                    }
                    db.closeConn();
                    MessageBox.Show("Nombre de auto actualizado correctamente.");

                    fillItems2Edit(qry_getAutos, whichItem);
                    break;
                case 2: // functions
                    updateItems += "funktion SET Funktion = '" + nombreItem.Text + "' WHERE ID = ";
                    updateItems += ItemsCB.SelectedValue.ToString();
                    db.openConn();
                    using (db.setComm(updateItems))
                    {
                        db.getComm().ExecuteNonQuery();
                    }
                    db.closeConn();
                    MessageBox.Show("Nombre de funcion actualizado correctamente.");
                    fillItems2Edit(qry_getFunciones, whichItem);
                    break;
                case 3: // systems
                    updateItems += "sistema SET nombre = '" + nombreItem.Text + "' WHERE ID = ";
                    updateItems += ItemsCB.SelectedValue.ToString();
                    db.openConn();
                    using (db.setComm(updateItems))
                    {
                        db.getComm().ExecuteNonQuery();
                    }
                    db.closeConn();
                    MessageBox.Show("Nombre de sistema actualizado correctamente.");

                    fillItems2Edit(qry_getSistemas, whichItem);
                    break;
            }
            
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            string item_selected = ItemsCB.Text;
            string ID_item = ItemsCB.SelectedValue.ToString();
            MessageBoxResult result = MessageBox.Show(
                "Estás seguro de eliminar : " + item_selected + ", con ID: " + ID_item + "?",
                "Eliminar Item",
                MessageBoxButton.YesNoCancel
            );
            switch (result)
            {
                case MessageBoxResult.Yes:
                    // update in db is not active, remove from CB and update stackpanel
                    updateOrDelete(0, ID_item);
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Elemento no eliminado.", "Eliminar Elemento");
                    break;
                case MessageBoxResult.Cancel:
                    MessageBox.Show("Elemento no eliminado.", "Eliminar Elemento");
                    break;
            }

        }


        // DELETE CARS FOREVER
        private void deleteOnceNForAll(string ID_item)
        {

            string deleteECF = "DELETE FROM edit_campos_funktion ";
            deleteECF += "WHERE auto_ID = " + ID_item;

            string deleteRAS = "DELETE FROM rel_autos_sist ";
            deleteRAS += "WHERE autos_ID = " + ID_item;

            string deleteAuto = "DELETE FROM autos ";
            deleteAuto += "WHERE ID = " + ID_item;

            try
            {
                db.openConn();
                db.tr = db.getConn().BeginTransaction();
                using (db.setComm(deleteECF))
                {
                    db.getComm().ExecuteNonQuery();
                }
                using (db.setComm(deleteRAS))
                {
                    db.getComm().ExecuteNonQuery();
                }
                using (db.setComm(deleteAuto))
                {
                    db.getComm().ExecuteNonQuery();
                }

                db.tr.Commit();

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: {0}", ex.ToString());

                if (db.tr != null)
                {
                    try
                    {
                        db.tr.Rollback();
                    }
                    catch (SQLiteException ex2)
                    {

                        Console.WriteLine("Transaction rollback failed.");
                        Console.WriteLine("Error: {0}", ex2.ToString());

                    }
                    finally
                    {
                        db.tr.Dispose();
                    }
                }
            }
            finally
            {
                if (db.getComm() != null)
                {
                    db.getComm().Dispose();
                }

                if (db.tr != null)
                {
                    db.tr.Dispose();
                }

                if (db.getConn() != null)
                {
                    try
                    {
                        db.getConn().Close();

                    }
                    catch (SQLiteException ex)
                    {
                        Console.WriteLine("Closing connection failed.");
                        Console.WriteLine("Error: {0}", ex.ToString());
                    }
                    finally
                    {
                        db.getConn().Dispose();
                    }
                }
            }
            MessageBox.Show("Información actualizada correctamente");         
        }

        private void deletePermanent(object sender, RoutedEventArgs e)
        {
            if (RestoreStack.Children.Count < 1) return;
            if (whichItem != 1)
            {
                MessageBox.Show("Solo puedes eliminar de manera permanente autos");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
               "Estás seguro de eliminar este(os) auto(s) de maner definitiva? Esta accion no se puede revertir.",
               "Eliminar Auto Permanente",
               MessageBoxButton.YesNoCancel
           );
            switch (result)
            {
                case MessageBoxResult.Yes:
                    string ID_currentCB;
                    foreach (CheckBox cb in RestoreStack.Children)
                    {
                        // we get the id of the name of each cb, combobox, 
                        // of course we have to know if cb was checked
                        if (cb.IsChecked.HasValue && cb.IsChecked.Value == true)
                        {
                            // ID OF CURRENT ITEM
                            ID_currentCB = cb.Name.ToString();
                            ID_currentCB = ID_currentCB.Trim(new char[] { '_' });
                            deleteOnceNForAll(ID_currentCB);
                        }
                    }
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Auto(s) no eliminado(s).", "Eliminar Auto Permanente");
                    break;
                case MessageBoxResult.Cancel:
                    MessageBox.Show("Auto(s) no eliminado(s).", "Eliminar Auto Permanente");
                    break;
            }
            fillItems2Edit(qry_getAutos, whichItem);
        }

        private void Restore(object sender, RoutedEventArgs e)
        {
            if (RestoreStack.Children.Count < 1) return;
            string ID_currentCB;
            foreach (CheckBox cb in RestoreStack.Children)
            {
                // we get the id of the name of each cb, combobox, 
                // of course we have to know if cb was checked
                if (cb.IsChecked.HasValue && cb.IsChecked.Value == true)
                {
                    // ID OF CURRENT ITEM
                    ID_currentCB = cb.Name.ToString();
                    ID_currentCB = ID_currentCB.Trim(new char[] { '_' });
                    updateOrDelete(1, ID_currentCB);
                }
            }

            switch (whichItem)
            {
                case 1:
                    fillItems2Edit(qry_getAutos, whichItem);
                    break;
                case 2:
                    fillItems2Edit(qry_getFunciones, whichItem);
                    break;
                case 3:
                    fillItems2Edit(qry_getSistemas, whichItem);
                    break;
            }
        }

        #endregion
        public void updateOrDelete(int isActive, string ID_item)
        {
            string updateItems = "UPDATE ";
            switch (whichItem)
            {
                case 1: //cars
                    updateItems += "autos SET isActive = " + isActive + " WHERE ID = ";
                    updateItems += ID_item;
                    db.openConn();
                    using (db.setComm(updateItems))
                    {
                        db.getComm().ExecuteNonQuery();
                    }
                    db.closeConn();
                    if(isActive == 0)
                    {
                        MessageBox.Show("Auto eliminado.");
                        fillItems2Edit(qry_getAutos, whichItem);
                    }
                    break;
                case 2: // functions
                    updateItems += "funktion SET isActive = " + isActive + " WHERE ID = ";
                    updateItems += ID_item;
                    db.openConn();
                    using (db.setComm(updateItems))
                    {
                        db.getComm().ExecuteNonQuery();
                    }
                    db.closeConn();
                    if (isActive == 0)
                    {
                        MessageBox.Show("Funcion eliminada.");
                        fillItems2Edit(qry_getFunciones, whichItem);
                    }
                    break;
                case 3: // systems
                    updateItems += "sistema SET isACtive = " + isActive + " WHERE ID = ";
                    updateItems += ID_item;
                    db.openConn();
                    using (db.setComm(updateItems))
                    {
                        db.getComm().ExecuteNonQuery();
                    }
                    db.closeConn();
                    if (isActive == 0)
                    {
                        MessageBox.Show("Sistema eliminado.");
                        fillItems2Edit(qry_getSistemas, whichItem);
                    }
                    break;
            }
        }
    }

    public class Item2Edit
    {
        public string ID { get; set; }
        public string nombre { get; set; }
    }

    public class Item2EditAuto
    {
        public string ID { get; set; }
        public string modelo { get; set; }
    }
}
