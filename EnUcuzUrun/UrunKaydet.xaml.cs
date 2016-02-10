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
    public partial class UrunKaydet : PhoneApplicationPage
    {
        Service1Client al = new Service1Client();
        Marketler veri = new Marketler();

        string barkodVerisi;  // scan.cs sayfasından gelen BarkodDatası ile dolduruluyor. Navigated kısmında.
        int MahalleId;
        int MarketId;
        int barkodId = -1;
        string barkodDatasi;
        int KullaniciId;
        string BarkodTur;
        bool BarkodKontrol = false;
        decimal ucret;
        decimal enUcuzFiyat = 0;
        public UrunKaydet()
        {
            InitializeComponent();
            al.IzmitMahallelerAsync();
            al.IzmitMahallelerCompleted += new EventHandler<IzmitMahallelerCompletedEventArgs>(al_IzmitMahallelerCompleted);
            KullaniciId = PivotPageEnUcuz.kullaniciid;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            barkodDatasi = NavigationContext.QueryString["barkod"];
            BarkodTur = NavigationContext.QueryString["barkodtur"];

            txtBarkod.Text = barkodDatasi;
            txtBarkodTur.Text = BarkodTur;

            barkodVerisi = barkodDatasi;
            al.BarkodVarMiAsync(barkodVerisi);
            al.BarkodVarMiCompleted += Al_BarkodVarMiCompleted;
        }

        private void Al_BarkodVarMiCompleted(object sender, BarkodVarMiCompletedEventArgs e)
        {
            if (e.Result.key)
            {
                BarkodKontrol = true;
                txtUrunIsim.Text = e.Result.value.UrunAd;
                barkodId = e.Result.value.BarkodId; // eğer barkodu okutulan ürünün daha önceden veritabanına kaydı varsa o ürünün barkodidsi atandı...
                enUcuzFiyat = e.Result.value.EnUcuzFiyat;
            }
            else
            {
                BarkodKontrol = false;
            }
        }
        void al_IzmitMahallelerCompleted(object sender, IzmitMahallelerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                lpkMahalle.ItemsSource = e.Result;
            }
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
                lpkMarket.SelectedIndex = lpkMarket.Items.Count - 1;
            }
        }

        private void lpkMarket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Marketler Market = lpkMarket.SelectedItem as Marketler;

            if (Market != null)
            {
                MarketId = Market.MarketId;
            }
        }

        private void btnMarketEkle_Click(object sender, RoutedEventArgs e)
        {
            if (txtMarketIsim.Text != string.Empty)
            {
                veri.MarketAdi = txtMarketIsim.Text;
                veri.Adres = txtMarketAdres.Text;
                veri.MahalleId = MahalleId;
                veri.MarketEkleyen = KullaniciId;
                al.MarketKayitAsync(veri);
                al.MarketKayitCompleted += new EventHandler<MarketKayitCompletedEventArgs>(al_MarketKayitCompleted);
            }
        }

        void al_MarketKayitCompleted(object sender, MarketKayitCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                txtMarketIsim.Text = string.Empty;
                txtMarketAdres.Text = string.Empty;
                MessageBox.Show("Marketiniz Eklendi!");

                al.MarketlerListeUrunKaydetAsync(MahalleId);
                al.MarketlerListeUrunKaydetCompleted += new EventHandler<MarketlerListeUrunKaydetCompletedEventArgs>(al_MarketlerListeUrunKaydetCompleted);
            }
        }

        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            if (txtFiyat.Text != string.Empty && txtUrunIsim.Text != string.Empty)
            {
                ucret = Convert.ToDecimal(txtFiyat.Text);

                if (!BarkodKontrol) // eğer ürün daha önceden eklenmemişse
                {
                    // ürünler veritabanına veri kaydı..
                    Urunler urun = new Urunler
                    {
                        Barkod = barkodDatasi,
                        UrunAd = txtUrunIsim.Text,
                        UrunIlkEkleyen = KullaniciId,
                        EnUcuzFiyat = ucret,
                        MarketId = MarketId,
                        UrunResim = null,
                        BarkodTur = BarkodTur
                    };
                    al.UrunlerKayitAsync(urun);
                    al.UrunlerKayitCompleted += new EventHandler<UrunlerKayitCompletedEventArgs>(al_UrunlerKayitCompleted);
                }

                if (BarkodKontrol)
                {
                    al.PaylasimUrunTakipKayitAsync(barkodId, ucret, MarketId, KullaniciId);
                    al.PaylasimUrunTakipKayitCompleted += Al_PaylasimUrunTakipKayitCompleted;
                }

                NavigationService.GoBack();
            }
        }

        private void Al_PaylasimUrunTakipKayitCompleted(object sender, PaylasimUrunTakipKayitCompletedEventArgs e)
        {
            foreach (string item in e.Result)
            {
                MessageBox.Show(item);
            }
            Temizle();
        }

        void al_UrunlerKayitCompleted(object sender, UrunlerKayitCompletedEventArgs e)
        {
            if (e.Result.key)
                MessageBox.Show("Ürün Başarıyla Kaydedildi!");  // bakılacak  // ürnler tablosundan son kaydedilmiş ürünün barkodidsi alınacak

            if (e.Error != null)
                MessageBox.Show("Ürün Kaydederken Bir Hata Oluştu!");

            if (!e.Result.key)
                MessageBox.Show("Ürün Kaydederken Bir Hata Oluştu!");
            Temizle();
        }

        private void btnAarama_Click(object sender, RoutedEventArgs e)
        {
            if (BarkodKontrol)
            {
                NavigationService.Navigate(new Uri("/UrunSayfasi.xaml?urun=" + barkodId + "&urunad=" + txtUrunIsim.Text, UriKind.RelativeOrAbsolute));
            }
            else
            {
                MessageBox.Show("Ürünle İlgili Bir Paylaşım Bulunmamaktadır!");
            }
        }

        public void Temizle()
        {
            List<TextBox> liste = new List<TextBox> { txtBarkod, txtBarkodTur, txtFiyat, txtMarketAdres, txtMarketIsim, txtUrunIsim };
            foreach (TextBox item in liste)
            {
                item.Text = "";
            }
        }
    }
}