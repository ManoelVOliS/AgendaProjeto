using Agenda.Core.Entities;
using Agenda.Infrastructure.Data;
using Bogus;
using Microsoft.EntityFrameworkCore;

const string connectionString = "Host=localhost;Port=5432;Database=agenda_db;Username=postgres;Password=root";

var contactFaker = new Faker<Contact>("en")
    .RuleFor(c => c.Name, f => f.Name.FullName())
    .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.Name, "", "testmail.com").ToLower())
    .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("##########"))

    .RuleFor(c => c.DateCreated, f => f.Date.Past(1).ToUniversalTime());

var contactsToSeed = contactFaker.Generate(100);

var optionsBuilder = new DbContextOptionsBuilder<AgendaDbContext>();
optionsBuilder.UseNpgsql(connectionString);

using (var context = new AgendaDbContext(optionsBuilder.Options))
{
    Console.WriteLine("Iniciando o Seeding...");

    if (context.Contacts.Any())
    {
        context.Contacts.RemoveRange(context.Contacts);
        context.SaveChanges();
        Console.WriteLine("Contatos antigos removidos.");
    }

    context.Contacts.AddRange(contactsToSeed);
    context.SaveChanges();

    Console.WriteLine($"Seeding concluído! {contactsToSeed.Count} contatos adicionados.");
}