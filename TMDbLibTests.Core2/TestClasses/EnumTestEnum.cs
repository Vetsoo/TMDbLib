using TMDbLib.Utilities;

namespace TMDbLibTests.Core2.TestClasses
{
    enum EnumTestEnum
    {
        A,
        [EnumValue("B-Description")]
        B
    }
}