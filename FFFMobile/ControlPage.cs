using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FFFMobile.ControlPages
{
    public class ControlPage : Page
    {

        public ControlPage()
        {
            this.SizeChanged += Page_SizeChanged;
        }

        protected void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonBase b = (ButtonBase)sender;

            NavigationRootPage.RootFrame.Navigate(typeof(ItemPage), b.Content.ToString());
        }

        protected void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width <= 768)
            {
                VisualStateManager.GoToState(this, "Below768Layout", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "DefaultLayout", true);
            }
        }
    }
}