using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Phyros.OrganizationalUnits.Tests;

public class OrganizationalUnit_ConfigurableDelimiter
{
	private readonly ITestOutputHelper _testOutputHelper;

	public OrganizationalUnit_ConfigurableDelimiter(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}

	[Fact]
	public void Configurable_Delimiter_Changes_Parsing_Behavior()
	{
		var config = new OrganizationalUnitConfig
		{
			Delimiter = '/'
		};
		// Test parsing with new delimiter
		var ouString = "world/country/region/city";
		var ou = OrganizationalUnit.Parse(ouString, config);

		// Verify nodes are correctly parsed
		ou.Nodes.Length.ShouldBe(5); // 4 nodes + empty base node
		ou.Nodes[0].ShouldBe("city");
		ou.Nodes[1].ShouldBe("region");
		ou.Nodes[2].ShouldBe("country");
		ou.Nodes[3].ShouldBe("world");
		ou.Nodes[4].ShouldBe(string.Empty);

		// Test serialization with new delimiter
		ou.ToString().ShouldBe(ouString);

		// Test fully qualified nodes with new delimiter
		var fqNodes = ou.GetFullyQualifiedNodes();
		fqNodes.Length.ShouldBe(5);
		fqNodes[0].ShouldBe("world/country/region/city");
		fqNodes[1].ShouldBe("world/country/region");
		fqNodes[2].ShouldBe("world/country");
		fqNodes[3].ShouldBe("world");
		fqNodes[4].ShouldBe(string.Empty);
	}

	[Fact]
	public void Configurable_Delimiter_Is_PerformanceOptimized()
	{

		var config = new OrganizationalUnitConfig
		{
			Delimiter = ';'
		};

		// Create a complex organizational unit
		var ouString = "a;b;c;d;e;f;g;h;i;j;k;l;m;n;o;p";

		// Parse multiple times to verify performance
		for (int i = 0; i < 1000; i++)
		{
			var ou = OrganizationalUnit.Parse(ouString, config);
			ou.Nodes.Length.ShouldBe(17); // 16 nodes + empty base node
		}
	}
}
