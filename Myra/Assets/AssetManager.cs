using System;
using System.Collections.Generic;
using System.IO;

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
			RegisterAssetLoader(new DrawableLoader());
			RegisterAssetLoader(new BitmapFontLoader());
			RegisterAssetLoader(new SpritesheetLoader());
			RegisterAssetLoader(new UIStylesheetLoader());
		}

		public void RegisterAssetLoader<T>(IAssetLoader<T> loader)
		{
			_loaders.Add(typeof (T), loader);
		}

		public void ClearCache()
		{
			_cache.Clear();
		}

		/// <summary>
		/// Opens a stream specified by asset path
		/// Throws an exception on failure
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public Stream Open(string path)
		{
			var stream = _assetResolver.Open(path);
			if (stream == null)
			{
				throw new Exception(string.Format("Can't open asset {0}", path));
			}
	
			return stream;
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

			var result = loader.Load(this, name);
			_cache[name] = result;

			return result;
		}
	}
}