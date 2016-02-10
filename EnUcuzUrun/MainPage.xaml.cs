using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.IO;
using System.IO.IsolatedStorage;
using EnUcuzUrun.ServiceReferenceEnUcuzUrun;

namespace EnUcuzUrun
{
    public partial class MainPage : PhoneApplicationPage
    {

        Service1Client client = new Service1Client();

        string parola = "";
        int KullaniciId = 0;
        bool beniHatirla = false;
        public MainPage()
        {
            InitializeComponent();
            parola = txtParola.Text;

        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string line = null;
            string line_to_delete = "0";
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream isfs = isf.OpenFile("EnUcuzUrunCookies", FileMode.OpenOrCreate))
                {
                    using (StreamReader sr = new StreamReader(isfs))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (string.Compare(line, line_to_delete) == 0)
                                continue;

                            if (!string.IsNullOrEmpty(line))
                                KullaniciId = Convert.ToInt32(line);
                        }
                        sr.Close();

                        if (KullaniciId != 0)
                            NavigationService.Navigate(new Uri("/PivotPageEnUcuz.xaml?kullaniciid=" + KullaniciId, UriKind.RelativeOrAbsolute));
                    }
                }
            }
        }

        private void hypbtnUyeOl_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/UyeOl.xaml", UriKind.Relative));
        }

        private void btnGiris_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(txtParola.Text))
            {
                client.GirisEmailAsync(txtEmail.Text);
                client.GirisEmailCompleted += new EventHandler<GirisEmailCompletedEventArgs>(al_GirisEmailCompleted);
            }
            else
            {
                MessageBox.Show("Email ve Parola Alanlarını Doldurunuz!");
            }
        }

        void al_GirisEmailCompleted(object sender, GirisEmailCompletedEventArgs e)
        {
            if (e.Result)
            {
                client.GirisOnayAsync(txtEmail.Text);
                client.GirisOnayCompleted += new EventHandler<GirisOnayCompletedEventArgs>(al_GirisOnayCompleted);
            }
            else
            {
                MessageBox.Show("Bu Email Adresi Kayıtlı Değildir!");
                txtEmail.Text = string.Empty;
                txtParola.Text = string.Empty;
            }
        }

        void al_GirisOnayCompleted(object sender, GirisOnayCompletedEventArgs e)
        {
            if (e.Result)
            {
                client.GirisParolaKontrolAsync(txtParola.Text, txtEmail.Text);
                client.GirisParolaKontrolCompleted += new EventHandler<GirisParolaKontrolCompletedEventArgs>(al_GirisParolaKontrolCompleted);
            }
            else
            {
                MessageBox.Show("Bu Email Adresi Onaylı Değildir!");

                txtEmail.Text = string.Empty;
                txtParola.Text = string.Empty;
            }
        }

        void al_GirisParolaKontrolCompleted(object sender, GirisParolaKontrolCompletedEventArgs e)
        {
            try
            {
                if (e.Result)
                {
                    client.KullaniciIdDondurAsync(txtEmail.Text);
                    client.KullaniciIdDondurCompleted += new EventHandler<KullaniciIdDondurCompletedEventArgs>(al_KullaniciIdDondurCompleted);
                }
                else
                {
                    MessageBox.Show("Parolayı Hatalı Girdiniz!");
                    txtParola.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void al_KullaniciIdDondurCompleted(object sender, KullaniciIdDondurCompletedEventArgs e)
        {
            try
            {
                UygulamaGiris(e.Result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UygulamaGiris(int kulid)
        {

            if (kulid == 14)
            {
                NavigationService.Navigate(new Uri("/PivotPageAdminPaneli.xaml", UriKind.Relative));
            }

            else if (beniHatirla && kulid != 14)
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream isfs = isf.OpenFile("EnUcuzUrunCookies", FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(isfs))
                        {
                            sw.WriteLine(kulid.ToString());
                            sw.Close();
                        }
                    }
                }
            }

            else
            {
                NavigationService.Navigate(new Uri("/PivotPageEnUcuz.xaml?kullaniciid=" + kulid, UriKind.RelativeOrAbsolute));
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/UyeOnay.xaml", UriKind.Relative));
        }

        private void hypbtnUnuttum_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SifremiUnuttum.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Scan.xaml", UriKind.Relative));
        }

        private void btnCikis_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Terminate();
        }

        private void chkbxBeniHatirla_Checked(object sender, RoutedEventArgs e)
        {
            beniHatirla = true;
        }

        private void chkbxBeniHatirla_Unchecked(object sender, RoutedEventArgs e)
        {
            beniHatirla = false;
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Harita.xaml", UriKind.Relative));
        }

        private void button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Resim.xaml", UriKind.Relative));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Harita.xaml", UriKind.Relative));
        }

        private void PasswordLostFocus(object sender, RoutedEventArgs e)
        {

        }


        private void PasswordGotFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}