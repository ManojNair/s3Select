using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.XRay.Recorder.Core.Internal.Utils;

namespace ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new AmazonS3Client(RegionEndpoint.APSoutheast2);
            var response = await client.SelectObjectContentAsync(new SelectObjectContentRequest()
            {
                Bucket = "iagarchonaws",
                Key = "alpha.json",
                Expression = $"select * from s3object s where s.company = 'AMAZON'",
                ExpressionType = ExpressionType.SQL,
                InputSerialization = new InputSerialization()
                    {JSON = new JSONInput(){JsonType = JsonType.Lines}},
                OutputSerialization = new OutputSerialization(){JSON = new JSONOutput()}
            });

           foreach (var s3Event in response.Payload)
           {
               if (s3Event is RecordsEvent r)
               {
                   using var sr = new StreamReader(r.Payload);
                   Console.WriteLine(sr.ReadToEnd());
               }
           }
            
           
        }
    }
}