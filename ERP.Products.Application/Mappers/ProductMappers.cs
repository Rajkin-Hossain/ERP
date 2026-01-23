using ERP.Products.Application.Results;
using ERP.Products.Domain.Entities;
using System.Linq.Expressions;
namespace ERP.Products.Application.Mappers;

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





