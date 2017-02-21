using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LQ
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> Where<T>(this IEnumerable<T> collection, Func<T, bool> expression)
		{
			foreach (var item in collection)
			{
				if (expression(item)) yield return item;
			}
		}

		public static IEnumerable<TResult> Select<TInput, TResult>(this IEnumerable<TInput> collection, Func<TInput, TResult> expression)
		{
			foreach (var item in collection) yield return expression(item);
		}

		public static IEnumerable<T> Take<T>(this IEnumerable<T> collection, int quantity)
		{
			IEnumerator<T> enumerator = collection.GetEnumerator();
			int i = -1;
			if (enumerator.MoveNext())
			{
				while (++i < quantity)
				{
					yield return enumerator.Current;
					if (!enumerator.MoveNext()) break;
				}
			}
		}

		public static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> collection, Func<T, bool> expression)
		{
			IEnumerator<T> enumerator = collection.GetEnumerator();
			if (enumerator.MoveNext())
			{
				while (expression(enumerator.Current))
				{
					yield return enumerator.Current;
					if (!enumerator.MoveNext()) break;
				}
			}
		}

		public static IEnumerable<T> Skip<T>(this IEnumerable<T> collection, int quantity)
		{
			IEnumerator<T> enumerator = collection.GetEnumerator();
			bool vazia = false;
			if (enumerator.MoveNext())
			{

				for (int i = 0; i < quantity; i++) if (!enumerator.MoveNext()) { vazia = true; break; }
				if (!vazia)
				{

					do
					{
						yield return enumerator.Current;
					} while (enumerator.MoveNext());
				}
			}
		}

		public static IEnumerable<T> SkipWhile<T>(this IEnumerable<T> collection, Func<T, bool> expression)
		{
			IEnumerator<T> enumerator = collection.GetEnumerator();
			if (enumerator.MoveNext())
			{
				while (expression(enumerator.Current))
				{
					enumerator.MoveNext();
				}
				do
				{
					yield return enumerator.Current;
				} while (enumerator.MoveNext());
			}
		}

		public static IEnumerable<T> OrderBy<T, TProperty>(this IEnumerable<T> collection, Func<T, TProperty> expression)
		{
			List<int> indexes = new List<int>();
			IEnumerator<T> enumerator = collection.GetEnumerator();
			int count = 0;
			while (enumerator.MoveNext()) count++;

			while (indexes.Count != count)
			{
				enumerator.Reset();
				if (enumerator.MoveNext())
				{
					T val = enumerator.Current;
					int index = -1;
					int i = 0;
					do
					{
						if (indexes.IndexOf(i) != -1) { i++; continue; }
						if (index == -1)
						{
							val = enumerator.Current;
							index = i;
						}
						int comparation = Comparer<TProperty>.Default.Compare(expression(val), expression(enumerator.Current));
						if (comparation == 1)
						{
							val = enumerator.Current;
							index = i;
						}
						i++;
					} while (enumerator.MoveNext());
					indexes.Add(index);
					yield return val;
				}
			}
		}

		public static Dictionary<TKey, ICollection<T>> GroupBy<T, TKey>(this IEnumerable<T> collection, Func<T, TKey> expression)
		{
			var enumerator = collection.GetEnumerator();
			var count = enumerator.Count();
			IEnumerable<T>[] result = new IEnumerable<T>[count];

			Dictionary<TKey, ICollection<T>> group = new Dictionary<TKey, ICollection<T>>();

			if (enumerator.MoveNext())
			{
				do
				{
					var key = expression(enumerator.Current);
					if(group.ContainsKey(key))
					{
						group[key].Add(enumerator.Current);
					}
					else
					{
						group[key] = new List<T>();
						group[key].Add(enumerator.Current);
					}
				} while (enumerator.MoveNext());
			}
			return group;
		}

		public static IEnumerable<T> Concat<T>(this IEnumerable<T> collection, IEnumerable<T> secondCollection)
		{
			List<T> result = new List<T>();
			var enumerator = collection.GetEnumerator();
			var enumerator2 = secondCollection.GetEnumerator();
			while (enumerator.MoveNext()) result.Add(enumerator.Current);
			while (enumerator2.MoveNext()) result.Add(enumerator2.Current);
			return result;
		}

		public static IEnumerable<T> Distinct<T>(this IEnumerable<T> collection)
		{
			return collection.Distinct(EqualityComparer<T>.Default);
		}

		public static IEnumerable<T> Distinct<T>(this IEnumerable<T> collection, IEqualityComparer<T> comparer )
		{
			List<T> list = new List<T>();
			var enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!Contains(list, enumerator.Current, comparer)) list.Add(enumerator.Current);
			}
			return list;
		}

		public static IEnumerable<T> Union<T>(this IEnumerable<T> collection, IEnumerable<T> secondCollection)
		{
			return collection.Union(secondCollection, EqualityComparer<T>.Default);
		}

		public static IEnumerable<T> Union<T>(this IEnumerable<T> collection, IEnumerable<T> secondCollection, IEqualityComparer<T> comparer)
		{
			List<T> list = new List<T>();
			var enumerator = collection.GetEnumerator();
			var enumerator2 = secondCollection.GetEnumerator();

			while (enumerator.MoveNext())
			{
				if (!Contains(list, enumerator.Current, comparer))
				{
					list.Add(enumerator.Current);
				}
			}

			while (enumerator2.MoveNext())
			{
				if (!Contains(list, enumerator2.Current, comparer))
				{
					list.Add(enumerator2.Current);
				}
			}

			return list;
		}

		public static IEnumerable<T> Intersect<T>(this IEnumerable<T> collection, IEnumerable<T> secondCollection)
		{
			return collection.Intersect(secondCollection, EqualityComparer<T>.Default);
		}

		public static IEnumerable<T> Intersect<T>(this IEnumerable<T> collection, IEnumerable<T> secondCollection, IEqualityComparer<T> comparer)
		{
			List<T> result = new List<T>();
			var enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (Contains(secondCollection, enumerator.Current, comparer)) result.Add(enumerator.Current);
			}
			return result;
		}

		public static IEnumerable<T> Except<T>(this IEnumerable<T> collection, IEnumerable<T> secondCollection)
		{
			return collection.Except(secondCollection, EqualityComparer<T>.Default);
		}

		public static IEnumerable<T> Except<T>(this IEnumerable<T> collection, IEnumerable<T> secondCollection, IEqualityComparer<T> comparer)
		{
			List<T> result = new List<T>();
			var enumerator = collection.GetEnumerator();
			var enumerator2 = secondCollection.GetEnumerator();

			while (enumerator.MoveNext())
			{
				if (!Contains(secondCollection, enumerator.Current, comparer)) result.Add(enumerator.Current);
			}
			while (enumerator2.MoveNext())
			{
				if (!Contains(collection, enumerator2.Current, comparer)) result.Add(enumerator2.Current);
			}

			return result;
		}

		public static IEnumerable<T> Reverse<T>(this IEnumerable<T> collection)
		{
			List<T> result = new List<T>();

			var enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				result.Insert(0, enumerator.Current);
			}

			return result;
		}

		public static bool SequenceEqual<T>(this IEnumerable<T> collection, IEnumerable<T> secondCollection)
		{
			var enumerator = collection.GetEnumerator();
			var enumerator2 = secondCollection.GetEnumerator();
			var comparer = EqualityComparer<T>.Default;

			bool m1, m2;
			do
			{
				m1 = enumerator.MoveNext();
				m2 = enumerator2.MoveNext();

				if ((m1 != m2) || m1 && !comparer.Equals(enumerator.Current, enumerator2.Current)) return false;
			} while (m1 && m2);

			return true;
		}

		// Métodos auxiliares
		#region Helpers
		private static bool Contains<T>(IEnumerable<T> collection, T toCompare, IEqualityComparer<T> comparer)
		{
			var enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (comparer.Equals(enumerator.Current, toCompare)) return true;
			}
			return false;
		}

		private static int Count<T>(this IEnumerator<T> enumerator)
		{
			var count = 0;
			while (enumerator.MoveNext()) count++;
			enumerator.Reset();
			return count;
		}
		#endregion

	}
}
