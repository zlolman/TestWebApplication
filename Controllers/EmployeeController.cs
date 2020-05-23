using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApplication.Data;
using TestWebApplication.Models;

namespace TestWebApplication.Controllers
{
    [ApiController]
    [Route("/employee")]
    public class EmployeeController : ControllerBase
    {
        ApplicationContext Db;
        public EmployeeController(ApplicationContext context)
        {
            Db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            try
            {
                return await Db.Employees.ToListAsync();
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
                Employee employee = await Db.Employees.FindAsync(id);
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
                    Db.Employees.Add(employee);
                    await Db.SaveChangesAsync();
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
                    Db.Update(employee);
                    await Db.SaveChangesAsync();
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
                Employee employee = Db.Employees.FirstOrDefault(x => x.id == id);

                if (employee != null)
                {
                    List<Vocation> vocations = new List<Vocation>();
                    vocations.AddRange(Db.Vocations.Where(voc => voc.employeeId == employee.id));
                    Db.Employees.Remove(employee);
                    foreach (Vocation voc in vocations)
                    {
                        Db.Vocations.Remove(voc);
                    }
                    await Db.SaveChangesAsync();
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