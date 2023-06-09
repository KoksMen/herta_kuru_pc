﻿using System;
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
using System.Xml.Linq;
using System.Text;
using KuruKuruClicker.pages;
using KuruKuruClicker.Properties;

namespace KuruKuruClicker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        static string hertaAudioFolder = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KuruKuruClicker\audio";
        static string hertaKuruToAudioJA = System.IO.Path.Combine(hertaAudioFolder, "kuruto.wav");
        static string hertaKuruRingAudioJA = System.IO.Path.Combine(hertaAudioFolder, "kuru1.wav");
        static string hertaKuruKuruAudioJA = System.IO.Path.Combine(hertaAudioFolder, "kuru2.wav");
        static string hertaKuruToAudioCN = System.IO.Path.Combine(hertaAudioFolder, "1转圈圈.wav");
        static string hertaKuruRingAudioCN = System.IO.Path.Combine(hertaAudioFolder, "2转圈圈咯.wav");
        static string hertaKuruKuruAudioCN = System.IO.Path.Combine(hertaAudioFolder, "3要坏掉了.wav");
        static string[] hertaAudio = new string[3];

        static string jsonCreditsFolderPC = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KuruKuruClicker\CreditsPC";
        static string jsonCreditsFolderWeb = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KuruKuruClicker\CreditsWeb";
        static string jsonCreditsPC = System.IO.Path.Combine(jsonCreditsFolderPC, "credits.pc.json");
        static string jsonCreditsWeb = System.IO.Path.Combine(jsonCreditsFolderWeb, "credits.web.json");


        private bool firstSquish = true;

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


        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            LoadAudioFiles();
            LoadJsonFiles();

            Count = Settings.Default.KuruCount;

            var hertaImage = new BitmapImage(new Uri($"img/hertaa_github.gif", UriKind.Relative));
            ImageBehavior.SetAnimatedSource(HertaBackgroundGif, hertaImage);

            ChangeLanguage(Settings.Default.TextLanguage);
            SelectAudioLanguage(Settings.Default.AudioLanguage);
        }

        private void SelectAudioLanguage(int audioLanguage)
        {
            if (audioLanguage == 0)
            {
                hertaAudio = new string[3] { hertaKuruToAudioJA, hertaKuruRingAudioJA, hertaKuruKuruAudioJA }; 
            }
            else if (audioLanguage == 1)
            {
                hertaAudio = new string[3] { hertaKuruToAudioCN, hertaKuruRingAudioCN, hertaKuruKuruAudioCN }; 
            }
        }

        private static void LoadAudioFiles()
        {
            try
            {
                if (!Directory.Exists(hertaAudioFolder))
                {
                    Directory.CreateDirectory(hertaAudioFolder);
                }
                string[] files = Directory.GetFiles(hertaAudioFolder);
                if (!files.Contains("kuru1.wav") || !files.Contains("kuru2.wav") || !files.Contains("kuruto.wav") ||
                    !files.Contains("1转圈圈.wav") || !files.Contains("2转圈圈咯.wav") || !files.Contains("3要坏掉了.wav"))
                {
                    using (Stream waveFile = Properties.Resources.kuruto)
                    {
                        using (var fileStream = new FileStream(hertaKuruToAudioJA, FileMode.Create, FileAccess.Write))
                        {
                            waveFile.CopyTo(fileStream);
                        }
                    }

                    using (Stream waveFile = Properties.Resources.kuru1)
                    {
                        using (var fileStream = new FileStream(hertaKuruRingAudioJA, FileMode.Create, FileAccess.Write))
                        {
                            waveFile.CopyTo(fileStream);
                        }
                    }

                    using (Stream waveFile = Properties.Resources.kuru2)
                    {
                        using (var fileStream = new FileStream(hertaKuruKuruAudioJA, FileMode.Create, FileAccess.Write))
                        {
                            waveFile.CopyTo(fileStream);
                        }
                    }

                    using (Stream waveFile = Properties.Resources._1转圈圈)
                    {
                        using (var fileStream = new FileStream(hertaKuruToAudioCN, FileMode.Create, FileAccess.Write))
                        {
                            waveFile.CopyTo(fileStream);
                        }
                    }

                    using (Stream waveFile = Properties.Resources._2转圈圈咯)
                    {
                        using (var fileStream = new FileStream(hertaKuruRingAudioCN, FileMode.Create, FileAccess.Write))
                        {
                            waveFile.CopyTo(fileStream);
                        }
                    }

                    using (Stream waveFile = Properties.Resources._3要坏掉了)
                    {
                        using (var fileStream = new FileStream(hertaKuruKuruAudioCN, FileMode.Create, FileAccess.Write))
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
        private static void LoadJsonFiles()
        {
            try
            {
                if (!Directory.Exists(jsonCreditsFolderPC))
                {
                    Directory.CreateDirectory(jsonCreditsFolderPC);
                }
                if (!Directory.Exists(jsonCreditsFolderWeb))
                {
                    Directory.CreateDirectory(jsonCreditsFolderWeb);
                }

                string[] creditsFilesOC = Directory.GetFiles(jsonCreditsFolderPC);
                string[] creditsFilesWeb = Directory.GetFiles(jsonCreditsFolderWeb);

                if (!creditsFilesOC.Contains("credits.pc.json") || !creditsFilesWeb.Contains("credits.web.json"))
                {
                    string jsonPC = Properties.Resources.credits_pc;

                    using (var fileStream = new FileStream(jsonCreditsPC, FileMode.Create, FileAccess.Write))
                    using (var writer = new StreamWriter(fileStream))
                    {
                        writer.Write(jsonPC);
                    }

                    string jsonWeb = Properties.Resources.credits_web;

                    using (var fileStream = new FileStream(jsonCreditsWeb, FileMode.Create, FileAccess.Write))
                    using (var writer = new StreamWriter(fileStream))
                    {
                        writer.Write(jsonWeb);
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
            Properties.Settings.Default.KuruCount = Count;
            Settings.Default.Save();

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
                        player.Stop();
                        player.Source = null;
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


        string[] squishInfoTexts; 
        string[] squishButtonTexts; 
        private void ChangeLanguage(int languageIndex)
        {
            switch (languageIndex)
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
                case 4:
                    {
                        SetLanguageBahasa();
                        break;
                    }
                case 5:
                    {
                        SetLanguageRussian();
                        break;
                    }
            }
        }
        private void SetLanguageEnglish()
        {
            welcometb.Text = "Welcome to Herta Kuru Kururing";
            siteInfoTB.Text = $"The application for Herta, the {"(annoying)"} cutest genius Honkai:  Star Rail character out there.";
            squishInfoTexts = new string[2]
            {
                "The kuru~ has been squished for",
                "Herta has been kuru~ for",
            };
            squishInfoTB.Text = squishInfoTexts[0];
            timesTB.Text = "times";
            squishButtonTexts = new string[2]
            {
                "Squish the kuru~!",
                "Kuru kuru~!",
            };
            squishButton.Content = squishButtonTexts[0];
            pcRepoTB.Text = "GitHub Repo PC:";
            siteRepoTB.Text = "GitHub Repo Site:";
        }
        private void SetLanguageChinise()
        {
            welcometb.Text = "黑塔转圈圈~";
            siteInfoTB.Text = $"给黑塔酱写的小程序，对，就是那个{"(烦人的)"}最可爱的《崩坏：星穹铁道》角色！";
            squishInfoTexts = new string[2]
            {
                "黑塔已经咕噜噜~了",
                "黑塔已经转了",
            };
            squishInfoTB.Text = squishInfoTexts[0];
            timesTB.Text = "圈";
            squishButtonTexts = new string[2]
            {
                "转圈圈~",
                "咕噜噜！",
            };
            squishButton.Content = squishButtonTexts[0];
            pcRepoTB.Text = "GitHub PC版仓库:";
            siteRepoTB.Text = "GitHub原版仓库:";
        }
        private void SetLanguageJapanise()
        {
            welcometb.Text = "ヘルタクルクルへようこそ";
            siteInfoTB.Text = $"このアプリはヘルタのために作られた、 あの崩壊：スターレイルの{"(悩ましい)"}かわいい天才キャラー。";
            squishInfoTexts = new string[2]
            {
                "もクルクル",
                "ヘルタクル回る",
            };
            squishInfoTB.Text = squishInfoTexts[0];
            timesTB.Text = "回";
            squishButtonTexts = new string[2]
            {
                "回る！",
                "クル クル～！",
            };
            squishButton.Content = squishButtonTexts[0];
            pcRepoTB.Text = "GitHub PCリポジトリ:";
            siteRepoTB.Text = "GitHubオリジンリポジトリ:";
        }
        private void SetLanguageKorean()
        {
            welcometb.Text = "헤르타빙글 환영합니다~";
            siteInfoTB.Text = $"이 응용 프로그램 헤르타를 위해 만들어졌습니다, 붕괴: 스타레일 의 {"(귀찮은)"} 귀여운 천재";
            squishInfoTexts = new string[2]
            {
                "쿠루~는",
                "헤르타는 쿠루에드였어",
            };
            squishInfoTB.Text = squishInfoTexts[0];
            timesTB.Text = "번";
            squishButtonTexts = new string[2]
            {
                "한 바퀴 돌다~!",
                "빙글 빙글~!",
            };
            squishButton.Content = squishButtonTexts[0];
            pcRepoTB.Text = "GitHub PC버전 리 포지 토리:";
            siteRepoTB.Text = "GitHub리 포지 토리:";
        }
        private void SetLanguageBahasa()
        {
            welcometb.Text = "Selamat datang di Herta kuru~";
            siteInfoTB.Text = $"Situs application yang dipersembahkan kepada Herta, sang Karakter Jenius {"(ngeselin)"} dari Honkai: Star Rail.";
            squishInfoTexts = new string[2]
            {
                "Kuru nya telah dipencet sebanyak",
                "Herta telah ter-kuru-kan sebanyak",
            };
            squishInfoTB.Text = squishInfoTexts[0];
            timesTB.Text = "kali";
            squishButtonTexts = new string[2]
            {
                "Pencet kuru nya~!",
                "Kuru kuru~!",
            };
            squishButton.Content = squishButtonTexts[0];
            pcRepoTB.Text = "GitHub PC repo:";
            siteRepoTB.Text = "GitHub repo:";
        }
        private void SetLanguageRussian()
        {
            welcometb.Text = "Добро пожаловать в Герта Куру Куруринг";
            siteInfoTB.Text = $"Приложение для Герты, самого {"(надоедливого)"} симпатичного и гениального персонажа Honkai: Star Rail.";
            squishInfoTexts = new string[2]
            {
                "Куру~ вращалась уже",
                "Герта куру~ вращалась уже",
            };
            squishInfoTB.Text = squishInfoTexts[0];
            timesTB.Text = "раз";
            squishButtonTexts = new string[2]
            {
                "Вращать куру~!",
                "Куру куру~!",
            };
            squishButton.Content = squishButtonTexts[0];
            pcRepoTB.Text = "GitHub PC репозиторий:";
            siteRepoTB.Text = "GitHub репозиторий сайта:";
        }
        private void refreshDynamicTexts()
        {
            squishButton.Content = squishButtonTexts[(new Random()).Next(0,2)];
            squishInfoTB.Text = squishInfoTexts[(new Random()).Next(0,2)];
        }


        private void ModalPage_Closed()
        {
            ModlaPage.Content = null;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreditsWebPage creditsWebPage = new CreditsWebPage();
            creditsWebPage.PageClosed += ModalPage_Closed;
            ModlaPage.Content = creditsWebPage;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CreditsPcPage creditsPcPage = new CreditsPcPage();
            creditsPcPage.PageClosed += ModalPage_Closed;
            ModlaPage.Content = creditsPcPage;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/KoksMen/herta_kuru_pc"));
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/duiqt/herta.kuru"));
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SettingsPage settingPage = new SettingsPage(Settings.Default.TextLanguage, Settings.Default.AudioLanguage);
            settingPage.SettingsPageClosed += ModalPage_Closed;
            settingPage.TextLanguageChanged += SettingPage_TextLanguageChanged;
            settingPage.AudioLanguageChanged += SettingPage_AudioLanguageChanged;
            ModlaPage.Content = settingPage;
        }


        private void SettingPage_AudioLanguageChanged(int audioIndex)
        {
            SelectAudioLanguage(audioIndex);
        }
        private void SettingPage_TextLanguageChanged(int languageIndex)
        {
            ChangeLanguage(languageIndex);
        }
    }
}
