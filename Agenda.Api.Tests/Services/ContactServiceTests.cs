using Agenda.Api.Services;
using Agenda.Core.Entities;
using Agenda.Core.Interface;
using AutoMapper;
using Moq;
using Xunit;

namespace Agenda.Api.Tests.Services
{
    public class ContactServiceTests
    {
        private readonly Mock<IContactRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;

        private readonly ContactService _service;

        public ContactServiceTests()
        {
            _mockRepository = new Mock<IContactRepository>();
            _mockMapper = new Mock<IMapper>();

            _service = new ContactService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetContactByIdAsync_ReturnContact_Id()
        {
            var fakeContact = new Contact 
            {
                Id = 1,
                Name = "Contact test"
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                            .ReturnsAsync(fakeContact);
            
            var result = await _service.GetContactByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Contact test", result.Name);
        }

        [Fact]
        public async Task GetContactByIdAsync_RetornarNull_Id()
        {
            
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                            .ReturnsAsync((Contact?)null);

            var result = await _service.GetContactByIdAsync(99);

            Assert.Null(result);
        }
    }
}