using Azure.Identity;

namespace Adoptrix.Logic.Tests;

public class TokenCredentialFactoryTests
{
    [Fact]
    public void Create_WhenCalled_ReturnsChainedTokenCredential()
    {
        // act
        var credential = TokenCredentialFactory.Create();

        // assert
        credential.Should().BeOfType<ChainedTokenCredential>();
    }
}
