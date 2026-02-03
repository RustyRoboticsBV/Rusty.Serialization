# Universal C# Serializer
**THIS PROJECT IS STILL IN PRE-ALPHA, DO NOT USE YET**

<p align="center">
  <img src="Images/Logo.svg" width="250">
</p>

A drop-in C# serialization/deserialization module. It is designed to make saving, loading, and transferring complex object graphs effortless and engine-agnostic.

It was built with two specific use-cases in mind:
- *Game Save Systems*: saving and loading of complex game state objects.
- *Engine-Agnostic Resources*: creating complex tool-generated assets, then loading them inside any C# context.

Key features:
- **Easy to use**: simple, one-line serialization and deserialization.
- **Engine-agnostic**: compatible with plain C#, Godot or Unity.
- **Broad type support**: supports a wide variety of types from the .NET, Godot and Unity APIs.
- **Multiple formats**: includes support for JSON, XML and a custom, compact data format called CSCD.
- **Flexible type handling**: handles arbitrary types that lack explicit support.
- **Reference preservation**: shared and cyclic references are preserved during serialization and deserialization.
- **Extendible design**: can be extended to provide support for additional types or data formats.

UCS prioritizes round-tripping correctness, extendability and ease-of-use over runtime performance. It is *not fully zero-allocation* and makes extensive use of reflection when handling types without explicit support. Consequently, it is NOT recommended for performance-critical or highly real-time scenarios. Object pooling is used to reduce GC pressure where possible.

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
UCS ucs = new(Format.Cscd);                               // Contains pre-defined serialization schema for all built-in types.
string serialized = ucs.Serialize(obj);                   // Serializes all public properties and fields of MyClass.
```

#### Deserializing
```
obj = UCS.Deserialize<MyClass>(serialized);               // Deserializes back to MyClass.
```

#### Conversion Between Formats
```
string xml = "...";
UCS xmlContext = new(Format.Xml);
UCS jsonContext = new(Format.Json);
string json = UCS.Convert(xml, xmlContext, jsonContext);  // Convert XML to JSON.
```

#### Freeing Up Memory
The module makes use of object pooling to avoid unnecessary GC pressure.

```
ucs.Dispose();
```

This will release all memory allocated by the context (if any). It is recommended to call this method after any (de)serialization operation that is not performance-sensitive.

#### Notes
Classes and structs without explicit support will automatically serialize using:
- Fields that are public and non-static.
- Properties that are public, non-static and non-readonly.
- Non-public fields and properties with the `[DataMember]` attribute.
- *Unity only*: Non-public fields with the `[SerializeField]` or `[SerializeReference]` attribute.
- *Godot only*: Non-public fields and properties with the `[Export]` attribute.
- **Note**: members with the `[NonSerialized]` attribute are never serialized.

#### GDScript
A GDscript wrapper is included with the module, see the [GDScript manual](Documentation\GDScript) for more information.

**Note**: Using GDScript still requires a .NET build of Godot, and only supports object roots with a type that can be stored in a `Variant`.

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

The node layer is fixed cannot be swapped. See the [node documentation document](Documentation/Nodes.md) a list of nodes.

## Compact Serialized C# Data
The module uses a custom, human-readable serialization format called Compact Serialized C# Data (CSCD). It is designed to represent complex object graphs with minimal structural overhead, preserving types, references, and supporting a variety of literal types. These literals allow common .NET and game engine types (such as date/time, vector, and color structs) to be encoded concisely while keeping the data readable and easy to maintain.

Below is an example of a custom serialized object with pretty printing. See the [specification document](Documentation/CSCD.md) for a formal description.

```
(MyType)<
    my_null: null,
    my_bool: true,
    my_int: 123,
    my_float: .45,
    my_infinity: -inf,
    my_nan: nan,
    my_char: 'A',
    my_string: "abc",
    my_color: #F08080,
    my_time: @1990-2-13; ,
    my_decimal: $1.00,
    my_bytes: b_SGVsbG8sIHdvcmxkIQ,
    my_list: [1, 2., "def"],
    my_dict: {
        10: 1.,
        'A': "ABC",
        [1, 2, 3] : false
    },
    `my_id` my_object: <
        a: 0,
        b: '\1F4A9;',
        c: @14:2:10.005;
    >,
    my_reference: &my_id;
>
```
