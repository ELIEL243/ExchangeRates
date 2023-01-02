using ExchangeRates.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;
using System.Windows;
using ExchangeRates.Services;
using ExchangeRates.Views.DataEntry;
using Org.BouncyCastle.Utilities;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ExchangeRates.ViewModel
{
    public class DeviseViewModel: INotifyPropertyChanged
    {
        public DeviseDbService dbService { get; set; }
        public DelegateCommand add_btn { get; set; }
        public DelegateCommand del_btn { get; set; }
        public DelegateCommand edit_btn { get; set; }
        public Devise selected_devise { get; set; }
        public Devise devise { get; set; }
        public bool check { get; set; }
        public bool uncheck { get; set; }
        public string message { get; set; }
        public ObservableCollection<Devise> devises_lst { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public DeviseViewModel()
        {
            dbService = new DeviseDbService();
            devises_lst = new ObservableCollection<Devise>();
            devises_lst = dbService.GetDevises();
            add_btn = new DelegateCommand(AddItem);
            del_btn = new DelegateCommand(DelItem);
            edit_btn = new DelegateCommand(EditItem);
            check = false;
            uncheck = false;
            devise = new Devise();
        }

        private void EditItem()
        {
            if (devise != null)
            {
                DeviseDataEntry itemDataEntry = new DeviseDataEntry();
                
                itemDataEntry.DataContext = this;
                itemDataEntry.ShowDialog();

                if (MessageBox.Show("Voulez-vous modifier ?", "Alert", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if (!string.IsNullOrEmpty(devise.Name))
                    {
                        dbService.EditItem(devise);
                        dbService.Refresh(devises_lst);
                        CallMessage("Devise actualisée avec succes !", "success");
                    }
                    else
                    {
                        //MessageBox.Show("Veuillez remplir le champs !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        dbService.Refresh(devises_lst);
                        CallMessage("Veuillez remplir le champs !", "alert");
                    }


                }
                else
                {
                    //units.Clear();
                    dbService.Refresh(devises_lst);

                }
            }
            else
            {
                dbService.Refresh(devises_lst);
                CallMessage("Veuillez selectionner un objet !", "alert");
                //MessageBox.Show("Veuillez selectionner un objet !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DelItem()
        {
            if (devise != null)
            {
                if (MessageBox.Show("Voulez-vous supprimer ?", "Alert", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    dbService.DelItem(devise);
                    devises_lst.Remove(devise);
                    devise = null;
                    message = "Devise de change supprimée avec succes !";
                    CallMessage("Devise de change supprimée avec succes !", "alert");
                }

            }
            else
            {
                //MessageBox.Show("Veuillez selectionner un objet !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                message = "Veuillez selectionner un objet !";
                CallMessage("Veuillez selectionner un objet !", "alert");

            }
        }

        private void AddItem()
        {
            devise = new Devise();
            DeviseDataEntry itemDataEntry = new DeviseDataEntry();
            //item.Price = null;
            itemDataEntry.DataContext = this;
            itemDataEntry.ShowDialog();
            try
            {
                if (!string.IsNullOrEmpty(devise.Name))
                {
                    dbService.AddDevise(devise);
                    dbService.Refresh(devises_lst);
                    //MessageBox.Show("Devise ajouté avec succes !", "Good", MessageBoxButton.OK, MessageBoxImage.Information);
                    CallMessage("Devise ajoutée avec succes !", "success");
                }
                else
                {
                    CallMessage("Veuillez remplir le champ !", "alert");

                }
            }
            catch (Exception)
            {

            }
            devise = null;
        }
        public void CallMessage(string msg, string type)
        {
            if(type == "success")
            {
                message = msg;
                OnPropertyChanged("message");
                check = true;
                OnPropertyChanged("check");
                uncheck = false;
                OnPropertyChanged("uncheck");

            }
            else
            {
                message = msg;
                OnPropertyChanged("message");
                uncheck = true;
                OnPropertyChanged("uncheck");
                check = false;
                OnPropertyChanged("check");

            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
