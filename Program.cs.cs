#if RUSTY_DEBUG
using Rusty.Serialization;
using Rusty.Serialization.Testing;

AllTests tests = new(new DefaultContext(), false);
tests.Run();
#endif