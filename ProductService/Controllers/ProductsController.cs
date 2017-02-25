using ProductService.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace ProductService.Controllers
{
    public class ProductsController : ODataController
    {
        #region Axiliary props and methods
        private ProductContext _db;
        protected ProductContext db
        {
            get
            {
                if (_db == null)
                {
                    _db = new ProductContext();
                }
                return _db;
            }
        }

        private bool ProductExists(int Key)
        {
            return db.Products.Any(p => p.Id == Key);
        }

        protected override void Dispose(bool disposing)
        {
            _db?.Dispose();
            base.Dispose(disposing);
        }
        #endregion

        [EnableQuery]
        public IQueryable<Product> Get()
        {
            return db.Products;
        }

        [EnableQuery]
        public SingleResult<Product> Get([FromODataUri] int key)
        {
            IQueryable<Product> result = db.Products.Where(p => p.Id == key);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return Created(product);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Product> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await db.Products.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            delta.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (ProductExists(key))
                {
                    throw;
                }
                else
                {   //  Product was deleted by another transaction
                    return NotFound();
                }
            }

            return Updated(entity);
        } 

        public async Task<IHttpActionResult> Put([FromODataUri] int key, Product update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.Id)
            {
                return BadRequest($"Requested Id = {key} does not comply with update Id {update.Id}. Rejected.");
            }
            db.Entry(update).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (ProductExists(key))
                {
                    throw;
                }
                else
                {
                    return NotFound();
                }
            }
            return Updated(update);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var product = await db.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}