using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using Myra.Graphics2D;

namespace Myra.Assets
{
	public class DrawableLoader : IAssetLoader<TextureRegion2D>
	{
		public TextureRegion2D Load(AssetManager assetManager, string assetName)
		{
			if (assetName.Contains(":"))
			{
				// Means page is sprite in the spritesheet
				var parts = assetName.Split(':');

				var spriteSheet = assetManager.Load<TextureAtlas>(parts[0]);


				var drawable = spriteSheet.GetDrawable(parts[1]);

				return drawable.TextureRegion;
			}

			return new TextureRegion2D(assetManager.Load<Texture2D>(assetName));
		}
	}
}