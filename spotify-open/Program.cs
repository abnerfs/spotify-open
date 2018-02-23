using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spotify_open
{
    static class Program
    {

        static void Main(string[] args)
        {
            try
            {
                Console.Title = "AbNeR Spotify Open";

                //args = new string[] { "https://open.spotify.com/track/6tHWl8ows5JOZq9Yfaqn3M?si=TunFQdrXS1yr8LxO4O3wgg" };
                //args = new string[] { "https://open.spotify.com/user/22z2r3smyxmv2w6twx2khspey/playlist/1KqhUkMT3cbxHdaLUfYpNh?si=lFqGTmZjSQ-rBMLGY7obWg" };
                //args = new string[] { "https://open.spotify.com/album/4NmYee9ONP9ZiOPJLItMn0?si=S6d0VddqSXiibuea0FCWzQ" };

                if(args.Length == 0)
                {
                    throw new Exception("Informe a URL");
                }

                var URL = args[0].Replace('\\', '/');


                var Splited = URL.Split('/');
                var Tamanho = Splited.Length;

                if (Tamanho < 3)
                {
                    throw new Exception("Invalid URL");
                }

                var Type = Splited[Tamanho - 2];
                var Id = Splited[Tamanho - 1];

                var Command = $"start spotify:{Type}:{Id}";
                Cmd.RunCommandHidden(Command);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                throw;
            }
        }
    }
}
