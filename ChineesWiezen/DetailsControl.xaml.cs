using ChineesWiezen.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ChineesWiezen
{
    public sealed partial class DetailsControl : UserControl
    {
        public ObservableCollection<Details> Values
        {
            get { return (ObservableCollection<Details>)GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        public string Title
        {
            get
            {
                var str = (string)GetValue(TitleProperty);

                return Values.Count > 0 ? str : "";
            }
            set { SetValue(TitleProperty, value); }
        }

        public DetailsControl()
        {
            this.InitializeComponent();
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DetailsControl), new PropertyMetadata(0));

        // Using a DependencyProperty as the backing store for Values.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register("Values", typeof(ObservableCollection<Details>), typeof(DetailsControl), new PropertyMetadata(0));
    }
}
