using Moq;
using Xunit;
using VisionaryAI.API.Database;
using VisionaryAI.API.Models;
using VisionaryAI.API.Services;
using Microsoft.EntityFrameworkCore;

namespace VisionaryAI.API.Tests
{
    public class EmpresaServiceTests
    {
        private readonly Mock<VisionaryAIDBContext> _mockDbContext;
        private readonly EmpresaService _service;

        public EmpresaServiceTests()
        {
            _mockDbContext = new Mock<VisionaryAIDBContext>();
            _service = new EmpresaService(_mockDbContext.Object);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarEmpresa_QuandoIdExistir()
        {
            // Arrange
            var empresaId = 1;
            var empresa = new Empresa { Id = empresaId, Nome = "Empresa 1", Cnpj = "123456789" };

            var mockDbSet = new Mock<DbSet<Empresa>>();
            mockDbSet.Setup(m => m.FindAsync(empresaId)).ReturnsAsync(empresa);

            _mockDbContext.Setup(c => c.Empresas).Returns(mockDbSet.Object);

            // Act
            var result = await _service.BuscarPorId(empresaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(empresaId, result.Id);
        }

        [Fact]
        public async Task BuscarTodasEmpresas_DeveRetornarListaDeEmpresas()
        {
            // Arrange
            var empresas = new List<Empresa>
        {
            new Empresa { Id = 1, Nome = "Empresa 1", Cnpj = "123456789" },
            new Empresa { Id = 2, Nome = "Empresa 2", Cnpj = "987654321" }
        }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Empresa>>();
            mockDbSet.As<IQueryable<Empresa>>().Setup(m => m.Provider).Returns(empresas.Provider);
            mockDbSet.As<IQueryable<Empresa>>().Setup(m => m.Expression).Returns(empresas.Expression);
            mockDbSet.As<IQueryable<Empresa>>().Setup(m => m.ElementType).Returns(empresas.ElementType);
            mockDbSet.As<IQueryable<Empresa>>().Setup(m => m.GetEnumerator()).Returns(empresas.GetEnumerator());

            _mockDbContext.Setup(c => c.Empresas).Returns(mockDbSet.Object);

            // Act
            var result = await _service.BuscarTodasEmpresas();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Adicionar_DeveAdicionarEmpresa()
        {
            // Arrange
            var empresa = new Empresa { Nome = "Empresa 1", Cnpj = "123456789" };

            var mockDbSet = new Mock<DbSet<Empresa>>();
            _mockDbContext.Setup(c => c.Empresas).Returns(mockDbSet.Object);

            // Act
            var result = await _service.Adicionar(empresa);

            // Assert
            mockDbSet.Verify(m => m.AddAsync(empresa, default), Times.Once);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
            Assert.Equal("Empresa 1", result.Nome);
        }

        [Fact]
        public async Task Atualizar_DeveAtualizarEmpresa_QuandoIdExistir()
        {
            // Arrange
            var empresaId = 1;
            var empresaExistente = new Empresa { Id = empresaId, Nome = "Empresa Antiga", Cnpj = "123456789" };
            var empresaAtualizada = new Empresa { Nome = "Empresa Nova", Cnpj = "987654321" };

            var mockDbSet = new Mock<DbSet<Empresa>>();
            mockDbSet.Setup(m => m.FindAsync(empresaId)).ReturnsAsync(empresaExistente);
            _mockDbContext.Setup(c => c.Empresas).Returns(mockDbSet.Object);

            // Act
            var result = await _service.Atualizar(empresaAtualizada, empresaId);

            // Assert
            Assert.Equal("Empresa Nova", result.Nome);
            Assert.Equal("987654321", result.Cnpj);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Apagar_DeveRemoverEmpresa_QuandoIdExistir()
        {
            // Arrange
            var empresaId = 1;
            var empresa = new Empresa { Id = empresaId };

            var mockDbSet = new Mock<DbSet<Empresa>>();
            mockDbSet.Setup(m => m.FindAsync(empresaId)).ReturnsAsync(empresa);
            _mockDbContext.Setup(c => c.Empresas).Returns(mockDbSet.Object);

            // Act
            var result = await _service.Apagar(empresaId);

            // Assert
            Assert.True(result);
            mockDbSet.Verify(m => m.Remove(empresa), Times.Once);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }
    }
}