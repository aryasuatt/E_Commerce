document.getElementById("login-btn").addEventListener("click", function(event) {
    event.preventDefault(); // Formun hemen gönderilmesini engelle

    const button = event.target;
    
    // Buton üzerinde kısa bir yüklenme animasyonu
    button.innerHTML = "Yükleniyor...";
    button.disabled = true;

    // 2 saniye sonra animasyonu sıfırla ve formu göndermeye devam et
    setTimeout(() => {
        button.innerHTML = "Giriş Yap";
        button.disabled = false;
        document.getElementById("login-form").submit();
    }, 2000);
});

// Kullanıcı girişi
async function login(username, password) {
    const response = await fetch('http://localhost:5500/api/auth/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ username, password })
    });

    if (response.ok) {
        const data = await response.json();
        localStorage.setItem('token', data.token);
        window.location.href = '/homepage'; // Ana sayfaya yönlendir
    } else {
        const errorMessage = await response.text();
        console.error('Giriş başarısız:', errorMessage);
    }
}

// Ürünleri alma
async function fetchProducts() {
    const token = localStorage.getItem('token');
    const response = await fetch('http://localhost:5002/api/products', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    if (response.ok) {
        const products = await response.json();
        console.log('Ürünler:', products);
        // Ürünleri sayfada göster
    } else {
        console.error('Yetkisiz erişim');
    }
}

// Belirli bir olay için fonksiyonu çağırma
document.getElementById('login-button').addEventListener('click', function () {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
    login(username, password);
});

// Ürünleri yüklemek için bir fonksiyon çağır
document.addEventListener('DOMContentLoaded', fetchProducts); // Sayfa yüklendiğinde ürünleri al