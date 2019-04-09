using System.IO;
using Microsoft.ML;
using Microsoft.Extensions.ObjectPool;
using SentimentServices.Interfaces;

namespace SentimentServices.Services
{
    public class SentimentService<TData, TPrediction> : ISentimentService<TData, TPrediction>
        where TData : class
        where TPrediction : class, new()
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _mlModel;
        private readonly ObjectPool<PredictionEngine<TData, TPrediction>> _predictionEnginePool;

        public SentimentService(string mlModelFullPath)
        {
            _mlContext = new MLContext();

            using (var fs = File.OpenRead(mlModelFullPath))
            {
                _mlModel = _mlContext.Model.Load(fs);
            }

            _predictionEnginePool = CreatePredictionEngineObjectPool();
        }

        private ObjectPool<PredictionEngine<TData, TPrediction>> CreatePredictionEngineObjectPool()
        {
            var predEnginePolicy = new PooledPredictionEnginePolicy<TData, TPrediction>(_mlContext, _mlModel);

            return new DefaultObjectPool<PredictionEngine<TData, TPrediction>>(predEnginePolicy);
        }

        public TPrediction Predict(TData dataSample)
        {
            PredictionEngine<TData, TPrediction> predictionEngine = _predictionEnginePool.Get();

            try
            {
                TPrediction prediction = predictionEngine.Predict(dataSample);
                return prediction;
            }
            finally
            {
                _predictionEnginePool.Return(predictionEngine);
            }
        }
    }
}
