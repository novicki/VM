//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VendingAutomat.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserBalance
    {
        public int id { get; set; }
        public Nullable<int> cashOwnerId { get; set; }
        public Nullable<int> balance { get; set; }
    
        public virtual CashOwner CashOwner { get; set; }
    }
}
