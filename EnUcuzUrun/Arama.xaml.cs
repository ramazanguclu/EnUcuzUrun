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
    public partial class Arama : PhoneApplicationPage
    {
        Service1Client al = new Service1Client();
        public Arama()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            al.UrunAramaAsync(txtUrun.Text);
            al.UrunAramaCompleted += new EventHandler<UrunAramaCompletedEventArgs>(al_UrunAramaCompleted);
        }

        void al_UrunAramaCompleted(object sender, UrunAramaCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                lstUrunlerArama.ItemsSource = e.Result;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            al.MarketAramaAsync(txtMarket.Text);
            al.MarketAramaCompleted += new EventHandler<MarketAramaCompletedEventArgs>(al_MarketAramaCompleted);
        }

        void al_MarketAramaCompleted(object sender, MarketAramaCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                lstMarketlerArama.ItemsSource = e.Result;
            }
        }

        private void lstUrunlerArama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sayi;

            Urunler UrunBilgi = lstUrunlerArama.SelectedItem as Urunler;

            if (UrunBilgi != null)
            {
                lstUrunlerArama.SelectedIndex = -1;
                sayi = UrunBilgi.BarkodId.ToString();
                NavigationService.Navigate(new Uri("/UrunSayfasi.xaml?urun=" + sayi, UriKind.RelativeOrAbsolute));

            }
        }

        private void lstMarketlerArama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int marketid;
            View_Marketler MarketBilgi = lstMarketlerArama.SelectedItem as View_Marketler;

            if (MarketBilgi != null)
            {
                lstMarketlerArama.SelectedIndex = -1;
                marketid = MarketBilgi.MarketId;
                NavigationService.Navigate(new Uri("/MarketSayfasi.xaml?marketid=" + marketid, UriKind.RelativeOrAbsolute));

            }
        }
    }
}