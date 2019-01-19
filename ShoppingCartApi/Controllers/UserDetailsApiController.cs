using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartApi.Models;
using ShoppingCartApi.Repositories;

namespace ShoppingCartApi.Controllers
{
    [Produces("application/json")]
    [Route("api/UserDetailsApi")]
    [EnableCors("AllowAll")]
    public class UserDetailsApiController : Controller
    {
        //IDbCollectionOperationsRepository<UserDetailsModel, string> _repo;


        private readonly IDocumentDBRepository<UserDetailsModel> _repo;
        private readonly string collectionId = "UserDetails";
        public UserDetailsApiController(IDocumentDBRepository<UserDetailsModel> r)
        {
            _repo = r;
        }
        [Route("User/All")]
        public IActionResult Get()
        {
            var users = _repo.GetItemsAsync(collectionId);
            return Ok(users);
        }
        [Route("User/{id}")]
        public IActionResult Get(string id)
        {
            var user = _repo.GetItemAsync(id, collectionId).Result;
            return Ok(user);
        }
        [Route("User/Create")]
        public IActionResult Post([FromBody]UserDetailsModel per)
        {
            per.createdDate = System.DateTime.Now.ToShortDateString();
            per.modifiedDate = System.DateTime.Now.ToShortDateString();
            per.createdBy = HttpContext.User.Identity.Name;
            per.modifiedBy = HttpContext.User.Identity.Name;
            var user = _repo.CreateItemAsync(per, collectionId).Result;
            return Ok(user);
        }
        //[Route("User/Update/{id}")]
        //public IActionResult Put(string id, [FromBody]UserDetailsModel per)
        //{
        //    var user = _repo.UpdateDocumentFromCollection(id, per);
        //    return Ok(user.Result);
        //}
        //[Route("User/Delete/{id}")]
        //public IActionResult Delete(string id)
        //{
        //    var res = _repo.DeleteDocumentFromCollectionAsync(id);
        //    return Ok(res.Status);
        //}
    }
}