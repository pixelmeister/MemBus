using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MemBus
{
    /// <summary>
    /// Standard resolver to be used in scenarios with low numbers of message throughput and subscribers due to its O(N)
    /// nature in finding all subscriptions that can handle a message
    /// </summary>
    public class StandardResolver : ISubscriptionResolver
    {
        private static class ImpossibleMessage { }
        private readonly ConcurrentDictionary<Type, CompositeSubscription> cachedSubscriptions = new ConcurrentDictionary<Type, CompositeSubscription>();

        /// <summary>
        /// ctor
        /// </summary>
        public StandardResolver()
        {
            cachedSubscriptions.TryAdd(typeof(ImpossibleMessage), new CompositeSubscription());
        }

        /// <summary>
        /// See <see cref="ISubscriptionResolver.GetSubscriptionsFor"/>
        /// </summary>
        public IEnumerable<ISubscription> GetSubscriptionsFor(object message)
        {
            var lookAtThis =
                cachedSubscriptions[typeof(ImpossibleMessage)].Where(s => s.Handles(message.GetType())).ToArray();
            return lookAtThis;
        }

        /// <summary>
        /// See <see cref="ISubscriptionResolver.Add"/>
        /// </summary>
        public bool Add(ISubscription subscription)
        {
            cachedSubscriptions[typeof(ImpossibleMessage)].Add(subscription);
            return true;
        }
    }
}