using System;

namespace SoftCircuits.FullTextSearchQuery
{
	public class FtsQuerySettings
	{
		/// <summary>
		/// If true, the standard list of FTS stopwords are added to the stopword list.
		/// </summary>
		public bool AddStandardStopWords { get; set; }

		/// <summary>
		/// Default conjunction between parsed words. Possible values: And, Or
		/// </summary>
		public DefaultConjunctionType DefaultConjunction { get; set; } = DefaultConjunctionType.And;

		/// <summary>
		/// Default TermForm for parsed words.
		/// </summary>
		public TermForm DefaultTermForm { get; set; } = TermForm.Inflectional;

		/// <summary>
		/// Add trailing wildcard for every parsed word.
		/// </summary>
		public bool UseTrailingWildcardForAllWords { get; set; }

		/// <summary>
		/// Disable parsing near and treat it as word.
		/// </summary>
		public bool DisableNear { get; set; }

		/// <summary>
		/// Disable punctuation chars.
		/// </summary>
		public char[] DisabledPunctuation { get; set; } = Array.Empty<char>();

		/// <summary>
		/// Add additional stopwords
		/// </summary>
		public string[] AdditionalStopWords { get; set; } = Array.Empty<string>();
	}

	/// <summary>
	/// Term conjunction types.
	/// </summary>
	public enum DefaultConjunctionType
	{
		And,
		Or,
	}
}