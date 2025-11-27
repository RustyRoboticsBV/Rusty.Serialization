# Compact Serialized C# Data
A C# serialization/deserialization module, with support for built-in C# types, as well as common API types from the Godot and Unity game engines. It's primarily meant for engine-agnostic tool development.

## Format
The module uses a custom serialization format, called Compact Serialized C# Data (CSCD), a JSON-like format that supports type labels and a wider variety of data types. It's designed to be compact, general and relatively easy to parse. See the specification document [here](FormatSpecification.md).

## Supported Types
For a list of types with explicit serialization support, see [here](TypeTable.md).