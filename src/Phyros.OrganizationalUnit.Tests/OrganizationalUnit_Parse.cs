using Shouldly;
using Xunit.Abstractions;

namespace Phyros.OrganizationalUnits.Tests;

public class OrganizationalUnit_Parse
{
	private readonly ITestOutputHelper _testOutputHelper;

	public OrganizationalUnit_Parse(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}


	[Theory]
	[InlineData("test.")]
	[InlineData("a..b")]
	[InlineData(".a.b")]
	[InlineData("a. .b")]
	[InlineData(".")]
	public void Throws_on_empty_nodes(string organizationalUnitString)
	{
		Should.Throw<ArgumentException>(() => OrganizationalUnit.Parse(organizationalUnitString));
	}

	[Theory]
	[InlineData(null, 1)]
	[InlineData("Core", 1)]
	[InlineData("", 1)]
	[InlineData("test", 2, "test", "")]
	[InlineData("world.country.region.city.department.unit", 7, "unit", "department", "city", "region", "country", "world", "")]
	[InlineData("   ", 1, "")]
	[InlineData("a.b-c.d_e", 4, "d_e", "b-c", "a", "")]
	[InlineData("a.b.c.d.e.f.g.h.i.j.k.l.m.n.o.p.q.r.s.t", 21, "t", "s", "r", "q", "p", "o", "n", "m", "l", "k", "j", "i", "h", "g", "f", "e", "d", "c", "b", "a", "")]
	[InlineData("world.country.region.city", 5, "city", "region", "country", "world", "")]
	public void Correctly_calculates_nodes(string? organizationalUnitString, int expectedCount, params string[] expectedNodeValues)
	{
		var organizationalUnit = OrganizationalUnit.Parse(organizationalUnitString);
		organizationalUnit.Nodes.Length.ShouldBe(expectedCount);
		for (var position = 0; position < expectedNodeValues.Length; position++)
		{
			organizationalUnit.Nodes[position].ShouldBe(expectedNodeValues[position]);
			_testOutputHelper.WriteLine($"Position {position} is correct, value '{organizationalUnit.Nodes[position]}'");
		}

		// verify that the first node is the base "empty" node
		organizationalUnit.Nodes.Last().ShouldBe(string.Empty);
	}

}