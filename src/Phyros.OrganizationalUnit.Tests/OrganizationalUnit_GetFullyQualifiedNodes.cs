using Shouldly;
using Xunit.Abstractions;

namespace Phyros.OrganizationalUnits.Tests;

public class OrganizationalUnit_GetFullyQualifiedNodes
{
	private readonly ITestOutputHelper _testOutputHelper;

	public OrganizationalUnit_GetFullyQualifiedNodes(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}

	[Theory]
	[InlineData(null, 1)]
	[InlineData("Core", 1)]
	[InlineData("", 1)]
	[InlineData("test", 2, "test")]
	[InlineData("world.country.region.city.department.unit", 7, "world.country.region.city.department.unit", "world.country.region.city.department", "world.country.region.city", "world.country.region", "world.country", "world")]
	public void Correctly_calculates_fully_qualified_nodes(string? organizationalUnitString, int expectedCount, params string[] expectedNodeValues)
	{
		var organizationalUnit = OrganizationalUnit.Parse(organizationalUnitString);
		organizationalUnit.Nodes.Length.ShouldBe(expectedCount);
		var fullyQualifiedNodes = organizationalUnit.GetFullyQualifiedNodes();
		for (var position = 0; position < expectedNodeValues.Length; position++)
		{
			fullyQualifiedNodes[position].ShouldBe(expectedNodeValues[position]);
			_testOutputHelper.WriteLine($"Position {position} is correct, value '{fullyQualifiedNodes[position]}'");
		}
		// verify that the last node is the empty "base" node
		fullyQualifiedNodes.Last().ShouldBe(string.Empty);
	}
}