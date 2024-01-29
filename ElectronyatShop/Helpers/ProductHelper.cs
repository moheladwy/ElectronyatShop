// ElectronyatShopElectronyatShopProductHelper.cs
// 202412918:25
// eladwyeladwy

using System.Collections.Immutable;
using ElectronyatShop.DTOs;
using ElectronyatShop.Models;

namespace ElectronyatShop.Helpers;

public static class ProductHelper
{
    public static IEnumerable<ProductDto> ConvertProductsToProductsDto(IEnumerable<Product> products)
    {
        return products.Select(product => new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageName = product.Image,
                AvailableQuantity = product.AvailableQuantity,
                DiscountPercentage = product.DiscountPercentage,
                Price = product.Price,
                ProductType = product.Type,
                Status = product.Status
            })
            .ToList();
    }

    public static ProductDto ConvertProductToProductDto(Product product)
    {
        return new ProductDto()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageName = product.Image,
            AvailableQuantity = product.AvailableQuantity,
            DiscountPercentage = product.DiscountPercentage,
            Price = product.Price,
            ProductType = product.Type,
            Status = product.Status
        };
    }
}