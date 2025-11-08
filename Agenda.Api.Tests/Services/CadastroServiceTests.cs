using System.Diagnostics.CodeAnalysis;
using Agenda.Api.Services;
using Agenda.Core.Entities;
using Agenda.Core.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Xunit;

namespace Agenda.Api.Tests.Services
{
    public class CadastroServiceTests
    {
        [Fact]
        public async Task GetContatoByIdAsync_RetornarContato_Id()
        {
            var contatoFalso = new Contato
            {
                Id = 1,
                Nome = "Contato teste"
            };

            var mockRepositorio = new Mock<IContatoRepository>();

            mockRepositorio.Setup(repo => repo.GetByIdAsync(1))
                           .Returns(Task.FromResult<Contato?>(contatoFalso));

            var servico = new CadastroService(mockRepositorio.Object);

            var resultado = await servico.GetContatoByIdAsync(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("Contato teste", resultado.Nome);
        }

        [Fact]
        public async Task GetContatoByIdAsync_RetornarNull_Id()
        {
            var mockRepositorio = new Mock<IContatoRepository>();

            mockRepositorio.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                            .Returns(Task.FromResult<Contato?>(null));

            var servico = new CadastroService(mockRepositorio.Object);
            var resultado = await servico.GetContatoByIdAsync(99);

            Assert.Null(resultado);
        }
    }
}