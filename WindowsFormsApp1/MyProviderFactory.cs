using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
// Необходимо для того, чтобы иметь определения общих интерфейсов 
//и различные объекты подключения 
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Configuration;
using System.Data.Common;

namespace AppConnectionFactory
{
    public class MyProviderFactory
    {
        static public DataSet GetMyConn()
        {
            // Получение строки подключения и поставщика из App.config
            string dataProvString = ConfigurationManager.AppSettings["provider"];
            string connectionString = ConfigurationManager.AppSettings["connectionStr"];
            // Получение генератора поставщика
            DbProviderFactory df = DbProviderFactories.GetFactory(dataProvString);
            // Получение объекта подключения
            DbConnection conn = df.CreateConnection();

            conn.ConnectionString = connectionString;
            conn.Open();

            DbCommand cmd = df.CreateCommand();
            cmd.Connection = conn;
            DataSet ds = new DataSet();
            // Из БД  используем таблицы Туристы, Информация о туристах, Туры

            cmd.CommandText = "select [Серия паспорта], Фамилия, Имя, Отчество, Фото from Туристы";
            DbDataAdapter dataadapter1 = df.CreateDataAdapter();
            dataadapter1.SelectCommand = cmd;
            dataadapter1.TableMappings.Add("Table", "Туристы");
            dataadapter1.Fill(ds);

            cmd.CommandText = "select [Код туриста], [Серия паспорта], Город, Страна, Телефон, Индекс  from  [Информация о туристах]";
            DbDataAdapter dataadapter2 = df.CreateDataAdapter();
            dataadapter2.SelectCommand = cmd;
            dataadapter2.TableMappings.Add("Table", "Информация о туристах");
            dataadapter2.Fill(ds);

            DataColumn dcTouristsID = ds.Tables["Туристы"].Columns["Серия паспорта"];
            DataColumn dcInfoTouristsID = ds.Tables["Информация о туристах"].Columns["Серия паспорта"];
            DataRelation dataRelation = new DataRelation("Дополнительная информация", dcTouristsID, dcInfoTouristsID);
            ds.Relations.Add(dataRelation);

            cmd.CommandText = "select * from Туры";
            DbDataAdapter dataadapter3 = df.CreateDataAdapter();
            dataadapter3.SelectCommand = cmd;
            dataadapter3.TableMappings.Add("Table", "Туры");
            dataadapter3.Fill(ds);

            return ds;

        }
    }
}
