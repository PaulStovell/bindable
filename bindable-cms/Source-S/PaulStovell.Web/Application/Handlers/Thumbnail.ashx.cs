using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using System.Drawing.Imaging;
using PaulStovell.Web.Application.Helpers;

namespace PaulStovell.Web.Application.Handlers
{
    /// <summary>
    /// Generates thumbnail images when an image is requested. Not the fastest thing in the world, but does the job.
    /// </summary>
    public class Thumbnail : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var virtualFileName = QueryString.GetRequired<string>("Image");
            var thumbnailWidth = QueryString.GetOptional<int>("Width", 300);
            var thumbnailHeight = QueryString.GetOptional<int>("Height", 200);
            var physicalName = context.Server.MapPath(virtualFileName);
            var bytes = File.ReadAllBytes(physicalName);
            var hash = Convert.ToBase64String(MD5.Create().ComputeHash(bytes));
            var thumbnailPath = string.Format("/Resources/Images/Thumbnails/{0}-{1}-{2}x{3}.png", Path.GetFileName(virtualFileName), hash, thumbnailWidth, thumbnailHeight);
            var thumbnailFileName = context.Server.MapPath(thumbnailPath);
            if (!File.Exists(thumbnailFileName))
            {
                var bitmap = new Bitmap(physicalName);
                var result = CreateThumbnail(bitmap, thumbnailWidth, thumbnailHeight, true);
                result.Save(thumbnailFileName, ImageFormat.Jpeg);
            }
            context.Response.ContentType = "image/png";
            context.Response.TransmitFile(thumbnailFileName);
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public static Bitmap CreateThumbnail(Bitmap source, int thumbnameWidth, int thumbnailHeight, bool maintainAspect)
        {
            // Return the source image if it's smaller than the designated thumbnail
            if (source.Width < thumbnameWidth && source.Height < thumbnailHeight) return source;

            try
            {
                var with = thumbnameWidth;
                var height = thumbnailHeight;

                if (maintainAspect)
                {
                    // Maintain the aspect ratio despite the thumbnail size parameters
                    if (source.Width > source.Height)
                    {
                        with = thumbnameWidth;
                        height = (int)(source.Height * ((decimal)thumbnameWidth / source.Width));
                    }
                    else
                    {
                        height = thumbnailHeight;
                        with = (int)(source.Width * ((decimal)thumbnailHeight / source.Height));
                    }
                }

                var result = new Bitmap(with, height);
                using (var g = Graphics.FromImage(result))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.FillRectangle(Brushes.White, 0, 0, with, height);
                    g.DrawImage(source, 0, 0, with, height);
                }
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}