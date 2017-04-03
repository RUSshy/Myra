using System.IO;

namespace Myra.Assets
{
	public interface IAssetLoader<out T>
	{
		T Load(AssetManager assetManager, Stream input);
	}
}
