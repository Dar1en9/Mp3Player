﻿using Mp3Player.Menu.UserMenu;

namespace Mp3Player.Menu;

public class MenuNavigator : IMenuNavigator
{
    public async Task NavigateTo(IMenu menu) {
        await menu.Run();
    }
}