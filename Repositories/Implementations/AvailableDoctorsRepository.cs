using HealthBridgeAPI.Data;
using HealthBridgeAPI.Models.Entities;
using HealthBridgeAPI.Repositories.Interfaces;

using HealthBridgeAPI.Data;
using HealthBridgeAPI.Models;
using HealthBridgeAPI.Models.Entities;
using HealthBridgeAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthBridgeAPI.Repositories
{
    public class AvailableDoctorsRepository : IAvailableDoctorsRepository
    {
        private readonly AppDbContext _context;

        public AvailableDoctorsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AvailableDoctor>> GetAvailableDoctorsAsync()
        {
            return await _context.AvailableDoctors.AsNoTracking().ToListAsync();
        }
    }
}