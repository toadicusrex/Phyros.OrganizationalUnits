namespace Phyros.OrganizationalUnits;

public static class OrganizationalUnitExtensions
{
	/// <summary>Determines whether [is descendant of] [the specified potential parent].</summary>
	/// <param name="orgUnitInQuestion">The organizational unit.</param>
	/// <param name="potentialParent">The potential parent.</param>
	/// <returns>
	///   <c>true</c> if [is descendant of] [the specified potential parent]; otherwise, <c>false</c>.</returns>
	public static bool IsDescendantOf(this OrganizationalUnit orgUnitInQuestion, OrganizationalUnit potentialParent)
	{
		if (orgUnitInQuestion.Nodes.Length < potentialParent.Nodes.Length) return false;
		int offset = orgUnitInQuestion.Nodes.Length - potentialParent.Nodes.Length;
		for (int i = 0; i < potentialParent.Nodes.Length; i++)
		{
			if (!string.Equals(orgUnitInQuestion.Nodes[offset + i], potentialParent.Nodes[i], StringComparison.Ordinal))
				return false;
		}
		return true;
	}

	/// <summary>Determines whether [is ancestor of] [the specified potential descendant].</summary>
	/// <param name="orgUnitInQuestion">The organizational unit.</param>
	/// <param name="potentialDescendent">The potential parent.</param>
	/// <returns>
	///   <c>true</c> if [is descendant of] [the specified potential parent]; otherwise, <c>false</c>.</returns>
	public static bool IsAncestorOf(this OrganizationalUnit orgUnitInQuestion, OrganizationalUnit potentialDescendent)
	{
		return potentialDescendent.IsDescendantOf(orgUnitInQuestion);
	}

	/// <summary>
	///   <para>
	/// Determines whether [is a direct child of] [the specified potential parent], i.e. not a descendent more than one node away, but is a descendent and is not the parent itself.</para>
	/// </summary>
	/// <param name="orgUnitInQuestion">The organizational unit.</param>
	/// <param name="potentialParent">The potential parent.</param>
	/// <returns>
	///   <c>true</c> if [is child of] [the specified potential parent]; otherwise, <c>false</c>.</returns>
	public static bool IsChildOf(this OrganizationalUnit orgUnitInQuestion, OrganizationalUnit potentialParent)
	{
		// verify that child nodes is exactly one node longer than parent nodes and that it's a descendant of the potential parent
		return orgUnitInQuestion.Nodes.Length == potentialParent.Nodes.Length + 1 && orgUnitInQuestion.IsDescendantOf(potentialParent);
	}

	/// <summary>
	///   <para> Determines whether [is a direct parent of] [the specified potential child], i.e. not a parent more than one node away, but is a parent and is not the child itself.</para> 
	/// </summary>
	///		<param name="orgUnitInQuestion">The organizational unit.</param>
	///		<param name="potentialChild">The potential child.</param>
	///		<returns>
	///   <c>true</c> if [is parent of] [the specified potential child]; otherwise, <c>false</c>.</returns>
	public static bool IsParentOf(this OrganizationalUnit orgUnitInQuestion, OrganizationalUnit potentialChild)
	{
		return potentialChild.IsChildOf(orgUnitInQuestion);
	}
}
