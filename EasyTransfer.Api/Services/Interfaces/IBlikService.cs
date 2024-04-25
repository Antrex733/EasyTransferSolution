namespace EasyTransfer.Api.Services.Interfaces
{
    public interface IBlikService
    {
        Task StartConnection(WebSocket webSocket, int? userId);
        //Task<string> SendAndReceiveMessage(WebSocket webSocket, string question);
        Task<string> BlikTransfer(int senderId, string code, decimal amount);
    }
}
