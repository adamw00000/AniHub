using LibVLCSharp.Shared;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AniHub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
        LibVLC _libVLC;
        LibVLCSharp.Shared.MediaPlayer _mediaPlayer;

        public MainWindow()
		{
			InitializeComponent();

			Core.Initialize();

            var label = new Label
            {
                Content = "TEST",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Foreground = new SolidColorBrush(Colors.Red)
            };
            test.Children.Add(label);

            _libVLC = new LibVLC();
            _mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC);

            // we need the VideoView to be fully loaded before setting a MediaPlayer on it.
            VideoView.Loaded += (sender, e) => VideoView.MediaPlayer = _mediaPlayer;
        }

        void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (VideoView.MediaPlayer.IsPlaying)
            {
                VideoView.MediaPlayer.Stop();
            }
        }

        void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (!VideoView.MediaPlayer.IsPlaying)
            {
                //VideoView.MediaPlayer.Play(new Media(_libVLC,
                //    "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4", FromType.FromLocation));
                VideoView.MediaPlayer.Play(new Media(_libVLC,
                    "C:\\Users\\Adam\\Desktop\\AniHub\\AniHubConsole\\bin\\Debug\\netcoreapp3.1\\Downloads\\[HorribleSubs] Usagi Drop - 01 [720p].mkv", FromType.FromPath));
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            VideoView.Dispose();
        }
    }
}
