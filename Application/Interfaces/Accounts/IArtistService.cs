using Artway.Infrastructure.Models.Customers;
using Artway.Presentation.DTOs.Customers;

namespace Artway.Application.Interfaces.Accounts
{
    public interface IArtistService
    {
        public Task<IEnumerable<ArtistDto>> GetAllArtists();

        public Task<ArtistDto> GetArtistById(int id);

        public Task<ArtistDto> GetArtistByDisplayName(string email);

        public Task<ArtistDto> AddNewArtist(ArtistDto artist);

        public Task<ArtistDto> UpdateArtist(ArtistDto id);

        public Task DeleteArtist(int id);
    }
}