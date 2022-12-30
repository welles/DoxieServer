namespace DoxieServer.Core;

public sealed class EnvironmentVariables : IEnvironmentVariables
{
    public EnvironmentVariables()
    {
        this.PdfEnabled = Environment.GetEnvironmentVariable("PDF_ENABLED")?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
        this.ImageEnabled = Environment.GetEnvironmentVariable("IMAGE_ENABLED")?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;

        if (!this.PdfEnabled && !this.ImageEnabled)
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

        this.SftpHost = Environment.GetEnvironmentVariable("SFTP_HOST") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(this.SftpHost))
        {
            throw new ArgumentException("SFTP_HOST environment variable is not set");
        }

        this.SftpPort = int.Parse(Environment.GetEnvironmentVariable("SFTP_PORT") ?? string.Empty);

        this.SftpUsername = Environment.GetEnvironmentVariable("SFTP_USERNAME") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(this.SftpUsername))
        {
            throw new ArgumentException("SFTP_USERNAME environment variable is not set");
        }

        this.SftpPassword = Environment.GetEnvironmentVariable("SFTP_PASSWORD") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(this.SftpPassword))
        {
            throw new ArgumentException("SFTP_PASSWORD environment variable is not set");
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

    public string SftpHost { get; }

    public int SftpPort { get; }

    public string SftpUsername { get; }

    public string SftpPassword { get; }

    // public string ImagePath { get; }

    public string TargetPath { get; }

    public string Username { get; }

    public string Password { get; }
}
