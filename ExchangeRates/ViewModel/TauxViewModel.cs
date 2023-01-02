using ExchangeRates.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRates.Models;
using System.Collections.ObjectModel;
using ExchangeRates.Views.DataEntry;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ExchangeRates.ViewModel
{
    public class TauxViewModel: INotifyPropertyChanged
    {
        public TauxDbService dbService { get; set; }
        public DeviseDbService deviseDbService { get; set; }
        public DelegateCommand add_btn { get; set; }
        public DelegateCommand del_btn { get; set; }
        public Taux taux { get; set; }
        public Devise current_devise { get; set; }
        public ObservableCollection<Taux> tauxs { get; set; }
        public ObservableCollection<Devise> devises_lst { get; set; }
        public Devise devise { get; set; }
        public string devise_s { get; set; }
        public string purchase_price { get; set; }
        public string sale_price { get; set; }
        public string dev_ref { get; set; }
        public string message { get; set; }
        public bool check { get; set; }
        public bool uncheck { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public TauxViewModel()
        {
            dbService = new TauxDbService();
            tauxs = new ObservableCollection<Taux>();
            deviseDbService = new DeviseDbService();
            devises_lst = new ObservableCollection<Devise>();
            devises_lst = deviseDbService.GetDevisesSpecial();
            tauxs = dbService.GetTaux();
            current_devise = deviseDbService.GetRefDevise();
            dev_ref = $"Devise de réference : {current_devise.Name}";
            add_btn = new DelegateCommand(AddItem);
            del_btn = new DelegateCommand(DelItem);
            check = false;
            uncheck = false;
        }

        private void DelItem()
        {
            if (taux != null)
            {
                if (MessageBox.Show("Voulez-vous supprimer ?", "Alert", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    dbService.DelItem(taux);
                    dbService.Refresh(tauxs);
                    CallMessage("Taux de change supprimé avec succes !", "alert");
                }

            }
            else
            {
                //MessageBox.Show("Veuillez selectionner un objet !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                CallMessage("Veuillez selectionner un objet !", "alert");
            }
        }

        private void AddItem()
        {
            taux = new Taux();
            TauxDataEntry tauxDataEntry = new TauxDataEntry();
            //item.Price = null;
            tauxDataEntry.DataContext = this;
            tauxDataEntry.ShowDialog();
            try
            {
                if (!string.IsNullOrEmpty(purchase_price) && !string.IsNullOrEmpty(purchase_price) && devise!=null)
                {
                    taux.ref_cur = current_devise.Name;
                    taux.sec_cur = devise.Name;
                    taux.purchase_price = int.Parse(purchase_price);
                    taux.sale_price = int.Parse(sale_price);
                    dbService.ChangeActive(taux);
                    dbService.AddTaux(taux);
                    
                    dbService.Refresh(tauxs);
                    //MessageBox.Show("Taux de change actualisé avec succes !", "Good", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    CallMessage("Taux de change ajoutée avec succes !", "success");
                }
                else
                {
                    CallMessage("Veuillez remplir le champ !", "alert");
                    // MessageBox.Show("Veuillez remplir tout les champs !", "Bad", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception)
            {

            }
            devise = null;
            taux = null;
        }
        public void CallMessage(string msg, string type)
        {
            if (type == "success")
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
