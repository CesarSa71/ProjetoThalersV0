namespace Projeto1.Services;

public interface IImportService
{
    Task<(int total, int imported, int errors, List<string> errorMessages)> ImportarCsvAsync(string caminhoArquivo);
}

