namespace Mp3Player.Menu.Buttons;
public class Button : IButton
{
    public delegate Task Act();
    public string Label { get; }
    
    private readonly Act _act;

    public Button(string label, Act act)
    {
        _act = act;
        Label = label;
    }

    public async Task OnClick()
    {
        //логи OnClick кнопки сработал
        await _act();
    }
}