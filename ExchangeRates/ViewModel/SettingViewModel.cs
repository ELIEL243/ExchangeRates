using ExchangeRates.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRates.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ExchangeRates.ViewModel
{
    public class SettingViewModel: INotifyPropertyChanged
    {
        public DelegateCommand val_btn { get; set; }
        public DeviseDbService dbService { get; set; }
        public Devise current_devise { get; set; }
        public ObservableCollection<Devise> devises_lst { get; set; }
        public Devise devise { get; set; }
        public bool check { get; set; }
        public string message { get; set; }
        public bool uncheck { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingViewModel()
        {
            val_btn = new DelegateCommand(ValiderChangement);
            dbService = new DeviseDbService();
            current_devise = dbService.GetRefDevise();
            devises_lst = dbService.GetDevises();
            check = false;
            uncheck = false;
        }

        private void ValiderChangement()
        {
            if (devise != null)
            {
                dbService.ChangeRefDevise(devise);
                //MessageBox.Show("Devise de reference changée avec succes !", "Good", MessageBoxButton.OK, MessageBoxImage.Information);
                message = "Devise de reference changée avec succes !";
                OnPropertyChanged("message");
                check = true;
                OnPropertyChanged("check");
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
