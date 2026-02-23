# Universal C# Serializer

<p align="center">
  <img src="Images/Logo.svg" width="250">
</p>

A drop-in C# serialization/deserialization module. It is designed to make saving, loading, and transferring complex object graphs effortless and engine-agnostic.

It was built with two specific use-cases in mind:
- *Game Save Systems*: saving and loading of game state objects.
- *Engine-Agnostic Resources*: creating tool-generated assets, then loading them inside any C# context.

Key features:
- **Easy to use**: simple, one-line serialization and deserialization.
- **Engine-agnostic**: compatible with plain C#, Godot or Unity.
- **Broad type support**: supports a wide variety of types from the .NET, Godot and Unity APIs.
- **Multiple formats**: includes support for JSON, XML, CSV and CSCD, a custom, compact data format.
- **Flexible type handling**: handles arbitrary types that lack explicit support.
- **Reference preservation**: shared and cyclic references are preserved during serialization and deserialization.
- **Extendible design**: can be extended to provide support for additional types or data formats.

UCS prioritizes round-tripping correctness, extendability and ease-of-use over runtime performance. It is *not fully zero-allocation* and makes extensive use of reflection when handling types without explicit support. Consequently, it is NOT recommended for performance-critical or highly real-time scenarios.

## Version Requirements
C# 9 or higher.
- Godot 4.0 or higher (when used in Godot).
- Unity 2022.1 or higher (when used in Unity).

## How To Use
#### Installation
Simply add the project folder to your C# project and add `using Rusty.Serialization` to your script - no further setup is needed.

#### Serializing
```
using Rusty.Serialization;

MyClass obj = new();
UCS cscd = new(Format.Cscd);                  // Creates a CSCD serializer context.
string serialized = cscd.Serialize(obj);      // Serializes MyClass object graph.
```

#### Deserializing
```
obj = cscd.Parse<MyClass>(serialized);        // Deserializes back to MyClass.
```

#### Conversion Between Formats
```
UCS xml = new(Format.Xml);
UCS json = new(Format.Json);
string xmlStr = "...";
string jsonStr = xml.Reformat(xmlStr, json);  // Converts XML to JSON.
```

#### Freeing Up Memory
The module makes use of object reusing and pooling to avoid unnecessary GC pressure. Heap memory can be freed up for garbage collection manually by calling the following method.

```
cscd.Clear();                                 // Releases allocated resources.
```

#### Notes
Classes and structs without explicit support will automatically serialize using:
- Fields that are public and non-static.
- Properties that are public, non-static and non-readonly.
- Non-public fields and properties with the `[DataMember]` attribute.
- *Unity only*: Non-public fields with the `[SerializeField]` or `[SerializeReference]` attribute.
- *Godot only*: Non-public fields and properties with the `[Export]` attribute.
- **Note**: members with the `[NonSerialized]` attribute are never serialized.

#### GDScript
A GDscript wrapper is included with the module, see the [GDScript manual](Documentation/GDScript.md) for more information.

**Note**: Using GDScript still requires a .NET build of Godot, and only supports object roots with a type that can be stored in a `Variant`.

## Architecture
The module separates the serialization process into two steps.
1. Convert between the C# object and an intermediate node-based representation.
2. Serialize/deserialize the node-based representation to/from the chosen format.

<p align="center">
  <img src="Images/Diagram.svg" alt="The structure of the serializer/deserializer architecture.">
</p>

Both the converter and serializer layers can be freely swapped out. This allows the system to be easily extended with additional C# object converters and support for additional formats.

The default converter layer has explicit support for various .NET, Godot and Unity data types (see the [type table](Documentation/TypeTable.md) for a comprehensive list).

The node layer is fixed cannot be swapped. See the [node documentation document](Documentation/Nodes.md) a list of nodes and their purpose.

## Format Support
The default serializer layer uses [CSCD](#compact-serialized-c-data), a custom serialization format that can natively express the node tree layer.

Other supported formats include [CSV](Documentation/Formats/CSV.md), [JSON](Documentation/Formats/JSON.md) and [XML](Documentation/Formats/XML.md). Each format requires specific non-standard formatting in order to be parsed, as node tree metadata is needed to reconstruct the original object graph. This leads to some verbosity, particularly with JSON.

## Compact Serialized C# Data
The module uses a custom, human-readable serialization format called Compact Serialized C# Data (CSCD). It is designed to represent complex object graphs with minimal structural overhead, preserving types, references, shadowed variables and supporting a variety of literal types. These literals allow common .NET and game engine types (such as timestamps, colors and collections) to be encoded concisely while keeping the data readable and easy to maintain.

Below is an example of a custom serialized object with pretty printing. See the [CSCD user manual](Documentation/CSCD/Manual.md) for more information, and the [specification document](Documentation/CSCD/Specification.md) for a formal syntax description.

A simple [syntax highlighting plugin](./VSCode) for Visual Studio Code is also provided.

```
~CSCD~
;; Copyright <My Name>         ;;
;; Licensed under <My License> ;;
(MyType)<
    my_null: null,
    my_boolean: true,
    my_integer: 123,
    my_float: .45,
    my_infinity: -inf,
    my_nan: nan,
    my_character: 'A',
    my_string: "abc",
    my_decimal: $1.00,
    my_color: #F08080,
    my_uid: %3f4e1a9c-8d3b-24e4-c8f7-b3a8d5c1e9f2,
    my_timestamp: |Z| @1990/2/13,18:30:05.001@,
    my_duration: 7d23h30m10s,
    my_bytes: !SGVsbG8sIHdvcmxkIQ,
    my_symbol: Wednesday,
    my_list: [1, 2.e-5, "def"],
    my_dictionary: {
        10: 1.,
        'A': "ABC",
        [1, 2, 3] : false
    },
    my_object: `my_address` <
        a: 1.0e15,
        b: '\1F4A9;',
        c: @14:2:10.005@,
        d: #0F0,
        e: Red,
        ^MyBaseClass^ e: 0s,
        f: |-2:30| @1990/1/5@
    >,
    my_reference: &my_address&
>
```

## Future Work
- The current JSON implementation relies on heavy usage of `{ "$tag": ... }` containers to tell various node types apart from each other, resulting in very verbose JSON that is difficult to read. This should be addressed in a future release.
- Significant performance improvements are still possible.
- Writing more comprehensive unit tests.
