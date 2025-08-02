using Shouldly;
using Xunit.Abstractions;

namespace Phyros.OrganizationalUnits.Tests;

public class OrganizationalUnit_ToUrlString
{
	private readonly ITestOutputHelper _testOutputHelper;

	public OrganizationalUnit_ToUrlString(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}

	[Theory]
	// Now the empty base node should be at the beginning, not the end
	[InlineData("core", "core")]
	[InlineData("one", "one")]
	[InlineData("one.two", "one", "two")]
	[InlineData("one.two.three", "one", "two", "three")]
	public void Properly_serializes(string expectedValue, params string[] nodes)
	{
		var organizationalUnit = new OrganizationalUnit(nodes);
		organizationalUnit.ToUrlString().ShouldBe(expectedValue, $" the value is \"{organizationalUnit.ToUrlString()}\"");
	}
}
