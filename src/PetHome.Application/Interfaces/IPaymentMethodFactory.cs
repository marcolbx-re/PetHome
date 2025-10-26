namespace PetHome.Application.Interfaces;

public interface IPaymentMethodFactory
{
	IPaymentMethod Create(string paymentType);
}
