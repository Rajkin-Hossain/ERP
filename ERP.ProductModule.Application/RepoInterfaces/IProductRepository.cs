using ERP.ProductModule.Domain.Entities;
using ERP.ProductModule.Domain.ValueObjects;
using ERP.SharedKernal.Interfaces;

namespace ERP.ProductModule.Application.RepoInterfaces;

public interface IProductRepository : IRepositoryBase<Product, ProductId> { }