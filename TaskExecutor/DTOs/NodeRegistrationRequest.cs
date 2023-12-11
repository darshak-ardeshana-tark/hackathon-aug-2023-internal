namespace TaskExecutor.DTOs;

public class NodeRegistrationRequest
{
    public string Name { get; }
    public string Address { get; }

    public NodeRegistrationRequest(string name, string address)
    {
        Name = name;
        Address = address;
    }

    public override string ToString()
    {
        return "Name: " + Name + " Address: " + Address;
    }
}