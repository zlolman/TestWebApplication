using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApplication.Models;
using TestWebApplication.Data;

namespace TestWebApplication.Controllers
{
    [ApiController]
    [Route("/employee")]
    public class EmployeeController : ControllerBase
    {
        EmployeeContext employeeDb;
        VocationContext vocationDb;
        public EmployeeController(EmployeeContext context, VocationContext vocationContext)
        {
            employeeDb = context;
            vocationDb = vocationContext;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            try
            {
                return await employeeDb.Employees.ToListAsync();
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
                Employee employee = await employeeDb.Employees.FindAsync(id);
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
                    employeeDb.Employees.Add(employee);
                    //db.SaveChanges();
                    await employeeDb.SaveChangesAsync();
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
                    employeeDb.Update(employee);
                    await employeeDb.SaveChangesAsync();
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
                Employee employee = employeeDb.Employees.FirstOrDefault(x => x.id == id);

                if (employee != null)
                {
                    List<Vocation> vocations = new List<Vocation>();
                    vocations.AddRange(vocationDb.Vocations.Where(voc => voc.employeeId == employee.id));
                    employeeDb.Employees.Remove(employee);
                    foreach (Vocation voc in vocations)
                    {
                        vocationDb.Vocations.Remove(voc);
                    }
                    await vocationDb.SaveChangesAsync();
                    await employeeDb.SaveChangesAsync();

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