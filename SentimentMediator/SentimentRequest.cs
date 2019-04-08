using MediatR;
using SentimentMediator.ViewModels;

namespace SentimentMediator
{
    public class SentimentRequest : IRequest<SentimentResponse>
    {
        public string Message { get; set; }
    }
}
