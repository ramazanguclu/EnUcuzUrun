﻿#pragma checksum "C:\Users\hcrb__000\Documents\Visual Studio 2015\Projects\EnUcuzUrun\EnUcuzUrun\PivotPageUrunKaydet.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7610D412B015B5D000C7A7073300F3D6"
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
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Shell;
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
    
    
    public partial class PivotPageUrunKaydet : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Image imgUrunResim;
        
        internal System.Windows.Controls.TextBlock tbBarkodData;
        
        internal System.Windows.Controls.TextBlock tbBarkodTur;
        
        internal System.Windows.Controls.TextBox txtUrunFiyat;
        
        internal System.Windows.Controls.TextBox txtUrunIsim;
        
        internal System.Windows.Controls.Button btnResimDegistir;
        
        internal System.Windows.Controls.Button btnResimTemizle;
        
        internal Microsoft.Phone.Maps.Controls.Map mapMarketler;
        
        internal System.Windows.Controls.TextBox txtMarketIsmi;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton appBtnPaylas;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton appBtnUrunAra;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/EnUcuzUrun;component/PivotPageUrunKaydet.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.imgUrunResim = ((System.Windows.Controls.Image)(this.FindName("imgUrunResim")));
            this.tbBarkodData = ((System.Windows.Controls.TextBlock)(this.FindName("tbBarkodData")));
            this.tbBarkodTur = ((System.Windows.Controls.TextBlock)(this.FindName("tbBarkodTur")));
            this.txtUrunFiyat = ((System.Windows.Controls.TextBox)(this.FindName("txtUrunFiyat")));
            this.txtUrunIsim = ((System.Windows.Controls.TextBox)(this.FindName("txtUrunIsim")));
            this.btnResimDegistir = ((System.Windows.Controls.Button)(this.FindName("btnResimDegistir")));
            this.btnResimTemizle = ((System.Windows.Controls.Button)(this.FindName("btnResimTemizle")));
            this.mapMarketler = ((Microsoft.Phone.Maps.Controls.Map)(this.FindName("mapMarketler")));
            this.txtMarketIsmi = ((System.Windows.Controls.TextBox)(this.FindName("txtMarketIsmi")));
            this.appBtnPaylas = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("appBtnPaylas")));
            this.appBtnUrunAra = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("appBtnUrunAra")));
        }
    }
}

