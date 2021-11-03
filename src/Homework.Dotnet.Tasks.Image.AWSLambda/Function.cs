using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Net;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Homework.Dotnet.Tasks.Image.AWSLambda
{
    public class FunctionInput
    {
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Key3 { get; set; }
    }

    public class Function
    {
        private static IAmazonS3 _s3Client;

        public static void SetupAmazonS3Client()
        {
            _s3Client = new AmazonS3Client();
        }

        /// <summary>
        /// A simple function that takes a string and returns both the upper and lower case version of the string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(FunctionInput input, ILambdaContext context)
        {
            SetupAmazonS3Client();
            ConsoleWriteLine("Hello World from AWS Lambda!");

            if (input != null)
            {
                ConsoleWriteLine($"Input: {input?.Key1} {input?.Key2} {input?.Key3}");
            }

            await ListAllBucketsAndDataAsync();
        }

        private async Task ListAllBucketsAndDataAsync()
        {
            await ExecuteSafeAsync(async () =>
            {
                var listBucketResponse = await _s3Client.ListBucketsAsync();
                if (listBucketResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    foreach (var bucket in listBucketResponse.Buckets)
                    {
                        ConsoleWriteLine($"Bucket: {bucket.BucketName}, created: {bucket.CreationDate}");

                        var listObjectResponse = await _s3Client.ListObjectsAsync(bucket.BucketName);
                        if (listObjectResponse.HttpStatusCode == HttpStatusCode.OK)
                        {
                            foreach (var item in listObjectResponse.S3Objects)
                            {
                                ConsoleWriteLine($"Key: {item.Key}, " +
                                    $"Size: {item.Size}, " +
                                    $"LastModified: {item.LastModified}, " +
                                    $"ETag: {item.ETag}, " +
                                    $"Owner: {item.Owner.DisplayName}");

                                //await UploadFileAsync(bucket.BucketName, "", "", item.Key);
                            }
                        }
                        else
                        {
                            ConsoleWriteLine($"Could not list of objects from AWS S3 bucket: {bucket.BucketName}!");
                        }
                    }
                }
                else
                {
                    ConsoleWriteLine("Could not list buckets from AWS S3!");
                }

                return Task.CompletedTask;
            });
        }

        /// <summary>
        /// Writes file to s3 bucket using specified contents, content type
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="contents"></param>
        /// <param name="contentType"></param>
        /// <param name="fileName"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        private async Task<bool> UploadFileAsync(string bucketName,
            string contents,
            string contentType,
            string fileName,
            string directory = null)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var bucketPath = !string.IsNullOrWhiteSpace(directory) ? bucketName + @"/" + directory : bucketName;

                //System.IO.Directory.CreateDirectory("output");
                //using (Image image = Image.Load("fb.jpg"))
                //{
                //    image.Mutate(x => x
                //         .Resize(image.Width / 2, image.Height / 2)
                //         .Grayscale());

                //    image.Save("output/fb.png"); // Automatic encoder selected based on extension.
                //}

                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketPath,
                    Key = fileName,
                    ContentBody = contents,
                    ContentType = contentType,
                    CannedACL = S3CannedACL.PublicRead
                };
                var response = await _s3Client.PutObjectAsync(putRequest);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    ConsoleWriteLine($"successfully uploaded {fileName} to {bucketPath} on {DateTime.UtcNow:O}");
                    return true;
                }

                ConsoleWriteLine($"failed to upload {fileName} to {bucketPath} on {DateTime.UtcNow:O}");
                return false;
            });
        }

        private async Task<T> ExecuteSafeAsync<T>(Func<Task<T>> func)
        {
            try
            {
                return await func();
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    ConsoleWriteLine("Please check the provided AWS Credentials.");
                }
                else
                {
                    ConsoleWriteLine($"An error occurred with the message '{amazonS3Exception.Message}'");
                }

                return default;
            }
        }

        private void ConsoleWriteLine(string message)
        {
            Console.Out.WriteLine(message);
        }
    }
}
