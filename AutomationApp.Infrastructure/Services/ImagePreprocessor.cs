using System.Drawing;
using System.IO;
using AutomationApp.Domain.Interfaces;

namespace AutomationApp.Infrastructure.Services
{
    public class ImagePreprocessor : IImagePreprocessor
    {
        public byte[] Preprocess(byte[] imageData)
        {
            // Pour l'instant, retourner l'image inchangée
            return imageData;
        }
        
        // Version alternative qui retourne Bitmap pour usage interne
        private Bitmap ByteArrayToBitmap(byte[] imageData)
        {
            using var ms = new MemoryStream(imageData);
            return new Bitmap(ms);
        }
        
        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
    }
}