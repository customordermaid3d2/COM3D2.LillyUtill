using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace COM3D2.LillyUtill
{
	public class MyUtill
	{
		public static string Join<T>(string separator, IEnumerable<T> values)
		{
			if (values == null)
			{
				return "";
			}
			if (separator == null)
			{
				separator = string.Empty;
			}
			string result;
			using (IEnumerator<T> enumerator = values.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					result = string.Empty;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (enumerator.Current != null)
					{
						T t = enumerator.Current;
						string text = t.ToString();
						if (text != null)
						{
							stringBuilder.Append(text);
						}
					}
					while (enumerator.MoveNext())
					{
						stringBuilder.Append(separator);
						if (enumerator.Current != null)
						{
							T t = enumerator.Current;
							string text2 = t.ToString();
							if (text2 != null)
							{
								stringBuilder.Append(text2);
							}
						}
					}
					result = stringBuilder.ToString();
				}
			}
			return result;
		}


		public static byte[] ExtractResource(Bitmap image)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				image.Save(ms, ImageFormat.Png);
				return ms.ToArray();
			}
		}

		public static T RandomEnum<T>(params T[] args)
		{
			//Array values = Enum.GetValues(typeof(T));
			List<T> lst = ((T[])Enum.GetValues(typeof(T))).ToList();
			for (int i = 0; i < args.Length; i++)
			{
				lst.Remove(args[i]);
			}
			return lst[UnityEngine.Random.Range(0, lst.Count)];
			//return lst[new Random().Next(0, lst.Count)];
			//return (T)values.GetValue(new Random().Next(0, values.Length));
		}

	}
}
