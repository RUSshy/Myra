using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Myra.Graphics2D
{
	public interface Drawable
	{
		Point Size { get; }
		TextureRegion2D TextureRegion { get; }

		void Draw(SpriteBatch batch, Rectangle dest);
	}
}
