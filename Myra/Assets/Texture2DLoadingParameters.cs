using System;

namespace Myra.Assets
{
	public class Texture2DLoadingParameters
	{
		private int? _requiredComponents;

		public int? RequiredComponents
		{
			get
			{
				return _requiredComponents;
			}

			set
			{
				if (value.HasValue && (value.Value < 1 || value.Value > 4))
				{
					throw new ArgumentOutOfRangeException("value");
				}

				_requiredComponents = value;
			}
		}

		public bool PremultiplyAlpha;
	}
}
