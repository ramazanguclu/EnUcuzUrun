﻿#pragma checksum "C:\Users\hcrb__000\Documents\Visual Studio 2015\Projects\EnUcuzUrun\EnUcuzUrun\UyeOl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C3DB193F833A3C14FC488DD7CAB5E673"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace EnUcuzUrun {
    
    
    public partial class UyeOl : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.PhoneTextBox txtEmail;
        
        internal Microsoft.Phone.Controls.PhoneTextBox txtAdSoyad;
        
        internal System.Windows.Controls.PasswordBox pwParola;
        
        internal System.Windows.Controls.PasswordBox pwParolaTekrar;
        
        internal Microsoft.Phone.Controls.ListPicker lpkMahalle;
        
        internal System.Windows.Controls.TextBlock textBlockError;
        
        internal System.Windows.Controls.Button btnKaydol;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/EnUcuzUrun;component/UyeOl.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.txtEmail = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("txtEmail")));
            this.txtAdSoyad = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("txtAdSoyad")));
            this.pwParola = ((System.Windows.Controls.PasswordBox)(this.FindName("pwParola")));
            this.pwParolaTekrar = ((System.Windows.Controls.PasswordBox)(this.FindName("pwParolaTekrar")));
            this.lpkMahalle = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("lpkMahalle")));
            this.textBlockError = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockError")));
            this.btnKaydol = ((System.Windows.Controls.Button)(this.FindName("btnKaydol")));
        }
    }
}

