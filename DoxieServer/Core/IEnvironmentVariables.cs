namespace DoxieServer.Core;

public interface IEnvironmentVariables
{
    public bool PdfEnabled { get; }

    string PdfPath { get; }

    public bool ImageEnabled { get; }

    string ImagePath { get; }

    string Username { get; }

    string Password { get; }
}
