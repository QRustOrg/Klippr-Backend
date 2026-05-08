using Klippr_Backend.Setting.Domain.Model.Aggregate;
using Klippr_Backend.Setting.Domain.Model.Commands;

namespace Klippr_Backend.Setting.Domain.Services;

public interface IPreferenceCommandService
{
    Task<Preference?> Handle(CreatePreferenceCommand command);
    Task<bool> Handle(DeletePreferenceByIdCommand command);
    Task<Preference?> Handle(UpdatePreferenceCommand command); 
}