using InfinniPlatform.Core.Extensions;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Extensions
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class CommonExtensionsTest
    {
        [Test]
        [TestCase(null, null, -1)]
        [TestCase("", "", -1)]
        [TestCase("Some OldValue", "OldValue", 5)]
        [TestCase("Some OldValue.", "OldValue", 5)]
        [TestCase("OldValue string", "OldValue", 0)]
        [TestCase("Some OldValue string", "OldValue", 5)]
        [TestCase("Some oldValue string", "OldValue", -1)]
        [TestCase("Some 1OldValue string", "OldValue", 6)]
        [TestCase("Some AOldValue string", "OldValue", 6)]
        [TestCase("Some OldValue1 string", "OldValue", 5)]
        [TestCase("Some OldValueA string", "OldValue", 5)]
        [TestCase("Some 'OldValue' string", "OldValue", 6)]
        [TestCase("Some \"OldValue\" string", "OldValue", 6)]
        [TestCase("Some 'OldValue1' string", "OldValue", 6)]
        [TestCase("Some 'OldValueA' string", "OldValue", 6)]
        [TestCase("Some '1OldValue' string", "OldValue", 7)]
        [TestCase("Some 'AOldValue' string", "OldValue", 7)]
        [TestCase("Some \"OldValue\" string", "OldValue", 6)]
        [TestCase("Some \"OldValue1\" string", "OldValue", 6)]
        [TestCase("Some \"OldvalueA\" string", "OldValue", -1)]
        [TestCase("Some \"1OldValue\" string", "OldValue", 7)]
        [TestCase("Some \"AOldValue\" string", "OldValue", 7)]
        [TestCase("Some .*? string", ".*?", 5)]
        public void FindNextIndexOfWhenMatchCaseAndNotWholeWord(string target, string value, int expected)
        {
            // When
            int actual = target.FindNextIndexOf(value, 0, true, false);

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(null, null, -1)]
        [TestCase("", "", -1)]
        [TestCase("Some OldValue", "OldValue", 5)]
        [TestCase("Some OldValue.", "OldValue", 5)]
        [TestCase("OldValue string", "OldValue", 0)]
        [TestCase("Some OldValue string", "OldValue", 5)]
        [TestCase("Some oldValue string", "OldValue", -1)]
        [TestCase("Some 1OldValue string", "OldValue", -1)]
        [TestCase("Some AOldValue string", "OldValue", -1)]
        [TestCase("Some OldValue1 string", "OldValue", -1)]
        [TestCase("Some OldValueA string", "OldValue", -1)]
        [TestCase("Some 'OldValue' string", "OldValue", 6)]
        [TestCase("Some \"OldValue\" string", "OldValue", 6)]
        [TestCase("Some 'OldValue1' string", "OldValue", -1)]
        [TestCase("Some 'OldValueA' string", "OldValue", -1)]
        [TestCase("Some '1OldValue' string", "OldValue", -1)]
        [TestCase("Some 'AOldValue' string", "OldValue", -1)]
        [TestCase("Some \"OldValue1\" string", "OldValue", -1)]
        [TestCase("Some \"OldValueA\" string", "OldValue", -1)]
        [TestCase("Some \"1OldValue\" string", "OldValue", -1)]
        [TestCase("Some \"AOldValue\" string", "OldValue", -1)]
        [TestCase("Some .*? string", ".*?", 5)]
        public void FindNextIndexOfWhenMatchCaseAndWholeWord(string target, string value, int expected)
        {
            // When
            int actual = target.FindNextIndexOf(value, 0, true, true);

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(null, null, -1)]
        [TestCase("", "", -1)]
        [TestCase("Some OldValue", "OldValue", 5)]
        [TestCase("Some OldValue.", "OldValue", 5)]
        [TestCase("Some oldValue", "OldValue", 5)]
        [TestCase("Some Oldvalue.", "OldValue", 5)]
        [TestCase("OldValue string", "OldValue", 0)]
        [TestCase("OLDValue string", "OldValue", 0)]
        [TestCase("Some OldValuE string", "OldValue", 5)]
        [TestCase("Some oldVaLue string", "OldValue", 5)]
        [TestCase("Some 1OldValue string", "OldValue", -1)]
        [TestCase("Some AOldValue string", "OldValue", -1)]
        [TestCase("Some OldValue1 string", "OldValue", -1)]
        [TestCase("Some OldValueA string", "OldValue", -1)]
        [TestCase("Some 'OldVaLue' string", "OldValue", 6)]
        [TestCase("Some \"OldVaLue\" string", "OldValue", 6)]
        [TestCase("Some 'OldValue1' string", "OldValue", -1)]
        [TestCase("Some 'OldValueA' string", "OldValue", -1)]
        [TestCase("Some '1OldValue' string", "OldValue", -1)]
        [TestCase("Some 'AOldValue' string", "OldValue", -1)]
        [TestCase("Some \"oldvalue\" string", "OldValue", 6)]
        [TestCase("Some \"OldValue1\" string", "OldValue", -1)]
        [TestCase("Some \"OldValueA\" string", "OldValue", -1)]
        [TestCase("Some \"1OldValue\" string", "OldValue", -1)]
        [TestCase("Some \"AOldValue\" string", "OldValue", -1)]
        [TestCase("Some .*? string", ".*?", 5)]
        public void FindNextIndexOfWhenNotMatchCaseAndWholeWord(string target, string value, int expected)
        {
            // When
            int actual = target.FindNextIndexOf(value, 0, false, true);

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(null, null, null, null)]
        [TestCase("", "", "", "")]
        [TestCase("Some OldValue", "OldValue", "NewValue", "Some NewValue")]
        [TestCase("Some OldValue.", "OldValue", "NewValue", "Some NewValue.")]
        [TestCase("OldValue string", "OldValue", "NewValue", "NewValue string")]
        [TestCase("Some OldValue string", "OldValue", "NewValue", "Some NewValue string")]
        [TestCase("Some oldValue string", "OldValue", "NewValue", "Some oldValue string")]
        [TestCase("Some 1OldValue string", "OldValue", "NewValue", "Some 1NewValue string")]
        [TestCase("Some AOldValue string", "OldValue", "NewValue", "Some ANewValue string")]
        [TestCase("Some OldValue1 string", "OldValue", "NewValue", "Some NewValue1 string")]
        [TestCase("Some OldValueA string", "OldValue", "NewValue", "Some NewValueA string")]
        [TestCase("Some 'OldValue' string", "OldValue", "NewValue", "Some 'NewValue' string")]
        [TestCase("Some \"OldValue\" string", "OldValue", "NewValue", "Some \"NewValue\" string")]
        [TestCase("Some 'OldValue1' string", "OldValue", "NewValue", "Some 'NewValue1' string")]
        [TestCase("Some 'OldValueA' string", "OldValue", "NewValue", "Some 'NewValueA' string")]
        [TestCase("Some '1OldValue' string", "OldValue", "NewValue", "Some '1NewValue' string")]
        [TestCase("Some 'AOldValue' string", "OldValue", "NewValue", "Some 'ANewValue' string")]
        [TestCase("Some \"OldValue\" string", "OldValue", "NewValue", "Some \"NewValue\" string")]
        [TestCase("Some \"OldValue1\" string", "OldValue", "NewValue", "Some \"NewValue1\" string")]
        [TestCase("Some \"OldValueA\" string", "OldValue", "NewValue", "Some \"NewValueA\" string")]
        [TestCase("Some \"1OldValue\" string", "OldValue", "NewValue", "Some \"1NewValue\" string")]
        [TestCase("Some \"AOldValue\" string", "OldValue", "NewValue", "Some \"ANewValue\" string")]
        [TestCase("Some .*? string", ".*?", "NewValue", "Some NewValue string")]
        public void ReplaceWhenMatchCaseAndNotWholeWord(string target, string oldValue, string newValue, string expected)
        {
            // When
            string actual = target.Replace(oldValue, newValue, true, false);

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(null, null, null, null)]
        [TestCase("", "", "", "")]
        [TestCase("Some OldValue", "OldValue", "NewValue", "Some NewValue")]
        [TestCase("Some OldValue.", "OldValue", "NewValue", "Some NewValue.")]
        [TestCase("OldValue string", "OldValue", "NewValue", "NewValue string")]
        [TestCase("Some OldValue string", "OldValue", "NewValue", "Some NewValue string")]
        [TestCase("Some oldValue string", "OldValue", "NewValue", "Some oldValue string")]
        [TestCase("Some 1OldValue string", "OldValue", "NewValue", "Some 1OldValue string")]
        [TestCase("Some AOldValue string", "OldValue", "NewValue", "Some AOldValue string")]
        [TestCase("Some OldValue1 string", "OldValue", "NewValue", "Some OldValue1 string")]
        [TestCase("Some OldValueA string", "OldValue", "NewValue", "Some OldValueA string")]
        [TestCase("Some 'OldValue' string", "OldValue", "NewValue", "Some 'NewValue' string")]
        [TestCase("Some \"OldValue\" string", "OldValue", "NewValue", "Some \"NewValue\" string")]
        [TestCase("Some 'OldValue1' string", "OldValue", "NewValue", "Some 'OldValue1' string")]
        [TestCase("Some 'OldValueA' string", "OldValue", "NewValue", "Some 'OldValueA' string")]
        [TestCase("Some '1OldValue' string", "OldValue", "NewValue", "Some '1OldValue' string")]
        [TestCase("Some 'AOldValue' string", "OldValue", "NewValue", "Some 'AOldValue' string")]
        [TestCase("Some \"OldValue\" string", "OldValue", "NewValue", "Some \"NewValue\" string")]
        [TestCase("Some \"OldValue1\" string", "OldValue", "NewValue", "Some \"OldValue1\" string")]
        [TestCase("Some \"OldValueA\" string", "OldValue", "NewValue", "Some \"OldValueA\" string")]
        [TestCase("Some \"1OldValue\" string", "OldValue", "NewValue", "Some \"1OldValue\" string")]
        [TestCase("Some \"AOldValue\" string", "OldValue", "NewValue", "Some \"AOldValue\" string")]
        [TestCase("Some .*? string", ".*?", "NewValue", "Some NewValue string")]
        public void ReplaceWhenMatchCaseAndWholeWord(string target, string oldValue, string newValue, string expected)
        {
            // When
            string actual = target.Replace(oldValue, newValue, true, true);

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(null, null, null, null)]
        [TestCase("", "", "", "")]
        [TestCase("Some OldValue", "OldValue", "NewValue", "Some NewValue")]
        [TestCase("Some OldValue.", "OldValue", "NewValue", "Some NewValue.")]
        [TestCase("Some oldValue", "OldValue", "NewValue", "Some NewValue")]
        [TestCase("Some Oldvalue.", "OldValue", "NewValue", "Some NewValue.")]
        [TestCase("OldValue string", "OldValue", "NewValue", "NewValue string")]
        [TestCase("OLDValue string", "OldValue", "NewValue", "NewValue string")]
        [TestCase("Some OldValuE string", "OldValue", "NewValue", "Some NewValue string")]
        [TestCase("Some oldVaLue string", "OldValue", "NewValue", "Some NewValue string")]
        [TestCase("Some 1OldValue string", "OldValue", "NewValue", "Some 1NewValue string")]
        [TestCase("Some AOldValue string", "OldValue", "NewValue", "Some ANewValue string")]
        [TestCase("Some OldValue1 string", "OldValue", "NewValue", "Some NewValue1 string")]
        [TestCase("Some OldValueA string", "OldValue", "NewValue", "Some NewValueA string")]
        [TestCase("Some 'OldVaLue' string", "OldValue", "NewValue", "Some 'NewValue' string")]
        [TestCase("Some \"OldVaLue\" string", "OldValue", "NewValue", "Some \"NewValue\" string")]
        [TestCase("Some 'OldValue1' string", "OldValue", "NewValue", "Some 'NewValue1' string")]
        [TestCase("Some 'OldValueA' string", "OldValue", "NewValue", "Some 'NewValueA' string")]
        [TestCase("Some '1OldValue' string", "OldValue", "NewValue", "Some '1NewValue' string")]
        [TestCase("Some 'AOldValue' string", "OldValue", "NewValue", "Some 'ANewValue' string")]
        [TestCase("Some \"oldvalue\" string", "OldValue", "NewValue", "Some \"NewValue\" string")]
        [TestCase("Some \"OldValue1\" string", "OldValue", "NewValue", "Some \"NewValue1\" string")]
        [TestCase("Some \"OldValueA\" string", "OldValue", "NewValue", "Some \"NewValueA\" string")]
        [TestCase("Some \"1OldValue\" string", "OldValue", "NewValue", "Some \"1NewValue\" string")]
        [TestCase("Some \"AOldValue\" string", "OldValue", "NewValue", "Some \"ANewValue\" string")]
        [TestCase("Some .*? string", ".*?", "NewValue", "Some NewValue string")]
        public void ReplaceWhenNotMatchCaseAndNotWholeWord(string target, string oldValue, string newValue,
                                                           string expected)
        {
            // When
            string actual = target.Replace(oldValue, newValue, false, false);

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(null, null, -1)]
        [TestCase("", "", -1)]
        [TestCase("Some OldValue", "OldValue", 5)]
        [TestCase("Some OldValue.", "OldValue", 5)]
        [TestCase("Some oldValue", "OldValue", 5)]
        [TestCase("Some Oldvalue.", "OldValue", 5)]
        [TestCase("OldValue string", "OldValue", 0)]
        [TestCase("OLDValue string", "OldValue", 0)]
        [TestCase("Some OldValuE string", "OldValue", 5)]
        [TestCase("Some oldVaLue string", "OldValue", 5)]
        [TestCase("Some 1OldValue string", "OldValue", 6)]
        [TestCase("Some AOldValue string", "OldValue", 6)]
        [TestCase("Some OldValue1 string", "OldValue", 5)]
        [TestCase("Some OldValueA string", "OldValue", 5)]
        [TestCase("Some 'OldVaLue' string", "OldValue", 6)]
        [TestCase("Some \"OldVaLue\" string", "OldValue", 6)]
        [TestCase("Some 'OldValue1' string", "OldValue", 6)]
        [TestCase("Some 'OldValueA' string", "OldValue", 6)]
        [TestCase("Some '1OldValue' string", "OldValue", 7)]
        [TestCase("Some 'AOldValue' string", "OldValue", 7)]
        [TestCase("Some \"oldvalue\" string", "OldValue", 6)]
        [TestCase("Some \"OldValue1\" string", "OldValue", 6)]
        [TestCase("Some \"OldValueA\" string", "OldValue", 6)]
        [TestCase("Some \"1OldValue\" string", "OldValue", 7)]
        [TestCase("Some \"AOldValue\" string", "OldValue", 7)]
        [TestCase("Some .*? string", ".*?", 5)]
        public void ReplaceWhenNotMatchCaseAndNotWholeWord(string target, string value, int expected)
        {
            // When
            int actual = target.FindNextIndexOf(value, 0, false, false);

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(null, null, null, null)]
        [TestCase("", "", "", "")]
        [TestCase("Some OldValue", "OldValue", "NewValue", "Some NewValue")]
        [TestCase("Some OldValue.", "OldValue", "NewValue", "Some NewValue.")]
        [TestCase("Some oldValue", "OldValue", "NewValue", "Some NewValue")]
        [TestCase("Some Oldvalue.", "OldValue", "NewValue", "Some NewValue.")]
        [TestCase("OldValue string", "OldValue", "NewValue", "NewValue string")]
        [TestCase("OLDValue string", "OldValue", "NewValue", "NewValue string")]
        [TestCase("Some OldValuE string", "OldValue", "NewValue", "Some NewValue string")]
        [TestCase("Some oldVaLue string", "OldValue", "NewValue", "Some NewValue string")]
        [TestCase("Some 1OldValue string", "OldValue", "NewValue", "Some 1OldValue string")]
        [TestCase("Some AOldValue string", "OldValue", "NewValue", "Some AOldValue string")]
        [TestCase("Some OldValue1 string", "OldValue", "NewValue", "Some OldValue1 string")]
        [TestCase("Some OldValueA string", "OldValue", "NewValue", "Some OldValueA string")]
        [TestCase("Some 'OldVaLue' string", "OldValue", "NewValue", "Some 'NewValue' string")]
        [TestCase("Some \"OldVaLue\" string", "OldValue", "NewValue", "Some \"NewValue\" string")]
        [TestCase("Some 'OldValue1' string", "OldValue", "NewValue", "Some 'OldValue1' string")]
        [TestCase("Some 'OldValueA' string", "OldValue", "NewValue", "Some 'OldValueA' string")]
        [TestCase("Some '1OldValue' string", "OldValue", "NewValue", "Some '1OldValue' string")]
        [TestCase("Some 'AOldValue' string", "OldValue", "NewValue", "Some 'AOldValue' string")]
        [TestCase("Some \"oldvalue\" string", "OldValue", "NewValue", "Some \"NewValue\" string")]
        [TestCase("Some \"OldValue1\" string", "OldValue", "NewValue", "Some \"OldValue1\" string")]
        [TestCase("Some \"OldValueA\" string", "OldValue", "NewValue", "Some \"OldValueA\" string")]
        [TestCase("Some \"1OldValue\" string", "OldValue", "NewValue", "Some \"1OldValue\" string")]
        [TestCase("Some \"AOldValue\" string", "OldValue", "NewValue", "Some \"AOldValue\" string")]
        [TestCase("Some .*? string", ".*?", "NewValue", "Some NewValue string")]
        public void ReplaceWhenNotMatchCaseAndWholeWord(string target, string oldValue, string newValue, string expected)
        {
            // When
            string actual = target.Replace(oldValue, newValue, false, true);

            // Then
            Assert.AreEqual(expected, actual);
        }
    }
}