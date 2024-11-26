namespace UnitTests.Menu.Button;

public class ButtonTest
{

    [Fact]
    public async Task TestOnClick()
    {
        var wasCalled = false;
        const string label = "Test Button";
        var button = new Mp3Player.Menu.Buttons.Button(label, async () => { wasCalled = true; await Task.CompletedTask; });

        await button.OnClick();

        Assert.Equal(label, button.Label);
        Assert.True(wasCalled);
    }
    
}