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
	// make sure the last node is an empty string to represent the "base" node.
	[InlineData("Core", "")]
	[InlineData("one", "One", "")]
	[InlineData("two.one", "One", "Two", "")]
	[InlineData("three.two.one", "One", "Two", "Three", "")]
	public void Properly_serializes(string expectedValue, params string[] nodes)
	{
		var organizationalUnit = new OrganizationalUnit(nodes);
		organizationalUnit.ToUrlString().ShouldBe(expectedValue, $" the value is \"{organizationalUnit.ToUrlString()}\"");
	}
}