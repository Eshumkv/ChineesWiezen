using ChineesWiezen.Models;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ChineesWiezen
{
    public sealed partial class ScoreControl : UserControl
    {
        public ScoreControl()
        {
            this.InitializeComponent();
        }

        public List<Score> Values
        {
            get { return (List<Score>)GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Values.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register("Values", typeof(List<Score>), typeof(ScoreControl), new PropertyMetadata(0));
    }
}
