// Giriş butonuna tıklama olayını dinle
document.getElementById("login-btn").addEventListener("click", function (event) {
    event.preventDefault(); // Formun hemen gönderilmesini engelle

    const button = event.target;

    // Buton üzerinde kısa bir yüklenme animasyonu
    button.innerHTML = "Yükleniyor...";
    button.disabled = true;

    // Kullanıcı adı ve şifreyi al
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    // Giriş yapmaya çalış
    login(username, password).then(() => {
        // Giriş başarılıysa 2 saniye sonra animasyonu sıfırla ve formu gönder
        setTimeout(() => {
            button.innerHTML = "Giriş Yap";
            button.disabled = false;
        }, 2000);
    });
});

// Kullanıcı girişi fonksiyonu
async function login(username, password) {
    try {
        const response = await fetch('https://localhost:5003/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem('token', data.token); // Token'ı sakla
            window.location.href = 'https://localhost:7160/Home/Index'; // Admin Kontrol sayfasına yönlendir
        } else {
            const errorMessage = await response.text();
            document.getElementById('error-message').innerText = 'Giriş bilgileri hatalı: ' + errorMessage;
        }
    } catch (error) {
        console.error('Giriş işlemi sırasında hata oluştu:', error);
        document.getElementById('error-message').innerText = 'Bir hata oluştu. Lütfen tekrar deneyin.';
    }
}

// Ürünleri alma fonksiyonu
async function fetchProducts() {
    const token = localStorage.getItem('token');
    if (!token) {
        console.error('Token bulunamadı.');
        return;
    }

    try {
        const response = await fetch('http://localhost:5002/api/products', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            const products = await response.json();
            console.log('Ürünler:', products);
            // Ürünleri sayfada gösterme işlemi burada yapılabilir
        } else {
            console.error('Yetkisiz erişim');
        }
    } catch (error) {
        console.error('Ürünler alınırken hata oluştu:', error);
    }
}

// Ürünleri sayfa yüklendiğinde almak için
document.addEventListener('DOMContentLoaded', fetchProducts);