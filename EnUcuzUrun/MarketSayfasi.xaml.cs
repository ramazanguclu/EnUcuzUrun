using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EnUcuzUrun.ServiceReferenceEnUcuzUrun;

namespace EnUcuzUrun
{
    public partial class MarketSayfasi : PhoneApplicationPage
    {
        int MarketId;
        string MarketAd;

        Service1Client al = new Service1Client();
        public MarketSayfasi()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MarketId = Convert.ToInt32(NavigationContext.QueryString["marketid"]);
            MarketAd = NavigationContext.QueryString["marketad"];
            tbMarketAd.Text = MarketAd;
            base.OnNavigatedTo(e);

            al.MarketUrunleriAsync(MarketId);
            al.MarketUrunleriCompleted += new EventHandler<MarketUrunleriCompletedEventArgs>(al_MarketUrunleriCompleted);

        }

        void al_MarketUrunleriCompleted(object sender, MarketUrunleriCompletedEventArgs e)
        {
            lstMarketUrunleri.ItemsSource = e.Result;
        }

        private void lstMarketUrunleri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string BarkodId;
            string urunad;

            View_MarketUrunleri MarketUrun = lstMarketUrunleri.SelectedItem as View_MarketUrunleri;

            if (MarketUrun != null)
            {
                BarkodId = MarketUrun.BarkodId.ToString();
                urunad = MarketUrun.UrunAd;
                NavigationService.Navigate(new Uri("/UrunSayfasi.xaml?urun=" + BarkodId + "&urunad=" + urunad, UriKind.RelativeOrAbsolute));
            }
        }
    }
}