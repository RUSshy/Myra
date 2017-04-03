using System.IO;
using Microsoft.Xna.Framework.Graphics;
using StbSharp;

namespace Myra.Assets
{
	internal class Texture2DLoader : IAssetLoader<Texture2D>
	{
		public Texture2D Load(AssetManager assetManager, Stream stream)
		{
			var reader = new ImageReader();

			var image = reader.Read(stream);

			var texture = new Texture2D(MyraEnvironment.GraphicsDevice, image.Width, image.Height, false, SurfaceFormat.Color);
			texture.SetData(image.Data);

			return texture;
		}
	}
}