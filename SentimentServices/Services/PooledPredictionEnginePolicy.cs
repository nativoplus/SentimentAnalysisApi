using Microsoft.ML;
using Microsoft.Extensions.ObjectPool;

namespace SentimentServices.Services
{
    public class PooledPredictionEnginePolicy<TData, TPrediction> : IPooledObjectPolicy<PredictionEngine<TData, TPrediction>>
        where TData : class
        where TPrediction : class, new()
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public PooledPredictionEnginePolicy(MLContext mlContext, ITransformer model)
        {
            _mlContext = mlContext;
            _model = model;
        }

        public PredictionEngine<TData, TPrediction> Create()
        {
            var predictionEngine = _model.CreatePredictionEngine<TData, TPrediction>(_mlContext);

            return predictionEngine;
        }

        public bool Return(PredictionEngine<TData, TPrediction> obj)
        {
            if (obj == null)
                return false;

            return true;
        }
    }
}
