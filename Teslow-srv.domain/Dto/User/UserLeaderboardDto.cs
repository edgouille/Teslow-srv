using System;

namespace Teslow_srv.Domain.Dto.User
{
    public class UserLeaderboardDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int GamesPlayed { get; set; }
    }
}
