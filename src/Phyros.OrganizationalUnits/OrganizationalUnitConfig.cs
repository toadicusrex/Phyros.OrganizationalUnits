namespace Phyros.OrganizationalUnits;

public class OrganizationalUnitConfig
{
	public static OrganizationalUnitConfig Default { get; } = new OrganizationalUnitConfig();
	public static void SetDefault(string baseOrganizationalUnit = DefaultBaseOrganizationalUnit, char delimiter = DefaultDelimiter)
	{
		Default.BaseOrganizationalUnit = baseOrganizationalUnit;
		Default.Delimiter = delimiter;
	}
	/// <summary>Gets or sets the base organizational unit, considered the parent or host organization which becomes the parent of all other organizations.</summary>
	/// <value>The base organizational unit.</value>
	public string BaseOrganizationalUnit { get; set; } = DefaultBaseOrganizationalUnit;
	public const string DefaultBaseOrganizationalUnit = "Core";
	/// <summary>Gets or sets the delimiter that is used between organizational unit nodes, default is '.'.</summary>
	/// <value>The delimiter.</value>
	public char Delimiter { get; set; } = DefaultDelimiter;
	public const char DefaultDelimiter = '.';
}
