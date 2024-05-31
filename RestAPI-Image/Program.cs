using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestAPI_Image.Services.Images;
using RestAPI_Image.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configura��o dos servi�os
builder.Services.AddControllers();
builder.Services.AddScoped<IImagesServices, ImagesServices>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Configura��o do Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();