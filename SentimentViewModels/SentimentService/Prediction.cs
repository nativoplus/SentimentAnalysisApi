namespace SentimentViewModels.SentimentService
{
    // Destination model for prediction engine
    public class Prediction
    {
        public float Probability { get; set; } // 0=bad, 1=good
        public float Percentage => Probability * 100;
    }
}
