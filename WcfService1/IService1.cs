using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService1
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<Kullanicilar> Listeleme();

        [OperationContract]
        List<Kullanicilar> Arama(string isim);

        [OperationContract]
        bool Kaydetme(Kullanicilar veri);
        [OperationContract]
        void Silme(int veri);
        [OperationContract]
        bool GirisEmail(string email);
        [OperationContract]
        bool GirisOnay(string email_onay);
        [OperationContract]
        bool GirisParolaKontrol(string parola, string email);
        [OperationContract]
        List<Kullanicilar> AramaEmail(string email);
        [OperationContract]
        int KullaniciIdDondur(string email);
        [OperationContract]
        List<IzmitMahalle> IzmitMahalleler();
        [OperationContract]
        int MahalleIdDondur(string mahalleadi);
        [OperationContract]
        List<View_AnasayfaUrunTakipListe> anasayfaliste(int kullaniciid);
        [OperationContract]
        List<View_UrunMalumat> UrunBilgi(int barkodid);
        [OperationContract]
        List<View_Yorumlar> YorumlariGetir(int barkodid);
        [OperationContract]
        List<View_Marketler> Marketler();
        [OperationContract]
        List<View_MarketUrunleri> MarketUrunleri(int MarketId);
        [OperationContract]
        List<View_UrunMalumat> ProfilPaylasimlar(int kullaniciid);
        [OperationContract]
        List<View_Marketler> ProfilMarketlerim(int kullaniciid);

        [OperationContract]
        KeyValuePair<bool, Urunler> BarkodVarMi(string barkodid);

        [OperationContract]
        List<Marketler> MarketlerListeUrunKaydet(int mahalleid);

        [OperationContract]
        bool MarketKayit(Marketler veri);

        [OperationContract]
        KeyValuePair<bool, string> UrunlerKayit(Urunler urunler);

        [OperationContract]
        int EnUcuz(int barkodid);
        [OperationContract]
        List<View_UrunlerMarketMahalle> EnUcuzListeDondur();
        [OperationContract]
        List<Urunler> UrunArama(string urunad);
        [OperationContract]
        List<View_Marketler> MarketArama(string marketad);
        [OperationContract]
        void KullaniciOnayGüncelle(bool onay, int kullaniciid);
        [OperationContract]
        void MArketAdGüncelle(string marketad, int marketid, string adres);
        [OperationContract]
        List<Urunler> Urunler();
        [OperationContract]
        void UrunAdGuncelle(int urunid, string urunad);

        [OperationContract]
        bool YorumKayit(Yorumlar yorum);
        [OperationContract]
        bool KullaniciKayit(Kullanicilar kullanici);
        [OperationContract]
        bool EmailVarMı(string email);
        [OperationContract]
        List<View_KullaniciBilgi> KullaniciAdDondur(int id);
        [OperationContract]
        bool PaylasimSil(int paylasimid, int barkodid, decimal fiyat);


        [OperationContract]
        int PaylasimGuncelle(int paylasimid, decimal fiyat, DateTime tarih, int barkodid, decimal eskifiyat, int marketid);

        [OperationContract]
        bool UrunlerEnUcuzuGir(int barkod, decimal fiyat, int marketid);
        [OperationContract]
        void uyelikmail(string Email);
        [OperationContract]
        bool uyelikonaylama(string email, string sayi);
        [OperationContract]
        bool sifremiunuttum(string Email);

        [OperationContract]
        List<string> PaylasimUrunTakipKayit(int barkodId, decimal ucret, int marketId, int kullaniciid);

        [OperationContract]
        string UploadImage(byte[] buffer, string blobName, string containerName);

        [OperationContract]
        byte[] DownloadImage(string blobName, string containerName);

        [OperationContract]
        List<byte[]> TakipEdilenUrunImages(string containerName, List<string> blobName);
    }
}
