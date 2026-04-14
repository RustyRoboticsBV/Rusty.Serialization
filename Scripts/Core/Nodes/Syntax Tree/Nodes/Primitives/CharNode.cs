namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A character serializer node.
    /// </summary>
    public sealed class CharNode : ValueNode<CharValue>
    {
        /* Constructors. */
        public CharNode() : base() { }

        public CharNode(CharValue value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            if (Value == '\0')
                return "char: Null";
            if (Value == 1)
                return "char: Start of Heading";
            if (Value == 2)
                return "char: Start of Text";
            if (Value == 3)
                return "char: End of Text";
            if (Value == 4)
                return "char: End of Transmission";
            if (Value == 5)
                return "char: Enquiry";
            if (Value == 6)
                return "char: Acknowledge";
            if (Value == '\a')
                return "char: Alert";
            if (Value == '\b')
                return "char: Backspace";
            if (Value == '\t')
                return "char: Horizontal Tab";
            if (Value == '\n')
                return "char: Line Feed";
            if (Value == '\v')
                return "char: Vertical Tab";
            if (Value == '\f')
                return "char: Form Feed";
            if (Value == '\r')
                return "char: Carriage Return";
            if (Value == 14)
                return "char: Shift In";
            if (Value == 15)
                return "char: Shift Out";
            if (Value == 16)
                return "char: Data Link Escape";
            if (Value == 17)
                return "char: Device Control One";
            if (Value == 18)
                return "char: Device Control Two";
            if (Value == 19)
                return "char: Device Control Three";
            if (Value == 20)
                return "char: Device Control Four";
            if (Value == 21)
                return "char: Negative Acknowledge";
            if (Value == 22)
                return "char: Synchronous Idle";
            if (Value == 23)
                return "char: End of Transmission Block";
            if (Value == 24)
                return "char: Cancel";
            if (Value == 25)
                return "char: End of Medium";
            if (Value == 26)
                return "char: Substitute";
            if (Value == 27)
                return "char: Escape";
            if (Value == 28)
                return "char: File Separator";
            if (Value == 29)
                return "char: Group Separator";
            if (Value == 30)
                return "char: Record Separator";
            if (Value == 31)
                return "char: Unit Separator";
            if (Value == ' ')
                return "char: Space";
            if (Value == 127)
                return "char: Delete";
            if (Value >= 128 && Value <= 159)
                return "char: C1 control code " + (int)Value;
            if (Value == 160)
                return "char: Non-breaking Space ";
            if (Value == 173)
                return "char: Soft Hyphen";
            return "char: " + Value;
        }
    }
}