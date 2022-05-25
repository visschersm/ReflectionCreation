using System.Collections.Generic;
using System.Linq.Expressions;
using ReflectionCreation.Domain;
using ReflectionCreation.Services;

var data = new DataClass
{
    Id = 200,
    Username = "test-user",
    Password = "test",
    Email = "test@test.test"
};

var instanceType = typeof(FooRecord);
var constructor = instanceType.GetConstructors().Single();
var parameters = constructor.GetParameters();
var arguments = new List<Expression>();
var paramExpressions = new List<ParameterExpression>();
foreach(var parameter in parameters)
{
    var dataProperty = typeof(DataClass).GetProperties()
        .Where(p => p.Name == parameter.Name)
        .First();

    var dataValue = dataProperty.GetValue(data);

    var parameterExpression = Expression.Parameter(dataProperty.PropertyType, dataProperty.Name);
    paramExpressions.Add(parameterExpression);
    arguments.Add(Expression.Assign(
        parameterExpression, 
        Expression.Constant(dataValue)));
}

var newExpression = Expression.New(constructor, arguments.ToArray());
var foo = Expression.Lambda<Func<FooRecord>>(newExpression, paramExpressions.ToArray());
var Creator = foo.Compile();
var result = Creator();
return;

var fooClass = Reflector.CreateInstanceFromData<FooClass>(data);
var fooStruct = Reflector.CreateInstanceFromData<FooStruct>(data);
var fooRecord = Reflector.CreateInstanceFromData<FooRecord>(data);
var fooRecordStruct = Reflector.CreateInstanceFromData<FooRecordStruct>(data);
var fooRecordWithProperties = Reflector.CreateInstanceFromData<FooRecordWithProperties>(data);

return;






