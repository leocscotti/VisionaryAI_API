using Moq;
using Xunit;
using VisionaryAI.API.Database;
using VisionaryAI.API.Models;
using VisionaryAI.API.Services;
using Microsoft.EntityFrameworkCore;

namespace VisionaryAI.API.Tests
{
    public class FonteDadosServiceTests
    {
        private readonly Mock<VisionaryAIDBContext> _mockDbContext;
        private readonly FonteDadosService _service;

        public FonteDadosServiceTests()
        {
            _mockDbContext = new Mock<VisionaryAIDBContext>();
            _service = new FonteDadosService(_mockDbContext.Object);
        }

        [Fact]
        public async Task BuscarPorId_DeveRetornarFonteDados_QuandoIdExistir()
        {
            // Arrange
            var fonteDadosId = 1;
            var fonteDados = new FonteDados { Id = fonteDadosId, Nome = "Fonte 1", Tipo = "Tipo A" };

            var mockDbSet = new Mock<DbSet<FonteDados>>();
            mockDbSet.Setup(m => m.FindAsync(fonteDadosId)).ReturnsAsync(fonteDados);

            _mockDbContext.Setup(c => c.FonteDeDados).Returns(mockDbSet.Object);

            // Act
            var result = await _service.BuscarPorId(fonteDadosId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fonteDadosId, result.Id);
        }

        [Fact]
        public async Task BuscarTodasFonteDados_DeveRetornarListaDeFonteDados()
        {
            // Arrange
            var fontesDados = new List<FonteDados>
        {
            new FonteDados { Id = 1, Nome = "Fonte 1", Tipo = "Tipo A" },
            new FonteDados { Id = 2, Nome = "Fonte 2", Tipo = "Tipo B" }
        }.AsQueryable();

            var mockDbSet = new Mock<DbSet<FonteDados>>();
            mockDbSet.As<IQueryable<FonteDados>>().Setup(m => m.Provider).Returns(fontesDados.Provider);
            mockDbSet.As<IQueryable<FonteDados>>().Setup(m => m.Expression).Returns(fontesDados.Expression);
            mockDbSet.As<IQueryable<FonteDados>>().Setup(m => m.ElementType).Returns(fontesDados.ElementType);
            mockDbSet.As<IQueryable<FonteDados>>().Setup(m => m.GetEnumerator()).Returns(fontesDados.GetEnumerator());

            _mockDbContext.Setup(c => c.FonteDeDados).Returns(mockDbSet.Object);

            // Act
            var result = await _service.BuscarTodasFonteDados();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Adicionar_DeveAdicionarFonteDados()
        {
            // Arrange
            var fonteDados = new FonteDados { Nome = "Fonte 1", Tipo = "Tipo A" };

            var mockDbSet = new Mock<DbSet<FonteDados>>();
            _mockDbContext.Setup(c => c.FonteDeDados).Returns(mockDbSet.Object);

            // Act
            var result = await _service.Adicionar(fonteDados);

            // Assert
            mockDbSet.Verify(m => m.AddAsync(fonteDados, default), Times.Once);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
            Assert.Equal("Fonte 1", result.Nome);
        }

        [Fact]
        public async Task Atualizar_DeveAtualizarFonteDados_QuandoIdExistir()
        {
            // Arrange
            var fonteDadosId = 1;
            var fonteDadosExistente = new FonteDados { Id = fonteDadosId, Nome = "Fonte Antiga", Tipo = "Tipo A" };
            var fonteDadosAtualizada = new FonteDados { Nome = "Fonte Nova", Tipo = "Tipo B" };

            var mockDbSet = new Mock<DbSet<FonteDados>>();
            mockDbSet.Setup(m => m.FindAsync(fonteDadosId)).ReturnsAsync(fonteDadosExistente);
            _mockDbContext.Setup(c => c.FonteDeDados).Returns(mockDbSet.Object);

            // Act
            var result = await _service.Atualizar(fonteDadosAtualizada, fonteDadosId);

            // Assert
            Assert.Equal("Fonte Nova", result.Nome);
            Assert.Equal("Tipo B", result.Tipo);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Apagar_DeveRemoverFonteDados_QuandoIdExistir()
        {
            // Arrange
            var fonteDadosId = 1;
            var fonteDados = new FonteDados { Id = fonteDadosId };

            var mockDbSet = new Mock<DbSet<FonteDados>>();
            mockDbSet.Setup(m => m.FindAsync(fonteDadosId)).ReturnsAsync(fonteDados);
            _mockDbContext.Setup(c => c.FonteDeDados).Returns(mockDbSet.Object);

            // Act
            var result = await _service.Apagar(fonteDadosId);

            // Assert
            Assert.True(result);
            mockDbSet.Verify(m => m.Remove(fonteDados), Times.Once);
            _mockDbContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }
    }
}
