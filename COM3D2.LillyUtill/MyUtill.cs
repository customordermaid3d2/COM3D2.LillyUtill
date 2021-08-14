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

		public static T[] ShuffleArray<T>(T[] array, int seed)
		{
			System.Random prng = new System.Random(seed);

			Shuffle(array, prng);

			return array;
		}

		public static void ShuffleArray<T>(ref T[] array, int seed)
		{
			System.Random prng = new System.Random(seed);

			Shuffle(array, prng);

			//return array;
		}

		public static T[] ShuffleArray<T>(T[] array)
		{
			System.Random prng = new System.Random();

			Shuffle(array, prng);

			return array;
		}

		public static void ShuffleArray<T>(ref T[] array)
		{
			System.Random prng = new System.Random();

			Shuffle(array, prng);

			//return array;
		}

		private static void Shuffle<T>(T[] array, Random prng)
		{
			int randomIndex;
			for (int i = 0; i < array.Length - 1; i++)
			{
				randomIndex = prng.Next(i, array.Length);
				T tempItem = array[randomIndex];
				array[randomIndex] = array[i];
				array[i] = tempItem;
			}
		}


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


		/// <summary>
		/// 버전 정보를 넣으면 빌드 시간을 반환. 
		/// 출처 : https://jsmun.com/50 
		/// </summary>
		/// <param name="version">System.Reflection.Assembly.GetExecutingAssembly().GetName().Version</param>
		/// <returns></returns>
		public static System.DateTime Get_BuildDateTime(System.Version version = null)
		{
			// 주.부.빌드.수정
			// 주 버전    Major Number
			// 부 버전    Minor Number
			// 빌드 번호  Build Number
			// 수정 버전  Revision NUmber

			//매개 변수가 존재할 경우
			if (version == null)
				version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

			/*
			//세번째 값(Build Number)은 2000년 1월 1일부터
			//Build된 날짜까지의 총 일(Days) 수 이다.
			int day = version.Build;
			System.DateTime dtBuild = (new System.DateTime(2000, 1, 1)).AddDays(day);

			//네번째 값(Revision NUmber)은 자정으로부터 Build된
			//시간까지의 지나간 초(Second) 값 이다.
			int intSeconds = version.Revision;
			intSeconds = intSeconds * 2;
			dtBuild = dtBuild.AddSeconds(intSeconds);
			*/

			System.DateTime dtBuild=new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);

			//시차 보정
			//System.Globalization.DaylightTime daylingTime = System.TimeZone.CurrentTimeZone
			//		.GetDaylightChanges(dtBuild.Year);
			//if (System.TimeZone.IsDaylightSavingTime(dtBuild, daylingTime))
			//	dtBuild = dtBuild.Add(daylingTime.Delta);
			//
			return dtBuild;
		}

		/// <summary>
		/// Get_BuildDateTime(version).ToString("u");
		/// </summary>
		/// <param name="version">System.Reflection.Assembly.GetExecutingAssembly().GetName().Version</param>
		/// <returns></returns>
		public static string GetBuildDateTime(System.Version version = null)
        {
			return Get_BuildDateTime(version).ToString("u");
		}
	}
}
