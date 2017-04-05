using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Myra.Graphics2D
{
	public class NinePatchDrawable : Drawable
	{
		private readonly NinePatchRegion2D _region;

		public Point Size
		{
			get { return new Point((int) _region.Size.Width, (int) _region.Size.Height); }
		}

		public TextureRegion2D TextureRegion
		{
			get
			{
				return _region;
			}
		}

		public NinePatchDrawable(NinePatchRegion2D region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}

			_region = region;
		}

		public void Draw(SpriteBatch batch, Rectangle dest)
		{
			batch.Draw(_region, dest, Color.White);
		}
	}
}