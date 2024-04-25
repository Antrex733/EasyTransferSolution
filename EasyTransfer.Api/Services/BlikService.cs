
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Net.WebSockets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EasyTransfer.Api.Services
{
    public class BlikService : IBlikService
    {
        private readonly EasyTransferDBContext _context;
        private readonly IUserContextService _userContext;
        private readonly IMemoryCache _memoryCache;

        public BlikService(EasyTransferDBContext context, IUserContextService userContext,
            IMemoryCache memoryCache)
        {
            _context = context;
            _userContext = userContext;
            _memoryCache = memoryCache;
        }
        public async Task StartConnection(WebSocket webSocket, int? userId)
        {
            // Uruchomienie operacji opóźnienia w tle
            /*
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(120));

                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, 
                                            "Connection timed out", CancellationToken.None);
            });*/

            var blikNumber = GenerateBlikNumber((int)userId);

            _memoryCache.Set(blikNumber, webSocket);

            string message = $"Code: {blikNumber} (it will expire in two minutes)";
            await SendQuestionToUser(blikNumber, message);

            await close(webSocket, blikNumber);
        }

        private async Task close(WebSocket webSocket, string code)
        {
            await Task.Delay(TimeSpan.FromSeconds(120));
            if (webSocket.State != WebSocketState.Closed)
            {
                _memoryCache.Remove(code);

                await webSocket.CloseAsync
                    (WebSocketCloseStatus.NormalClosure, "Zamknięcie połączenia. Upłynął czas",
                    CancellationToken.None);
            }
        }

        private string GenerateBlikNumber(int userId)
        {
            Random rnd = new Random();

            var userBliks = _context.Bliks
                .Where(u => u.OwnerId == userId).ToList();

            var activeBlik = userBliks
                .FirstOrDefault(u => IsBlikNumberValid(u.WillExpire));
            
            if (activeBlik != null)
            {
                return activeBlik.Number;
            }

            var blikNumber = rnd.Next(111111, 999999).ToString();

            var blik = new Blik()
            { Number = blikNumber, OwnerId = (int)userId };

            _context.Add(blik);
            _context.SaveChanges();

            return blikNumber;
        }

        private bool IsBlikNumberValid(DateTime blik)
        {
            return blik > DateTime.Now;
        }

        private async Task SendQuestionToUser(string blikNumber, string question)
        {
            WebSocket webSocket;
            if (_memoryCache.TryGetValue(blikNumber, out webSocket))
            { 
                // Konwertuj pytanie na tablicę bajtów
                var questionBytes = Encoding.UTF8.GetBytes(question);

                // Wyślij pytanie do użytkownika przez WebSocket
                await webSocket.SendAsync(new ArraySegment<byte>(questionBytes), 
                                            WebSocketMessageType.Text, true, 
                                            CancellationToken.None);
            }
        }

        public async Task<bool> BlikTransferRequest(WebSocket webSocket, string blikCode, string question)
        {
            if (webSocket != null)
            { 
                await SendQuestionToUser(blikCode, question);

                var answer = await ReceiveAnswerFromUser(blikCode);

                return answer.ToString().ToLower().Trim() == "yes";
            }
            return false;
        }
        private async Task<string> ReceiveAnswerFromUser(string blikCode)
        {
            WebSocket webSocket;
            if (_memoryCache.TryGetValue(blikCode, out webSocket))
            {
                var buffer = new byte[1024];

                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                var answer = Encoding.UTF8.GetString(buffer, 0, result.Count);

                return answer;
            }
            return default;
        }

        public async Task<string> BlikTransfer(int senderId, string code, decimal amount)
        {
            //walidacja danych wejściowych
            var sender = _context.Users.FirstOrDefault(i => i.Id == senderId);

            WebSocket webSocket;
            if (_memoryCache.TryGetValue(code, out webSocket))
                if (webSocket == null || 
                    await BlikTransferRequest(webSocket, code, 
                         $"Do you accept transfers from: " +
                         $"{_context.Users.FirstOrDefault(i => i.Id == senderId).FirstName} for {amount} PLN (yes/no)") == false)
                {
                    throw new Exception("Bad data");
                }

            //przelew
            BlikOperation(senderId, code, amount);

            _memoryCache.Remove(code);
            //zakmnięcie połączenia WebSocket, wiadomość, że przelew się powiódł 
            await webSocket.CloseAsync
                    (WebSocketCloseStatus.NormalClosure, "Blik is done",
                    CancellationToken.None);
            return "Blik has been accepted";
        }
        private void BlikOperation(int recipientId, string code, decimal amount)
        {
            var recipientBankAccount = _context.BankAccounts
                .FirstOrDefault(i => (i.OwnerId == recipientId && i.Currency == Currency.PLN));

            var senderBlik = _context.Bliks
                .FirstOrDefault(i => i.Number == code && i.WillExpire > DateTime.Now);

            if (recipientBankAccount == null || senderBlik == null)
            {
                throw new Exception("Bad data");
            }


            var senderBankAccount = _context.BankAccounts
                .FirstOrDefault(u => u.OwnerId == senderBlik.OwnerId && u.Currency == Currency.PLN);

            if(senderBankAccount == null)
            {
                throw new Exception("Bad data");
            }

            if (senderBankAccount.Balance < amount)
            {
                throw new BadRequestException("Insufficient funds in the account");
            }
            recipientBankAccount.Balance += amount;
            senderBankAccount.Balance -= amount;

            _context.SaveChanges();
        }
    }
}
