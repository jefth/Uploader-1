﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using Common;

namespace UploadHandle
{
    /// <summary>
    /// 后台缩略图处理，
    /// 注：前台上传后，生成缩略图 大图、小图，适合原图上传模式
    /// </summary>
    public class ThumbnailHandle
    {
        /// <summary>
        /// 源文件目录
        /// </summary>
        public string OriginFolder = "origin";
        /// <summary>
        /// 大图文件夹
        /// </summary>
        public string Folder { get; set; }
        /// <summary>
        /// 大图宽度
        /// </summary>
        public int Width { get; set; } = 960;

        /// <summary>
        /// 文件信息
        /// </summary>
        private UploadInfo _Info = null;
        /// <summary>
        /// 源文件，绝对路径
        /// </summary>
        private string FullName { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info">上传文件信息</param>
        public ThumbnailHandle(UploadInfo info, string folder, int width)
        {
            this.Folder = folder;
            this._Info = info;
            this.FullName = info.GetFullName();
            if (string.IsNullOrEmpty(info.GetSubFolder()))
                throw new Exception("缩略图处理模式，上传子目录不能为空");
            this.Width = width;
        }

        /// <summary>
        /// 处理图片到文件夹
        /// </summary>
        public void AutoHandle()
        {
            Bitmap bitmap = ImgHelper.GetBitmapForGra(this.FullName);
            //保持图片的比例不变，缩放图片
            int width=this.Width,
                height=this.Width;
            if (bitmap.Width > bitmap.Height)
            {
                //宽度为大,计算高度
                height =Convert.ToInt32(width * (bitmap.Height * 1.0 / bitmap.Width));
            }
            else {
                //高度为大，计算宽度
                width = Convert.ToInt32(height * (bitmap.Width * 1.0 / bitmap.Height)); 
            }

            Bitmap result = ImgHelper.ResizeImage(bitmap, width, height);

            string bigpath = GetFullName();
            //保存图片
            result.Save(bigpath);
            result.Dispose();
            bitmap.Dispose();
        }
        /// <summary>
        /// 获取图片的相对地址
        /// </summary>
        /// <returns></returns>
        public string GetRelativeName()
        {
            return this._Info.GetRelativeName().Replace(this.OriginFolder, Folder);
        }
        /// <summary>
        /// 获取处理后的图片绝对路径
        /// </summary>
        /// <returns></returns>
        public string GetFullName()
        {
            string fullname = this.FullName.Replace(this.OriginFolder, Folder);
            string path = fullname.Replace(this._Info.NewName, "");
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            return fullname;
        }
    }
}
