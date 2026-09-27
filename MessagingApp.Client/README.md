# MessagingApp.Client - Terminal İstemcisi Çalıştırma Talimatı

Bu proje, staj projesi kapsamında kullanıcıların terminal üzerinden Web API ile haberleşerek sohbet odaları açmasını, odaları listelemesini ve polling yöntemiyle canlı mesajlaşmasını sağlayan konsol istemcisidir.

## Çalıştırma Adımları
1. Öncelikle `MessagingApp.Api` projesinin çalışır durumda olduğundan emin olun.
2. Terminali bu klasörde açarak şu komutu çalıştırın:
   ```bash
   dotnet run
   ```
3. Uygulama açıldığında bir kullanıcı adı girin.
4. Bağlanılacak sunucu adresi olarak varsayılan `http://localhost:5000` adresini (veya yerel ağdaysanız sunucu bilgisayarın yerel IP adresini) girin.
5. Ana menü üzerinden odaları listeleyebilir, yeni oda açabilir veya odalara giriş yaparak mesajlaşabilirsiniz. Odadan çıkmak için `:çıkış` komutunu kullanabilirsiniz.
