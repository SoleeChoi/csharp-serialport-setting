using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace Settings
{
    public class Startup
    {
        public async Task<object> Invoke(dynamic type)
        {
            String line;

            // Pass the file path and file name to the StreamReader constructor
            string settingsFile = System.IO.Path.GetFullPath("Settings.txt");
            StreamReader sr = new StreamReader(settingsFile);

            // Read the first line of text
            line = sr.ReadLine();

            while (line != null)
            {
                string target = line.Replace(" ", String.Empty); // white space 제거
                String pattern = @type;
                String[] matches = Regex.Split(target, pattern);
                if (matches.Length > 1)
                {
                    sr.Close();
                    return matches[1];
                }

                // Read the next line
                line = sr.ReadLine();
            }

            // close the file
            sr.Close();

            throw new Exception("설정된 " + type + "값을 찾을 수 없습니다.");
        }

        public async Task<object> setPosSettings(dynamic settings)
        {
            String line;
            String type = settings.type;
            String value = settings.value;

            // Pass the file path and file name to the StreamReader constructor
            string settingsFile = Path.GetFullPath("Settings.txt");
            StreamReader sr = new StreamReader(settingsFile);

            FileStream tempFile = createTempSettings();

            // Read the first line of text
            line = sr.ReadLine();

            while (line != null)
            {
                String newLine;
                String pattern = @type;
                String[] matches = Regex.Split(line, pattern);
                if (matches.Length > 1)
                {
                    newLine = type + " " + value + "\r\n";
                }
                else
                {
                    newLine = line + "\r\n";
                }
                Byte[] lineToBeWritten = new UTF8Encoding(true).GetBytes(newLine);
                tempFile.Write(lineToBeWritten, 0, lineToBeWritten.Length);

                // Read the next line
                line = sr.ReadLine();
            }

            // close the file
            sr.Close();
            tempFile.Close();

            renameFile();

            return 0;
        }

        public FileStream createTempSettings()
        {
            string path = Path.GetFullPath("TempSettings.txt");

            // Delete the file if it exists.
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            FileStream fs = File.Create(path);
            return fs;
        }

        public void renameFile()
        {
            string settingsPath = Path.GetFullPath("Settings.txt");

            if (File.Exists(settingsPath))
            {
                File.SetAttributes(settingsPath, FileAttributes.Normal);
                File.Delete(settingsPath);
            }

            File.Move("TempSettings.txt", "Settings.txt");
        }
    }
}
