using Microsoft.AspNetCore.Mvc;

namespace Mp3Player.Menu;

public class MenuNavigator : IMenuNavigator
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MenuNavigator> _logger;

    public MenuNavigator(HttpClient httpClient, ILogger<MenuNavigator> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IActionResult> NavigateTo(string menuLabel, string? message = default)
    {
        if (message is not null)
        {
            _logger.LogDebug("Получено сообщение: {Message}", message);
            return new JsonResult(new { message });
        }

        _logger.LogDebug("Навигация к меню: {MenuLabel}", menuLabel);

        var response = await _httpClient.GetAsync($"api/menu/show?label={menuLabel}");
        if (response.IsSuccessStatusCode)
        {
            var buttons = await response.Content.ReadFromJsonAsync<Dictionary<int, string>>();
            if (buttons != null) return new JsonResult(new { menuLabel, buttons });
            _logger.LogWarning("Ответ не содержит кнопок для меню: {MenuLabel}", menuLabel);
            return new JsonResult(new { message = "Ответ не содержит кнопок для меню." });
        }
        _logger.LogError("Ошибка при навигации к меню: {MenuLabel}", menuLabel);
        return new JsonResult(new { message = "Ошибка при навигации к меню." });
    }
}
