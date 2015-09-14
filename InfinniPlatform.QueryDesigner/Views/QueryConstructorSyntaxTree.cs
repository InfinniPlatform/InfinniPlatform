using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils.Drawing;
using DevExpress.XtraNavBar;
using DevExpress.XtraNavBar.ViewInfo;
using InfinniPlatform.QueryDesigner.Contracts;
using InfinniPlatform.QueryDesigner.Properties;

namespace InfinniPlatform.QueryDesigner.Views
{
    public partial class QueryConstructorSyntaxTree : UserControl, IInitializedOnLoad
    {
        private readonly Dictionary<NavBarItemLink, Control> _associatedControls =
            new Dictionary<NavBarItemLink, Control>();

        public QueryConstructorSyntaxTree()
        {
            InitializeComponent();
        }

        public Action<Control> OnPressFromSection { get; set; }
        public Action<Control> OnPressSelectSection { get; set; }
        public Action<Control> OnPressWhereAction { get; set; }
        public Action<Control> OnPressJoinAction { get; set; }
        public Func<NavBarItemLink, Control> OnAddFromControl { get; set; }
        public Func<NavBarItemLink, Control> OnAddJoinControl { get; set; }
        public Action<Control> OnRemoveJoinControl { get; set; }
        public Action<Control> OnRemoveWhereControl { get; set; }
        public Func<NavBarItemLink, Control> OnAddSelectControl { get; set; }
        public Func<NavBarItemLink, Control> OnAddWhereControl { get; set; }

        public void OnLoad()
        {
            if (DesignMode)
            {
                return;
            }

            NavBarSyntax.Items.Clear();
            var group = NavBarSyntax.Groups.Cast<NavBarGroup>().First(g => g.Name == "GroupFROM");

            var itemlink = group.AddItem();
            itemlink.Item.Caption = "From";
            itemlink.Item.Name = "FromItem";

            if (OnAddFromControl != null)
            {
                var control = OnAddFromControl(itemlink);
                _associatedControls.Add(itemlink, control);
            }


            group = NavBarSyntax.Groups.Cast<NavBarGroup>().First(g => g.Name == "GroupSELECT");

            itemlink = group.AddItem();
            itemlink.Item.Caption = "Select";
            itemlink.Item.Name = "SelectItem";

            if (OnAddSelectControl != null)
            {
                var control = OnAddSelectControl(itemlink);
                _associatedControls.Add(itemlink, control);
            }

            group = NavBarSyntax.Groups.Cast<NavBarGroup>().First(g => g.Name == "GroupWHERE");

            itemlink = group.AddItem();
            itemlink.Item.Caption = "Where";
            itemlink.Item.Name = "WhereItem";

            if (OnAddWhereControl != null)
            {
                var control = OnAddWhereControl(itemlink);
                _associatedControls.Add(itemlink, control);
            }

            foreach (var navGroup in NavBarSyntax.Groups.Cast<NavBarGroup>())
            {
                navGroup.Expanded = true;
            }
        }

        private Control GetAssociatedControl(NavBarItemLink link)
        {
            return _associatedControls[link];
        }

        private void RemoveAssociatedControl(NavBarItemLink link)
        {
            _associatedControls.Remove(link);
        }

        private void ButtonAddJoinClick(object sender, EventArgs e)
        {
            var group = NavBarSyntax.Groups.Cast<NavBarGroup>().First(g => g.Name == "GroupJOIN");

            var itemLinkNum = (group.ItemLinks.Count + 1).ToString();
            var itemlink = group.AddItem();
            itemlink.Item.Caption = "Join_" + itemLinkNum;
            itemlink.Item.Name = "JoinItem_" + itemLinkNum;
            itemlink.Item.SmallImage = Resources.delete_16x16;
            if (OnAddJoinControl != null)
            {
                var control = OnAddJoinControl(itemlink);
                _associatedControls.Add(itemlink, control);
            }
        }

        private void NavBarSyntaxLinkClicked(object sender, NavBarLinkEventArgs e)
        {
            if (e.Link.Item.Name == "SelectItem")
            {
                if (OnPressSelectSection != null)
                {
                    OnPressSelectSection(GetAssociatedControl(e.Link));
                }
            }

            if (e.Link.Item.Name == "FromItem")
            {
                if (OnPressFromSection != null)
                {
                    OnPressFromSection(GetAssociatedControl(e.Link));
                }
            }

            if (e.Link.Item.Name.Contains("WhereItem"))
            {
                if (OnPressWhereAction != null)
                {
                    OnPressWhereAction(GetAssociatedControl(e.Link));
                }
            }

            if (e.Link.Item.Name.Contains("JoinItem"))
            {
                if (OnPressJoinAction != null)
                {
                    OnPressJoinAction(GetAssociatedControl(e.Link));
                }
            }
        }

        private void NavBarSyntax_CustomDrawLink(object sender, CustomDrawNavBarElementEventArgs e)
        {
            if (e.ObjectInfo.State == ObjectState.Hot || e.ObjectInfo.State == ObjectState.Pressed)
            {
                LinearGradientBrush brush;
                var textFormat = new StringFormat();
                textFormat.LineAlignment = StringAlignment.Center;
                var linkInfo = e.ObjectInfo as NavLinkInfoArgs;
                if (linkInfo.Link.Group.GroupCaptionUseImage == NavBarImage.Large)
                {
                    textFormat.Alignment = StringAlignment.Center;
                }
                else
                {
                    textFormat.Alignment = StringAlignment.Near;
                }
                if (e.ObjectInfo.State == ObjectState.Hot)
                {
                    brush = new LinearGradientBrush(e.RealBounds, Color.Orange, Color.PeachPuff,
                        LinearGradientMode.Horizontal);
                }
                else
                    brush = new LinearGradientBrush(e.RealBounds, Color.PeachPuff, Color.Orange,
                        LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(new SolidBrush(Color.OrangeRed), e.RealBounds);
                e.Graphics.FillRectangle(brush, e.RealBounds);
                if (e.Image != null)
                {
                    var imageRect = linkInfo.ImageRectangle;
                    imageRect.X += (imageRect.Width - e.Image.Width)/2;
                    imageRect.Y += (imageRect.Height - e.Image.Height)/2;
                    imageRect.Size = e.Image.Size;
                    e.Graphics.DrawImageUnscaled(e.Image, imageRect);
                }
                var rect = e.RealBounds;
                rect.Inflate(-1, -1);
                e.Appearance.DrawString(e.Cache, e.Caption, linkInfo.RealCaptionRectangle, Brushes.White);
                e.Graphics.DrawRectangle(Pens.Indigo, rect);
                e.Handled = true;
            }
        }

        private void NavBarSyntax_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var hitInfo = NavBarSyntax.CalcHitInfo(e.Location);
                if (hitInfo.HitTest == NavBarHitTest.LinkImage && hitInfo.Link.Item.Name.Contains("Join"))
                {
                    if (MessageBox.Show(
                        string.Format("Please, confirm delete section \"{0}\"", hitInfo.Link.Item.Name),
                        "Confirm needed",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (OnRemoveJoinControl != null)
                        {
                            OnRemoveJoinControl(_associatedControls[hitInfo.Link]);
                        }

                        RemoveAssociatedControl(hitInfo.Link);
                        var group = hitInfo.Link.Group;
                        group.ItemLinks.Remove(hitInfo.Link);
                    }
                }
            }
        }
    }
}