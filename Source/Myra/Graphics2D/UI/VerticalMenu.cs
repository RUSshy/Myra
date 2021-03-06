﻿using Myra.Graphics2D.UI.Styles;

namespace Myra.Graphics2D.UI
{
	public class VerticalMenu : Menu
	{
		public override Orientation Orientation
		{
			get { return Orientation.Vertical; }
		}

		public VerticalMenu(MenuStyle style) : base(style)
		{
			HorizontalAlignment = HorizontalAlignment.Left;
			VerticalAlignment = VerticalAlignment.Stretch;
		}

		public VerticalMenu()
			: base(DefaultAssets.UIStylesheet.VerticalMenuStyle)
		{
		}
	}
}
