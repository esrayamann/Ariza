window.handlePrint = function (button) {
    console.log("Yazdır butonuna basıldı!");

    const ad = button.getAttribute('data-ad') || "";
    const soyad = button.getAttribute('data-soyad') || "";
    const tel = button.getAttribute('data-tel') || "";
    const email = button.getAttribute('data-email') || "";
    const birim = button.getAttribute('data-birim') || "";
    const ariza = button.getAttribute('data-ariza') || "";
    const YapilanIslem = button.getAttribute('data-YapilanIslem') || "";
    const tarih = button.getAttribute('data-tarih') || "";

    const printContent = `
        <div class="container">
            <table>
                <td colspan="2" style="border: 1px solid black; padding: 2mm; text-align: center; font-weight: bold; font-family: 'Arial', sans-serif; font-size: 14pt;">
        SDÜ Bilgi İşlem Daire Başkanlığı Teknik Servis Arıza Formu
                </td>
               <tr>
                    <th style="width: 20%;">Ad Soyad</th>
                    <td style="width: 80%;">${ad} ${soyad}</td>
                </tr>
                <tr>
                    <th style="width: 20%;">Telefon / E-mail</th>  
                    <td style="width: 80%;">${tel} / ${email}</td>
                </tr>
                <tr>
                    <th style="width: 20%;">Birimi</th>  
                    <td style="width: 80%;">${birim}</td>
                </tr>
                <tr>
                    <th style="width: 20%;">Arıza</th>  
                    <td class="ariza" style="width: 80%;">${ariza}</td>
                </tr>
                <tr>
                    <th style="width: 20%;">Yapılan İşlem</th>  
                    <td class="yapilan-islem" style="width: 80%;">${YapilanIslem}</td>
                </tr>
                <tr>
                    <td colspan="2" style="border: 1px solid black; padding: 2mm; text-align: center; background-color: white;">
                        İşlem Sonrası
                    </td>
                </tr>
                <tr>

                    <td colspan="2">
                       <table style="width: 100%; border: none;">
                            <tr>
                                <td style="width: 20%; padding: 2mm;">
                                    <div style="font-weight: bold;">Tarih</div>
                                </td>
                                <td style="width: 40%; padding: 2mm;">
                                    <div style="font-weight: bold;">Teslim Alan</div>
                                    <div>&nbsp;</div>
                                </td>
                                <td style="width: 40%; padding: 2mm;">
                                    <div style="font-weight: bold;">İmza</div>
                                    <div>&nbsp;</div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>${tarih}</div>
                                </td>
                                <td>  </td>
                                <td>  </td>
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
                <title>Yazdır</title>
                <style>
                    @page {
                        size: A5 landscape;
                        margin: 5mm;
                    }
                    body, html {
                        margin: 0;
                        padding: 0;
                        width: 100%;
                        height: 100%;
                    }
                    .container {
                        width: 100%;
                        height: 100%;
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        font-family: Arial, sans-serif;
                    }
                    table {
                        width: 95%;
                        height: 95%;
                        border-collapse: collapse;
                        table-layout: auto;
                        margin: auto;
                    }
                    th, td {
                        border: 1px solid black;
                        padding: 8px;
                        text-align: left;
                        word-break: break-word;
                        white-space: normal;
                    }                    
                    .ariza, .yapilan-islem {
                        max-height: none;
                        overflow-y: visible;
                    }
                </style>
            </head>
            <body>
                ${printContent}
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
        printWindow.focus();
    } catch (error) {
        console.error("Yazdırma hatası:", error);
    }
};
