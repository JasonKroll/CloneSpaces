using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;

#nullable enable
namespace CloneSpaces
{
  class Program
  {
    private static Parameters Params = new Parameters();

    static async Task<int> Main(string[] args)
    {
      var rootCommand = new RootCommand
                  {
                      new Argument<string>(
                          "SourceBucket",
                          description: "The name of the source bucket"),
                      new Argument<string>(
                          "DestinationBucket",
                          description: "The name of the destination bucket"),
                      new Option<string>(
                          "--endpoint-url",
                          description: "The name of the destination bucket"),
                      new Option<bool>(
                          "--s3",
                          description: "Use Amazon S3.")
                  };

      rootCommand.Description = "Clone Amazon S3 or DigitalOcean Spaces";

      rootCommand.Handler = CommandHandler.Create<string, string, string?, bool?>(GetParams);
      await rootCommand.InvokeAsync(args);
      var config = new AmazonS3Config
      {
        ServiceURL = Params.EndpointURL
      };
      // Create an S3 client object.
      var s3Client = new AmazonS3Client(Params.Key, Params.Secret, config);

      var count = 0;

      var keys = await GetKeysAsync(s3Client, Params.SourceBucket);
      var start = DateTime.Now;
      foreach (string key in keys)
      {
        Console.WriteLine(key);
        await CopyingObjectAsync(s3Client, Params.SourceBucket, key, Params.DestinationBucket);
        count += 1;
      }
      var stop = DateTime.Now;
      var time = stop - start;
      Console.WriteLine($"\n----------------------");
      Console.WriteLine($"\n{count} objects copied in {time.Seconds} seconds");
      Console.WriteLine($"\n----------------------");

      return 0;
    }

    static void GetParams(string sourceBucket, string destinationBucket, string? endpointUrl, bool? s3)
    {
      var useS3 = s3 ??= false;
      var keyEnv = useS3 ? "AWS_ACCESS_KEY_ID" : "SPACES_KEY";
      var secretEnv = useS3 ? "AWS_ACCESS_KEY_ID" : "SPACES_SECRET";
      var key = Environment.GetEnvironmentVariable(keyEnv);
      var secret = Environment.GetEnvironmentVariable(secretEnv);
      var endPoint = endpointUrl ?? "https://sfo2.digitaloceanspaces.com";

      Params = new Parameters
      {
        Key = key,
        Secret = secret,
        SourceBucket = sourceBucket,
        DestinationBucket = destinationBucket,
        EndpointURL = endPoint
      };
    }

    static async Task<List<string>> GetKeysAsync(IAmazonS3 client, string bucketName)
    {
      var keys = new List<string>();
      try
      {
        ListObjectsV2Request request = new ListObjectsV2Request
        {
          BucketName = bucketName
        };
        ListObjectsV2Response response;
        do
        {
          response = await client.ListObjectsV2Async(request);
          keys.AddRange(response.S3Objects.Select(e => e.Key).ToList());
          request.ContinuationToken = response.NextContinuationToken;
        } while (response.IsTruncated);
      }
      catch (AmazonS3Exception amazonS3Exception)
      {
        Console.WriteLine("S3 error occurred. Exception: " + amazonS3Exception.ToString());
        Console.ReadKey();
      }
      catch (Exception e)
      {
        Console.WriteLine("Exception: " + e.ToString());
        Console.ReadKey();
      }
      return keys;
    }

    static async Task CopyingObjectAsync(IAmazonS3 s3Client, string sourceBucket, string objectKey, string destinationBucket)
    {
      try
      {
        CopyObjectRequest request = new CopyObjectRequest
        {
          SourceBucket = sourceBucket,
          SourceKey = objectKey,
          DestinationBucket = destinationBucket,
          DestinationKey = objectKey,
          CannedACL = S3CannedACL.PublicRead
        };
        CopyObjectResponse response = await s3Client.CopyObjectAsync(request);
      }
      catch (AmazonS3Exception e)
      {
        Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
      }
      catch (Exception e)
      {
        Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
      }
    }

  }
}
#nullable disable
