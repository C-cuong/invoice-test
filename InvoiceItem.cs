//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace invoice_test
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvoiceItem
    {
        public int ItemID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
    
        public virtual Invoice Invoice { get; set; }
    }
}
