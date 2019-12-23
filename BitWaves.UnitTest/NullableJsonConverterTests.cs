using BitWaves.WebAPI.Utils;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BitWaves.UnitTest
{
    public sealed class NullableJsonConverterTests
    {
        [Test]
        public void WriteNull()
        {
            int? value = null;
            Assert.AreEqual("null", JsonConvert.SerializeObject(value, new NullableJsonConverter()));
        }

        [Test]
        public void WriteNonNull()
        {
            int? value = 10;
            Assert.AreEqual("10", JsonConvert.SerializeObject(value, new NullableJsonConverter()));
        }
    }
}
