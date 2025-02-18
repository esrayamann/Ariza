window.handlePrint = function (button) {
    console.log("Yazdır butonuna basıldı!");

    const ad = button.getAttribute('data-ad');
    const soyad = button.getAttribute('data-soyad');
    const tel = button.getAttribute('data-tel');
    const email = button.getAttribute('data-email');
    const birim = button.getAttribute('data-birim');
    const ariza = button.getAttribute('data-ariza');
    const YapilanIslem = button.getAttribute('data-YapilanIslem');
    const tarih = button.getAttribute('data-tarih');

    const printContent = `
        <div style="width: 210mm; height: 148mm; margin: 0 auto; font-family: Arial, sans-serif; padding: 5mm;">
            <table style="width: 100%; border-collapse: collapse; font-size: 11pt;">
                <tr>
                    <td colspan="2" style="border: 1px solid black; padding: 2mm; text-align: center; font-weight: bold; font-family: 'Arial', sans-serif; font-size: 14pt;">
                        SDÜ Bilgi İşlem Daire Başkanlığı Teknik Servis Arıza Formu
                    </td>
                </tr>
                <tr>
                    <th style="border: 1px solid black; padding: 2mm; text-align: left;">Ad Soyad</th>
                    <td style="border: 1px solid black; padding: 2mm;">${ad} ${soyad}</td>
                </tr>
                <tr>
                    <th style="border: 1px solid black; padding: 2mm; text-align: left;">Telefon / E-mail</th>
                    <td style="border: 1px solid black; padding: 2mm;">${tel} / ${email}</td>
                </tr>
                <tr>
                    <th style="border: 1px solid black; padding: 2mm; text-align: left;">Birimi</th>
                    <td style="border: 1px solid black; padding: 2mm;">${birim}</td>
                </tr>
                <tr>
                    <th style="border: 1px solid black; padding: 2mm; text-align: left;">Arıza</th>
                    <td style="border: 1px solid black; padding: 2mm;">${ariza}</td>
                </tr>
                <tr>
                    <th style="border: 1px solid black; padding: 2mm; text-align: left;">Yapılan İşlem</th>
                    <td style="border: 1px solid black; padding: 2mm;">${YapilanIslem}</td>
                </tr>
                <tr>
                    <td colspan="2" style="border: 1px solid black; padding: 2mm; text-align: center;">
                        İşlem Sonrası
                    </td>
                </tr>
                <tr style="height: 15mm;">
                    <td colspan="2" style="border: 1px solid black; padding: 0;">
                        <table style="width: 100%; height: 100%;">
                            <tr>
                                <td style="width: 33.33%; border-right: 1px solid black; padding: 2mm; font-weight: bold;">Tarih</td>
                                <td style="width: 33.33%; border-right: 1px solid black; padding: 2mm; font-weight: bold;">Teslim Alan</td>
                                <td style="width: 33.33%; padding: 2mm; font-weight: bold;">İmza</td>
                            </tr>
                            <tr>
                                <td style="padding: 2mm;">${tarih}</td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    `;

    try {
        const printWindow = window.open('', '_blank');
        if (!printWindow) {
            alert("Pop-up engelleyicisini devre dışı bırakın.");
            return;
        }
        printWindow.document.write(`
            <html>
            <head>
                <title>Talep Yazdır</title>
                <style>
                    @page {
                        size: A5 landscape; /* A5 boyutu ve yatay mod */
                        margin: 10mm;
                    }
                    body {
                        margin: 0;
                        padding: 0;
                    }
                    .container {
                        width: 210mm;
                        height: 148mm;
                        margin: 0 auto; /* Sayfanın ortasında düzgün durmasını sağlar */
                    }
                    table { width: 100%; border-collapse: collapse; }
                    th, td { border: 1px solid black; padding: 2mm; text-align: left; }
                    th { background-color: white; }
                </style>
            </head>
            <body>
                <div class="container">
                    ${printContent}
                </div>
                <script>
                    window.onload = function() {
                        window.print();
                        window.close();
                    };
                </script>
            </body>
            </html>
        `);
        printWindow.document.close();
    } catch (error) {
        console.error("Yazdırma hatası:", error);
    }
};
