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
    
    public partial class BrandImprove
    {
        public string Id { get; set; }
        public string NaturalInfoId { get; set; }
        public string Src { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<System.DateTime> AddTime { get; set; }
    
        public virtual NaturalInfo NaturalInfo { get; set; }
    }
}
