using Klippr_Backend.Setting.Domain.Model.Aggregate;
using Klippr_Backend.Setting.Domain.Model.Queries;

namespace Klippr_Backend.Setting.Domain.Services;

public interface IPreferenceQueryServices
{
    Task<IEnumerable<Preference>> Handle(GetAllPreferenceQuery query);
    Task<Preference?> Handle(GetPreferenceByIdQuery query);
}