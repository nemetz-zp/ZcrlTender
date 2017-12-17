using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace TenderLibrary
{
    public class ZcrlTenderContext : DbContext
    {
        public ZcrlTenderContext(string connectionStringName) : base(connectionStringName) { }

        public DbSet<BalanceChanges> BalanceChanges { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<DkCode> DkCodes { get; set; }
        public DbSet<Estimate> Estimates { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<KekvCode> KekvCodes { get; set; }
        public DbSet<MoneySource> MoneySources { get; set; }
        public DbSet<PlannedSpending> PlannedSpending { get; set; }
        public DbSet<TenderPlanRecord> TenderPlanRecords { get; set; }
        public DbSet<TenderYear> TenderYears { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<ContractChange> ContractChanges { get; set; }
        public DbSet<TenderPlanRecordChange> TenderPlanRecordChanges { get; set; }
        public DbSet<ContactPerson> ContactPersons { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Настройки внешних ключей для TenderPlanChanges
            modelBuilder.Entity<TenderPlanRecord>()
                .HasRequired(p => p.PrimaryKekv)
                .WithMany()
                .HasForeignKey(p => p.PrimaryKekvId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<TenderPlanRecord>()
               .HasRequired(p => p.SecondaryKekv)
               .WithMany()
               .HasForeignKey(p => p.SecondaryKekvId)
               .WillCascadeOnDelete(false);

            // Настройки внешних ключей для BalanceChanges
            modelBuilder.Entity<BalanceChanges>()
               .HasRequired(p => p.PrimaryKekv)
               .WithMany()
               .HasForeignKey(p => p.PrimaryKekvId)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<BalanceChanges>()
               .HasRequired(p => p.SecondaryKekv)
               .WithMany()
               .HasForeignKey(p => p.SecondaryKekvId)
               .WillCascadeOnDelete(false);

            // Настройки внешних ключей для Contract
            modelBuilder.Entity<Contract>()
               .HasRequired(p => p.PrimaryKekv)
               .WithMany()
               .HasForeignKey(p => p.PrimaryKekvId)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<Contract>()
               .HasRequired(p => p.SecondaryKekv)
               .WithMany()
               .HasForeignKey(p => p.SecondaryKekvId)
               .WillCascadeOnDelete(false);

            // Настройки внешних ключей для PlannedSpending
            modelBuilder.Entity<PlannedSpending>()
               .HasRequired(p => p.PrimaryKekv)
               .WithMany()
               .HasForeignKey(p => p.PrimaryKekvId)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<PlannedSpending>()
               .HasRequired(p => p.SecondaryKekv)
               .WithMany()
               .HasForeignKey(p => p.SecondaryKekvId)
               .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
