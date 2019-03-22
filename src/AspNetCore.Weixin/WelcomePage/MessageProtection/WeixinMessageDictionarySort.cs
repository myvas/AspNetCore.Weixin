using System.Collections;

namespace AspNetCore.Weixin.DataProtection
{
	public class WeixinMessageDictionarySort : IComparer
	{
		public int Compare(object x, object y)
		{
			string sx = x as string;
			string sy = y as string;
			int lensx = sx.Length;
			int lensy = sy.Length;
			int index = 0;
			while (index < lensx && index < lensy)
			{
				if (sx[index] < sy[index])
					return -1;
				else if (sx[index] > sy[index])
					return 1;
				else
					index++;
			}
			return lensx - lensy;
		}
	}
}
