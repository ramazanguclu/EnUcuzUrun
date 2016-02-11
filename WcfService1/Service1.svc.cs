using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web;
using System.Data.Entity.Validation;
using System.Security.Cryptography;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;


namespace WcfService1
{
    public class Service1 : IService1
    {
        veritabaniEnucuzUrunEntities col = new veritabaniEnucuzUrunEntities();
        public List<Kullanicilar> Listeleme()
        {
            return col.Kullanicilars.ToList();
        }
        public List<Kullanicilar> Arama(string isim)
        {

            return (from c in col.Kullanicilars
                    where c.Ad == isim
                    select c).ToList();
        }
        public bool Kaydetme(Kullanicilar veri)
        {
            col.Kullanicilars.Add(veri);
            col.SaveChanges();
            return true;
        }
        public void Silme(int veri)
        {
            var silmeislemi = (from al in col.Kullanicilars
                               where al.KullaniciId == veri
                               select al).Single();

            col.Kullanicilars.Remove(silmeislemi);
            col.SaveChanges();
        }
        public bool GirisEmail(string email)
        {
            return col.Kullanicilars.Any(f => f.Email == email);
        }

        public bool GirisOnay(string email_onay)
        {
            return (from f in col.Kullanicilars
                    where f.Email == email_onay && f.Onay == true
                    select f).SingleOrDefault() != null;
        }

        public bool GirisParolaKontrol(string parola, string email)
        {
            parola = MD5Sifrele(parola);
            return (from f in col.Kullanicilars
                    where f.Parola == parola && f.Email == email
                    select f).SingleOrDefault() != null;
        }

        public List<Kullanicilar> AramaEmail(string email)
        {
            return (from c in col.Kullanicilars
                    where c.Email == email
                    select c).ToList();
        }

        public int KullaniciIdDondur(string email)
        {
            var veri = (from al in col.Kullanicilars
                        where al.Email == email
                        select al.KullaniciId).Single();

            return veri;
        }

        public List<IzmitMahalle> IzmitMahalleler()
        {
            col.Database.Connection.Open();
            return col.IzmitMahalles.ToList();
        }

        public int MahalleIdDondur(string mahalleadi)
        {
            var mah = (from al in col.IzmitMahalles
                       where al.MahalleAdi == mahalleadi
                       select al.MahalleId).Single();

            return mah;
        }

        public List<View_AnasayfaUrunTakipListe> anasayfaliste(int kullaniciid)
        {

            List<View_AnasayfaUrunTakipListe> liste = (from c in col.View_AnasayfaUrunTakipListe
                                                       where c.KullaniciId == kullaniciid && c.Takip == true
                                                       select c).ToList();
            return liste;
        }

        public List<View_UrunMalumat> UrunBilgi(int barkodid)
        {
            var liste2 = (from c in col.View_UrunMalumat
                          where c.BarkodId == barkodid
                          select c).ToList();
            return liste2;
        }

