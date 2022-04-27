using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers.Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : BaseController, iBaseController<School>
    {
        public SchoolController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                School itmSchool = await _context.Schools.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmSchool);
                //await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<School> lstSchools = await _context.Schools.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstSchools);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            School itmSchool = await _context.Schools.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmSchool);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] School Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _sch = await _context.Schools.Where(x => x.SchoolId == Item.SchoolId).FirstOrDefaultAsync();


                if (_sch != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "School Already Exists");
                }

                _sch = new School();
                _sch.SchoolId = Item.SchoolId;
                _sch.SchoolName = Item.SchoolName;

                _context.Schools.Add(_sch);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(Item.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] School Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _sch = await _context.Schools.Where(x => x.SchoolId == Item.SchoolId).FirstOrDefaultAsync();


                if (_sch != null)
                {
                    await this.Post(Item);
                    return Ok();
                }

                _sch = new School();
                _sch.SchoolId = Item.SchoolId;
                _sch.SchoolName = Item.SchoolName;

                _context.Schools.Update(_sch);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(Item.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
