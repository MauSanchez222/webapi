using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using Microsoft.AspNetCore.Cors;


namespace WebAPI.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public readonly ApidbContext _dbContext;

        public ProductoController(ApidbContext _context)
        {
            _dbContext = _context;
        }

        [HttpGet]
        [Route("Productos")]

        public IActionResult Get()
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                productos = _dbContext.Productos.Include(x => x.objCategoria).ToList();
                return StatusCode(StatusCodes.Status200OK, productos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet]
        [Route("Obtener/{idProducto:int}")]

        public IActionResult GetIndividual(int idProducto)
        {
            Producto producto = _dbContext.Productos.Find(idProducto);

            if (producto == null)
            {
                return BadRequest();
            }

            try {
                producto = _dbContext.Productos.Include(x => x.objCategoria).Where(p => p.IdProducto == idProducto).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, producto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Cargar")]

        public IActionResult Post([FromBody] Producto producto)
        {

            try
            {
                _dbContext.Productos.Add(producto);
                _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Editar")]

        public IActionResult Edit([FromBody] Producto producto)
        {
            Producto prod = _dbContext.Productos.Find(producto.IdProducto);

            if (producto == null)
            {
                return BadRequest();
            }

            try
            {
                prod.CodigoBarra = producto.CodigoBarra is null ? prod.CodigoBarra : producto.CodigoBarra;
                prod.Descripcion = producto.Descripcion is null ? prod.Descripcion : producto.Descripcion;
                prod.Marca = producto.Marca is null ? prod.Marca : producto.Marca;
                prod.IdCategoria = producto.IdCategoria is null ? prod.IdCategoria : producto.IdCategoria;
                prod.Precio = producto.Precio is null ? prod.Precio : producto.Precio;
                _dbContext.Productos.Update(prod);
                _dbContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]

        public IActionResult Delete(int idProducto)
        {
            Producto prod = _dbContext.Productos.Find(idProducto);

            if (prod == null)
            {
                return BadRequest();
            }

            try
            {
                _dbContext.Productos.Remove(prod);
                _dbContext.SaveChanges();
                return (StatusCode(StatusCodes.Status200OK, new { mensaje = "a casa" })) ;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
