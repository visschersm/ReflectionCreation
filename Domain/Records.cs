namespace ReflectionCreation.Domain;
public record FooRecord(int Id, string Username, string Password, string Email);
public record struct FooRecordStruct(int Id, string Username, string Password, string Email);

public record FooRecordWithProperties
{
    public int Id { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public string Email { get; init; }
}
