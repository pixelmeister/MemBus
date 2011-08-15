using System;
using System.Collections.Generic;
using System.Linq;

namespace MemBus.Support
{
    internal class TypeNode
    {
        private readonly List<TypeNode> _childs = new List<TypeNode>();

        public static TypeNode From<T>()
        {
            return new TypeNode(typeof(T));
        }

        public TypeNode() : this(typeof(object))
        {
        }

        public TypeNode(Type type)
        {
            Type = type;
        }

        public Type Type { get; private set; }
        public TypeNode Parent { get; private set; }

        public IEnumerable<TypeNode> Childs
        {
            get { return _childs; }
        }

        public Type LogicalParent
        {
            get { return Type.BaseType; }
        }

        public void AddChild(TypeNode node)
        {
            node.Parent = this;
            _childs.Add(node);
        }

        public override string ToString()
        {
            return Type.Name;
        }

        public IEnumerable<TypeNode> AllDescendants()
        {
            return Childs.Concat(_childs.SelectMany(n=>n.AllDescendants()));
        }

        public IEnumerable<TypeNode> AllParents()
        {
            return Parent != null ? Parent.AsEnumerable().Concat(Parent.AllParents()) : Enumerable.Empty<TypeNode>();
        }
    }
}