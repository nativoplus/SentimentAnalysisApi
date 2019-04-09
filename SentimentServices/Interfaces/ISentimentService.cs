namespace SentimentServices.Interfaces
{
    public interface ISentimentService<TData, TPrediction>
    {
        TPrediction Predict(TData dataSample);
    }
}
