using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Myra.Graphics2D.Text
{
	public class BitmapFont
	{
		public static bool DrawFames { get; set; }

		private readonly TextureRegion[] _pages;
		private readonly Dictionary<char, Glyph> _glyphs;

		public Dictionary<char, Glyph> Glyphs
		{
			get { return _glyphs; }
		}

		public TextureRegion[] Pages
		{
			get { return _pages; }
		}

		public int LineHeight { get; private set; }

		public BitmapFont(Dictionary<char, Glyph> glyphs, TextureRegion[] pages)
		{
			if (glyphs == null)
			{
				throw new ArgumentNullException("glyphs");
			}

			if (pages == null)
			{
				throw new ArgumentNullException("pages");
			}

			_glyphs = glyphs;
			_pages = pages;
		}

		public static BitmapFont CreateFromFNT(Stream stream, Func<string, TextureRegion> pageResolver)
		{
			var data = new Cyotek.Drawing.BitmapFont.BitmapFont();
			data.LoadText(stream);

			// Resolve pages
			var pageRegions = new TextureRegion[data.Pages.Length];
			for (var i = 0; i < data.Pages.Length; ++i)
			{
				var region = pageResolver(data.Pages[i].FileName);
				if (region == null)
				{
					throw new Exception(string.Format("Unable to resolve page {0}", data.Pages[i].FileName));
				}

				pageRegions[i] = region;
			}

			var glyphs = new Dictionary<char, Glyph>();

			foreach (var pair in data.Characters)
			{
				var character = pair.Value;

				var bounds = character.Bounds;

				var region = new TextureRegion(pageRegions[pair.Value.TexturePage], bounds);
				var glyph = new Glyph
				{
					Id = character.Char,
					Region = region,
					Offset = character.Offset,
					XAdvance = character.XAdvance
				};

				glyphs[glyph.Id] = glyph;
			}

			// Process kernings
			foreach (var pair in data.Kernings)
			{
				var kerning = pair.Key;

				Glyph glyph;
				if (!glyphs.TryGetValue(kerning.FirstCharacter, out glyph))
				{
					continue;
				}

				glyph.Kerning[kerning.SecondCharacter] = kerning.Amount;
			}

			var result = new BitmapFont(glyphs, pageRegions)
			{
				LineHeight = data.LineHeight
			};

			return result;
		}

		public void Draw(SpriteBatch batch, string text, Point pos, Color color)
		{
			for (var i = 0; i < text.Length; ++i)
			{
				var ch = text[i];
				Glyph glyph;
				if (!_glyphs.TryGetValue(ch, out glyph))
				{
					// Glyph not found
					continue;
				}

				var dest = new Rectangle(pos.X + glyph.Offset.X,
					pos.Y + glyph.Offset.Y,
					glyph.Region.Bounds.Width,
					glyph.Region.Bounds.Height);

				glyph.Region.Draw(batch, dest, color);

				pos.X += glyph.XAdvance;

				if (i < text.Length - 1)
				{
					pos.X += glyph.GetKerning(text[i + 1]);
				}
			}
		}

		public Point Measure(string text)
		{
			var result = Point.Zero;

			result.Y = LineHeight;

			if (string.IsNullOrEmpty(text))
			{
				return result;
			}

			var posX = 0;
			for (var i = 0; i < text.Length; ++i)
			{
				var ch = text[i];
				Glyph glyph;
				if (!_glyphs.TryGetValue(ch, out glyph))
				{
					// Glyph not found
					continue;
				}

				posX += glyph.XAdvance;

				if (i < text.Length - 1)
				{
					posX += glyph.GetKerning(text[i + 1]);
				}
			}

			result.X = posX;

			return result;
		}
	}
}