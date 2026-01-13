using ERP.ProductModule.Application.Results;
using ERP.ProductModule.Domain.Entities;
using System.Linq.Expressions;

namespace ERP.ProductModule.Application.Mappers;

public static class ProductMappers
{
    public static Expression<Func<Product, ProductResult>> ToProductToProductResultExpression
        => product => new ProductResult(
            product.ProductName.Value,
            product.ImageUrl.Value,
            product.Price.Value
        );
}