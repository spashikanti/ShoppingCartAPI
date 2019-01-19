using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.Repositories;
using ShoppingCartApi.Models;
using Microsoft.AspNetCore.Cors;

namespace ShoppingCartApi.Controllers
{
    [Produces("application/json")]
    //[Route("api/UserDetailsApi")]
    [EnableCors("AllowAll")]
    public class ItemController : Controller
    {
        private readonly IDocumentDBRepository<ItemModel> Respository;
        private readonly string collectionId = "ItemDetails";
        public ItemController(IDocumentDBRepository<ItemModel> Respository)
        {
            this.Respository = Respository;
        }

        [ActionName("Index")]
        public IActionResult Index()
        {
            var items = Respository.GetItemsAsync(d => !d.Completed, collectionId);
            return Ok(items);
            //return View(items);
        }


#pragma warning disable 1998
        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync()
        {
            return View();
        }
#pragma warning restore 1998

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("Id,Name,Description,Completed")] ItemModel item)
        {
            if (ModelState.IsValid)
            {
                await Respository.CreateItemAsync(item, "Pass collectionID");
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind("Id,Name,Description,Completed")] ItemModel item)
        {
            if (ModelState.IsValid)
            {
                await Respository.UpdateItemAsync(item.Id, item, collectionId);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id, string collectionId)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ItemModel item = await Respository.GetItemAsync(id, collectionId);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id, string collectionId)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ItemModel item = await Respository.GetItemAsync(id, collectionId);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("Id")] string id)
        {
            await Respository.DeleteItemAsync(id, collectionId);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id, string collectionId)
        {
            ItemModel item = await Respository.GetItemAsync(id, collectionId);
            return View(item);
        }
    }
}