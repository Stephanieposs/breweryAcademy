
namespace YMS.Exceptions
{
	public class CheckInNotFoundException:NotFoundException
	{
		public CheckInNotFoundException(int id) : base("CheckIn", id) { }
	}
}
