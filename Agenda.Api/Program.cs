using Agenda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Agenda.Core.Interface;
using Agenda.Infrastructure.Repositories;
using Agenda.Api.Services;
using Agenda.Api.Mappers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Agenda.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                        policy  =>
                        {
                            policy.WithOrigins("http://localhost:5173") 
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                        });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<CreateContactValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<AgendaDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddScoped<IContactRepository, ContactRepository>();
    
builder.Services.AddScoped<IContactService, ContactService>();
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  
    app.UseSwaggerUI(); 
}

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();