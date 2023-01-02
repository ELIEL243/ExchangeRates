using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ExchangeRates.Models;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ExchangeRates.Views.Loader;
using System.Security.Cryptography;

namespace ExchangeRates.ViewModel
{
    public class ForexViewModel
    {
        public ObservableCollection<Forex> forex_lst { get; set; }
        public bool signal { get; set; }
        public Loader loader { get;set; }
        public string jsonString { get; private set; }
        public string[] symb_tab { get; set; }
        public DispatcherTimer timer { get; set; }
        public int count = 0;
        Thread thread_news { get; set; }
        public string base_api = "https://api.twelvedata.com/time_series?symbol=EUR/USD,AUD/USD,EUR/GBP,GBP/USD,USD/JPY,USD/CAD,GBP/JPY&interval=1min&outputsize=1&apikey=c15430600ab24dba83e9146d574586f2";
        public ForexViewModel()
        {
            forex_lst = new ObservableCollection<Forex>();
            symb_tab = new string[] { "EUR/USD", "AUD/USD", "EUR/GBP", "GBP/USD", "USD/JPY", "USD/CAD", "GBP/JPY" };
            ShowLoader();
            timer = new DispatcherTimer();
            timer.Start();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += timer_Tick;




        }

        private void timer_Tick(object sender, EventArgs e)
        {
            GetApiResponses();
        }

        public void CallApiResponses()
        {
            Thread thread_news = new Thread(new ThreadStart(GetApiResponses));
            thread_news.Start();

        }

        public void ShowLoader()
        {
            thread_news = new Thread(new ThreadStart(CallLoader));
            thread_news.SetApartmentState(ApartmentState.STA);
            thread_news.IsBackground = true;
            thread_news.Start();
            
        }

        public void CallLoader()
        {

            
            loader = new Loader();
            loader.ShowDialog();
            Dispatcher.Run();
            

        }

        public void CloseWindowSafe(Window w)
        {
            if (w != null)
            {
                if (w.Dispatcher.CheckAccess())
                    w.Dispatcher.Invoke(new ThreadStart(w.Close));
                else
                    w.Dispatcher.Invoke(new ThreadStart(w.Close));
            }
            
        }

        public void GetApiResponses()
        {

                    
                using (WebClient wc = new WebClient())
                {

                try
                    {
            
                        if (string.IsNullOrEmpty(base_api))
                        {
                            
                            jsonString = wc.DownloadString(base_api);

                        }
                        else
                        {
                            jsonString = wc.DownloadString(base_api);
                            CloseWindowSafe(loader);
                        }
                    
                        byte[] bytes = Encoding.Default.GetBytes(jsonString);
                        jsonString = Encoding.UTF8.GetString(bytes);
                        SetInfo();
                        
                }
                catch (Exception ex){
                    jsonString = null;
                    if (string.IsNullOrEmpty(jsonString))
                    {
                        CloseWindowSafe(loader);
                        MessageBox.Show("Veuillez vous connecter à internet !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        
                    }
                    
                }

            }
        }

        public void CallSetInfo()
        {
            Thread thread_news = new Thread(new ThreadStart(SetInfo));
            thread_news.Start();

        }
        public void SetInfo()
        {
            try
            {
                JObject jsobj = JObject.Parse(jsonString);
                forex_lst.Clear();
                for (int i = 0; i < symb_tab.Length; i++)
                {
                    Console.WriteLine(jsonString);
                    if (!jsobj.ContainsKey("code"))
                    {
                        forex_lst.Add(new Forex { symbole = jsobj[symb_tab[i]]["meta"]["symbol"].ToString(), currency_base = jsobj[symb_tab[i]]["meta"]["currency_base"].ToString(), currency_quote = jsobj[symb_tab[i]]["meta"]["currency_quote"].ToString(), datetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), open = jsobj[symb_tab[i]]["values"][0]["open"].ToString(), high = jsobj[symb_tab[i]]["values"][0]["high"].ToString(), low = jsobj[symb_tab[i]]["values"][0]["low"].ToString(), close = jsobj[symb_tab[i]]["values"][0]["close"].ToString() });

                    }

                }
                
                


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
