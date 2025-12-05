# Universal C# Serializer
<p align="center">
  <img src="Logo.svg" width="250">
</p>

A configurable, extendable, engine-agnostic C# serialization/deserialization module. It is designed primarily for game development, but can be used in any C# context.

Key features:
- **Easy to use**: simple, one-line serialization and deserialization.
- **Engine-agnostic**: can be used in plain C#, Godot or Unity.
- **Broad type support**: supports a wide variety of types from the .NET, Godot and Unity APIs.
- **Multiple formats**: includes support for JSON, XML and a custom, compact data format.
- **Flexible type handling**: handles arbitrary types that lack explicit support.
- **Extendible design**: can be extended to provide support for additional types or data formats.

## Version Requirements
C# 9 or higher.
- Godot 4.0 or higher (when used in Godot).
- Unity 2022.1 or higher (when used in Unity).

## Usage
Serializing:
```
MyClass obj = new();
DefaultContext context = new(Format.Cscd);  // Contains the serialization schema for all built-in types.
string serialized = context.Serialize(obj); // Serializes all public properties and fields of MyClass.
```

Deserializing:
```
obj = context.Deserialize<MyClass>(serialized); // Deserializes back to MyClass.
```

If a type that lacks explicit support is serialized, then *all* public, non-static, non-readonly fields and properties are serialized.

## Architecture
The module uses two steps in the serialization process.
1. Convert between the C# object and an intermediate node-based representation.
2. Serialize/deserialize the node-based representation to/from the serialized format.

<p align="center">
  <img src="Diagram.svg" alt="The structure of the serializer/deserializer architecture.">
</p>

Both the converter and serializer layers can be freely swapped out.
- The default converter layer has explicit support for various .NET, Godot and Unity data types (see [here](TypeTable.md) for a comprehensive list).
- The default serializer layer uses a custom serialization format (see below). The JSON and XML formats are also supported.

The following node types are available:
- Primitives: `null`, `bool`, `int`, `real`, `char`, `string`, `color`, `time`, `binary`.
- Collections: `list`, `dict`, `object`.
- Metadata: `type`.

Using these nodes, any C# object can be represented unambiguously.

## Compact Serialized C# Data
The module uses a custom serialization format, called Compact Serialized C# Data (CSCD). It's a human-readable format that resembles JSON, but adds type labels and a wider variety of literal types. This allows it to concisely express the intermediate node tree. It's designed to be compact, general and unambiguous. See the specification document [here](FormatSpecification.md) for a more detailed description of the syntax.
