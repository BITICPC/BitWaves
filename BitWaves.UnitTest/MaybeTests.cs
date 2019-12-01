using System.ComponentModel.DataAnnotations;
using BitWaves.Data.Utils;
using BitWaves.WebAPI.Utils;
using BitWaves.WebAPI.Validation;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BitWaves.UnitTest
{
    /// <summary>
    /// 为 <see cref="Maybe{T}"/> 及其相关服务提供单元测试逻辑。
    /// </summary>
    public sealed class MaybeTests
    {
        [SetUp]
        public void Setup()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new MaybeJsonConverter());

                return settings;
            };
        }

        [Test]
        public void ConvertToJson()
        {
            var obj = new Maybe<int>();
            Assert.AreEqual("undefined", JsonConvert.SerializeObject(obj));

            obj = new Maybe<int>(706);
            Assert.AreEqual("706", JsonConvert.SerializeObject(obj));
        }

        [Test]
        public void ConvertFromJson()
        {
            var json = "3";
            Assert.AreEqual(new Maybe<int>(3), JsonConvert.DeserializeObject<Maybe<int>>(json));

            json = "undefined";
            Assert.AreEqual(new Maybe<int>(), JsonConvert.DeserializeObject<Maybe<int>>(json));
        }

        [Test]
        public void ValidateEmptyOptional()
        {
            var validationAttr = new InnerAttribute(typeof(RegularExpressionAttribute), @"^\d{11}$");
            var optional = new Maybe<string>();

            Assert.IsTrue(validationAttr.IsValid(optional));
        }

        [Test]
        public void ValidateNonEmptyOptional()
        {
            var validationAttr = new InnerAttribute(typeof(RegularExpressionAttribute), @"\d{11}$");
            var optionalOk = new Maybe<string>("13901234567");
            var optionalBad = new Maybe<string>("12345");

            Assert.IsTrue(validationAttr.IsValid(optionalOk));
            Assert.IsFalse(validationAttr.IsValid(optionalBad));
        }
    }
}
