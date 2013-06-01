using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Star.Game;

namespace Star.GameManagement
{
	#region DataHolder
	public class DataHolder
	{
		SpecificDataHolder<string> stringData;
		SpecificDataHolder<double> doubleData;
		SpecificDataHolder<int> intData;
		SpecificDataHolder<float> floatData;
		SpecificDataHolder<bool> boolData;

		public DataHolder()
		{
			stringData = new SpecificDataHolder<string>();
			doubleData = new SpecificDataHolder<double>();
			intData = new SpecificDataHolder<int>();
			floatData = new SpecificDataHolder<float>();
			boolData = new SpecificDataHolder<bool>();
		}

		public bool PutData<TValue>(string key, TValue value)
		{
			bool success = false;

			if (value is string)
			{
				success = stringData.PutData(key, (string)Convert.ChangeType(value, typeof(string)));
			}
			else if (value is int)
			{
				success = intData.PutData(key, (int)Convert.ChangeType(value, typeof(int)));

			}
			else if (value is double)
			{
				success = doubleData.PutData(key, (double)Convert.ChangeType(value, typeof(double)));

			}
			else if (value is float)
			{
				success = floatData.PutData(key, (float)Convert.ChangeType(value, typeof(float)));

			}
			else if (value is bool)
			{
				success = boolData.PutData(key, (bool)Convert.ChangeType(value, typeof(bool)));

			}
			else
			{
				FileManager.WriteInErrorLog(this, "Type of TValue does not match any storableData", typeof(ArgumentException));
				throw new ArgumentException("Type of TValue does not match any storableData", "TValue");
			}

			return success;

		}

		public TValue GetData<TValue>(string key)
		{
			if (typeof(TValue) == typeof(string))
			{
				return (TValue)Convert.ChangeType(stringData.GetData(key), typeof(TValue));
			}
			else if (typeof(TValue) == typeof(int))
			{
				return (TValue)Convert.ChangeType(intData.GetData(key), typeof(TValue));

			}
			else if (typeof(TValue) == typeof(double))
			{
				return (TValue)Convert.ChangeType(doubleData.GetData(key), typeof(TValue));

			}
			else if (typeof(TValue) == typeof(float))
			{
				return (TValue)Convert.ChangeType(floatData.GetData(key), typeof(TValue));

			}
			else if (typeof(TValue) == typeof(bool))
			{
				return (TValue)Convert.ChangeType(boolData.GetData(key), typeof(TValue));

			}
			else
			{
				FileManager.WriteInErrorLog(this, "Type of TValue does not match any storableData", typeof(ArgumentException));
				throw new ArgumentException("Type of TValue does not match any storableData", "TValue");
			}
		}


	}

	class SpecificDataHolder<TValue>
	{
		Dictionary<string, TValue> dictionary;

		public SpecificDataHolder()
		{
			dictionary = new Dictionary<string, TValue>();
		}

		public bool PutData(string key, TValue value)
		{
			bool success = false;
			try
			{
				if (dictionary.ContainsKey(key))
				{
					dictionary[key] = value;
				}
				else
				{
					dictionary.Add(key, value);
				}
				success = true;
			}
			catch (Exception e)
			{
				FileManager.WriteInErrorLog(this, e.Message, e.GetType());
			}
			return success;
		}

		public TValue GetData(string key)
		{
			return dictionary[key];
		}

		public void Clear()
		{
			dictionary.Clear();
		}
	}

	public static class EnumExtenstion
	{
		public static string GetKey<T>(this T type)
			where T : struct, IConvertible, IComparable, IFormattable
		{
			return type.GetType().ToString() + "." + type.ToString();
		}
	}
	#endregion
}
