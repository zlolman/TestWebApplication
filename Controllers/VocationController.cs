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
        //VocationContext vocationDb;
        ApplicationContext Db;
        AddVocationCheckService addService;
        public VocationController(/*VocationContext vocationContext,*/ ApplicationContext employeeContext, AddVocationCheckService service)
        {
            //vocationDb = vocationContext;
            Db = employeeContext;
            addService = service;
            //vocationDb.SaveChanges();
            //Db.SaveChanges();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vocation>>> Get()
        {
            try
            {
                return await Db.Vocations.ToListAsync();

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
                Vocation vocation = Db.Vocations.FirstOrDefault(x => x.id == id);
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
                    if (addService.Check(vocation, Db))//, vocationDb))
                    {
                        Db.Vocations.Add(vocation);
                        await Db.SaveChangesAsync();
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
                    Db.Update(vocation);
                    await Db.SaveChangesAsync();
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
                Vocation vocation = Db.Vocations.FirstOrDefault(x => x.id == id);
                if (vocation != null)
                {
                    Db.Vocations.Remove(vocation);
                    await Db.SaveChangesAsync();
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