using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Adoptrix.Jobs.Tests.Fixtures;

public class CustomizedAutoDataAttribute()
    : AutoDataAttribute(() => new Fixture().Customize(new AutoMoqCustomization()));
