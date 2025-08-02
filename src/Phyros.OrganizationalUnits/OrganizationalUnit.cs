namespace Phyros.OrganizationalUnits;

public class OrganizationalUnit
{
	/// <summary>
	/// Nodes of the organizational unit are ordered from least specific to most specific, 
	/// i.e. ["country", "state", "city", "household", "child"] etc.
	/// Nodes represent units of organization/containment. The base organizational unit 
	/// is represented by an empty array.
	/// </summary>
	private readonly string[] _nodes; // Private backing field	
	
	/// <summary>
	/// Gets the nodes of this organizational unit. All nodes are stored in lowercase for consistent case-insensitive behavior.
	/// </summary>
	public string[] Nodes => _nodes;	

	private readonly OrganizationalUnitConfig _config;

	/// <summary>Initializes a new instance of the <see cref="OrganizationalUnit" /> targeted to the "Empty" organizational unit that represents the base.</summary>
	public OrganizationalUnit(OrganizationalUnitConfig? config = null) : this(Array.Empty<string>(), config)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="OrganizationalUnit" /> class from a string representation.</summary>
	/// <param name="organizationalUnitString">The organizational unit string to parse.</param>
	/// <param name="config"></param>
	public OrganizationalUnit(string organizationalUnitString, OrganizationalUnitConfig? config = null) 
		: this(ParseToArray(organizationalUnitString, config), config)
	{
	}

	internal OrganizationalUnit(IEnumerable<string> nodes, OrganizationalUnitConfig? config = null)
	{
		_config = config ?? OrganizationalUnitConfig.Default;
		
		// Handle normalization at construction time
		var nodeArray = nodes?.ToArray() ?? Array.Empty<string>();
		
		// Handle old format where empty node was at the end
		if (nodeArray.Length > 0 && string.IsNullOrEmpty(nodeArray[nodeArray.Length - 1]))
		{
			// Create a new array without the empty node at the end
			var correctedArray = new string[nodeArray.Length - 1];
			if (nodeArray.Length > 1)
			{
				Array.Copy(nodeArray, 0, correctedArray, 0, nodeArray.Length - 1);
			}
			nodeArray = correctedArray;
		}
		
		// Apply ToLower for case-insensitivity
		_nodes = nodeArray.Select(x => x?.ToLower() ?? string.Empty).ToArray();
	}

	/// <summary>
	/// Gets fully qualified nodes in order from most complete to least complete in a qualified format.  
	/// For nodes ["country", "state", "city"] this would become the fully qualified nodes 
	/// ["", "country", "country.state", "country.state.city"]
	/// Note that the empty string as the first element represents the base.
	/// </summary>
	/// <returns></returns>
	public string[] GetFullyQualifiedNodes()
	{
		// Create an array with room for all node combinations plus the empty string at the beginning
		var result = new string[_nodes.Length + 1];
		
		// First element is always the empty base node
		result[0] = string.Empty;
		
		// Generate fully qualified paths, from least specific to most specific
		for (int i = 0; i < _nodes.Length; i++)
		{
			// For each iteration, take the first (i+1) nodes
			var nodesToInclude = _nodes.Take(i + 1);
			
#if NETSTANDARD2_0
			result[i + 1] = string.Join(_config.Delimiter.ToString(), nodesToInclude);
#else
			result[i + 1] = string.Join(_config.Delimiter, nodesToInclude);
#endif
		}
		
		return result;
	}

	/// <summary>
	/// Converts an organizational unit to a URL friendly structure.
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		if (_nodes.Length == 0)
			return string.Empty;
		
#if NETSTANDARD2_0
        return string.Join(_config.Delimiter.ToString(), _nodes);
#else
		return string.Join(_config.Delimiter, _nodes);
#endif
	}

	/// <summary>
	/// Converts an organizational unit to a URL friendly structure (i.e. the root node is replaced by <see cref="OrganizationalUnitConfig.BaseOrganizationalUnitAlias"/>).  
	/// </summary>
	/// <returns></returns>
	public string ToUrlString()
	{
		var serialized = ToString();
		if (string.IsNullOrEmpty(serialized))
		{
			return _config.BaseOrganizationalUnitAlias;
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
	public static implicit operator OrganizationalUnit(string organizationalUnitString) => new OrganizationalUnit(ParseToArray(organizationalUnitString), OrganizationalUnitConfig.Default);

	/// <summary>
	/// Implicitly converts an <see cref="OrganizationalUnit" /> to its string representation.
	/// </summary>
	/// <param name="organizationalUnit">The organizational unit.</param>
	public static implicit operator string(OrganizationalUnit organizationalUnit) => organizationalUnit.ToString();

	/// <summary>
	/// Parses an organizational unit string into an array of strings. Nodes in the organizational unit are ordered from least specific to most specific,
	/// describing "containment" (i.e. node 1 is contained by node 2, etc.).
	/// </summary>
	/// <param name="organizationalUnitString">The organizational unit string.</param>
	/// <param name="config"></param>
	/// <returns>Array of nodes</returns>
	/// <exception cref="ArgumentException">Thrown when any node is empty or whitespace.</exception>
	private static string[] ParseToArray(string? organizationalUnitString, OrganizationalUnitConfig? config = null)
	{
		config ??= OrganizationalUnitConfig.Default;
		if (string.IsNullOrWhiteSpace(organizationalUnitString))
		{
			return Array.Empty<string>();
		}

		// Direct match with base alias case - use case-insensitive comparison
		if (string.Equals(organizationalUnitString, config.BaseOrganizationalUnitAlias, StringComparison.OrdinalIgnoreCase))
		{
			return Array.Empty<string>();
		}

		// Split the string into parts
		var parts = organizationalUnitString!.Split(config.Delimiter);
    
		// Check if the first part is the base organizational unit alias or an empty string
		bool startsWithBaseOU = parts.Length > 0 && 
			(string.Equals(parts[0], config.BaseOrganizationalUnitAlias, StringComparison.OrdinalIgnoreCase) || 
			 string.IsNullOrEmpty(parts[0]));
    
		// Check for empty nodes except when it's the first node and we're dealing with a base OU alias
		for (int i = 0; i < parts.Length; i++)
		{
			if (string.IsNullOrWhiteSpace(parts[i]) && !(i == 0 && startsWithBaseOU))
			{
				throw new ArgumentException(
					"Organizational unit string cannot have empty nodes, except for the root node.", 
					nameof(organizationalUnitString));
			}
		}
		
		// Skip the first part if it's the base OU alias or empty
		return startsWithBaseOU ? parts.Skip(1).ToArray() : parts;
	}

	public static OrganizationalUnit Parse(string? organizationalUnitString, OrganizationalUnitConfig? config = null)
	{
		return new OrganizationalUnit(ParseToArray(organizationalUnitString, config), config);
	}

	/// <inheritdoc />
	public override bool Equals(object? obj)
	{
		if (obj is not OrganizationalUnit other)
			return false;

		if (_nodes.Length != other._nodes.Length)
			return false;

		// Since nodes are already lowercased when stored, we can use ordinal comparison
		for (int i = 0; i < _nodes.Length; i++)
		{
			if (_nodes[i] != other._nodes[i])
				return false;
		}

		return true;
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		// Since nodes are already lowercased, we can use the standard GetHashCode
		int hash = 17;
		foreach (var node in _nodes)
		{
			hash = hash * 31 + (node?.GetHashCode() ?? 0);
		}
		return hash;
	}
}
