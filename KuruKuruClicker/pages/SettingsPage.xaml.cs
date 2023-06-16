using KuruKuruClicker.Properties;
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

namespace KuruKuruClicker.pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        public event Action<int> TextLanguageChanged;
        public event Action<int> AudioLanguageChanged;
        public event Action SettingsPageClosed;
        public SettingsPage(int currentLanguageIndex, int currentAudioLanguageIndex)
        {
            InitializeComponent();

            DataContext = this;

            languageComboBox.SelectedIndex = currentLanguageIndex;
            audiolanguageComboBox.SelectedIndex = currentAudioLanguageIndex;
        }

        private void languageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = languageComboBox.SelectedIndex;
            Settings.Default.TextLanguage = selectedIndex;
            TextLanguageChanged?.Invoke(selectedIndex);
        }
        private void audiolanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = audiolanguageComboBox.SelectedIndex;
            Settings.Default.AudioLanguage = selectedIndex;
            AudioLanguageChanged?.Invoke(selectedIndex);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsPageClosed?.Invoke();
        }

    }
}
