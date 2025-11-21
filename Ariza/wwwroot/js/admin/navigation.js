document.addEventListener("DOMContentLoaded", function () {
    console.log("Admin panel navigation.js yüklendi!");

    // Geçerli URL'yi al
    var currentUrl = window.location.pathname;

    // Tüm menü öğelerini seç
    var menuItems = document.querySelectorAll(".nav-link");

    menuItems.forEach(function (item) {
        // Eğer menü öğesinin href değeri, şu anki sayfa URL'sini içeriyorsa aktif yap
        if (item.href.includes(currentUrl)) {
            item.classList.add("active");
        }
    });
});
