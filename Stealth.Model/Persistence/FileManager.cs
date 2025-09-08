using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stealth.Model.Utils;

namespace Stealth.Persistence
{
    public class FileManager
    {
        private readonly string _path;
        public FileManager(string path)
        {
            _path = path;
        }
        public StealthMap Load()
        {
            if (_path == "")
                throw new FileManagerException();
            return Make(_path);
        }

        private StealthMap Make(string path)
        {
            string[] lines;
            int size;
            List<Coordinate> coor = new List<Coordinate>();
            try
            {
                lines = File.ReadAllLines(path);
                size = CheckFile(lines);
                int lineCount = lines[0].Split().Length;
                for (int i = lineCount; i < lines.Length; ++i)
                {
                    string[] line = lines[i].Split();
                    coor.Add(new Coordinate(int.Parse(line[0]), int.Parse(line[1])));
                }
            }
            catch (Exception)
            {
                throw new FileManagerException("bad format");
            }
            return new StealthMap(size, lines, coor);
        }

        private int CheckFile(string[] lines)//return with the size of map, if the map is illegal, returns with -1
        {
            int lineCount;
            string[] line;
            if (lines.Length == 0)
                return -1;
            lineCount = lines[0].Split().Length;
            for (int i = 0; i < lineCount; ++i)
            {
                line = lines[i].Split();
                int charCount = line.Length;
                if (lineCount == charCount)
                {
                    for (int j = 0; j < charCount; ++j)
                    {
                        if (line[i] == "#" || line[i] == "_" || line[i] == "E")
                            continue;
                        else
                            return -1;
                    }
                }
                else
                    return -1;

            }
            return lineCount;
        }
    }
}
