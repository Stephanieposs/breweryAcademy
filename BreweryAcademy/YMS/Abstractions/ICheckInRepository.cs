namespace YMS.Abstractions
{
	public interface ICheckInRepository
	{
		Task<CheckIn> CreateCheckIn(CheckIn checkIn);
		
		Task<List<CheckIn>> GetAllCheckIns();
		Task<CheckIn?> GetCheckIn(int id);

	}
}
