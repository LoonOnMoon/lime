using Lime.Web.StartupExtensions;

using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add services to the container.
services.AddDatabase(builder.Configuration, builder.Environment);
services.AddAuth(builder.Configuration);
services.AddControllers();

// services.AddApiVersioning(opt =>
//     {
//         opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
//         opt.AssumeDefaultVersionWhenUnspecified = true;
//         opt.ReportApiVersions = true;
//         opt.ApiVersionReader = ApiVersionReader.Combine(
//             new UrlSegmentApiVersionReader(),
//             new HeaderApiVersionReader("x-api-version"),
//             new MediaTypeApiVersionReader("x-api-version"));
//     });
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();

// Configure the HTTP request pipeline.
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuth();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();