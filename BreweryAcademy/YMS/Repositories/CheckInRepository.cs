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
	}
}
