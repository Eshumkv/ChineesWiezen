using ChineesWiezen.Data;
using ChineesWiezen.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ChineesWiezen
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TrickPage : Page
    {
        private int _slag;

        private GameState state;
        private int slag
        {
            get
            {
                return _slag;
            }
            set
            {
                _slag = value;
                txtSlag.Text = _slag.ToString();
            }
        }
        private int gok;
        private Player speler;
        private ObservableCollection<Details> vorigeslagen = new ObservableCollection<Details>();

        private Brush suitColor
        {
            get
            {
                return state.CurrentSuit.ToColor();
            }
        }

        public TrickPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            state = e.Parameter as GameState;
            speler = state.GetNextToTrick;
            txtSlag.Text = slag.ToString();
            gok = state.CurrentRound.Guesses[speler];
            txtInfo.Text = string.Format("Je had {0} gegokt.", gok);
            Details.VulCollection(vorigeslagen, state.CurrentRound.Tricks);
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            slag += 1;
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            slag -= 1;

            if (slag < 0)
                slag = 0;
        }

        private void btnVorige_Click(object sender, RoutedEventArgs e)
        {
            if (state.CurrentRound.Tricks.Any())
            {
                state.RemovePreviousTrick();
                Frame.Navigate(typeof(TrickPage), state);
            } 
            else
            {
                Frame.Navigate(typeof(SpeelPage), state);
            }
        }

        private async void btnVolgende_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                state.AddTrick(speler, slag);

                if (state.EveryoneHasTricked)
                {
                    if (state.NumberOfTricksIsCorrect)
                    {
                        var dialog = new MessageDialog("Weet u zeker dat alles juist is ingevuld?");
                        dialog.Commands.Add(new UICommand("Ja") { Id = 0 });
                        dialog.Commands.Add(new UICommand("Nee") { Id = 1 });
                        dialog.DefaultCommandIndex = 0;
                        dialog.CancelCommandIndex = 1;

                        var result = await dialog.ShowAsync();

                        if (result.Id.ToString() == "0")
                        {
                            state.NextRound();

                            if (state.IsGameFinished)
                            {
                                Frame.Navigate(typeof(FinishPage), state);
                            }
                            else
                            {
                                Frame.Navigate(typeof(DeelPage), state);
                            }
                        }
                        else
                        {
                            state.RemovePreviousTrick();
                        }
                    }
                    else
                    {
                        state.RemovePreviousTrick();
                        await showErrorDialog();
                    }

                }
                else
                {
                    Frame.Navigate(typeof(TrickPage), state);
                }
            } 
            catch (ArgumentOutOfRangeException)
            {
                await showErrorDialog();
            }
        }

        private IAsyncOperation<IUICommand> showErrorDialog()
        {
            var dialog = new MessageDialog("Er klopt iets niet met het aantal slagen!");
            dialog.Commands.Add(new UICommand("Ok") { Id = 0 });
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;

            return dialog.ShowAsync();
        }

        private void txtInfo_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            slag = gok;
        }
    }
}
