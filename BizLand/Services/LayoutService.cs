using BizLand.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BizLand.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _db;
        public LayoutService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Dictionary<string,string>> GetSettings()
        {
            Dictionary<string,string> settings= await _db.Settings.ToDictionaryAsync(x=>x.Key,x=>x.Value);
            return settings;
        }
    }
}
