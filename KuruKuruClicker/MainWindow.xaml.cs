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

            var hertaImage = new BitmapImage(new Uri($"img/hertaa_github.gif", UriKind.Relative));
            ImageBehavior.SetAnimatedSource(HertaBackgroundGif, hertaImage);

            SetLanguageEnglish();
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

        string[] squishInfoTexts; 
        string[] squishButtonTexts; 
        private void SetLanguageEnglish()
        {
            welcometb.Text = "Welcome to Herta Kuru Kururing";
            siteInfoTB.Text = $"The website for Herta, the {"(annoying)"} cutest genius Honkai:  Star Rail character out there.  ";
            squishInfoTexts = new string[2]
            {
                "The kuru~ has been squished for",
                "Herta has been kuru~ed for",
            };
            squishInfoTB.Text = squishInfoTexts[0];
            timesTB.Text = "times";
            squishButtonTexts = new string[2]
            {
                "Squish the kuru~!",
                "Kuru kuru~!",
            };
            squishButton.Content = squishButtonTexts[0];
            gifMadeTB.Text = "Herta GIF:";
            pcAuthorTB.Text = "PC Verion Author:";
            pcRepoTB.Text = "GitHub Repo PC:";
            siteRepoTB.Text = "GitHub Repo Site:";
        }

        private void SetLanguageChinise()
        {
            welcometb.Text = "欢迎来到赫塔*库鲁*库鲁林";
            siteInfoTB.Text = "赫塔的网站,最可爱的天才韩凯:星轨人物在那里.  ";
            squishInfoTexts = new string[2]
            {
                "库鲁~已经被压扁了。",
                "赫塔已经被库鲁-埃德了。",
            };
            squishInfoTB.Text = squishInfoTexts[0];
            timesTB.Text = "时代";
            squishButtonTexts = new string[2]
            {
                "转圈圈~",
                "咕噜噜! ",
            };
            squishButton.Content = squishButtonTexts[0];
            gifMadeTB.Text = "赫塔GIF:";
            pcAuthorTB.Text = "PC版作者:";
            pcRepoTB.Text = "GitHub回购PC:";
            siteRepoTB.Text = "GitHub回购站点:";
        }
    
        private void SetLanguageJapanise()
        {
            welcometb.Text = "ヘルタクルクルリングへようこそ";
            siteInfoTB.Text = "ヘルタのためのウェブサイト、（迷惑な）かわいい天才Honkai：そこにスターレールキャラクター。  ";
            squishInfoTexts = new string[2]
            {
                "クル～のために押しつぶされてきました",
                "ヘルタはクル-エドのためにされています",
            };
            squishInfoTB.Text = squishInfoTexts[0];
            timesTB.Text = "回";
            squishButtonTexts = new string[2]
            {
                "クルを潰して～！",
                "クル クル~!",
            };
            squishButton.Content = squishButtonTexts[0];
            gifMadeTB.Text = "ヘルタ-ギフ:";
            pcAuthorTB.Text = "PC版作者:";
            pcRepoTB.Text = "GitHubリポジトリPC:";
            siteRepoTB.Text = "GitHubレポサイト:";
        }
        private void SetLanguageKorean()
        {
            welcometb.Text = "헤르타 쿠루 쿠루링에 오신 것을 환영합니다";
            siteInfoTB.Text = "헤르타에 대한 웹 사이트,(성가신)귀여운 천재 혼카이:거기에 스타 레일 문자.  ";
            squishInfoTexts = new string[2]
            {
                "쿠루~는",
                "헤르타는 쿠루에드였어",
            };
            squishInfoTB.Text = squishInfoTexts[0];
            timesTB.Text = "시간";
            squishButtonTexts = new string[2]
            {
                "쿠루를 뭉개 버려~!",
                "빙글 빙글~!",
            };
            squishButton.Content = squishButtonTexts[0];
            gifMadeTB.Text = "헤르타 지프:";
            pcAuthorTB.Text = "버전 작성자:";
            pcRepoTB.Text = "기투브 레포 컴퓨터:";
            siteRepoTB.Text = "지투브 레포 사이트:";
        }

        private void refreshDynamicTexts()
        {
            squishButton.Content = squishButtonTexts[(new Random()).Next(0,2)];
            squishInfoTB.Text = squishInfoTexts[(new Random()).Next(0,2)];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ModlaPage.Content = new pages.CreditPage();
            Process.Start(new ProcessStartInfo("https://twitter.com/Seseren_kr"));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://steamcommunity.com/id/KoksMen/"));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/KoksMen/herta_kuru_pc"));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/duiqt/herta.kuru"));
        }

    }
}
