using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace ConexionGestionPedidos
{
    /// <summary>
    /// Lógica de interacción para ActualizarCliente.xaml
    /// </summary>
    public partial class ActualizarCliente : Window
    {
        SqlConnection ConnectionSql;
        private int ClienteId;

        public ActualizarCliente(int Id)
        {
            InitializeComponent();

            ClienteId = Id;
            string Connection = ConfigurationManager.ConnectionStrings["ConexionGestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            ConnectionSql = new SqlConnection(Connection);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string query = "UPDATE Clientes SET Nombre=@Nombre WHERE Id=" + ClienteId;
            SqlCommand CommandSql = new SqlCommand(query, ConnectionSql);

            ConnectionSql.Open();
            CommandSql.Parameters.AddWithValue("@Nombre", TextBoxActualizar.Text);
            CommandSql.ExecuteNonQuery();
            ConnectionSql.Close();

            this.Close(); 
        }
    }
}
