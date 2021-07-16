using System;

namespace SoftCircuits.FullTextSearchQuery
{
	public class FtsQuerySettings
	{
		/// <summary>
		///     If true, the standard list of stopwords are added to the stopword list.
		/// </summary>
		public bool AddStandardStopWords { get; set; }

		/// <summary>
		///     Default conjunction between parsed words.
		/// </summary>
		public ConjunctionType DefaultConjunction { get; set; } = ConjunctionType.And;

		/// <summary>
		///     Default TermForm for parsed words.
		/// </summary>
		public TermForm DefaultTermForm { get; set; } = TermForm.Inflectional;

		/// <summary>
		///     Add trailing wildcard for every parsed word.
		/// </summary>
		public bool UseTrailingWildcardForAllWords { get; set; }

		/// <summary>
		///     Disable parsing near and treat it as word.
		/// </summary>
		public bool DisableNear { get; set; }

		/// <summary>
		///     Disable punctuation chars.
		/// </summary>
		public char[] DisabledPunctuation { get; set; } = Array.Empty<char>();

		/// <summary>
		///     Add additional stopwords
		/// </summary>
		public string[] AdditionalStopWords { get; set; } = Array.Empty<string>();
	}
}