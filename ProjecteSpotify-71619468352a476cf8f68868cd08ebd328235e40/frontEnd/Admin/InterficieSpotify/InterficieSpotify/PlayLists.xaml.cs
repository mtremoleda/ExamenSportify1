using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using UI.Spotify.MODELS;

namespace InterficieSpotify
{
    public partial class PlayLists : Window
    {
        public PlayLists()
        {
            InitializeComponent();
        }

        private async void CarregarDades_Click(object sender, RoutedEventArgs e)
        {
            await CargarPlaylistsAsync();
        }

        private async Task CargarPlaylistsAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "https://localhost:56832/playlists";
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        var opciones = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true //ignora les majuscules/minuscules a l'hora d'emaprellar-lo amb la calsse.

                        };
                        var playlists = JsonSerializer.Deserialize<List<LlistaReproduccio>>(json, opciones);


                        dgPlayLists.ItemsSource = playlists; // Aqui emplenem el DataGrid
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
