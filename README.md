# Staj Projesi: Basit Mesajlasma Uygulamasi

Bu proje, staj kapsaminda gelistirilen katmanli mimariye sahip bir **ASP.NET Core Web API** ve bu servise terminalden baglanan **Console Client (Terminal Istemcisi)** uygulamasidir.

Uygulama, verilerin kalici olarak PostgreSQL veritabaninda saklandigi, istemci tarafinda polling (periyodik sorgulama) yontemiyle canli iletisimin saglandigi ve yerel agda (LAN) coklu istemci destegi sunan bir mesajlasma sistemidir.

---

## Genel Mimari ve Teknolojiler

- **Backend:** C#, .NET, ASP.NET Core Web API, RESTful Mimari, Swagger / OpenAPI
- **ORM & Veritabani:** Entity Framework Core (Code-First), PostgreSQL
- **IoC & Bagimlilik Yonetimi:** Autofac Dependency Injection
- **Tasarim Desenleri:** Generic Repository Deseni, Result Deseni (IResult / IDataResult), DTO Deseni
- **Istemci:** .NET Console / Terminal Client (Asenkron HttpClient ve Arka Plan Polling)

---

## 1. Backend (Web API) Calistirma Talimati

Backend servisi, veritabani baglantilarini yoneten ve istemcilere RESTful API saglayan ana sunucu katmanidir.

### Gereksinimler:
- .NET SDK (versiyon 8 veya uzeri)
- PostgreSQL Veritabani Sunucusu (Varsayilan port: 5432)

### Calistirma Adimlari:
1. PostgreSQL servisinin calistigindan ve `MessagingAppDb` veritabaninin erisilebilir oldugundan emin olun.
2. Terminalden proje ana dizinindeyken su komutu calistirin:
   ```bash
   dotnet run --project MessagingApp.Api
   ```
   *(Veya Visual Studio icerisinde `MessagingApp.Api` projesini baslangic projesi olarak secip F5 tusuna basin.)*
3. Web API varsayilan olarak `http://localhost:5000` portunda baslayacaktir.
4. Swagger test arayuzune tarayicinizdan `http://localhost:5000/swagger` adresinden ulasabilirsiniz.

---

## 2. Console Client (Terminal Istemcisi) Calistirma Talimati

Terminal istemcisi, kullanicilarin sohbet odalari acmasina, odalari listelemesine ve canli olarak mesajlasmasina olanak tanir.

### Calistirma Adimlari:
1. Web API calisir durumdayken, yeni bir terminal penceresi acin ve su komutu calistirin:
   ```bash
   dotnet run --project MessagingApp.Client
   ```
   *(Veya Visual Studio icerisinde `MessagingApp.Client` projesini calistirin.)*
2. Uygulama acildiginda kullanici adinizi girin (Maksimum 50 karakter).
3. Baglanilacak sunucu adresini girin:
   - Ayni bilgisayarda test ediyorsaniz: Enter tusuna basarak varsayilan adresi (`http://localhost:5000`) kabul edin.
   - Yerel agdaki baska bir bilgisayardan baglaniyorsaniz: API'nin calistigi bilgisayarin yerel IP adresini girin (Ornegin: `http://192.168.1.35:5000`).
4. Ana menuden istediginiz islemi secin:
   - `[1]` Sohbet Odalarini Listele & Odaya Gir
   - `[2]` Yeni Sohbet Odasi Ac
   - `[3]` Cikis (`:cikis`)

---

## 3. Coklu Kullanici ve Ag (LAN) Uzerinde Test

Iki farkli terminal penceresi (veya ayni yerel agdaki iki farkli bilgisayar) acilarak farkli kullanici adlariyla ayni sohbet odasina girildiginde; polling mekanizmasi sayesinde gonderilen mesajlar karsi tarafin terminalinde anlik olarak goruntulenecektir.
