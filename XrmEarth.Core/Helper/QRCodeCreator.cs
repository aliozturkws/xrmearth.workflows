using QRCoder;
using System;
using System.Drawing;
using System.IO;

namespace XrmEarth.Core
{
    public class QRCodeCreator
    {
        public static string Create(string text)
        {
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            var byteImage = BitmapToBytesCode(qrCodeImage);

            return Convert.ToBase64String(byteImage);
        }

        private static Byte[] BitmapToBytesCode(Bitmap image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
