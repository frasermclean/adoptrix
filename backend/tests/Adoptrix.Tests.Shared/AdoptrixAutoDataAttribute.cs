namespace Adoptrix.Tests.Shared;

public class AdoptrixAutoDataAttribute()
    : AutoDataAttribute(() => new Fixture().Customize(new CompositeCustomization(
        new AutoMoqCustomization(),
        new TwoYearsAgoDateOnlyCustomization())));
