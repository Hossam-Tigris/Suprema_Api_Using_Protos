
using Google.Protobuf.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Suprema_Api_Using_Protos.DTOs;
using Suprema_Api_Using_Protos.Helper;
using Suprema_Api_Using_Protos.Services;
using System.Net;
using System.Net.Sockets;

namespace Suprema_Api_Using_Protos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string Ip = Dns.GetHostEntry(Dns.GetHostName())
                                .AddressList
                                .First(x => x.AddressFamily == AddressFamily.InterNetwork)
                                .ToString();

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var response = new ApiResponse<List<string>>(
                        data: errors,
                        success: false,
                        message: "Validation error"
                    );

                    return new BadRequestObjectResult(response);
                };
            }); ;
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.Configure<GatewayOptions>(
              builder.Configuration.GetSection("Gateway"));
            builder.Services.Configure<List<DeviceOptions>>(
                builder.Configuration.GetSection("Devices"));

            builder.Services.AddSingleton<DeviceManager>(sp =>
            {
                var gatewayOptions = sp.GetRequiredService<IOptions<GatewayOptions>>().Value;
                var deviceOptions = sp.GetRequiredService<IOptions<List<DeviceOptions>>>().Value;

                var manager = new DeviceManager(
                    gatewayOptions.CaPath,
                    Ip,
                    gatewayOptions.Port
                );

                //manager.ConnectDevicesAsync(
                //deviceOptions.Select(d => (d.Ip, d.Port, d.UseSSL)).ToList()).GetAwaiter().GetResult();

                return manager;
            });


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
