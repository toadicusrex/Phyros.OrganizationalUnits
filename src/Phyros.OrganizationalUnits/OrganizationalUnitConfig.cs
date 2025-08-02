namespace Phyros.OrganizationalUnits;

public class OrganizationalUnitConfig
{
	public static OrganizationalUnitConfig Default { get; } = new OrganizationalUnitConfig();
	public static void SetDefault(string? baseOrganizationalUnitAlias = DefaultBaseOrganizationalUnitAlias, char? delimiter = DefaultDelimiter)
	{
		Default.BaseOrganizationalUnitAlias = (baseOrganizationalUnitAlias ?? DefaultBaseOrganizationalUnitAlias).ToLower();
		Default.Delimiter = delimiter ?? DefaultDelimiter;
	}
	/// <summary>Gets or sets the base organizational unit alias, considered the parent or host organization which becomes the parent of all other organizations.</summary>
	/// <value>The base organizational unit alias.</value>
	public string BaseOrganizationalUnitAlias { get; set; } = DefaultBaseOrganizationalUnitAlias.ToLower();
	public const string DefaultBaseOrganizationalUnitAlias = "core";
	/// <summary>Gets or sets the delimiter that is used between organizational unit nodes, default is '.'.</summary>
	/// <value>The delimiter.</value>
	public char Delimiter { get; set; } = DefaultDelimiter;
	public const char DefaultDelimiter = '.';
}
