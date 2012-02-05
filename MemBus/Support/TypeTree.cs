using System;
using System.Collections.Generic;
using System.Linq;

namespace MemBus.Support
{
    internal class TypeTree
    {
        readonly Dictionary<Type,TypeNode> _index = new Dictionary<Type, TypeNode>();

        public TypeTree()
        {
            _index.Add(typeof(object), TypeNode.From<object>());
        }

        public TypeNode this[Type type]
        {
            get
            {
                TypeNode node;
                return _index.TryGetValue(type, out node) ? node : null;
            }
        }

        public TypeNode AddNode(Type type)
        {
            var node = this[type] ?? new TypeNode(type);
            AddNode(node);
            return node;
        }

        private void AddNode(TypeNode node)
        {
            if (_index.ContainsKey(node.Type))
                return;
            var parent = this[node.LogicalParent];
            if (parent != null)
            {
                parent.AddChild(node);
                _index.Add(node.Type, node);
                return;
            }
            var parentNode = new TypeNode(node.LogicalParent);
            parentNode.AddChild(node);
            AddNode(parentNode);
        }

        public IEnumerable<TypeNode> AllDescendants(Type type)
        {
            var n = this[type];
            if (n == null)
                return Enumerable.Empty<TypeNode>();

            return n.AllDescendants();
        }

        public IEnumerable<TypeNode> AllParents(Type type)
        {
            var n = this[type];
            if (n == null)
                return Enumerable.Empty<TypeNode>();
            return n.AllParents();
        }

        public IEnumerable<ISubscription> AllSubscriptionsHandling(Type messageType)
        {
            
        }
    }
}