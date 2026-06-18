using Klippr_Backend.Setting.Domain.Model.Aggregate;
using Klippr_Backend.Setting.Domain.Model.Queries;
using Klippr_Backend.Setting.Domain.Repositories;
using Klippr_Backend.Setting.Domain.Services;

namespace Klippr_Backend.Setting.Application.Internal.QueryServices;

public class PreferenceQueryService(IPreferenceRepository preferenceRepository)
    : IPreferenceQueryServices
{
    public async Task<Preference?> Handle(GetPreferenceByIdQuery query)
    {
        return await preferenceRepository.FindByIdAsync(query.Id);
    }
    
    public async Task<IEnumerable<Preference>> Handle(GetAllPreferenceQuery query)
    {
        return await preferenceRepository.ListAsync();
    }
}