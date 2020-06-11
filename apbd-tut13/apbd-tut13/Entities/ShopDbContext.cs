using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_tut13.Entities
{
    public partial class ShopDbContext : DbContext
    {
        public ShopDbContext()
        {
        }

        public ShopDbContext(DbContextOptions<ShopDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Confectionery> Confectionery { get; set; }
        public virtual DbSet<ConfectioneryOrder> ConfectioneryOrder { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Order> Order { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Confectionery>(entity =>
            {
                entity.HasKey(e => e.IdConfectionery)
                    .HasName("Confectionery_pk");

                entity.Property(e => e.IdConfectionery)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PricePerItem)
                    .IsRequired()
                    .HasColumnType("real");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<ConfectioneryOrder>(entity =>
            {
                entity.HasKey(e => new { e.IdConfectionery, e.IdOrder })
                    .HasName("Confectionery_Order_pk");

                entity.Property(e => e.IdOrder).ValueGeneratedNever();

                entity.Property(e => e.IdConfectionery).ValueGeneratedNever();

                entity.ToTable("Confectionery_Order");

                entity.Property(e => e.Quantity)
                    .IsRequired();

                entity.Property(e => e.Notes)
                    .HasMaxLength(255);

                entity.HasOne(e => e.IdConfectioneryNavigation)
                    .WithMany(c => c.ConfectioneryOrder)
                    .HasForeignKey(e => e.IdConfectionery)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Confectionery_Order_Confectionery");

                entity.HasOne(e => e.IdOrderNavigation)
                    .WithMany(o => o.ConfectioneryOrder)
                    .HasForeignKey(e => e.IdOrder)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Confectionery_Order_Order");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.IdOrder)
                    .HasName("Order_pk");

                entity.Property(e => e.IdOrder).ValueGeneratedNever();

                entity.Property(e => e.DateAccepted).HasColumnType("date");

                entity.Property(e => e.DateFinished).HasColumnType("date");

                entity.Property(e => e.Notes)
                    .HasMaxLength(255);

                entity.HasOne(e => e.IdCustomerNavigation)
                    .WithMany(c => c.Order)
                    .HasForeignKey(e => e.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Order_Customer");

                entity.HasOne(e => e.IdEmployeeNavigation)
                    .WithMany(emp => emp.Order)
                    .HasForeignKey(e => e.IdEmployee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Order_Employee");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.IdClient)
                    .HasName("Customer_pk");

                entity.Property(e => e.IdClient)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.IdEmployee)
                    .HasName("Employee_pk");

                entity.Property(e => e.IdEmployee)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(60);
            });

            var cutomerSeed = new List<Customer>();
            cutomerSeed.Add(new Customer { IdClient = 1, Name = "CName1", Surname = "CSurame1" });
            cutomerSeed.Add(new Customer { IdClient = 2, Name = "CName2", Surname = "CSurame2" });
            cutomerSeed.Add(new Customer { IdClient = 3, Name = "CName3", Surname = "CSurame3" });
            modelBuilder.Entity<Customer>().HasData(cutomerSeed);

            var employeeSeed = new List<Employee>();
            employeeSeed.Add(new Employee { IdEmployee = 1, Name = "EName1", Surname = "ESurame1" });
            employeeSeed.Add(new Employee { IdEmployee = 2, Name = "EName2", Surname = "ESurame1" });
            employeeSeed.Add(new Employee { IdEmployee = 3, Name = "EName3", Surname = "ESurame1" });
            modelBuilder.Entity<Employee>().HasData(employeeSeed);

            var confectionerySeed = new List<Confectionery>();
            confectionerySeed.Add(new Confectionery { IdConfectionery = 1, Name = "ConName1", PricePerItem = 10, Type = "ConType1" });
            confectionerySeed.Add(new Confectionery { IdConfectionery = 2, Name = "ConName2", PricePerItem = 20, Type = "ConType2" });
            confectionerySeed.Add(new Confectionery { IdConfectionery = 3, Name = "ConName3", PricePerItem = 30, Type = "ConType3" });
            modelBuilder.Entity<Confectionery>().HasData(confectionerySeed);

            var orderSeed = new List<Order>();
            orderSeed.Add(new Order { IdOrder = 1, DateAccepted = DateTime.Now.AddMonths(-1), DateFinished = DateTime.Now.AddDays(-1), Notes = "OrderNotes1", IdClient = 1, IdEmployee = 1 });
            orderSeed.Add(new Order { IdOrder = 2, DateAccepted = DateTime.Now.AddMonths(-2), DateFinished = DateTime.Now.AddDays(-2), Notes = "OrderNotes2", IdClient = 2, IdEmployee = 2 });
            orderSeed.Add(new Order { IdOrder = 3, DateAccepted = DateTime.Now.AddMonths(-3), DateFinished = DateTime.Now.AddDays(-3), Notes = "OrderNotes3", IdClient = 3, IdEmployee = 3 });
            modelBuilder.Entity<Order>().HasData(orderSeed);

            var confectionery_order = new List<ConfectioneryOrder>();
            confectionery_order.Add(new ConfectioneryOrder { IdConfectionery = 1, IdOrder = 1, Quantity = 1, Notes = "ConOrderNotes1" });
            confectionery_order.Add(new ConfectioneryOrder { IdConfectionery = 2, IdOrder = 2, Quantity = 2, Notes = "ConOrderNotes2" });
            confectionery_order.Add(new ConfectioneryOrder { IdConfectionery = 3, IdOrder = 3, Quantity = 3, Notes = "ConOrderNotes3" });
            modelBuilder.Entity<ConfectioneryOrder>().HasData(confectionery_order);

            // In Package Manager Console: 
            //      "add-migration <Name of migration>
            //      "script-migration" to get the SQL code which will be sent to the db
            //      "update-database"
        }
    }
}
