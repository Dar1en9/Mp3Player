﻿using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Mp3Player.Menu;

public class MenuNavigator(ILogger logger) : IMenuNavigator
{
    public async Task NavigateTo(IMenu menu, string? message = default) {
        AnsiConsole.Clear();
        if (message is not null)
        {
            logger.LogDebug("Получено сообщение: {Message}", message);
            await Console.Out.WriteLineAsync(message);
        }
        logger.LogDebug("Навигация к меню: {Menu}", menu.Label);
        await menu.Run();
    }
}