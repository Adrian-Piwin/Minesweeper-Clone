using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Playback;
using Windows.Media.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Minesweeper_Term_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        string rule1 = "The objective is to place flags on all tiles that are suspected to be bombs as well as uncover all other tiles.";
        string rule2 = "If a tile with a bomb is clicked, you lose.";
        string rule3 = "When a tile without a bomb is clicked, a number will appear that informs you how many bomb tiles are touching that specific tile. This includes tiles to the right, left, above, below, and diagonal.";


        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Button to start game event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStart(object sender, RoutedEventArgs e)
        {
            MainStack.Visibility = Visibility.Collapsed;
            TextBlock loading = new TextBlock();

            loading.Text = "Loading...";
            loading.FontSize = 50;
            loading.VerticalAlignment = VerticalAlignment.Center;
            loading.HorizontalAlignment = HorizontalAlignment.Center;

            mainGrid.Children.Add(loading);

            this.Frame.Navigate(typeof(MainGame));
        }

        private async void OnCheckingHowToPlay(object sender, RoutedEventArgs e)
        {
            ContentDialog howToPlayDialog = new ContentDialog
            {
                Title = "Rules / Instructions:",
                Content = $"\n{rule1}\n\n{rule2}\n\n{rule3}",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await howToPlayDialog.ShowAsync();
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
