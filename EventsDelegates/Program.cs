using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsDelegates
{
    class Program
    {
        static void Main(string[] args)
        {
            var video = new Video() { Title = "Game of thrown" };
            var encoder = new VideoEncoder();

            var mailService = new MailService();
            encoder.VideoEncoded += mailService.OnVideoEncoded;

            var massageService = new MassageService();
            encoder.VideoEncoded += massageService.OnVideoEncoded;

            encoder.Encode(video);

            Console.ReadLine();
        }
    }

    public class MailService
    {
        public void OnVideoEncoded(object source, VideoEventArgs args)
        {
            Console.WriteLine("Mail service: sending the mail" + args.Video.Title);
        }
    }

    public class MassageService
    {
        public void OnVideoEncoded(object source, VideoEventArgs args)
        {
            Console.WriteLine("Massage service: sending the massage" + args.Video.Title);
        }
    }
}
