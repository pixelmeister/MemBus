using NUnit.Framework;

namespace MemBus.Tests.OfTypeTree
{
    [TestFixture]
    internal class type_tree_hierarchy_1 : Type_tree_context
    {
        [TestFixtureSetUp]
        public void Given()
        {
            AddNodes(typeof(A));
        }

        [Test]
        public void hierarchy_preserved()
        {
            ChildCountCorrect(typeof(object), 1);
        }

        [Test]
        public void parents_correct()
        {
            ParentsOfTAre<A>(typeof(object));
        }
    }
}