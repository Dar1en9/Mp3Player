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
        _logger.LogDebug("Запись истории поиска: {KeyWord}", keyWord);
        await File.WriteAllTextAsync(_fullPath, keyWord);
        _logger.LogDebug("История поиска успешно записана");
    }
    
    public async Task<string> GetHistory() 
    {
        _logger.LogDebug("Получение истории поиска");
        if (!File.Exists(_fullPath))
        {
            _logger.LogError("Файл истории поиска не найден: {FullPath}", _fullPath);
            throw new FileNotFoundException();
        }
        var keyWord = await File.ReadAllTextAsync(_fullPath);
        _logger.LogDebug("Из истории поиска получен ключ: {KeyWord}", keyWord);
        return keyWord;
    }
}