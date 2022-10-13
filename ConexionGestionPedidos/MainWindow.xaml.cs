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
            MuestraTodosPedidos();
        }

        private void MuestraClientes()
        {
            try
            {
                string query = "SELECT * FROM Clientes";
                SqlDataAdapter AdapterSql = new SqlDataAdapter(query, ConnectionSql);

                using (AdapterSql)
                {
                    DataTable ClientesTabla = new DataTable();
                    AdapterSql.Fill(ClientesTabla);
                    ListaClientes.DisplayMemberPath = "Nombre";
                    ListaClientes.SelectedValuePath = "Id";
                    ListaClientes.ItemsSource = ClientesTabla.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            
            
        }

        private void MuestraPedidos()
        {
            try
            {
                string query = "SELECT * FROM Pedidos P INNER JOIN Clientes C ON C.Id = P.CodigoCliente WHERE C.Id=@ClienteId";
                SqlCommand CommandSql = new SqlCommand(query, ConnectionSql);
                SqlDataAdapter AdapterSql = new SqlDataAdapter(CommandSql);

                using (AdapterSql)
                {
                    CommandSql.Parameters.AddWithValue("@ClienteId", ListaClientes.SelectedValue);
                    DataTable PedidosTabla = new DataTable();
                    AdapterSql.Fill(PedidosTabla);
                    PedidosCliente.DisplayMemberPath = "FechaPedido";
                    PedidosCliente.SelectedValuePath = "Id";
                    PedidosCliente.ItemsSource = PedidosTabla.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void MuestraTodosPedidos()
        {
            try
            {
                string query = "SELECT *,CONCAT(CodigoCliente,' ',FechaPedido, ' ',FormaPago) AS PedidosCompletos FROM Pedidos ";
                SqlDataAdapter AdapterSql = new SqlDataAdapter(query, ConnectionSql);

                using (AdapterSql)
                {
                    DataTable TodosPedidosTabla = new DataTable();
                    AdapterSql.Fill(TodosPedidosTabla);
                    TodosPedidos.DisplayMemberPath = "PedidosCompletos";
                    TodosPedidos.SelectedValuePath = "Id";
                    TodosPedidos.ItemsSource = TodosPedidosTabla.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        //private void ListaClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    MuestraPedidos();
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(TodosPedidos.SelectedValue.ToString());

            string query = "DELETE FROM Pedidos WHERE  Id=@PedidoId";
            SqlCommand CommandSql = new SqlCommand(query,ConnectionSql);
            
            ConnectionSql.Open();
            CommandSql.Parameters.AddWithValue("@PedidoId", TodosPedidos.SelectedValue);
            CommandSql.ExecuteNonQuery();
            ConnectionSql.Close();

            MuestraTodosPedidos();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string query = "INSERT INTO Clientes (Nombre) VALUES (@Nombre)";
            SqlCommand CommandSql = new SqlCommand(query, ConnectionSql);

            ConnectionSql.Open();
            CommandSql.Parameters.AddWithValue("@Nombre", InsertarCliente.Text);
            CommandSql.ExecuteNonQuery();
            ConnectionSql.Close();

            MuestraClientes();

            InsertarCliente.Text = "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string query = "DELETE FROM Clientes WHERE  Id=@ClienteId";
            SqlCommand CommandSql = new SqlCommand(query, ConnectionSql);

            ConnectionSql.Open();
            CommandSql.Parameters.AddWithValue("@ClienteId", ListaClientes.SelectedValue);
            CommandSql.ExecuteNonQuery();
            ConnectionSql.Close();

            MuestraClientes();
        }

        private void ListaClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MuestraPedidos();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ActualizarCliente VentanaActualizar = new ActualizarCliente((int)ListaClientes.SelectedValue);
            VentanaActualizar.Show();
            try
            {
                string query = "SELECT Nombre FROM Clientes WHERE Id=@ClienteId";
                SqlCommand CommandSql = new SqlCommand(query, ConnectionSql);
                SqlDataAdapter AdapterSql = new SqlDataAdapter(CommandSql);

                using (AdapterSql)
                {
                    CommandSql.Parameters.AddWithValue("@ClienteId", ListaClientes.SelectedValue);
                    DataTable ClientesTabla = new DataTable();
                    AdapterSql.Fill(ClientesTabla);
                    VentanaActualizar.TextBoxActualizar.Text= ClientesTabla.Rows[0]["Nombre"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //VentanaActualizar.ShowDialog();

            //MuestraClientes();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            MuestraClientes();
        }
    }
}
