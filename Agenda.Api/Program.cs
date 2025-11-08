using Agenda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Agenda.Core.Interface; // (singular, como a sua pasta)
using Agenda.Infrastructure.Repositories;
using Agenda.Api.Services;
using Agenda.Api.Mappers;
using FluentValidation;
using FluentValidation.AspNetCore; // <-- ADICIONE ESTA LINHA
using Agenda.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.
builder.Services.AddControllers();

// Adiciona o Swagger (Configuração Clássica Completa)
builder.Services.AddEndpointsApiExplorer(); // <-- MUDANÇA
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<CreateContatoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAutoMapper(typeof(MappingProfile));
// 1. Pega a string de conexão
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Configura o DbContext...
builder.Services.AddDbContext<AgendaDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// 3. Regista o Repositório...
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
    
// 4. Regista o Serviço...
builder.Services.AddScoped<ICadastroService, CadastroService>();
    
var app = builder.Build();

// Configura o pipeline de requisições HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // <-- MUDANÇA
    app.UseSwaggerUI(); // <-- MUDANÇA
}

// app.UseHttpsRedirection(); // Pode comentar isto para evitar o aviso

app.UseAuthorization();

// Diz à API para usar os Controladores que criamos
app.MapControllers();

app.Run();