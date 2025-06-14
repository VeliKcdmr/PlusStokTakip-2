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
    
    public partial class BankAccounts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BankAccounts()
        {
            this.BankTransactions = new HashSet<BankTransactions>();
        }
    
        public int AccountID { get; set; }
        public string BankName { get; set; }
        public string Iban { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BankTransactions> BankTransactions { get; set; }
    }
}
