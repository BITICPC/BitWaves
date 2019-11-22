using System;
using System.Collections.Generic;
using System.Linq;
using BitWaves.WebAPI.Utils;
using NUnit.Framework;

namespace BitWaves.UnitTest
{
    /// <summary>
    /// 为 <see cref="EnumInfo"/> 提供单元测试。
    /// </summary>
    public class EnumInfoTests
    {
        private enum NonFlagsSigned : int
        {
            Variant1,
            Variant2
        }

        [Flags]
        private enum FlagsSigned : int
        {
            Variant1 = 1,
            Variant2 = 2
        }

        private enum ValueHuge : long
        {
            Min = long.MinValue,
            Max = long.MaxValue
        }

        private EnumInfo _nonFlagsSigned;
        private EnumInfo _flagsSigned;
        private EnumInfo _valueHuge;

        [SetUp]
        public void Setup()
        {
            _nonFlagsSigned = EnumInfo.From(typeof(NonFlagsSigned));
            _flagsSigned = EnumInfo.From(typeof(FlagsSigned));
            _valueHuge = EnumInfo.From(typeof(ValueHuge));
        }

        [Test]
        public void HasValue()
        {
            Assert.IsTrue(_nonFlagsSigned.HasValue(NonFlagsSigned.Variant1));
            Assert.IsTrue(_nonFlagsSigned.HasValue(NonFlagsSigned.Variant2));
            Assert.IsFalse(_nonFlagsSigned.HasValue((NonFlagsSigned) 100));
        }

        [Test]
        public void HasValueFlags()
        {
            Assert.IsFalse(_flagsSigned.HasValue(FlagsSigned.Variant1 | FlagsSigned.Variant2));
        }

        [Test]
        public void HasValueTypeMismatch()
        {
            Assert.Throws<ArgumentException>(() => _nonFlagsSigned.HasValue(FlagsSigned.Variant1));
        }

        [Test]
        public void HasValueNull()
        {
            Assert.Throws<ArgumentNullException>(() => _nonFlagsSigned.HasValue(null));
        }

        [Test]
        public void HasName()
        {
            Assert.IsTrue(_nonFlagsSigned.HasName(nameof(NonFlagsSigned.Variant1)));
            Assert.IsFalse(_nonFlagsSigned.HasName("Variant3"));
        }

        [Test]
        public void HasNameNull()
        {
            Assert.Throws<ArgumentNullException>(() => _nonFlagsSigned.HasName(null));
        }

        [Test]
        public void HasFlags()
        {
            Assert.IsTrue(_nonFlagsSigned.HasFlags(NonFlagsSigned.Variant1));
            Assert.IsTrue(_flagsSigned.HasFlags(FlagsSigned.Variant1));
            Assert.IsTrue(_flagsSigned.HasFlags(FlagsSigned.Variant1 | FlagsSigned.Variant2));
            Assert.IsFalse(_flagsSigned.HasFlags((FlagsSigned) 100));
        }

        [Test]
        public void HasFlagsTypeMismatch()
        {
            Assert.Throws<ArgumentException>(() => _flagsSigned.HasFlags(NonFlagsSigned.Variant1));
        }

        [Test]
        public void HasFlagsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _flagsSigned.HasFlags(null));
        }

        [Test]
        public void ToUInt64Unchecked()
        {
            Assert.AreEqual(0ul, _nonFlagsSigned.ToUInt64Unchecked(NonFlagsSigned.Variant1));
            Assert.AreEqual(1ul, _nonFlagsSigned.ToUInt64Unchecked(NonFlagsSigned.Variant2));
        }

        [Test]
        public void ToUInt64UncheckedFlags()
        {
            Assert.AreEqual((ulong) (FlagsSigned.Variant1 | FlagsSigned.Variant2),
                            _flagsSigned.ToUInt64Unchecked(FlagsSigned.Variant1 | FlagsSigned.Variant2));
        }

        [Test]
        public void ToUInt64UncheckedOverflow()
        {
            Assert.AreEqual((ulong) long.MaxValue, _valueHuge.ToUInt64Unchecked(ValueHuge.Max));
            Assert.AreEqual(unchecked((ulong) long.MinValue), _valueHuge.ToUInt64Unchecked(ValueHuge.Min));
        }

        [Test]
        public void ToUInt64UncheckedTypeMismatch()
        {
            Assert.Throws<ArgumentException>(() => _nonFlagsSigned.ToUInt64Unchecked(FlagsSigned.Variant1));
        }

        [Test]
        public void ToUInt64UncheckedNull()
        {
            Assert.Throws<ArgumentNullException>(() => _nonFlagsSigned.ToUInt64Unchecked(null));
        }

        [Test]
        public void GetComponents()
        {
            var comparer = HashSet<Enum>.CreateSetComparer();
            Assert.IsTrue(comparer.Equals(_nonFlagsSigned.GetComponents(NonFlagsSigned.Variant1).ToHashSet(),
                                          new HashSet<Enum> { NonFlagsSigned.Variant1 }));
        }

        [Test]
        public void GetComponentsFlags()
        {
            var comparer = HashSet<Enum>.CreateSetComparer();
            Assert.IsTrue(comparer.Equals(
                              _flagsSigned.GetComponents(FlagsSigned.Variant1 | FlagsSigned.Variant2).ToHashSet(),
                              new HashSet<Enum> { FlagsSigned.Variant1, FlagsSigned.Variant2 }));
        }

        [Test]
        public void GetComponentsTypeMismatch()
        {
            Assert.Throws<ArgumentException>(() => _nonFlagsSigned.GetComponents(FlagsSigned.Variant1));
        }

        [Test]
        public void GetComponentsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _nonFlagsSigned.GetComponents(null));
        }
    }
}
