using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using TestWebApplication.Data;
using TestWebApplication.Services;
namespace TestWebApplication.Controllers
{
    [ApiController]
    [Route("/vocation")]
    public class VocationController : ControllerBase
    {
        VocationContext vocationDb;
        EmployeeContext employeeDb;
        AddVocationCheckService addService;
        public VocationController(VocationContext vocationContext, EmployeeContext employeeContext, AddVocationCheckService service)
        {
            vocationDb = vocationContext;
            employeeDb = employeeContext;
            addService = service;
            vocationDb.SaveChanges();
            employeeDb.SaveChanges();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vocation>>> Get()
        {
            try
            {
                return await vocationDb.Vocations.ToListAsync();

            }
            catch
            {
                return null;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vocation>> Get(int id)
        {
            try
            {
                Vocation vocation = vocationDb.Vocations.FirstOrDefault(x => x.id == id);
                return vocation;
            }
            catch
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<ActionResult<Vocation>> Post(Vocation vocation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (addService.Check(vocation, employeeDb, vocationDb))
                    {
                        vocationDb.Vocations.Add(vocation);
                        await vocationDb.SaveChangesAsync();
                        return Ok(vocation);
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }
                return BadRequest(ModelState);
            }
            catch
            {
                throw;
            }

        }

        [HttpPut]
        public async Task<ActionResult> Put(Vocation vocation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    vocationDb.Update(vocation);
                    await vocationDb.SaveChangesAsync();
                    return Ok(vocation);
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
                Vocation vocation = vocationDb.Vocations.FirstOrDefault(x => x.id == id);
                if (vocation != null)
                {
                    vocationDb.Vocations.Remove(vocation);
                    await vocationDb.SaveChangesAsync();
                }
                return Ok(vocation);
            }
            catch
            {
                throw;
            }
        }

    }
}