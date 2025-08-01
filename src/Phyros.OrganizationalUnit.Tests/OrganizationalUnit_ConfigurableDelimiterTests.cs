﻿using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Phyros.OrganizationalUnits.Tests;

public class OrganizationalUnit_ConfigurableDelimiterTests
{
	private readonly ITestOutputHelper _testOutputHelper;

	public OrganizationalUnit_ConfigurableDelimiterTests(ITestOutputHelper testOutputHelper)
	{
		_testOutputHelper = testOutputHelper;
	}

	[Fact]
	public void Configurable_Delimiter_Changes_Parsing_Behavior()
	{
		// Change delimiter to slash
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

	}
}
