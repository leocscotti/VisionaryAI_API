using Microsoft.ML;
using VisionaryAI.API.Database;
using VisionaryAI.API.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MLModelService
{
    private readonly MLContext _mlContext;
    private ITransformer _model;
    private readonly VisionaryAIDBContext _dbContext;

    public MLModelService(VisionaryAIDBContext dbContext)
    {
        _mlContext = new MLContext();
        _dbContext = dbContext;
        InicializarModelo();
    }

    public void TreinarModelo(IList<FonteDadosInput> dadosTreinamento)
    {
        var trainingData = _mlContext.Data.LoadFromEnumerable(dadosTreinamento);
        var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(FonteDadosInput.Tipo))
            .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: nameof(FonteDadosInput.Tendencia), maximumNumberOfIterations: 100));

        _model = pipeline.Fit(trainingData);
    }

    public void InicializarModelo()
    {
        var dadosTreinamento = _dbContext.FonteDeDados.ToList();
        var dadosTreinamentoInput = dadosTreinamento.Select(f => new FonteDadosInput
        {
            Tipo = MapTipoToFloat(f.Tipo),
            Tendencia = 0f
        }).ToList();

        TreinarModelo(dadosTreinamentoInput);
    }

    public async Task<string> PreverTendenciaPorId(int fonteDadosId)
    {
        var fonteDados = await _dbContext.FonteDeDados.FindAsync(fonteDadosId);
        if (fonteDados == null)
            throw new Exception($"Fonte de dados com ID {fonteDadosId} não encontrada.");

        var dadosInput = new FonteDadosInput
        {
            Tipo = MapTipoToFloat(fonteDados.Tipo)
        };

        return PreverTendencia(dadosInput);
    }

    private static float MapTipoToFloat(string tipo)
    {
        return tipo switch
        {
            "Tipo1" => 1f,
            "Tipo2" => 2f,
            "Tipo3" => 3f,
            _ => 0f
        };
    }

    public string PreverTendencia(FonteDadosInput dadosInput)
    {
        if (_model == null)
            throw new InvalidOperationException("O modelo de ML não foi treinado. Chame 'InicializarModelo()' antes de fazer previsões.");

        var predictionEngine = _mlContext.Model.CreatePredictionEngine<FonteDadosInput, ResultadoPrevisao>(_model);
        var resultado = predictionEngine.Predict(dadosInput);

        return resultado.Tendencia > 0
            ? "A previsão é de subida nas vendas."
            : resultado.Tendencia < 0
                ? "A previsão é de queda nas vendas."
                : "A previsão é estável nas vendas.";
    }
}
