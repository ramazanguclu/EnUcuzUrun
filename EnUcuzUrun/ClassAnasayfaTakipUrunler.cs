using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace EnUcuzUrun
{
    class ClassAnasayfaTakipUrunler
    {
        public int BarkodId { get; set; }
        public int KullaniciId { get; set; }
        public bool Takip { get; set; }
        public int TakipId { get; set; }
        public string Barkod { get; set; }
        public string EnUcuzFiyat { get; set; }
        public string UrunAd { get; set; }
        public int UrunIlkEkleyen { get; set; }
        public WriteableBitmap UrunResim { get; set; }
        public int MarketId { get; set; }
        public string MarketAdi { get; set; }
    }
}
