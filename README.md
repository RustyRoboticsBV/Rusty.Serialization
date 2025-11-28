# Rusty Serialization
A configurable C# serialization/deserialization module, with support for built-in C# types, as well as common API types from the Godot and Unity game engines. It's primarily meant for engine-agnostic tool development.

## Architecture
The module uses two steps in the serialization process.
1. Convert between the C# object and an intermediate node-based representation.
2. Serialize/deserialize the node-based representation to/from the serialized format.

![A diagram of the architecture](Diagram.svg)

Both the converter and serializer layers can be freely swapped out.
- The default converter layer has explicit support for various .NET, Godot and Unity data types (see [here](TypeTable.md) for a comprehensive list).
- Three serializer schemes are available: JSON, XML and a custom format called CSCD (which is the default scheme).

## Compact Serialized C# Data
The module uses a custom serialization format, called Compact Serialized C# Data (CSCD), a JSON-like format that supports type labels and a wider variety of data types. It's designed to be compact, general and relatively easy to parse. See the specification document [here](FormatSpecification.md).