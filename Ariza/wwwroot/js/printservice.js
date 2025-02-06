// Print service functionality
function createPrintTemplate(data) {
    return `
        <html>
        <head>
            <title>Arıza Formu</title>
            <link rel="stylesheet" href="/css/print-styles.css">
        </head>
        <body>
            <div class="print-header">
                <h4>SDÜ Bilgi İşlem Daire Başkanlığı Teknik Servis Arıza Formu</h4>
            </div>
            <div class="teslim-alan">(Teslim Alan: )</div>
            <table class="print-table">
                <tr>
                    <td class="print-label">Ad Soyad</td>
                    <td>${data.Ad} ${data.Soyad}</td>
                </tr>
                <tr>
                    <td class="print-label">Telefon / E-mail</td>
                    <td>${data.TelNo} / ${data.Email}</td>
                </tr>
                <tr>
                    <td class="print-label">Birimi</td>
                    <td>${data.Birim}</td>
                </tr>
                <tr>
                    <td class="print-label">Arıza</td>
                    <td>${data.Ariza}</td>
                </tr>
                <tr>
                    <td class="print-label">Açıklama</td>
                    <td>Yapılan İşlem</td>
                </tr>
                <tr>
                    <td class="print-label">Tarih</td>
                    <td>${data.Tarih}</td>
                </tr>
                <tr>
                    <td class="print-label">Teslim Alan</td>
                    <td style="border-right: none;"></td>
                    <td style="border-left: none; width: 100px;">İmza</td>
                </tr>
            </table>
        </body>
        </html>
    `;
}

function handlePrint(button) {
    const data = {
        Ad: button.dataset.ad,
        Soyad: button.dataset.soyad,
        TelNo: button.dataset.tel,
        Email: button.dataset.email,
        Birim: button.dataset.birim,
        Ariza: button.dataset.ariza,
        Tarih: button.dataset.tarih
    };

    const printWindow = window.open('', '_blank');
    printWindow.document.write(createPrintTemplate(data));

    printWindow.document.close();
    printWindow.focus();
    setTimeout(() => {
        printWindow.print();
        printWindow.close();
    }, 250);
}

document.addEventListener('DOMContentLoaded', function () {
    console.log("DOM Yüklendi, print-button event ekleniyor...");
    const printButtons = document.querySelectorAll('.print-button');
    printButtons.forEach(button => {
        button.addEventListener('click', () => {
            console.log("Yazdır butonuna tıklandı!");
            handlePrint(button);
        });
    });
});