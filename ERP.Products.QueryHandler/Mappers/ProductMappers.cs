using ERP.Products.Domain.Entities;
using ERP.Shared.Query.Contracts.Modules.Products.AppResult;
using System.Linq.Expressions;

namespace ERP.Products.QueryHandler.Mappers;

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
