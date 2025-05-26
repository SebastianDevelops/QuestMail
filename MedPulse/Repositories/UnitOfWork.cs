using System.Threading.Tasks;
using MedPulse.DbContext;
using MedPulse.Models;
using MedPulse.Repositories.Interfaces;

namespace MedPulse.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QuestMailContext _context;
        public QuestRepository Quests { get; }
        public UserRepository Users { get; }
        public CompanionRepository Companions { get;  }
        public TrophyRepository Trophies { get; }
        public UserMessageRepository UserMessages { get;  }

        public UnitOfWork(
            QuestMailContext context,
            QuestRepository quests,
            UserRepository users,
            CompanionRepository companions,
            TrophyRepository trophies,
            UserMessageRepository userMessages)
        {
            _context = context;
            Quests = quests;
            Users = users;
            Companions = companions;
            Trophies = trophies;
            UserMessages = userMessages;
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    }
}