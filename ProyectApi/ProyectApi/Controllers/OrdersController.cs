using ApiRestBilling.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectApi.Data;
using ProyectApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProyectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPurchaseOrdersService _purchaseOrdersService;                     

        public OrdersController(ApplicationDbContext context, IPurchaseOrdersService purchaseOrdersService)
        {

            this._context = context;
            this._purchaseOrdersService = purchaseOrdersService;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            return await _context.Orders.Include(io=>io.OrderItems).ToListAsync();

        }



        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(int id)
        {

            if (_context.Orders == null
)
            {
                return NotFound();
            }

            //var order = await _context.Orders.FindAsync(id);

            var order = await _context.Orders.Include(oi=> oi.OrderItems).FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
            {
                return NotFound();
            }
            return order;
        }



        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders' is null.");
            }

            //recorremos cada detalle de la orden de compra 
            foreach (var detalle in order.OrderItems)
            {
                //Asignar el precio unitario del producto al detalle 
                detalle.UnitPrice = await _purchaseOrdersService.CheckUnitPrice(detalle);

                // Calcular el subtotal 
                detalle.Subtotal = await _purchaseOrdersService.CalculateSubtotalOrderItem(detalle);
            }
            //Asignar el total calculado a la orden de compra (si tienes una propiedad explicita para el total de tu modelo)
            order.TotalAmount = _purchaseOrdersService.CalculateTotalOrderItems((List<OrderItem>) order.OrderItems);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = order.Id }, order);
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {

            if (id != order.Id)
            {
                return BadRequest();
            }
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!OrdersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool OrdersExists(int id)
        {
            return (_context.Suppliers?.Any(o => o.Id == id)).GetValueOrDefault();
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Orders is null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}
