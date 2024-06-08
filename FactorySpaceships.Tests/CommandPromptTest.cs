using FactorySpaceships.Models;

namespace FactorySpaceships.Tests;

public class CommandPromptTest
{
    [Fact]
    public void TestExtractCommandWithArguments()
    {
        string input = "[USER_INSTRUCTION] 10 Vaisseau1, 20 Vaisseau2";
        
        string command = CommandPrompt.ExtractValidCommandFromInput(input);
        
        Assert.Equal("[USER_INSTRUCTION]", command);
    }
    
    [Fact]
    public void TestExtractArgumentsWithMultipleValidEntries()
    {
        string input = "COMMAND 10 Vaisseau1, 20 Vaisseau2, 30 Vaisseau3";
        
        string[] arguments = CommandPrompt.ExtractValidArgumentsFromInput(input);
        
        Assert.Equal(3, arguments.Length);
        Assert.Equal("10 Vaisseau1", arguments[0]);
        Assert.Contains("20 Vaisseau2", arguments[1]);
        Assert.Contains("30 Vaisseau3", arguments[2]);
    }
    
    [Fact]
    public void TestAggregationOfArgumentsToDictionary()
    {
        string[] arguments = { "10 Vaisseau1", "20 Vaisseau1", "15 Vaisseau2" };
        Dictionary<string, int> result = CommandPrompt.ConvertValidArgumentsToDictionary(arguments);
        Assert.Equal(30, result["Vaisseau1"]);
        Assert.Equal(15, result["Vaisseau2"]);
    }
}