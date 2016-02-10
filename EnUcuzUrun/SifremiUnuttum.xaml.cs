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
    public partial class SifremiUnuttum : PhoneApplicationPage
    {
        Service1Client al = new Service1Client();
        public SifremiUnuttum()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            al.sifremiunuttumAsync(txtEmail.Text);
            al.sifremiunuttumCompleted += new EventHandler<sifremiunuttumCompletedEventArgs>(al_sifremiunuttumCompleted);
        }

        void al_sifremiunuttumCompleted(object sender, sifremiunuttumCompletedEventArgs e)
        {
            if (e.Result)
            {
                MessageBox.Show("Şifreniz Mailinize Gönderilmiştir.");
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Geçersiz Email Adresi !");
            }
        }
    }
}