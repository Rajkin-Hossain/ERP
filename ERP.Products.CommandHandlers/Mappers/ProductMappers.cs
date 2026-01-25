using ERP.Products.CommandHandlers.Results;
using ERP.Products.Domain.Entities;
using System.Linq.Expressions;
namespace ERP.Products.CommandHandlers.Mappers;

public static class ProductMappers
{
    public static Expression<Func<Product, ProductResult>> ToProductToProductResultExpression
        => p => new ProductResult(
            p.Id,
            p.CategoryId,
            p.ProductName,
            p.ImageUrl,
            p.Price
        );
}





