﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HoiTroWebsite.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HoiTroEntities : DbContext
    {
        public HoiTroEntities()
            : base("name=HoiTroEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Detail> Details { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
    }
}
