using ChineesWiezen.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ChineesWiezen
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly ViewModel vm;

        public MainPage()
        {
            this.InitializeComponent();

            vm = new ViewModel()
            {
                state = new GameState()
            };
        }

        private void btnToevoegen_Click(object sender, RoutedEventArgs e)
        {
            toevoegen();
        }

        private void btnOmhoog_Click(object sender, RoutedEventArgs e)
        {
            if (ListNames.SelectedIndex < 0)
                return;

            var position = (ListNames.SelectedItem as Player).Position;
            vm.state.MovePlayerUp(position);
            fillPlayers();
            ListNames.SelectedIndex = Math.Max(position - 1, 0);
        }

        private void btnOmlaag_Click(object sender, RoutedEventArgs e)
        {
            if (ListNames.SelectedIndex < 0)
                return;

            var position = (ListNames.SelectedItem as Player).Position;
            vm.state.MovePlayerDown(position);
            fillPlayers();
            ListNames.SelectedIndex = Math.Min(position + 1, vm.state.Players.Count - 1);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            vm.state.NewGame();
            Frame.Navigate(typeof(DeelPage), vm.state);
        }

        private void fillPlayers()
        {
            vm.players.Clear();

            foreach (var player in vm.state.Players.OrderBy(x => x.Position))
            {
                vm.players.Add(player);
            }

            btnStart.Visibility = vm.StartButtonVisibility;
            btnVerwijder.Visibility = vm.VerwijderButtonVisibility;
        }

        private void btnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            if (ListNames.SelectedIndex < 0)
                return;

            vm.state.Players.Remove(ListNames.SelectedItem as Player);
            fillPlayers();
        }

        private void txtNaam_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                toevoegen();
            }
        }

        private void toevoegen()
        {
            if (string.IsNullOrWhiteSpace(txtNaam.Text))
            {
                return;
            }

            vm.state.Players.Add(new Player(txtNaam.Text, vm.state.Players.Count));
            fillPlayers();
            txtNaam.Text = string.Empty;
            txtNaam.Focus(FocusState.Programmatic);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            fillPlayers();
        }

        public sealed class ViewModel
        {
            public GameState state;
            public readonly ObservableCollection<Player> players;

            public Visibility StartButtonVisibility
            {
                get
                {
                    return players.Count >= 2 ? Visibility.Visible : Visibility.Collapsed;
                }
            }

            public Visibility VerwijderButtonVisibility
            {
                get
                {
                    return players.Any() ? Visibility.Visible : Visibility.Collapsed;
                }
            }

            public ViewModel()
            {
                players = new ObservableCollection<Player>();
            }
        }
    }
}
