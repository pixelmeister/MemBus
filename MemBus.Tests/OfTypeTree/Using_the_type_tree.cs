using System;
using NUnit.Framework;

namespace MemBus.Tests.OfTypeTree
{
    [TestFixture]
    internal class type_tree_new : Type_tree_context
    {
        [Test]
        public void root_is_known_on_fresh_tree()
        {
            TheseNodesExist(typeof(object));
        }
    }
}