﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        #region Products

        // GET /products
        [HttpGet]
        public async Task<ActionResult<ProductsDto>> GetProductsAsync()
        {
            try
            {
                var allProducts = await _productService.GetAllProducts();
                return allProducts;
            }
            catch (Exception ex)
            {
                // TODO: log message
                return Problem(ex.Message);
            }
        }

        // GET /products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductAsync(Guid id)
        {
            //TODO check what isNew is
            //var product = new Product(id);
            //if (product.IsNew)
            //    throw new Exception();

            //return product;

            var product = await _productService.GetProductById(id);
            if (product is null)
            {
                return NoContent();
            }

            // Handle exception?
            return product;
        }

        // POST /products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProductAsync(AddUpdateProductDto product)
        {
            try
            {
                var newProduct = await _productService.AddProduct(product);

                return CreatedAtAction(nameof(GetProductAsync), new { id = newProduct.Id }, newProduct);
            }
            catch (Exception ex)
            {
                //log error
                //throw server error
                return Problem(ex.Message);
            }
        }

        // PUT /products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, AddUpdateProductDto product)
        {
            try
            {
                await _productService.UpdateProduct(id, product);
                return NoContent();
            }
            catch (ApplicationException appEx)
            {
                return BadRequest(appEx.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // DELETE /products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return NoContent();
            }
            catch (ApplicationException appEx)
            {
                return BadRequest(appEx.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        #endregion


        #region ProductOptions

        // GET /products/{productId}/options
        [HttpGet("{productId}/options")]
        public async Task<ActionResult<ProductOptionsDto>> GetAllOptionsAsync(Guid productId)
        {
            try
            {
                var allProductOptions = await _productService.GetAllProductOptionsForProduct(productId);

                return allProductOptions;
            }
            catch (Exception ex)
            {
                //TODO: log error
                return Problem(ex.Message);
            }
        }

        [HttpGet("{productId}/options/{id}")]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = new ProductOption(id);
            if (option.IsNew)
                throw new Exception();

            return option;
        }

        [HttpPost("{productId}/options")]
        public async Task<ActionResult<ProductOptionDto>> CreateOptionAsync(Guid productId,
            AddUpdateProductOptionDto productOption)
        {
            try
            {
                var newProductOption = await _productService.AddOptionForProduct(productId, productOption);
                //return CreatedAtAction(nameof(GetProductAsync),
                //    new { productId = productId , id = newProductOption.Id }, newProductOption);
                //TODO: create at action

                return Ok();
            }
            catch (ApplicationException appEx)
            {
                //TODO: log 
                return BadRequest(appEx.Message);
            }
            catch (Exception ex)
            {
                //TODO: log
                return Problem(ex.Message);
            }
        }

        [HttpPut("{productId}/options/{id}")]
        public void UpdateOption(Guid id, ProductOption option)
        {
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [HttpDelete("{productId}/options/{id}")]
        public void DeleteOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
        }

        #endregion

    }
}