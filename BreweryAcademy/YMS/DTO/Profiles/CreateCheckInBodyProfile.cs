namespace YMS.DTO.Profiles
{
	public class CreateCheckInBodyProfile : Profile
	{
		public CreateCheckInBodyProfile()
		{
            // Map InvoiceItemDto to InvoiceItem
            CreateMap<InvoiceItemDto, InvoiceItem>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.InvoiceId, opt => opt.Ignore()); // Ignore InvoiceId if set separately

            // Map InvoiceItem to InvoiceItemDto
            CreateMap<InvoiceItem, InvoiceItemDto>();

            /*
            // Map InvoiceDto to Invoice
            CreateMap<InvoiceDto, Invoice>()
                .ForMember(dest => dest.InvoiceType, opt => opt.MapFrom(src =>
                    Enum.TryParse<InvoiceType>(src.InvoiceType, true, out var invoiceType) ? invoiceType : InvoiceType.None)) // Handle invalid or null strings gracefully
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignore Id, as it's usually auto-generated
            */

            CreateMap<InvoiceDto, Invoice>()
    .ForMember(dest => dest.InvoiceType, opt => opt.MapFrom(src => EnumHelper.ParseInvoiceType(src.InvoiceType))) // Usando a função auxiliar
    .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignore Id, como ele é geralmente gerado pelo banco de dados


            // Map Invoice to InvoiceDto
            CreateMap<Invoice, InvoiceDto>()
                .ForMember(dest => dest.InvoiceType, opt => opt.MapFrom(src => src.InvoiceType.ToString())); // Enum to String conversion

            // Map CreateCheckInBody to CheckIn
            CreateMap<CreateCheckInBody, CheckIn>();

            // Map CheckIn to CreateCheckInResponse
            CreateMap<CheckIn, CreateCheckInResponse>().ForMember(x => x.Id, src => src.MapFrom(x => x.Id));
        }

        public static class EnumHelper
        {
            public static InvoiceType ParseInvoiceType(string invoiceType)
            {
                return Enum.TryParse<InvoiceType>(invoiceType, true, out var result) ? result : InvoiceType.None;
            }
        }
    }
}
