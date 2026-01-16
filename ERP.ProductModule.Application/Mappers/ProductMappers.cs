using ERP.ProductModule.Application.Results;
using ERP.ProductModule.Domain.Entities;
using System.Linq.Expressions;

namespace ERP.ProductModule.Application.Mappers;

public static class ProductMappers
{
    public static Expression<Func<Product, ProductResult>> ToProductToProductResultExpression
        => p => new ProductResult(
            p.ProductName.Value, // should be OK because converter stores scalar string
            p.ImageUrl.Value,
            p.Price.Value
        );
}