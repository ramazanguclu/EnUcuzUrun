using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using EnUcuzUrun.ServiceReferenceEnUcuzUrun;
using System.IO.IsolatedStorage;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace EnUcuzUrun
{
    public partial class PivotPageEnUcuz : PhoneApplicationPage
    {
        public static int kullaniciid;
        int MahalleId;
        int MarketId;
        decimal fiyat = 0;
        decimal enucuzfiyat = 0;
        string enucuzucret;
        string ucret;
        public static string kullaniciad;
        public static string mahalle;
        public static int mahalleid;
        ObservableCollection<byte[]> dizim = new ObservableCollection<byte[]>();
        int dizimSay = 0;
        List<int> barkodlar = new List<int>();
        List<View_MarketUrunleri> a = new List<View_MarketUrunleri>();
        List<View_UrunlerMarketMahalle> enucuz = new List<View_UrunlerMarketMahalle>();
        ObservableCollection<View_AnasayfaUrunTakipListe> listem4 = null;
        Service1Client al = new Service1Client();
        public PivotPageEnUcuz()
        {
            InitializeComponent();
            al.IzmitMahallelerAsync();
            al.IzmitMahallelerCompleted += new EventHandler<IzmitMahallelerCompletedEventArgs>(al_IzmitMahallelerCompleted);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            kullaniciid = Convert.ToInt32(NavigationContext.QueryString["kullaniciid"]);

            base.OnNavigatedTo(e);
            NavigationService.RemoveBackEntry();

            al.KullaniciAdDondurAsync(kullaniciid);
            al.KullaniciAdDondurCompleted += new EventHandler<KullaniciAdDondurCompletedEventArgs>(al_KullaniciAdDondurCompleted);

            al.anasayfalisteAsync(kullaniciid);
            al.anasayfalisteCompleted += new EventHandler<anasayfalisteCompletedEventArgs>(al_anasayfalisteCompleted);

            al.MarketlerAsync();
            al.MarketlerCompleted += new EventHandler<MarketlerCompletedEventArgs>(al_MarketlerCompleted);

            al.ProfilPaylasimlarAsync(kullaniciid);
            al.ProfilPaylasimlarCompleted += new EventHandler<ProfilPaylasimlarCompletedEventArgs>(al_ProfilPaylasimlarCompleted);

            al.ProfilMarketlerimAsync(kullaniciid);
            al.ProfilMarketlerimCompleted += new EventHandler<ProfilMarketlerimCompletedEventArgs>(al_ProfilMarketlerimCompleted);

            if (lstUrunlerim.SelectedIndex == -1)
            {
                txtYeniFiyat.Visibility = Visibility.Collapsed;
                btnGuncelleFiyat.Visibility = Visibility.Collapsed;
            }
        }

        void al_KullaniciAdDondurCompleted(object sender, KullaniciAdDondurCompletedEventArgs e)
        {
            View_KullaniciBilgi kulbilgi = e.Result[0] as View_KullaniciBilgi;

            kullaniciad = kulbilgi.Ad;
            mahalle = kulbilgi.MahalleAdi;
            mahalleid = Convert.ToInt32(kulbilgi.MahalleId);

            txtAd.Text = kullaniciad;
            txtMahalle.Text = mahalle;
        }
        private void al_IzmitMahallelerCompleted(object sender, IzmitMahallelerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                lpkMahalle.ItemsSource = e.Result;
            }
        }

        void al_ProfilMarketlerimCompleted(object sender, ProfilMarketlerimCompletedEventArgs e)
        {
            lstMarketlerimProfil.ItemsSource = e.Result;
        }

        void al_ProfilPaylasimlarCompleted(object sender, ProfilPaylasimlarCompletedEventArgs e)
        {
            lstUrunlerim.ItemsSource = e.Result;
        }

        void al_MarketlerCompleted(object sender, MarketlerCompletedEventArgs e)
        {
            lstMarketlerim.ItemsSource = e.Result;
        }
        void al_anasayfalisteCompleted(object sender, anasayfalisteCompletedEventArgs e)
        {
            listem4 = e.Result;
            ObservableCollection<string> listem = new ObservableCollection<string>();
            if (e.Result != null)
            {
                foreach (View_AnasayfaUrunTakipListe item in e.Result)
                {
                    listem.Add(item.UrunResim);
                }
                al.TakipEdilenUrunImagesAsync("urunresimleri", listem);
                al.TakipEdilenUrunImagesCompleted += Al_TakipEdilenUrunImagesCompleted;
            }
        }

        private void Al_TakipEdilenUrunImagesCompleted(object sender, TakipEdilenUrunImagesCompletedEventArgs e)
        {

            ObservableCollection<ClassAnasayfaTakipUrunler> liste2 = new ObservableCollection<ClassAnasayfaTakipUrunler>();


            foreach (View_AnasayfaUrunTakipListe item in listem4)
            {
                ClassAnasayfaTakipUrunler sinifim = new ClassAnasayfaTakipUrunler();
                sinifim.Barkod = item.Barkod;
                sinifim.BarkodId = item.BarkodId;
                sinifim.EnUcuzFiyat = string.Format("{0:C}", item.EnUcuzFiyat);
                sinifim.KullaniciId = item.KullaniciId;
                sinifim.MarketAdi = item.MarketAdi + " Market";
                sinifim.MarketId = item.MarketId;
                sinifim.Takip = item.Takip;
                sinifim.TakipId = item.TakipId;
                sinifim.UrunAd = item.UrunAd.ToUpper().Trim();
                sinifim.UrunIlkEkleyen = item.UrunIlkEkleyen;
                using (MemoryStream ms = new MemoryStream(e.Result[dizimSay]))
                {
                    WriteableBitmap wb = new WriteableBitmap(90, 90);
                    wb.SetSource(ms);
                    sinifim.UrunResim = wb;
                }
                liste2.Add(sinifim);
                dizimSay++;
            }
            lstKullanici.ItemsSource = liste2;
            dizimSay = 0;
        }

        private void Kamera_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Scan.xaml", UriKind.Relative));
        }

        private void lstKullanici_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sayi;
            string urunad;

            ClassAnasayfaTakipUrunler UrunBilgi = lstKullanici.SelectedItem as ClassAnasayfaTakipUrunler;

            if (UrunBilgi != null)
            {
                sayi = UrunBilgi.BarkodId;
                urunad = UrunBilgi.UrunAd;
                NavigationService.Navigate(new Uri("/UrunSayfasi.xaml?urun=" + sayi + "&urunad=" + urunad, UriKind.RelativeOrAbsolute));
            }
        }

        private void lstMarketlerim_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int marketid;
            string marketad;
            View_Marketler MarketBilgi = lstMarketlerim.SelectedItem as View_Marketler;

            if (MarketBilgi != null)
            {
                marketid = MarketBilgi.MarketId;
                marketad = MarketBilgi.MarketAdi;
                NavigationService.Navigate(new Uri("/MarketSayfasi.xaml?marketid=" + marketid + "&marketad=" + marketad, UriKind.RelativeOrAbsolute));
            }
        }

        private void lstMarketlerimProfil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtYeniFiyat.Visibility = Visibility.Collapsed;
            btnGuncelleFiyat.Visibility = Visibility.Collapsed;

            int MarketId;
            string marketad;

            View_Marketler Market = (View_Marketler)lstMarketlerimProfil.SelectedItem;

            if (Market != null)
            {
                MarketId = Market.MarketId;
                marketad = Market.MarketAdi;
                NavigationService.Navigate(new Uri("/MarketSayfasi.xaml?marketid=" + MarketId + "&marketad=" + marketad, UriKind.RelativeOrAbsolute));
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
                try
                {

                }
                catch (Exception)
                {
                }

            }
            else
            {
                lstSepetimUrunler.Items.Clear();
                lstSepetimUrunlerSecilen.Items.Clear();
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
                MarketId = Market.MarketId;

                al.MarketUrunleriAsync(MarketId);
                al.MarketUrunleriCompleted += new EventHandler<MarketUrunleriCompletedEventArgs>(al_MarketUrunleriCompleted);
            }

        }

        void al_MarketUrunleriCompleted(object sender, MarketUrunleriCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                lstSepetimUrunler.ItemsSource = e.Result;

            }
            else
            {
                MessageBox.Show("Bu Markete Ait Kayıtlı Ürün Yok!");
            }


        }

        private void lstSepetimUrunler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                btnSepeteEkle.Click += new EventHandler(btnSepeteEkle_Click);
            }
            catch (Exception)
            {

            }
        }

        void btnSepeteEkle_Click(object sender, EventArgs e)
        {
            int a;
            View_MarketUrunleri urun = lstSepetimUrunler.SelectedItem as View_MarketUrunleri;
            if (urun != null)
            {
                a = urun.BarkodId;


                for (int i = 0; i <= lstSepetimUrunlerSecilen.Items.Count - 1; i++)
                {
                    View_MarketUrunleri sepet = lstSepetimUrunlerSecilen.Items[i] as View_MarketUrunleri;

                    if (a == sepet.BarkodId)
                    {
                        lstSepetimUrunler.SelectedIndex = -1; break;
                    }
                }

                if (lstSepetimUrunler.SelectedIndex != -1)
                {
                    lstSepetimUrunlerSecilen.Items.Add(lstSepetimUrunler.SelectedValue);
                    lstSepetimUrunler.SelectedIndex = -1;
                }
            }
        }

        private void btnSepettenÇıkar_Click(object sender, EventArgs e)
        {
            lstSepetimUrunlerSecilen.Items.Remove(lstSepetimUrunlerSecilen.SelectedValue);
            lstSepetimUrunlerSecilen.SelectedIndex = -1;
        }

        private void lstSepetimUrunlerSecilen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                btnSepettenÇıkar.Click += new EventHandler(btnSepettenÇıkar_Click);
            }
            catch (Exception)
            {

            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 2:
                    this.ApplicationBar = this.Resources["AppBar2"] as ApplicationBar;
                    break;

                case 3:
                    this.ApplicationBar = this.Resources["AppBarProfilim"] as ApplicationBar;
                    break;

                default:
                    this.ApplicationBar = this.Resources["AppBar1"] as ApplicationBar;
                    break;
            }
        }

        private void btnEnUcuzGor_Click(object sender, RoutedEventArgs e)
        {
            lstEnUcuz.ItemsSource = null;
            enucuz.Clear();
            barkodlar.Clear();

            for (int i = 0; i < lstSepetimUrunlerSecilen.Items.Count; i++)
            {
                View_MarketUrunleri sepet = lstSepetimUrunlerSecilen.Items[i] as View_MarketUrunleri;
                barkodlar.Add(sepet.BarkodId);
            }

            al.EnUcuzListeDondurAsync();
            al.EnUcuzListeDondurCompleted += new EventHandler<EnUcuzListeDondurCompletedEventArgs>(al_EnUcuzListeDondurCompleted);
        }

        void al_EnUcuzListeDondurCompleted(object sender, EnUcuzListeDondurCompletedEventArgs e)
        {

            for (int i = 0; i <= e.Result.Count - 1; i++)
            {
                if (enucuz.Count == barkodlar.Count)
                {
                    break;
                }

                View_UrunlerMarketMahalle ucuzlar = e.Result[i] as View_UrunlerMarketMahalle;

                for (int j = 0; j <= barkodlar.Count - 1; j++)
                {
                    if (ucuzlar.BarkodId == barkodlar[j])
                    {
                        enucuz.Add(ucuzlar); break;
                    }
                }
            }

            lstEnUcuz.ItemsSource = enucuz;

            fiyat = 0;
            for (int i = 0; i < lstSepetimUrunlerSecilen.Items.Count; i++)
            {
                View_MarketUrunleri fiyatlar = (View_MarketUrunleri)lstSepetimUrunlerSecilen.Items[i];

                fiyat += fiyatlar.Fiyat;
            }
            ucret = fiyat.ToString("N");

            enucuzfiyat = 0;
            for (int i = 0; i <= lstSepetimUrunlerSecilen.Items.Count - 1; i++)
            {
                View_UrunlerMarketMahalle fiyatlar = lstEnUcuz.Items[i] as View_UrunlerMarketMahalle;

                enucuzfiyat = enucuzfiyat + fiyatlar.EnUcuzFiyat;
            }
            decimal kazanc = fiyat - enucuzfiyat;
            string kazancım = kazanc.ToString("N");
            enucuzucret = enucuzfiyat.ToString("N");

            txbFiyatToplam.Visibility = Visibility.Visible;
            txbFiyatToplam.Text = "Sepet " + ucret + " TL - EnUcuz " + enucuzucret + " TL = " + kazancım + " TL";
        }

        private void btnUcuzCikis_Click(object sender, RoutedEventArgs e)
        {
            lstEnUcuz.ItemsSource = null;
            enucuz.Clear();
            barkodlar.Clear();
            txbFiyatToplam.Visibility = Visibility.Collapsed;
        }

        private void lstEnUcuz_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Arama.xaml", UriKind.Relative));
        }

        private void btnPaylasimSil_Click(object sender, EventArgs e)
        {
            int paylasimid;
            if (lstUrunlerim.SelectedIndex > -1)
            {
                View_UrunMalumat UrunBilgi = lstUrunlerim.SelectedItem as View_UrunMalumat;

                if (UrunBilgi != null)
                {
                    paylasimid = UrunBilgi.PaylasimId;

                    al.PaylasimSilAsync(paylasimid, UrunBilgi.BarkodId, UrunBilgi.Fiyat);
                    al.PaylasimSilCompleted += new EventHandler<PaylasimSilCompletedEventArgs>(al_PaylasimSilCompleted);
                }
            }
        }

        void al_PaylasimSilCompleted(object sender, PaylasimSilCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                MessageBox.Show("Paylaşımınız Silindi !");

                lstUrunlerim.SelectedIndex = -1;
                al.ProfilPaylasimlarAsync(kullaniciid);
                al.ProfilPaylasimlarCompleted += new EventHandler<ProfilPaylasimlarCompletedEventArgs>(al_ProfilPaylasimlarCompleted);
            }

            if (e.Result == false)
            {
                MessageBox.Show("Paylaşımınız En Ucuz Üründür Silemezsiniz !");
            }
        }

        private void btnPaylasimGuncel_Click(object sender, EventArgs e)
        {
            if (lstUrunlerim.SelectedIndex > -1)
            {
                txtYeniFiyat.Visibility = Visibility.Visible;
                btnGuncelleFiyat.Visibility = Visibility.Visible;
            }
        }

        private void btnPaylasimaGit_Click(object sender, EventArgs e)
        {
            int sayi;
            string urunad;

            txtYeniFiyat.Visibility = Visibility.Collapsed;
            btnGuncelleFiyat.Visibility = Visibility.Collapsed;



            View_UrunMalumat UrunBilgi = lstUrunlerim.SelectedItem as View_UrunMalumat;

            if (UrunBilgi != null)
            {
                sayi = UrunBilgi.BarkodId;
                urunad = UrunBilgi.UrunAd;
                NavigationService.Navigate(new Uri("/UrunSayfasi.xaml?urun=" + sayi + "&urunad=" + urunad, UriKind.RelativeOrAbsolute));
            }
        }

        private void btnGuncelleFiyat_Click(object sender, RoutedEventArgs e)
        {
            if (txtYeniFiyat.Text != "")
            {


                View_UrunMalumat UrunBilgi = lstUrunlerim.SelectedItem as View_UrunMalumat;

                if (UrunBilgi != null)
                {
                    int sayi;
                    int marketid = Convert.ToInt32(UrunBilgi.MarketId);

                    sayi = UrunBilgi.PaylasimId;

                    al.PaylasimGuncelleAsync(sayi, Convert.ToDecimal(txtYeniFiyat.Text), DateTime.Now, UrunBilgi.BarkodId, UrunBilgi.Fiyat, marketid);
                    al.PaylasimGuncelleCompleted += new EventHandler<PaylasimGuncelleCompletedEventArgs>(al_PaylasimGuncelleCompleted);
                }
            }

            else
            {
                MessageBox.Show("Fiyat Giriniz !");

            }

        }

        void al_PaylasimGuncelleCompleted(object sender, PaylasimGuncelleCompletedEventArgs e)
        {
            if (e.Result == 1)
            {
                MessageBox.Show("Fiyat Güncellendi En Ucuz Ürün Sizin Paylaşımınız Oldu !");

                txtYeniFiyat.Text = "";
                txtYeniFiyat.Visibility = Visibility.Collapsed;
                btnGuncelleFiyat.Visibility = Visibility.Collapsed;

                al.ProfilPaylasimlarAsync(kullaniciid);
                al.ProfilPaylasimlarCompleted += new EventHandler<ProfilPaylasimlarCompletedEventArgs>(al_ProfilPaylasimlarCompleted);

                lstUrunlerim.SelectedIndex = -1;
            }

            if (e.Result == 2)
            {
                MessageBox.Show("Paylaşımınız Güncellendi !");

                txtYeniFiyat.Visibility = Visibility.Collapsed;
                btnGuncelleFiyat.Visibility = Visibility.Collapsed;

                al.ProfilPaylasimlarAsync(kullaniciid);
                al.ProfilPaylasimlarCompleted += new EventHandler<ProfilPaylasimlarCompletedEventArgs>(al_ProfilPaylasimlarCompleted);

                lstUrunlerim.SelectedIndex = -1;
            }

            if (e.Result == 3)
            {
                MessageBox.Show("En Ucuz Fiyat Güncellenemez !");
            }

            if (e.Result == 4)
            {
                MessageBox.Show("Paylaşım Güncellenemedi Tekrar Deneyiniz !");
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            txtYeniFiyat.Visibility = Visibility.Collapsed;
            btnGuncelleFiyat.Visibility = Visibility.Collapsed;

            lstUrunlerim.SelectedIndex = -1;

            base.OnBackKeyPress(e);
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ProfilGuncelle.xaml?kullaniciid=" + kullaniciid + "&kullaniciad=" + kullaniciad + "&mahalleid=" + mahalleid, UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarMenuItem_Click_2(object sender, EventArgs e)  // login ekranına giriş...
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream isfs = isf.OpenFile("EnUcuzUrunCookies", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(isfs))
                    {
                        sw.WriteLine("0");
                        sw.Close();
                    }
                }
            }
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));   // cookie silindi...
        }
    }
}