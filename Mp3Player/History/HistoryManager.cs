using Microsoft.Extensions.Logging;

namespace Mp3Player.History;

public class HistoryManager: IHistoryManager
{
    private readonly string _fullPath;
    private readonly ILogger _logger;

    public HistoryManager(string path, ILogger logger)
    {
        _fullPath = Path.Combine(path, "history.txt");
        _logger = logger;
    }
    
    public async Task WriteHistory(string keyWord)
    {
        _logger.LogInformation("Запись истории поиска: {KeyWord}", keyWord);
        await File.WriteAllTextAsync(_fullPath, keyWord);
        _logger.LogInformation("История поиска успешно записана");
    }
    
    public async Task<string> GetHistory() 
    {
        _logger.LogInformation("Получение истории поиска");
        if (!File.Exists(_fullPath))
        {
            _logger.LogError("Файл истории поиска не найден: {FullPath}", _fullPath);
            throw new FileNotFoundException();
        }
        var keyWord = await File.ReadAllTextAsync(_fullPath);
        _logger.LogInformation("Из истории поиска получен ключ: {KeyWord}", keyWord);
        return keyWord;
    }
}