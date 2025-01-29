namespace YMS.DTO.Profiles
{
	public class GetCheckInProfile:Profile
	{
		public GetCheckInProfile() {
			CreateMap<CheckIn, GetCheckIn>().ReverseMap();
		}
	}
}
