using ERP.Products.Application.Contracts;
using ERP.Shared.Domain.Entities;

using System.Linq.Expressions;

using ERP.Products.Application.Interfaces;
using ERP.Products.Domain.Entities;
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





