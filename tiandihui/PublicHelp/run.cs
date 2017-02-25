using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ServiceModel.Channels;
using System.Net.Http;
using System.Web;
using Model;
namespace PublicHelp
{
    public class run
    {

        //#region 执行添加、修改、删除的sql，返回模型
        ///// <summary>
        ///// 执行添加、修改、删除的sql，返回模型
        ///// </summary>
        ///// <param name="sql">SQL语句</param>
        ///// <param name="args">参数</param>
        ///// <returns>PublicResult.pb_execute</returns>
        //public PublicResult.pb_execute exac(string sql, DbParameter[] args)
        //{
        //    TiDiHuiEntities dbContext = new TiDiHuiEntities();
        //    PublicResult.pb_execute rs = new PublicResult.pb_execute();
        //    int rowCount = 0;
        //    rowCount = dbContext.Database.ExecuteSqlCommand(sql, args);
        //    if (rowCount == 0)
        //    {
        //        rs.success = false;
        //        rs.result = "操作失败";
        //    }
        //    else
        //    {
        //        rs.success = true;
        //        rs.result = "操作成功";
        //    }
        //    return rs;
        //}
        //#endregion

        //#region 生成图形验证码
        ///// <summary>
        ///// 生成图形验证码
        ///// </summary>
        ///// <param name="checkCode">验证码</param>
        ///// <returns>byte[]</returns>
        //public byte[] CreateCheckCodeImage(string checkCode)
        //{
        //    byte[] data = null;
        //    if (checkCode == null || checkCode.Trim() == String.Empty)
        //        return data;

        //    System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 13.5)), 28);
        //    Graphics g = Graphics.FromImage(image);
        //    try
        //    {
        //        //生成随机生成器
        //        Random random = new Random();

        //        //清空图片背景色
        //        g.Clear(Color.White);

        //        //画图片的背景噪音线
        //        for (int i = 0; i < 2; i++)
        //        {
        //            int x1 = random.Next(image.Width);
        //            int x2 = random.Next(image.Width);
        //            int y1 = random.Next(image.Height);
        //            int y2 = random.Next(image.Height);

        //            g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
        //        }

        //        Font font = new System.Drawing.Font("Arial", 14, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
        //        System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
        //        g.DrawString(checkCode, font, brush, 2, 2);

        //        //画图片的前景噪音点
        //        for (int i = 0; i < 50; i++)
        //        {
        //            int x = random.Next(image.Width);
        //            int y = random.Next(image.Height);

        //            image.SetPixel(x, y, Color.FromArgb(random.Next()));
        //        }

        //        //画图片的边框线
        //        g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

        //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //        image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        //        ms.Position = 0;
        //        data = new byte[ms.Length];
        //        ms.Read(data, 0, Convert.ToInt32(ms.Length));
        //        ms.Flush();
        //        //Response.ClearContent();
        //        //Response.ContentType = "image/Gif";
        //        //Response.BinaryWrite(ms.ToArray());
        //    }
        //    finally
        //    {
        //        g.Dispose();
        //        image.Dispose();
        //    }
        //    return data;
        //}
        //#endregion

        //#region 分页查询 + 条件查询 + 排序
        ///// <summary>  
        ///// 分页查询 + 条件查询 + 排序  
        ///// </summary>  
        ///// <typeparam name="Tkey">泛型</typeparam>  
        ///// <param name="pageSize">每页大小</param>  
        ///// <param name="pageIndex">当前页码</param>  
        ///// <param name="total">总数量</param>  
        ///// <param name="where">查询条件</param>  
        ///// <param name="order">排序条件</param>  
        ///// <param name="isAsc">是否升序</param>  
        ///// <returns>IQueryable 泛型集合</returns> 
        //public List<dynamic> getPageDateW<T, TKey>(Expression<Func<T, dynamic>> select, Expression<Func<T, bool>> where, Expression<Func<T, TKey>> order, int pageIndex, bool isAsc, int pageSize, out int Total)
        //    where T : class
        //{
        //    if (isAsc)
        //    {
        //        TiDiHuiEntities db = new TiDiHuiEntities();
        //        Total = db.Set<T>().AsNoTracking().Where(where).Count();
        //        var list = db.Set<T>().AsNoTracking().Where(where).OrderBy(order).Select(select).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //        return list.ToList();
        //    }
        //    else
        //    {
        //        TiDiHuiEntities db = new TiDiHuiEntities();
        //        Total = db.Set<T>().AsNoTracking().Where(where).Count();
        //        var list = db.Set<T>().AsNoTracking().Where(where).OrderByDescending(order).Select(select).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //        return list.ToList();
        //    }
        //}
        //#endregion

