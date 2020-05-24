using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
        private readonly IDataRepository<Employee> employeeRepo;
        private readonly IDataRepository<Vocation> vocationRepo;
        public EmployeeController(IDataRepository<Employee> _repo, IDataRepository<Vocation> _repoVoc)
        {
            employeeRepo = _repo;
            vocationRepo = _repoVoc;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            try
            {
                return Ok(await employeeRepo.GetAll());
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
                var employee = await employeeRepo.Get(id);

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
                if ((string.IsNullOrEmpty(employee.name)) || (string.IsNullOrEmpty(employee.position)))
                {
                    return BadRequest(ModelState);
                }

                employeeRepo.Add(employee);
                var save = await employeeRepo.SaveAsync(employee);

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
                if ((string.IsNullOrEmpty(employee.name)) || (string.IsNullOrEmpty(employee.position)))
                {
                    return BadRequest(ModelState);
                }
                employeeRepo.Update(employee);
                var save = await employeeRepo.SaveAsync(employee);

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
                var employee = await employeeRepo.Get(id);

                if (employee != null)
                {                    
                    IEnumerable<Vocation> vocations = await vocationRepo.GetAll();
                    vocations = vocations.Where(voc => voc.employeeId == employee.id);
                    employeeRepo.Delete(employee);

                    foreach (var voc in vocations)
                    {
                        vocationRepo.Delete(voc);
                    }

                    var save = await employeeRepo.SaveAsync(employee);

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