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
    public class GradeConversionController : BaseController, iBaseController<GradeConversion>
    {
        public GradeConversionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
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
                GradeConversion itmGradeConversion = await _context.GradeConversions.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmGradeConversion);
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
        [Route("Delete/{PKSchoolId}/{PKCourseNo}")]
        public async Task<IActionResult> Delete(int PKSchoolId, string PKLetterGrade)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                GradeConversion itmGradeConversion = await _context.GradeConversions.Where(x => x.SchoolId == PKSchoolId && x.LetterGrade.Equals(PKLetterGrade)).FirstOrDefaultAsync();
                _context.Remove(itmGradeConversion);
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
            List<GradeConversion> lstGradeConversions = await _context.GradeConversions.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstGradeConversions);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmGradeConversion);
        }
        [HttpGet]
        [Route("Get/{PKSchoolId}/{PKCourseNo}")]
        public async Task<IActionResult> Get(int PKSchoolId, string PKLetterGrade)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions.Where(x => x.SchoolId == PKSchoolId && x.LetterGrade.Equals(PKLetterGrade)).FirstOrDefaultAsync();
            return Ok(itmGradeConversion);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeConversion Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grdConver = await _context.GradeConversions.Where(x => x.SchoolId == Item.SchoolId && x.LetterGrade == Item.LetterGrade).FirstOrDefaultAsync();


                if (_grdConver != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "GradeConversion Already Exists");
                }

                _grdConver = new GradeConversion();
                _grdConver.SchoolId = Item.SchoolId;
                _grdConver.LetterGrade = Item.LetterGrade;
                _grdConver.GradePoint = Item.GradePoint;
                _grdConver.MaxGrade = Item.MaxGrade;
                _grdConver.MinGrade = Item.MinGrade;

                _context.GradeConversions.Add(_grdConver);

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
        public async Task<IActionResult> Put([FromBody] GradeConversion Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grdConver = await _context.GradeConversions.Where(x => x.SchoolId == Item.SchoolId && x.LetterGrade == Item.LetterGrade).FirstOrDefaultAsync();


                if (_grdConver != null)
                {
                    await this.Post(Item);
                    return Ok();
                }

                _grdConver = new GradeConversion();
                _grdConver.SchoolId = Item.SchoolId;
                _grdConver.LetterGrade = Item.LetterGrade;
                _grdConver.GradePoint = Item.GradePoint;
                _grdConver.MaxGrade = Item.MaxGrade;
                _grdConver.MinGrade = Item.MinGrade;

                _context.GradeConversions.Update(_grdConver);

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
