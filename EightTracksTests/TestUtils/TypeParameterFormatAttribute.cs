using System;
using StoryQ.Formatting.Parameters;

namespace EightTracksTests.TestUtils
{
    public class TypeParameterFormatAttribute : ParameterFormatAttribute
    {
        public override string Format(object value)
        {
            Type type = (Type) value;
            return type.Name;
        }
    }
}