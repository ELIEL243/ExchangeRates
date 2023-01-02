using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using ExchangeRates.Models;
using static iTextSharp.text.pdf.AcroFields;
using System.Collections.ObjectModel;
using Org.BouncyCastle.Utilities;
using MaterialDesignThemes.Wpf;

namespace ExchangeRates.Services
{
    public class DeviseDbService
    {
        public SQLiteConnection connection { get; set; }
        public SQLiteCommand command { get; set; }
        public SQLiteDataReader reader { get; set; }
        public string query { get; set; }
        public string cs { get; set; }

        public DeviseDbService()
        {
            cs = @"data source = TauxChange.db";
            connection = new SQLiteConnection(cs);
        }

        public ObservableCollection<Devise> GetDevises()
        {
            ObservableCollection<Devise> items = new ObservableCollection<Devise>();
            query = "select * from Devise";
            connection.Open();
            command = new SQLiteCommand(query, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                items.Add(new Devise { No=reader.GetInt32(0), Name=reader.GetString(1)});
            }
            connection.Close();
            return items;


        }

        public ObservableCollection<Devise> GetDevisesSpecial()
        {
            ObservableCollection<Devise> items = new ObservableCollection<Devise>();
            query = "select * from Devise where is_ref=0";
            connection.Open();
            command = new SQLiteCommand(query, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                items.Add(new Devise { No = reader.GetInt32(0), Name = reader.GetString(1) });
            }
            connection.Close();
            return items;


        }

        public void AddDevise(Devise item)
        {
            query = $"Insert into Devise (name,is_ref) values ('{item.Name}',0)";
            command = new SQLiteCommand(query, connection);
            connection.Open();
            //command.Parameters.AddWithValue("@name", unit.Name);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DelItem(Devise devise)
        {
            query = $"Delete from Devise where id={devise.No}";
            command = new SQLiteCommand(query, connection);
            connection.Open();
            //command.Parameters.AddWithValue("@name", unit.Name);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void EditItem(Devise devise)
        {
            query = $"Update Devise set name='{devise.Name}' where id={devise.No}";
            command = new SQLiteCommand(query, connection);
            connection.Open();
            //command.Parameters.AddWithValue("@name", unit.Name);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public Devise GetRefDevise()
        {
            Devise devise = new Devise();
            query = "select * from Devise where is_ref=1";
            connection.Open();
            command = new SQLiteCommand(query, connection);
            reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    devise.No = reader.GetInt32(0);
                    devise.Name = reader.GetString(1);
                }
            }

            connection.Close();
            return devise;
            
        }

        public void ChangeRefDevise(Devise devise)
        {
            try
            {
                connection.Open();
                
                query = $"Update Devise set is_ref=0 where name != '{devise.Name}'";
                command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();

                query = $"Update Devise set is_ref=1 where name = '{devise.Name}'";
                command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception)
            {

            }
        }

        public void Refresh(ObservableCollection<Devise> items)
        {
            query = "select * from Devise";
            connection.Open();
            command = new SQLiteCommand(query, connection);
            reader = command.ExecuteReader();
            items.Clear();
            while (reader.Read())
            {
                items.Add(new Devise { No = reader.GetInt32(0), Name = reader.GetString(1) });

            }
            connection.Close();
        }
    }
}
