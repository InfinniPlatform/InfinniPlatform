using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using InfinniPlatform.Sdk.Properties;
using Microsoft.CSharp.RuntimeBinder;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace InfinniPlatform.Sdk.Dynamic
{
	public static class ObjectHelper
	{
		private static readonly IEnumerable<CSharpArgumentInfo> GetPropertyArgumentInfo
			= new List<CSharpArgumentInfo>
			  {
				  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
			  };

		private static readonly IEnumerable<CSharpArgumentInfo> SetPropertyArgumentInfo
			= new List<CSharpArgumentInfo>
			  {
				  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
				  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
			  };


		/// <summary>
		/// Возвращает значение свойства.
		/// </summary>
		/// <param name="target">Исходный объект.</param>
		/// <param name="propertyPath">Путь к свойству объекта.</param>
		/// <returns>Значение свойства.</returns>
		public static object GetProperty(this object target, string propertyPath)
		{
			var propertyPathTerms = SplitPropertyPath(propertyPath);
			return GetPropertyByPath(target, propertyPathTerms);
		}

		/// <summary>
		/// Устанавливает значение свойства.
		/// </summary>
		/// <param name="target">Исходный объект.</param>
		/// <param name="propertyPath">Путь к свойству объекта.</param>
		/// <param name="propertyValue">Значение свойства.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public static void SetProperty(this object target, string propertyPath, object propertyValue)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target", Resources.TargetObjectCannotBeNull);
			}

			if (string.IsNullOrEmpty(propertyPath))
			{
				throw new ArgumentNullException("propertyPath", Resources.PropertyPathCannotBeNullOrEmpty);
			}

			var propertyPathTerms = SplitPropertyPath(propertyPath);
			SetPropertyByPath(target, propertyPathTerms, propertyValue);
		}

		/// <summary>
		/// Заменяет свойства одного объекта свойствами другого.
		/// </summary>
		/// <param name="target">Исходный объект.</param>
		/// <param name="value">Объект замены.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public static void ReplaceProperties(this object target, object value)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target", Resources.TargetObjectCannotBeNull);
			}

			// Удаление свойств в исходном объекте

			var targetAsDynamic = (dynamic)target;
			targetAsDynamic.Clear();

			if (value != null)
			{
				// Копирование свойств в исходный объект

				var valueAsDynamic = (dynamic)value;

				foreach (var property in valueAsDynamic)
				{
					targetAsDynamic[property.Key] = valueAsDynamic[property.Key];
				}
			}
		}

		private static string[] SplitPropertyPath(string propertyPath)
		{
			return string.IsNullOrEmpty(propertyPath) ? null : propertyPath.TrimEnd('.', '$').Split('.');
		}

		private static object GetPropertyByPath(object target, string[] propertyPathTerms)
		{
			if (target != null && propertyPathTerms != null)
			{
				var parent = target;
				var length = propertyPathTerms.Length;

				for (var i = 0; i < length; ++i)
				{
					if (parent != null)
					{
						var term = propertyPathTerms[i];
						int termCollectionIndex;

						parent = IsCollectionIndex(term, out termCollectionIndex)
									 ? GetItem(parent, termCollectionIndex)
									 : GetObjectProperty(parent, term);
					}
					else
					{
						return null;
					}
				}

				return parent;
			}

			return target;
		}

		private static void SetPropertyByPath(object target, string[] propertyPathTerms, object propertyValue)
		{
			var parent = target;
			var length = propertyPathTerms.Length - 1;

			var term = propertyPathTerms[0];
			int termCollectionIndex;
			var termIsCollectionIndex = IsCollectionIndex(term, out termCollectionIndex);

			for (var i = 0; i < length; ++i)
			{
				var termValue = termIsCollectionIndex
									? GetItem(parent, termCollectionIndex)
									: GetObjectProperty(parent, term);

				var nextTerm = propertyPathTerms[i + 1];
				int nextTermCollectionIndex;
				var nextTermIsCollectionIndex = IsCollectionIndex(nextTerm, out nextTermCollectionIndex);

				if (termValue == null)
				{
					if (nextTermIsCollectionIndex)
					{
						termValue = new List<dynamic>();
					}
					else
					{
						termValue = new DynamicWrapper();
					}

					if (termIsCollectionIndex)
					{
						SetItem(parent, termCollectionIndex, termValue);
					}
					else
					{
						SetObjectProperty(parent, term, termValue);
					}
				}

				parent = termValue;
				term = nextTerm;
				termCollectionIndex = nextTermCollectionIndex;
				termIsCollectionIndex = nextTermIsCollectionIndex;
			}

			if (termIsCollectionIndex)
			{
				SetItem(parent, termCollectionIndex, propertyValue);
			}
			else
			{
				SetObjectProperty(parent, term, propertyValue);
			}
		}

		private static bool IsCollectionIndex(string term, out int index)
		{
			if (term == "$")
			{
				index = 0;
			}
			else if (int.TryParse(term, out index) == false)
			{
				index = -1;
			}

			return (index >= 0);
		}

		private static object GetObjectProperty(object target, string propertyName)
		{
			var targetContext = GetTargetContext(target);
			var callSiteBinder = Binder.GetMember(CSharpBinderFlags.None, propertyName, targetContext, GetPropertyArgumentInfo);
			var callSite = CallSite<Func<CallSite, object, object>>.Create(callSiteBinder);

			try
			{
				return callSite.Target(callSite, target);
			}
			catch (RuntimeBinderException)
			{
				// Если заданного свойства не существует

				return null;
			}
		}

		private static void SetObjectProperty(object target, string propertyName, object propertyValue)
		{
			var targetContext = GetTargetContext(target);
			var callSiteBinder = Binder.SetMember(CSharpBinderFlags.None, propertyName, targetContext, SetPropertyArgumentInfo);
			var callSite = CallSite<Func<CallSite, object, object, object>>.Create(callSiteBinder);
			callSite.Target(callSite, target, propertyValue);
		}

		private static Type GetTargetContext(object target)
		{
			var context = target.GetType();

			if (context.IsArray)
			{
				context = typeof(object);
			}

			return context;
		}


		/// <summary>
		/// Возвращает элемент коллекции.
		/// </summary>
		/// <param name="target">Исходная коллекция.</param>
		/// <param name="index">Индекс элемента.</param>
		/// <returns>Элемент коллекции.</returns>
		public static object GetItem(this object target, int index)
		{
			object result = null;

			if (target != null && index >= 0)
			{
				var list = target as IList;

				if (list != null)
				{
					if (index < list.Count)
					{
						result = list[index];
					}
				}
				else
				{
					var enumerable = target as IEnumerable;

					if (enumerable != null)
					{
						foreach (var item in enumerable)
						{
							if (index == 0)
							{
								result = item;
								break;
							}

							--index;
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Устанавливает элемент коллекции.
		/// </summary>
		/// <param name="target">Исходная коллекция.</param>
		/// <param name="index">Индекс элемента.</param>
		/// <param name="item">Элемент коллекции.</param>
		public static void SetItem(this object target, int index, object item)
		{
			ModifyCollection(target, list => list[index] = item);
		}

		/// <summary>
		/// Добавляет элемент в коллекцию.
		/// </summary>
		/// <param name="target">Исходная коллекция.</param>
		/// <param name="item">Элемент коллекции.</param>
		public static void AddItem(this object target, object item)
		{
			ModifyCollection(target, list => list.Add(item));
		}

		/// <summary>
		/// Заменяет элемент в коллекции.
		/// </summary>
		/// <param name="target">Исходная коллекция.</param>
		/// <param name="item">Элемент коллекции.</param>
		/// <param name="newItem">Новый элемент коллекции.</param>
		public static void ReplaceItem(this object target, object item, object newItem)
		{
			ModifyCollection(target, list =>
									 {
										 var index = list.IndexOf(item);

										 if (index >= 0)
										 {
											 list[index] = newItem;
										 }
										 else
										 {
											 list.Add(newItem);
										 }
									 });
		}

		/// <summary>
		/// Перемещает элемент в коллекции.
		/// </summary>
		/// <param name="target">Исходная коллекция.</param>
		/// <param name="item">Элемент коллекции.</param>
		/// <param name="delta">Смещение индекса.</param>
		public static void MoveItem(this object target, object item, int delta)
		{
			ModifyCollection(target, list =>
									 {
										 var index = list.IndexOf(item);

										 if (index >= 0)
										 {
											 var newIndex = index + delta;

											 if (newIndex != index && newIndex >= 0 && newIndex < list.Count)
											 {
												 list.RemoveAt(index);
												 list.Insert(newIndex, item);
											 }
										 }
									 });
		}

		/// <summary>
		/// Добавляет элемент в коллекцию.
		/// </summary>
		/// <param name="target">Исходная коллекция.</param>
		/// <param name="index">Индекс элемента.</param>
		/// <param name="item">Элемент коллекции.</param>
		public static void InsertItem(this object target, int index, object item)
		{
			ModifyCollection(target, list => list.Insert(index, item));
		}

		/// <summary>
		/// Удаляет элемент из коллекции.
		/// </summary>
		/// <param name="target">Исходная коллекция.</param>
		/// <param name="item">Элемент коллекции.</param>
		public static void RemoveItem(this object target, object item)
		{
			ModifyCollection(target, list => list.Remove(item));
		}

		/// <summary>
		/// Удаляет элемент из коллекции.
		/// </summary>
		/// <param name="target">Исходная коллекция.</param>
		/// <param name="index">Индекс элемента.</param>
		public static void RemoveItemAt(this object target, int index)
		{
			ModifyCollection(target, list => list.RemoveAt(index));
		}


		private static void ModifyCollection(object target, Action<IList> modify)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target", Resources.TargetObjectCannotBeNull);
			}

			var list = target as IList;

			if (list == null)
			{
				throw new NotSupportedException(Resources.CollectionCanNotBeModified);
			}

			modify(list);
		}


		/// <summary>
		/// Возвращает значение свойства по индексу.
		/// </summary>
		/// <param name="target">Исходный объект с индексатором.</param>
		/// <param name="indexes">Список индексов для доступа к значению.</param>
		/// <returns>Значение свойства.</returns>
		public static object GetIndexProperty(this object target, IEnumerable<object> indexes)
		{
			object result = null;

			if (target != null && indexes != null)
			{
				var arrayIndexes = indexes.ToArray();
				var length = arrayIndexes.Length;

				if (length > 0)
				{
					if (target is Array)
					{
						result = ((Array)target).GetValue(arrayIndexes.Cast<int>().ToArray());
					}
					else if (target is IEnumerable)
					{
						return ((IEnumerable)target).Cast<object>().ElementAt((int)arrayIndexes[0]);
					}
					else
					{
						var properties = target.GetType().GetProperties();

						foreach (var property in properties)
						{
							if (property.GetIndexParameters().Length == length)
							{
								try
								{
									return property.GetValue(target, arrayIndexes);
								}
								catch (TargetInvocationException error)
								{
									if (error.InnerException != null)
									{
										throw error.InnerException;
									}

									throw;
								}
							}
						}
					}
				}
			}

			return result;
		}
	}
}