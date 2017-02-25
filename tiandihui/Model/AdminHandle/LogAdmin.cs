using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PublicHelp;
namespace Model.AdminHandle
{
    public class LogAdmin
    {
        run rn = new run();
        LogsHandlerEntities dbContext = new LogsHandlerEntities();
        private string sql = string.Empty;
        
        
        //添加日志
        public PublicResult.pb_execute Add(AdminLog item, HttpRequestMessage httprq)
        {
            
            
            PublicResult.pb_execute rs = new PublicResult.pb_execute();
            var id = rn.GetId<AdminLog>();
            if (id == null)
            {
                rs.success = false;
                rs.result = "id注册失败，请重新注册";
                return rs;
            }
            else
            {
                sql = @"insert into AdminLog(Id,Users,LastIp,Time,Act) values(@Id,@Users,@LastIp,@Time,@Act)";
                var args = new DbParameter[] {
                                    new SqlParameter { ParameterName = "Id", Value = id},
                                    new SqlParameter { ParameterName = "Users", Value=item.Users},
                                    new SqlParameter { ParameterName = "LastIp", Value=Assistant.GetClientIp(httprq)},
                                    new SqlParameter { ParameterName = "Time", Value=DateTime.Now},
                                    new SqlParameter { ParameterName = "Act", Value=item.Act}
                                };
                return rn.exac(sql, args);
            }
        }

        //删除日志
        public PublicResult.pb_execute Del(AdminLog item)
        {
            sql = @"delete from AdminLog where Id=@Id";
            var args = new DbParameter[] {
                                    new SqlParameter { ParameterName = "Id", Value = item.Id}
                                };
            return rn.exac(sql, args);
        }
    }
}
