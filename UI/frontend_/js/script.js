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
