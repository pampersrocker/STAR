using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Star.Game.Level;
using System.Collections;

namespace Star.Game
{
    public static class FileManager
    {
        public static void WriteFile(string data,string path)
        {
            StreamWriter writer = new StreamWriter(path);
            writer.Write(data);
            writer.Close();
        }

        public static string ReadFile(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            return reader.ReadToEnd();
        }

        public static string ConvertLevelToString(Level.Level level)
        {
            string data = LevelData(level);
            data += level.LevelVariables.ToString();

            return data;
        }

        private static string LevelData(Level.Level level)
        {
            string data;
            Tile[,] tiles = level.Tiles;
            data = "Level=\n";
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    data += ((int)tiles[y, x].TileType).ToString() + ",";
                }
                data += ":\n";
            }
            data += ";\n";
            //string temp = level.LevelVariables.ToString();
            return data;
        }

        public static void WriteInErrorLog(object sender,string message)
        {
            StreamWriter writer = new StreamWriter("Errors.log", true);
            string data = DateTime.Now.ToShortDateString()+ " , "  + DateTime.Now.ToShortTimeString() + "|||Sender: " + sender.ToString() +" |||Error: " +message + "\n";
            writer.Write(data);
            writer.Close();
        }

        public static void WriteInErrorLog(object sender, string message,Type errorType)
        {
            StreamWriter writer = new StreamWriter("Errors.log", true);
            string data = DateTime.Now.ToShortDateString() + " , " + DateTime.Now.ToShortTimeString() + "|||Sender: " + sender.ToString() + " |||Error: " + message + " |||ErrorType: " + errorType.ToString() + "\n" ;
            writer.Write(data);
            writer.Close();
        }

        /// <summary>
        /// Reads out the specified File and returns a Dicionary T,string 
        /// </summary>
        ///<exception cref="System.ArgumentException">Will be thrown when T is not an Enum</exception>
        ///<exception cref="System.IO.FileNotFoundException">Will be thrown if the file is not present.</exception>
        ///<exception cref="System.IO.DirectoryNotFoundException">Will be thrown if the directory is no present.</exception>
        /// <typeparam name="T">Must be an Enum</typeparam>
        /// <param name="filepath">The File which will be read out.</param>
        /// <returns>A Dictionary  containig all valid values in the file of type: "Key=Value;"</returns>
        public static Dictionary<T, string> GetFileDict<T>(string filepath)
			where T : struct, IConvertible, IComparable, IFormattable
        {
            if (!(typeof(T).IsEnum))
            {
                WriteInErrorLog("FileManager", "ArgumentException: T must be an Enum");
                throw new ArgumentException("T must be an Enum!", "T");
            }

                Dictionary<T, string> dict = new Dictionary<T, string>();
                string data;
                string[] lines;

                data = ReadFile(filepath);

                lines = data.Split(';');

                foreach (string line in lines)
                {
                    string[] values;
                    values = line.Split('=');
                    if (values.Length > 1)
                    {
                        string key = values[0].Trim();
                        string value = values[1].Trim();
						try
						{
							dict.Add(
								(T)Enum.Parse(typeof(T), key, true),
								value);
						}
						//Avoids Errors through missing Keys
						catch { }
                    }
                }

                foreach (T type in Enum.GetValues(typeof(T)))
                {
                    if (!dict.Keys.Contains(type))
                    {
                        dict.Add(type, string.Empty);
                    }
                }
                return dict;
        }

		public static Dictionary<string, string> GetFileDictString(string fileName)
		{
			Dictionary<string, string> fileDict = new Dictionary<string,string>();
			string data;
			string[] lines;

			data = ReadFile(fileName);
			lines = data.Split(';');

			foreach (string line in lines)
			{
				try
				{
					string[] values = line.Split('=');
					if (values.Length >= 2)
					{
						string key = values[0].Trim();
						string value = values[1].Trim();

						fileDict.Add(key, value);
					}
				}
				catch
				{
				}
			}

			return fileDict;
		}

        /// <summary>
        /// Writes each entry of a Dictionary in a File with the Syntax: "Key=Value;\n"
        /// </summary>
        /// <typeparam name="T">Must be an Enum</typeparam>
        /// <exception cref="System.ArgumentException">Will be thrown when T is not an Enum</exception>
        /// <exception cref="StreamWriterExceptions">All StreamWriter Exceptions</exception>
        /// <param name="dict">The Dictionary which should be written</param>
        /// <param name="filename">The File which should be created</param>
        public static void EnumDictToFile<T>(Dictionary<T, string> dict, string filename)
			where T : struct, IConvertible, IComparable, IFormattable
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an Enum!");
            }
            string data="";
            foreach (T key in Enum.GetValues(typeof(T)))
            {
                try
                {
                    data += key.ToString() + "=" + dict[key] + ";\n";
                }
                //Avoids Errors through missing Keys    
                catch { }
            }
            WriteFile(data, filename);
        }
    }
}
