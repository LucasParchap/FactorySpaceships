using FactorySpaceships.Error;
using FactorySpaceships.Models;

namespace FactorySpaceships.Tests;

public class CommandLineErrorHandlerTest
{
    public static CommandLineErrorHandler CommandLineErrorHandler = new CommandLineErrorHandler();
    [Fact]
    public void TestCommandWithNoArguments()
    {
        var command = "STOCKS";
        bool result = CommandLineErrorHandler.ValidateArgumentsStructure(command);
        Assert.True(result);
    }
    [Fact]
    public void TestValidArgumentsStructure()
    {
        var commandInput = "STOCKS 10 Vaisseau1, 20 Vaisseau2";

        bool isValid = CommandLineErrorHandler.ValidateArgumentsStructure(commandInput);
        
        Assert.True(isValid, "La structure de la commande devrait être valide et retourner true.");
    }
    [Fact]
    public void TestInvalidCommandStructureWithEmptyArgument()
    {
        var commandInput = "STOCKS 10 Vaisseau1, , 20 Vaisseau2"; 

        bool isValid = CommandLineErrorHandler.ValidateArgumentsStructure(commandInput);

        Assert.False(isValid, "La structure de la commande devrait être invalide et retourner false.");
        Assert.Equal("Empty argument detected. Ensure arguments are separated by a single comma without extra spaces.", CommandLineErrorHandler.GetErrorMessage());
    }
    [Fact]
    public void TestInvalidCommandStructureWithTooManyParameters()
    {
       
        var commandInput = "STOCKS 10 Vaisseau1, 20 Vaisseau2 Vaisseau3"; 
        
        bool isValid = CommandLineErrorHandler.ValidateArgumentsStructure(commandInput);


        Assert.False(isValid, "La structure de la commande devrait être invalide et retourner false.");
        Assert.Equal($"Each argument must consist of exactly two parts: a quantity and a name, separated by one space. Problem found with '20 Vaisseau2 Vaisseau3'.", CommandLineErrorHandler.GetErrorMessage());
    }

    [Fact]
    public void TestValidArguments()
    {
        string[] arguments = { "10 Vaisseau1", "15 Vaisseau2" };

        bool isValid = CommandLineErrorHandler.ValidateArguments(arguments);

        Assert.True(isValid, "Les arguments sont valides et devraient renvoyer true.");
    }
    [Fact]
    public void TestInvalidNumericValueInArguments()
    {
        string[] arguments = { "abc Vaisseau1", "15 Vaisseau2" };
        
        bool isValid = CommandLineErrorHandler.ValidateArguments(arguments);
        
        Assert.False(isValid, "Le premier argument n'est pas numérique et devrait rendre la validation invalide.");
        Assert.Equal("A numeric value is expected as the first argument.", CommandLineErrorHandler.GetErrorMessage());
    }
    
    [Fact]
    public void TestInvalidStringValueInArguments()
    {
        string[] arguments = { "10 1234", "15 Vaisseau2" };
        
        bool isValid = CommandLineErrorHandler.ValidateArguments(arguments);

        Assert.False(isValid, "Le second argument est uniquement numérique et devrait rendre la validation invalide.");
        Assert.Equal("A string value is expected as the second argument, not just numbers.", CommandLineErrorHandler.GetErrorMessage());
    }
    
 
}