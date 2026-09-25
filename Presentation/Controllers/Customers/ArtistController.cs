using Artway.Application.Interfaces.Accounts;
using Artway.Presentation.DTOs.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artway.Presentation.Controllers.Customers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ArtistController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IArtistService _artistService;
        public ArtistController(ILogger logger, IArtistService artistService)
        {
            _logger = logger;
            _artistService = artistService;
        }

        [HttpGet("/api/artists")]
        public async Task<ActionResult<IEnumerable<ArtistDto>>> GetAllArtists()
        {
            var artistList = await _artistService.GetAllArtists();
            return Ok(artistList);
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult<ArtistDto>> GetArtistById(int id)
        {
            var artist = await _artistService.GetArtistById(id);
            return Ok(artist);
        }

        [HttpGet("email")]
        public async Task<ActionResult<ArtistDto>> GetArtistByEmail(string email)
        {
            var artist = await _artistService.GetArtistByDisplayName(email);
            return Ok(artist);
        }

        [HttpPost]
        public async Task<ActionResult<ArtistDto>> AddNewArtist([FromBody]ArtistDto artist)
        {
            var newArtist = await _artistService.AddNewArtist(artist);
            return Ok(newArtist);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ArtistDto>> UpdateArtist(ArtistDto artist)
        {
            var updatedArtist = await _artistService.UpdateArtist(artist);
            return Ok(updatedArtist);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            await _artistService.DeleteArtist(id);
            return NoContent();
        }
    }
}