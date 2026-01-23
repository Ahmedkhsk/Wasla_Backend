namespace Wasla_Backend.Helpers.MlHelper
{
    public class ToxicityClassifier
    {
        private readonly IConfiguration _configuration;
        private readonly MLContext _mlContext;
        private ITransformer _model;
        private PredictionEngine<ModelBinaryInput, ModelOutput> _predictionEngine;
        private readonly BadWordsService _badWords;


        public ToxicityClassifier(IConfiguration configuration,BadWordsService badWordsService)
        {
            _configuration = configuration;
            _mlContext = new MLContext();
            _badWords = badWordsService;
        }


        public void Train(string dataPath)
        {
            IDataView rawData = _mlContext.Data.LoadFromTextFile<ModelInput>(
                path: dataPath, hasHeader: true, separatorChar: ',');

            var enumerableData = _mlContext.Data.CreateEnumerable<ModelInput>(rawData, reuseRowObject: false);

            var processedDataList = enumerableData.Select(x => new ModelBinaryInput
            {
                Text = x.Tweet,
                Label = x.Class.Trim().ToLower() != "normal"
            }).ToList();

            IDataView trainingData = _mlContext.Data.LoadFromEnumerable(processedDataList);

            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(ModelBinaryInput.Text))
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

            _model = pipeline.Fit(trainingData);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelBinaryInput, ModelOutput>(_model);
        }

        public bool IsBadWord(string text)
        {
            var predicate= _badWords.ContainsBadWord(text);
            if(predicate)
                return true;
            if (string.IsNullOrEmpty(text) || _predictionEngine == null) return false;

            var input = new ModelBinaryInput { Text = text };
            var result = _predictionEngine.Predict(input);

            return result.Prediction; 
        }

        public void SaveModel(string modelPath)
        {
            _mlContext.Model.Save(_model, null, modelPath);
        }

        public void LoadModel(string modelPath)
        {
            _model = _mlContext.Model.Load(modelPath, out var _);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelBinaryInput, ModelOutput>(_model);
        }
    }
}