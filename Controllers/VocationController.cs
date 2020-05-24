using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly IDataRepository<Vocation> repo;
        AddVocationCheckService addService;

        public VocationController(AddVocationCheckService service, IDataRepository<Vocation> _repo)
        {
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
                if (addService.Check(vocation))
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
                if ((DateTimeOffset.Compare(vocation.startDate,DateTimeOffset.MinValue) == 0) 
                    || (DateTimeOffset.Compare(vocation.startDate, DateTimeOffset.MinValue) == 0)
                    || (vocation.employeeId == 0))
                {
                    return BadRequest(ModelState);
                }
                var voc = await repo.Get(vocation.id);
                if (voc != null)
                {
                    repo.Update(vocation);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {                
                var vocation = await repo.Get(id);

                if (vocation != null)
                {
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