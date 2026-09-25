using Artway.Infrastructure.Models.Customers;
using Artway.Presentation.DTOs.Customers;

namespace Artway.Application.Interfaces.Accounts
{
    public interface IArtistRepository
    {
        public Task<List<ArtistDto>> GetAllArtists();

        public Task<ArtistDto> GetArtistById(int id);

        public Task<ArtistDto> GetArtistByDisplayName(string email);

        public Task<ArtistDto> AddNewArtist(Artist artist);

        public Task<ArtistDto> UpdateArtist(Artist artist);

        public Task DeleteArtist(int id);
    }
}