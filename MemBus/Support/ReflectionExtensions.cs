using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace MemBus.Support
{
    /// <summary>
    /// A number of reflection Extension Methods to help you
    /// in working with attributes and other reflection mechanisms
    /// </summary>
    public static class ReflectionExtensions
    {

        public static bool IsAssignableFrom(this Type assignableFrom, Type otherType) 
        {
            return assignableFrom.GetTypeInfo().IsAssignableFrom(otherType.GetTypeInfo());
        }

        public static IEnumerable<MethodInfo> GetMethods(this Type type)
        {
            return type.GetTypeInfo().DeclaredMethods;
        }

        public static IEnumerable<Type> GetInterfaces(this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces;
        }

        public static IEnumerable<MemberInfo> GetMember(this Type type, string name)
        {
            return type.GetTypeInfo().DeclaredMembers.Where(mi => mi.Name.Equals(name));
        }

        public static string GetTypeNameFromObject(this object obj)
        {
            return obj.GetType().GetName();
        }

        public static bool IsClass(this Type type)
        {
            return type.GetTypeInfo().IsClass;
        }

        public static string GetName(this Type type)
        {
            return type.GetTypeInfo().Name;
        }

        /// <summary>
        /// Ask some class whether it implements a certain interface
        /// </summary>
        /// <typeparam name="T">The interface you are looking for</typeparam>
        /// <param name="type">the inspected type</param>
        /// <returns>true if the interface is imaplemented</returns>
        public static bool ImplementsInterface<T>(this Type type) where T : class
        {
            return type.GetTypeInfo().ImplementedInterfaces.Any(t => t == typeof(T));
        }


        /// <summary>
        /// Raise an event
        /// </summary>
        public static void Raise(this EventHandler @event, object sender)
        {
            if (@event != null)
                @event(sender, EventArgs.Empty);
        }

    }
}