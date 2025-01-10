using Microsoft.AspNetCore.Mvc;
using Mp3Player.InputReaders;
using Mp3Player.Menu.Buttons;

namespace Mp3Player.Menu;

/*
[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase, IMenu
{
    public Dictionary<int, IButton>? Buttons { get; set; }
    public string Label { get; set; }
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ILogger<MenuController> _logger;

    public MenuController(ILogger<MenuController> logger)
    {
        _logger = logger;
        _cancellationTokenSource = new CancellationTokenSource();
        Label = "Menu";
    }

    [HttpGet("show")]
    public async Task<IActionResult> ShowMenu([FromQuery] string label)
    {
        _logger.LogDebug("Показ меню: {Label}", label);
        var buttons = await ShowHelp();
        return Ok(buttons);
    }

    [HttpPost("click/{buttonKey}")]
    public async Task<IActionResult> ClickButton(int buttonKey)
    {
        _logger.LogDebug("Нажатие кнопки: {ButtonKey}", buttonKey);
        var result = await ButtonClick(buttonKey);
        if (result == null)
        {
            _logger.LogWarning("Кнопка с ключом {ButtonKey} не найдена", buttonKey);
            var buttons = ShowHelp();
            return BadRequest(new { message = "Кнопка не найдена. Пожалуйста, выберите одну из доступных кнопок.", buttons });
        }
        return Ok("Кнопка нажата");
    }

    [HttpPost("run")]
    public async Task<IActionResult> Run()
    {
        _logger.LogDebug("Запуск меню: {Label}", Label);
        _logger.LogWarning("Экземпляр MenuController: {Label}", Label);
        var buttons = await ShowHelp(); 
        _logger.LogWarning("Кнопки в MenuController при запуске: {Buttons}", 
            Buttons == null ? "null" : string.Join(", ", Buttons.Keys));
        _logger.LogDebug("Показаны все кнопки меню"); 
        return Ok(buttons);
    }

    [HttpPost("buttonclick")]
    public async Task<IButton?> ButtonClick(int buttonKey)
    {
        if (Buttons != null && Buttons.TryGetValue(buttonKey, out var button))
        {
            _logger.LogDebug("Обработка нажатия кнопки: {ButtonLabel}", button.Label);
            await button.OnClick();
            _logger.LogDebug("Обработка кнопки {ButtonLabel} выполнена", button.Label);
            return button;
        }

        _logger.LogWarning("Кнопка с ключом {ButtonKey} не найдена", buttonKey);
        return null;
    }

    [HttpGet("showhelp")]
    public async Task<Dictionary<int, string>> ShowHelp()
    {
        _logger.LogDebug("Показ справки для меню: {Label}", Label);
        var buttonDescriptions = new Dictionary<int, string>();

        if (Buttons == null)
        {
            _logger.LogWarning("В меню нет кнопок");
            return buttonDescriptions;
        }

        foreach (var button in Buttons)
        {
            buttonDescriptions.Add(button.Key, button.Value.Label);
            _logger.LogDebug("Добавлена кнопка для показа: {Key} — {Label}", button.Key, button.Value.Label);
        }

        return await Task.FromResult(buttonDescriptions);
    }
}
*/

