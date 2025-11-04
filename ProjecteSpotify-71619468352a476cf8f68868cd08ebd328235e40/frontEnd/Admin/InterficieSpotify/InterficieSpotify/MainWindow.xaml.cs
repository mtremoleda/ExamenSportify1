using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InterficieSpotify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Songs songs = new Songs();
            this.Close();
            songs.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Users users = new Users();
            this.Close();
            users.Show();


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            PlayLists playlists = new PlayLists();
            this.Close();
            playlists.Show();

        }
    }
}