using NUnit.Framework;

namespace MemBus.Tests.OfTypeTree
{
    [TestFixture]
    internal class full_tree_retrieval : Type_tree_context
    {
        [TestFixtureSetUp]
        public void Given()
        {
            AddNodes(typeof(A), typeof(B), typeof(C), typeof(D), typeof(E), typeof(F));
        }

        [Test]
        public void child_count_object_correct()
        {
            ChildCountCorrect(typeof(object), 1);
        }

        [Test]
        public void child_count_a_correct()
        {
            ChildCountCorrect(typeof(A), 2);
        }

        [Test]
        public void child_count_b_correct()
        {
            ChildCountCorrect(typeof(B), 1);
        }

        [Test]
        public void descendants_of_a()
        {
            InheritorsOfTAre<A>(typeof(B), typeof(C), typeof(D), typeof(E), typeof(F));
        }

        [Test]
        public void descendants_of_c()
        {
            InheritorsOfTAre<C>(typeof(E), typeof(F));
        }

        [Test]
        public void descendants_of_b()
        {
            InheritorsOfTAre<B>(typeof(D));
        }

        [Test]
        public void parents_of_d()
        {
            ParentsOfTAre<D>(typeof(B), typeof(A), typeof(object));
        }

        [Test]
        public void parents_of_e()
        {
            ParentsOfTAre<E>(typeof(C), typeof(A), typeof(object));
        }
    }
}