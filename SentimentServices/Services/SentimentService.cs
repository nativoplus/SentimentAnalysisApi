using System.IO;
using Microsoft.ML;
using Microsoft.Extensions.ObjectPool;
using Microsoft.AspNetCore.Hosting;

namespace SentimentServices.Services
{
    public class SentimentService<TData, TPrediction>
        where TData : class
        where TPrediction : class, new()
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _mlModel;
        private readonly ObjectPool<PredictionEngine<TData, TPrediction>> _predictionEnginePool;

        private static string _sentimentModelFilePath = "TrainedModel\\SentimentModel.zip";

        public SentimentService(IHostingEnvironment hostingEnvironment)
        {
            _mlContext = new MLContext();

            string contentRootPath = hostingEnvironment.ContentRootPath;
            string modelFullPath = Path.Combine(contentRootPath, _sentimentModelFilePath);

            using (var fs = File.OpenRead(modelFullPath))
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
