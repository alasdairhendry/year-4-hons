using System;

namespace QuestFlow
{
    [AttributeUsage ( AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
    public class GenericDisplayNameAttribute : Attribute
    {
        public string displayName { get; protected set; }

        public GenericDisplayNameAttribute (string displayName)
        {
            this.displayName = displayName;
        }
    }
}