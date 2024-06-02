namespace stela_api.src.App.IService
{
    public interface IPhoneService
    {
        Task<bool> SendMessage(string destinationPhoneNumber, string message);
    }
}