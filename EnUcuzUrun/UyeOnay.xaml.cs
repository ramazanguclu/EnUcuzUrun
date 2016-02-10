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
    public partial class UyeOnay : PhoneApplicationPage
    {
        Service1Client al = new Service1Client();
        public UyeOnay()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text.Trim()) && !string.IsNullOrEmpty(txtOnaySayi.Text.Trim()))
            {
                al.uyelikonaylamaAsync(txtEmail.Text, txtOnaySayi.Text);
                al.uyelikonaylamaCompleted += new EventHandler<uyelikonaylamaCompletedEventArgs>(al_uyelikonaylamaCompleted);
            }
            else
                MessageBox.Show("Lütfen mail ve onay numarası alanlarını düzgün biçimde doldurunuz.");

        }

        void al_uyelikonaylamaCompleted(object sender, uyelikonaylamaCompletedEventArgs e)
        {
            if (e.Result)
            {
                MessageBox.Show("Uyeliğiniz Onaylandı Giriş Yapabilrsiniz !");
                NavigationService.GoBack();
            }

            else
            {
                MessageBox.Show("Hatalı Email yada Onay Kodu !");
            }
        }
    }
}