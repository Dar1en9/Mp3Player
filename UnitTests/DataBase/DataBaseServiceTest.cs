using System.Data;
using System.Data.Common;
using Dapper;
using Moq;
using Mp3Player.DataBase;
using Mp3Player.TrackHandler;

namespace UnitTests.DataBase;

using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Moq;
using Xunit;

using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Moq;
using Xunit;

public class DataBaseServiceTests
{
    private readonly Mock<IDataBaseService> _mockDataBaseService;

    public DataBaseServiceTests()
    {
        _mockDataBaseService = new Mock<IDataBaseService>();
    }

    [Fact]
    public async Task QueryAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var expectedResults = new List<string> { "Result1", "Result2" };
        var sql = "SELECT * FROM TestTable";
        var parameters = new DynamicParameters();

        _mockDataBaseService.Setup(service => service.QueryAsync<string>(sql, parameters))
            .ReturnsAsync(expectedResults);

        // Act
        var result = await _mockDataBaseService.Object.QueryAsync<string>(sql, parameters);

        // Assert
        Assert.Equal(expectedResults, result);
        _mockDataBaseService.Verify(service => service.QueryAsync<string>(sql, parameters), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnAffectedRowsCount()
    {
        // Arrange
        var sql = "UPDATE TestTable SET Column1 = @Value WHERE Id = @Id";
        var parameters = new DynamicParameters();
        parameters.Add("@Value", "NewValue");
        parameters.Add("@Id", 1);
        var expectedAffectedRows = 1;

        _mockDataBaseService.Setup(service => service.ExecuteAsync(sql, parameters))
            .ReturnsAsync(expectedAffectedRows);

        // Act
        var result = await _mockDataBaseService.Object.ExecuteAsync(sql, parameters);

        // Assert
        Assert.Equal(expectedAffectedRows, result);
        _mockDataBaseService.Verify(service => service.ExecuteAsync(sql, parameters), Times.Once);
    }
}