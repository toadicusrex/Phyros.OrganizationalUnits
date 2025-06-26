using Shouldly;

namespace Phyros.OrganizationalUnits.Tests;

public class OrganizationalUnit_IsDescendantOf
{
	[Theory]
	[InlineData("", "", true)]
	[InlineData("", "fakeparent", false)]
	[InlineData("Core", "Core", true)]
	[InlineData("Core", "fakeparent", false)]
	[InlineData("fakeparent", "", true)]
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