namespace DoxieServer.Core;

public interface IEnvironmentVariables
{
    public bool PdfEnabled { get; }

    // string PdfPath { get; }

    public bool ImageEnabled { get; }

    // string ImagePath { get; }

    public string TargetPath { get; }

    public string SftpHost { get; }

    public int SftpPort { get; }

    public string SftpUsername { get; }

    public string SftpPassword { get; }

    string Username { get; }

    string Password { get; }
}
