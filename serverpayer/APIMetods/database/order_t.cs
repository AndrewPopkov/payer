//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace APIMetods.database
{
    using System;
    using System.Collections.Generic;
    
    public partial class order_t
    {
        public int order_id { get; set; }
        public int consumer_id { get; set; }
        public int vendor_id { get; set; }
        public decimal amount_kop { get; set; }
        public int status_id { get; set; }
    }
}
