using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using EnUcuzUrun.ServiceReferenceEnUcuzUrun;
using System.IO;

namespace EnUcuzUrun
{
    public partial class PivotPageUrunKaydet : PhoneApplicationPage
    {
        Service1Client al = new Service1Client();
        int KullaniciId = PivotPageEnUcuz.kullaniciid;
        string barkodDatasi = "";
        string BarkodTur = "";
        bool BarkodKontrol = false;
        int barkodId = -1;
        decimal enUcuzFiyat = 0;
        decimal ucret;
        byte[] fotoDizisi = null;
        string uzanti = "";
        bool fotoDegistimi = false;
        string path = "";
        byte[] dbFotoDizi = null;
        WriteableBitmap wb = new WriteableBitmap(200, 222);
        public PivotPageUrunKaydet()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (string.IsNullOrEmpty(tbBarkodData.Text))
            {
                base.OnNavigatedTo(e);

                barkodDatasi = NavigationContext.QueryString["barkod"];
                BarkodTur = NavigationContext.QueryString["barkodtur"];

                tbBarkodData.Text = barkodDatasi;
                tbBarkodTur.Text = BarkodTur;
                if (!string.IsNullOrEmpty(barkodDatasi))
                {
                    al.BarkodVarMiAsync(barkodDatasi);
                    al.BarkodVarMiCompleted += Al_BarkodVarMiCompleted;
                }
            }

        }

        private void Al_BarkodVarMiCompleted(object sender, BarkodVarMiCompletedEventArgs e)
        {
            if (e.Result.key)
            {
                BarkodKontrol = true;
                txtUrunIsim.Text = e.Result.value.UrunAd;
                barkodId = e.Result.value.BarkodId; // eğer barkodu okutulan ürünün daha önceden veritabanına kaydı varsa o ürünün barkodidsi atandı...
                enUcuzFiyat = e.Result.value.EnUcuzFiyat;
                al.DownloadImageAsync(e.Result.value.UrunResim, "urunresimleri");
                al.DownloadImageCompleted += Al_DownloadImageCompleted;
            }
            else
            {
                BarkodKontrol = false;
                BitmapImage licoriceImage = new BitmapImage(new Uri("/Assets/sepet.png", UriKind.Relative));
                imgUrunResim.Source = licoriceImage;
            }
        }

        private void Al_DownloadImageCompleted(object sender, DownloadImageCompletedEventArgs e)
        {
            dbFotoDizi = e.Result;
            VeritabanındanFotoYukle();
        }

        private void VeritabanındanFotoYukle()
        {
            wb.Clear();
            using (MemoryStream ms = new MemoryStream(dbFotoDizi))
            {
                wb.SetSource(ms);
                imgUrunResim.Source = wb;
            }
        }

        private void btnResimDegistir_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureTask kamera = new CameraCaptureTask();
            kamera.Show();
            kamera.Completed += Kamera_Completed;
        }

        private void Kamera_Completed(object sender, PhotoResult e)
        {
            uzanti = "";
            uzanti = Path.GetExtension(e.OriginalFileName);

            //BitmapImage kaynak = new BitmapImage();
            //kaynak.SetSource(e.ChosenPhoto);
            //imgUrunResim.Source = kaynak;
            wb.Clear();
            wb.SetSource(e.ChosenPhoto);
            wb = wb.Resize(Convert.ToInt32(200), Convert.ToInt32(222), WriteableBitmapExtensions.Interpolation.Bilinear);
            imgUrunResim.Source = wb;

            fotoDegistimi = true;
        }

        private void appBtnPaylas_Click(object sender, EventArgs e)
        {
            if (txtUrunFiyat.Text != string.Empty && txtUrunIsim.Text != string.Empty)
            {
                ucret = Convert.ToDecimal(txtUrunFiyat.Text);
                path = "/Assets/sepet.png";

                if (fotoDegistimi)
                {
                    path = txtUrunIsim.Text + tbBarkodData.Text + uzanti;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        WriteableBitmap btmMap = new WriteableBitmap(imgUrunResim.Source as BitmapSource);
                        btmMap = btmMap.Resize(Convert.ToInt32(200), Convert.ToInt32(222), WriteableBitmapExtensions.Interpolation.Bilinear);
                        btmMap.SaveJpeg(ms, btmMap.PixelWidth, btmMap.PixelHeight, 0, 50);
                        fotoDizisi = ms.ToArray();
                    }
                    al.UploadImageAsync(fotoDizisi, txtUrunIsim.Text + tbBarkodData.Text + uzanti, "urunresimleri");
                    al.UploadImageCompleted += Al_UploadImageCompleted;
                }


                if (!BarkodKontrol) // eğer ürün daha önceden eklenmemişse
                {
                    Urunler urun = new Urunler    // ürünler veritabanına veri kaydı..
                    {
                        Barkod = barkodDatasi,
                        UrunAd = txtUrunIsim.Text,
                        UrunIlkEkleyen = KullaniciId,
                        EnUcuzFiyat = ucret,
                        MarketId = 1,
                        UrunResim = path,
                        BarkodTur = BarkodTur
                    };

                    al.UrunlerKayitAsync(urun);
                    al.UrunlerKayitCompleted += new EventHandler<UrunlerKayitCompletedEventArgs>(al_UrunlerKayitCompleted);
                }


                if (BarkodKontrol)
                {
                    al.PaylasimUrunTakipKayitAsync(barkodId, ucret, 1, KullaniciId);
                    al.PaylasimUrunTakipKayitCompleted += Al_PaylasimUrunTakipKayitCompleted;
                }

                NavigationService.GoBack();
            }
        }

        private void Al_UploadImageCompleted(object sender, UploadImageCompletedEventArgs e)
        {
            MessageBox.Show(e.Result);
        }

        private void Al_PaylasimUrunTakipKayitCompleted(object sender, PaylasimUrunTakipKayitCompletedEventArgs e)
        {
            foreach (string item in e.Result)
            {
                MessageBox.Show(item);
            }
        }
        void al_UrunlerKayitCompleted(object sender, UrunlerKayitCompletedEventArgs e)
        {
            if (e.Result.key)
            {
                MessageBox.Show("Ürün Başarıyla Kaydedildi!");  // bakılacak  // ürnler tablosundan son kaydedilmiş ürünün barkodidsi alınacak     
            }


            else if (!e.Result.key)
                MessageBox.Show("Ürün Kaydederken Bir Hata Oluştu! " + e.Result.value);

        }

        private void appBtnUrunAra_Click(object sender, EventArgs e)
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

        private void btnResimTemizle_Click(object sender, RoutedEventArgs e)
        {
            if (BarkodKontrol)
            {
                VeritabanındanFotoYukle();
            }
            else
            {
                BitmapImage licoriceImage = new BitmapImage(new Uri("/Assets/sepet.png", UriKind.Relative));
                imgUrunResim.Source = licoriceImage;
            }
            fotoDegistimi = false;
        }
    }
}