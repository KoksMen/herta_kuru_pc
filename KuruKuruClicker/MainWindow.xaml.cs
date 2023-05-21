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

            Count = 0;
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
            double squishButtonBottom = squishButton.TransformToAncestor(HertaShowerGrid).Transform(new Point(0, squishButton.ActualHeight)).Y;

            Random random = new Random();
            int randomIndex = random.Next(1, 3);


            Image hertaImageElement = new Image
            {
                Width = 200,
                Height = 200,
                RenderTransform = new TranslateTransform(500, 0),
                RenderTransformOrigin = new Point(0.5, 0.5)
            };

            var hertaImage = new BitmapImage(new Uri($"img/hertaa{randomIndex}.gif", UriKind.Relative));

            ImageBehavior.SetAnimatedSource(hertaImageElement, hertaImage);

            HertaShowerGrid.Children.Add(hertaImageElement);
            Grid.SetZIndex(hertaImageElement, -1);

            DoubleAnimation hertaMoveAnimation = new DoubleAnimation
            {
                To =  - 500,
                Duration = TimeSpan.FromSeconds(1),
            };

            TranslateTransform hertaTransformAnimation = hertaImageElement.RenderTransform as TranslateTransform;
            hertaTransformAnimation.BeginAnimation(TranslateTransform.XProperty, hertaMoveAnimation);

            hertaMoveAnimation.Completed += (s, e) =>
            {
                HertaShowerGrid.Children.Remove(hertaImageElement);
                hertaImageElement = null;
                hertaImage = null;
            };
        }

        string[] hertaAudio = new string[3]
        {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "audio", "kuruto.wav"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "audio", "kuru1.wav"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "audio", "kuru2.wav")
        };

        private void PlayKuru()
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
                player.Source = new Uri(hertaAudio[(new Random()).Next(0,3)]);

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
    }
}
