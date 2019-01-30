using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using ShoppingCartApi.Repositories;
using ShoppingCartApi.Models;

namespace ShoppingCartApi.Controllers
{
    [Produces("application/json")]
    [Route("api/ProductApi")]
    [EnableCors("AllowAll")]
    public class ProductController : Controller
    {
        private readonly IDocumentDBRepository<ProductModel> _repo;
        private readonly string collectionId = "ProductDetails";

        public ProductController(IDocumentDBRepository<ProductModel> r)
        {
            _repo = r;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _repo.GetItemsAsync(collectionId);
            return Ok(products);
        }

        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _repo.GetItemAsync(id, collectionId);
            return Ok();
        }

        [Route("CreateProduct")]
        public IActionResult Post([FromBody]ProductModel model)
        {
            model.createdDate = System.DateTime.Now.ToShortDateString();
            model.modifiedDate = System.DateTime.Now.ToShortDateString();
            model.createdBy = HttpContext.User.Identity.Name;
            model.modifiedBy = HttpContext.User.Identity.Name;
            var product = _repo.CreateItemAsync(model, collectionId).Result;
            return Ok(product);
        }
        [Route("UpdateProduct/{id}")]
        public IActionResult Put(string id, [FromBody]ProductModel per)
        {
            var product = _repo.UpdateItemAsync(id, per, collectionId);
            return Ok(product.Result);
        }
        [Route("DeleteProduct/{id}")]
        public IActionResult Delete(string id)
        {
            var res = _repo.DeleteItemAsync(id, collectionId);
            return Ok(res.Status);
        }
    }
}