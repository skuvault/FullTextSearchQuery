// Copyright (c) 2019-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftCircuits.FullTextSearchQuery;

namespace Fts.Test
{
    [TestClass]
    public class FtsTests
    {
        [TestMethod]
        public void BasicTests()
        {
            FtsQuery query = new FtsQuery(true);

            // Inflectional forms
            Assert.AreEqual("FORMSOF(INFLECTIONAL, abc)", query.Transform("abc"));
            // Thesaurus variations
            Assert.AreEqual("FORMSOF(THESAURUS, abc)", query.Transform("~abc"));
            // Exact term
            Assert.AreEqual("\"abc\"", query.Transform("\"abc\""));
            // Exact term
            Assert.AreEqual("\"abc\"", query.Transform("+abc"));
            // Exact term "abc" near exact term "def"
            Assert.AreEqual("\"abc\" NEAR \"def\"", query.Transform("\"abc\" near \"def\""));
            // Words that start with "abc"
            Assert.AreEqual("\"abc*\"", query.Transform("abc*"));
            // Inflectional forms of "def" but not inflectional forms of "abc"
            Assert.AreEqual("FORMSOF(INFLECTIONAL, def) AND NOT FORMSOF(INFLECTIONAL, abc)", query.Transform("-abc def"));
            // Inflectional forms of both "abc" and "def"
            Assert.AreEqual("FORMSOF(INFLECTIONAL, abc) AND FORMSOF(INFLECTIONAL, def)", query.Transform("abc def"));
            // Exact term "abc" near exact term "def"
            Assert.AreEqual("\"abc\" NEAR \"def\"", query.Transform("<+abc +def>"));
            // Inflectional forms of both "abc", and either "def" or "ghi".
            Assert.AreEqual("FORMSOF(INFLECTIONAL, abc) AND (FORMSOF(INFLECTIONAL, def) OR FORMSOF(INFLECTIONAL, ghi))", query.Transform("abc and (def or ghi)"));
        }

