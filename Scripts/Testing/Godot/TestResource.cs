#if GODOT
using Godot;

namespace Rusty.Serialization.Testing
{
    [GlobalClass]
    public partial class TestResource : Resource
    {
        [Export] private int i = 0;
        [Export] private TestResource nested;
        [Export] private Variant variant;

        public int I => i;
    }
}

#endif