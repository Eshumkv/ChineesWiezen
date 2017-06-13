using ChineesWiezen.Data;
using ChineesWiezen.Models;
using System.Collections.Generic;
using System.Linq;
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
    public sealed partial class DeelPage : Page
    {
        private ViewModel vm;

        public DeelPage()
        {
            this.InitializeComponent();
            vm = new ViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            vm.state = e.Parameter as GameState;
            vm.deler = vm.state.Dealer.Name;
            vm.score = Score.getScore(vm.state).ToList();
        }

        private void btnVolgende_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GokPage), vm.state);
        }

        public class ViewModel
        {
            public GameState state;
            public string deler;
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
