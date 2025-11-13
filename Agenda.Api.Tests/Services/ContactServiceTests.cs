using Agenda.Api.Services;
using Agenda.Core.Entities;
using Agenda.Core.Interface;
using Agenda.Core.Dtos;
using Agenda.Core.Exceptions;
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
            var fakeContact = new Contact { Id = 1, Name = "Contact test" };
            var fakeDto = new ContactResponseDto { Id = 1, Name = "Test Contact" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1))
                           .ReturnsAsync(fakeContact);

            _mockMapper.Setup(mapper => mapper.Map<ContactResponseDto?>(fakeContact))
                       .Returns(fakeDto);

            var result = await _service.GetContactByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Contact", result.Name);
        }

        [Fact]
        public async Task GetContactByIdAsync_ReturnNull_Id()
        {
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Contact?)null);

            _mockMapper.Setup(mapper => mapper.Map<ContactResponseDto?>(null))
                       .Returns((ContactResponseDto?)null);

            var result = await _service.GetContactByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllContactsAsync_ReturnAll_Exist()
        {
            var fakeContacts = new List<Contact>
            {
                new Contact { Id = 1, Name = "Test 1" },
                new Contact { Id = 2, Name = "Test 2" }
            };
            var fakeDtos = new List<ContactResponseDto>
            {
                new ContactResponseDto { Id = 1, Name = "Test 1" },
                new ContactResponseDto { Id = 2, Name = "Test 2" }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(fakeContacts);
            _mockMapper.Setup(m => m.Map<IEnumerable<ContactResponseDto>>(fakeContacts)).Returns(fakeDtos);

            var result = await _service.GetAllContactsAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(fakeDtos, result);
        }

        [Fact]
        public async Task CreateContactAsync_ReturnNewContact_DataIsValid()
        {
            var inputDto = new CreateContactDto { Name = "New", Email = "new@test.com", Phone = "123" };
            var mappedContact = new Contact { Name = "New", Email = "new@test.com", Phone = "123" };
            var savedContact = new Contact { Id = 1, Name = "New", Email = "new@test.com", Phone = "123" };
            var outputDto = new ContactResponseDto { Id = 1, Name = "New" };

            _mockMapper.Setup(m => m.Map<Contact>(inputDto)).Returns(mappedContact);
            _mockRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((Contact?)null);
            _mockRepository.Setup(r => r.GetByPhoneAsync(It.IsAny<string>())).ReturnsAsync((Contact?)null);
            _mockRepository.Setup(r => r.AddAsync(mappedContact)).ReturnsAsync(savedContact);
            _mockMapper.Setup(m => m.Map<ContactResponseDto>(savedContact)).Returns(outputDto);

            var result = await _service.CreateContactAsync(inputDto);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockRepository.Verify(r => r.AddAsync(mappedContact), Times.Once);
        }

        [Fact]
        public async Task CreateContactAsync_ThrowException_EmailExists()
        {
            var inputDto = new CreateContactDto { Email = "dupe@test.com" };
            var mappedContact = new Contact { Email = "dupe@test.com" };
            var existingContact = new Contact { Id = 99, Email = "dupe@test.com" };

            _mockMapper.Setup(m => m.Map<Contact>(inputDto)).Returns(mappedContact);
            _mockRepository.Setup(r => r.GetByEmailAsync("dupe@test.com")).ReturnsAsync(existingContact);

            var exception = await Assert.ThrowsAsync<DuplicateDataException>(
                () => _service.CreateContactAsync(inputDto)
            );
            
            Assert.Equal("Email já cadastrado", exception.Message);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Contact>()), Times.Never);
        }

        [Fact]
        public async Task CreateContactAsync_ThrowException_PhoneExists()
        {
            var inputDto = new CreateContactDto { Phone = "123" };
            var mappedContact = new Contact { Phone = "123" };
            var existingContact = new Contact { Id = 99, Phone = "123" };

            _mockMapper.Setup(m => m.Map<Contact>(inputDto)).Returns(mappedContact);
            _mockRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((Contact?)null);
            _mockRepository.Setup(r => r.GetByPhoneAsync("123")).ReturnsAsync(existingContact);

            await Assert.ThrowsAsync<DuplicateDataException>(
                () => _service.CreateContactAsync(inputDto)
            );
        }

        [Fact]
        public async Task UpdateContactAsync_Complete_DataIsValid()
        {
            var inputDto = new UpdateContactDto { Name = "Updated Name" };
            var mappedContact = new Contact { Id = 1, Name = "Updated Name" };
            var existingContact = new Contact { Id = 1, Name = "Original Name", Email = "test@test.com", Phone = "123" };

            _mockMapper.Setup(m => m.Map<Contact>(inputDto)).Returns(mappedContact);
            _mockRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((Contact?)null);
            _mockRepository.Setup(r => r.GetByPhoneAsync(It.IsAny<string>())).ReturnsAsync((Contact?)null);
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingContact);
            _mockRepository.Setup(r => r.UpdateAsync(existingContact)).Returns(Task.CompletedTask);

            await _service.UpdateContactAsync(1, inputDto);

            _mockRepository.Verify(r => r.UpdateAsync(existingContact), Times.Once);
            Assert.Equal("Updated Name", existingContact.Name);
        }

        [Fact]
        public async Task UpdateContactAsync_ThrowException_ContactNotFound()
        {
            var inputDto = new UpdateContactDto();
            var mappedContact = new Contact { Id = 99 };

            _mockMapper.Setup(m => m.Map<Contact>(inputDto)).Returns(mappedContact);
            _mockRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((Contact?)null);
            _mockRepository.Setup(r => r.GetByPhoneAsync(It.IsAny<string>())).ReturnsAsync((Contact?)null);
            _mockRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Contact?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.UpdateContactAsync(99, inputDto)
            );
        }

        [Fact]
        public async Task UpdateContactAsync_ThrowException_EmailExistsOnAnother()
        {
            var inputDto = new UpdateContactDto { Email = "dupe@test.com" };
            var mappedContact = new Contact { Id = 1, Email = "dupe@test.com" };
            var existingDupeContact = new Contact { Id = 2, Email = "dupe@test.com" };

            _mockMapper.Setup(m => m.Map<Contact>(inputDto)).Returns(mappedContact);
            _mockRepository.Setup(r => r.GetByEmailAsync("dupe@test.com")).ReturnsAsync(existingDupeContact);

            await Assert.ThrowsAsync<DuplicateDataException>(
                () => _service.UpdateContactAsync(1, inputDto)
            );
        }

        [Fact]
        public async Task DeleteContactAsync_Complete_ContactExists()
        {
            var existingContact = new Contact { Id = 1 };

            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingContact);
            _mockRepository.Setup(r => r.DeleteAsync(existingContact)).Returns(Task.CompletedTask);

            await _service.DeleteContactAsync(1);

            _mockRepository.Verify(r => r.DeleteAsync(existingContact), Times.Once);
        }

        [Fact]
        public async Task DeleteContactAsync_DoNothing_ContactDoesNotExist()
        {
            _mockRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Contact?)null);

            await _service.DeleteContactAsync(99);

            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Contact>()), Times.Never);
        }
    }
}