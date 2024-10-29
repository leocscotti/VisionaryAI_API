using Moq;
using Xunit;
using VisionaryAI.API.Database;
using VisionaryAI.API.Models;
using VisionaryAI.API.Services;
using Microsoft.EntityFrameworkCore;

namespace VisionaryAI.API.Tests
{
    public class CidadeServiceTests
    {
        private readonly Mock<VisionaryAIDBContext> _mockDbContext;
        private readonly CidadeService _service;

        public CidadeServiceTests()
        {
            _mockDbContext = new Mock<VisionaryAIDBContext>();
            _service = new CidadeService(_mockDbContext.Object);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarCidade_QuandoIdExistir()
        {
            // Arrange
            var cidadeId = 1;
            var cidade = new Cidade { Id = cidadeId, Nome = "Cidade 1", Uf = "SP" };

            var mockDbSet = new Mock<DbSet<Cidade>>();
            mockDbSet.Setup(m => m.FindAsync(cidadeId)).ReturnsAsync(cidade);

            _mockDbContext.Setup(c => c.Cidades).Returns(mockDbSet.Object);

            // Act
            var result = await _service.BuscarPorId(cidadeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cidadeId, result.Id);
        }

        [Fact]
        public async Task BuscarTodasCidades_DeveRetornarListaDeCidades()
        {
            // Arrange
            var cidades = new List<Cidade>
        {
            new Cidade { Id = 1, Nome = "Cidade 1", Uf = "SP" },
            new Cidade { Id = 2, Nome = "Cidade 2", Uf = "RJ" }
        }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Cidade>>();
            mockDbSet.As<IQueryable<Cidade>>().Setup(m => m.Provider).Returns(cidades.Provider);
            mockDbSet.As<IQueryable<Cidade>>().Setup(m => m.Expression).Returns(cidades.Expression);
            mockDbSet.As<IQueryable<Cidade>>().Setup(m => m.ElementType).Returns(cidades.ElementType);
            mockDbSet.As<IQueryable<Cidade>>().Setup(m => m.GetEnumerator()).Returns(cidades.GetEnumerator());

            _mockDbContext.Setup(c => c.Cidades).Returns(mockDbSet.Object);

            // Act
            var result = await _service.BuscarTodasCidades();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Adicionar_DeveAdicionarCidade()
        {
            // Arrange
            var cidade = new Cidade { Nome = "Cidade 1", Uf = "SP" };

            var mockDbSet = new Mock<DbSet<Cidade>>();
            _mockDbContext.Setup(c => c.Cidades).Returns(mockDbSet.Object);

            // Act
            var result = await _service.Adicionar(cidade);

            // Assert
            mockDbSet.Verify(m => m.AddAsync(cidade, default), Times.Once);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
            Assert.Equal("Cidade 1", result.Nome);
        }

        [Fact]
        public async Task Atualizar_DeveAtualizarCidade_QuandoIdExistir()
        {
            // Arrange
            var cidadeId = 1;
            var cidadeExistente = new Cidade { Id = cidadeId, Nome = "Cidade Antiga", Uf = "SP" };
            var cidadeAtualizada = new Cidade { Nome = "Cidade Nova", Uf = "RJ" };

            var mockDbSet = new Mock<DbSet<Cidade>>();
            mockDbSet.Setup(m => m.FindAsync(cidadeId)).ReturnsAsync(cidadeExistente);
            _mockDbContext.Setup(c => c.Cidades).Returns(mockDbSet.Object);

            // Act
            var result = await _service.Atualizar(cidadeAtualizada, cidadeId);

            // Assert
            Assert.Equal("Cidade Nova", result.Nome);
            Assert.Equal("RJ", result.Uf);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Apagar_DeveRemoverCidade_QuandoIdExistir()
        {
            // Arrange
            var cidadeId = 1;
            var cidade = new Cidade { Id = cidadeId };

            var mockDbSet = new Mock<DbSet<Cidade>>();
            mockDbSet.Setup(m => m.FindAsync(cidadeId)).ReturnsAsync(cidade);
            _mockDbContext.Setup(c => c.Cidades).Returns(mockDbSet.Object);

            // Act
            var result = await _service.Apagar(cidadeId);

            // Assert
            Assert.True(result);
            mockDbSet.Verify(m => m.Remove(cidade), Times.Once);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }
    }
}