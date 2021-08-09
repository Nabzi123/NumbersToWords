using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumbersToWordsNS;
using System;
using System.Collections.Generic;
using System.Text;

namespace NumbersToWordsNS.Tests
{
    [TestClass()]
    public class NumbersToWordsTests
    {
        [DataTestMethod]
        [DataRow("One Hundred Thousand", 100000)]
        [DataRow("One Million Two Hundred Thousand and Seventy Eight", 1200078)]
        [DataRow("Nine Billion Eight Million Seven Hundred Thousand Six Hundred Fifty Four", 9008700654)]
        public void WordsToNumbersTest(string input, long expectedOutput)
        {

            long actualOutput = NumbersToWords.WordsToNumbers(input);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [DataTestMethod]
        [DataRow("1", "One Pounds Only")]
        [DataRow("356.22", "Three Hundred Fifty Six Pounds and Twenty Two Pence")]
        [DataRow("102467", "One Hundred Two Thousand Four Hundred Sixty Seven Pounds Only")]
        public void TestNumbersToWords(string input, string expectedOutput)
        {
            string testString = NumbersToWords.ConvertToWords(input);

            Assert.AreEqual(expectedOutput, testString);
        }
    }
}