using ERP.Products.Domain.ValueObjects;

using ERP.Products.Application.Interfaces;
using ERP.Products.Domain.Entities;
namespace ERP.Products.Application.Contracts;

public sealed record ProductResult(
    ProductId ProductId,
    CategoryId CategoryId,
    ProductName ProductName,
    ImageUrl ImageUrl,
    Price Price
);


