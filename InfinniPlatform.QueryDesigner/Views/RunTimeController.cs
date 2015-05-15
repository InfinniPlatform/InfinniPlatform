using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfinniPlatform.QueryDesigner.Contracts;
using InfinniPlatform.QueryDesigner.Contracts.Implementation;

namespace InfinniPlatform.QueryDesigner.Views
{
	public sealed class RunTimeController
	{
		private readonly List<Control> _controls = new List<Control>();

		public RunTimeController(IEnumerable<Control> controls)
		{
			_controls.AddRange(controls);
			_queryParts.AddRange(controls.OfType<IQueryBlockProvider>());
		}


		private List<IQueryBlockProvider> _queryParts = new List<IQueryBlockProvider>(); 

		public void RegisterControl(Control control)
		{
			var provider = control as IQueryBlockProvider;
			if (provider != null)
			{
				_queryParts.Add(provider);
			}
			_controls.Add(control);
		}

		public void UnregisterControl(Control control)
		{
			var provider = control as IQueryBlockProvider;
			if (provider != null)
			{
				_queryParts.Remove(provider);
			}
			_controls.Remove(control);
		}

		public IEnumerable<T> GetProviderList<T>(Control parent = null ) where T : class
		{

			IEnumerable<Control> controlList;
			if (parent == null)
			{
				controlList = _controls;
			}
			else
			{
				controlList = parent.Controls.Cast<Control>().ToList();
			}

			var result = new List<T>();
			foreach (var control in controlList)
			{
				var provider = control as T;
				if (provider != null)
				{
					result.Add(provider);
				}

				result.AddRange(GetProviderList<T>(control));
			}
			return result;
		}

		public IEnumerable<IQueryBlockProvider> GetQueryPartProviders()
		{
			return _queryParts;
		}
	}
}
