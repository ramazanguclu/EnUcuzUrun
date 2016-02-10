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
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Input;
using Windows.Storage;
using Windows.Foundation;

namespace EnUcuzUrun
{
    public partial class UyeOl : PhoneApplicationPage
    {
        SolidColorBrush green = new SolidColorBrush(Colors.Green);
        SolidColorBrush red = new SolidColorBrush(Colors.Red);
        int minPasswordChar = 5;
        int sayi;

        Service1Client al = new Service1Client();
        harfler AdDuzenle = new harfler();

        string parola1;
        string parola2;
        public UyeOl()
        {
            InitializeComponent();

            try
            {
                al.IzmitMahallelerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }


            al.IzmitMahallelerCompleted += new EventHandler<IzmitMahallelerCompletedEventArgs>(al_IzmitMahallelerCompleted);


            Random sayi2 = new Random();
            sayi = sayi2.Next();

        }

        public static bool ValidateEmail(string str)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(str, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            txtEmail.BorderBrush = green;
            textBlockError.Text = "";
            textBlockError.Visibility = Visibility.Collapsed;
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            txtEmail.BorderBrush = null;
        }

        private void pwParola_LostFocus(object sender, RoutedEventArgs e)
        {
            pwParola.BorderBrush = null;
        }

        private void pwParola_GotFocus(object sender, RoutedEventArgs e)
        {
            pwParola.BorderBrush = green;
            textBlockError.Text = "";
            textBlockError.Visibility = Visibility.Collapsed;

        }

        private void pwParolaTekrar_GotFocus(object sender, RoutedEventArgs e)
        {
            pwParolaTekrar.BorderBrush = green;
            textBlockError.Text = "";
            textBlockError.Visibility = Visibility.Collapsed;

        }

        private void pwParolaTekrar_LostFocus(object sender, RoutedEventArgs e)
        {
            pwParolaTekrar.BorderBrush = null;
        }

        private void al_IzmitMahallelerCompleted(object sender, IzmitMahallelerCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    lpkMahalle.ItemsSource = e.Result;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("" + ex);
            }

        }

        private void btnKaydol_Click(object sender, RoutedEventArgs e)
        {
            if (txtAdSoyad.Text != string.Empty && txtEmail.Text != string.Empty && pwParola.Password != string.Empty && pwParolaTekrar.Password != string.Empty)
            {
                parola1 = pwParola.Password;
                parola2 = pwParolaTekrar.Password;

                if (ValidateEmail(txtEmail.Text))
                {
                    al.EmailVarMıAsync(txtEmail.Text);
                    al.EmailVarMıCompleted += new EventHandler<EmailVarMıCompletedEventArgs>(al_EmailVarMıCompleted);
                }

                else
                {
                    textBlockError.Text = "Geçersiz Email Adresi !";
                    txtEmail.BorderBrush = red;
                    textBlockError.Visibility = Visibility.Visible;
                    txtEmail.Text = string.Empty;
                }
            }

            else
            {
                textBlockError.Text = "Tüm Alanları Doldurunuz! !";
                txtEmail.BorderBrush = red;
                textBlockError.Visibility = Visibility.Visible;
            }
        }

        void al_EmailVarMıCompleted(object sender, EmailVarMıCompletedEventArgs e)
        {
            if (e.Result == false)
            {
                if (parola1 == parola2)
                {
                    if (pwParola.Password.Length > minPasswordChar)
                    {
                        Kullanicilar kullanici = new Kullanicilar { Email = txtEmail.Text, Ad = AdDuzenle.TextLowerAndFirstUpper(txtAdSoyad.Text), Parola = pwParola.Password, Tarih = DateTime.Now, Onay = false, Sayi = sayi.ToString(), MahalleId = lpkMahalle.SelectedIndex + 1, KullaniciResim = null };
                        al.KullaniciKayitAsync(kullanici);
                        al.KullaniciKayitCompleted += new EventHandler<KullaniciKayitCompletedEventArgs>(al_KullaniciKayitCompleted);

                    }
                    else
                    {
                        textBlockError.Text = "Parola " + minPasswordChar + " karakterden fazla olmalı !";
                        pwParola.BorderBrush = red;
                        textBlockError.Visibility = Visibility.Visible;
                        pwParola.Password = string.Empty;
                        pwParolaTekrar.Password = string.Empty;
                    }
                }

                else
                {
                    textBlockError.Text = "Parolalar Uyuşmamaktdır !";
                    txtEmail.BorderBrush = red;
                    textBlockError.Visibility = Visibility.Visible;
                    pwParola.Password = string.Empty;
                    pwParolaTekrar.Password = string.Empty;
                }
            }

            else
            {
                textBlockError.Text = "Bu Email Adresi Kullanılmaktadır !";
                txtEmail.BorderBrush = red;
                textBlockError.Visibility = Visibility.Visible;
                pwParola.Password = string.Empty;
                pwParolaTekrar.Password = string.Empty;
                txtEmail.Text = string.Empty;
            }
        }

        void al_KullaniciKayitCompleted(object sender, KullaniciKayitCompletedEventArgs e)
        {
            if (e.Result)
            {
                MessageBox.Show("Kayıt Tamamlandı !");

                al.uyelikmailAsync(txtEmail.Text);
                al.uyelikmailCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(al_uyelikmailCompleted);
            }

        }

        void al_uyelikmailCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Üyeliğinizin Onaylanması İçin Mailinize Gelen Linke Tıklayın !");
            NavigationService.GoBack();
        }
    }
}