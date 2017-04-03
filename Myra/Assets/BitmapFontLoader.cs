using System.IO;
using Myra.Graphics2D;
using Myra.Graphics2D.Text;

namespace Myra.Assets
{
	public class BitmapFontLoader : IAssetLoader<BitmapFont>
	{
		public BitmapFont Load(AssetManager assetManager, string path)
		{
			string text;
			using (var input = assetManager.Open(path))
			{
				using (var textReader = new StreamReader(input))
				{
					text = textReader.ReadToEnd();
				}
			}

			var result = BitmapFont.LoadFromFnt(text, s => assetManager.Load<TextureRegion>(s));

			return result;
		}
	}
}