using Microsoft.ML;
using Microsoft.ML.Data;
using VisionaryAI.API.Database;
using VisionaryAI.API.Models;
using System.Collections.Generic;
using System.Linq;

namespace VisionaryAI.API.Services
{
    public class MLModelService
    {
        private readonly MLContext _mlContext;
        private ITransformer _model;

        public MLModelService()
        {
            _mlContext = new MLContext();
        }

        public void TreinarModelo(IList<FonteDadosInput> dadosTreinamento)
        {
            // Converter lista para IDataView
            IDataView trainingData = _mlContext.Data.LoadFromEnumerable(dadosTreinamento);

            // Definir pipeline de aprendizado
            var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(FonteDadosInput.Tipo))
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: nameof(FonteDadosInput.Tendencia), maximumNumberOfIterations: 100));

            // Treinar o modelo
            _model = pipeline.Fit(trainingData);
        }

        public void InicializarModelo(VisionaryAIDBContext dbContext)
        {
            // Carregar dados da tabela FonteDados
            var dadosTreinamento = dbContext.FonteDeDados.ToList();

            // Mapear os dados carregados para FonteDadosInput
            var dadosTreinamentoInput = dadosTreinamento.Select(f => new FonteDadosInput
            {
                Tipo = MapTipoToFloat(f.Tipo), // Chama o método estático
                Tendencia = 0f // Ajuste se precisar de uma tendência inicial
            }).ToList();

            // Chamar o método de treinamento
            TreinarModelo(dadosTreinamentoInput);
        }


        // Método para mapear o tipo de fonte de dados para um valor numérico
        private static float MapTipoToFloat(string tipo)
        {
            return tipo switch
            {
                "Tipo1" => 1f,
                "Tipo2" => 2f,
                "Tipo3" => 3f,
                // Adicione outros tipos conforme necessário
                _ => 0f // Valor padrão se o tipo não for reconhecido
            };
        }
        public string PreverTendencia(FonteDadosInput dadosInput)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<FonteDadosInput, ResultadoPrevisao>(_model);
            var resultado = predictionEngine.Predict(dadosInput);

            // Lógica para determinar a mensagem de saída
            if (resultado.Tendencia > 0)
                return "A previsão é de subida nas vendas.";
            else if (resultado.Tendencia < 0)
                return "A previsão é de queda nas vendas.";
            else
                return "A previsão é estável nas vendas.";
        }
    }
}
