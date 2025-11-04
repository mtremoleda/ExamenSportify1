using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using UI.Spotify.MODELS;

namespace InterficieSpotify
{
    public partial class Users : Window
    {
        public Users()
        {
            InitializeComponent();
        }

        private async void CarregarDades_Click(object sender, RoutedEventArgs e)
        {
            await CargarUsuarisAsync();
        }

        private async Task CargarUsuarisAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "https://localhost:56832/usuaris";

                    

                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        var opciones = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true //ignora les majuscules/minuscules a l'hora d'emaprellar-lo amb la calsse.

                        };
                        var usuaris = JsonSerializer.Deserialize<List<Usuari>>(json, opciones);


                        dgUsers.ItemsSource = usuaris; // Aqui emplenem el DataGrid
                    }
                    else
                    {
                        MessageBox.Show("Error al obtener los datos de la API");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error de red: {ex.Message}");
            }
        }

        private void Enrrere_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Close();
            main.Show();
        }
    }
}
