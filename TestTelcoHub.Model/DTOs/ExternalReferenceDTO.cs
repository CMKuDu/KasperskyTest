namespace TestTelcoHub.Model.DTOs
{
    public class ExternalReferenceDTO
    {
        public string ExternalReferenceid { get; set; } = string.Empty;// Mã định danh đối tác
        public string ExternalOrderId { get; set; } = string.Empty;// Mã đơn hàng trong Distributor
        public string ExternalLineItemId { get; set; } = string.Empty;// Mã positon trong hệ thôngs của Distributor
    }
}
