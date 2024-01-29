using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bank.Pages
{
    /// <summary>
    /// Логика взаимодействия для Cabinet.xaml
    /// </summary>
    public partial class Cabinet : Window
    {
        int IdUser = 0;
        public Cabinet(int id)
        {
            InitializeComponent();
            string Connection = @"Data Source=msi;Initial Catalog=Bank;Integrated Security=True";
            SqlConnection sc = new SqlConnection(Connection);
            string select = $"select * from [User] where IDUser = " + id;
            SqlCommand sqlCommand = new SqlCommand(select, sc);
            sc.Open();
          
            sqlCommand.ExecuteNonQuery();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                IdUser = reader.GetInt32(0);
                lbname.Content = reader.GetString(3) + " " + reader.GetString(4) + " " + reader.GetString(5);
                lb_address.Content = reader.GetString(9);
                lb_data_rojdenia.Content = reader.GetDateTime(13);
                lb_mesto_rojdenia.Content= reader.GetString(14);
                lb_pochta.Content = reader.GetString(10);
                lb_telefon.Content = reader.GetString(8);
            }
            reader.Close();
            select = "Select History.NameOperation as \"Операция\", History.DateTime as \"Дата\", History.Amount as \"Сумма\" from History\r\njoin BankAccount on History.Account = BankAccount.NumberAccount\r\njoin [User] on [User].IDUser = BankAccount.UserID where [User].IDUser = " + IdUser;
            DataTable dt = new DataTable();
            using (var da = new SqlDataAdapter(select, Connection))
            {
                da.Fill(dt);
            }
            DataGridHistory.DataContext = dt;
            select = "select BankAccount.NumberAccount from BankAccount join [User] on [User].IDUser = BankAccount.UserID where [User].IDUser = " + IdUser;
            sqlCommand = new SqlCommand(select, sc);
            sqlCommand.ExecuteNonQuery();
            reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                lb_account.Items.Add(reader.GetString(0));
            }
        }


        private void Search(object sender, RoutedEventArgs e)
        {
            DateTime? selected = DatePicker.SelectedDate;
            string Connection = @"Data Source=msi;Initial Catalog=Bank;Integrated Security=True";
            DataTable dt = new DataTable();
            if (selected.HasValue)
            {
                int year = Convert.ToInt32(selected.Value.ToString("yyyy", System.Globalization.CultureInfo.InvariantCulture));
                int month = Convert.ToInt32(selected.Value.ToString("MM", System.Globalization.CultureInfo.InvariantCulture));
                int day = Convert.ToInt32(selected.Value.ToString("dd", System.Globalization.CultureInfo.InvariantCulture));
                string select = "select History.NameOperation as \"Операция\", History.DateTime as \"Дата\", History.Amount as \"Сумма\" from History join BankAccount on History.Account = BankAccount.NumberAccount join [User] on [User].IDUser = BankAccount.UserID where day(History.DateTime) = " + day + " and month(History.DateTime) = " + month + " and year(History.DateTime) = " + year + "and [User].IDUser = " + IdUser;
                using (var da = new SqlDataAdapter(select, Connection))
                {
                    da.Fill(dt);
                }
                DataGridHistory.DataContext = dt;
            }

        }

        private void Clear (object sender, RoutedEventArgs e)
        {
            string Connection = @"Data Source=msi;Initial Catalog=Bank;Integrated Security=True";
            string select = "Select History.NameOperation as \"Операция\", History.DateTime as \"Дата\", History.Amount as \"Сумма\" from History\r\njoin BankAccount on History.Account = BankAccount.NumberAccount\r\njoin [User] on [User].IDUser = BankAccount.UserID where [User].IDUser = " + IdUser;
            DataTable dt = new DataTable();
            using (var da = new SqlDataAdapter(select, Connection))
            {
                da.Fill(dt);
            }
            DataGridHistory.DataContext = dt;
        }
    }
}
