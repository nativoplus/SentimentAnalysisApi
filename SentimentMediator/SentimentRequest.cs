using MediatR;
using SentimentViewModels.SentimentMediator;

namespace SentimentMediator
{
    public class SentimentRequest : IRequest<SentimentResponse>
    {
        public string Message { get; set; }
    }
}
