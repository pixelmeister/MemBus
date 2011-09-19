using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace MemBus.Subscribing
{
    public static class ConstructSubscriptionExtension
    {
        public static ISubscription ConstructSubscription(this MethodInfo info, object target)
        {
            var parameterType = info.GetParameters()[0].ParameterType;

            var fittingDelegateType = typeof(Action<>).MakeGenericType(parameterType);

            var p = Expression.Parameter(parameterType);
            var call = Expression.Call(p, info);
            var @delegate = Expression.Lambda(fittingDelegateType, call, p);

            var fittingMethodSubscription = typeof(MethodInvocation<>).MakeGenericType(parameterType);
            var sub = Activator.CreateInstance(fittingMethodSubscription, @delegate.Compile());

            return (ISubscription)sub;
        }

        public static IEnumerable<ISubscription> ConstructSubscriptions(this IEnumerable<MethodInfo> infos, object target)
        {
            return infos.Select(i => i.ConstructSubscription(target));
        }
    }
}