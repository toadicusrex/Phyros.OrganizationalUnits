using Shouldly;
using Xunit.Abstractions;

namespace Phyros.OrganizationalUnits.Tests;

public class OrganizationalUnit_ToString
{
	private readonly ITestOutputHelper _testOutputHelper;

	public OrganizationalUnit_ToString(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}

	[Theory]
	// make sure the last node is an empty string to represent the "base" node.
	[InlineData("", "")]
	[InlineData("one", "One", "")]
	[InlineData("one.two", "One", "Two", "")]
	[InlineData("one.two.three", "One", "Two", "Three", "")]
	public void Properly_serializes(string expectedValue, params string[] nodes)
	{
		var organizationalUnit = new OrganizationalUnit(nodes);
		organizationalUnit.ToString().ShouldBe(expectedValue, $"the value is \"{organizationalUnit.ToUrlString()}\"");
		_testOutputHelper.WriteLine(organizationalUnit.ToUrlString());
	}
}
