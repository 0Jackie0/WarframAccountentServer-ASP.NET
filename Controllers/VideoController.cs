using System;
using System.IO;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Warframeaccountant.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        //private static readonly String WELLCOME_VIDEO_PATH = Path.Combine(AppDomain.CurrentDomain.FriendlyName, "static\\video", "welcome_video.mp4");
        private static readonly String WELLCOME_VIDEO_PATH = "static\\video\\" + "welcome_video.mp4";
        //C:\Users\l1290\Desktop\Work\.net Workspace\Warframeaccountant\Warframeaccountant\static\video\welcome_video.mp4

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public void RangeDownload()
        {
            Console.WriteLine("Get wellcome video range request");

            long size, start, end, length, fp = 0;

            using (StreamReader reader = new StreamReader(new FileInfo(WELLCOME_VIDEO_PATH).FullName))
            {
                size = reader.BaseStream.Length;
                start = 0;
                end = size - 1;
                length = size;

                Response.Headers.Add("Accept-Ranges", "0-" + size);

                if (!String.IsNullOrEmpty(Request.Headers["Range"]))
                {
                    Console.WriteLine("Has range value");

                    long anotherStart = start;
                    long anotherEnd = end;
                    string[] arr_split = Request.Headers["Range"].ToString().Split(new char[] { Convert.ToChar("=") });
                    string range = arr_split[1];

                    // Make sure the client hasn't sent us a multibyte range
                    if (range.IndexOf(",") > -1)
                    {
                        // (?) Shoud this be issued here, or should the first
                        // range be used? Or should the header be ignored and
                        // we output the whole content?
                        Response.Headers.Add("Content-Range", "bytes " + start + "-" + end + "/" + size);
                        /*throw new HttpException(416, "Requested Range Not Satisfiable");*/
                        return;
                    }

                    // If the range starts with an '-' we start from the beginning
                    // If not, we forward the file pointer
                    // And make sure to get the end byte if spesified
                    if (range.StartsWith("-"))
                    {
                        // The n-number of the last bytes is requested
                        anotherStart = size - Convert.ToInt64(range.Substring(1));
                    }
                    else
                    {
                        arr_split = range.Split(new char[] { Convert.ToChar("-") });
                        anotherStart = Convert.ToInt64(arr_split[0]);
                        long temp = 0;
                        anotherEnd = (arr_split.Length > 1 && Int64.TryParse(arr_split[1].ToString(), out temp)) ? Convert.ToInt64(arr_split[1]) : size;
                    }


                    // End bytes can not be larger than $end.
                    anotherEnd = (anotherEnd > end) ? end : anotherEnd;
                    // Validate the requested range and return an error if it's not correct.
                    if (anotherStart > anotherEnd || anotherStart > size - 1 || anotherEnd >= size)
                    {
                        Response.Headers.Add("Content-Range", "bytes " + start + "-" + end + "/" + size);
                        /*throw new HttpException(416, "Requested Range Not Satisfiable");*/
                        return;
                    }
                    start = anotherStart;
                    end = anotherEnd;

                    length = end - start + 1; // Calculate new content length
                    fp = reader.BaseStream.Seek(start, SeekOrigin.Begin);
                    Response.StatusCode = 206;

                    Console.WriteLine($"{start}=--={end}");
                }
            }
            // Notify the client the byte range we'll be outputting
            Response.Headers.Add("Content-Range", "bytes " + start + "-" + end + "/" + size);
            Response.Headers.Add("Content-Length", length.ToString());

            // Start buffered download
            Response.Body.WriteAsync(System.IO.File.ReadAllBytes(new FileInfo(WELLCOME_VIDEO_PATH).FullName), (int)fp, (int)length);
            /*Response.Body.EndWrite();*/
            /*context.Response.end();*/

        }

    }
}
