using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Back.Services.Audit;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SmartNutriTracker.Test.Services
{
    public class AuditServiceTests
    {
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly Mock<ILogger<AuditService>> _mockLogger;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly AuditService _auditService;

        public AuditServiceTests()
        {
            _mockDbContext = new Mock<ApplicationDbContext>();
            _mockLogger = new Mock<ILogger<AuditService>>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            _auditService = new AuditService(
                _mockDbContext.Object,
                _mockHttpContextAccessor.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task LogAsync_ShouldLogError_WhenExceptionOccurs()
        {
            // Arrange
            _mockDbContext.Setup(db => db.SaveChangesAsync(default)).ThrowsAsync(new Exception("Database error"));

            // Act
            await _auditService.LogAsync("TestAction", "INFO", "TestDetail");

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error registrando auditoría")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }
    }
}