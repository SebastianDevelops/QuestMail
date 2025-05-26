using MedPulse.Models;
using MedPulse.Repositories.Interfaces;

namespace MedPulse.Repositories;

public interface IUnitOfWork
{
    QuestRepository Quests { get; }
    UserRepository Users { get; }
    CompanionRepository Companions { get; }
    TrophyRepository Trophies { get; }
    UserMessageRepository UserMessages { get; }
    Task<int> CompleteAsync();
}