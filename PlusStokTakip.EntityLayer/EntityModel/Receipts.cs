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
    
    public partial class Receipts
    {
        public int ReceiptID { get; set; }
        public int CustomerID { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime ReceiptDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Customers Customers { get; set; }
    }
}
