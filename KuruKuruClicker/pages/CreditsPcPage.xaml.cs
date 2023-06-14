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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KuruKuruClicker.pages
{
    /// <summary>
    /// Логика взаимодействия для CreditsPcPage.xaml
    /// </summary>
    public partial class CreditsPcPage : UserControl
    {
        public event Action PageClosed;
        public List<DeveloperPC> Developers { get; set; }
        public CreditsPcPage()
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
            string jsonFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KuruKuruClicker\CreditsPC\credits.pc.json";
            string jsonData = File.ReadAllText(jsonFilePath);
            var data = JsonConvert.DeserializeObject<DataPC>(jsonData);
            Developers = data.contributors;
        }
    }

    public class DataPC
    {
        public List<DeveloperPC> contributors { get; set; }
    }

    public class DeveloperPC
    {
        public string username { get; set; }
        public string name { get; set; }
        public string thing { get; set; }
        public SocialMediaPC socialmedia { get; set; }
        public string icon { get; set; }
    }

    public class SocialMediaPC
    {
        public string github { get; set; }
        public string bilibili { get; set; }
        public string twitter { get; set; }
    }
}
