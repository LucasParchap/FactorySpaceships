using FactorySpaceships.Models.State;

namespace FactorySpaceships.Models.Commands;

public class OrderCommand : ICommand
{
    private readonly OrderContext _context;
    private readonly string _args;

    public OrderCommand(OrderContext context, string[] args)
    {
        _context = context;
        _args = string.Join(", ", args);
    }

    public void Execute()
    {
        _context.ProcessOrder(_args);
    }
}