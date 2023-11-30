using Microsoft.EntityFrameworkCore;
using TestTelcoHub.Infastruture.Interface;
using TestTelcoHub.Model.Data;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Infastruture.Repository
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetRefreshToken(string tokenId);
        Task<RefreshToken> GetByIdAsync(string id);
        Task<RefreshToken> GetRefreshTokenByUserId(string userId);
        Task<RefreshToken> GetRefreshTokenByToken(string refreshToken);
    }
    public class RefreshTokenRipository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRipository(ApiDbContext _context) : base(_context) { }

        public async Task<RefreshToken> GetByIdAsync(string id)
        {
            var refreshToken = await _context.RefreshTokens.FindAsync(id);
            return refreshToken!;
        }

        public async Task<RefreshToken> GetRefreshToken(string tokenId)
        {
            var refreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token.ToString() == tokenId);
            return refreshToken!;
        }

        public async Task<RefreshToken> GetRefreshTokenByToken(string refreshToken)
        {
            var refreshTokenEntity = await _context.RefreshTokens
                .SingleOrDefaultAsync(x => x.Token == refreshToken);

            return refreshTokenEntity!;
        }

        public async Task<RefreshToken> GetRefreshTokenByUserId(string userId)
        {
            var getRefreshToke = await _context.RefreshTokens
            .Where(r => r.UserId == userId)
            .SingleOrDefaultAsync()!;
            return getRefreshToke!;
        }
    }
}
