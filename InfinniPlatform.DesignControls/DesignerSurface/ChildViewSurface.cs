﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.DesignControls.Controls.ChildViews;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;

namespace InfinniPlatform.DesignControls.DesignerSurface
{
	public partial class ChildViewSurface : UserControl
	{
		public ChildViewSurface()
        {
            InitializeComponent();

            gridBinding.DataSource = ChildViews;
        }

		private readonly List<ChildViewObject> _childViews = new List<ChildViewObject>();

		public List<ChildViewObject> ChildViews
	    {
		    get { return _childViews; }
	    }


	    private void repositoryItemButtonEditSource_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var childVIew = ChildViews.ElementAt(GridViewChildViews.FocusedRowHandle).ChildView as IPropertiesProvider;
            if (childVIew != null)
            {
                var form = new PropertiesForm();

	            var validationRules = childVIew.GetValidationRules();
				form.SetValidationRules(validationRules);
				var propertyEditors = childVIew.GetPropertyEditors();
				form.SetPropertyEditors(propertyEditors);
                var simpleProperties = childVIew.GetSimpleProperties();
                form.SetSimpleProperties(simpleProperties);
                var collectionProperties = childVIew.GetCollections();
                form.SetCollectionProperties(collectionProperties);

                if (form.ShowDialog() == DialogResult.OK)
                {
	                childVIew.ApplySimpleProperties();
	                childVIew.ApplyCollections();
	                GridViewChildViews.HideEditor();
	                GridViewChildViews.RefreshData();
                }
                else
                {
	                form.RevertChanges();
                }
            }
        }

        private void AddChildViewButtonClick(object sender, EventArgs e)
        {
            var childViewObject = new ChildViewObject();
            ChildViews.Add(childViewObject);

	        var childView = new ChildView();
            childViewObject.ChildView = childView;
	        childView.ObjectInspector = ObjectInspector;
            GridViewChildViews.RefreshData();
            
        }

        private void DeleteChildViewButtonClick(object sender, EventArgs e)
        {
            if (GridViewChildViews.FocusedRowHandle >= 0)
            {
                ChildViews.RemoveAt(GridViewChildViews.FocusedRowHandle);
                GridViewChildViews.RefreshData();
            }
        }

        private void GetLayoutButtonClick(object sender, EventArgs e)
        {
            dynamic value = GetLayout();

            var valueEdit = new ValueEdit();
            valueEdit.Value = value.ToString();
            valueEdit.ShowDialog();
        }

        private void SetLayoutButtonClick(object sender, EventArgs e)
        {
            var valueEdit = new ValueEdit();
            if (valueEdit.ShowDialog() == DialogResult.OK)
            {
                ProcessJson(valueEdit.Value);
            }
        }


        public void SetLayout(dynamic value)
        {
            
        }

	    public void ProcessJson(dynamic childViews)
        {
            ChildViews.Clear();
            foreach (dynamic source in childViews)
            {
                var viewObject = new ChildViewObject();
                var childView = new ChildView();
	            childView.ObjectInspector = ObjectInspector;
                childView.LoadProperties(source);
                viewObject.ChildView = childView;
                ChildViews.Add(viewObject);
            }
            GridViewChildViews.RefreshData();
        }

        public string GetPropertyName()
        {
            return "ChildViews";
        }

        private void GridViewCustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            var value = (ChildViewObject) GridViewChildViews.GetRow(e.RowHandle);

            if (string.IsNullOrEmpty(value.ChildViewName))
            {
                e.DisplayText = "<ChildView name not specified>";
            }
            else
            {
                e.DisplayText = value.ChildViewName;
            }
        }

        public dynamic GetLayout()
        {
            dynamic instanceLayout = new List<dynamic>();
            foreach (ChildViewObject childViewObject in ChildViews)
            {
                dynamic layout = childViewObject.GetLayout();
                instanceLayout.Add(layout);
            }
            return instanceLayout;
        }

		public ObjectInspectorTree ObjectInspector { get; set; }

		public void Clear()
		{
			_childViews.Clear();
			GridControlChildView.RefreshDataSource();
		}
	}
}
