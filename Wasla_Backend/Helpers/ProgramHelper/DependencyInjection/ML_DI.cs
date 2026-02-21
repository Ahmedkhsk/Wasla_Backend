namespace Wasla_Backend.Helpers.ProgramHelper.DependencyInjection
{
    public static class ML_DI
    {
        public static IServiceCollection AddMLServices(this IServiceCollection services)
        {
            services.AddSingleton<ToxicityClassifier>();
            services.AddSingleton<BadWordsService>();

            services.AddSingleton<ToxicityClassifier>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var env = sp.GetRequiredService<IWebHostEnvironment>();
                var badWordsService = sp.GetRequiredService<BadWordsService>();

                string modelName = config["MLSettings:ModelOutputPath"] ?? "toxicity_model.zip";

                string modelPath = Path.Combine(
                    env.WebRootPath,
                    FileSetting.MLModelsPath.TrimStart('/'),
                    modelName
                );

                var classifier = new ToxicityClassifier(config, badWordsService);
                classifier.LoadModel(modelPath);

                return classifier;
            });
            return services;
        }
    }
}
