using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApplication.Models;

namespace TestWebApplication.Controllers
{
    [ApiController]
    [Route("/employee")]
    public class EmployeeController : ControllerBase
    {
        ApplicationContext db;
        VocationContext vocationDb;
        public EmployeeController(ApplicationContext context, VocationContext vocationContext)
        {
            db = context;
            vocationDb = vocationContext;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            try
            {
                return await db.Employees.ToListAsync();
            }
            catch
            {
                throw;
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> Get(int id)
        {
            try
            {
                Employee employee = await db.Employees.FindAsync(id);
                return employee;
            }
            catch
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<ActionResult> Post(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Employees.Add(employee);
                    //db.SaveChanges();
                    await db.SaveChangesAsync();
                    return Ok(employee);
                }
                return BadRequest(ModelState);
            }
            catch
            {
                throw;
            }

        }

        [HttpPut]
        public async Task<ActionResult> Put(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Update(employee);
                    await db.SaveChangesAsync();
                    return Ok(employee);
                }
                return BadRequest(ModelState);
            }
            catch
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Employee employee = db.Employees.FirstOrDefault(x => x.id == id);

                if (employee != null)
                {
                    List<Vocation> vocations = new List<Vocation>();
                    vocations.AddRange(vocationDb.Vocations.Where(voc => voc.employeeId == employee.id));
                    db.Employees.Remove(employee);
                    foreach (Vocation voc in vocations)
                    {
                        vocationDb.Vocations.Remove(voc);
                    }
                    await vocationDb.SaveChangesAsync();
                    await db.SaveChangesAsync();

                }
                return Ok(employee);
            }
            catch
            {
                throw;
            }
        }
    }
}