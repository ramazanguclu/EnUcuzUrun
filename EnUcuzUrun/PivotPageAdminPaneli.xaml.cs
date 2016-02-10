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
    public partial class PivotPageAdminPaneli : PhoneApplicationPage
    {
        Service1Client al = new Service1Client();
        int MahalleId;
        int MarketId;
        int barkod;
        int id;
        public PivotPageAdminPaneli()
        {
            InitializeComponent();

            al.ListelemeAsync();
            al.ListelemeCompleted += new EventHandler<ListelemeCompletedEventArgs>(al_ListelemeCompleted);

            al.IzmitMahallelerAsync();
            al.IzmitMahallelerCompleted += new EventHandler<IzmitMahallelerCompletedEventArgs>(al_IzmitMahallelerCompleted);

            al.UrunlerAsync();
            al.UrunlerCompleted += new EventHandler<UrunlerCompletedEventArgs>(al_UrunlerCompleted);

        }

        void al_UrunlerCompleted(object sender, UrunlerCompletedEventArgs e)
        {
            lpkUrunler.ItemsSource = e.Result;
        }

        private void al_IzmitMahallelerCompleted(object sender, IzmitMahallelerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                lpkMahalle.ItemsSource = e.Result;
            }
        }

        private void lpkKullanici_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            View_KullaniciBilgi kullanici = lpkMahalle.SelectedItem as View_KullaniciBilgi;

            if (kullanici != null)
            {
                txbEmail.Text = kullanici.Email;
                txbMahalle.Text = kullanici.MahalleAdi;
                txtOnay.Text = kullanici.Onay.ToString();
                id = kullanici.KullaniciId;
            }

            else
            {
               // lpkKullanici.SelectedIndex = -1;
            }
        }

        private void btnEnUcuzGor_Click(object sender, RoutedEventArgs e)
        {
            al.KullaniciOnayGüncelleAsync(true, id);   // düzeltilecek

            al.KullaniciOnayGüncelleCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(al_KullaniciOnayGüncelleCompleted);

        }

        void al_KullaniciOnayGüncelleCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Kullanıcınız Güncellendi.");
            txtOnay.Text = string.Empty;
            al.ListelemeAsync();
            al.ListelemeCompleted += new EventHandler<ListelemeCompletedEventArgs>(al_ListelemeCompleted);
        }

        void al_ListelemeCompleted(object sender, ListelemeCompletedEventArgs e)
        {
            lpkKullanici.ItemsSource = e.Result;
        }

        private void lpkMahalle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IzmitMahalle Mahalle = lpkMahalle.SelectedItem as IzmitMahalle;

            if (Mahalle != null)
            {
                MahalleId = Mahalle.MahalleId;
                
                al.MarketlerListeUrunKaydetAsync(MahalleId);
                al.MarketlerListeUrunKaydetCompleted += new EventHandler<MarketlerListeUrunKaydetCompletedEventArgs>(al_MarketlerListeUrunKaydetCompleted);

                
            }
        }

        void al_MarketlerListeUrunKaydetCompleted(object sender, MarketlerListeUrunKaydetCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                lpkMarket.ItemsSource = e.Result;
            }
        }

        private void lpkMarket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Marketler Market = lpkMarket.SelectedItem as Marketler;

            if (Market != null)
            {
                txtMarketAd.Text = Market.MarketAdi;
                txtMarketAdres.Text = Market.Adres;
                MarketId = Market.MarketId;
            }
        }

        private void btnMarketGuncelle_Click(object sender, RoutedEventArgs e)
        {
            al.MArketAdGüncelleAsync(txtMarketAd.Text, MarketId, txtMarketAdres.Text);
            al.MArketAdGüncelleCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(al_MArketAdGüncelleCompleted);
        }

        void al_MArketAdGüncelleCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Marketiniz Güncellendi.");

            txtMarketAd.Text = string.Empty;
            txtMarketAdres.Text = string.Empty;

            al.IzmitMahallelerCompleted += new EventHandler<IzmitMahallelerCompletedEventArgs>(al_IzmitMahallelerCompleted);
        }

        private void btnUrunGuncelle_Click(object sender, RoutedEventArgs e)
        {
            al.UrunAdGuncelleAsync(barkod, txtUrun.Text);
            al.UrunAdGuncelleCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(al_UrunAdGuncelleCompleted);
        }

        void al_UrunAdGuncelleCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Ürün Güncellendi.");
            txtUrun.Text = string.Empty;

            al.UrunlerAsync();
            al.UrunlerCompleted += new EventHandler<UrunlerCompletedEventArgs>(al_UrunlerCompleted);
        }

        private void lpkUrunler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Urunler urun = lpkUrunler.SelectedItem as Urunler;

            if (urun != null)
            {
                barkod = urun.BarkodId;
                txtUrun.Text = urun.UrunAd;
                txbBarkod.Text = barkod.ToString();
            }
        }        
    }
}