using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Models;
using TimescaleAPI.Application.Utilities;

namespace TimescaleAPI.Tests;

public class ConverterTests
{
    [Fact]
    public void ToValueModel_MapsAllFields()
    {
        var origin = new Origin("test.csv");
        var date = new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Utc);
        var dto = new TimescaleValueDto(date, 200, 55.5);

        var model = dto.ToValueModel(origin);

        Assert.Equal(date.ToUniversalTime(), model.Date);
        Assert.Equal(200, model.ExecutionTime);
        Assert.Equal(55.5, model.IndicatorValue);
        Assert.Equal(origin, model.Origin);
    }

    [Fact]
    public void ToValuesDto_MapsAllFields()
    {
        var date = new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Utc);
        var model = new Value(date, 150, 33.3);

        var dto = model.ToValuesDto();

        Assert.Equal(date, dto.Date);
        Assert.Equal(150, dto.ExecutionTime);
        Assert.Equal(33.3, dto.Value);
    }

    [Fact]
    public void ToResultDto_MapsAllFields()
    {
        var minDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var result = new Result(86400, minDate, 150.0, 42.5, 40.0, 100.0, 5.0);

        var dto = result.ToResultDto("report.csv");

        Assert.Equal("report.csv", dto.FileName);
        Assert.Equal(86400, dto.DeltaDate);
        Assert.Equal(minDate, dto.MinDate);
        Assert.Equal(150.0, dto.AvgExecutionTime);
        Assert.Equal(42.5, dto.AvgValue);
        Assert.Equal(40.0, dto.MedianValue);
        Assert.Equal(100.0, dto.MaxValue);
        Assert.Equal(5.0, dto.MinValue);
    }

    [Fact]
    public void TwoWay_ValueDto_Model()
    {
        var origin = new Origin("test.csv");
        var date = new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Utc);
        var originalDto = new TimescaleValueDto(date, 100, 42.5);

        var model = originalDto.ToValueModel(origin);
        var twoWayDto = model.ToValuesDto();

        Assert.Equal(originalDto.Date, twoWayDto.Date);
        Assert.Equal(originalDto.ExecutionTime, twoWayDto.ExecutionTime);
        Assert.Equal(originalDto.Value, twoWayDto.Value);
    }
}
