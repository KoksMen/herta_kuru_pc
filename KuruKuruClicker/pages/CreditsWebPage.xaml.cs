using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KuruKuruClicker.pages
{
    /// <summary>
    /// Логика взаимодействия для CreditPage.xaml
    /// </summary>
    public partial class CreditsWebPage : UserControl
    {
        public event Action PageClosed;
        public List<Developer> Developers { get; set; }
        public CreditsWebPage()
        {
            InitializeComponent();
            LoadDevelopers();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PageClosed?.Invoke();
        }

        private void LoadDevelopers()
        {
            string jsonFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KuruKuruClicker\CreditsWeb\credits.web.json";
            string jsonData = File.ReadAllText(jsonFilePath);
            var data = JsonConvert.DeserializeObject<Data>(jsonData);
            Developers = data.contributors;
        }
    }

    public class Data
    {
        public List<Developer> contributors { get; set; }
    }

    public class Developer
    {
        public string username { get; set; }
        public string name { get; set; }
        public string thing { get; set; }
        public SocialMedia socialmedia { get; set; }
        public string icon { get; set; }
    }

    public class SocialMedia
    {
        public string github { get; set; }
        public string bilibili { get; set; }
        public string twitter { get; set; }
    }
}
