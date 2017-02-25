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
            return db.Product.Any(p => p.Id == Key);
        }

        protected override void Dispose(bool disposing)
        {
            _db?.Dispose();
            base.Dispose(disposing);
        }
    }
}