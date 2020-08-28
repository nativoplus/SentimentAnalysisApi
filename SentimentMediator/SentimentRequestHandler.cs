using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SentimentInterfaces.SentimentService;
using SentimentViewModels.SentimentMediator;
using SentimentViewModels.SentimentService;

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
            var prediction = await _sentimentService.PredictAsync(new SourceData { SentimentText = request?.Message ?? string.Empty });

            return new SentimentResponse
            {
                Status = "OK",
                Code = (int)HttpStatusCode.OK,
                Result = new SentimentResult
                {
                    SentimentText = request.Message,
                    Score = prediction?.Percentage ?? 0
                }
            };
        }
    }
}
