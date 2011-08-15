using System;
using System.Linq;
using MemBus.Support;
using MemBus.Tests.Frame;
using NUnit.Framework;

namespace MemBus.Tests.OfTypeTree
{
    class A { }             //        A
    class B : A { }         //    |-------|
    class C : A { }         //    B       C
    class D : B { }         //    |     |----|
    class E : C { }         //    D     E    F
    class F : C { }

    class Type_tree_context
    {
        readonly TypeTree _tree = new TypeTree();

        protected void AddNodes(params Type[] nodeTypes)
        {
            foreach (var t in nodeTypes)
                _tree.AddNode(t);
        }

        protected void TheseNodesExist(params Type[] nodes)
        {
            foreach (var node in nodes.Select(t => _tree[t]))
            {
                node.ShouldNotBeNull();
            }
        }

        protected void ChildCountCorrect(Type type, int count)
        {
            var node = _tree[type];
            node.ShouldNotBeNull();
            node.Childs.ShouldHaveCount(count);
        }

        protected void SecondIsChildFromFirst(Type parent, Type child)
        {
            var node = _tree[child];
            node.ShouldNotBeNull();
            Assert.AreEqual(parent, node.Parent.Type);
        }

        protected void InheritorsOfTAre<T>(params Type[] types)
        {
            var nodes = _tree.AllDescendants(typeof(T)).ToHashSet(n => n.Type);
            Assert.IsTrue(types.All(nodes.Contains) && types.Length == nodes.Count, 
                "Retrieved descendants were: (" +  string.Join(", " , nodes.Select(n => n.Name)) + "). ");
        }

        protected void ParentsOfTAre<T>(params Type[] types)
        {
            var nodes = _tree.AllParents(typeof(T)).Select(t => t.Type);
            nodes.SequenceEqual(types).ShouldBeTrue();
        }
    }
}