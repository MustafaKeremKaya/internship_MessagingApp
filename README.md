# Staj Projesi: Basit Mesajlaşma Uygulaması

Bu proje, staj kapsamında geliştirilen katmanlı mimariye sahip bir **ASP.NET Core Web API** ve bu servise terminalden bağlanan **Console Client** uygulamasıdır.

Uygulama, verilerin kalıcı olarak PostgreSQL veritabanında saklandığı, istemci tarafında polling yöntemiyle canlı iletişimin sağlandığı ve yerel ağda çoklu istemci desteği sunan bir mesajlaşma sistemidir.

---

## Genel Mimari ve Teknolojiler

- **Backend:** C#, .NET, ASP.NET Core Web API, RESTful Mimari, Swagger / OpenAPI
- **ORM ve Veritabanı:** Entity Framework Core (Code-First), PostgreSQL
- **IoC ve Bağımlılık Yönetimi:** Autofac Dependency Injection
- **Tasarım Desenleri:** Generic Repository Deseni, Result Deseni (IResult / IDataResult), DTO Deseni
- **İstemci:** .NET Console / Terminal Client (Asenkron HttpClient ve Arka Plan Polling)

---

## 1. Backend (Web API) Çalıştırma Talimatı

Backend servisi, veritabanı bağlantılarını yöneten ve istemcilere RESTful API sağlayan ana sunucu katmanıdır.

### Gereksinimler:
- .NET SDK (versiyon 8 veya üzeri)
- PostgreSQL Veritabanı Sunucusu (Varsayılan port: 5432)

### Çalıştırma Adımları:
1. PostgreSQL servisinin çalıştığından ve `MessagingAppDb` veritabanının erişilebilir olduğundan emin olun.
2. Terminalden proje ana dizinindeyken şu komutu çalıştırın:
   ```bash
   dotnet run --project MessagingApp.Api
   ```
   *(Veya Visual Studio içerisinde `MessagingApp.Api` projesini başlangıç projesi olarak seçip F5 tuşuna basın.)*
3. Web API varsayılan olarak `http://localhost:5000` portunda başlayacaktır.
4. Swagger test arayüzüne tarayıcınızdan `http://localhost:5000/swagger` adresinden ulaşabilirsiniz.

---

## 2. Console Client (Terminal İstemcisi) Çalıştırma Talimatı

Terminal istemcisi, kullanıcıların sohbet odaları açmasına, odaları listelemesine ve canlı olarak mesajlaşmasına olanak tanır.

### Çalıştırma Adımları:
1. Web API çalışır durumdayken, yeni bir terminal penceresi açın ve şu komutu çalıştırın:
   ```bash
   dotnet run --project MessagingApp.Client
   ```
   *(Veya Visual Studio içerisinde `MessagingApp.Client` projesini çalıştırın.)*
2. Uygulama açıldığında kullanıcı adınızı girin (Maksimum 50 karakter).
3. Bağlanılacak sunucu adresini girin:
   - Aynı bilgisayarda test ediyorsanız: Enter tuşuna basarak varsayılan adresi (`http://localhost:5000`) kabul edin.
   - Yerel ağdaki başka bir bilgisayardan bağlanıyorsanız: API'nin çalıştığı bilgisayarın yerel IP adresini girin (Örneğin: `http://192.168.1.35:5000`).
4. Ana menüden istediğiniz işlemi seçin:
   - `[1]` Sohbet Odalarını Listele ve Odaya Gir
   - `[2]` Yeni Sohbet Odası Aç
   - `[3]` Çıkış (`:çıkış`)

---

## 3. Çoklu Kullanıcı ve Ağ (LAN) Üzerinde Test

İki farklı terminal penceresi (veya aynı yerel ağdaki iki farklı bilgisayar) açılarak farklı kullanıcı adlarıyla aynı sohbet odasına girildiğinde; polling mekanizması sayesinde gönderilen mesajlar karşı tarafın terminalinde anlık olarak görüntülenecektir.
