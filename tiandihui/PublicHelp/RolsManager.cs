using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicHelp
{

    public enum RolsType
    { 
        bsnsAdmin //商家管理员
    }

    public class RolsTypeManager
    {
        public static string getRolId(RolsType rolType)
        {
            switch (rolType)
            {
                case RolsType.bsnsAdmin: return "20170209054417441eb99e859d4bf38eb3f0b961f64a6b";

                default: return "";
            }
        }

    }
}
