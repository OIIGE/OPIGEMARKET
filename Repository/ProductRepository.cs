﻿using EliteMart.Data;
using EliteMart.DTOS.Product;
using EliteMart.Helpers;
using EliteMart.Interfaces;
using EliteMart.Model;
using Microsoft.EntityFrameworkCore;

namespace EliteMart.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product productModel)
        {
            await _context.Products.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<Product?> DeleteAsync(int id)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (productModel == null)
            {
                return null;
            }

            _context.Products.Remove(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<List<Product>> GetAllAsync(QueryObject query)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.ProductName))
            {
                products = products.Where(s => s.ProductName.Contains(query.ProductName));
            }

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                products = products.Where(s => s.Category.Contains(query.Category));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("ProductName", StringComparison.OrdinalIgnoreCase))
                {
                    products = query.IsDecending ? products.OrderByDescending(s => s.ProductName) : products.OrderBy(s => s.ProductName);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            return await products.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public Task<Product> GetByNameAsync(Product productModel)
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> UpdateAsync(int id, UpdateProductDto productDto)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Price = productDto.Price;
            existingProduct.Unit = productDto.Unit;
            existingProduct.IsInStock = productDto.IsInStock;

            await _context.SaveChangesAsync();
            return existingProduct;

        }
    }
}