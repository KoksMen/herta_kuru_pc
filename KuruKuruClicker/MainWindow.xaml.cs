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

namespace KuruKuruClicker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
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

            DataContext = this;

            LoadFiles();

            Count = 0;


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
    }
}
