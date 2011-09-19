using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace MemBus.Support
{
    public static class RespondExtension
    {
        public static bool TryInvoke(this object target, Action<dynamic> selector)
        {
            var dynExists = new DynamicExists(target);
            selector(dynExists);
            if (dynExists.OperationExists)
                selector(target);
            return dynExists.OperationExists;
        }

        public static bool RespondsTo(this object target, Func<dynamic,object> selector)
        {
            var dynExists = new DynamicExists(target);
            selector(dynExists);
            return dynExists.OperationExists;
        }

        [Api]
        public static bool RespondsTo(this object target, Action<dynamic> selector)
        {
            var dynExists = new DynamicExists(target);
            selector(dynExists);
            return dynExists.OperationExists;
        }

        private class DynamicExists : DynamicObject
        {
            private readonly object instance;
            private bool workingWithEvent;

            private static readonly ConcurrentDictionary<string,bool> cache = new ConcurrentDictionary<string, bool>();

            public DynamicExists(object instance)
            {
                this.instance = instance;
            }

            public bool OperationExists { get; private set; }

            public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
            {
                var possibleMembers =
                    from mi in instance.GetType().GetMember(binder.Name).OfType<MethodInfo>()
                    let parms = mi.GetParameters()
                    where parms.Length == args.Length
                    let correlates = parms.Zip(args, 
                      (pi, o) => (o == null && pi.ParameterType.IsClass()) || 
                                 ( o != null && pi.ParameterType.IsAssignableFrom(o.GetType())))
                                 .All(truth => truth)
                    where correlates
                    select mi;

                OperationExists = possibleMembers.Count() == 1; //Expecting 1 or none as method overloading rules would be violated
                result = null;

                return true; // We always "find" a member
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                if (!workingWithEvent) //TryGetMember is called before set when working with an event
                    workWithSetter(binder, value);
                return true; // We always "find" a member
            }


            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var possibleMembers = instance.GetType().GetMember(binder.Name);
                OperationExists = possibleMembers.Any();
                if (OperationExists && possibleMembers.First() is EventInfo)
                {
                    result = possibleMembers.First();
                    workingWithEvent = true;
                }
                else
                {
                    result = 1; //The int will support a possible += operator of an event that was searched but not found.
                }
                return true; // We always "find" a member
            }

            private void workWithSetter(SetMemberBinder binder, object value)
            {
                OperationExists = cache.GetOrAdd(instance.GetType().FullName + binder.Name + (value != null ? value.GetType().FullName : "null"),
                               _ =>
                                   {
                                       var possibleMembers = from pi in instance.GetType().GetMember(binder.Name).OfType<PropertyInfo>()
                                                             where pi.SetMethod != null &&
                                                                   (
                                                                       (pi.PropertyType.IsClass() && value == null) ||
                                                                       (value != null && pi.PropertyType.IsAssignableFrom(value.GetType()))
                                                                   )
                                                             select pi;
                                       return possibleMembers.Count() == 1;
                                   });
                
            }
        }
    }
}