using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SentimentMediator.ViewModels;
using SentimentServices.Models;
using SentimentServices.Services;

namespace SentimentMediator
{
    public class SentimentRequestHandler : IRequestHandler<SentimentRequest, SentimentResponse>
    {
        private readonly SentimentService<SourceData, Prediction> _sentimentService;

        public SentimentRequestHandler(SentimentService<SourceData, Prediction> sentimentService)
        {
            _sentimentService = sentimentService;
        }

        public async Task<SentimentResponse> Handle(SentimentRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
                new SentimentResponse
                {
                    Message = request.Message,
                    Score = _sentimentService.Predict(new SourceData { SentimentText = request.Message }).Percentage
                }
            );
        }
    }
}
