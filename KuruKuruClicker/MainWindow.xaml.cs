using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Media;
using WpfAnimatedGif;
using System.Windows.Media.Animation;
using System.Runtime.CompilerServices;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Text;

namespace KuruKuruClicker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool firstStart = true;

        private int count;
        public int Count
        {
            get { return count; }
            set 
            { 
                count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        private bool firstSquish = true;

        public MainWindow()
        {
            InitializeComponent();
            firstStart = false;

            DataContext = this;

            LoadFiles();

            Count = 0;

            SetLanguageEnglish();

            var hertaImage = new BitmapImage(new Uri($"img/hertaa_github.gif", UriKind.Relative));
            ImageBehavior.SetAnimatedSource(HertaBackgroundGif, hertaImage);

        }

        private static void LoadFiles()
        {
            try
            {
                if (!Directory.Exists(hertaAudioFolder))
                {
                    Directory.CreateDirectory(hertaAudioFolder);
                }
                string[] files = Directory.GetFiles(hertaAudioFolder);
                if (!files.Contains("kuru1.wav") || !files.Contains("kuru2.wav") || !files.Contains("kuruto.wav"))
                {
                    using (Stream waveFile = Properties.Resources.kuruto)
                    {
                        using (var fileStream = new FileStream(hertaKuruToAudio, FileMode.Create, FileAccess.Write))
                        {
                            waveFile.CopyTo(fileStream);
                        }
                    }

                    using (Stream waveFile = Properties.Resources.kuru1)
                    {
                        using (var fileStream = new FileStream(hertaKuruRingAudio, FileMode.Create, FileAccess.Write))
                        {
                            waveFile.CopyTo(fileStream);
                        }
                    }

                    using (Stream waveFile = Properties.Resources.kuru2)
                    {
                        using (var fileStream = new FileStream(hertaKuruKuruAudio, FileMode.Create, FileAccess.Write))
                        {
                            waveFile.CopyTo(fileStream);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void SquishButton_Click(object sender, RoutedEventArgs e)
        {
            Count++;

            PlayKuru();
            AnimateHerta();
            refreshDynamicTexts();
        }

        private void AnimateHerta()
        {
            Random random = new Random();
            int randomIndex = random.Next(1, 3);

            Image hertaImageElement = new Image
            {
                Width = 200,
                Height = 200,
                RenderTransform = new TranslateTransform(500, 0),
                RenderTransformOrigin = new Point(0.5, 0.5),
                VerticalAlignment = VerticalAlignment.Bottom
            };

            var hertaImage = new BitmapImage(new Uri($"img/hertaa{randomIndex}.gif", UriKind.Relative));

            ImageBehavior.SetAnimatedSource(hertaImageElement, hertaImage);

            HertaShowerGrid.Children.Add(hertaImageElement);
            Grid.SetZIndex(hertaImageElement, -1);

            DoubleAnimation hertaMoveAnimation = new DoubleAnimation
            {
                To = -500,
                Duration = TimeSpan.FromSeconds(1),
            };

            TranslateTransform hertaTransformAnimation = hertaImageElement.RenderTransform as TranslateTransform;
            hertaTransformAnimation.BeginAnimation(TranslateTransform.XProperty, hertaMoveAnimation);

            hertaTransformAnimation.Changed += (s, e) =>
            {
                if (hertaTransformAnimation.X == -500)
                {
                    HertaShowerGrid.Children.Remove(hertaImageElement);
                }
            };
        }

        static string hertaAudioFolder = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KuruKuruClicker\audio";
        static string hertaKuruToAudio = System.IO.Path.Combine(hertaAudioFolder, "kuruto.wav");
        static string hertaKuruRingAudio = System.IO.Path.Combine(hertaAudioFolder, "kuru1.wav");
        static string hertaKuruKuruAudio = System.IO.Path.Combine(hertaAudioFolder, "kuru2.wav");

        static string[] hertaAudio = new string[3] { hertaKuruToAudio, hertaKuruRingAudio, hertaKuruKuruAudio };

        private void PlayKuru()
        {
            try
            {
                if (firstSquish)
                {
                    MediaElement player = new MediaElement();
                    player.LoadedBehavior = MediaState.Manual;
                    player.Visibility = Visibility.Hidden;
                    player.Source = new Uri(hertaAudio[0]);

                    HertaShowerGrid.Children.Add(player);
                    Grid.SetZIndex(player, -2);

                    player.MediaEnded += (sender, e) =>
                    {
                        HertaShowerGrid.Children.Remove(player);
                        player = null;
                    };

                    player.Play();

                    firstSquish = false;
                }
                else
                {
                    MediaElement player = new MediaElement();
                    player.LoadedBehavior = MediaState.Manual;
                    player.Visibility = Visibility.Hidden;
                    player.Source = new Uri(hertaAudio[(new Random()).Next(0, 3)]);

                    HertaShowerGrid.Children.Add(player);
                    Grid.SetZIndex(player, -2);

                    player.MediaEnded += (sender, e) =>
                    {
                        HertaShowerGrid.Children.Remove(player);
                        player = null;
                    };

                    player.Play();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void TwitterGif_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void SteamKoks_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void GitHubRepoKoks_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void GitHubRepoOriginal_RequestNavigate_1(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void languageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!firstStart)
            {
                int selectedIndex = languageComboBox.SelectedIndex;
                switch (selectedIndex)
                {
                    case 0:
                        {
                            SetLanguageEnglish();
                            break;
                        }
                    case 1:
                        {
                            SetLanguageChinise();
                            break;
                        }
                    case 2:
                        {
                            SetLanguageJapanise();
                            break;
                        }
                    case 3:
                        {
                            SetLanguageKorean();
                            break;
                        }
                }
            }
            
        }

        string[] squishButtonTexts; 
        private void SetLanguageEnglish()
        {
            welcometb.Text = "Welcome to Herta Kuru Kururing";
            siteInfoTB.Text = $"The website for Herta, the {"(annoying)"} cutest genius Honkai:  Star Rail character out there.  ";
            //TODO dynamic text
            squishInfoTB.Text = "The kuru~ has been squished";
            timesTB.Text = "times";
            squishButtonTexts = new string[2]
            {
                "Squish the kuru~!",
                "Kuru kuru~!",
            };
            squishButton.Content = squishButtonTexts[0];
        }

        private void SetLanguageChinise()
        {
            welcometb.Text = "欢迎来到赫塔*库鲁*库鲁林";
            siteInfoTB.Text = "赫塔的网站,最可爱的天才韩凯:星轨人物在那里.  ";
            //TODO dynamic text
            squishInfoTB.Text = "库鲁~被压扁了";
            timesTB.Text = "时代";
            squishButtonTexts = new string[2]
            {
                "转圈圈~",
                "咕噜噜! ",
            };
            squishButton.Content = squishButtonTexts[0];
        }
    
        private void SetLanguageJapanise()
        {
            welcometb.Text = "ヘルタクルクルリングへようこそ";
            siteInfoTB.Text = "ヘルタのためのウェブサイト、（迷惑な）かわいい天才Honkai：そこにスターレールキャラクター。  ";
            //TODO dynamic text
            squishInfoTB.Text = "クル～が潰されてしまった";
            timesTB.Text = "回";
            squishButtonTexts = new string[2]
            {
                "クルを潰して～！",
                "クル クル~!",
            };
            squishButton.Content = squishButtonTexts[0];
        }
        private void SetLanguageKorean()
        {
            welcometb.Text = "헤르타 쿠루 쿠루링에 오신 것을 환영합니다";
            siteInfoTB.Text = "헤르타에 대한 웹 사이트,(성가신)귀여운 천재 혼카이:거기에 스타 레일 문자.  ";
            //TODO dynamic text
            squishInfoTB.Text = "쿠루~이 찌그러졌다";
            timesTB.Text = "시간";
            squishButtonTexts = new string[2]
            {
                "쿠루를 뭉개 버려~!",
                "빙글 빙글~!",
            };
            squishButton.Content = squishButtonTexts[0];
        }

        private void refreshDynamicTexts()
        {
            squishButton.Content = squishButtonTexts[(new Random()).Next(0,2)];
        }
    }
}
