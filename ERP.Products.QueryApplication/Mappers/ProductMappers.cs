using ERP.Products.Domain.Entities;
using ERP.Products.QueryApplication.AppResults;
using System.Linq.Expressions;

namespace ERP.Products.QueryApplication.Mappers;

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
