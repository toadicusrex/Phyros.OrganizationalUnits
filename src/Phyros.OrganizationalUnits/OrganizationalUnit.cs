namespace Phyros.OrganizationalUnits;

public class OrganizationalUnit
{
	/// <summary>Nodes of the organizational unit are ordered from most specific to least specific, i.e. ["child", "household", "city", "state", "country", "continent", "planet"] etc.
	/// Nodes represent units of organization/containment.  Note that the empty "base" node is always last.</summary>
	public readonly string[] Nodes;
	private readonly OrganizationalUnitConfig _config;

	/// <summary>Initializes a new instance of the <see cref="OrganizationalUnit" /> targeted to the "Empty" organizational unit that represents the base.</summary>
	public OrganizationalUnit(OrganizationalUnitConfig? config = null) : this([], config)
	{
		
	}

	/// <summary>Initializes a new instance of the <see cref="OrganizationalUnit" /> class from a string representation.</summary>
	/// <param name="organizationalUnitString">The organizational unit string to parse.</param>
	/// <param name="config"></param>
	public OrganizationalUnit(string organizationalUnitString, OrganizationalUnitConfig? config = null) : this(Parse(organizationalUnitString).Nodes, config)
	{
	}

	internal OrganizationalUnit(IEnumerable<string> nodes, OrganizationalUnitConfig? config = null)
	{
		_config = config ?? OrganizationalUnitConfig.Default;
		Nodes = [.. nodes.Select(x => x.ToLower())];
	}
	/// <summary>
	/// Gets fully qualified nodes in order from least specific to most specific in a qualified format.  ["child", "household", "city"] would become the fully qualified nodes ["city.household.child", "city.household", "city", ""]
	/// Note the empty node representing base as the final node.
	/// </summary>
	/// <returns></returns>
	public string[] GetFullyQualifiedNodes()
	{
		if (Nodes.Length == 1)
		{
			return [string.Empty];
		}
		var fullyQualifiedNodes = new List<string>();
		var reversed = Nodes.Reverse().Skip(1).ToList();
		var count = reversed.Count;
		for (var i = 0; i < count; i++)
		{
#if NETSTANDARD2_0
			fullyQualifiedNodes.Add(string.Join(_config.Delimiter.ToString(), reversed.Take(count - i)));
#else
			fullyQualifiedNodes.Add(string.Join(_config.Delimiter, reversed.Take(count - i)));
#endif
		}
		// add the base node
		fullyQualifiedNodes.Add(String.Empty);
		return [.. fullyQualifiedNodes];
	}

	/// <summary>
	/// Converts an organizational unit to a URL friendly structure (i.e. the base node is an empty string).  
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		if (Nodes.Length == 1)
		{
			return String.Empty;
		}
		var reversed = Nodes.Reverse().Skip(1).ToList();
#if NETSTANDARD2_0
		return Nodes.Length == 1 ? Nodes[0] : string.Join(_config.Delimiter.ToString(), reversed);
#else
		return Nodes.Length == 1 ? Nodes[0] : string.Join(_config.Delimiter, reversed);
#endif
	}

	/// <summary>
	/// Converts an organizational unit to a URL friendly structure (i.e. the root node is replaced by <see cref="OrganizationalUnitConfig.BaseOrganizationalUnit"/>).  
	/// </summary>
	/// <returns></returns>
	public string ToUrlString()
	{
		var serialized = ToString();
		if (string.IsNullOrEmpty(serialized))
		{
			return _config.BaseOrganizationalUnit;
		}
		else
		{
			return serialized;
		}
	}

	/// <summary>
	/// Implicitly converts a string to an <see cref="OrganizationalUnit" /> by parsing it.
	/// </summary>
	/// <param name="organizationalUnitString">The organizational unit string.</param>
	public static implicit operator OrganizationalUnit(string organizationalUnitString) => Parse(organizationalUnitString);

	/// <summary>
	/// Implicitly converts an <see cref="OrganizationalUnit" /> to its string representation.
	/// </summary>
	/// <param name="organizationalUnit">The organizational unit.</param>
	public static implicit operator string(OrganizationalUnit organizationalUnit) => organizationalUnit.ToString();

	/// <summary>
	/// Parses an organizational unit string into the OrganizationalUnit structure.  Nodes in the organizational unit are ordered from most specific to least specific, describing "containment" (i.e. node 1 is contained by node 2, etc.).  Note that the "base" node is always the last node in the Organizational Unit, suggesting that all Organizational Units fall under the base container.
	/// </summary>
	/// <param name="organizationalUnitString">The organizational unit string.</param>
	/// <param name="config"></param>
	/// <returns>
	///   <br />
	/// </returns>
	public static OrganizationalUnit Parse(string? organizationalUnitString, OrganizationalUnitConfig? config = null)
	{
		config ??= OrganizationalUnitConfig.Default;
		if (string.IsNullOrWhiteSpace(organizationalUnitString))
		{
			return new OrganizationalUnit([string.Empty], config);
		}

		// At this point, organizationalUnitString is not null, empty, or whitespace
		var orgUnitStr = organizationalUnitString!;
		if (orgUnitStr.Equals(config.BaseOrganizationalUnit, StringComparison.InvariantCultureIgnoreCase))
		{
			return new OrganizationalUnit([string.Empty], config);
		}
		var split = orgUnitStr.ToLower().Split(config.Delimiter).Reverse().ToList();
		// Check for empty or whitespace nodes (except for the final base node)
		if (split.Any(n => string.IsNullOrWhiteSpace(n)))
		{
			throw new ArgumentException("Organizational unit string contains empty or whitespace node(s).", nameof(organizationalUnitString));
		}
		split.Add(string.Empty);
		return new OrganizationalUnit([.. split], config);
	}

	/// <inheritdoc />
	public override bool Equals(object? obj)
	{
		return obj is OrganizationalUnit && String.Equals(ToString(), obj.ToString());
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		return Nodes.GetHashCode();
	}
}
