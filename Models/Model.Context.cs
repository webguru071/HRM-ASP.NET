﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EMSEntities : DbContext
    {
        public EMSEntities()
            : base("name=EMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ATTENDANCE_DETAILS> ATTENDANCE_DETAILS { get; set; }
        public virtual DbSet<DEPARTMENT_INFO> DEPARTMENT_INFO { get; set; }
        public virtual DbSet<DIVISION_INFO> DIVISION_INFO { get; set; }
        public virtual DbSet<EMPLOYEE_INFO> EMPLOYEE_INFO { get; set; }
        public virtual DbSet<NOTICE_BOARD> NOTICE_BOARD { get; set; }
        public virtual DbSet<SALARY_INFO> SALARY_INFO { get; set; }
        public virtual DbSet<TRANSACTION_ITEM> TRANSACTION_ITEM { get; set; }
        public virtual DbSet<USER_INFO> USER_INFO { get; set; }
        public virtual DbSet<LEAVE_TYPE> LEAVE_TYPE { get; set; }
        public virtual DbSet<LEAVE_APPLICATION> LEAVE_APPLICATION { get; set; }
        public virtual DbSet<TRANSACTION_SHEET> TRANSACTION_SHEET { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TEAM_INFO> TEAM_INFO { get; set; }
        public virtual DbSet<TEAM_DETAILS> TEAM_DETAILS { get; set; }
        public virtual DbSet<EMPLOYEE_APPLICATION> EMPLOYEE_APPLICATION { get; set; }
        public virtual DbSet<POSITIONAL_INFO> POSITIONAL_INFO { get; set; }
        public virtual DbSet<SALARY_GRADE> SALARY_GRADE { get; set; }
    }
}
