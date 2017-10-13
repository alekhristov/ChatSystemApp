using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.OperationContracts
{
    public interface IChatService
    {
        IConnectResponse Connect(IConnectRequest request);

        IChatHistoryResponse GetChatHistory(IChatHistoryRequest request)
        {
            var list<Messages> = ...
            var list<IMessages> mapper.Convert(list<Messages>);
        }
    }

    class ChatHistoryResponse
    {
        IList<IMessages>
    }

    interface IMessages
    {
        public string Message { get; set; }
    }

    public class Message : IMessages
    {
        public string Message { get; set; }
    }
}
