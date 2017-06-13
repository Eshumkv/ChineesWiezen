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
    public sealed partial class SpeelPage : Page
    {
        private ViewModel vm;

        public SpeelPage()
        {
            this.InitializeComponent();
            vm = new ViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            vm.state = e.Parameter as GameState;
            vm.wiebegint = string.Format("{0} moet beginnen!", vm.state.PlayerThatStarts.Name);
            Details.VulCollection(vm.vorigegokken, vm.state.CurrentRound.Guesses);
            vm.score = Score.getScore(vm.state).ToList();
        }

        private void btnVorige_Click(object sender, RoutedEventArgs e)
        {
            vm.state.RemovePreviousGuess();
            Frame.Navigate(typeof(GokPage), vm.state);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TrickPage), vm.state);
        }

        public class ViewModel
        {
            public string wiebegint;
            public ObservableCollection<Details> vorigegokken = new ObservableCollection<Details>();
            public List<Score> score = new List<Score>();
            public GameState state;
            public Brush suitColor
            {
                get
                {
                    if (state.CurrentSuit.Value <= 2)
                    {
                        return new SolidColorBrush(Colors.Red);
                    }
                    else if (state.CurrentSuit.Value == 5)
                    {
                        return new SolidColorBrush(Colors.Blue);
                    }
                    else
                    {
                        return new SolidColorBrush(Colors.Black);
                    }
                }
            }
        }
    }
}
