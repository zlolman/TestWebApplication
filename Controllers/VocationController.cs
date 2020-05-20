using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace TestWebApplication.Controllers
{
    [ApiController]
    [Route("/vocation")]
    public class VocationController : ControllerBase
    {
        VocationContext db;
        ApplicationContext applicationContext;
        public VocationController(VocationContext context, ApplicationContext employeeDb)
        {
            db = context;
            applicationContext = employeeDb;
            db.SaveChanges();
            applicationContext.SaveChanges();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vocation>>> Get()
        {
            try
            {
                return await db.Vocations.ToListAsync();

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
                Vocation vocation = db.Vocations.FirstOrDefault(x => x.id == id);
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
                    if (AddVocationCheck.Check(vocation, applicationContext, db))
                    {
                        db.Vocations.Add(vocation);
                        await db.SaveChangesAsync();
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
                    db.Update(vocation);
                    await db.SaveChangesAsync();
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
                Vocation vocation = db.Vocations.FirstOrDefault(x => x.id == id);
                if (vocation != null)
                {
                    db.Vocations.Remove(vocation);
                    await db.SaveChangesAsync();
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