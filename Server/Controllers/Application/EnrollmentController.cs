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
    public class EnrollmentController : BaseController, iBaseController<Enrollment>
    {

        public EnrollmentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue) //Might need anot
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Enrollment itmEnrollment = await _context.Enrollments.Where(x => x.StudentId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmEnrollment);
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
        [Route("Delete/{PKSchoolId}/{PKSectionId}/{PKStudentId}")]
        public async Task<IActionResult> Delete(int PKSchoolId, int PKSectionId, int PKStudentId)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Enrollment itmEnrollment = await _context.Enrollments.Where(x => x.SchoolId == PKSchoolId && x.SectionId == PKSectionId && x.StudentId == PKStudentId).FirstOrDefaultAsync();
                _context.Remove(itmEnrollment);
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
            List<Enrollment> lstEnrollments = await _context.Enrollments.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lstEnrollments);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Enrollment itmEnrollment = await _context.Enrollments.Where(x => x.StudentId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmEnrollment);
        }
        [HttpGet]
        [Route("Get/{PKSchoolId}/{PKSectionId}/{PKStudentId}")]
        public async Task<IActionResult> Get(int PKSchoolId, int PKSectionId, int PKStudentId)
        {
            Enrollment itmEnrollment = await _context.Enrollments.Where(x => x.SchoolId == PKSchoolId && x.SectionId == PKSectionId && x.StudentId == PKStudentId).FirstOrDefaultAsync();
            return Ok(itmEnrollment);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Enrollment Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _enroll = await _context.Enrollments.Where(x => x.SchoolId == Item.SchoolId && x.SectionId == Item.SectionId && x.StudentId == Item.StudentId).FirstOrDefaultAsync();


                if (_enroll != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Enrollment Already Exists");
                }

                _enroll = new Enrollment();
                _enroll.StudentId = Item.StudentId;
                _enroll.SectionId = Item.SectionId;
                _enroll.EnrollDate = Item.EnrollDate;
                _enroll.FinalGrade = Item.FinalGrade;
                _enroll.SchoolId = Item.SchoolId;

                _context.Enrollments.Add(_enroll);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(Item.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Enrollment Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _enroll = await _context.Enrollments.Where(x => x.SchoolId == Item.SchoolId && x.SectionId == Item.SectionId && x.StudentId == Item.StudentId).FirstOrDefaultAsync();


                if (_enroll != null)
                {
                    await this.Post(Item);
                    return Ok();
                }

                _enroll = new Enrollment();
                _enroll.StudentId = Item.StudentId;
                _enroll.SectionId = Item.SectionId;
                _enroll.EnrollDate = Item.EnrollDate;
                _enroll.FinalGrade = Item.FinalGrade;
                _enroll.SchoolId = Item.SchoolId;

                _context.Enrollments.Update(_enroll);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(Item.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
