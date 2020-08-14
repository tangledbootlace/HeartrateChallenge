using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorHRC.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorHRC.Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly HRCContext _hrcContext;

        public UsersController(HRCContext hrcContext)
        {
            _hrcContext = hrcContext;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<List<User>> Get()
        {
            return await _hrcContext.Users.ToListAsync();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<bool> Create([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                _hrcContext.Add(user);
                try
                {
                    await _hrcContext.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateException)
                {
                    return false;
                }
            }

            return false;
        }

        [HttpGet]
        [Route("Details/{username}")]
        public async Task<User> Details(string username)
        {
            return await _hrcContext.Users.FindAsync(username);
        }

        [HttpPut]
        [Route("Edit/{username}")]
        public async Task<bool> Edit(string username, [FromBody] User user)
        {
            if (username != user.Username)
            {
                return false;
            }

            _hrcContext.Entry(user).State = EntityState.Modified;
            await _hrcContext.SaveChangesAsync();
            return true;
        }

        [HttpDelete]
        [Route("Delete/{username}")]
        public async Task<bool> DeleteConfirmed(string username)
        {
            var user = await _hrcContext.Users.FindAsync(username);
            if (user == null)
            {
                return false;
            }

            _hrcContext.Users.Remove(user);
            await _hrcContext.SaveChangesAsync();
            return true;
        }
    }
}
