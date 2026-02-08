#if GODOT
using Godot;

namespace Rusty.Serialization.Testing
{
    [GlobalClass]
    public partial class ResourceTester : Node
    {
        [Export] private TestResource resource;

        private UCS ucs = new UCS();
        private string str = "";

        public override void _Process(double delta)
        {
            if (Input.IsKeyPressed(Key.S))
            {
                str = ucs.Serialize(resource, Settings.All);
                GD.Print(str);
            }

            if (Input.IsKeyPressed(Key.P))
            {
                resource = ucs.Parse<TestResource>(str);
                GD.Print(resource.I);
            }
        }
    }
}

#endif