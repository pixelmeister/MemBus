using NUnit.Framework;

namespace MemBus.Tests.OfTypeTree
{
    [TestFixture]
    internal class type_tree_hierarchy_2 : Type_tree_context
    {
        [TestFixtureSetUp]
        public void Given()
        {
            AddNodes(typeof(A), typeof(B));
        }

        [Test]
        public void child_count_of_object()
        {
            ChildCountCorrect(typeof(object), 1);
        }

        [Test]
        public void child_count_of_a()
        {
            ChildCountCorrect(typeof(A), 1);
            SecondIsChildFromFirst(typeof(A), typeof(B));
        }

        [Test]
        public void child_count_of_b()
        {
            ChildCountCorrect(typeof(B), 0);
        }

        [Test]
        public void inheritor_chain_correct()
        {
            InheritorsOfTAre<object>(typeof(A), typeof(B));
        }

        [Test]
        public void parent_chain_correct()
        {
            ParentsOfTAre<B>(typeof(A), typeof(object));
        }
    }
}