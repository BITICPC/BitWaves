using System.ComponentModel.DataAnnotations;
using BitWaves.WebAPI.Utils;
using BitWaves.WebAPI.Validation;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BitWaves.UnitTest
{
    /// <summary>
    /// 为 <see cref="Optional{T}"/> 及其相关服务提供单元测试逻辑。
    /// </summary>
    public sealed class OptionalTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConvertToJson()
        {
            var obj = new Optional<int>();
            Assert.AreEqual("undefined", JsonConvert.SerializeObject(obj));

            obj = new Optional<int>(706);
            Assert.AreEqual("706", JsonConvert.SerializeObject(obj));
        }

        [Test]
        public void ConvertFromJson()
        {
            var json = "3";
            Assert.AreEqual(new Optional<int>(3), JsonConvert.DeserializeObject<Optional<int>>(json));

            json = "undefined";
            Assert.AreEqual(new Optional<int>(), JsonConvert.DeserializeObject<Optional<int>>(json));
        }

        [Test]
        public void ValidateEmptyOptional()
        {
            var validationAttr = new OptionalValidationAttribute(typeof(RegularExpressionAttribute), @"^\d{11}$");
            var optional = new Optional<string>();

            Assert.IsTrue(validationAttr.IsValid(optional));
        }

        [Test]
        public void ValidateNonEmptyOptional()
        {
            var validationAttr = new OptionalValidationAttribute(typeof(RegularExpressionAttribute), @"\d{11}$");
            var optionalOk = new Optional<string>("13901234567");
            var optionalBad = new Optional<string>("12345");

            Assert.IsTrue(validationAttr.IsValid(optionalOk));
            Assert.IsFalse(validationAttr.IsValid(optionalBad));
        }
    }
}
