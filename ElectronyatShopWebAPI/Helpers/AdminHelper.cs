using ElectronyatShopWebAPI.DTOs;
using ElectronyatShopWebAPI.Models;

namespace ElectronyatShopWebAPI.Helpers;

public static class AdminHelper
{
    public static Product CreateProduct(ProductDto productDto)
    {
        return new Product
        {
            Name = productDto.Name,
            Image = ProcessUploadedImage(productDto),
            Type = productDto.ProductType,
            Description = productDto.Description,
            Price = productDto.Price,
            AvailableQuantity = productDto.AvailableQuantity,
            DiscountPercentage = productDto.DiscountPercentage,
            Status = productDto.Status
        };
    }

    public static void UpdateProduct(Product product, Product productToBeUpdated)
    {
        product.Name = productToBeUpdated.Name;
        product.Type = productToBeUpdated.Type;
        product.Description = productToBeUpdated.Description;
        product.Price = productToBeUpdated.Price;
        product.AvailableQuantity = productToBeUpdated.AvailableQuantity;
        product.DiscountPercentage = productToBeUpdated.DiscountPercentage;
        product.Status = productToBeUpdated.Status;
        product.Image = productToBeUpdated.Image;
    }
    
    private static string ProcessUploadedImage(ProductDto productDto)
    {
        var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
        var imageName = $"{DateTime.Now:dddd-MMM-dd-yyyy-hh-mm-ss}-{productDto.Image?.FileName}";
        var imagePath = Path.Combine(imagesDirectory, imageName);
        var stream = new FileStream(imagePath, FileMode.Create);
        productDto.Image?.CopyTo(stream);
        return imageName;
    }
    
    private static void DeleteImage(string imageName)
    {
        var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
        var imagePath = Path.Combine(imagesDirectory, imageName);
        // TODO: Figure out what the cause of the IOException that happens upon deleting the Image.
        // System.IO.File.Delete(ImagePath);
    }
}