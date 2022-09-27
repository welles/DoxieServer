using DoxieServer.Authorization;
using DoxieServer.Core;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Renci.SshNet;

namespace DoxieServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public sealed class DocumentsController : ControllerBase
{
    private IEnvironmentVariables EnvironmentVariables { get; }

    private ILogger<DocumentsController> Logger { get; }

    public DocumentsController(IEnvironmentVariables environmentVariables, ILogger<DocumentsController> logger)
    {
        this.EnvironmentVariables = environmentVariables;
        this.Logger = logger;
    }

    [HttpPost]
    [BasicAuthorization]
    public IActionResult Post([FromForm] IFormFileCollection? document)
    {
        if (document == null || !document.Any())
        {
            return this.BadRequest("No document has been supplied");
        }

        if (document.Any(x => x.ContentType != "image/jpeg"))
        {
            return this.BadRequest("Supplied documents must be of content type 'image/jpeg'");
        }

        using var sftpClient = new SftpClient(this.EnvironmentVariables.SftpHost,
            this.EnvironmentVariables.SftpPort,
            this.EnvironmentVariables.SftpUsername,
            this.EnvironmentVariables.SftpPassword);

        sftpClient.Connect();

        if (!sftpClient.IsConnected)
        {
            throw new InvalidOperationException("SFTP Client could not connect");
        }

        sftpClient.ChangeDirectory(this.EnvironmentVariables.TargetPath);

        foreach (var formFile in document)
        {
            var fileName = $"Scan_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}"; // System.IO.Path.GetFileNameWithoutExtension(formFile.FileName);

            this.Logger.LogInformation("Received file \'{FileName}\', saving to disk...", fileName);

            if (this.EnvironmentVariables.ImageEnabled)
            {
                var path = fileName + ".jpg";

                var attempt = 1;
                while (sftpClient.Exists(path))
                {
                    path = $"{fileName}_{attempt++}.jpg";
                }

                using var stream = new MemoryStream();

                formFile.CopyTo(stream);

                stream.Position = 0;
                sftpClient.UploadFile(stream, path, false);
            }

            if (this.EnvironmentVariables.PdfEnabled)
            {
                var path = fileName + ".pdf";

                var attempt = 1;
                while (sftpClient.Exists(path))
                {
                    path = $"{fileName}_{attempt++}.pdf";
                }

                using var stream = new MemoryStream();
                using var writer = new PdfWriter(stream);
                writer.SetCloseStream(false);
                using var pdf = new PdfDocument(writer);

                using var imageStream = new MemoryStream();
                formFile.CopyTo(imageStream);
                var imageBytes = imageStream.ToArray();

                var imageData = ImageDataFactory.Create(imageBytes);
                var image = new Image(imageData);

                using var page = new Document(pdf, new PageSize(image.GetImageWidth(), image.GetImageHeight()));
                page.SetMargins(0, 0, 0, 0);
                page.Add(image);

                pdf.Close();

                stream.Position = 0;
                sftpClient.UploadFile(stream, path, false);
            }
        }

        sftpClient.Disconnect();

        return this.Ok();
    }
}
