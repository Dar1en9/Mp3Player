using Microsoft.AspNetCore.Mvc;

namespace Mp3Player.Runners;

public interface IProgramRunner
{
    Task<IActionResult> Run();
}