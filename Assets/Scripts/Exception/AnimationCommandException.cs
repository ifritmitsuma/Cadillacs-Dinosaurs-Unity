using System;
using System.Runtime.Serialization;

[Serializable]
public class AnimationCommandException : Exception
{
    public AnimationCommandException()
    {
    }

    public AnimationCommandException(string message) : base("Playing the animation " + message + " caused an exception")
    {
    }

    public AnimationCommandException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected AnimationCommandException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}