using System.Threading.Tasks;

namespace SentimentInterfaces.SentimentService
{
    public interface ISentimentService<TData, TPrediction>
    {
        Task<TPrediction> PredictAsync(TData dataSample);
    }
}
