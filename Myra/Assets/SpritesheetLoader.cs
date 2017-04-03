using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D;

namespace Myra.Assets
{
	public class SpritesheetLoader : IAssetLoader<SpriteSheet>
	{
		public SpriteSheet Load(AssetManager assetManager, string path)
		{
			string text;
			using (var input = assetManager.Open(path))
			{
				using (var textReader = new StreamReader(input))
				{
					text = textReader.ReadToEnd();
				}
			}

			return SpriteSheet.LoadGDX(text, s => assetManager.Load<Texture2D>(s));
		}
	}
}