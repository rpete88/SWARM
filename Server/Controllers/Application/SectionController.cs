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
    public class SectionController : BaseController, iBaseController<Section>
    {
        public SectionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
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
                Section itmSection = await _context.Sections.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
                _context.Remove(itmSection);
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
        [Route("Delete/{PKSchoolId}/{PKSectionId}")]
        public async Task<IActionResult> Delete(int PKSchoolId, int PKSectionId)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Section itmSection = await _context.Sections.Where(x => x.SchoolId == PKSchoolId && x.SectionId == PKSectionId).FirstOrDefaultAsync();
                _context.Remove(itmSection);
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
            List<Section> lstSections = await _context.Sections.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstSections);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Section itmSection = await _context.Sections.Where(x => x.SchoolId == KeyValue).FirstOrDefaultAsync();
            return Ok(itmSection);
        }
        [HttpGet]
        [Route("Get/{PKSchoolId}/{PKSectionId}")]
        public async Task<IActionResult> Get(int PKSchoolId, int PKSectionId)
        {
            Section itmSection = await _context.Sections.Where(x => x.SchoolId == PKSchoolId && x.SectionId == PKSectionId).FirstOrDefaultAsync();
            return Ok(itmSection);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Section Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _sect = await _context.Sections.Where(x => x.SchoolId == Item.SchoolId && x.SectionId == Item.SectionId).FirstOrDefaultAsync();


                if (_sect != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Section Already Exists");
                }

                _sect = new Section();
                _sect.SectionId = Item.SectionId;
                _sect.CourseNo = Item.CourseNo;
                _sect.SectionNo = Item.SectionNo;
                _sect.StartDateTime = Item.StartDateTime;
                _sect.Location = Item.Location;
                _sect.InstructorId = Item.InstructorId;
                _sect.Capacity = Item.Capacity;
                _sect.SchoolId = Item.SchoolId;

                _context.Sections.Add(_sect);

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
        public async Task<IActionResult> Put([FromBody] Section Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _sect = await _context.Sections.Where(x => x.SchoolId == Item.SchoolId && x.SectionId == Item.SectionId).FirstOrDefaultAsync();


                if (_sect != null)
                {
                    await this.Post(Item);
                    return Ok();
                }

                _sect = new Section();
                _sect.SectionId = Item.SectionId;
                _sect.CourseNo = Item.CourseNo;
                _sect.SectionNo = Item.SectionNo;
                _sect.StartDateTime = Item.StartDateTime;
                _sect.Location = Item.Location;
                _sect.InstructorId = Item.InstructorId;
                _sect.Capacity = Item.Capacity;
                _sect.SchoolId = Item.SchoolId;

                _context.Sections.Update(_sect);

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
