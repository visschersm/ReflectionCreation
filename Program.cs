using System.Collections.Generic;

var data = new DataClass
{
    Id = 200,
    Username = "test-user",
    Password = "test",
    Email = "test@test.test"
};

var fooClass = CreateInstanceFromData<FooClass>(data);
var fooStruct = CreateInstanceFromData<FooStruct>(data);
var fooRecord = CreateInstanceFromData<FooRecord>(data);
var fooRecordStruct = CreateInstanceFromData<FooRecordStruct>(data);
var fooRecordWithProperties = CreateInstanceFromData<FooRecordWithProperties>(data);

var bar = new FooRecord(300, "test", "test", "test");
typeof(FooRecord).GetProperty("Id").SetValue(bar, 666);

return;
T CreateInstanceFromData<T>(DataClass data)
{
    Type instanceType = typeof(T); 
    bool createWithParamlessCstr = false;
    var hasDefaultConstructor = instanceType.GetConstructor(Type.EmptyTypes) != null;
    if(instanceType.IsClass && hasDefaultConstructor)
    {
        createWithParamlessCstr = true;
    }
    
    var properties = typeof(T).GetProperties();
    List<object> arguments = new();
    
    // Class with parameterless constructor
    if(instanceType.IsClass && hasDefaultConstructor)
    {
        var instance = Activator.CreateInstance<T>();

        foreach(var property in properties)
        {
            var dataProperty = typeof(DataClass).GetProperties()
                .Where(p => p.Name == property.Name)
                .Where(p => p.GetType() == property.GetType())
                .First();
            
            property.SetValue(instance, dataProperty.GetValue(data));
        }

        return instance;
    }

    // Struct without constructor
    if(instanceType.IsValueType)
    {
        var instance = Activator.CreateInstance<T>();

        foreach(var property in properties)
        {
            var dataProperty = typeof(DataClass).GetProperties()
                .Where(p => p.Name == property.Name)
                .Where(p => p.GetType() == property.GetType())
                .First();
            object boxed = instance; 
            property.SetValue(boxed, dataProperty.GetValue(data));
            instance = (T)boxed;
        }

        return instance;
    }

    // Class record
    if(instanceType.IsClass && !hasDefaultConstructor)
    { 
        foreach(var property in properties)
        {
            var dataProperty = typeof(DataClass).GetProperties()
                .Where(p => p.Name == property.Name)
                .Where(p => p.GetType() == property.GetType())
                .First();

            arguments.Add(dataProperty.GetValue(data));
        }

        return (T)Activator.CreateInstance(instanceType, arguments.ToArray());
    }

    throw new NotImplementedException();
}

public class DataClass
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}
public class FooClass
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }

}

public struct FooStruct
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}

public record FooRecord(int Id, string Username, string Password, string Email);
public record struct FooRecordStruct(int Id, string Username, string Password, string Email);

public record FooRecordWithProperties
{
    public int Id { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public string Email { get; init; }
}