﻿using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StbSharp;

namespace Myra.Assets
{
	internal class Texture2DLoader : IAssetLoader<Texture2D>
	{
		internal class Texture2DLoadingParameters
		{
			public string image { get; set; }
			public bool premultiplyAlpha { get; set; }
		}

		private static byte ApplyAlpha(byte color, byte alpha)
		{
			var fc = color/255.0f;
			var fa = alpha/255.0f;

			var fr = (int) (255.0f*fc*fa);

			if (fr < 0)
			{
				fr = 0;
			}

			if (fr > 255)
			{
				fr = 255;
			}

			return (byte) fr;
		}

		public Texture2D Load(AssetManager assetManager, string path)
		{
			var premultiplyAlpha = false;

			if (path.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
			{
				string text;
				using (var input = assetManager.Open(path))
				{
					using (var textReader = new StreamReader(input))
					{
						text = textReader.ReadToEnd();
					}
				}

				var parameters = JsonConvert.DeserializeObject<Texture2DLoadingParameters>(text);

				path = parameters.image;
				premultiplyAlpha = parameters.premultiplyAlpha;
			}

			Image image;
			var reader = new ImageReader();

			using (var stream = assetManager.Open(path))
			{
				image = reader.Read(stream, Stb.STBI_rgb_alpha);
			}

			var data = image.Data;
			if (premultiplyAlpha)
			{
				// Premultiply alpha
				for (var i = 0; i < image.Width*image.Height; ++i)
				{
					var a = data[i*4 + 3];

					data[i*4] = ApplyAlpha(data[i*4], a);
					data[i*4 + 1] = ApplyAlpha(data[i*4 + 1], a);
					data[i*4 + 2] = ApplyAlpha(data[i*4 + 2], a);
				}
			}

			var texture = new Texture2D(MyraEnvironment.GraphicsDevice, image.Width, image.Height, false, SurfaceFormat.Color);
			texture.SetData(image.Data);

			return texture;
		}
	}
}