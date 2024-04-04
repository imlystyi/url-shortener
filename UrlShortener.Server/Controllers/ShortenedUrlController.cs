using Microsoft.AspNetCore.Mvc;
using UrlShortener.Server.Contexts;
using UrlShortener.Server.Exceptions;
using UrlShortener.Server.Managers;
using UrlShortener.Server.Models.Dto;

namespace UrlShortener.Server.Controllers;

[ApiController]
[Route("/")]
public class ShortenedUrlController(ShortenedUrlContext shortenedUrlContext) : Controller
{
    private readonly ShortenedUrlManager _shortenedUrlManager = new(shortenedUrlContext);

    [HttpGet("{shortUrl}")]
    public void RedirectFromShortenedUrl([FromRoute] string shortUrl)
    {
        try
        {
            string fullUrl = _shortenedUrlManager.GetFullUrl(shortUrl);
            string prefix = !fullUrl.Contains("https://")
                    ? "https://"
                    : !fullUrl.Contains("http://")
                            ? "http://"
                            : string.Empty;

            this.Response.Redirect(prefix + fullUrl);
        }
        catch (ShortenedUrlNotFound)
        {
            this.Response.StatusCode = 404;
        }
        catch (Exception)
        {
            this.Response.StatusCode = 500;
        }
    }

    [HttpGet("api/url/get-all")]
    public ActionResult<IEnumerable<ShortenedUrlTableOutputDto>> GetAll()
    {
        try
        {
            return this.Ok(_shortenedUrlManager.GetShortenedUrlList());
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("api/url/get-info/{id}")]
    public ActionResult<ShortenedUrlInfoOutputDto> GetInfo([FromRoute] long id)
    {
        try
        {
            return this.Ok(_shortenedUrlManager.GetShortenedUrlInfo(id));
        }
        catch (ShortenedUrlNotFound)
        {
            return this.NotFound();
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("api/url/add")]
    public ActionResult Add([FromBody] ShortenedUrlInputDto shortenedUrlDto)
    {
        try
        {
            _shortenedUrlManager.AddShortenedUrl(shortenedUrlDto);

            return this.Ok();
        }
        catch (FullUrlAlreadyShortenedException)
        {
            return this.Conflict();
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("api/url/delete/{id}")]
    public ActionResult Delete([FromRoute] long id)
    {
        try
        {
            _shortenedUrlManager.DeleteShortenedUrl(id);

            return this.Ok();
        }
        catch (ShortenedUrlNotFound)
        {
            return this.NotFound();
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("api/url/delete-all")]
    public ActionResult DeleteAll()
    {
        try
        {
            _shortenedUrlManager.DeleteAllShortenedUrls();

            return this.Ok();
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
