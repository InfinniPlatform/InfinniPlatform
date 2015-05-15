using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json
{
	/// <summary>
	/// Предоставляет методы для перебора элементов JSON-массива из потока.
	/// </summary>
	public sealed class JsonArrayStreamEnumerator : IEnumerator<object>
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="stream">Поток, содержащий JSON-массив.</param>
		public JsonArrayStreamEnumerator(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException();
			}

			if (stream.CanRead == false)
			{
				throw new ArgumentException();
			}

			_stream = stream;

			Reset();
		}


		private readonly Stream _stream;


		private bool _canRead;
		private int _openArrays;
		private JToken _element;
		private JsonTextReader _reader;


		public void Reset()
		{
			_canRead = true;
			_openArrays = 0;
			_element = null;

			RecreateReader();
		}

		public bool MoveNext()
		{
			var result = false;

			if (_canRead)
			{
				FindStartArray();

				var element = FindNextElement();

				result = (element != null);

				_element = element;
			}

			return result;
		}

		public void Dispose()
		{
			CloseReader();
		}


		private void RecreateReader()
		{
			CloseReader();

			_stream.Position = 0;

			_reader = new JsonTextReader(new StreamReader(_stream));
		}

		private void CloseReader()
		{
			if (_reader != null)
			{
				_reader.Close();
				_reader = null;
			}
		}


		private void FindStartArray()
		{
			if (_openArrays == 0)
			{
				var token = ReadNextToken();

				if (token != JsonToken.StartArray)
				{
					throw new InvalidOperationException("The source document does not contains an array.");
				}

				++_openArrays;
			}
		}

		private JToken FindNextElement()
		{
			JContainer container = null;

			var readConstant = false;

			do
			{
				var token = ReadNextToken();

				switch (token)
				{
					case JsonToken.None:
					case JsonToken.Comment:
						{
							continue;
						}
					case JsonToken.StartObject:
						{
							var content = new JObject();
							AddToContainer(container, content);
							container = content;

							break;
						}
					case JsonToken.EndObject:
						{
							JContainer parent;

							if (ParentIsRoot(container, out parent))
							{
								return container;
							}

							container = parent;

							break;
						}
					case JsonToken.StartArray:
						{
							++_openArrays;

							var content = new JArray();
							AddToContainer(container, content);
							container = content;

							break;
						}
					case JsonToken.EndArray:
						{
							if (EndArray())
							{
								return null;
							}

							JContainer parent;

							if (ParentIsRoot(container, out parent))
							{
								return container;
							}

							container = parent;

							break;
						}
					case JsonToken.StartConstructor:
						{
							var content = new JConstructor(_reader.Value as string);
							AddToContainer(container, content);
							container = content;

							break;
						}
					case JsonToken.EndConstructor:
						{
							JContainer parent;

							if (ParentIsRoot(container, out parent))
							{
								return container;
							}

							container = parent;

							break;
						}
					case JsonToken.PropertyName:
						{
							var parentObject = container as JObject;

							if (parentObject == null)
							{
								throw new InvalidOperationException("The source document has the wrong format.");
							}

							var propertyName = (_reader.Value as string);
							var property = new JProperty(propertyName, null);

							var parentObjectProperty = parentObject.Property(propertyName);

							if (parentObjectProperty != null)
							{
								parentObject.Replace(property);
							}
							else
							{
								parentObject.Add(property);
							}

							container = property;

							break;
						}
					case JsonToken.String:
					case JsonToken.Integer:
					case JsonToken.Float:
					case JsonToken.Boolean:
					case JsonToken.Date:
					case JsonToken.Bytes:
						{
							var content = new JValue(_reader.Value);
							AddToContainer(container, content);

							if (container == null)
							{
								return content;
							}

							readConstant = true;

							break;
						}
					case JsonToken.Null:
					case JsonToken.Undefined:
						{
							var content = new JValue((object)null);
							AddToContainer(container, content);

							if (container == null)
							{
								return content;
							}

							readConstant = true;

							break;
						}
					default:
						{
							throw new NotSupportedException(string.Format("The token '{0}' is not supported.", _reader.Value));
						}
				}

				var parentAsProperty = container as JProperty;

				if (parentAsProperty != null && (readConstant || (parentAsProperty.Value != null && (parentAsProperty.Value.HasValues || parentAsProperty.Value.Type != JTokenType.Null))))
				{
					container = container.Parent;
				}

				readConstant = false;
			} while (true);
		}

		private JsonToken ReadNextToken()
		{
			if (_reader.Read() == false)
			{
				throw new InvalidOperationException("The source document has the wrong format.");
			}

			return _reader.TokenType;
		}

		private static bool ParentIsRoot(JContainer container, out JContainer parent)
		{
			if (container == null)
			{
				throw new InvalidOperationException("The source document has the wrong format.");
			}

			parent = container.Parent;

			return parent == null;
		}

		private static void AddToContainer(JContainer container, JToken content)
		{
			if (container != null)
			{
				if (container is JProperty)
				{
					((JProperty)container).Value = content;
				}
				else
				{
					container.Add(content);
				}
			}
		}

		private bool EndArray()
		{
			--_openArrays;

			if (_openArrays < 0)
			{
				throw new ArgumentException("The source document has the wrong format.");
			}

			if (_openArrays == 0)
			{
				_canRead = false;

				return true;
			}

			return false;
		}


		public object Current
		{
			get { return _element; }
		}

		object IEnumerator.Current
		{
			get { return Current; }
		}
	}
}