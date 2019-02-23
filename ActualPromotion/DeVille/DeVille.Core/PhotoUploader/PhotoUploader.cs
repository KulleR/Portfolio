using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Core.PhotoUploader
{
    public class PhotoUploader
    {
        /// <summary>
        /// Выполняет сохранение файла в директорию проекта.</summary>
        /// <param name="file">Файл, который необходимо сохранить</param>
        /// <param name="savePath">Относительный путь</param>
        /// <returns>Отображенный объект назначения</returns>
        /*public Upload(HttpPostedFileBase file, string savePath){
            var fileName = Path.GetFileName(ImgCover.FileName);
                    var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                    ImgCover.SaveAs(savingPath);
                    return VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));
        }*/
    }
}
