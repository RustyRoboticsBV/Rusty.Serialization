# Universal C# Serializer
A configurable, extendable, engine-agnostic C# serialization/deserialization module. It is designed primarily for game development, but can be used in any C# context.

Features:
- Easy, one-line serialization and deserialization.
- Engine-agnostic: can be used in plain C#, Godot or Unity.
- Supports a wide variety of types from the .NET, Godot and Unity APIs.
- Handles arbitrary types that lack explicit support.
- A custom, compact serialized format.
- Can be extended to provide handling of custom types or alternate data formats.

## Version Requirements
C# 9 or higher.
- Godot 4.0 or higher (when used in Godot).
- Unity 2022.1 or higher (when used in Unity).

## Usage
Serializing:
```
MyClass obj = new();
DefaultContext context = new(Format.Cscd);  // Contains the serialization scheme for all built-in types.
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

![A diagram of the architecture](Diagram.svg)

Both the converter and serializer layers can be freely swapped out.
- The default converter layer has explicit support for various .NET, Godot and Unity data types (see [here](TypeTable.md) for a comprehensive list).
- The default serializer layer uses a custom serialization format (see below).

## Compact Serialized C# Data
The module uses a custom serialization format, called Compact Serialized C# Data (CSCD), a JSON-like format that supports type labels and a wider variety of data types. It's designed to be compact, general and relatively easy to parse. See the specification document [here](FormatSpecification.md).
