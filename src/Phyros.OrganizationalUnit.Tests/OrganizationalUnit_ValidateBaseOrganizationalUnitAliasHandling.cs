using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Xunit.Abstractions;

namespace Phyros.OrganizationalUnits.Tests;
public class OrganizationalUnit_ValidateBaseOrganizationalUnitAliasHandling
{
	private readonly ITestOutputHelper _testOutputHelper;

	public OrganizationalUnit_ValidateBaseOrganizationalUnitAliasHandling(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}
	[Theory]
	[InlineData("", OrganizationalUnitConfig.DefaultBaseOrganizationalUnitAlias, true)]
	[InlineData(OrganizationalUnitConfig.DefaultBaseOrganizationalUnitAlias, "", true)]
	[InlineData("Core.Region.City.Facility", "Region.City.Facility", true)]
	public void ValidateBaseOrganizationalUnitAliasHandling(string organizationalUnitString, string expectedMatch, bool shouldMatch)
	{
		// Arrange
		var config = new OrganizationalUnitConfig
		{
			BaseOrganizationalUnitAlias = OrganizationalUnitConfig.DefaultBaseOrganizationalUnitAlias,
		};
		var organizationalUnit = new OrganizationalUnit(organizationalUnitString, config);
		var matchOrganizationalUnit = new OrganizationalUnit(expectedMatch, config);
		_testOutputHelper.WriteLine($"OrganizationalUnit: \"{organizationalUnit}\", number of elements: {organizationalUnit.Nodes.Count()}");
		_testOutputHelper.WriteLine($"MatchOrganizationalUnit: \"{matchOrganizationalUnit}\", number of elements: {matchOrganizationalUnit.Nodes.Count()}");

		// Act
		var isMatch = organizationalUnit.Equals(matchOrganizationalUnit);

		// Assert
		isMatch.ShouldBe(shouldMatch);
		
		if (shouldMatch)
		{
			var alias = organizationalUnit.ToString();
			alias.ShouldBe(matchOrganizationalUnit.ToString());
		}
		else
		{
			Action act = () => organizationalUnit.ToString();
			act.ShouldThrow<InvalidOperationException>();
		}
	}
}
