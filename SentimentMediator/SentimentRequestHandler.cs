using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SentimentMediator.ViewModels;
using SentimentServices.Interfaces;
using SentimentServices.Models;

namespace SentimentMediator
{
    public class SentimentRequestHandler : IRequestHandler<SentimentRequest, SentimentResponse>
    {
        private readonly ISentimentService<SourceData, Prediction> _sentimentService;

        public SentimentRequestHandler(ISentimentService<SourceData, Prediction> sentimentService)
        {
            _sentimentService = sentimentService;
        }

        public async Task<SentimentResponse> Handle(SentimentRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
                new SentimentResponse
                {
                    Status = "OK",
                    Code = (int)HttpStatusCode.OK,
                    Result = new SentimentResult
                    {
                        SentimentText = request.Message,
                        Score = _sentimentService.Predict(new SourceData { SentimentText = request.Message }).Percentage
                    }
                }
            );
        }
    }
}
