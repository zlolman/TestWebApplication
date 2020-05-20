using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Routing;

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
        public IEnumerable<Vocation> Get()
        {
            try
            {
                return db.Vocations.ToList();

            }
            catch
            {
                return null;
            }
        }

        [HttpGet("{id}")]
        public Vocation Get(int id)
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
        public IActionResult Put(Vocation vocation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Update(vocation);
                    db.SaveChanges();
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
        public IActionResult Delete(int id)
        {
            try
            {
                Vocation vocation = db.Vocations.FirstOrDefault(x => x.id == id);
                if (vocation != null)
                {
                    db.Vocations.Remove(vocation);
                    db.SaveChanges();
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