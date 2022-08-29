using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DotnetCore_Database;
using DotnetCore_Database.DbEntities;
using System;

namespace DotnetCore_API.Controllers
{

    public class BuggyController : BaseApiController
    {
        private readonly DbConnection _context;
        public BuggyController(DbConnection context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }
        [HttpGet("not-found")]
        public ActionResult<Users> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null)
            {
                return NotFound();
            }
            return Ok(thing);
        }
        [HttpGet("server-error")]
        public string GetServerError()
        {
            var thing = _context.Users.Find(-1);
            var thingToReturn = thing.ToString();
            return thingToReturn;
        }
        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest("this is bad request");
        }
    }
}
