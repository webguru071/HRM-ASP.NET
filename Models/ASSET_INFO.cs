//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EMSApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ASSET_INFO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ASSET_INFO()
        {
            this.EQUEPMENTS_INFO = new HashSet<EQUEPMENTS_INFO>();
        }
    
        public long ASSET_ID { get; set; }
        public string ASSET_TITLE { get; set; }
        public string REMARKS { get; set; }
        public long ACTION_BY { get; set; }
        public System.DateTime ACTION_DATE { get; set; }
        public Nullable<long> UPDATE_BY { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EQUEPMENTS_INFO> EQUEPMENTS_INFO { get; set; }
    }
}
