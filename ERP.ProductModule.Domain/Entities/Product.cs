using ERP.ProductModule.Domain.ValueObjects;
using ERP.SharedKernal.Entities;

namespace ERP.ProductModule.Domain.Entities;

public class Product : AggregateRoot<ProductId>
{
    public ProductId ProductId { get; private set; }
    public ProductName ProductName { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public ImageUrl ImageUrl { get; private set; }
    public Price Price { get; private set; }

    private Product() { } // For EF Core

    private Product(
        ProductId productId,
        ProductName productName,
        CategoryId categoryId,
        ImageUrl imageUrl,
        Price price)
    {
        ProductId = productId;
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
        return new Product(ProductId.New(), productName, categoryId, imageUrl, price);
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
