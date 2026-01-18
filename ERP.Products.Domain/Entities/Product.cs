using ERP.Products.Domain.DomainEvents;
using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Domain.Entities;

namespace ERP.Products.Domain.Entities;

public class Product : AggregateRoot<ProductId>
{
    public ProductName ProductName { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public ImageUrl ImageUrl { get; private set; }
    public Price Price { get; private set; }

    private Product() { }

    private Product(
        ProductId id,
        ProductName productName,
        CategoryId categoryId,
        ImageUrl imageUrl,
        Price price)
    {
        Id = id;
        ProductName = productName;
        CategoryId = categoryId;
        ImageUrl = imageUrl;
        Price = price;
    }

    // "Add Product" / Create
    public static Product Create(
        ProductName productName,
        CategoryId categoryId,
        ImageUrl imageUrl,
        Price price)
    {
        var product = new Product(ProductId.New(), productName, categoryId, imageUrl, price);

        product.AddDomainEvent(new ProductCreatedDomainEvent(
            product.Id,
            product.ProductName,
            product.CategoryId,
            product.ImageUrl,
            product.Price));

        return product;
    }

    // "Update Product"
    public void UpdateDetails(
        ProductName productName,
        ImageUrl imageUrl,
        Price price)
    {
        ProductName = productName;
        ImageUrl = imageUrl;
        Price = price;

        AddDomainEvent(new ProductUpdatedDomainEvent(
            Id,
            ProductName,
            CategoryId,
            ImageUrl,
            Price));
    }

    // "UpdatePrice"
    public void UpdatePrice(Price newPrice)
    {
        // Example rule: cannot set same price (optional)
        if (newPrice.Value == Price.Value)
            return;

        Price = newPrice;
    }
}






