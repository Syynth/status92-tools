using System;

namespace Status92.Tools
{
    public class RequiredByComponentAttribute : Attribute
    {
        public readonly Type RequiredBy;

        public RequiredByComponentAttribute(Type requiredBy)
        {
            RequiredBy = requiredBy;
        }
    }
}