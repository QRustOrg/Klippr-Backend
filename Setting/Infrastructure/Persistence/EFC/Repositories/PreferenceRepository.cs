using Klippr_Backend.Setting.Domain.Model.Aggregate;
using Klippr_Backend.Setting.Domain.Repositories;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Klippr_Backend.Setting.Infrastructure.Persistence.EFC.Repositories;

public class PreferenceRepository(AppDbContext context)
    : BaseRepository<Preference>(context), IPreferenceRepository
{
    
}