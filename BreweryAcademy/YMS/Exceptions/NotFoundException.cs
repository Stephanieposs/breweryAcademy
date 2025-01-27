namespace YMS.Exceptions
{
	public class NotFoundException:Exception
	{
		public NotFoundException(string message):base(message) { }

		public NotFoundException(string identity, int id) : base($"{identity} ({id}) Not Found") { }
	}
}
