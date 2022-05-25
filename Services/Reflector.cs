using System.Linq.Expressions;
using ReflectionCreation.Domain;

namespace ReflectionCreation.Services;

public class Reflector
{
    public static T CreateInstanceFromData<T>(DataClass data)
    {
        Type instanceType = typeof(T);
        var constructor = instanceType.GetConstructor(Type.EmptyTypes);
        var properties = typeof(T).GetProperties();
        List<object> arguments = new();

        if(constructor != null)
        {
            // foreach(var property in properties)
            // {
            //     var dataProperty = typeof(DataClass).GetProperties()
            //         .Where(p => p.Name == property.Name)
            //         .Where(p => p.GetType() == property.GetType())
            //         .First();
                
            //     var dataValue = dataProperty.GetValue(data);
            //     // Create PropertySetterExpression
            //     property.SetValue(instance, );
            // }

            var newInstanceExpression = Expression.New(typeof(T));
            var instantiator = (Func<T>)Expression
                .Lambda<Func<T>>(newInstanceExpression)
                .Compile();
            T res = instantiator();

            var foo = Expression.Lambda<T>(Expression.New(constructor)).Compile();
        }
        
        bool createWithParamlessCstr = false;
        var hasDefaultConstructor = instanceType.GetConstructor(Type.EmptyTypes) != null;
        if(instanceType.IsClass && hasDefaultConstructor)
        {
            createWithParamlessCstr = true;
        }
        
        
        
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
}
