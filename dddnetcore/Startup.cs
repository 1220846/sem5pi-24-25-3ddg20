using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DDDSample1.Infrastructure;
using DDDSample1.Infrastructure.Categories;
using DDDSample1.Infrastructure.Products;
using DDDSample1.Infrastructure.Families;
using DDDSample1.Infrastructure.Shared;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Categories;
using DDDSample1.Domain.Products;
using DDDSample1.Domain.Families;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Infrastructure.OperationTypes;
using DDDSample1.Domain.Specializations;
using DDDSample1.Infrastructure.Specializations;
using DDDSample1.Domain.OperationTypesSpecializations;
using DDDSample1.Infrastructure.OperationTypesSpecializations;
using DDDSample1.Domain.Users;
using DDDSample1.Infrastructure.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DDDSample1.Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using System;
using DotNetEnv;
using dddnetcore.Domain.Staffs;
using DDDSample1.DataAnnotations.Staffs;
using dddnetcore.Infraestructure.Staffs;
using dddnetcore.Domain.AvailabilitySlots;
using dddnetcore.Infraestructure.AvailabilitySlots;
using DDDSample1.DataAnnotations.Patients;
using DDDSample1.Infrastructure.Patients;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Infrastructure.OperationRequests;
using DDDSample1.Domain.Patients;
using dddnetcore.Domain.Patients;
using DDDSample1.Domain.Emails;
using DDDSample1.Infrastructure.Emails;

namespace DDDSample1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Env.Load();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            /*services.AddDbContext<DDDSample1DbContext>(opt =>
                opt.UseInMemoryDatabase("DDDSample1DB")
                .ReplaceService<IValueConverterSelector, StronglyEntityIdValueConverterSelector>());*/


            // Add database in mysql
            var databaseName = Environment.GetEnvironmentVariable("Database_Name");
            var databaseUser = Environment.GetEnvironmentVariable("Database_User");
            var databasePassword = Environment.GetEnvironmentVariable("Database_Password");
            var databaseHost = Environment.GetEnvironmentVariable("Database_HostName");
            var databasePort = Environment.GetEnvironmentVariable("Database_Port");

            var connection = $"server={databaseHost};port={databasePort};database={databaseName};user={databaseUser};password={databasePassword};";

            services.AddDbContext<DDDSample1DbContext>(opt => opt.UseMySql(connection,
                    new MySqlServerVersion(new Version(8, 0, 21))).ReplaceService<IValueConverterSelector, StronglyEntityIdValueConverterSelector>());
            ConfigureMyServices(services);
            
            //Add
            var Domain = Environment.GetEnvironmentVariable("Auth0_Domain");
            var Audience = Environment.GetEnvironmentVariable("Auth0_Audience");
            var Namespace_Roles = Environment.GetEnvironmentVariable("Auth0_Namespace_Roles");

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => { options.Authority = $"https://{Domain}/";

            options.Audience = Audience;
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true};});

            services.AddAuthorization(options =>{
                options.AddPolicy("read:messages", policy =>
                    policy.Requirements.Add(new HasScopeRequirement("read:messages", $"https://{Domain}/")));
                options.AddPolicy("RequiredBackofficeRole",policy => policy.RequireClaim($"{Namespace_Roles}/roles","Admin","Doctor","Nurse","Technician"));
                options.AddPolicy("RequiredAdminRole",policy => policy.RequireClaim($"{Namespace_Roles}/roles","Admin"));
                options.AddPolicy("RequiredDoctorRole",policy => policy.RequireClaim($"{Namespace_Roles}/roles","Doctor"));
                options.AddPolicy("RequiredNurseRole",policy => policy.RequireClaim($"{Namespace_Roles}/roles","Nurse"));
                options.AddPolicy("RequiredTechnicianRole",policy => policy.RequireClaim($"{Namespace_Roles}/roles","Technician"));
                options.AddPolicy("RequiredPatientRole",policy => policy.RequireClaim($"{Namespace_Roles}/roles","Patient"));
                });
                
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication(); //Add
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureMyServices(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork,UnitOfWork>();

            services.AddTransient<ICategoryRepository,CategoryRepository>();
            services.AddTransient<CategoryService>();

            services.AddTransient<IProductRepository,ProductRepository>();
            services.AddTransient<ProductService>();

            services.AddTransient<IFamilyRepository,FamilyRepository>();
            services.AddTransient<FamilyService>();

            services.AddTransient<IOperationTypeRepository,OperationTypeRepository>();
            services.AddTransient<OperationTypeService>();

            services.AddTransient<ISpecializationRepository,SpecializationRepository>();
            services.AddTransient<SpecializationService>();

            services.AddTransient<IOperationTypeSpecializationRepository,OperationTypeSpecializationRepository>();
            
            services.AddTransient<IUserRepository,UserRepository>();
            services.AddTransient<UserService>();

            services.AddTransient<AuthenticationService>();

            services.AddTransient<IStaffRepository,StaffRepository>();
            services.AddTransient<StaffService>();

            services.AddTransient<IPatientRepository,PatientRepository>();
            services.AddTransient<PatientService>();

            services.AddTransient<IOperationRequestRepository,OperationRequestRepository>();
            services.AddTransient<OperationRequestService>();

            services.AddTransient<IAvailabilitySlotRepository,AvailabilitySlotRepository>();

            services.AddTransient<IAnonymizedPatientDataRepository,AnonymizedPatientDataRepository>();

            services.AddTransient<IEmailService, EmailService>();

        }
    }
}
