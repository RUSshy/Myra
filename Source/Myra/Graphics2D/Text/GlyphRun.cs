﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Myra.Graphics2D.Text
{
	public class GlyphRun
	{
		private readonly List<GlyphRender> _glyphRenders = new List<GlyphRender>();
		private string _text;
		private readonly BitmapFont _bitmapFont;
		private bool _dirty = true;
		private Point _size;

		public string Text
		{
			get
			{
				return _text;
			}
		}

		public Point Size
		{
			get
			{
				Update();

				return _size;
			}
		}

		public Rectangle? RenderedBounds { get; private set; }

		public int Count
		{
			get { return _glyphRenders.Count; }
		}

		public GlyphRender this[int index]
		{
			get { return _glyphRenders[index]; }
		}

		public List<GlyphRender> GlyphRenders
		{
			get { return _glyphRenders; }
		}

		public BitmapFont BitmapFont
		{
			get { return _bitmapFont; }
		}

		public GlyphRun(BitmapFont font)
		{
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}

			_bitmapFont = font;
		}

		public void Clear()
		{
			_glyphRenders.Clear();
			_size = Point.Zero;
			_text = string.Empty;
		}

		public void Append(CharInfo ci, Color? color)
		{
			var render = new GlyphRender(ci, this, color);
			_glyphRenders.Add(render);

			_dirty = true;
			_text += ci.Value;
		}

		public void Append(IEnumerable<CharInfo> charInfos, Color? color)
		{
			foreach (var ci in charInfos)
			{
				Append(ci, color);
			}
		}

		public void ResetDraw()
		{
			RenderedBounds = null;
		}

		private void Update()
		{
			if (!_dirty)
			{
				return;
			}

			var glyphs = _bitmapFont.GetGlyphs(_text, Vector2.Zero);
			for (var i = 0; i < glyphs.Length; ++i)
			{
				_glyphRenders[i].Glyph = glyphs[i];
			}

			var sz = _bitmapFont.MeasureString(_text);
			_size = new Point((int)sz.Width, (int)sz.Height);

			_dirty = false;
		}

		public void Draw(SpriteBatch batch, Point pos, int totalWidth, Color color, bool drawPartialLastSymbol = false)
		{
			Update();
			var right = pos.X + totalWidth;
			var x = pos.X;
			foreach (var gr in _glyphRenders)
			{
				if (x > right)
				{
					// Completely out
					break;
				}

				if (x + gr.XAdvance > right && !drawPartialLastSymbol)
				{
					break;
				}

				gr.Draw(batch, new Point(x, pos.Y), color);
				x += gr.XAdvance;
			}
				
			RenderedBounds = new Rectangle(pos.X, pos.Y, x - pos.X, Size.Y);
//			if (BitmapFont.DrawFames)
			{
//				batch.DrawRectangle(RenderedBounds.Value, Color.Blue);
			}
		}
	}
}