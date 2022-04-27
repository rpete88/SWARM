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
    public class GradeTypeWeightController : BaseController, iBaseController<GradeTypeWeight>
    {
        public GradeTypeWeightController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
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
                GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmGradeTypeWeight);
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
        [Route("Delete/{PKSchoolId}/{PKSectionId}/{PKGradeTypeCode}")]
        public async Task<IActionResult> Delete(int PKSchoolId, int PKSectionId, string PKGradeTypeCode)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SchoolId == PKSchoolId && x.SectionId == PKSectionId && x.GradeTypeCode.Equals(PKGradeTypeCode)).FirstOrDefaultAsync();
                _context.Remove(itmGradeTypeWeight);
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
            List<GradeTypeWeight> lstGradeTypeWeights = await _context.GradeTypeWeights.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstGradeTypeWeights);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmGradeTypeWeight);
        }
        [HttpGet]
        [Route("Get/{PKSchoolId}/{PKSectionId}/{PKGradeTypeCode}")]
        public async Task<IActionResult> Get(int PKSchoolId, int PKSectionId, string PKGradeTypeCode)
        {
            GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SchoolId == PKSchoolId && x.SectionId == PKSectionId && x.GradeTypeCode.Equals(PKGradeTypeCode)).FirstOrDefaultAsync();
            return Ok(itmGradeTypeWeight);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _gtw = await _context.GradeTypeWeights.Where(x => x.SchoolId == Item.SchoolId && x.SectionId == Item.SectionId && x.GradeTypeCode == Item.GradeTypeCode).FirstOrDefaultAsync();


                if (_gtw != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade Type Weight Already Exists");
                }

                _gtw = new GradeTypeWeight();
                _gtw.SchoolId = Item.SchoolId;
                _gtw.SectionId = Item.SectionId;
                _gtw.GradeTypeCode = Item.GradeTypeCode;
                _gtw.NumberPerSection = Item.NumberPerSection;
                _gtw.PercentOfFinalGrade = Item.PercentOfFinalGrade;
                _gtw.DropLowest = Item.DropLowest;

                _context.GradeTypeWeights.Add(_gtw);

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
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _gtw = await _context.GradeTypeWeights.Where(x => x.SchoolId == Item.SchoolId && x.SectionId == Item.SectionId && x.GradeTypeCode == Item.GradeTypeCode).FirstOrDefaultAsync();


                if (_gtw != null)
                {
                    await this.Post(Item);
                    return Ok();
                }

                _gtw = new GradeTypeWeight();
                _gtw.SchoolId = Item.SchoolId;
                _gtw.SectionId = Item.SectionId;
                _gtw.GradeTypeCode = Item.GradeTypeCode;
                _gtw.NumberPerSection = Item.NumberPerSection;
                _gtw.PercentOfFinalGrade = Item.PercentOfFinalGrade;
                _gtw.DropLowest = Item.DropLowest;

                _context.GradeTypeWeights.Update(_gtw);

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
