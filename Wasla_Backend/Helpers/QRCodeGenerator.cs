
namespace Wasla_Backend.Helpers
{
    public static class QRHelper
    {
        public static Bitmap GenerateQR<T>(T data, int pixelsPerModule = 20)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            string text = data is string str ? str : JsonSerializer.Serialize(data);

            using var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

            using var qrCode = new QRCode(qrData);
            return qrCode.GetGraphic(pixelsPerModule);
        }

        public static IFormFile GenerateQRFile<T>(T data, int pixelsPerModule = 20, string fileName = "qrcode.png")
        {
            Bitmap bitmap = GenerateQR(data, pixelsPerModule);

            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Position = 0;

            IFormFile formFile = new FormFile(ms, 0, ms.Length, "qrcode", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"  
            };

            return formFile;
        }

    }

}
