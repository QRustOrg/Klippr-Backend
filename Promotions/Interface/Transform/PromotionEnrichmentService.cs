using Klippr_Backend.IAM.Domain.Repositories;
using Klippr_Backend.Profile.Domain.Repositories;
using Klippr_Backend.Promotions.Domain.Aggregates;

namespace Klippr_Backend.Promotions.Interface.Transform;

/// <summary>
/// Enriquece promociones con datos de perfil necesarios para respuestas HTTP.
/// </summary>
public class PromotionEnrichmentService(
    IBusinessProfileRepository businessProfileRepository,
    IUserRepository userRepository)
{
    public async Task<PromotionResource> ToResourceAsync(
        Promotion promotion,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(promotion);

        return PromotionResourceFromEntityAssembler.ToResource(
            promotion,
            await ResolveBusinessNameAsync(promotion.BusinessId, cancellationToken).ConfigureAwait(false));
    }

    public async Task<IReadOnlyList<PromotionResource>> ToResourcesAsync(
        IEnumerable<Promotion> promotions,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(promotions);

        var resources = new List<PromotionResource>();
        var businessNames = new Dictionary<Guid, string?>();

        foreach (var promotion in promotions)
        {
            if (!businessNames.TryGetValue(promotion.BusinessId, out var businessName))
            {
                businessName = await ResolveBusinessNameAsync(promotion.BusinessId, cancellationToken)
                    .ConfigureAwait(false);
                businessNames[promotion.BusinessId] = businessName;
            }

            resources.Add(PromotionResourceFromEntityAssembler.ToResource(promotion, businessName));
        }

        return resources;
    }

    private async Task<string?> ResolveBusinessNameAsync(Guid businessId, CancellationToken cancellationToken)
    {
        var businessProfile = await businessProfileRepository
            .GetByUserIdAsync(businessId, cancellationToken)
            .ConfigureAwait(false);

        businessProfile ??= await businessProfileRepository
            .GetByIdAsync(businessId, cancellationToken)
            .ConfigureAwait(false);

        if (!string.IsNullOrWhiteSpace(businessProfile?.BusinessName))
            return businessProfile.BusinessName;

        var user = await userRepository
            .GetByIdAsync(businessId, cancellationToken)
            .ConfigureAwait(false);

        return string.IsNullOrWhiteSpace(user?.BusinessName) ? null : user.BusinessName;
    }
}
