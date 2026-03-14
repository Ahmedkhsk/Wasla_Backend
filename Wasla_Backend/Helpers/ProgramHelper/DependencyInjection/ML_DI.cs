namespace Wasla_Backend.Helpers.ProgramHelper.DependencyInjection
{
    public static class ML_DI
    {
        public static IServiceCollection AddMLServices(this IServiceCollection services)
        {
            services.AddSingleton<BadWordsService>();
            services.AddSingleton<ToxicityClassifier>(sp =>
            {
                using var scope = sp.CreateScope();
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                var badWordsService = scope.ServiceProvider.GetRequiredService<BadWordsService>();
                var fileUrlBuilder = scope.ServiceProvider.GetRequiredService<IFileUrlBuilderService>();

                string modelName = config["MLSettings:ModelOutputPath"] ?? "toxicity_model.zip";
                string modelPath = Path.Combine(
                    fileUrlBuilder.GetPath(MediaType.MLModel),
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