# Adding Converters

Often, the default conversion scheme will not be appropriate for a type. In these cases, a custom converter must be created. All custom converters must inherit from the abstract `Converter` base class.

```
using Rusty.Serialization.Core.Conversion;

public class MyTypeConverter : Converter
{
    
}
```

## Augmenting The Object Codec

First, let's add your new converter to the object codec, so that it can be used. This is fairly simple:

```
UCS ucs = new(Format.Json);
ucs.ObjectCodec.RegisterConverter<MyType, MyTypeConverter>();
```

This will tell the object codec that all instances of `MyType` must be handled by `MyTypeConverter`.

Generic converters can also be added in a similar way.

```
ucs.ObjectCodec.RegisterConverter(typeof(MyGenericType<>), typeof(MyGenericTypeConverter<>));
```

This will cause any closed variant of `MyGenericType` to be matched with a corresponding closed variant of `MyGenericTypeConverter`.

## Serialization
Implementing the converter's serialization requires you to override the `CreateNode` method. This method returns an object that implements `INode`.

Here is a simple example for the type `ulong`, which converts to an `IntNode`.

```
public override INode CreateNode(object obj, CreateNodeContext context)
{
    ulong integer = (ulong)obj;
    return new IntNode(integer);
}
```


For composite types, serialization requires two methods to be implemented: `CreateNode` and `PopulateNode`.
// TODO

## Deserialization
// TODO