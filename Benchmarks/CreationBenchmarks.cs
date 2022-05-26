using System;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ReflectionCreation.Domain;
using ReflectionCreation.Services;

var summary = BenchmarkRunner.Run<CreationBenchmarks>();

public class CreationBenchmarks
{
    private DataClass data;

    public CreationBenchmarks()
    {
        data = new DataClass
        {
            Id = 200,
            Username = "test-user",
            Password = "test",
            Email = "test@test.test"
        };
    }

    [Benchmark]
    public FooClass ExpressionCreation() => Reflector.CreateInstanceUsingExpressions<FooClass>(data);

    [Benchmark]
    public FooClass ActivatorCreation() => Reflector.CreateInstanceUsingActivator<FooClass>(data);
}