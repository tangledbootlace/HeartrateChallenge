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
    public class WorkoutsController : ControllerBase
    {
        private readonly HRCContext _hrcContext;

        public WorkoutsController(HRCContext hrcContext)
        {
            _hrcContext = hrcContext;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<List<Workout>> Get()
        {
            string username = "";
            return await _hrcContext.Workouts.Where(w => w.Username == (username == "" ? w.Username : username)).ToListAsync();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<bool> Create([FromBody] Workout workout)
        {
            if (ModelState.IsValid)
            {
                workout.Id = Guid.NewGuid().ToString();
                _hrcContext.Add(workout);
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
        [Route("Details/{id}")]
        public async Task<Workout> Details(string id)
        {
            return await _hrcContext.Workouts.FindAsync(id);
        }

        [HttpPut]
        [Route("Edit/{id}")]
        public async Task<bool> Edit(string id, [FromBody] Workout workout)
        {
            if (id != workout.Id)
            {
                return false;
            }

            _hrcContext.Entry(workout).State = EntityState.Modified;
            await _hrcContext.SaveChangesAsync();
            return true;
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<bool> DeleteConfirmed(string id)
        {
            var workout = await _hrcContext.Workouts.FindAsync(id);
            if (workout == null)
            {
                return false;
            }

            _hrcContext.Workouts.Remove(workout);
            await _hrcContext.SaveChangesAsync();
            return true;
        }
    }
}
