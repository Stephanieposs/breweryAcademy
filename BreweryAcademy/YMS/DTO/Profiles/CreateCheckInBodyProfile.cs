namespace YMS.DTO.Profiles
{
	public class CreateCheckInBodyProfile : Profile
	{
		public CreateCheckInBodyProfile()
		{
			// Map InvoiceItemDto to InvoiceItem and vice versa
			CreateMap<InvoiceItemDto, InvoiceItem>()
				.ForMember(dest => dest.Invoice, opt => opt.MapFrom(src => new InvoiceItem
				{
					 Quantity = src.Quantity,
					 ProductId = src.ProductId
				})); // Ignore non-matching fields

			CreateMap<InvoiceItem, InvoiceItemDto>();

			// Map InvoiceDto to Invoice
			CreateMap<InvoiceDto, Invoice>()
				.ForMember(dest => dest.InvoiceType, opt => opt.MapFrom(src => Enum.Parse<InvoiceType>(src.InvoiceType))); // String to Enum conversion

			CreateMap<Invoice, InvoiceDto>()
				.ForMember(dest => dest.InvoiceType, opt => opt.MapFrom(src => src.InvoiceType.ToString())); // Enum to String conversion

			// Map CreateCheckInBody to CheckIn
			CreateMap<CreateCheckInBody, CheckIn>()
				.ForMember(dest => dest.InvoiceReferenced, opt => opt.Ignore()); // Handle custom logic if needed

			// Map CheckIn to CreateCheckInResponse
			CreateMap<CheckIn, CreateCheckInResponse>();
		}
	}
}
