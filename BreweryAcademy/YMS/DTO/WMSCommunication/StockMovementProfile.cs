namespace YMS.DTO.WMSCommunication
{
	public class StockMovementProfile:Profile
	{
		public StockMovementProfile() {

			CreateMap<InvoiceItemDto, Item>().ForMember(x => x.Id, src => src.MapFrom(x => x.ProductId)).ReverseMap();
			CreateMap<InvoiceDto, StockMovement>()
				.ConstructUsing((src, context) => new StockMovement(
					src.InvoiceId,
					src.InvoiceType,
					context.Mapper.Map<List<Item>>(src.Items) // Mapeamento manual para converter a lista
				));
		}

		// Método auxiliar para mapear uma lista de InvoiceItemDto para uma lista de Item
		
	}
}