        public List<View_Yorumlar> YorumlariGetir(int barkodid)
        {
            var liste3 = (from c in col.View_Yorumlar
                          where c.BarkodId == barkodid
                          select c).ToList();
            return liste3;
        }
        public List<View_Marketler> Marketler()
        {
            return col.View_Marketler.ToList();
        }
        public List<View_MarketUrunleri> MarketUrunleri(int MarketId)
        {
            var liste4 = (from c in col.View_MarketUrunleri
                          where c.MarketId == MarketId
                          select c).ToList();
            return liste4;
        }
        public List<View_UrunMalumat> ProfilPaylasimlar(int kullaniciid)
        {
            var liste5 = (from c in col.View_UrunMalumat
                          where c.KullaniciId == kullaniciid
                          select c).ToList();
            return liste5;
        }
        public List<View_Marketler> ProfilMarketlerim(int kullaniciid)
        {
            var liste6 = (from c in col.View_Marketler
                          where c.MarketEkleyen == kullaniciid
                          select c).ToList();
            return liste6;
        }
        public KeyValuePair<bool, Urunler> BarkodVarMi(string barkodid)
        {
            KeyValuePair<bool, Urunler> sonuc = new KeyValuePair<bool, Urunler>(false, null);
            if (col.Urunlers.Any(f => f.Barkod == barkodid))
            {
                Urunler urun = (from nesne in col.Urunlers
                                where nesne.Barkod == barkodid
                                select nesne).FirstOrDefault();
                sonuc = new KeyValuePair<bool, Urunler>(true, urun);
            }
            else
                sonuc = new KeyValuePair<bool, Urunler>(false, null);

            return sonuc;
        }
        public List<Marketler> MarketlerListeUrunKaydet(int mahalleid)
        {
            var liste6 = (from c in col.Marketlers
                          where c.MahalleId == mahalleid
                          select c).ToList();

            return liste6;
        }
        public bool MarketKayit(Marketler veri)
        {
            col.Marketlers.Add(veri);
            col.SaveChanges();
            return true;
        }
        public KeyValuePair<bool, string> UrunlerKayit(Urunler urunler)
        {
            KeyValuePair<bool, string> sonuc = new KeyValuePair<bool, string>(false, null);
            try
            {
                col.Urunlers.Add(urunler);
                int sayi = col.SaveChanges();
                if (sayi > 0)
                {
                    sonuc = new KeyValuePair<bool, string>(true, "Kayıt İşlemi Başarılı");
                }
            }
            catch (Exception ex)
            {
                sonuc = new KeyValuePair<bool, string>(false, ex.ToString());
            }
            return sonuc;
        }
        public int EnUcuz(int barkodid)
        {
            var liste = (from c in col.View_MarketUrunleri
                         where c.BarkodId == barkodid
                         select c.PaylasimId).Single();
            return liste;
        }
        public List<View_UrunlerMarketMahalle> EnUcuzListeDondur()
        {
            return col.View_UrunlerMarketMahalle.ToList();
        }
        public List<Urunler> UrunArama(string urunad)
        {
            var urun = (from c in col.Urunlers
                        where c.UrunAd.Contains(urunad)
                        select c).ToList();
            return urun;
        }
        public List<View_Marketler> MarketArama(string marketad)
        {
            var market = (from c in col.View_Marketler
                          where c.MarketAdi.Contains(marketad)
                          select c).ToList();
            return market;
        }
        public void KullaniciOnayGüncelle(bool onay, int kullaniciid)
        {
            Kullanicilar veri = (from c in col.Kullanicilars
                                 where c.KullaniciId == kullaniciid
                                 select c).FirstOrDefault();

            if (veri != null)
            {
                veri.Onay = onay;
                col.SaveChanges();
            }


        }
        public void MArketAdGüncelle(string marketad, int marketid, string adres)
        {
            Marketler veri = (from c in col.Marketlers
                              where c.MarketId == marketid
                              select c).FirstOrDefault();
            if (veri != null)
            {
                veri.MarketAdi = marketad;
                veri.Adres = adres;
                col.SaveChanges();
            }
        }
        public List<Urunler> Urunler()
        {
            return col.Urunlers.ToList();
        }
        public void UrunAdGuncelle(int urunid, string urunad)
        {
            Urunler veri = (from c in col.Urunlers
                            where c.BarkodId == urunid
                            select c).FirstOrDefault();
            if (veri != null)
            {
                veri.UrunAd = urunad;
                col.SaveChanges();
            }
        }
        public bool YorumKayit(Yorumlar yorum)
        {
            col.Yorumlars.Add(yorum);
            col.SaveChanges();
            return true;
        }
        public bool KullaniciKayit(Kullanicilar kullanici)
        {
            kullanici.Parola = MD5Sifrele(kullanici.Parola);
            col.Kullanicilars.Add(kullanici);
            col.SaveChanges();
            return true;
        }
        public bool EmailVarMı(string email)
        {
            return col.Kullanicilars.Any(f => f.Email == email);
        }
        public List<View_KullaniciBilgi> KullaniciAdDondur(int id)
        {
            var kullanici = (from c in col.View_KullaniciBilgi
                             where c.KullaniciId == id
                             select c).ToList();
            return kullanici;
        }
        public bool PaylasimSil(int paylasimid, int barkodid, decimal fiyat)
        {
            var sorgu = (from c in col.Urunlers
                         where c.BarkodId == barkodid && c.EnUcuzFiyat != fiyat
                         select c).FirstOrDefault();

            if (sorgu != null)
            {
                var silmeislemi = (from al in col.Paylasimlars
                                   where al.PaylasimId == paylasimid
                                   select al).FirstOrDefault();

                col.Paylasimlars.Remove(silmeislemi);
                col.SaveChanges();

                return true;
            }

            else
            {
                return false;
            }
        }
        public int PaylasimGuncelle(int paylasimid, decimal fiyat, DateTime tarih, int barkodid, decimal eskifiyat, int marketid)
        {
            decimal enucuzdb;
            decimal guncelfiyat = fiyat;

            List<Urunler> enucuzmu = (from c in col.Urunlers
                                      where c.BarkodId == barkodid && c.EnUcuzFiyat == eskifiyat && c.MarketId == marketid
                                      select c).ToList();
            Urunler enucuzmunesne = enucuzmu[0];
            if (enucuzmunesne != null)
            {
                return 3;
            }

            List<Urunler> sorgu = (from c in col.Urunlers
                                   where c.BarkodId == barkodid
                                   select c).ToList();


            Urunler urunveri = sorgu[0];
            enucuzdb = urunveri.EnUcuzFiyat;

            if (enucuzdb < guncelfiyat)
            {
                Paylasimlar veri = (from c in col.Paylasimlars
                                    where c.PaylasimId == paylasimid
                                    select c).FirstOrDefault();
                if (veri != null)
                {
                    veri.Fiyat = fiyat;
                    veri.Tarih = tarih;
                    col.SaveChanges();
                }

                return 2;
            }

            if (enucuzdb >= guncelfiyat)
            {

                Paylasimlar veri = (from c in col.Paylasimlars
                                    where c.PaylasimId == paylasimid
                                    select c).FirstOrDefault();
                if (veri != null)
                {
                    veri.Fiyat = fiyat;
                    veri.Tarih = tarih;
                    col.SaveChanges();
                }

                Urunler guncelleurun = (from s in col.Urunlers
                                        where s.BarkodId == barkodid
                                        select s).FirstOrDefault();

                if (guncelleurun != null)
                {
                    guncelleurun.EnUcuzFiyat = fiyat;
                    guncelleurun.MarketId = veri.MarketId;
                    col.SaveChanges();
                }

                return 1;

            }

            else
            {
                return 4;
            }
        }
        public bool UrunlerEnUcuzuGir(int barkod, decimal fiyat, int marketid)
        {
            bool deger = false;
            Urunler veri = (from c in col.Urunlers
                            where c.BarkodId == barkod
                            select c).FirstOrDefault();
            if (veri != null)
            {
                if (veri.EnUcuzFiyat > fiyat)
                {
                    veri.EnUcuzFiyat = fiyat;
                    veri.MarketId = marketid;
                    col.SaveChanges();
                    deger = true;
                }
            }
            return deger;
        }
        public void uyelikmail(string Email)
        {
            var veri = (from c in col.Kullanicilars
                        where c.Email == Email
                        select c.Sayi).FirstOrDefault();

            MailMessage msg = new MailMessage();//yeni bir mail nesnesi Oluşturuldu.
            msg.IsBodyHtml = true; //mail içeriğinde html etiketleri kullanılsın mı?

            msg.To.Add(Email);//Kime mail gönderilecek.
            msg.From = new MailAddress("hcrb_4321@hotmail.com", "izmitenucuzurun", System.Text.Encoding.UTF8);//mail kimden geliyor, hangi ifade görünsün?
            msg.Subject = "Üyelik Onay Maili";//mailin konusu
            msg.Body = "<a>Üyelik Onay Kodunuz = " + veri + " </a>";//mailin içeriği

            SmtpClient smp = new SmtpClient();
            smp.Credentials = new NetworkCredential("hcrb_4321@hotmail.com", "*******");//kullanıcı adı şifre
            smp.Port = 587;
            smp.Host = "smtp.live.com";//gmail üzerinden gönderiliyor.
            smp.EnableSsl = true;
            smp.Send(msg);//msg isimli mail gönderiliyor.
        }
        public bool uyelikonaylama(string email, string sayi)
        {
            var sorgu = (from c in col.Kullanicilars
                         where c.Email == email && c.Sayi == sayi
                         select c).FirstOrDefault();
            if (sorgu != null)
            {
                Kullanicilar veri = (from c in col.Kullanicilars
                                     where c.Email == email
                                     select c).FirstOrDefault();
                veri.Onay = true;
                col.SaveChanges();

                return true;
            }

            else
            {
                return false;
            }
        }
        public bool sifremiunuttum(string Email)
        {
            Kullanicilar sorgu = (from c in col.Kullanicilars
                                  where c.Email == Email
                                  select c).FirstOrDefault();
            if (sorgu != null)
            {
                string parola = sorgu.Parola;

                MailMessage msg = new MailMessage();//yeni bir mail nesnesi Oluşturuldu.
                msg.IsBodyHtml = true; //mail içeriğinde html etiketleri kullanılsın mı?

                msg.To.Add(Email);//Kime mail gönderilecek.
                msg.From = new MailAddress("izmitenucuzurun@gmail.com", "izmitenucuzurun", System.Text.Encoding.UTF8);//mail kimden geliyor, hangi ifade görünsün?
                msg.Subject = "Parola Hatırlatma Maili";//mailin konusu
                msg.Body = "<a>Parolanız = " + parola + " </a>";//mailin içeriği

                SmtpClient smp = new SmtpClient();
                smp.Credentials = new NetworkCredential("izmitenucuzurun@gmail.com", "*******");//kullanıcı adı şifre
                smp.Port = 587;
                smp.Host = "smtp.gmail.com";//gmail üzerinden gönderiliyor.
                smp.EnableSsl = true;
                smp.Send(msg);//msg isimli mail gönderiliyor.

                return true;
            }

            else
            {
                return false;
            }
        }
        public static string MD5Sifrele(string metin)
        {
            // MD5CryptoServiceProvider nesnenin yeni bir instance'sını oluşturalım.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            //Girilen veriyi bir byte dizisine dönüştürelim ve hash hesaplamasını yapalım.
            byte[] btr = Encoding.UTF8.GetBytes(metin);
            btr = md5.ComputeHash(btr);

            //byte'ları biriktirmek için yeni bir StringBuilder ve string oluşturalım.
            StringBuilder sb = new StringBuilder();


            //hash yapılmış her bir byte'ı dizi içinden alalım ve her birini hexadecimal string olarak formatlayalım.
            foreach (byte ba in btr)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            //hexadecimal(onaltılık) stringi geri döndürelim.
            return sb.ToString();
        }
        public List<string> PaylasimUrunTakipKayit(int barkodId, decimal ucret, int marketId, int kullaniciid)
        {
            List<string> liste = new List<string>();
            try
            {
                Paylasimlar paylasim = (from nesne in col.Paylasimlars
                                        where barkodId == nesne.BarkodId && marketId == nesne.MarketId
                                        select nesne).FirstOrDefault();

                if (paylasim != null) // daha önce aynı markette paylaşım yapılmış demek
                {
                    if (paylasim.Fiyat == ucret) // aynı markette aynı fiyat girildiyse
                    {
                        liste.Add("Daha Önce Bu Markette Aynı Fiyatla Ürün Paylaşımı Yapıldı");
                    }
                    else
                    {
                        paylasim.Fiyat = ucret;
                        col.SaveChanges();
                        liste.Add("Teşekkürler! Ürünün Bu Marketteki Fiyatını Güncellediniz.");

                        if (UrunlerEnUcuzuGir(paylasim.BarkodId, ucret, marketId))  // farklı fiyat ve en ucuz fiyatsa
                        {
                            liste.Add("Ve En Ucuz Ürün Fiyatını Girdiniz!");
                        }
                    }
                }

                else  // daha önce bu markette o ürünle ilgili paylaşım yok demek
                {
                    Paylasimlar kayit = new Paylasimlar
                    {
                        BarkodId = barkodId,
                        KullaniciId = kullaniciid,
                        MarketId = marketId,
                        Fiyat = ucret,
                        Tarih = DateTime.Now
                    };
                    col.Paylasimlars.Add(kayit);
                    col.SaveChanges();
                    liste.Add("Tebrikler! Ürünün Bu Marketteki İlk Kaydını Siz Yaptınız.");

                    if (UrunlerEnUcuzuGir(barkodId, ucret, marketId))  // paylaşım en ucuz ürün ise
                    {
                        liste.Add("Ve En Ucuz Ürün Fiyatını Girdiniz!");
                    }
                }
            }
            catch (Exception ex)
            {
                liste.Add(ex.Message);
            }

            return liste;
        }

        public string UploadImage(byte[] buffer, string blobName, string containerName)
        {

            string deger = "";

            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                // Retrieve a reference to a container 
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                // Create the container if it doesn't already exist
                container.CreateIfNotExists();
                // Retrieve reference to a blob named "myblob"
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
                // Create or overwrite the "myblob" blob with contents from a local file.
                blockBlob.UploadFromByteArray(buffer, 0, buffer.Length);
                deger = "Fotoğraf Yüklendi!";
            }
            catch (Exception ex)
            {
                deger = ex.Message;
            }
            return deger;
        }

        public byte[] DownloadImage(string blobName, string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

            byte[] dizi = null;
            using (var memoryStream = new MemoryStream())
            {
                blockBlob.DownloadToStream(memoryStream);
                dizi = memoryStream.ToArray();
            }
            return dizi;
        }
        public List<byte[]> TakipEdilenUrunImages(string containerName, List<string> blobName)
        {
            List<byte[]> diziListesi = new List<byte[]>();
            foreach (string item in blobName)
            {
                diziListesi.Add(DownloadImage(item, containerName));
            }
            return diziListesi;
        }
    }
}
