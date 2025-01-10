using DataVision.Application.Common.Models;
using DataVision.Application.Common.Validators;
using DataVision.Domain.Constants;
using FluentAssertions;
using NUnit.Framework;

namespace DataVision.Application.UnitTests
{
    public class PaginationValidatorTests
    {
        [Test]
        public void ShouldFail_WhenPageNumberIsLessThanMinimum()
        {
            // Arrange
            var validator = new PaginationValidator();
            var query = new PaginatedQuery()
            {
                PageNumber = DefaultValues.MinPageNumber - 1,
                PageSize = 10
            };

            // Act
            var result = validator.Validate(query);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(query.PageNumber));
        }

        [Test]
        public void ShouldFail_WhenPageNumberIsGreaterThanMaximum()
        {
            // Arrange
            var validator = new PaginationValidator();
            var query = new PaginatedQuery()
            {
                PageNumber = DefaultValues.MaxPageNumber + 1,
                PageSize = 10
            };

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(query.PageNumber));
        }

        [Test]
        public void ShouldFail_WhenPageSizeIsLessThanMinimum()
        {
            // Arrange
            var validator = new PaginationValidator();
            var query = new PaginatedQuery()
            {
                PageNumber = 1,
                PageSize = DefaultValues.MinPageSize - 1
            };

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(query.PageSize));
        }

        [Test]
        public void ShouldFail_WhenPageSizeIsGreaterThanMaximum()
        {
            // Arrange
            var validator = new PaginationValidator();
            var query = new PaginatedQuery()
            {
                PageNumber = 1,
                PageSize = DefaultValues.MaxPageSize + 1
            };

            // Act
            var result = validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(query.PageSize));
        }
    }
}
