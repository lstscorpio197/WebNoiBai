//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebNoiBai.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SRolePermission
    {
        public int Id { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> PermissionId { get; set; }
        public Nullable<int> IsGranted { get; set; }
    
        public virtual SRole SRole { get; set; }
        public virtual SUser SUser { get; set; }
        public virtual SPermission SPermission { get; set; }
    }
}
