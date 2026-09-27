# MessagingApp.Api - Backend Servisi Çalıştırma Talimatı

Bu proje, staj projesi kapsamında geliştirilen mesajlaşma uygulamasının veritabanı bağlantılarını yöneten ve istemcilere RESTful uç noktalar sunan ASP.NET Core Web API katmanıdır.

## Gereksinimler
- .NET SDK (8 veya üzeri)
- PostgreSQL Veritabanı Sunucusu (Varsayılan port: 5432)

## Çalıştırma Adımları
1. PostgreSQL sunucusunun çalıştığından ve `MessagingAppDb` veritabanının erişilebilir olduğundan emin olun.
2. Terminali bu klasörde açarak şu komutu çalıştırın:
   ```bash
   dotnet run
   ```
3. Servis varsayılan olarak `http://localhost:5000` adresinde dinlemeye başlayacaktır.
4. Swagger test arayüzüne tarayıcınızdan erişmek için: `http://localhost:5000/swagger`
