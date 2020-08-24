using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CurrencyScanner
{
    public partial class Form1 : Form
    {
        ILogger logger;
        SqlConnection connection;
        Dictionary<string, string> hashTickers = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
            logger = new FormLogger(lbLog, errorProvider1);

            //TODO - через Configuration
            string datasource = @".\SQLEXPRESS";
            string dbName = "currencyDb";
            string connectionString = $"Data Source={datasource};Initial Catalog={dbName};Integrated Security=True";

            connection = new SqlConnection(connectionString);
        }

        public CurrencyList ParseDocument(string url)
        {
            WebRequest request = WebRequest.Create(new Uri(url));
            HttpWebResponse webResponce = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(CurrencyList));
            CurrencyList res = null;
            using (Stream stream = webResponce.GetResponseStream())
            {
                using (StreamReader rd = new StreamReader(stream, Encoding.Default))
                {
                    res = (CurrencyList)serializer.Deserialize(rd);
                    logger.Log($"с сервера получены значения для {res.Currencies.Count} валют");
                }
            }
            webResponce.Close();

            return res;
        }

        public void InitTables()
        {
             new SqlCommandNonqueryHelper(logger, connection, @"
                        CREATE TABLE Currencies
                        (
                        id_cur INTEGER PRIMARY KEY IDENTITY NOT NULL,
                        name CHAR(50) NOT NULL,
                        ticker CHAR(3) NOT NULL
                        );
                    ").TryExecute();

            new SqlCommandNonqueryHelper(logger, connection, @"    
                        CREATE TABLE Prices
                        (
                        id_price INTEGER PRIMARY KEY IDENTITY,
                        price FLOAT NOT NULL,
                        daydate DATE,
                        id_cur INTEGER NOT NULL FOREIGN KEY(id_cur) REFERENCES Currencies(id_cur)  
                        ); 
                    ").TryExecute();
        }

        public void UpdateDbForDate(DateTime date)
        {
            try
            {
                string dateTime = date.ToString("dd.MM.yyyy");
                string url = $"http://www.cbr.ru/scripts/xml_daily.asp?date_req={dateTime}";
                var model = ParseDocument(url);
                int year = date.Year;
                int mon = date.Month;
                int day = date.Day;

                foreach (var currency in model.Currencies)
                {
                    new SqlCommandNonqueryHelper(logger, connection, $@"IF(EXISTS (SELECT * FROM Currencies WHERE ticker = '{currency.Ticker}'))
                                            BEGIN
                                                UPDATE Currencies
                                                SET name = '{currency.Fullname}', ticker = '{currency.Ticker}'
                                                WHERE  ticker = '{currency.Ticker}'
                                            END
                                            ELSE
                                            BEGIN
                                                INSERT INTO Currencies (name, ticker)
                                                VALUES ('{currency.Fullname}', '{currency.Ticker}')
                                            END").TryExecute();


                    object id_cur = null;
                    using (SqlDataReader rd = new SqlCommandReaderHelper(logger, connection, $"SELECT * FROM Currencies WHERE Ticker = '{currency.Ticker}'").TryExecute() as SqlDataReader)
                    {
                        if (rd != null && rd.Read())
                        {
                            id_cur = rd.GetValue(0);
                            string ticker = rd.GetValue(2).ToString();
                            if (!hashTickers.ContainsKey(ticker))
                            {
                                hashTickers.Add(ticker, ticker);
                                lbTickers.Items.Add(ticker);
                            }
                        }
                    }

                    bool exists = false;
                    using (SqlDataReader rd = new SqlCommandReaderHelper(logger, connection, $"SELECT * FROM Currencies JOIN Prices ON Currencies.id_cur = Prices.id_cur").TryExecute() as SqlDataReader)
                    {
                        exists = rd!= null && rd.HasRows && rd.Read();
                    }


                    if (exists)
                        new SqlCommandNonqueryHelper(logger, connection, $@"IF(EXISTS (SELECT * FROM Prices WHERE id_cur = {id_cur} AND daydate = DATEFROMPARTS({year}, {mon}, {day})))
                                            BEGIN
                                                UPDATE Prices
                                                SET price = '{currency.Price.ToString().Replace(',', '.')}', daydate = DATEFROMPARTS({year}, {mon}, {day}), id_cur = {id_cur}
                                                WHERE  id_cur = {id_cur} AND daydate = DATEFROMPARTS({year}, {mon}, {day})
                                            END
                                            ELSE
                                            BEGIN
                                                INSERT INTO Prices (price, daydate, id_cur)
                                                VALUES ({currency.Price.ToString().Replace(',', '.')}, DATEFROMPARTS({year}, {mon}, {day}), {id_cur})
                                            END").TryExecute();
                    else
                        new SqlCommandNonqueryHelper(logger, connection, $@"INSERT INTO Prices (price, daydate, id_cur) VALUES ({currency.Price.ToString().Replace(',', '.')}, DATEFROMPARTS({year}, {mon}, {day}), {id_cur})").TryExecute();

                }

                if (lbTickers.Items.Count > 0 && lbTickers.SelectedIndex < 0)
                    lbTickers.SelectedIndex = 0;
               
            }
            catch
            {
                logger.Log("Ошибка при загрузке данных", true);
            }
        }

        public double GetPrice(string ticker, DateTime date)
        {
            int year = date.Year;
            int mon = date.Month;
            int day = date.Day;
            object price = null;
            using (SqlDataReader rd = new SqlCommandReaderHelper(logger, connection, $@"SELECT price FROM Currencies JOIN Prices ON Currencies.id_cur = Prices.id_cur  
                                                                                        WHERE (Currencies.ticker = '{ticker}' AND Prices.daydate = DATEFROMPARTS({year}, {mon}, {day}))").TryExecute() as SqlDataReader)
            {
                if (rd != null && rd.Read())
                {
                    price = rd.GetValue(0);
                }
                else 
                {
                    throw new EntryNotFoundException($"entry for date ${date.Date} not found");
                }
            }
            return Convert.ToDouble(price);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                logger.Log("Подключение открыто");

                InitTables();
                UpdateDbForDate(DateTime.Now);
                label1.Text = "Курс на выбранную дату: " + GetPrice(lbTickers.SelectedItem.ToString(), calend.SelectionEnd).ToString() + " руб.";
            }
            catch (SqlException ex)
            {
                logger.Log(ex.Message, true);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateDbForDate(DateTime.Now);
            }
            catch (SqlException ex)
            {
                logger.Log(ex.Message, true);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            connection.Close();
        }

        private void btLoadPrice_Click(object sender, EventArgs e)
        {
            try
            {
                label1.Text = "Курс на выбранную дату: " + GetPrice(lbTickers.SelectedItem.ToString(), calend.SelectionEnd).ToString() + " руб.";
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message, true);
            }
        }
    }
}
