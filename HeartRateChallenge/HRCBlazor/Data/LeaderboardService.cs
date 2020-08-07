using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRCDB.Data.HRC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HRCBlazor.Data
{
    public class LeaderboardService
    {
        private readonly HRCContext _context;

        public LeaderboardService(HRCContext context)
        {
            _context = context;
        }

        public async Task<Leaderboard[]> GetLeaderboardsAsync()
        {
            Leaderboard[] data =
                _context.Leaderboard
                .FromSqlRaw($"EXECUTE sp_SelectLeaderboard")
                .ToArray();

            return data;
        }
    }
}
