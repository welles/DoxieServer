namespace DoxieServer.Core;

public sealed class EnvironmentVariables : IEnvironmentVariables
{
    public EnvironmentVariables()
    {
        this.PdfEnabled = Environment.GetEnvironmentVariable("PDF_ENABLED")?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
        this.ImageEnabled = Environment.GetEnvironmentVariable("IMAGE_ENABLED")?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;

        if (!this.PdfEnabled || !this.ImageEnabled)
        {
            throw new ArgumentException("Either PDF_ENABLED or IMAGE_ENABLED environment variable must be set to 'true'");
        }

        // this.PdfPath = Environment.GetEnvironmentVariable("PDF_PATH") ?? string.Empty;
        //
        // if (this.PdfEnabled && (string.IsNullOrWhiteSpace(this.PdfPath) || !Directory.Exists(this.PdfPath)))
        // {
        //     throw new ArgumentException("PDF_PATH environment variable is not set or not a valid path");
        // }

        // this.ImagePath = Environment.GetEnvironmentVariable("IMAGE_PATH") ?? string.Empty;
        //
        // if (this.ImageEnabled && (string.IsNullOrWhiteSpace(this.ImagePath) || !Directory.Exists(this.ImagePath)))
        // {
        //     throw new ArgumentException("IMAGE_PATH environment variable is not set or not a valid path");
        // }

        this.TargetPath = Environment.GetEnvironmentVariable("TARGET_PATH") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(this.TargetPath))
        {
            throw new ArgumentException("TARGET_PATH environment variable is not set");
        }

        this.Username = Environment.GetEnvironmentVariable("USERNAME") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(this.Username))
        {
            throw new ArgumentException("USERNAME environment variable is not set");
        }

        this.Password = Environment.GetEnvironmentVariable("PASSWORD") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(this.Password))
        {
            throw new ArgumentException("PASSWORD environment variable is not set");
        }
    }

    public bool PdfEnabled { get; }

    // public string PdfPath { get; }

    public bool ImageEnabled { get; }

    // public string ImagePath { get; }

    public string TargetPath { get; }

    public string Username { get; }

    public string Password { get; }
}
