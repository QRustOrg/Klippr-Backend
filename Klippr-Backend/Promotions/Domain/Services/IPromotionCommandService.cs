using Klippr_Backend.Promotions.Domain.Commands;

namespace Klippr_Backend.Promotions.Domain.Services;

/// <summary>
/// Define las operaciones de escritura disponibles para el agregado de promociones.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Este contrato coordina casos de uso de comandos sin contener reglas propias;
/// las invariantes permanecen dentro de Promotions aggregate.
/// </remarks>
public interface IPromotionCommandService
{
    /// <summary>
    /// Crea una promoción en estado borrador.
    /// </summary>
    /// <param name="command">Datos necesarios para crear la promoción.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Identificador único de la promoción creada.</returns>
    Task<Guid> CreateAsync(CreatePromotionCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza los datos editables de una promoción existente.
    /// </summary>
    /// <param name="command">Datos actualizados de la promoción.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    Task UpdateAsync(UpdatePromotionCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publica una promoción previamente creada como borrador.
    /// </summary>
    /// <param name="command">Datos requeridos para publicar la promoción.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <remarks>
    /// El comando debe incluir el resultado de verificación del negocio resuelto por el contexto de perfil.
    /// </remarks>
    Task PublishAsync(PublishPromotionCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancela una promoción en borrador o publicada.
    /// </summary>
    /// <param name="command">Datos requeridos para cancelar la promoción.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    Task CancelAsync(CancelPromotionCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina una promoción existente.
    /// </summary>
    /// <param name="command">Datos requeridos para eliminar la promoción.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    Task DeleteAsync(DeletePromotionCommand command, CancellationToken cancellationToken = default);
}
