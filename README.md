# .NET 7 ve React ile oluşturulmuş web tabanlı bir görev yönetimi uygulaması. Bu uygulama, kullanıcıların görevleri yönetmesine, bunları diğer kullanıcılara atamasına ve ilerlemeyi verimli bir şekilde izlemesine olanak tanır.

# Proje Açıklaması
Görev Yönetimi Uygulaması, kullanıcıların görevlerini oluşturabilecekleri, düzenleyebilecekleri ve takip edebilecekleri bir platformdur. Uygulama, yöneticilerin görev atayabileceği ve kullanıcıların görev durumlarını güncelleyebileceği basit bir arayüz sunar. Bu proje .NET 7 backend ve React frontend ile geliştirildi.

# Özellikler
- Kullanıcılar görevlerini oluşturabilir, düzenleyebilir ve silebilir.
- Kullanıcılar görevlerinin durumunu güncelleyebilir (tamamlandı, devam ediyor vb.).
- JWT tabanlı güvenli giriş işlemleri.
- RESTful API ile frontend ve backend arasında haberleşme.

  # Teknolojiler
- **Backend**: .NET 7 (ASP.NET Core)
- **Frontend**: React.js
- **Veritabanı**: SQL Server
- **Loglama** : SeriLog
- **Kimlik Doğrulama**: JWT (JSON Web Token) ve Identity Kontrolü
- **Veritabanı Yönetimi**: Entity Framework Core
- **API**: RESTful API

## Kurulum
Proje, iki ayrı klasörde çalışan frontend ve backend ile yapılandırılmıştır. Aşağıdaki adımları takip ederek projeyi bilgisayarınızda çalıştırabilirsiniz.

### Backend (API)
1. **Backend dizinine gidin**:
    ```bash
    cd TaskManagementApp.API
    ```
2. **NuGet paketlerini yükleyin**:
    ```bash
    dotnet restore
    ```
3. **Veritabanı migrasyonlarını uygulayın**:
    ```bash
    dotnet ef database update
    ```
4. **API'yi çalıştırın**:
    ```bash
    dotnet run
    ```
API, `http://localhost:5000` adresinde çalışmaya başlayacaktır.

### Frontend (React)
1. **Frontend dizinine gidin**:
    ```bash
    cd TaskManagementApp.UI
    ```
2. **Gerekli npm paketlerini yükleyin**:
    ```bash
    npm install
    ```
3. **React uygulamasını çalıştırın**:
    ```bash
    npm start
    ```
Frontend uygulaması, `http://localhost:3000` adresinde çalışmaya başlayacaktır.
