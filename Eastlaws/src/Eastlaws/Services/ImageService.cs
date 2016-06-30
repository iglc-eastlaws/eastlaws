using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eastlaws.Entities;
using Eastlaws.Infrastructure;
using Eastlaws;
using Dapper;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Eastlaws.Services
{
    public class ImageService
    {
        public static string ClearFolder = "C:\\Images\\";
        public static string ConvertedFolder = "C:\\ConvertedImages\\";

        public static IEnumerable<Images> GetImages(int ServiceID)
        {
            SqlConnection con = DataHelpers.GetConnection(DbConnections.Data);
            var Data = con.Query<Images>("GetServiceImages", new { ServiceTypeID = 1 , ServiceID = ServiceID }, null, true, null, System.Data.CommandType.StoredProcedure);
            return Data;
        }


        public static string ConvertImage(byte RecType, int MasterID, int ImageID, string OldPath)
        {
            OldPath = OldPath.Substring(3);
            OldPath = ClearFolder + OldPath;

            FileInfo fi = new FileInfo(OldPath);
           
            string newPath = GetImagePath(RecType, MasterID, ImageID);

            var res = CopyRight(OldPath, newPath, RecType, MasterID, ImageID);
            if (res) return newPath;

            return "";
        }

        public static string testPath()
        {
            string ConvertedFoldertest = "C:\\ImgConverted\\";
            return ConvertedFoldertest + "1_1_269402" + ".gif";
        }

        public static string GetImagePath(byte RecType, int MasterID, int ImageID)
        {
            return ConvertedFolder + RecType + "_" + MasterID + "_" + ImageID + ".gif";
        }

        public static bool CopyRight(string oldPath, string NewPath, byte RecType, int MasterID, int ImageID)
        {
            //Image imgLogo = Image.FromFile(HostingEnvironment.MapPath("~/CopyRight.gif"));
            //int lWidth = imgLogo.Width, lHeight = imgLogo.Height;

            //Image imgOriginal = Image.FromFile(oldPath);
            //int w = imgOriginal.Width, h = imgOriginal.Height;
            //Size newSize = getScaledSize(new Size(w, h));
            //w = newSize.Width;
            //h = newSize.Height;


            //Bitmap bitMap = new Bitmap(w, h + 80, PixelFormat.Format16bppRgb555);
            //Graphics g = Graphics.FromImage(bitMap);

            //g.DrawImage(imgOriginal, new Rectangle(0, 80, w, h));


            //Image imgWaterMrk = Image.FromFile(HostingEnvironment.MapPath("~/Watermark.png"));
            //Size wtrMarkSize = imgWaterMrk.Size;
            //if (wtrMarkSize.Width > w || wtrMarkSize.Height > h)
            //    wtrMarkSize = getScaledSize(imgWaterMrk.Size, newSize);

            //int x1 = (w - wtrMarkSize.Width) / 2;
            //int y1 = ((h - wtrMarkSize.Height) / 2) + 80;

            //g.DrawImage(imgWaterMrk, x1, y1, wtrMarkSize.Width, wtrMarkSize.Height);


            //g.FillRectangle(Brushes.White, 0, 0, w, 80);
            //g.DrawRectangle(Pens.Black, 0, 0, w - 2, 80);
            //if (lWidth <= w)
            //{
            //    int x;
            //    x = (w / 2) - (lWidth / 2);
            //    x -= 50;
            //    g.DrawImage(imgLogo, x, 10);
            //}

            //bitMap.Save(NewPath, ImageFormat.Gif);
            //bitMap.Dispose();
            //bitMap = null;

            //DAL.ExecuteSQL(string.Format("Update ConvertedImages Set Width={0} , Height={1} Where RecType={2} And MasterID={3} and ID={4}", w, h, RecType, MasterID, ImageID));
            return true;
        }

        private static Size getScaledSize(Size big, Size small)
        {
            decimal ratio = Math.Max(Decimal.Divide(big.Width, small.Width), Decimal.Divide(big.Height, small.Height));
            int nWidth = Convert.ToInt32(Decimal.Divide(big.Width, ratio));
            int nheight = Convert.ToInt32(Decimal.Divide(big.Height, ratio));
            return new Size(nWidth, nheight);

        }
    }
}
