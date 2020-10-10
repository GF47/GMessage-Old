using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace GenerateUniqueID
{
    class Program
    {
        private static Regex _idNameRegex = new Regex("^A-Z_");

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Args missing, please input at least 2 args");
                return;
            }

            var filePath = args[0];
            var name = _idNameRegex.Replace(args[1], string.Empty);
            var comment = args.Length > 2 ? args[2] : string.Empty;
            var messageType = args.Length > 3 ? args[3] : string.Empty;

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Target file doesn't exist");
                return;
            }

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    long indicator = 0;
                    for (int i = 1; i <= fs.Length; i++)
                    {
                        fs.Seek(-i, SeekOrigin.End);
                        char c = Convert.ToChar(fs.ReadByte());
                        if (c.Equals('}')) { indicator = fs.Position - 1; }
                        if (c.Equals('{')) { break; }
                    }


                    fs.Position = indicator;

                    while (fs.Position > -1)
                    {
                        char c = Convert.ToChar(fs.ReadByte());
                        if (c.Equals('\n'))
                        {
                            indicator = fs.Position;
                            break;
                        }
                        fs.Seek(-2, SeekOrigin.Current);
                    }

                    byte[] end = new byte[fs.Length - indicator];
                    fs.Read(end, 0, end.Length);

                    fs.Position = indicator;

                    string s = string.Format(
                        "\n\n" +
                        "       /// <summary>\n" +
                        "       /// {0}\n" +
                        "       /// <para>message type: {1}\n" +
                        "       /// <summary>\n" +
                        "       [System.ComponentModel.Description(\"{0}, message: {1}\")]\n" +
                        "       public const int {1} = {2};\n",
                        comment, messageType, name, (int)DateTime.UtcNow.ToBinary());

                    byte[] sb = Encoding.UTF8.GetBytes(s);
                    byte[] b = new byte[sb.Length + end.Length];
                    sb.CopyTo(b, 0);
                    end.CopyTo(b, sb.Length);

                    fs.Write(b, 0, b.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

