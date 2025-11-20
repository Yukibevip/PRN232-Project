using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public static class AccusationDAO
    {
        public static async Task<IEnumerable<Accusation>> GetAll()
        {
            using var context = new CallioTestContext();
            return await context.Accusations.Include(a => a.Accused).Include(a => a.Reported).ToListAsync();
        }

        public static async Task<Accusation> Get(int id)
        {
            using var context = new CallioTestContext();
            return await context.Accusations.FindAsync(id);
        }

        public static async Task<bool> Add(Accusation accusation)
        {
            using var context = new CallioTestContext();
            await context.Accusations.AddAsync(accusation);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public static async Task<bool> Update(Accusation accusation)
        {
            using var context = new CallioTestContext();
            context.Accusations.Update(accusation);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public static async Task<bool> Delete(int id)
        {
            using var context = new CallioTestContext();
            var accusation = await Get(id);
            if (accusation == null)
                return false;

            context.Accusations.Remove(accusation);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }
    }
}
