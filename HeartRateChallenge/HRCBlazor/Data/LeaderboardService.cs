using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HRCDB.Data.HRC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HRCBlazor.Data
{
    public class LeaderboardService
    {
        private readonly HRCContext _context;

        public LeaderboardService(IHRCContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Leaderboard>> GetLeaderboardsAsync()
        {
            var db = new HRCContext(null);
            var data = (from a in db.Leaderboards select a);

            return data;
        }
    }
}
