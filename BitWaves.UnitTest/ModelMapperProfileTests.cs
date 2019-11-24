using AutoMapper;
using BitWaves.WebAPI.Models;
using NUnit.Framework;

namespace BitWaves.UnitTest
{
    public sealed class ModelMapperProfileTests
    {
        [Test]
        public void TestModelMapping()
        {
            var config = new MapperConfiguration(opt => opt.AddProfile<ModelMapperProfile>());

            Assert.DoesNotThrow(() => config.AssertConfigurationIsValid());
        }
    }
}
