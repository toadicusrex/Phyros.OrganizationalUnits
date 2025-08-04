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
	[InlineData(".test.")]
	[InlineData("a..b")]
	[InlineData("a. .b")]
	[InlineData(".")]
	public void Throws_on_empty_nodes(string organizationalUnitString)
	{
		Should.Throw<ArgumentException>(() => OrganizationalUnit.Parse(organizationalUnitString));
	}

	[Theory]
	[InlineData(null, 0)]
	[InlineData("Core", 0)]
	[InlineData("", 0)]
	[InlineData("test", 1, "test")]
	[InlineData("world.country.region.city.department.unit", 6, "world", "country", "region", "city", "department", "unit")]
	[InlineData("   ", 0)]
	[InlineData("a.b-c.d_e", 3, "a", "b-c", "d_e")]
	[InlineData("a.b.c.d.e.f.g.h.i.j.k.l.m.n.o.p.q.r.s.t", 20, "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t")]
	[InlineData("world.country.region.city", 4, "world", "country", "region", "city")]
	public void Correctly_calculates_nodes(string? organizationalUnitString, int expectedCount, params string[] expectedNodeValues)
	{
		var organizationalUnit = OrganizationalUnit.Parse(organizationalUnitString);
		organizationalUnit.Nodes.Length.ShouldBe(expectedCount);
		for (var position = 0; position < expectedNodeValues.Length; position++)
		{
			organizationalUnit.Nodes[position].ShouldBe(expectedNodeValues[position]);
			_testOutputHelper.WriteLine($"Position {position} is correct, value '{organizationalUnit.Nodes[position]}'");
		}
	}
}
