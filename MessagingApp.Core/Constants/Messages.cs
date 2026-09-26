namespace MessagingApp.Core.Constants
{

    public static class Messages
    {

        public static readonly string ConversationTitleEmpty = "Sohbet başlığı boş bırakılamaz.";
        public static readonly string ConversationTitleTooLong = "Sohbet başlığı 100 karakterden uzun olamaz.";
        public static readonly string ConversationCreated = "Sohbet başarıyla oluşturuldu.";
        public static readonly string ConversationsListed = "Sohbetler başarıyla listelendi.";
        public static readonly string ConversationDetailListed = "Sohbet detayı başarıyla getirildi.";
        public static readonly string ConversationNotFound = "Belirtilen ID'ye sahip sohbet bulunamadı.";
        public static readonly string ConversationForMessageNotFound = "Mesaj gönderilecek sohbet bulunamadı.";

        public static readonly string MessageRequestEmpty = "Mesaj isteği boş olamaz.";
        public static readonly string SenderEmpty = "Gönderen bilgisi boş bırakılamaz.";
        public static readonly string SenderTooLong = "Gönderen ismi 50 karakterden uzun olamaz.";
        public static readonly string ContentEmpty = "Mesaj içeriği boş bırakılamaz.";
        public static readonly string ContentTooLong = "Mesaj içeriği 500 karakterden uzun olamaz.";
        public static readonly string MessageSent = "Mesaj başarıyla gönderildi.";
        public static readonly string MessagesListed = "Mesajlar başarıyla listelendi.";

        public static readonly string ServerConnectionError = "Sunucuya bağlanılamadı. Lütfen API'nin çalıştığından ve doğru adreste olduğundan emin olun.";
        public static readonly string ServerTimeout = "Sunucu yanıt vermedi. Lütfen ağ bağlantınızı kontrol edin.";
        public static readonly string EmptyResponse = "Sunucudan boş yanıt döndü.";
        public static readonly string EmptyMessageList = "Sunucudan boş mesaj listesi döndü.";
        public static readonly string ServerConnectionLost = "Sunucu bağlantısı kesildi.";
        public static readonly string MessageSendConnectionError = "Mesaj gönderilemedi: Sunucuya ulaşılamıyor.";

        public static readonly string NotFound404 = "İstenen kayıt veya sohbet bulunamadı.";
        public static readonly string BadRequest400 = "Gönderilen bilgiler geçersiz veya eksik.";
        public static readonly string InternalServerError500 = "Sunucuda içsel bir hata oluştu.";
        public static readonly string UnexpectedError = "Sunucuda beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
    }
}
