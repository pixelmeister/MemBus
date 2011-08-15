using System;
using System.Collections.Generic;
using System.Linq;
using MemBus.Support;

namespace MemBus
{
    /// <summary>
    /// This resolver orders subscriptions into a tree of message inheritances
    /// and hence provides improved lookup times for subscriptions.
    /// </summary>
    public class CachingResolver : ISubscriptionResolver
    {
        private readonly CompositeSubscription _entryPoint = new CompositeSubscription();
        private readonly TypeTree _tree = new TypeTree();

        public IEnumerable<ISubscription> GetSubscriptionsFor(object message)
        {
            var subs = _entryPoint.Where(sub => sub.Handles(message.GetType())).ToArray();
            _entryPoint.RemoveRange(subs);
            var node = _tree.AddNode(message.GetType());
            node.Subscriptions.AddRange(subs);
            return subs;
        }

        public bool Add(ISubscription subscription)
        {
            _entryPoint.Add(subscription);
            return true;
        }
    }
}