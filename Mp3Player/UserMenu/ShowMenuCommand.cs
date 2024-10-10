﻿using System.Collections;

namespace Mp3Player.UserMenu;

public class ShowMenuCommand: ICommand<bool, string>
{
    private readonly Dictionary<int, IUniCommand<string>> _commands;
    public string? Description { get; } = "Список команд";

    public ShowMenuCommand(Dictionary<int, IUniCommand<string>> commands) 
    {
        _commands = commands;
    }
    
    Task IUniCommand<string>.Execute(string? arg)
    {
        return Execute(arg);
    }
    
    public async Task<bool> Execute(string? arg = default)
    {
        await Console.Out.WriteLineAsync(Description + ":");
        foreach (var command in _commands) 
            await Console.Out.WriteLineAsync($"{command.Key}: {command.Value.Description}"); 
        return true;
    }
}