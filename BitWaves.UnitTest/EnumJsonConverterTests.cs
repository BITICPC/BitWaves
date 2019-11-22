using System;
using BitWaves.WebAPI.Utils;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BitWaves.UnitTest
{
    /// <summary>
    /// 为 <see cref="EnumJsonConverter"/> 提供单元测试。
    /// </summary>
    public class EnumJsonConverterTests
    {
        private enum NonFlagsValue
        {
            Variant1,
            Variant2
        }

        [Flags]
        private enum FlagsValue
        {
            Variant1 = 1,
            Variant2 = 2
        }

        private JsonSerializerSettings _serializationSettings;

        [SetUp]
        public void Setup()
        {
            _serializationSettings = new JsonSerializerSettings();
            _serializationSettings.Converters.Add(new EnumJsonConverter());
        }

        [Test]
        public void SerializeNormal()
        {
            Assert.AreEqual("\"Variant1\"",
                            JsonConvert.SerializeObject(NonFlagsValue.Variant1, _serializationSettings));
        }

        [Test]
        public void SerializeNotDefined()
        {
            Assert.AreEqual("100", JsonConvert.SerializeObject((NonFlagsValue) 100, _serializationSettings));
        }

        [Test]
        public void SerializeFlags()
        {
            var json = JsonConvert.SerializeObject(FlagsValue.Variant1 | FlagsValue.Variant2, _serializationSettings);
            var array = JsonConvert.DeserializeObject<string[]>(json);

            Assert.AreEqual(2, array.Length);
            Assert.Contains("Variant1", array);
            Assert.Contains("Variant2", array);
        }

        [Test]
        public void DeserializeNormal()
        {
            Assert.AreEqual(NonFlagsValue.Variant1,
                            JsonConvert.DeserializeObject<NonFlagsValue>("\"Variant1\"", _serializationSettings));
        }

        [Test]
        public void DeserializeNotDefined()
        {
            Assert.AreEqual((NonFlagsValue) 100,
                            JsonConvert.DeserializeObject<NonFlagsValue>("100", _serializationSettings));
        }

        [Test]
        public void DeserializeFlags()
        {
            Assert.AreEqual(FlagsValue.Variant1 | FlagsValue.Variant2,
                            JsonConvert.DeserializeObject<FlagsValue>("[\"Variant2\", \"Variant1\"]",
                                                                      _serializationSettings));
        }

        [Test]
        public void DeserializeInvalid()
        {
            Assert.Throws<JsonSerializationException>(() => JsonConvert.DeserializeObject<FlagsValue>("[1, 2]"));
        }
    }
}
