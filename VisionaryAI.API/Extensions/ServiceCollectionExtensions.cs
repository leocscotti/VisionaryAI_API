using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VisionaryAI.API.Configuration;
using VisionaryAI.API.Database;
using VisionaryAI.API.Services;

namespace VisionaryAI.API.Extensions
{
    public static class ServiceColletionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddScoped<IEmpresaService,EmpresaService>();
            service.AddScoped<ICidadeService, CidadeService>();
            service.AddScoped<IFonteDadosService, FonteDadosService>();
            service.AddScoped<MLModelService>();

            return service;
        }

        public static IServiceCollection AddDBContexts(this IServiceCollection service, AppConfiguration appConfiguration)
        {
            service.AddDbContext<VisionaryAIDBContext>(options =>
            {
                options.UseOracle(appConfiguration.ConnectionStrings.OracleVisionaryAI);
            });

            return service;
        }


        public static IServiceCollection AddSwagger(this IServiceCollection service, AppConfiguration appConfiguration)
        {

            service.AddSwaggerGen(swagger =>
            {
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = appConfiguration.Swagger.Title,
                    Version = "v1",
                    Description = appConfiguration.Swagger.Description,
                }
                );
            });


            return service;
        }
    }
}
