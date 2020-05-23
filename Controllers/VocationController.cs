using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApplication.Data;
using TestWebApplication.Models;
using TestWebApplication.Services;
namespace TestWebApplication.Controllers
{
    [ApiController]
    [Route("/vocation")]
    public class VocationController : ControllerBase
    {
        private readonly ApplicationContext Db;
        private readonly IDataRepository<Vocation> repo;
        AddVocationCheckService addService;

        public VocationController(ApplicationContext employeeContext, AddVocationCheckService service, IDataRepository<Vocation> _repo)
        {
            Db = employeeContext;
            addService = service;
            repo = _repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vocation>>> Get()
        {
            try
            {
                return Ok(await repo.GetAll());
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var vocation = await repo.Get(id);

                if (vocation == null)
                {
                    return NotFound();
                }
                else 
                {
                    return Ok(vocation);
                }                
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
                if (!ModelState.IsValid) 
                {
                    return BadRequest(ModelState);
                }
                if (addService.Check(vocation, Db))
                {
                    repo.Add(vocation);
                    var save = await repo.SaveAsync(vocation);
                    return Ok(vocation);
                }
                else
                {
                    return BadRequest(ModelState);
                }
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
                if (!ModelState.IsValid) 
                {
                    return BadRequest(ModelState);
                }
                repo.Update(vocation);
                var save = await repo.SaveAsync(vocation);
                
                return Ok(vocation);
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

                var vocation = await Db.Vocations.FindAsync(id);

                if (vocation != null)
                {
                    //Db.Vocations.Remove(vocation);
                    repo.Delete(vocation);
                    var save = await repo.SaveAsync(vocation);

                    return Ok(vocation);
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