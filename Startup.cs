using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OnBoardConsultantWebApi.DataAccessLayer.DataLayerUtilities;
using OnBoardConsultantWebApi.DataAccessLayer.IRepository;
using OnBoardConsultantWebApi.DataAccessLayer.Repository;
using OnBoardConsultantWebApi.Models;
using OnBoardConsultantWebApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnBoardConsultantWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSetting = this.Configuration.GetSection("AppSettings." + this.Configuration.GetSection("Environment").Value);

            var UseDBConnection = appSetting.GetSection("DatabaseConnection").Value;
            services.AddDbContextPool<HROnBoard_TestContext>(options => options.UseSqlServer(UseDBConnection));

            services.AddCors();
            services.AddControllers();

            services.AddSingleton<TrackUser>();

            services.AddScoped<IStatusCodeUtility, StatusCodeUtility>();
            services.AddScoped<IPermissionUtility, PermissionUtility>();

            services.AddScoped<IPersonRepository, PersonRepository>(); 
            services.AddScoped<IConsultantWorkFlowRepository, ConsultantWorkFlowRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseCors(
                options => options.WithOrigins("http://localhost:4200")
                 .AllowAnyMethod()
                 .AllowAnyHeader()
            );
            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware<TrackUserMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}



// Scaffold-DbContext "Data Source=192.168.9.69,1433;Initial Catalog=HROnBoard_Test;Persist Security Info=True;User ID=sa;Password=Jun32o0N1n3" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Tables Applications,ApplicationPermissions,Client,ClientWorkFlow,CodeType,CodeList,DocumentTypeTemplate,DocumentTemplateRole,Memberships,MembershipUser,Person,PersonContact,Permissions,PermissionsInRole,Policy,Roles,StageDocumentType,SubContractorConsultant,SubContractor,UserDocument,UserWorkFlow,UserPerson,UserWorkFlowStage,UsersInRole,UserStageDocument,UserStageDocumentInstruction,UserPolicy,WorkFlowStage -f