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
    public partial class UrunSayfasi : PhoneApplicationPage
    {
        Service1Client al = new Service1Client();

        int barkodid = -1;
        string urunad = "";
        int KullaniciId = PivotPageEnUcuz.kullaniciid;

        public UrunSayfasi()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            barkodid = Convert.ToInt32(NavigationContext.QueryString["urun"]);
            urunad = NavigationContext.QueryString["urunad"];
            //tbUrunAd.Text = urunad;
            if (barkodid != -1)
            {
                al.UrunBilgiAsync(barkodid);
                al.YorumlariGetirAsync(barkodid);

                al.UrunBilgiCompleted += new EventHandler<UrunBilgiCompletedEventArgs>(al_UrunBilgiCompleted);
                al.YorumlariGetirCompleted += new EventHandler<YorumlariGetirCompletedEventArgs>(al_YorumlariGetirCompleted);
            }
        }

        void al_YorumlariGetirCompleted(object sender, YorumlariGetirCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                lstYorumlar.ItemsSource = e.Result;
            }
        }

        void al_UrunBilgiCompleted(object sender, UrunBilgiCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                lstUrunBilgi.ItemsSource = e.Result;
            }
        }
       
        private void appBtnPaylas_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtYorum.Text.Trim()))
            {
                Yorumlar yorum = new Yorumlar { BarkodId = barkodid, KullaniciId = KullaniciId, Tarih = DateTime.Now, Yorum = txtYorum.Text };
                al.YorumKayitAsync(yorum);
                al.YorumKayitCompleted += new EventHandler<YorumKayitCompletedEventArgs>(al_YorumKayitCompleted);
            }
        }
        void al_YorumKayitCompleted(object sender, YorumKayitCompletedEventArgs e)
        {
            if (e.Result)
            {
                txtYorum.Text = string.Empty;
                MessageBox.Show("Yorumunuz Gönderildi.");
                al.YorumlariGetirAsync(barkodid);
                al.YorumlariGetirCompleted += new EventHandler<YorumlariGetirCompletedEventArgs>(al_YorumlariGetirCompleted);
            }
        }

    }
}