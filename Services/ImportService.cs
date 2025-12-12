using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Projeto1.Data;
using Projeto1.Models;

namespace Projeto1.Services;

public class ImportService : IImportService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ImportService> _logger;

    public ImportService(ApplicationDbContext context, ILogger<ImportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<(int total, int imported, int errors, List<string> errorMessages)> ImportarCsvAsync(string caminhoArquivo)
    {
        var errorMessages = new List<string>();
        int total = 0;
        int imported = 0;
        int errors = 0;

        try
        {
            if (string.IsNullOrWhiteSpace(caminhoArquivo))
            {
                errorMessages.Add("Caminho do arquivo não foi informado.");
                return (0, 0, 1, errorMessages);
            }

            if (!File.Exists(caminhoArquivo))
            {
                errorMessages.Add($"Arquivo não encontrado: {caminhoArquivo}");
                return (0, 0, 1, errorMessages);
            }

            // Verificar se a tabela Pessoas existe
            try
            {
                await _context.Database.CanConnectAsync();
            }
            catch (Exception dbEx)
            {
                errorMessages.Add($"Erro de conexão com o banco de dados: {dbEx.Message}");
                _logger.LogError(dbEx, "Erro ao conectar ao banco de dados");
                return (0, 0, 1, errorMessages);
            }

            var linhas = await File.ReadAllLinesAsync(caminhoArquivo, Encoding.UTF8);
            
            if (linhas.Length < 2)
            {
                errorMessages.Add("Arquivo CSV inválido: deve conter pelo menos o cabeçalho e uma linha de dados");
                return (0, 0, 1, errorMessages);
            }

            // Pular o cabeçalho (linha 0)
            for (int i = 1; i < linhas.Length; i++)
            {
                total++;
                var linha = linhas[i];

                if (string.IsNullOrWhiteSpace(linha))
                    continue;

                try
                {
                    var campos = ParseCsvLine(linha);
                    
                    if (campos.Count < 26)
                    {
                        errors++;
                        errorMessages.Add($"Linha {i + 1}: Número insuficiente de colunas (esperado: 26, encontrado: {campos.Count})");
                        continue;
                    }

                    var pessoa = new Pessoa
                    {
                        Canal = GetValue(campos, 0),
                        CodigoCliente = GetValue(campos, 1),
                        DataAdesao = ParseDate(GetValue(campos, 2)),
                        Status = GetValue(campos, 3),
                        CpfCnpj = GetValue(campos, 4),
                        Nome = GetValue(campos, 5),
                        RazaoSocial = GetValue(campos, 6),
                        Endereco = GetValue(campos, 7),
                        Numero = GetValue(campos, 8),
                        Complemento = GetValue(campos, 9),
                        Bairro = GetValue(campos, 10),
                        Cidade = GetValue(campos, 11),
                        Estado = GetValue(campos, 12),
                        Cep = GetValue(campos, 13),
                        RamoAtividade = GetValue(campos, 14),
                        Email = GetValue(campos, 15),
                        Celular = GetValue(campos, 16),
                        Telefone = GetValue(campos, 17),
                        CpfAgenteComercial = GetValue(campos, 18),
                        NomeAgenteComercial = GetValue(campos, 19),
                        Column21 = GetValue(campos, 20),
                        StatusDocumentacao = GetValue(campos, 21),
                        FaixaRenda = GetValue(campos, 22),
                        OrigemRenda = GetValue(campos, 23),
                        ComentarioOrigemRenda = GetValue(campos, 24),
                        FaixaPatrimonio = GetValue(campos, 25)
                    };

                    _context.Pessoas.Add(pessoa);
                    imported++;

                    // Salvar em lotes de 100 para melhor performance
                    if (imported % 100 == 0)
                    {
                        try
                        {
                            await _context.SaveChangesAsync();
                            _logger.LogInformation($"Importadas {imported} pessoas...");
                        }
                        catch (Exception saveEx)
                        {
                            errors++;
                            errorMessages.Add($"Erro ao salvar lote na linha {i + 1}: {saveEx.Message}");
                            _logger.LogError(saveEx, $"Erro ao salvar lote na linha {i + 1}");
                            // Continuar processando mesmo com erro de salvamento
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors++;
                    errorMessages.Add($"Linha {i + 1}: {ex.Message}");
                    _logger.LogError(ex, $"Erro ao processar linha {i + 1}");
                }
            }

            // Salvar registros restantes
            if (imported > 0 && imported % 100 != 0)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception saveEx)
                {
                    errors++;
                    errorMessages.Add($"Erro ao salvar registros finais: {saveEx.Message}");
                    _logger.LogError(saveEx, "Erro ao salvar registros finais");
                }
            }

            _logger.LogInformation($"Importação concluída: {imported} registros importados, {errors} erros de {total} linhas processadas");
        }
        catch (Exception ex)
        {
            errorMessages.Add($"Erro geral na importação: {ex.Message}");
            _logger.LogError(ex, "Erro ao importar arquivo CSV");
        }

        return (total, imported, errors, errorMessages);
    }

    private List<string> ParseCsvLine(string linha)
    {
        var campos = new List<string>();
        var campoAtual = new StringBuilder();
        bool dentroAspas = false;

        for (int i = 0; i < linha.Length; i++)
        {
            char c = linha[i];

            if (c == '"')
            {
                dentroAspas = !dentroAspas;
            }
            else if (c == ';' && !dentroAspas)
            {
                campos.Add(campoAtual.ToString().Trim());
                campoAtual.Clear();
            }
            else
            {
                campoAtual.Append(c);
            }
        }

        // Adicionar o último campo
        campos.Add(campoAtual.ToString().Trim());

        return campos;
    }

    private string? GetValue(List<string> campos, int index)
    {
        if (index < campos.Count && !string.IsNullOrWhiteSpace(campos[index]))
        {
            var valor = campos[index].Trim();
            return string.IsNullOrEmpty(valor) ? null : valor;
        }
        return null;
    }

    private DateTime? ParseDate(string? dataStr)
    {
        if (string.IsNullOrWhiteSpace(dataStr))
            return null;

        // Tentar diferentes formatos de data
        string[] formatos = { "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy" };

        foreach (var formato in formatos)
        {
            if (DateTime.TryParseExact(dataStr, formato, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime data))
            {
                return data;
            }
        }

        // Tentar parse genérico
        if (DateTime.TryParse(dataStr, CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out DateTime dataGenerica))
        {
            return dataGenerica;
        }

        return null;
    }
}

