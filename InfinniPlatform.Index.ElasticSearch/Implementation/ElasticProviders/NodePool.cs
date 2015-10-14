using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
	/// <summary>
	/// Пул узлов.
	/// </summary>
	/// <remarks>
	/// Обеспечивает простейший механизм актуализации состояния узлов (работает/не работает).
	/// </remarks>
	sealed class NodePool
	{
		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="nodeAddresses">Список всех узлов.</param>
		public NodePool(IEnumerable<Uri> nodeAddresses)
		{
			_nodeList = new Dictionary<Uri, NodeInfo>();

			if (nodeAddresses != null)
			{
				foreach (var nodeAddress in nodeAddresses)
				{
					_nodeList[nodeAddress] = new NodeInfo(nodeAddress);
				}
			}
		}


		private volatile IEnumerable<Uri> _actualNodes;
		private readonly Dictionary<Uri, NodeInfo> _nodeList;
		private readonly object _actualNodesSync = new object();


		/// <summary>
		/// Помечает узел, как рабочий.
		/// </summary>
		/// <remarks>
		/// Пользователи узла вызывают этот метод, если считают, что узел рабочий.
		/// </remarks>
		public void NodeIsWork(Uri nodeAddress)
		{
			// Если состояние узла поменялось, актуализируем список узлов

			NodeInfo node;

			if (_nodeList.TryGetValue(nodeAddress, out node))
			{
				if (!node.IsWork)
				{
					lock (_actualNodesSync)
					{
						if (!node.IsWork)
						{
							node.IsWork = true;

							_actualNodes = null;
						}
					}
				}
			}
		}

		/// <summary>
		/// Помечает узел, как нерабочий.
		/// </summary>
		/// <remarks>
		/// Пользователи узла вызывают этот метод, если считают, что узел нерабочий.
		/// </remarks>
		public void NodeIsNotWork(Uri nodeAddress)
		{
			// Если состояние узла поменялось, актуализируем список узлов

			NodeInfo node;

			if (_nodeList.TryGetValue(nodeAddress, out node))
			{
				if (node.IsWork)
				{
					lock (_actualNodesSync)
					{
						if (node.IsWork)
						{
							node.IsWork = false;

							_actualNodes = null;
						}
					}
				}
			}
		}

		/// <summary>
		/// Возврвщает список актуальный узлов (сначала рабочие).
		/// </summary>
		public IEnumerable<Uri> GetActualNodes()
		{
			var result = _actualNodes;

			if (result == null)
			{
				lock (_actualNodesSync)
				{
					result = _actualNodes;

					if (result == null)
					{
						// Сортируем узлы по их доступности (сначала рабочие)

						result = _nodeList.Values
							.OrderByDescending(i => i.IsWork)
							.Select(i => i.Address)
							.ToArray();

						_actualNodes = result;
					}
				}
			}

			return result;
		}


		private class NodeInfo
		{
			public NodeInfo(Uri address)
			{
				Address = address;
				IsWork = true;
			}

			public readonly Uri Address;

			public bool IsWork;
		}
	}
}