using System;
using Labo.WebSiteOptimizer.ResourceManagement.Hasher;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement.Hasher
{
    [TestFixture]
    public class Md5ResourceHasherTestFixture
    {
        [TestCase("Bacon ipsum dolor sit amet strip steak tail andouille, short loin ham hock short ribs ball tip turkey shankle", "88e4c6f3129a86ef8b3cc8457a99cf83")]
        [TestCase("", "d41d8cd98f00b204e9800998ecf8427e")]
        public void HashContent(string content, string expectedHash)
        {
            Md5ResourceHasher hasher = new Md5ResourceHasher();

            string hash = hasher.HashContent(content);

            Assert.AreEqual(expectedHash, hash);
        }

        [Test]
        public void HashContent_WithNullString()
        {
            Md5ResourceHasher hasher = new Md5ResourceHasher();

            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => hasher.HashContent(null));

            Assert.AreEqual("Cannot hash null string.", ex.Message);
        }
    }
}
