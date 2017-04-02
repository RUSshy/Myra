using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D;

namespace Myra.Assets
{
	public class SpritesheetLoader : IAssetLoader<SpriteSheet>
	{
		public SpriteSheet Load(AssetManager assetManager, Stream input, object parameters)
		{
			var result = SpriteSheet.LoadGDX(input, s => assetManager.Load<Texture2D>(s));

			return result;
		}
	}
}