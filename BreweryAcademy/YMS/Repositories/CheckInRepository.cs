using YMS.Abstractions;

namespace YMS.Repositories
{
	public class CheckInRepository(DefaultContext context) : ICheckInRepository
	{
		public async Task<CheckIn> CreateCheckIn(CheckIn checkIn)
		{
			context.CheckIns.Add(checkIn);
			await context.SaveChangesAsync();
			return checkIn;
		}

		public async Task<List<CheckIn>> GetAllCheckIns()
		{
			var result = await context.CheckIns.AsNoTracking().Include(e => e.Invoice).Include(e => e.Invoice.Items).ToListAsync();
			return result;
		}

		public async Task<CheckIn?> GetCheckIn(int id)
		{
			var checkIn = await context.CheckIns.Include(e => e.Invoice).Include(e => e.Invoice.Items).AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
			return checkIn;
		}
	}
}
