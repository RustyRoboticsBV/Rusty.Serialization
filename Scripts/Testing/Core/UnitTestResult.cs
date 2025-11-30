#if RUSTY_DEBUG
namespace Rusty.Serialization.Testing
{
    public enum UnitTestResult
    {
        Uninitialized,
        CorrectResult,
        WrongResult,
        CorrectException,
        WrongException,
        TypeMismatch
    }
}
#endif