using System.Text.RegularExpressions;

namespace Mp3Player.RequestCheckers;

public static partial class TrackRequestChecker
{
    public static bool CheckProfessor(string name)
    {
        var regex = ProfRegex();
        return !string.IsNullOrWhiteSpace(name)&&regex.IsMatch(name);
    }

    public static bool CheckTrackName(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }

    public static bool CheckAudioPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return false;

        return AudioRegex().IsMatch(path) && File.Exists(path);
    }
    
    [GeneratedRegex(@"^[А-ЯЁ][а-яё]+ [А-ЯЁ]\. [А-ЯЁ]\.$")]
    private static partial Regex ProfRegex();
    
    [GeneratedRegex(@"^.*\\.*\.(mp3|wav|flac|aac)$", RegexOptions.IgnoreCase, "ru-RU")]
    private static partial Regex AudioRegex();

}