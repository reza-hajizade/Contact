using Contact.EndPoints;
using Contact.Infrastructure;
using Contact.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.BrokerConfigure();

builder.AddApplicationServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map contact endpoints under /api/v1/contact with "Contact" Swagger tag
app.MapGroup("/api/v1/contact")
    .WithTags("Contact")
    .MapContactEndpoints();

app.Run();
