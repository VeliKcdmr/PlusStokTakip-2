//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlusStokTakip.EntityLayer.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class StockMovements
    {
        public int MovementID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string MovementType { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<System.DateTime> MovementDate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Products Products { get; set; }
    }
}
