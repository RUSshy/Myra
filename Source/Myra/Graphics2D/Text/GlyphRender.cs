using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;

namespace Myra.Graphics2D.Text
{
	public class GlyphRender
	{
		public BitmapFont Font { get; private set; }
		public CharInfo CharInfo { get; private set; }
		public GlyphRun Run { get; private set; }
		public BitmapFontRegion Glyph { get; private set; }
		public Color? Color { get; private set; }
		public Point Location { get; private set; }
		public Rectangle? RenderedBounds { get; private set; }

		public int XAdvance
		{
			get { return Glyph != null ? Glyph.XAdvance : 0; }
		}

		public GlyphRender(BitmapFont font, CharInfo charInfo, GlyphRun run, BitmapFontRegion glyph, Color? color, Point location)
		{
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}

			if (run == null)
			{
				throw new ArgumentNullException("run");
			}

			Font = font;
			CharInfo = charInfo;
			Run = run;
			Glyph = glyph;
			Color = color;
			Location = location;
		}

		public void ResetDraw()
		{
			RenderedBounds = null;
		}

		public void Draw(SpriteBatch batch, Point pos, Color color)
		{
			if (Glyph == null)
			{
				return;
			}

			var v = new Vector2(pos.X + Location.X + Glyph.XOffset, pos.Y + Location.Y + Glyph.YOffset);

			var glyphColor = Color ?? color;

			batch.Draw(Glyph.TextureRegion, v, glyphColor);

			var bounds = new Rectangle((int)v.X, (int)v.Y, Glyph.Width, Glyph.Height);
			if (bounds.Width == 0 || bounds.Height == 0)
			{
				bounds.X = Location.X;
				bounds.Y = Location.Y;
				bounds.Width = XAdvance;
				bounds.Height = Font.LineHeight;
				bounds.Offset(pos);
			}

			RenderedBounds = bounds;
		}
	}
}
