using System;
using System.Collections.Generic;
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
using ExchangeRates.ViewModel;
using ExchangeRates.Views;

namespace ExchangeRates
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ForexViewModel forexViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            forexViewModel = new ForexViewModel();
            UCForex uCForex = new UCForex();
            uCForex.DataContext = forexViewModel;
            container.Content = uCForex;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (forexViewModel != null)
            {
                forexViewModel.timer.Stop();
                //forexViewModel.CloseWindowSafe(forexViewModel.loader);

            }
            UCConvertor ucConvertor = new UCConvertor();
            ucConvertor.DataContext = new ConvertorViewModel();
            container.Content = ucConvertor;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (forexViewModel != null)
            {
                forexViewModel.timer.Stop();
                //forexViewModel.CloseWindowSafe(forexViewModel.loader);

            }
            UCDevise devise = new UCDevise();
            devise.DataContext = new DeviseViewModel();
            container.Content = devise;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if(forexViewModel != null)
            {
                forexViewModel.timer.Stop();
                //forexViewModel.CloseWindowSafe(forexViewModel.loader);

            }
            UCSetting uCSetting = new UCSetting();
            uCSetting.DataContext = new SettingViewModel();
            container.Content = uCSetting;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (forexViewModel != null)
            {
                forexViewModel.timer.Stop();
                //forexViewModel.CloseWindowSafe(forexViewModel.loader);

            }
            UCTaux uCTaux = new UCTaux();
            uCTaux.DataContext = new TauxViewModel();
            container.Content = uCTaux;
        }
    }
}
