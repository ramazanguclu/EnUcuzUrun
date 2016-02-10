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
using System.IO;
using Microsoft.Xna.Framework.Media;
using EnUcuzUrun.ServiceReferenceEnUcuzUrun;
using System.Configuration;
using System.Windows.Media;


namespace EnUcuzUrun
{
    public partial class Resim : PhoneApplicationPage
    {
        Service1Client client = null;
        public Resim()
        {
            InitializeComponent();
            try
            {
                client = new Service1Client();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
                  
        }
    }

}