# Arıza Takip

> ASP.NET Core MVC ile geliştirilmiş, kurum içi teknik destek ve arıza talebi yönetim uygulaması.

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC-512BD4)](https://learn.microsoft.com/aspnet/core/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

## Özellikler

- Güvenli cookie oturumu ve rol bazlı yetkilendirme
- Teknik destek talebi oluşturma ve kişisel talep geçmişini görüntüleme
- Yalnızca yönetici rolüne açık merkezi talep görünümü
- Sunucu taraflı model doğrulama ve CSRF koruması
- SQL Server + Entity Framework Core altyapısı
- Geliştirme ortamında isteğe bağlı yönetici hesabı oluşturma

## Teknoloji seti

- .NET 9 / ASP.NET Core MVC
- Entity Framework Core + SQL Server
- Bootstrap 5
- Cookie Authentication

> Proje şu an .NET 9’u hedefler. .NET 9 kısa destekli bir sürüm olduğundan, canlıya alma öncesinde .NET 10 LTS geçişi planlanmalıdır.

## Hızlı başlangıç

### 1. Gereksinimler

- .NET 9 SDK
- Erişilebilir bir SQL Server örneği

### 2. Gizli yapılandırma

Connection string veya parola içeren ayarları depoya eklemeyin. Geliştirme ortamında User Secrets kullanın:

```bash
dotnet user-secrets --project Ariza set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=ArizaDb;User Id=sa;Password=YOUR_STRONG_PASSWORD;TrustServerCertificate=True"
dotnet user-secrets --project Ariza set "BootstrapAdmin:Email" "admin@example.com"
dotnet user-secrets --project Ariza set "BootstrapAdmin:Password" "YOUR_STRONG_ADMIN_PASSWORD"
```

`BootstrapAdmin` değerleri yalnızca geliştirme ortamında, kullanıcı yoksa ilk yönetici hesabını oluşturmak için kullanılır. Bunları canlı ortamda kullanmayın.

### 3. Çalıştırma

```bash
dotnet restore Ariza.sln
dotnet tool restore
dotnet tool run dotnet-ef database update --project Ariza
dotnet run --project Ariza -- --seed
dotnet run --project Ariza
```

`--seed` komutu yalnızca geliştirme ortamında, temel rolleri ve User Secrets ile tanımlanmış ilk yönetici hesabını oluşturur. Üretimde migration’ları uygulama başlangıcından bağımsız, kontrollü bir dağıtım adımı olarak çalıştırın:

```bash
dotnet tool run dotnet-ef database update --project Ariza
```

> Daha önce migration takibi olmadan oluşturulmuş bir veritabanınız varsa, bu ilk migration’ı doğrudan uygulamayın. Önce yedek alın ve mevcut şemayı EF Core migration geçmişine güvenle baseline edecek ayrı bir geçiş planı uygulayın.

## Güvenlik notları

- Parolalar artık düz metin karşılaştırılmaz; ASP.NET Core `PasswordHasher` ile doğrulanır.
- Eski veritabanındaki düz metin parolalar güvenli bir hash’e dönüştürülmeden giriş yapamaz. Canlı sistemlerde kontrollü parola sıfırlama süreci uygulanmalıdır.
- Tüm durum değiştiren formlar anti-forgery token doğrulaması kullanır.
- Yönetim ekranı `Admin` rolü gerektirir; kullanıcılar yalnızca kendi taleplerini görüntüler.
- Üretimde connection string, bootstrap bilgileri ve Data Protection anahtarları güvenli bir secret manager’da tutulmalıdır.

## Proje yapısı

```text
Ariza/
├── Controllers/       # MVC uç noktaları ve yetkilendirme
├── Models/            # Varlıklar ve form view model’leri
├── Views/             # Razor arayüzleri
├── Data/              # Geliştirme ortamı başlangıç verisi
├── wwwroot/           # Statik dosyalar
└── Program.cs          # Servis, veritabanı ve oturum yapılandırması
```

## Yol haritası

- [ ] Talep durumunu yönetici tarafından güncelleme ve atama akışı
- [x] EF Core başlangıç migration’ı
- [ ] Birim bazlı filtreleme, bildirimler ve raporlama
- [ ] Entegrasyon testleri ve .NET 10 LTS geçişi

## Lisans

Bu proje [MIT License](LICENSE) ile lisanslanmıştır.
