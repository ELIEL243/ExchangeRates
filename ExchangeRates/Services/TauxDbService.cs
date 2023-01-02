using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using ExchangeRates.Models;
using System.Collections.ObjectModel;

namespace ExchangeRates.Services
{
    public class TauxDbService
    {
        public SQLiteConnection connection { get; set; }
        public SQLiteCommand command { get; set; }
        public SQLiteDataReader reader { get; set; }
        public string query { get; set; }
        public string cs { get; set; }

        public TauxDbService()
        {
            cs = @"data source = TauxChange.db";
            connection = new SQLiteConnection(cs);
        }

        public ObservableCollection<Taux> GetTaux()
        {
            ObservableCollection<Taux> items = new ObservableCollection<Taux>();
            query = "select * from TauxChange order by is_active DESC";
            connection.Open();
            command = new SQLiteCommand(query, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                if(reader.GetInt32(7) == 1)
                {
                    items.Add(new Taux { id = reader.GetInt32(0), ref_cur = reader.GetString(1), sec_cur = reader.GetString(2), purchase_price = reader.GetFloat(3), sale_price = reader.GetFloat(4), date_added = reader.GetString(5), hour = reader.GetString(6), is_active = "/Ressources/check.png" });
                }
                else
                {
                    items.Add(new Taux { id = reader.GetInt32(0), ref_cur = reader.GetString(1), sec_cur = reader.GetString(2), purchase_price = reader.GetFloat(3), sale_price = reader.GetFloat(4), date_added = reader.GetString(5), hour = reader.GetString(6), is_active = "/Ressources/cancel.png" });
                }
            }
            connection.Close();
            return items;


        }

        public void Refresh(ObservableCollection<Taux> tauxes)
        {
            query = "select * from TauxChange order by is_active DESC";
            connection.Open();
            command = new SQLiteCommand(query, connection);
            reader = command.ExecuteReader();
            tauxes.Clear();
            while (reader.Read())
            {
                if (reader.GetInt32(7) == 1)
                {
                    tauxes.Add(new Taux { id = reader.GetInt32(0), ref_cur = reader.GetString(1), sec_cur = reader.GetString(2), purchase_price = reader.GetFloat(3), sale_price = reader.GetFloat(4), date_added = reader.GetString(5), hour = reader.GetString(6), is_active = "/Ressources/check.png" });
                }
                else
                {
                    tauxes.Add(new Taux { id = reader.GetInt32(0), ref_cur = reader.GetString(1), sec_cur = reader.GetString(2), purchase_price = reader.GetFloat(3), sale_price = reader.GetFloat(4), date_added = reader.GetString(5), hour = reader.GetString(6), is_active = "/Ressources/cancel.png" });
                }
            }
            connection.Close();
            
        }

        public void ChangeActive(Taux taux)
        {
            connection.Open();
            query = $"Update TauxChange set is_active=0 where ref_cur='{taux.ref_cur}' and sec_cur='{taux.sec_cur}' and is_active=1";
            command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
            
            connection.Close();

        }


        public void AddTaux(Taux item)
        {
            query = $"Insert into TauxChange (ref_cur,sec_cur,purchase_price,sale_price,date_added,hour,is_active) values ('{item.ref_cur}', '{item.sec_cur}', {item.purchase_price}, {item.sale_price}, '{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}', '{DateTime.Now.ToString("HH:mm:ss")}', 1)";
            command = new SQLiteCommand(query, connection);
            connection.Open();
            //command.Parameters.AddWithValue("@name", unit.Name);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DelItem(Taux taux)
        {
            query = $"Delete from TauxChange where id={taux.id}";
            command = new SQLiteCommand(query, connection);
            connection.Open();
            //command.Parameters.AddWithValue("@name", unit.Name);
            command.ExecuteNonQuery();
            connection.Close();
        }

    }
}
