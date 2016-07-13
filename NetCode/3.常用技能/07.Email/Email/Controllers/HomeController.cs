﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace Email.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 附件路径
        /// </summary>
        public static IList<string> filePathList = new List<string>();

        #region 文件上传
        /// <summary>
        /// LoTUploader-文件上传
        /// </summary>
        /// <returns></returns>
        public JsonResult FileUpload(System.Web.HttpPostedFileBase file)
        {
            if (file == null) { return Json(new { status = false, msg = "文件提交失败" }); }
            if (file.ContentLength > 10485760) { return Json(new { status = false, msg = "文件10M以内" }); }
            string filterStr = ".gif,.jpg,.jpeg,.bmp,.png|.rar,.7z,.zip";
            string fileExt = Path.GetExtension(file.FileName).ToLower();
            if (!filterStr.Contains(fileExt)) { return Json(new { status = false, msg = "请上传图片或压缩包" }); }
            //防止黑客恶意绕过，判断下文件头文件
            if (!file.InputStream.CheckingExt("7173", "255216", "6677", "13780", "8297", "55122", "8075"))
            {
                //todo：一次危险记录
                return Json(new { status = false, msg = "请上传图片或压缩包" });
            }
            //todo: md5判断一下文件是否已经上传过,如果已经上传直接返回 return Json(new { status = true, msg = sqlPath });

            string path = string.Format("{0}/{1}", "/lotFiles", DateTime.Now.ToString("yyyy-MM-dd"));
            string fileName = string.Format("{0}{1}", Guid.NewGuid().ToString("N"), fileExt);
            string sqlPath = string.Format("{0}/{1}", path, fileName);
            string dirPath = Request.MapPath(path);

            if (!Directory.Exists(dirPath)) { Directory.CreateDirectory(dirPath); }
            try
            {
                //todo：缩略图
                file.SaveAs(Path.Combine(dirPath, fileName));
                filePathList.Add(sqlPath); //todo: 未来写存数据库的Code
            }
            catch { return Json(new { status = false, msg = "文件保存失败" }); }
            return Json(new { status = true, msg = sqlPath });
        }
        #endregion

        public JsonResult SendMsg()
        {
            return Json("");
        }
    }
}