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
	[InlineData(null, 0, "")]
	[InlineData("Core", 0, "")]
	[InlineData("", 0, "")]
	[InlineData("test", 1, "", "test")]
	[InlineData("world.country.region.city.department.unit", 6, "", "world", "world.country", "world.country.region", "world.country.region.city", "world.country.region.city.department", "world.country.region.city.department.unit")]
	public void Correctly_calculates_fully_qualified_nodes(string? organizationalUnitString, int expectedCount, params string[] expectedNodeValues)
	{
		var organizationalUnit = OrganizationalUnit.Parse(organizationalUnitString);
		organizationalUnit.Nodes.Length.ShouldBe(expectedCount);
		var fullyQualifiedNodes = organizationalUnit.GetFullyQualifiedNodes();
		for (var position = 0; position < expectedNodeValues.Length; position++)
		{
			fullyQualifiedNodes[position].ShouldBe(expectedNodeValues[position]);
			_testOutputHelper.WriteLine($"Fully qualified node at node {position} is correct, value '{fullyQualifiedNodes[position]}'");
		}
		// verify that the last node is the empty "base" node
		fullyQualifiedNodes.First().ShouldBe(string.Empty);
	}
}
