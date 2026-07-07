using Klippr_Backend.Promotions.Domain.Aggregates;
using Klippr_Backend.Promotions.Domain.Commands;
using Klippr_Backend.Promotions.Domain.ValueObjects;
using Klippr_Backend.Promotions.Interface.REST;
using Klippr_Backend.Promotions.Interface.Transform;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Klippr_Backend.Tests;

public class PromotionsImageKeyTests
{
    [Fact]
    public void CreatePromotionEndpointDocumentsCreatedAndBadRequestResponses()
    {
        var method = typeof(PromotionController).GetMethod(nameof(PromotionController.CreateAsync));

        Assert.NotNull(method);

        var responseTypes = method!
            .GetCustomAttributes(typeof(ProducesResponseTypeAttribute), inherit: false)
            .Cast<ProducesResponseTypeAttribute>()
            .ToList();

        Assert.Contains(responseTypes, responseType =>
            responseType.StatusCode == StatusCodes.Status201Created &&
            responseType.Type?.Name == "PromotionCreatedResource");
        Assert.Contains(responseTypes, responseType =>
            responseType.StatusCode == StatusCodes.Status400BadRequest);
    }

    [Theory]
    [InlineData(typeof(CreatePromotionResource))]
    [InlineData(typeof(UpdatePromotionResource))]
    [InlineData(typeof(PromotionResource))]
    [InlineData(typeof(Promotion))]
    public void PromotionContractsExposeNullableImageKey(Type contractType)
    {
        var imageKeyProperty = contractType.GetProperty("ImageKey");

        Assert.NotNull(imageKeyProperty);
        Assert.Equal(typeof(string), Nullable.GetUnderlyingType(imageKeyProperty!.PropertyType) ?? imageKeyProperty.PropertyType);
    }

    [Fact]
    public void CreateStoresValidImageKey()
    {
        var command = CreateCommand("comida_pizza");

        var promotion = Promotion.Create(command);

        Assert.Equal("comida_pizza", promotion.ImageKey);
    }

    [Fact]
    public void CreateResourceAssemblerCarriesImageKey()
    {
        var resource = new CreatePromotionResource(
            Guid.NewGuid(),
            "Promo pizza",
            "Pizza especial",
            50,
            "Percentage",
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7),
            25,
            "comida_pizza");

        var command = CreatePromotionCommandFromResourceAssembler.ToCommand(resource);

        Assert.Equal("comida_pizza", command.ImageKey);
    }

    [Fact]
    public void CreateRejectsUnknownImageKey()
    {
        var command = CreateCommand("imagen_inexistente");

        var exception = Assert.Throws<ArgumentException>(() => Promotion.Create(command));

        Assert.Contains("ImageKey", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CreateNormalizesWhitespaceImageKeyToNull()
    {
        var command = CreateCommand("   ");

        var promotion = Promotion.Create(command);

        Assert.Null(promotion.ImageKey);
    }

    [Fact]
    public void CreateAllowsMissingImageKeyForExistingCompatibility()
    {
        var command = CreateCommand(null);

        var promotion = Promotion.Create(command);

        Assert.Null(promotion.ImageKey);
    }

    [Fact]
    public void UpdateStoresValidImageKey()
    {
        var promotion = Promotion.Create(CreateCommand(null));
        var command = UpdateCommand(promotion.Id, "deportes_basket");

        promotion.Update(command);

        Assert.Equal("deportes_basket", promotion.ImageKey);
    }

    [Fact]
    public void UpdateResourceAssemblerCarriesImageKey()
    {
        var resource = new UpdatePromotionResource(
            "Promo actualizada",
            "Descripcion actualizada",
            40,
            "Percentage",
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7),
            15,
            "deportes_basket");

        var command = UpdatePromotionCommandFromResourceAssembler.ToCommand(Guid.NewGuid(), resource);

        Assert.Equal("deportes_basket", command.ImageKey);
    }

    [Fact]
    public void PromotionResourceAssemblerReturnsImageKey()
    {
        var promotion = Promotion.Create(CreateCommand("entretenimiento_cine"));

        var resource = PromotionResourceFromEntityAssembler.ToResource(promotion);

        Assert.Equal("entretenimiento_cine", resource.ImageKey);
    }

    [Fact]
    public void PromotionResourceAssemblerCarriesBusinessName()
    {
        var promotion = Promotion.Create(CreateCommand(null));

        var resource = PromotionResourceFromEntityAssembler.ToResource(promotion, "Pizzeria Central");

        Assert.Equal("Pizzeria Central", resource.BusinessName);
    }

    [Fact]
    public void UpdateRejectsUnknownImageKey()
    {
        var promotion = Promotion.Create(CreateCommand(null));
        var command = UpdateCommand(promotion.Id, "otro_banner");

        var exception = Assert.Throws<ArgumentException>(() => promotion.Update(command));

        Assert.Contains("ImageKey", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    private static CreatePromotionCommand CreateCommand(string? imageKey) => new(
        Guid.NewGuid(),
        "Promo pizza",
        "Pizza especial",
        new DiscountValue(50, DiscountType.Percentage),
        new TimeFrame(DateTime.UtcNow, DateTime.UtcNow.AddDays(7)),
        25,
        imageKey);

    private static UpdatePromotionCommand UpdateCommand(Guid promotionId, string? imageKey) => new(
        promotionId,
        Guid.NewGuid(),
        "Promo actualizada",
        "Descripcion actualizada",
        new DiscountValue(40, DiscountType.Percentage),
        new TimeFrame(DateTime.UtcNow, DateTime.UtcNow.AddDays(7)),
        15,
        imageKey);
}
