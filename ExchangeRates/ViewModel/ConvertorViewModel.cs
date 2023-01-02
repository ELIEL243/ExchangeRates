using ExchangeRates.Models;
using ExchangeRates.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.ViewModel
{
    public class ConvertorViewModel:INotifyPropertyChanged
    {
        public Devise devise_to_change { get; set; }
        public Devise current_devise { get; set; }
        public ObservableCollection<Devise> devises_lst { get; set; }
        public ObservableCollection<Taux> taux_lst { get; set; }
        public DeviseDbService deviseDbService { get; set; }
        public TauxDbService tauxDbService { get; set; }
        public string base_amount { get; set; }
        public string foreign_amount { get; set; }
        public DelegateCommand convert { get; set; }
        public DelegateCommand convert2 { get; set; }
        public DelegateCommand convert3 { get; set; }
        public DelegateCommand change_type { get; set; }
        public string price_type { get; set; }
        public string[] prices { get; set; } 
        public bool change_devise { get; set; }
       
        public event PropertyChangedEventHandler PropertyChanged;

        public ConvertorViewModel()
        {
            devises_lst = new ObservableCollection<Devise>();
            taux_lst = new ObservableCollection<Taux>();
            deviseDbService = new DeviseDbService();
            tauxDbService = new TauxDbService();
            prices = new string[] { "A l'achat", "A la vente" };
            price_type = prices.First();
            devises_lst = deviseDbService.GetDevisesSpecial();
            taux_lst = tauxDbService.GetTaux();
            devise_to_change = devises_lst.First();
            convert = new DelegateCommand(ConvertAmount);
            convert2 = new DelegateCommand(ConvertAmount2);
            convert3 = new DelegateCommand(ChangeDeviseToChange);
            change_type = new DelegateCommand(ChangeDeviseToChange);
        }

        private void ConvertAmount()
        {
            float res = 0;

            if (price_type == "A l'achat")
            {

                if (!string.IsNullOrEmpty(devise_to_change.Name) && !string.IsNullOrEmpty(base_amount))
                {
                    res = float.Parse(base_amount) / GetTauxToChange("Achat");
                    foreign_amount = Math.Round(res, 2).ToString();
                    OnPropertyChanged("foreign_amount");
                    change_devise = false;

                }
            }
            else if(price_type == "A la vente")
            {
                if (!string.IsNullOrEmpty(devise_to_change.Name) && !string.IsNullOrEmpty(base_amount))
                {
                    res = float.Parse(base_amount) / GetTauxToChange("Vente");
                    foreign_amount = Math.Round(res, 2).ToString();
                    OnPropertyChanged("foreign_amount");
                    change_devise = false;

                }
            }
            if (base_amount == "")
            {
                foreign_amount = "";
                OnPropertyChanged("foreign_amount");

            }

        }

        private void ConvertAmount2()
        {
            float res = 0;
            if (price_type == "A l'achat")
            {
                if (!string.IsNullOrEmpty(devise_to_change.Name) && !string.IsNullOrEmpty(foreign_amount))
                {
                    res = float.Parse(foreign_amount) * GetTauxToChange("Achat");
                    base_amount = Math.Round(res, 2).ToString();
                    OnPropertyChanged("base_amount");
                    change_devise = true;

                }
            }
            else if(price_type == "A la vente")
            {
                if (!string.IsNullOrEmpty(devise_to_change.Name) && !string.IsNullOrEmpty(foreign_amount))
                {
                    res = float.Parse(foreign_amount) * GetTauxToChange("Vente");
                    base_amount = Math.Round(res, 2).ToString();
                    OnPropertyChanged("base_amount");
                    change_devise = true;

                }
            }
            if(foreign_amount == "")
            {
                base_amount = "";
                OnPropertyChanged("base_amount");

            }
        }

        private void ChangeDeviseToChange()
        {
            if (!change_devise)
            {
                ConvertAmount();
            }
            else
            {
                ConvertAmount2();
            }
        }


        public float GetTauxToChange(string type)
        {
            if (type == "Achat")
            {
                var query_set = from t in taux_lst where t.sec_cur == devise_to_change.Name && t.is_active == "/Ressources/check.png" select t;
                return query_set.First().purchase_price;
            }
            else
            {
                var query_set = from t in taux_lst where t.sec_cur == devise_to_change.Name && t.is_active == "/Ressources/check.png" select t;
                return query_set.First().sale_price;
            }
            
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
