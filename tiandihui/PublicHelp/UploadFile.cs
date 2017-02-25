using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PublicHelp
{
    public class UploadFile
    {
        private static readonly string[] excleExten = { ".xls", ".xlsx" };
        private static readonly string[] imageExten = { ".jpg", ".png", ".jpeg" };


        /// <summary>
        /// 返回文件名，包含扩展名
        /// </summary>
        /// <param name="saveUrl"></param>
        /// <param name="filetype"></param>
        /// <param name="file"></param>
        /// <param name="filename">不带扩展名</param>
        /// <returns></returns>
        public static string saveFile(string saveUrl, fileType filetype, HttpPostedFileBase file, string filename = "")
        {
            if (file != null)
            {

                try
                {
                    string extenName = Path.GetExtension(file.FileName).ToLower();

                    if (checkFile(file, extenName, filetype)) //如果上传的文件合法
                    {
                        if (string.IsNullOrEmpty(filename))
                        {
                            filename = Guid.NewGuid().ToString();
                        }
                        file.SaveAs(saveUrl + filename + extenName);
                        return filename + extenName;
                    }
                    else
                    {
                        return "-1";
                    }
                }
                catch
                {
                    return "-1";
                }
            }
            return "-2";

        }




        public enum fileType
        {

            excel,
            image


        }

        /// <summary>
        /// 判断上传的文件是否合法
        /// </summary>
        /// <param name="filetype"></param>
        /// <returns></returns>
        public static bool checkFile(HttpPostedFileBase file, string filesExten, fileType filetype)
        {
            if (file.ContentLength > 0)
            {
                return checkExten(filesExten, filetype);

            }
            return false;
        }

        public static bool checkFile(HttpPostedFileBase file, fileType filetype)
        {
            string extenName = Path.GetExtension(file.FileName);
            if (file.ContentLength > 0)
            {
                return checkExten(extenName, filetype);
            }
            return false;

        }


        /// <summary>
        /// 判断文件扩展名是否正确
        /// </summary>
        /// <param name="filesExten"></param>
        /// <param name="filetype"></param>
        /// <returns></returns>
        private static bool checkExten(string filesExten, fileType filetype)
        {

            if (filetype == fileType.excel)
            {
                return excleExten.Contains(filesExten);

            }
            else if (filetype == fileType.image)
            {
                return imageExten.Contains(filesExten);
            }
            return false;


        }

    }
}
