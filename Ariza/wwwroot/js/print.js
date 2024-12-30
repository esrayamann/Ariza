function printRow(button) {
    const row = button.closest('tr');
    const talepData = {
        Ad: row.cells[0].textContent.split(' ')[0],
        Soyad: row.cells[0].textContent.split(' ')[1],
        TelNo: row.cells[1].textContent,
        Birim: row.cells[2].textContent,
        Ariza: row.cells[3].textContent,
        CreatedDate: row.cells[4].textContent
    };

    // Create print window
    const printWindow = window.open('', '_blank');
    printWindow.document.write(`
        <html>
        <head>
            <title>Arıza Formu</title>
            <style>
                @media print {
                    body { font-family: Arial, sans-serif; }
                    .print-header { margin-bottom: 20px; }
                    .print-table {
                        width: 100%;
                        border-collapse: collapse;
                    }
                    .print-table td {
                        border: 1px solid #000;
                        padding: 8px;
                    }
                    .print-label {
                        font-weight: bold;
                        width: 150px;
                    }
                }
            </style>
        </head>
        <body>
            <div class="print-header">
                <h4 style="text-align: center; margin-bottom: 15px;">SDÜ Bilgi İşlem Daire Başkanlığı Teknik Servis Arıza Formu</h4>
                <div style="text-align: right; margin-bottom: 10px;">(Teslim Alan: )</div>
            </div>
            <table class="print-table">
                <tr>
                    <td class="print-label">Ad Soyad</td>
                    <td>${talepData.Ad} ${talepData.Soyad}</td>
                </tr>
                <tr>
                    <td class="print-label">Telefon / E-mail</td>
                    <td>${talepData.TelNo}</td>
                </tr>
                <tr>
                    <td class="print-label">Birimi</td>
                    <td>${talepData.Birim}</td>
                </tr>
                <tr>
                    <td class="print-label">Arıza</td>
                    <td>${talepData.Ariza}</td>
                </tr>
                <tr>
                    <td class="print-label">Açıklama</td>
                    <td>Yapılan İşlem</td>
                </tr>
                <tr>
                    <td class="print-label">Tarih</td>
                    <td>${talepData.CreatedDate}</td>
                </tr>
                <tr>
                    <td class="print-label">Teslim Alan</td>
                    <td colspan="2" style="border-right: none;"></td>
                    <td style="border-left: none; width: 100px;">İmza</td>
                </tr>
            </table>
        </body>
        </html>
    `);

    printWindow.document.close();
    printWindow.focus();
    setTimeout(() => {
        printWindow.print();
        printWindow.close();
    }, 250);
}