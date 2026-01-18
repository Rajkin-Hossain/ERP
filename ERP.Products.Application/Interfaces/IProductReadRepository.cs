using BuildingBlocks.Application.Interfaces;
using ERP.Products.Domain.Entities;
using ERP.Products.Domain.ValueObjects;

namespace ERP.Products.Application.Interfaces;

public interface IProductReadRepository : IRepositoryBase<Product, ProductId> { }