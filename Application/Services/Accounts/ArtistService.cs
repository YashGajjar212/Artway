using Artway.Application.Exceptions;
using Artway.Application.Interfaces.Accounts;
using Artway.Infrastructure.Models.Customers;
using Artway.Presentation.DTOs.Customers;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Artway.Application.Services.Accounts
{
    public class ArtistService : IArtistService
    {
        private readonly ILogger _logger;

        private readonly IMapper _mapper;
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IArtistRepository artistRepository, ILogger logger, IMapper mapper)
        {
            _artistRepository = artistRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArtistDto>> GetAllArtists()
        {
            var artistDtoList = await _artistRepository.GetAllArtists();

            if (artistDtoList == null)
                throw new Exception("An error occured while fetching the list");

            var artistList = _mapper.Map<IEnumerable<ArtistDto>>(artistDtoList);
            return artistList;
        }

        public async Task<ArtistDto> GetArtistById(int id)
        {
            var artist = await _artistRepository.GetArtistById(id);

            if (artist == null)
                throw new NotFoundException("No artist found");

            var artistDto = _mapper.Map<ArtistDto>(artist);
            return artistDto;
        }

        public async Task<ArtistDto> GetArtistByDisplayName(string email)
        {
            var artist = await _artistRepository.GetArtistByDisplayName(email);

            if (artist == null)
                throw new NotFoundException("No artist found");

            var artistDto = _mapper.Map<ArtistDto>(artist);
            return artistDto;
        }

        public async Task<ArtistDto> AddNewArtist(ArtistDto artist)
        {
            var existingArtist = await _artistRepository.GetArtistByDisplayName(artist.DisplayName);

            if (existingArtist != null)
                throw new Exception("DisplayName has to be unique");

            var newArtist = _mapper.Map<Artist>(artist);

            var result = await _artistRepository.AddNewArtist(newArtist);

            if (result == null)
                throw new Exception("An error occured while inserting new artist");

            var resulDto = _mapper.Map<ArtistDto>(result);
            return resulDto;
        }

        public async Task<ArtistDto> UpdateArtist(ArtistDto artist)
        {
            var artistDto = await _artistRepository.GetArtistById(artist.ArtistId);

            if (artistDto == null)
                throw new Exception("Artist not found");

            Artist updateArtist = new Artist();
            updateArtist.Name = artist.Name;
            updateArtist.DisplayName = artist.DisplayName;
            updateArtist.Bio = artist.Bio;

            await _artistRepository.UpdateArtist(updateArtist);
            return _mapper.Map<ArtistDto>(updateArtist);
        }

        public async Task DeleteArtist(int id)
        {
            var artist = await _artistRepository.GetArtistById(id);

            if (artist == null)
                throw new Exception("Artist not found");

            await _artistRepository.DeleteArtist(id);
        }
    }
}
