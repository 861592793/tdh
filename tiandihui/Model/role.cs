//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class role
    {
        public role()
        {
            this.role_menu = new HashSet<role_menu>();
        }
    
        public string rol_id { get; set; }
        public string rol_name { get; set; }
        public string rol_description { get; set; }
        public Nullable<bool> rol_isAdmin { get; set; }
        public string rol_identity { get; set; }
        public Nullable<bool> rol_isDefault { get; set; }
    
        public virtual ICollection<role_menu> role_menu { get; set; }
    }
}
