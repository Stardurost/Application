//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRMTelmate.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClientService
    {
        public int IDcs { get; set; }
        public int IDClient { get; set; }
        public int IDService { get; set; }
        public System.DateTime StartTime { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual Service Service { get; set; }
    }
}
