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
    public class InstructorController : BaseController, iBaseController<Instructor>
    {
        public InstructorController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
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
                Instructor itmInstructor = await _context.Instructors.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmInstructor);
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
        [Route("Delete/{PKSchoolId}/{PKInstructorId}")]
        public async Task<IActionResult> Delete(int PKSchoolId, int PKInstructorId)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Instructor itmInstructor = await _context.Instructors.Where(x => x.SchoolId == PKSchoolId && x.InstructorId == PKInstructorId).FirstOrDefaultAsync();
                _context.Remove(itmInstructor);
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
            List<Instructor> lstInstructors = await _context.Instructors.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstInstructors);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Instructor itmInstructor = await _context.Instructors.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmInstructor);
        }
        [HttpGet]
        [Route("Get/{PKSchoolId}/{PKInstructorId}")]
        public async Task<IActionResult> Get(int PKSchoolId, int PKInstructorId)
        {
            Instructor itmInstructor = await _context.Instructors.Where(x => x.SchoolId == PKSchoolId && x.InstructorId == PKInstructorId).FirstOrDefaultAsync();
            return Ok(itmInstructor);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Instructor Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _inst = await _context.Instructors.Where(x => x.SchoolId == Item.SchoolId && x.InstructorId == Item.InstructorId).FirstOrDefaultAsync();


                if (_inst != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Instructor Already Exists");
                }

                _inst = new Instructor();
                _inst.SchoolId = Item.SchoolId;
                _inst.InstructorId = Item.InstructorId;
                _inst.Salutation = Item.Salutation;
                _inst.FirstName = Item.FirstName;
                _inst.LastName = Item.LastName;
                _inst.StreetAddress = Item.StreetAddress;
                _inst.Zip = Item.Zip;
                _inst.Phone = Item.Phone;

                _context.Instructors.Add(_inst);

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
        public async Task<IActionResult> Put([FromBody] Instructor Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _inst = await _context.Instructors.Where(x => x.SchoolId == Item.SchoolId && x.InstructorId == Item.InstructorId).FirstOrDefaultAsync();


                if (_inst != null)
                {
                    await this.Post(Item);
                    return Ok();
                }

                _inst = new Instructor();
                _inst.SchoolId = Item.SchoolId;
                _inst.InstructorId = Item.InstructorId;
                _inst.Salutation = Item.Salutation;
                _inst.FirstName = Item.FirstName;
                _inst.LastName = Item.LastName;
                _inst.StreetAddress = Item.StreetAddress;
                _inst.Zip = Item.Zip;
                _inst.Phone = Item.Phone;

                _context.Instructors.Update(_inst);

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
