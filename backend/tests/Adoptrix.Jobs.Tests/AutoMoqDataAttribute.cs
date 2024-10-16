using AutoFixture;
using AutoFixture.AutoMoq;

namespace Adoptrix.Jobs.Tests;

public class AutoMoqDataAttribute()
    : AutoDataAttribute(() => new Fixture().Customize(new AutoMoqCustomization()));
