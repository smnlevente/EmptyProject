using System;

[Serializable]
public class StringStack : Stack<string, StringStack>
{
    public StringStack() : base()
    {
    }
}
