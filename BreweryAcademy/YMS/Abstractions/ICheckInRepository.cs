namespace YMS.Abstractions
{
	public interface ICheckInRepository
	{
		Task<CheckIn> CreateCheckIn(CheckIn checkIn);

	}
}
