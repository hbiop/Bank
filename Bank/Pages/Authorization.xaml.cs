using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace Bank.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void Enter(object sender, RoutedEventArgs e)
        {
            string Connection = @"Data Source=msi;Initial Catalog=Bank;Integrated Security=True";
            SqlConnection sc = new SqlConnection(Connection);
            string select = $"select IDUser from [User] where Login like '"+tb_login.Text+"' and Passwoord like '"+tb_parol.Password+"'";
            SqlCommand sqlCommand = new SqlCommand(select, sc);
            sc.Open();
            sqlCommand.ExecuteNonQuery();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if(reader.Read())
            {
                int id = Convert.ToInt32(reader.GetValue(0));
                Cabinet cabinet = new Cabinet(id);
                cabinet.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Такого пользователя не существует");
            }
        }
    }
}
