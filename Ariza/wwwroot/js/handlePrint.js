export function handlePrint(data) {
    console.log("Yazdırılacak veri:", data); // Debugging için

    const printWindow = window.open('', '_blank', 'width=800,height=600');
    if (!printWindow) {
        alert('Lütfen pop-up engelleyiciyi devre dışı bırakın.');
        return;
    }

    const printContent = `
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset="utf-8">
            <title>Teknik Servis Talep Formu</title>
            <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
            <style>
                body {
                    font-family: system-ui, -apple-system, sans-serif;
                    padding: 40px;
                    background: #fff;
                }
                .print-container {
                    max-width: 800px;
                    margin: 0 auto;
                    padding: 30px;
                    background: #fff;
                    border: 1px solid #ddd;
                    border-radius: 8px;
                }
                .header {
                    text-align: center;
                    margin-bottom: 30px;
                    padding-bottom: 20px;
                    border-bottom: 2px solid #eee;
                }
                .header h1 {
                    font-size: 24px;
                    color: #333;
                    margin: 0;
                }
                .content {
                    margin-top: 20px;
                }
                .info-row {
                    display: flex;
                    justify-content: space-between;
                    margin-bottom: 15px;
                    padding-bottom: 15px;
                    border-bottom: 1px solid #eee;
                }
                .info-label {
                    font-weight: 600;
                    color: #555;
                }
                .info-value {
                    color: #333;
                }
                @media print {
                    body { padding: 0; }
                    .print-container { border: none; padding: 0; }
                }
            </style>
        </head>
        <body>
            <div class="print-container">
                <div class="header">
                    <h1>Teknik Servis Talep Formu</h1>
                    <p>Süleyman Demirel Üniversitesi</p>
                </div>
                <div class="content">
                    <div class="info-row"><span class="info-label">Ad Soyad:</span><span class="info-value">${data.ad} ${data.soyad}</span></div>
                    <div class="info-row"><span class="info-label">Telefon:</span><span class="info-value">${data.tel}</span></div>
                    <div class="info-row"><span class="info-label">E-posta:</span><span class="info-value">${data.email}</span></div>
                    <div class="info-row"><span class="info-label">Birim:</span><span class="info-value">${data.birim}</span></div>
                    <div class="info-row"><span class="info-label">Arıza Açıklaması:</span><span class="info-value">${data.ariza}</span></div>
                    <div class="info-row"><span class="info-label">Talep Tarihi:</span><span class="info-value">${data.tarih}</span></div>
                </div>
            </div>
            <script>
                window.onload = function() {
                    setTimeout(() => {
                        window.print();
                        window.close();
                    }, 500);
                };
            </script>
        </body>
        </html>
    `;

    printWindow.document.write(printContent);
    printWindow.document.close();

    window.handlePrint = handlePrint;

}
