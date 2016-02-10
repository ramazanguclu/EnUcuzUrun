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
    public partial class ProfilGuncelle : PhoneApplicationPage
    {
        int kullaniciid;
        string kullaniciad;
        int mahalleid;

        Service1Client al = new Service1Client();
        public ProfilGuncelle()
        {
            InitializeComponent();


        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            kullaniciid = Convert.ToInt32(NavigationContext.QueryString["kullaniciid"]);
            kullaniciad = NavigationContext.QueryString["kullaniciad"];
            mahalleid = Convert.ToInt32(NavigationContext.QueryString["mahalleid"]);
            base.OnNavigatedTo(e);

            txtAdSoyad.Text = kullaniciad;
            al.IzmitMahallelerAsync();
            al.IzmitMahallelerCompleted += new EventHandler<IzmitMahallelerCompletedEventArgs>(al_IzmitMahallelerCompleted);

        }

        private void al_IzmitMahallelerCompleted(object sender, IzmitMahallelerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                lpkMahalle.ItemsSource = e.Result;

            }

        }

        private void btnGuncelle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lpkMahalle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {



        }
    }
}