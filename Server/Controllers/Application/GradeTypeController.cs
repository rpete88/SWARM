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
    public class GradeTypeController : BaseController, iBaseController<GradeType>
    {
        public GradeTypeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
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
                GradeType itmGradeType = await _context.GradeTypes.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmGradeType);
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
        [Route("Delete/{PKSchoolId}/{PKGradeTypeCode}")]
        public async Task<IActionResult> Delete(int PKSchoolId, string PKGradeTypeCode)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                GradeType itmGradeType = await _context.GradeTypes.Where(x => x.SchoolId == PKSchoolId && x.GradeTypeCode.Equals(PKGradeTypeCode)).FirstOrDefaultAsync();
                _context.Remove(itmGradeType);
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
            List<GradeType> lstGradeTypes = await _context.GradeTypes.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstGradeTypes);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            GradeType itmGradeType = await _context.GradeTypes.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmGradeType);
        }
        [HttpGet]
        [Route("Get/{PKSchoolId}/{PKGradeTypeCode}")]
        public async Task<IActionResult> Get(int PKSchoolId, string PKGradeTypeCode)
        {
            GradeType itmGradeType = await _context.GradeTypes.Where(x => x.SchoolId == PKSchoolId && x.GradeTypeCode.Equals(PKGradeTypeCode)).FirstOrDefaultAsync();
            return Ok(itmGradeType);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeType Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grdType = await _context.GradeTypes.Where(x => x.SchoolId == Item.SchoolId && x.GradeTypeCode == Item.GradeTypeCode).FirstOrDefaultAsync();


                if (_grdType != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "GradeType Already Exists");
                }

                _grdType = new GradeType();
                _grdType.SchoolId = Item.SchoolId;
                _grdType.GradeTypeCode = Item.GradeTypeCode;
                _grdType.Description = Item.Description;

                _context.GradeTypes.Add(_grdType);

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
        public async Task<IActionResult> Put([FromBody] GradeType Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _grdType = await _context.GradeTypes.Where(x => x.SchoolId == Item.SchoolId && x.GradeTypeCode == Item.GradeTypeCode).FirstOrDefaultAsync();


                if (_grdType != null)
                {
                    await this.Post(Item);
                    return Ok();
                }

                _grdType = new GradeType();
                _grdType.SchoolId = Item.SchoolId;
                _grdType.GradeTypeCode = Item.GradeTypeCode;
                _grdType.Description = Item.Description;

                _context.GradeTypes.Update(_grdType);

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
