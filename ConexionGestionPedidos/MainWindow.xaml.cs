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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ConexionGestionPedidos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection ConnectionSql;

        public MainWindow()
        {
            InitializeComponent();

            string Connection = ConfigurationManager.ConnectionStrings["ConexionGestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            ConnectionSql = new SqlConnection(Connection);

            MuestraClientes();
        }

        private void MuestraClientes()
        {
            string query = "SELECT * FROM Clientes";
            SqlDataAdapter AdapterSql = new SqlDataAdapter(query,ConnectionSql);

            using (AdapterSql)
            {
                DataTable ClientesTabla = new DataTable();
                AdapterSql.Fill(ClientesTabla);
                ListaClientes.DisplayMemberPath = "Nombre";
                ListaClientes.SelectedValuePath = "Id";
                ListaClientes.ItemsSource = ClientesTabla.DefaultView;
            }
        }

        private void MuestraPedidos()
        {
            string query = "SELECT * FROM Pedidos P INNER JOIN Clientes C ON C.Id = P.CodigoCliente WHERE C.Id=@ClienteId";
            SqlCommand CommandSql = new SqlCommand(query,ConnectionSql);
            SqlDataAdapter AdapterSql = new SqlDataAdapter(CommandSql);

            using (AdapterSql)
            {
                CommandSql.Parameters.AddWithValue("@ClienteId",ListaClientes.SelectedValue);
                DataTable PedidosTabla = new DataTable();
                AdapterSql.Fill(PedidosTabla);
                PedidosCliente.DisplayMemberPath = "FechaPedido";
                PedidosCliente.SelectedValuePath = "Id";
                PedidosCliente.ItemsSource = PedidosTabla.DefaultView;
            }
        }

        private void ListaClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraPedidos();
        }
    }
}
