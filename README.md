# Universal C# Serializer
**THIS PROJECT IS STILL IN PRE-ALPHA, DO NOT USE YET**

<p align="center">
  <img src="Images/Logo.svg" width="250">
</p>

A configurable, extendable, engine-agnostic C# serialization/deserialization module. It is designed to simplify saving, loading, and transferring data in any C# context, with a special focus on game development.

Key features:
- **Easy to use**: simple, one-line serialization and deserialization.
- **Engine-agnostic**: can be used in plain C#, Godot or Unity.
- **Broad type support**: supports a wide variety of types from the .NET, Godot and Unity APIs.
- **Multiple formats**: includes support for JSON, XML and a custom, compact data format called CSCD.
- **Flexible type handling**: handles arbitrary types that lack explicit support.
- **Reference preservation**: shared and cyclic references are preserved during serialization and deserialization.
- **Extendible design**: can be extended to provide support for additional types or data formats.

## Version Requirements
C# 9 or higher.
- Godot 4.0 or higher (when used in Godot).
- Unity 2022.1 or higher (when used in Unity).

## How To Use
#### Installation
Simply add the project folder to your C# project and add `using Rusty.Serialization` to your script - no further setup is needed.

#### Serializing
```
MyClass obj = new();
DefaultContext context = new(Format.Cscd);      // Contains pre-defined serialization schema for all built-in types.
string serialized = context.Serialize(obj);     // Serializes all public properties and fields of MyClass.
```

#### Deserializing
```
obj = context.Deserialize<MyClass>(serialized); // Deserializes back to MyClass.
```

#### Notes
Classes and structs without explicit support will automatically serialize using:
- Fields that are public and non-static.
- Properties that are public, non-static and non-readonly.
- Non-public fields and properties with the `[DataMember]` attribute.
- *Unity only*: Non-public fields with the `[SerializeField]` or `[SerializeReference]` attribute.
- *Godot only*: Non-public fields and properties with the `[Export]` attribute.
- **Note**: members with the `[NonSerialized]` attribute are never serialized.

## Architecture
The module separates the serialization process into two steps.
1. Convert between the C# object and an intermediate node-based representation.
2. Serialize/deserialize the node-based representation to/from the chosen format.

<p align="center">
  <img src="Images/Diagram.svg" alt="The structure of the serializer/deserializer architecture.">
</p>

Both the converter and serializer layers can be freely swapped out.
- The default converter layer has explicit support for various .NET, Godot and Unity data types (see the [type table](Documentation/TypeTable.md) for a comprehensive list).
- The default serializer layer uses a custom serialization format (see below). The JSON and XML formats are also supported, though input must be structured in a way that [matches the parser's expectations](Documentation/FormatTable.md).

### Nodes
The node layer recognizes the following nodes:
- Primitives: `null`, `bool`, `int`, `real`, `char`, `string`, `color`, `time`, `bytes`, `ref`.
- Collections: `list`, `dict`, `object`.
- Metadata: `type`, `ID`.

These are more abstract than a C# type. For example, an `int` node can represent any integer type, a `real` node can represent any decimal type, a `time` node covers various date/time structs, a `list` node can represent any linear data structure, etc.

## Compact Serialized C# Data
The module uses a custom serialization format called Compact Serialized C# Data (CSCD). CSCD is a human-readable format that supports references, type labels and a wide variety of literal types. This allows it to concisely represent complex C# objects that would require more verbosity in other formats. It is designed to be compact, general and unambiguous.

Below is an example of a custom serialized object with pretty printing. See the [specification document](Documentation/CSCD.md) for a more detailed description of the syntax.

```
(MyType)<
    my_null: null,
    my_bool: true,
    my_int: 123,
    my_real: .45,
    my_char: 'A',
    my_string: "abc",
    my_color: #F08080,
    my_time: Y1990M2D13,
    my_bytes: b1234ABCD,
    my_list: [1, 2., "def"],
    my_dict: {
        10: 1.0,
        'A': "ABC",
        [1, 2, 3] : false
    },
    my_object: <
        my_nested_object_with_id: `my_id` <
            my_int: 0
        >
    >,
    my_reference: &my_id
>
```
