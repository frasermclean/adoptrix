namespace Adoptrix.Tests.Shared;

public class MoqAutoDataAttribute()
    : AutoDataAttribute(() => new Fixture().Customize(new AutoMoqCustomization()));
