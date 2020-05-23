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
        private readonly ApplicationContext Db;
        private readonly IDataRepository<Employee> repo;
        public EmployeeController(ApplicationContext context, IDataRepository<Employee> _repo)
        {
            Db = context;
            repo = _repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            try
            {
                return Ok(await repo.GetAll());
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                };

                var employee = await repo.Get(id);

                if (employee == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(employee);
                }
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                repo.Add(employee);
                var save = await repo.SaveAsync(employee);

                return Ok(employee);
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                repo.Update(employee);
                var save = await repo.SaveAsync(employee);

                return Ok(employee);
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var employee = await Db.Employees.FindAsync(id);

                if (employee != null)
                {
                    List<Vocation> vocations = new List<Vocation>();
                    vocations.AddRange(Db.Vocations.Where(voc => voc.employeeId == employee.id));
                    repo.Delete(employee);

                    foreach (Vocation voc in vocations)
                    {
                        Db.Vocations.Remove(voc);
                    }

                    var save = await repo.SaveAsync(employee);

                    return Ok(employee);
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}