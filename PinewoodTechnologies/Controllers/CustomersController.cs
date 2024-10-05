using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinewoodTechnologies.Code;
using PinewoodTechnologies.Models;

namespace PinewoodTechnologies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private PineTechContext _context;
        // Automapper is used to convert data from the ViewModel to the database model and vice versa
        private IMapper mapper;

        public CustomersController()
        {
            _context = new PineTechContext();
            mapper = new Automapper().configuration.CreateMapper();
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ViewCustomer>> GetCustomer([FromRoute] int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return mapper.Map<ViewCustomer>(customer);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // The validation attribute is a custom attribute that allows the use of model based validation to return back errors
        // about invalid or missing data
        [HttpPut("{id}")]
        [Validation]
        public async Task<IActionResult> PutCustomer(int id, ViewCustomer customer)
        {
            Customer cust = mapper.Map<Customer>(customer);
            if (id != cust.Id)
            {
                return BadRequest();
            }

            _context.Entry(cust).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Validation]
        public async Task<ActionResult<ViewCustomer>> PostCustomer(ViewCustomer customer)
        {
            var cust = mapper.Map<Customer>(customer);
            _context.Customers.Add(cust);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = cust.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
