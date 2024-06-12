using backend.Data.Models;
using backend.Data.Models.DataBase;
using backend.Data.Models.ManyToManyConnections;
using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DataBase _ctx;
    public string baseUrl = "http://localhost:5004";
    
    public ProductRepository(DataBase ctx)
    {
        _ctx = ctx;
    }
    
    
    public async Task<IEnumerable<ProductDTO>> GetAllProductsByCategoryAsync(int categoryId)
    {
        var productsInCategory = await _ctx.Products
            .Where(p => p.CategoryProducts.Any(cp => cp.CategoryId == categoryId))
            .Include(p => p.CategoryProducts)
            .ThenInclude(cp => cp.Category)
            .Include(p => p.Photos)
            .ToListAsync();

        var productDTOs = new List<ProductDTO>();

        foreach (var product in productsInCategory)
        {
            var categoryIds = new List<int>();
            if (product.CategoryProducts != null)
            {
                foreach (var categoryProduct in product.CategoryProducts)
                {
                    categoryIds.Add(categoryProduct.CategoryId.Value);
                }
            }
        
            var productDTO = new ProductDTO()
            {
                IdProduct = product.IdProduct,
                ProductName = product.ProductName,
                Subtitle = product.Subtitle,
                amountOf = product.amountOf,
                Price = product.Price,
                CategoryIds = categoryIds,
                Photos = product.Photos.Select(p => p.Url).ToList()
            };

            productDTOs.Add(productDTO);
        }

        return productDTOs;
    }
    
    
    public async Task<IEnumerable<ProductDTO>> GetAllUserProducts(int userId)
    {
        var products = await _ctx.Products
            .Where(p => p.UserId == userId)
            .Include(p => p.CategoryProducts)
            .ThenInclude(cp => cp.Category)
            .ToListAsync();
        var productDTOs = new List<ProductDTO>();
        foreach (var product in products)
        {
            var categoryIds = new List<int>();

            if (product.CategoryIds != null)
            {
                foreach (var categoryProduct in product.CategoryIds)
                {
                    categoryIds.Add(categoryProduct);
                }
            }

            // Fetch photos associated with the current product
            var photos = await _ctx.Photos
                .Where(p => p.ProductId == product.IdProduct)
                .Select(p => $"{baseUrl}/images/{p.Url}")
                .ToListAsync();

            var productDTO = new ProductDTO()
            {
                IdProduct = product.IdProduct,
                ProductName = product.ProductName,
                Subtitle = product.Subtitle,
                amountOf = product.amountOf,
                Price = product.Price,
                CategoryIds = categoryIds,
                Photos = photos
            };

            productDTOs.Add(productDTO);
        }

        return productDTOs;
    }
    
    public async Task<IEnumerable<ProductDTO>> GetAllProducts()
    {
        var products = await _ctx.Products
            .Include(p => p.CategoryProducts)
            .ThenInclude(cp => cp.Category)
            .ToListAsync();

        var productDTOs = new List<ProductDTO>();

        foreach (var product in products)
        {
            var categoryIds = new List<int>();

            if (product.CategoryIds != null)
            {
                foreach (var categoryProduct in product.CategoryIds)
                {
                    categoryIds.Add(categoryProduct);
                }
            }

            // Fetch photos associated with the current product
            var photos = await _ctx.Photos
                .Where(p => p.ProductId == product.IdProduct)
                .Select(p => p.Url.Substring(p.Url.LastIndexOf('/') + 1))
                .Select(p => $"{baseUrl}/images/{p}")
                .ToListAsync();

            var productDTO = new ProductDTO()
            {
                IdProduct = product.IdProduct,
                ProductName = product.ProductName,
                Subtitle = product.Subtitle,
                amountOf = product.amountOf,
                Price = product.Price,
                CategoryIds = categoryIds,
                Photos = photos
            };

            productDTOs.Add(productDTO);
        }

        return productDTOs;
    }
    

    public async Task<ProductDTO?> GetProductById(int productId)
    {
        var product = await _ctx.Products
            .Include(p => p.CategoryProducts)
            .ThenInclude(cp => cp.Category)
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(p => p.IdProduct == productId);

        if (product == null)
            return null;
        
        var categoryIds = new List<int>();

        if (product.CategoryIds != null)
        {
            foreach (var categoryProduct in product.CategoryIds)
            {
                categoryIds.Add(categoryProduct);
            }
        }
        
        var photos = product.Photos
            .Select(photo => $"{baseUrl}/images/{photo.Url}")
            .ToList();
        
        

        var productDTO = new ProductDTO
        {
            IdProduct = product.IdProduct,
            ProductName = product.ProductName,
            Subtitle = product.Subtitle,
            amountOf = product.amountOf,
            UserId = product.UserId,
            Price = product.Price,
            CategoryIds = categoryIds,
            Photos = photos
        };

        return productDTO;
    }
    
    public async Task AddPhotoToProduct(int productId, IFormFile file, string description)
    {
        // Ścieżka do zapisywania plików
        var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "backend.api", "images");

        // Upewnij się, że folder istnieje
        if (!Directory.Exists(uploadsFolderPath))
        {
            Directory.CreateDirectory(uploadsFolderPath);
        }

        // Generowanie unikalnej nazwy pliku
        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

        // Zapisywanie pliku na serwerze
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Pobierz produkt z bazy danych
        var product = await _ctx.Products.Include(p => p.Photos).FirstOrDefaultAsync(p => p.IdProduct == productId);
        if (product == null)
        {
            throw new NullReferenceException("Product not found.");
        }

        // Dodaj zdjęcie do produktu
        var photo = new Photo
        {
            Url = $"{baseUrl}/images/{uniqueFileName}",
            Description = description,
            ProductId = productId
        };

        product.Photos.Add(photo);

        // Zapisz zmiany w bazie danych
        _ctx.Products.Update(product);
        await _ctx.SaveChangesAsync();
    }

    
    public async Task<Product> CreateProduct(int userId, ProductDTO productDTO, List<IFormFile> photos, string description)
    {
        var categoryIds = new List<int>();
        var productPhotos = new List<Photo>();

        foreach (var categoryId in productDTO.CategoryIds)
        {
            categoryIds.Add(categoryId);
        }

        var product = new Product()
        {
            UserId = userId,
            ProductName = productDTO.ProductName,
            Subtitle = productDTO.Subtitle,
            amountOf = productDTO.amountOf,
            Price = productDTO.Price,
            CategoryIds = categoryIds,
            Photos = productPhotos
        };

        await _ctx.Products.AddAsync(product);
        await _ctx.SaveChangesAsync(); // Save product first to get its ID

        // Add photos to product using AddPhotoToProduct method
        foreach (var photo in photos)
        {
            await AddPhotoToProduct(product.IdProduct, photo, description);
        }

        return product;
    }

    public async Task<Product> SEEDCreateProduct(int userId, ProductDTO productDTO)
    {
        var categoryIds = new List<int>();
        var photos = new List<Photo>();
        
        


        foreach (var categoryId in productDTO.CategoryIds)
        {
            categoryIds.Add(categoryId);
        }


        foreach (var photoUrl in productDTO.Photos)
        {
            photos.Add(new Photo
            {
                Url = $"{baseUrl}/images/{photoUrl}",
                Description = "Photo description"
            });
        }

        var product = new Product()
        {
            UserId = userId,
            ProductName = productDTO.ProductName,
            Subtitle = productDTO.Subtitle,
            amountOf = productDTO.amountOf,
            Price = productDTO.Price,
            CategoryIds = categoryIds,
            Photos = photos
        };

        await _ctx.Products.AddAsync(product);
        await _ctx.SaveChangesAsync();
        return product;
    }


    public async Task UpdateProduct(Product product)
    {
        _ctx.Products.Update(product);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task DeleteProduct(ProductDTO productDTO)
    {
        var product = await _ctx.Products.FindAsync(productDTO.IdProduct);
        if (product != null)
        {
            _ctx.Products.Remove(product);
            await _ctx.SaveChangesAsync();
        }
    }

    
    public async Task<IEnumerable<ProductDTO>> GetUserFavouritesByUserIdAsyncNOW(int userId)
    {
        var favourites = await _ctx.UserFavourites
            .Where(uf => uf.UserId == userId)
            .Include(uf => uf.Product)
            .ThenInclude(p => p.CategoryProducts)
            .ToListAsync();

        var productDTOs = new List<ProductDTO>();
            
        foreach (var favourite in favourites)
        {
            var categoryIds = new List<int>();

            if (favourite.Product.CategoryProducts != null)
            {
                foreach (var categoryProduct in favourite.Product.CategoryIds)
                {
                    categoryIds.Add(categoryProduct);
                }
            }

            var productDTO = new ProductDTO()
            {
                IdProduct = favourite.ProductId,
                ProductName = favourite.Product.ProductName,
                Subtitle = favourite.Product.Subtitle,
                amountOf = favourite.Product.amountOf,
                Price = favourite.Product.Price,
                CategoryIds = categoryIds
            };

            productDTOs.Add(productDTO);
        }

        return productDTOs;
    }

    
    public async Task AddUserFavouriteAsync(UserFavourite userFavourite)
    {
        _ctx.UserFavourites.Add(userFavourite);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task<Favourite?> GetFavouriteByUserIdAndProductIdAsync(int userId, int productId)
    {
        return await _ctx.Favourities
            .Where(f => f.UserFavourites.Any(uf => uf.UserId == userId && uf.ProductId == productId))
            .FirstOrDefaultAsync();
    }
    
    public async Task RestoreOldPriceAndRemoveOnSale(int productId)
    {
        var onSale = await _ctx.OnSales.FirstOrDefaultAsync(os => os.ProductId == productId);
        if (onSale != null)
        {
            var product = await _ctx.Products.FindAsync(productId);
            if (product != null)
            {
                product.Price = onSale.OldPrice;
                _ctx.Products.Update(product);
            }
            _ctx.OnSales.Remove(onSale);
            await _ctx.SaveChangesAsync();
        }
    }

    
    public async Task CreateFavouriteAsync(Favourite favourite)
    {
        _ctx.Favourities.Add(favourite);
        await _ctx.SaveChangesAsync();
    }

    public async Task AddOnSale(int days, int months, int hours, int minutes, decimal newPrice, int productId)
    {
        var existingProduct = await _ctx.Products.FindAsync(productId);
        if (existingProduct == null)
        {
            throw new NullReferenceException("Product not found in the database.");
        }

        var onSale = new OnSale
        {
            OldPrice = existingProduct.Price,
            OnSalePrice = newPrice,
            ProductId = existingProduct.IdProduct,
            ExpirationTime = DateTime.Now.AddDays(days).AddMonths(months).AddHours(hours).AddMinutes(minutes)
        };
        existingProduct.Price = newPrice;
        _ctx.OnSales.Add(onSale);
        _ctx.Products.Update(existingProduct);
        await _ctx.SaveChangesAsync();
    }
    
    public async Task<OnSale?> GetOnSaleByProductId(int productId)
    {
        return await _ctx.OnSales
            .FirstOrDefaultAsync(os => os.ProductId == productId);
    }
    
    public async Task RemoveUserFavouriteAsync(int userId, int productId)
    {
        var userFavourite = await _ctx.UserFavourites
            .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.ProductId == productId);
    
        if (userFavourite != null)
        {
            _ctx.UserFavourites.Remove(userFavourite);
            await _ctx.SaveChangesAsync();
        }
    }

}
