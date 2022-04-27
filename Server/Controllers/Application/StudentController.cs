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
    public class StudentController : BaseController, iBaseController<Student>
    {
        public StudentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
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
                Student itmStudent = await _context.Students.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmStudent);
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
        [Route("Delete/{PKSchoolId}/{PKStudentId}")]
        public async Task<IActionResult> Delete(int PKSchoolId, int PKStudentId)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Student itmStudent = await _context.Students.Where(x => x.SchoolId == PKSchoolId && x.StudentId == PKStudentId).FirstOrDefaultAsync();
                _context.Remove(itmStudent);
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
            List<Student> lstStudent = await _context.Students.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstStudent);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Student itmStudent = await _context.Students.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmStudent);
        }
        [HttpGet]
        [Route("Get/{PKSchoolId}/{PKStudentId}")]
        public async Task<IActionResult> Get(int PKSchoolId, int PKStudentId)
        {
            Student itmStudent = await _context.Students.Where(x => x.SchoolId == PKSchoolId && x.StudentId == PKStudentId).FirstOrDefaultAsync();
            return Ok(itmStudent);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _stu = await _context.Students.Where(x => x.SchoolId == Item.SchoolId && x.StudentId == Item.StudentId).FirstOrDefaultAsync();


                if (_stu != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Student Already Exists");
                }

                _stu = new Student();
                _stu.StudentId = Item.StudentId;
                _stu.Salutation = Item.Salutation;
                _stu.FirstName = Item.FirstName;
                _stu.LastName = Item.LastName;
                _stu.StreetAddress = Item.StreetAddress;
                _stu.Zip = Item.Zip;
                _stu.Phone = Item.Phone;
                _stu.Employer = Item.Employer;
                _stu.RegistrationDate = Item.RegistrationDate;
                _stu.SchoolId = Item.SchoolId;

                _context.Students.Add(_stu);

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
        public async Task<IActionResult> Put([FromBody] Student Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _stu = await _context.Students.Where(x => x.SchoolId == Item.SchoolId && x.StudentId == Item.StudentId).FirstOrDefaultAsync();


                if (_stu != null)
                {
                    await this.Post(Item);
                    return Ok();
                }

                _stu = new Student();
                _stu.StudentId = Item.StudentId;
                _stu.Salutation = Item.Salutation;
                _stu.FirstName = Item.FirstName;
                _stu.LastName = Item.LastName;
                _stu.StreetAddress = Item.StreetAddress;
                _stu.Zip = Item.Zip;
                _stu.Phone = Item.Phone;
                _stu.Employer = Item.Employer;
                _stu.RegistrationDate = Item.RegistrationDate;
                _stu.SchoolId = Item.SchoolId;

                _context.Students.Update(_stu);

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
