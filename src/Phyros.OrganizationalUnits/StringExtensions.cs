namespace Phyros.OrganizationalUnits;

public static class StringExtensions
{
	/// <summary>
	/// Parses an organizational unit string into the OrganizationalUnit structure.  Nodes in the organizational unit are ordered from most specific to least specific, describing "containment" (i.e. node 1 is contained by node 2, etc.).  Note that the "base" node is always the last node in the Organizational Unit, suggesting that all Organizational Units fall under the base container.
	/// </summary>
	/// <param name="organizationalUnitString">The organizational unit string.</param>
	/// <param name="config"></param>
	/// <returns>
	///   <br />
	/// </returns>
	public static OrganizationalUnit ToOrganizationalUnit(this string organizationalUnitString, OrganizationalUnitConfig? config = null)
	{
		return new OrganizationalUnit(organizationalUnitString, config);
	}
}
