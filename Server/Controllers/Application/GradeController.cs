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
    public class GradeController : BaseController, iBaseController<Grade>
    {
        public GradeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
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
                Grade itmGrade = await _context.Grades.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmGrade);
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
        [Route("Delete/{PKSchoolId}/{PKStudentId}/{PKSectionId}/{PKGradeTypeCode}/{PKGradeCodeOccurrence}")]
        public async Task<IActionResult> Delete(int PKSchoolId, int PKStudentId, int PKSectionId, string PKGradeTypeCode, int PKGradeCodeOccurrence)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Grade itmGrade = await _context.Grades.Where(x => x.SchoolId == PKSchoolId && x.StudentId == PKStudentId && x.SectionId == PKSectionId && x.GradeTypeCode.Equals(PKGradeTypeCode) && x.GradeCodeOccurrence == PKGradeCodeOccurrence).FirstOrDefaultAsync();
                _context.Remove(itmGrade);
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
            List<Grade> lstGrades = await _context.Grades.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstGrades);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Grade itmGrade = await _context.Grades.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmGrade);
        }
        [HttpGet]
        [Route("Get/{PKSchoolId}/{PKStudentId}/{PKSectionId}/{PKGradeTypeCode}/{PKGradeCodeOccurrence}")]
        public async Task<IActionResult> Get(int PKSchoolId, int PKStudentId, int PKSectionId, string PKGradeTypeCode, int PKGradeCodeOccurrence)
        {
            Grade itmGrade = await _context.Grades.Where(x => x.SchoolId == PKSchoolId && x.StudentId == PKStudentId && x.SectionId == PKSectionId && x.GradeTypeCode.Equals(PKGradeTypeCode) && x.GradeCodeOccurrence == PKGradeCodeOccurrence).FirstOrDefaultAsync();
            return Ok(itmGrade);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Grade Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grd = await _context.Grades.Where(x => x.SchoolId == Item.SchoolId && x.StudentId == Item.StudentId && x.SectionId == Item.SectionId && x.GradeTypeCode == Item.GradeTypeCode && x.GradeCodeOccurrence == Item.GradeCodeOccurrence).FirstOrDefaultAsync();


                if (_grd != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade Already Exists");
                }

                _grd = new Grade();
                _grd.SchoolId = Item.SchoolId;
                _grd.StudentId = Item.StudentId;
                _grd.SectionId = Item.SectionId;
                _grd.GradeTypeCode = Item.GradeTypeCode;
                _grd.GradeCodeOccurrence = Item.GradeCodeOccurrence;
                _grd.NumericGrade = Item.NumericGrade;
                _grd.Comments = Item.Comments;

                _context.Grades.Add(_grd);

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
        public async Task<IActionResult> Put([FromBody] Grade Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grd = await _context.Grades.Where(x => x.SchoolId == Item.SchoolId && x.StudentId == Item.StudentId && x.SectionId == Item.SectionId && x.GradeTypeCode == Item.GradeTypeCode && x.GradeCodeOccurrence == Item.GradeCodeOccurrence).FirstOrDefaultAsync();


                if (_grd != null)
                {
                    await this.Post(Item);
                    return Ok();
                }

                _grd = new Grade();
                _grd.SchoolId = Item.SchoolId;
                _grd.StudentId = Item.StudentId;
                _grd.SectionId = Item.SectionId;
                _grd.GradeTypeCode = Item.GradeTypeCode;
                _grd.GradeCodeOccurrence = Item.GradeCodeOccurrence;
                _grd.NumericGrade = Item.NumericGrade;
                _grd.Comments = Item.Comments;

                _context.Grades.Update(_grd);

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
