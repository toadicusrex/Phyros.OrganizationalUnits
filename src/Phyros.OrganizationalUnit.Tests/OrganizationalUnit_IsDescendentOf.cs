using Shouldly;

namespace Phyros.OrganizationalUnits.Tests;

public class OrganizationalUnit_IsDescendantOf
{
	[Theory]
	[InlineData("", "", false)]
	[InlineData("", "fakeparent", false)]
	[InlineData(OrganizationalUnitConfig.DefaultBaseOrganizationalUnitAlias, OrganizationalUnitConfig.DefaultBaseOrganizationalUnitAlias, false)]
	[InlineData(OrganizationalUnitConfig.DefaultBaseOrganizationalUnitAlias, "fakeparent", false)]
	[InlineData("fakeparent", "", true)]
	[InlineData("fakeparent", OrganizationalUnitConfig.DefaultBaseOrganizationalUnitAlias, true)]
	[InlineData("fakeparent.fakechild", "fakeparent", true)]
	[InlineData("fakeparent", "fakeparent.fakechild", false)]
	public void IsDescendantOf_calculates_properly(string targetOuString, string potentialParentOuString, bool shouldBeDescendant)
	{
		// arrange
		var targetOu = OrganizationalUnit.Parse(targetOuString);
		var potentialOu = OrganizationalUnit.Parse(potentialParentOuString);

		// act
		var result = targetOu.IsDescendantOf(potentialOu);

		// assert
		result.ShouldBe(shouldBeDescendant);
	}
}
