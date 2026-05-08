using Klippr_Backend.Setting.Domain.Model.Aggregate;
using Klippr_Backend.Setting.Domain.Model.Commands;
using Klippr_Backend.Setting.Domain.Repositories;
using Klippr_Backend.Setting.Domain.Services;
using Klippr_Backend.Shared.Domain.Repositories;

namespace Klippr_Backend.Setting.Application.Internal.CommandServices;

public class PreferenceCommandService(IPreferenceRepository preferenceRepository,
    IUnitOfWork unitOfWork) 
    : IPreferenceCommandService 
{
    public async Task<Preference?> Handle(CreatePreferenceCommand command)
    {
        var preference= new Preference(command);
        try
        {
            await preferenceRepository.AddAsync(preference);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR AL CREAR PREFERENCE: {ex.Message}");
            Console.WriteLine(ex.InnerException?.Message);
            throw;
        }
        return preference;
    }
    
    public async Task<bool> Handle(DeletePreferenceByIdCommand command)
    {
        var preference = await preferenceRepository.FindByIdAsync(command.Id);
        if (preference is null) return false;
        try
        {
            preferenceRepository.Remove(preference);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR AL ELIMINAR PREFERENCE: {ex.Message}");
            return false;
        }
    }
    
    public async Task<Preference?> Handle(UpdatePreferenceCommand command)
    {
        var preference = await preferenceRepository.FindByIdAsync(command.Id);
        if (preference is null) return null;
        try
        {
            preference.UpdatePreference(command);
            preferenceRepository.Update(preference);
            await unitOfWork.CompleteAsync();
            return preference;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR AL ACTUALIZAR PREFERENCE: {ex.Message}");
            return null;
        }
    }
}