using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Core.Extension
{
	/// <summary>
	/// Array utilities
	/// </summary>
	public static class CollectionExtensions
	{
		/// <summary>
		/// Returns the list as a read-only collection
		/// </summary>
		/// <typeparam name="T">Item type</typeparam>
		/// <param name="list">List</param>
		/// <returns>Read-only collection</returns>
		public static ReadOnlyCollection<T> AsReadOnly<T>(this List<T> list)
		{
			return new ReadOnlyCollection<T>(list);
		}

		/// <summary>
		/// Selects a random element from the given array
		/// </summary>
		/// <typeparam name="TItemType">Type of the elements</typeparam>
		/// <param name="elements">Element array</param>
		/// <returns>Random elements</returns>
		public static TItemType RandomElement<TItemType>(this IEnumerable<TItemType> elements)
		{
			if (elements == null)
			{
				throw new ArgumentNullException(nameof(elements));
			}

			List<TItemType> elementList = new List<TItemType>(elements);
			int randomIndex = new Random((int)DateTime.Now.Ticks).Next(0, elementList.Count - 1);
			return elementList[randomIndex];
		}

		/// <summary>
		/// Shuffles collection elements
		/// </summary>
		/// <typeparam name="TItemType">Type of the element</typeparam>
		/// <param name="collection">Element collection</param>
		/// <param name="seed">Seed for randomness</param>
		/// <param name="elementCountToReturn">Number of elements to return</param>
		/// <returns>Shuffled Collection</returns>
		public static IEnumerable<TItemType> Shuffle<TItemType>(this IEnumerable<TItemType> collection, int? seed = null, int elementCountToReturn = -1)
		{
			return Shuffle(collection as IEnumerable, seed ?? (int)DateTime.UtcNow.Ticks, elementCountToReturn).Cast<TItemType>();
		}

		/// <summary>
		/// Shuffles collection elements
		/// </summary>
		/// <param name="collection">Element collection</param>
		/// <param name="seed">Seed for randomness</param>
		/// <param name="elementCountToReturn">Number of elements to return</param>
		/// <returns>Shuffled Collection</returns>
		public static IEnumerable Shuffle(this IEnumerable collection, int? seed = null, int elementCountToReturn = -1)
		{
			if (collection == null)
			{
				return null;
			}
			else
			{
				List<object> resultList = collection.Cast<object>().ToList();
				Random random = new Random(seed ?? (int)DateTime.UtcNow.Ticks);

				for (int iItem = 0, nItems = resultList.Count; iItem < nItems; ++iItem)
				{
					int swapIndex = random.Next(iItem, nItems);
					if (swapIndex != iItem)
					{
						object tempItem = resultList[iItem];
						resultList[iItem] = resultList[swapIndex];
						resultList[swapIndex] = tempItem;
					}
				}

				if (elementCountToReturn < 0)
				{
					return resultList;
				}
				else
				{
					List<object> subsetList = new List<object>();
					for (int i = 0; i < elementCountToReturn; ++i)
					{
						subsetList.Add(resultList[i]);
					}
					return subsetList;
				}
			}
		}

		/// <summary>
		/// Retrieves a typed value if present. Else returns default value
		/// </summary>
		/// <typeparam name="TKey">Type of the Key</typeparam>
		/// <typeparam name="TValue">Type of the value</typeparam>
		/// <param name="dictionary">Collection</param>
		/// <param name="key">Name of the value</param>
		/// <param name="defaultValue">Default value to be returned if not present</param>
		/// <returns>Typed value if present. Default value otherwise</returns>
		public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException(nameof(dictionary));
			}
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			TValue value;
			if (!dictionary.TryGetValue(key, out value))
			{
				return defaultValue;
			}
			return value;
		}

		/// <summary>
		/// Adds a range of items to the collection
		/// </summary>
		/// <typeparam name="TItemType">Item type</typeparam>
		/// <param name="collection">Collection</param>
		/// <param name="items">Items to add</param>
		public static void AddRange<TItemType>(this ICollection<TItemType> collection, IEnumerable<TItemType> items)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(nameof(collection));
			}
			if (items == null)
			{
				throw new ArgumentNullException(nameof(items));
			}
			foreach (TItemType item in items)
			{
				collection.Add(item);
			}
		}

		/// <summary>
		/// Removes a range of items from a collection
		/// </summary>
		/// <typeparam name="TItemType">Item type</typeparam>
		/// <param name="collection">Collection</param>
		/// <param name="items">Items to remove</param>
		public static void RemoveRange<TItemType>(this ICollection<TItemType> collection, IEnumerable<TItemType> items)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(nameof(collection));
			}
			if (items == null)
			{
				throw new ArgumentNullException(nameof(items));
			}
			foreach (TItemType item in items)
			{
				collection.Remove(item);
			}
		}

		/// <summary>
		/// Merges a value with an existing dictionary
		/// </summary>
		/// <typeparam name="TKeyType">Key type</typeparam>
		/// <typeparam name="TValueType">Value type</typeparam>
		/// <param name="dictionary">Dictionary</param>
		/// <param name="key">Key</param>
		/// <param name="value">Value</param>
		/// <param name="replaceExisting">Whether to replace existing value</param>
		public static void Merge<TKeyType, TValueType>(this IDictionary<TKeyType, TValueType> dictionary, TKeyType key, TValueType value, bool replaceExisting = false)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException(nameof(dictionary));
			}

			if (replaceExisting || !dictionary.ContainsKey(key))
			{
				dictionary[key] = value;
			}
		}

		/// <summary>
		/// Merges a value with an existing dictionary
		/// </summary>
		/// <typeparam name="TKeyType">Key type</typeparam>
		/// <typeparam name="TValueType">Value type</typeparam>
		/// <param name="dictionary">Dictionary</param>
		/// <param name="keyValuePairs">Collection of key-value pairs to merge</param>
		/// <param name="replaceExisting">Whether to replace existing value</param>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public static void MergeRange<TKeyType, TValueType>(this IDictionary<TKeyType, TValueType> dictionary, IEnumerable<KeyValuePair<TKeyType, TValueType>> keyValuePairs, bool replaceExisting = false)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException(nameof(dictionary));
			}
			if (keyValuePairs != null)
			{
				foreach (KeyValuePair<TKeyType, TValueType> entry in keyValuePairs)
				{
					if (replaceExisting || !dictionary.ContainsKey(entry.Key))
					{
						dictionary[entry.Key] = entry.Value;
					}
				}
			}
		}

		/// <summary>
		/// Removes a range of keys from the specified dictionary
		/// </summary>
		/// <typeparam name="TKeyType">Key type</typeparam>
		/// <typeparam name="TValueType">Value type</typeparam>
		/// <param name="dictionary">Dictionary</param>
		/// <param name="keys">Keys to remove</param>
		public static void RemoveRange<TKeyType, TValueType>(this IDictionary<TKeyType, TValueType> dictionary, IEnumerable<TKeyType> keys)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException(nameof(dictionary));
			}
			if (keys == null)
			{
				throw new ArgumentNullException(nameof(keys));
			}
			foreach (TKeyType key in keys)
			{
				dictionary.Remove(key);
			}
		}

		/// <summary>
		/// Retrieves the value for a key from the dictionary
		/// </summary>
		/// <typeparam name="TKey">Dictionary key type</typeparam>
		/// <typeparam name="TValue">Dictionary value type</typeparam>
		/// <param name="dictionary">Dictionary</param>
		/// <param name="key">Key</param>
		/// <param name="defaultValue">Default value if key was not found</param>
		/// <returns>Found value or default</returns>
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
			TValue defaultValue = default(TValue))
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException(nameof(dictionary));
			}

			TValue val;
			if (!dictionary.TryGetValue(key, out val))
			{
				val = defaultValue;
			}
			return val;
		}

		/// <summary>
		/// Executes a code block for each of the items in the collection
		/// </summary>
		/// <typeparam name="TItemType">Item type</typeparam>
		/// <param name="collection">Collection</param>
		/// <param name="action">Action to be executed for each item</param>
		public static void ForEach<TItemType>(this IEnumerable<TItemType> collection, Action<TItemType> action)
		{
			if ((collection != null) && (action != null))
			{
				foreach (TItemType item in collection)
				{
					action(item);
				}
			}
		}

		/// <summary>
		/// Executes a code block for each of the items in the collection
		/// </summary>
		/// <typeparam name="TItemType">Item type</typeparam>
		/// <param name="collection">Collection</param>
		/// <param name="action">Action to be executed for each item</param>
		public static void ForEach<TItemType>(this IEnumerable<TItemType> collection, Action<TItemType, int> action)
		{
			if ((collection != null) && (action != null))
			{
				int iItem = 0;
				foreach (TItemType item in collection)
				{
					action(item, iItem);
					++iItem;
				}
			}
		}

		/// <summary>
		/// Executes a code block for each of the items in the collection
		/// </summary>
		/// <param name="collection">Collection</param>
		/// <param name="action">Action to be executed for each item</param>
		public static void ForEach(this IEnumerable collection, Action<object> action)
		{
			if ((collection != null) && (action != null))
			{
				foreach (object item in collection)
				{
					action(item);
				}
			}
		}

		/// <summary>
		/// Executes a code block for each of the items in the collection
		/// </summary>
		/// <param name="collection">Collection</param>
		/// <param name="action">Action to be executed for each item</param>
		public static void ForEach(this IEnumerable collection, Action<object, int> action)
		{
			if ((collection != null) && (action != null))
			{
				int iItem = 0;
				foreach (object item in collection)
				{
					action(item, iItem);
					++iItem;
				}
			}
		}

		private const string _defaultStringizeDictionaryEntrySeparator = "&#^@@";
		private const string _defaultStringizeDictionaryKeyValueSeparator = "^#*$%";
		/// <summary>
		/// Converts a string dictionary to string
		/// </summary>
		/// <param name="dictionary">Dictionary</param>
		/// <param name="entrySeparator">Entry Separator</param>
		/// <param name="keyValueSeparator">Key Value Separator</param>
		/// <returns>Stringized dictionary</returns>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Stringize")]
		public static string StringizeDictionary(this Dictionary<string, string> dictionary, string entrySeparator = null,
			string keyValueSeparator = null)
		{
			if (dictionary == null)
			{
				return null;
			}
			entrySeparator = entrySeparator ?? _defaultStringizeDictionaryEntrySeparator;
			keyValueSeparator = keyValueSeparator ?? _defaultStringizeDictionaryKeyValueSeparator;

			// Combine
			var sbBuffer = new StringBuilder();
			foreach (var entry in dictionary)
			{
				sbBuffer.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}",
					entrySeparator,
					entry.Key,
					keyValueSeparator,
					(entry.Value != null) ? 1 : 0,
					entry.Value ?? string.Empty);
			}
			return sbBuffer.ToString();
		}

		/// <summary>
		/// Converts a string to a string dictionary
		/// </summary>
		/// <param name="dictionary">Dictionary to fill</param>
		/// <param name="dictionaryText">Dictionary text</param>
		/// <param name="entrySeparator">Entry Separator</param>
		/// <param name="keyValueSeparator">Key Value Separator</param>
		/// <returns>Stringized dictionary</returns>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unstringize")]
		public static int UnstringizeDictionary(this Dictionary<string, string> dictionary, string dictionaryText, string entrySeparator = null,
			string keyValueSeparator = null)
		{
			if ((dictionary == null) || string.IsNullOrWhiteSpace(dictionaryText))
			{
				return 0;
			}
			dictionaryText = dictionaryText.Trim();
			entrySeparator = entrySeparator ?? _defaultStringizeDictionaryEntrySeparator;
			keyValueSeparator = keyValueSeparator ?? _defaultStringizeDictionaryKeyValueSeparator;

			// Split
			var entries = dictionaryText.Split(new[] { entrySeparator }, StringSplitOptions.None);

			// Combine
			foreach (var entry in entries.Skip(1))
			{
				var kvPair = entry.Split(new[] { keyValueSeparator }, StringSplitOptions.None);
				var key = kvPair[0];
				var valNonNull = kvPair[1][0] == '1';
				var val = valNonNull ? kvPair[1].Substring(1) : null;
				dictionary[key] = val;
			}
			return entries.Length - 1;
		}

		/// <summary>
		/// Adds a value for a key in the name-value collection
		/// </summary>
		/// <param name="nameValueCollection">Name value collection</param>
		/// <param name="key">Key</param>
		/// <param name="value">Value</param>
		public static void Add(this Dictionary<string, List<string>> nameValueCollection, string key, string value)
		{
			if (nameValueCollection == null)
			{
				throw new ArgumentNullException(nameof(nameValueCollection));
			}

			List<string> valueList;
			if (!nameValueCollection.TryGetValue(key, out valueList))
			{
				nameValueCollection[key] = valueList = new List<string>();
			}
			valueList.Add(value);
		}

		/// <summary>
		/// Gets the values against the specified key
		/// </summary>
		/// <param name="nameValueCollection">Name value collection</param>
		/// <param name="key">Key</param>
		public static string[] GetValues(this Dictionary<string, List<string>> nameValueCollection, string key)
		{
			if (nameValueCollection == null)
			{
				throw new ArgumentNullException(nameof(nameValueCollection));
			}

			List<string> valueList;
			if (nameValueCollection.TryGetValue(key, out valueList))
			{
				return valueList.ToArray();
			}
			return null;
		}
	}
}
