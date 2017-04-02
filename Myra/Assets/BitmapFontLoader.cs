using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D;
using Myra.Graphics2D.Text;

namespace Myra.Assets
{
	public class BitmapFontLoader : IAssetLoader<BitmapFont>
	{
		public BitmapFont Load(AssetManager assetManager, Stream input, object parameters)
		{
			var result = BitmapFont.CreateFromFNT(input, path =>
			{
				if (path.Contains("/"))
				{
					// Means page is sprite in the spritesheet
					var parts = path.Split('/');

					var spriteSheet = assetManager.Load<SpriteSheet>(parts[0]);
					ImageDrawable drawable;
					if (!spriteSheet.Drawables.TryGetValue(parts[1], out drawable))
					{
						throw new Exception(string.Format("Could not find drawable '{0}'", parts[1]));
					}

					return drawable.TextureRegion;
				}

				// Ordinary texture
				var texture = assetManager.Load<Texture2D>(path);
				return new TextureRegion(texture);
			});

			return result;
		}
	}
}