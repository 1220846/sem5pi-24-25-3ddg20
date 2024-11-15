using Microsoft.EntityFrameworkCore;
using DDDSample1.Domain.Categories;
using DDDSample1.Domain.Products;
using DDDSample1.Domain.Families;
using DDDSample1.Infrastructure.Categories;
using DDDSample1.Infrastructure.Products;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Infrastructure.OperationTypes;
using DDDSample1.Infrastructure.Specializations;
using DDDSample1.Domain.Specializations;
using DDDSample1.Infrastructure.OperationTypesSpecializations;
using DDDSample1.Domain.OperationTypesSpecializations;
using DDDSample1.Domain.Users;
using DDDSample1.Infrastructure.Users;
using dddnetcore.Domain.AvailabilitySlots;
using dddnetcore.Infraestructure.AvailabilitySlots;
using DDDSample1.Domain.Staffs;
using dddnetcore.Infraestructure.Staffs;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Infrastructure.OperationRequests;
using DDDSample1.Infrastructure.Patients;
using DDDSample1.Domain.Patients;
using DDDSample1.Domain.SystemLogs;
using DDDSample1.Domain.Appointments;
using dddnetcore.Infraestructure.Appointments;

namespace DDDSample1.Infrastructure
{
    public class DDDSample1DbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Family> Families { get; set; }

        public DbSet<OperationType> OperationTypes { get; set; }

        public DbSet<Specialization> Specializations { get; set; }

        public DbSet<User> Users {get;set;} 

        public DbSet<OperationTypeSpecialization> OperationTypesSpecializations { get; set; } 

        public DbSet<AvailabilitySlot> AvailabilitySlots {get; set;}

        public DbSet<Staff> Staffs {get; set;}

        public DbSet<Patient> Patients {get; set;}
        public DbSet<OperationRequest> OperationRequest { get; set; }

        public DbSet<AnonymizedPatientData> AnonymizedPatientsData { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<SystemLog> SystemLogs { get; set; }

        public DDDSample1DbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FamilyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperationTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SpecializationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperationTypeSpecializationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AvailabilitySlotEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StaffEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperationRequestEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PatientEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AppointmentEntityTypeConfiguration());
        }
    }
}