using System;
using PixGenerator;
using QRCoder;

class Program
{
    static void Main(string[] args)
    {
        string key = "gabrielmelobatista@hotmail.com";
        string receiverName = "Gabriel";
        decimal amount = 50.00m;

        var pixGenerator = new PixGenerator(key, receiverName);
        string pixCode = pixGenerator.GeneratePixCode(amount);

        using (var qrGenerator = new QRCodeGenerator())
        {
            var qrCodeData = qrGenerator.CreateQrCode(pixCode, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);

            using (var qrImage = qrCode.GetGraphic(20))
            {
                qrImage.Save("pixCode.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        Console.WriteLine("QR Code gerado com sucesso!");
    }
}
