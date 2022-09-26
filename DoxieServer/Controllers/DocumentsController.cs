using CliWrap;
using DoxieServer.Authorization;
using DoxieServer.Core;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;

namespace DoxieServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public sealed class DocumentsController : ControllerBase
{
    private IEnvironmentVariables EnvironmentVariables { get; }

    public DocumentsController(IEnvironmentVariables environmentVariables)
    {
        this.EnvironmentVariables = environmentVariables;
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

        foreach (var formFile in document)
        {
            //this.Logger.LogInformation("Received file \'{FileName}\', saving to disk...", formFile.FileName);

            var fileName = $"Scan_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}"; // System.IO.Path.GetFileNameWithoutExtension(formFile.FileName);
            var extension = System.IO.Path.GetExtension(formFile.FileName);

            if (this.EnvironmentVariables.ImageEnabled)
            {
                var path = System.IO.Path.Combine(this.EnvironmentVariables.TargetPath, "JPG", fileName + extension);

                var attempt = 1;
                while (System.IO.File.Exists(path))
                {
                    path = System.IO.Path.Combine(this.EnvironmentVariables.TargetPath, "JPG", $"{fileName}_{attempt++}{extension}");
                }

                using var stream = new FileStream(path, FileMode.CreateNew);

                formFile.CopyTo(stream);
            }

            if (this.EnvironmentVariables.PdfEnabled)
            {
                var path = System.IO.Path.Combine(this.EnvironmentVariables.TargetPath, "PDF", fileName + ".pdf");

                var attempt = 1;
                while (System.IO.File.Exists(path))
                {
                    path = System.IO.Path.Combine(this.EnvironmentVariables.TargetPath, "PDF", $"{fileName}_{attempt++}.pdf");
                }

                using var stream = new FileStream(path, FileMode.CreateNew);
                using var writer = new PdfWriter(stream);
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
            }
        }

        return this.Ok();
    }
}
