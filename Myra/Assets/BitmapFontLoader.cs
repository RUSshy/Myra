using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D;
using Myra.Graphics2D.Text;

namespace Myra.Assets
{
	public class BitmapFontLoader : IAssetLoader<BitmapFont>
	{
		public BitmapFont Load(AssetManager assetManager, Stream input)
		{
			string text;
			using (var textReader = new StreamReader(input))
			{
				text = textReader.ReadToEnd();
			}

			var result = BitmapFont.LoadFromFnt(text, fn =>
			{
				if (fn.Contains(":"))
				{
					// Means page is sprite in the spritesheet
					var parts = fn.Split(':');

					var spriteSheet = assetManager.Load<SpriteSheet>(parts[0]);
					var drawable = spriteSheet.EnsureDrawable(parts[1]);

					return drawable.TextureRegion;
				}
				return new TextureRegion(assetManager.Load<Texture2D>(fn));
			});

			return result;
		}
	}
}