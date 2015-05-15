﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.DesignControls.Layout;

namespace InfinniPlatform.DesignControls.DesignerSurface
{
	public sealed class ChildViewObject : ILayoutProvider
	{
		public string ChildViewName
		{
			get { return ChildView != null ? ObjectHelper.GetProperty(ChildView, "Name") : string.Empty; }
		}

		public dynamic ChildView { get; set; }

		public string ChildViewString
		{
			get { return ChildView != null ? 
				ChildView.ToString() : 
				string.Empty; }
		}

		public dynamic GetLayout()
		{
			var layoutProvider = ChildView as ILayoutProvider;
			if (layoutProvider != null)
			{
				return layoutProvider.GetLayout();
			}
			return new DynamicWrapper();
		}

		public void SetLayout(dynamic value)
		{
			//no layout
		}

		public string GetPropertyName()
		{
			var layoutProvider = ChildView as ILayoutProvider;
			if (layoutProvider != null)
			{
				return layoutProvider.GetPropertyName();
			}
			return string.Empty;
		}
	}
}
