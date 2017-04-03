using System;
using System.Collections.Generic;

namespace Myra.Assets
{
	public class AssetManager
	{
		private readonly Dictionary<Type, object> _loaders = new Dictionary<Type, object>();
		private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
		private IAssetResolver _assetResolver;

		public IAssetResolver AssetResolver
		{
			get { return _assetResolver; }

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				_assetResolver = value;
			}
		}

		public AssetManager(IAssetResolver assetResolver)
		{
			AssetResolver = assetResolver;
			RegisterDefaultLoaders();
		}

		private void RegisterDefaultLoaders()
		{
			RegisterAssetLoader(new Texture2DLoader());
			RegisterAssetLoader(new BitmapFontLoader());
			RegisterAssetLoader(new SpritesheetLoader());
			RegisterAssetLoader(new UIStylesheetLoader());
		}

		public void RegisterAssetLoader<T>(IAssetLoader<T> loader)
		{
			_loaders.Add(typeof (T), loader);
		}

		public T Load<T>(string name, object parameters = null)
		{
			object cached;
			if (_cache.TryGetValue(name, out cached))
			{
				return (T) cached;
			}

			var type = typeof (T);
			object loaderBase;
			if (!_loaders.TryGetValue(typeof (T), out loaderBase))
			{
				throw new Exception(string.Format("Unable to resolve AssetLoader for type {0}", type.Name));
			}

			var loader = (IAssetLoader<T>) loaderBase;

			var stream = _assetResolver.Open(name);
			if (stream == null)
			{
				throw new Exception(string.Format("Can't open asset {0}", name));
			}

			try
			{
				var result = loader.Load(this, stream);
				_cache[name] = result;

				return result;
			}
			finally
			{
				stream.Dispose();
			}
		}
	}
}