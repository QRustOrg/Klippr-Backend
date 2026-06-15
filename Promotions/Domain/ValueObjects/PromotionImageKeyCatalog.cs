namespace Klippr_Backend.Promotions.Domain.ValueObjects;

/// <summary>
/// Catalogo de claves locales permitidas para imagenes promocionales.
/// </summary>
public static class PromotionImageKeyCatalog
{
    private static readonly HashSet<string> AllowedKeys = new(StringComparer.Ordinal)
    {
        "comida_pizza",
        "comida_hamburguesas",
        "comida_pollo_frito",
        "comida_ceviche",
        "salud_pastillas",
        "salud_medicamentos",
        "entretenimiento_cine",
        "entretenimiento_bolos",
        "entretenimiento_bares",
        "entretenimiento_cibercafe",
        "deportes_futbol",
        "deportes_volley",
        "deportes_basket"
    };

    public static string? Normalize(string? imageKey)
    {
        if (string.IsNullOrWhiteSpace(imageKey))
            return null;

        var normalizedImageKey = imageKey.Trim();
        if (AllowedKeys.Contains(normalizedImageKey))
            return normalizedImageKey;

        throw new ArgumentException(
            $"ImageKey '{imageKey}' is not allowed. Allowed values: {string.Join(", ", AllowedKeys.Order())}.",
            nameof(imageKey));
    }
}
