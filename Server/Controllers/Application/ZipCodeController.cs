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
    public class ZipCodeController : BaseController, iBaseController<Zipcode>
    {
        public ZipCodeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(string KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Zipcode itmZipCode = await _context.Zipcodes.Where(x => x.Zip.Equals(KeyValue)).FirstOrDefaultAsync();
                _context.Remove(itmZipCode);
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

        public Task<IActionResult> Delete(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Zipcode> lstZipCodes = await _context.Zipcodes.OrderBy(x => x.Zip).ToListAsync();
            return Ok(lstZipCodes);
        }

        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(string KeyValue)
        {
            Zipcode itmZipCode = await _context.Zipcodes.Where(x => x.Zip.Equals(KeyValue)).FirstOrDefaultAsync();
            return Ok(itmZipCode);
        }

        public Task<IActionResult> Get(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Zipcode Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _zip = await _context.Zipcodes.Where(x => x.Zip == Item.Zip).FirstOrDefaultAsync();


                if (_zip != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "ZipCode Already Exists");
                }

                _zip = new Zipcode();
                _zip.Zip = Item.Zip;
                _zip.City = Item.City;
                _zip.State = Item.State;

                _context.Zipcodes.Add(_zip);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(Item.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Zipcode Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _zip = await _context.Zipcodes.Where(x => x.Zip == Item.Zip).FirstOrDefaultAsync();


                if (_zip != null)
                {
                    await this.Post(Item);
                    return Ok();
                }

                _zip = new Zipcode();
                _zip.Zip = Item.Zip;
                _zip.City = Item.City;
                _zip.State = Item.State;

                _context.Zipcodes.Update(_zip);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(Item.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