        //#region 分页查询 + 排序
        ///// <summary>  
        ///// 分页查询 + 排序  
        ///// </summary>  
        ///// <typeparam name="Tkey">泛型</typeparam>  
        ///// <param name="pageSize">每页大小</param>  
        ///// <param name="pageIndex">当前页码</param>  
        ///// <param name="total">总数量</param>  
        ///// <param name="order">排序条件</param>  
        ///// <param name="isAsc">是否升序</param>  
        ///// <returns>IQueryable 泛型集合</returns> 
        //public List<dynamic> getPageDate<T, TKey>(Expression<Func<T, dynamic>> select, Expression<Func<T, TKey>> order, int pageIndex, bool isAsc, int pageSize, out int Total)
        //    where T : class
        //{
        //    if (isAsc)
        //    {
        //        TiDiHuiEntities db = new TiDiHuiEntities();
        //        Total = db.Set<T>().AsNoTracking().Count();
        //        var list = db.Set<T>().AsNoTracking().OrderBy(order).Select(select).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //        return list.ToList();
        //    }
        //    else
        //    {
        //        TiDiHuiEntities db = new TiDiHuiEntities();
        //        Total = db.Set<T>().AsNoTracking().Count();
        //        var list = db.Set<T>().AsNoTracking().OrderByDescending(order).Select(select).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //        return list.ToList();
        //    }
        //}
        //#endregion

        //#region 获取客户端IP
        ///// <summary>
        ///// 获取客户端IP
        ///// </summary>
        ///// <param name="request">request</param>
        ///// <returns>ip</returns>
        //public string GetClientIp(HttpRequestMessage request)
        //{
        //    if (request.Properties.ContainsKey("MS_HttpContext"))
        //    {
        //        return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
        //    }
        //    else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
        //    {
        //        RemoteEndpointMessageProperty prop;
        //        prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
        //        return prop.Address;
        //    }
        //    else if (HttpContext.Current != null)
        //    {
        //        return HttpContext.Current.Request.UserHostAddress;
        //    }  
        //    else
        //    {
        //        return null;
        //    }
        //}
        //#endregion

        //#region 获取id
        ///// <summary>  
        ///// 获取id 
        ///// </summary>  
        ///// <typeparam name="T">泛型</typeparam>   
        ///// <returns>id</returns> 
        ////public string GetId<T>()
        ////    where T:class
        ////{
        ////    TiDiHuiEntities db = new TiDiHuiEntities();
        ////    var id = DateTime.Now.ToString("yyyyMMddhhmmss") + Guid.NewGuid().ToString("N");
        ////    var isRepeat = db.Set<T>().Find(id);
        ////    var isSuccess = true;
        ////    if (isRepeat != null) // id已经存在
        ////    {
        ////        id = DateTime.Now.ToString("yyyyMMddhhmmss") + Guid.NewGuid().ToString("N");
        ////        var i = 0;
        ////        while(db.Set<T>().Find(id) != null) //id存在
        ////        {
        ////            if (i == 5)
        ////            {
        ////                isSuccess = false;
        ////                break;
        ////            }
        ////            id = DateTime.Now.ToString("yyyyMMddhhmmss") + Guid.NewGuid().ToString("N");
        ////            i++;
        ////        }
        ////        if (isSuccess)
        ////        {
        ////            return id;
        ////        }
        ////        else
        ////        {
        ////            return null;
        ////        }
        ////    }
        ////    else
        ////    {
        ////        return id;
        ////    }
        ////}
        //#endregion
    }
}
