﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ICSharpCode.WpfDesign.PropertyGrid
{
	/// <summary>
	/// Extends ObservableCollection{T} with an AddSorted method to insert items in a sorted collection.
	/// </summary>
	public class SortedObservableCollection<T, K> : ObservableCollection<T>
	{
		/// <summary>
		/// Creates a new SortedObservableCollection instance.
		/// </summary>
		/// <param name="keySelector">The function to select the sorting key.</param>
		public SortedObservableCollection(Func<T, K> keySelector)
		{
			this.keySelector = keySelector;
			this.comparer = Comparer<K>.Default;
		}

		Func<T, K> keySelector;
		IComparer<K> comparer;

		/// <summary>
		/// Adds an item to a sorted collection.
		/// </summary>
		public void AddSorted(T item)
		{
			int i = 0;
			int j = Count - 1;

			while (i <= j) {
				int n = (i + j) / 2;
				int c = comparer.Compare(keySelector(item), keySelector(this[n]));

				if (c == 0) { i = n; break; }
				if (c > 0) i = n + 1;
				else j = n - 1;
			}

			Insert(i, item);
		}
	}
	
	/// <summary>
	/// A SortedObservableCollection{PropertyNode, string} that sorts by the PropertyNode's Name.
	/// </summary>
	public class PropertyNodeCollection : SortedObservableCollection<PropertyNode, string>
	{
		/// <summary>
		/// Creates a new PropertyNodeCollection instance.
		/// </summary>
		public PropertyNodeCollection() : base(n => n.Name)
		{
		}
	}
}
