using ChineesWiezen.Data;
using ChineesWiezen.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ChineesWiezen
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GokPage : Page
    {
        private ViewModel vm;
        private Brush prevColor;

        public GokPage()
        {
            this.InitializeComponent();
            vm = new ViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            vm.state = e.Parameter as GameState;
            vm.speler = vm.state.GetNextToGuess;
            txtGok.Text = vm.gok.ToString();

            txtNietGokken.Text = vm.state.CurrentRound.NextCannotGuess != -1 ?
                string.Format("Je mag geen {0} gokken!", vm.state.CurrentRound.NextCannotGuess):
                string.Format("Je mag gokken wat je wil!");
            btnVorige.Visibility = vm.state.CurrentRound.Guesses.Any() ? Visibility.Visible : Visibility.Collapsed;

            Details.VulCollection(vm.vorigegokken, vm.state.CurrentRound.Guesses);
            vm.score = Score.getScore(vm.state).ToList();

            prevColor = txtNietGokken.Foreground;
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            vm.gok += 1;
            txtGok.Text = vm.gok.ToString();
            checkGok();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            vm.gok -= 1;

            if (vm.gok < 0)
                vm.gok = 0;
            txtGok.Text = vm.gok.ToString();
            checkGok();
        }

        private void checkGok()
        {
            if (vm.gok == vm.state.CurrentRound.NextCannotGuess)
            {
                btnVolgende.Visibility = Visibility.Collapsed;
                // Make it the same as the hearts color
                txtNietGokken.Foreground = Suit.Harten.ToColor();
            } 
            else
            {
                btnVolgende.Visibility = Visibility.Visible;
                txtNietGokken.Foreground = prevColor;
            }
        }

        private void btnVolgende_Click(object sender, RoutedEventArgs e)
        {
            vm.state.AddGuess(vm.speler, vm.gok);

            if (vm.state.EveryoneHasGuessed)
            {
                Frame.Navigate(typeof(SpeelPage), vm.state);
            }
            else
            {
                Frame.Navigate(typeof(GokPage), vm.state);
            }
        }

        private void btnVorige_Click(object sender, RoutedEventArgs e)
        {
            vm.state.RemovePreviousGuess();
            Frame.Navigate(typeof(GokPage), vm.state);
        }

        public class ViewModel
        {
            public GameState state;
            public int gok;
            public Player speler;
            public ObservableCollection<Details> vorigegokken = new ObservableCollection<Details>();
            public List<Score> score = new List<Score>();

            public Brush suitColor
            {
                get
                {
                    return state.CurrentSuit.ToColor();
                }
            }
        }
    }
}