        [TestMethod]
        public void WordsJoinedWithOr_WhenSettingTurnedOn()
        {
            var query = new FtsQuery(new FtsQuerySettings {DefaultConjunction = ConjunctionType.Or});

            var actual = query.Transform("\"dk product\" dkp dkp123");

            const string expected = "\"dk product\" OR FORMSOF(INFLECTIONAL, dkp) OR FORMSOF(INFLECTIONAL, dkp123)";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TrailingWildcardAddedToWords_WhenSettingTurnedOn()
        {
            var query = new FtsQuery(new FtsQuerySettings {UseTrailingWildcardForAllWords = true});

            var actual = query.Transform("\"dk product\" dkp dkp123");

            const string expected = "\"dk product\" AND \"dkp*\" AND \"dkp123*\"";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InflectionalSearchIsNotUsed_WhenDefaultTermFormIsLiteral()
        {
            var query = new FtsQuery(new FtsQuerySettings {DefaultTermForm = TermForm.Literal});

            Assert.AreEqual("\"abc\"", query.Transform("abc"));

            Assert.AreEqual("\"def\" AND NOT \"abc\"", query.Transform("-abc def"));

            Assert.AreEqual("\"abc\" AND \"def\"", query.Transform("abc def"));

            Assert.AreEqual("\"abc\" AND (\"def\" OR \"ghi\")", query.Transform("abc and (def or ghi)"));
        }

        [TestMethod]
        public void HyphenIsNotParsedAsPunctuation_WhenHyphenIsDisabled()
        {
            var query = new FtsQuery(new FtsQuerySettings {DisabledPunctuation = new[] {'-'}});

            var actual = query.Transform("t-shirt");

            const string expected = "FORMSOF(INFLECTIONAL, t-shirt)";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TildeIsNotParsedAsPunctuation_WhenTildeIsDisabled()
        {
            var query = new FtsQuery(new FtsQuerySettings {DisabledPunctuation = new[] {'~'}});

            var actual = query.Transform("~abc");

            const string expected = "FORMSOF(INFLECTIONAL, ~abc)";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NearIsNotParsedAsPunctuation_WhenSingleAngleQuotationMarks_andPlusAreDisabled()
        {
            var query = new FtsQuery(new FtsQuerySettings {DisabledPunctuation = new[] {'<','>', '+'}});

            var actual = query.Transform("<+abc +def>");

            const string expected = "FORMSOF(INFLECTIONAL, <+abc) AND FORMSOF(INFLECTIONAL, +def>)";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NearIsNotParsedAsPunctuation_WhenItsDisabledInSettings()
        {
            var query = new FtsQuery(new FtsQuerySettings {DisableNear = true});

            var actual = query.Transform("\"abc\" NEAR \"def\"");

            const string expected = "\"abc\" AND FORMSOF(INFLECTIONAL, NEAR) AND \"def\"";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TrailingWildcardAddedToWords_WhenDefaultTermFormIsLiteral_andTrailingWildcardEnabled()
        {
            var query = new FtsQuery(new FtsQuerySettings
            {
                DefaultTermForm = TermForm.Literal,
                UseTrailingWildcardForAllWords = true
            });

            var actual = query.Transform("abc def ghi*");

            const string expected = "\"abc*\" AND \"def*\" AND \"ghi*\"";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TrailingWildcardIsNotAddedToAnd_WhenDefaultTermFormIsLiteral_andTrailingWildcardEnabled()
        {
            var query = new FtsQuery(new FtsQuerySettings
            {
                DefaultTermForm = TermForm.Literal,
                UseTrailingWildcardForAllWords = true
            });

            var actual = query.Transform("abc AND def");

            const string expected = "\"abc*\" AND \"def*\"";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TrailingWildcardIsNotAddedToOr_WhenDefaultTermFormIsLiteral_andTrailingWildcardEnabled()
        {
            var query = new FtsQuery(new FtsQuerySettings
            {
                DefaultTermForm = TermForm.Literal,
                UseTrailingWildcardForAllWords = true
            });

            var actual = query.Transform("abc OR def");

            const string expected = "\"abc*\" OR \"def*\"";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TrailingWildcardIsNotAddedToAndOr_WhenDefaultTermFormIsLiteral_andTrailingWildcardEnabled()
        {
            var query = new FtsQuery(new FtsQuerySettings
            {
                DefaultTermForm = TermForm.Literal,
                UseTrailingWildcardForAllWords = true
            });

            var actual = query.Transform("abc and (def or ghi)");

            const string expected = "\"abc*\" AND (\"def*\" OR \"ghi*\")";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WordsJoinedWithOr_andUsedTrailingWildcard_WhenBothSettingsTurnedOn()
        {
            var query = new FtsQuery(new FtsQuerySettings
            {
                DefaultConjunction = ConjunctionType.Or,
                UseTrailingWildcardForAllWords = true
            });

            var actual = query.Transform("\"dk product\" dkp dkp123");

            const string expected = "\"dk product\" OR \"dkp*\" OR \"dkp123*\"";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TrailingWildcardAddedToAllWords_andPunctuationSkipped()
        {
            var query = new FtsQuery(new FtsQuerySettings
            {
                DefaultConjunction = ConjunctionType.And,
                DefaultTermForm = TermForm.Literal,
                UseTrailingWildcardForAllWords = true,
                DisabledPunctuation = new[] {'~', '-', '+', '<', '>'}
            });

            var actual = query.Transform("\"dk product\" dkp OR <+dkp+123+> -ab-c AND ~def*");

            const string expected = "\"dk product\" AND \"dkp*\" OR \"<+dkp+123+>*\" AND \"-ab-c*\" AND \"~def*\"";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FixupTests()
        {
            FtsQuery query = new FtsQuery(true);

            // Subexpressions swapped
            Assert.AreEqual("FORMSOF(INFLECTIONAL, term2) AND NOT FORMSOF(INFLECTIONAL, term1)", query.Transform("NOT term1 AND term2"));
            // Expression discarded
            Assert.AreEqual("", query.Transform("NOT term1"));
            // Expression discarded if node is grouped (parenthesized) or is the root node;
            // otherwise, the parent node may contain another subexpression that will make
            // this one valid.
            Assert.AreEqual("", query.Transform("NOT term1 AND NOT term2"));
            // Expression discarded
            Assert.AreEqual("", query.Transform("term1 OR NOT term2"));
            // NEAR conjunction changed to AND
            Assert.AreEqual("FORMSOF(INFLECTIONAL, term1) AND NOT FORMSOF(INFLECTIONAL, term2)", query.Transform("term1 NEAR NOT term2"));
        }

        [TestMethod]
        public void StopwordsTests()
        {
            FtsQuery query = new FtsQuery();
            Assert.AreEqual(0, query.StopWords.Count);
            query = new FtsQuery(true);
            Assert.AreNotEqual(0, query.StopWords.Count);
        }
        
        [TestMethod]
        public void AdditionalStopwordsTests()
        {
            var additionalStopWords = new[] {"aa", "bb", "cc"};

            var query = new FtsQuery(new FtsQuerySettings {AdditionalStopWords = additionalStopWords});
            Assert.AreEqual(3, query.StopWords.Count);

            query = new FtsQuery(new FtsQuerySettings
                {AddStandardStopWords = true, AdditionalStopWords = additionalStopWords});
            Assert.IsTrue(query.StopWords.Count > 3, "Expected actualCount to be greater than 3.");
        }
    }
}
