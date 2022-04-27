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
    public class SchoolUserController : BaseController, iBaseController<SchoolUser>
    {
        public SchoolUserController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
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
                SchoolUser itmSchoolUser = await _context.SchoolUsers.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmSchoolUser);
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
        [HttpDelete]
        [Route("Delete/{PKSchoolId}/{PKUserName}")]
        public async Task<IActionResult> Delete(int PKSchoolId, int PKUserName)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                SchoolUser itmSchoolUser = await _context.SchoolUsers.Where(x => x.SchoolId == PKSchoolId && x.UserName.Equals(PKUserName)).FirstOrDefaultAsync();
                _context.Remove(itmSchoolUser);
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
            List<SchoolUser> lstSchoolUsers = await _context.SchoolUsers.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstSchoolUsers);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            SchoolUser itmSchoolUser = await _context.SchoolUsers.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmSchoolUser);
        }
        [HttpGet]
        [Route("Get/{PKSchoolId}/{PKUserName}")]
        public async Task<IActionResult> Get(int PKSchoolId, int PKUserName)
        {
            SchoolUser itmSchoolUser = await _context.SchoolUsers.Where(x => x.SchoolId == PKSchoolId && x.UserName.Equals(PKUserName)).FirstOrDefaultAsync();
            return Ok(itmSchoolUser);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SchoolUser Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _schuser = await _context.SchoolUsers.Where(x => x.SchoolId == Item.SchoolId && x.UserName == Item.UserName).FirstOrDefaultAsync();


                if (_schuser != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "SchoolUser Already Exists");
                }

                _schuser = new SchoolUser();
                _schuser.SchoolId = Item.SchoolId;
                _schuser.UserName = Item.UserName;

                _context.SchoolUsers.Add(_schuser);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(Item.UserName);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SchoolUser Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _schuser = await _context.SchoolUsers.Where(x => x.SchoolId == Item.SchoolId && x.UserName == Item.UserName).FirstOrDefaultAsync();


                if (_schuser != null)
                {
                    await this.Post(Item);
                    return Ok();
                }

                _schuser = new SchoolUser();
                _schuser.SchoolId = Item.SchoolId;
                _schuser.UserName = Item.UserName;


                _context.SchoolUsers.Update(_schuser);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(Item.UserName);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
