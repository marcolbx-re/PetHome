namespace PetHome.Application.DTOs;

public class PaymentGatewayResponse
{
	public bool Success { get; set; }
	public string Message { get; set; }
	public string Reference { get; set; }
	public string ErrorCode { get; set; }
	public Dictionary<string, string> Metadata { get; set; }
}
