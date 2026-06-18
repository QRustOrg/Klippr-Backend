using System.Net.Mime;
using Klippr_Backend.Setting.Domain.Model.Commands;
using Klippr_Backend.Setting.Domain.Model.Queries;
using Klippr_Backend.Setting.Domain.Services;
using Klippr_Backend.Setting.Interfaces.REST.Resources;
using Klippr_Backend.Setting.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Klippr_Backend.Setting.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Preference Endpoints")]
public class PreferencesController(
    IPreferenceCommandService preferenceCommandService,
    IPreferenceQueryServices preferenceQueryServices) : ControllerBase
{
    /// Get Preference by id
    [HttpGet("{preferenceId:int}")]
    [SwaggerOperation(
        Summary = "Get a Preference by Id",
        Description = "Get a Preference by its Id",
        OperationId = "GetPreferenceById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The list of Preferences", typeof(PreferenceResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No Preferences found")]
    public async Task<IActionResult> GetPreferenceById(int preferenceId)
    {
        var getPreferenceByIdQuery = new GetPreferenceByIdQuery(preferenceId);
        var preference = await preferenceQueryServices.Handle(getPreferenceByIdQuery);
        if (preference is null) return NotFound();
        var resource = PreferenceResourceFromEntityAssembler.ToResourceFromEntity(preference);
        return Ok(resource);
    }
    
    /// Get all Preferences
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all Preferences",
        Description = "Get all Preferences",
        OperationId = "GetAllPreferences")]
    [SwaggerResponse(StatusCodes.Status200OK, "The list of Preferences", typeof(IEnumerable<PreferenceResource>))]
    public async Task<IActionResult> GetAllPreferences()
    {
        var preferences = await preferenceQueryServices.Handle(new GetAllPreferenceQuery());
        var preferenceResources = preferences.Select(PreferenceResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(preferenceResources);
    }
    
    /// Create a new Preference
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new Preference",
        Description = "Create a new Preference",
        OperationId = "CreatePreference")]
    [SwaggerResponse(StatusCodes.Status201Created, "The Preference was created", typeof(PreferenceResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The Preference could not be created")]
    public async Task<IActionResult> CreatePreference([FromBody] CreatePreferenceResource resource)
    {
        var createPreferenceCommand = CreatePreferenceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var preference = await preferenceCommandService.Handle(createPreferenceCommand);
        if (preference is null) return BadRequest();
        var preferenceResource = PreferenceResourceFromEntityAssembler.ToResourceFromEntity(preference);
        return CreatedAtAction(nameof(GetPreferenceById), new { preferenceId = preference.Id }, preferenceResource);
    }
    
    /// Update a new Preference
    [HttpPut("{preferenceId:int}")]
    [SwaggerOperation(
        Summary = "Update Preference by Id",
        Description = "Update all fields of a Preference by its Id",
        OperationId = "UpdatePreferenceById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The Preference was updated", typeof(PreferenceResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Preference not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The Preference could not be updated")]
    public async Task<IActionResult> UpdatePreference(int preferenceId, [FromBody] UpdatePreferenceResource resource)
    {
        var command = UpdatePreferenceCommandFromResourceAssembler.ToCommandFromResource(preferenceId, resource);
        var preference = await preferenceCommandService.Handle(command);
        if (preference is null) return NotFound();
        var preferenceResource = PreferenceResourceFromEntityAssembler.ToResourceFromEntity(preference);
        return Ok(preferenceResource);
    }
    
    /// Delete a new Preference
    [HttpDelete("{preferenceId:int}")]
    [SwaggerOperation(
        Summary = "Delete Preference by Id",
        Description = "Delete a Preference by its Id",
        OperationId = "DeletePreferenceById")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The Preference was deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Preference not found")]
    public async Task<IActionResult> DeletePreference(int preferenceId)
    {
        var command = new DeletePreferenceByIdCommand(preferenceId);
        var result = await preferenceCommandService.Handle(command);
        if (!result) return NotFound();
        return NoContent();
    }
}