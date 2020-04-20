using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Masticore
{
    /// <summary>
    /// Class for gathering all miscellaneous extensions and methods
    /// </summary>
    public static class MasticoreExtensions
    {
        /// <summary>
        /// Generates a GUID string
        /// </summary>
        /// <returns></returns>
        public static string GenerateGuidString()
        {
            return Guid.NewGuid().ToString("N").ToLower();
        }

        /// <summary>
        /// Generates a new GUID string value for the external ID of the given object
        /// </summary>
        /// <param name="obj"></param>
        public static void GenerateUniversalId(this IUniversal obj)
        {
            obj.UniversalId = GenerateGuidString();
        }

        /// <summary>
        /// Sets the CreatedUtc  to the current UTC time
        /// </summary>
        /// <param name="obj"></param>
        public static void SetCreatedUtc(this IAuditable obj)
        {
            obj.CreatedUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Sets the ModifiedUtc to the current UTC time
        /// </summary>
        /// <param name="obj"></param>
        public static void SetModifiedUtc(this IAuditable obj)
        {
            obj.UpdatedUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Returns true if the given object has been modified; otherwise, returns false
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool HasBeenModified(this IAuditable obj)
        {
            return obj.UpdatedUtc != null;
        }

        /// <summary>
        /// Marks the given object as soft-deleted
        /// </summary>
        /// <param name="obj"></param>
        public static void SoftDelete(this ISoftDeletable obj)
        {
            obj.DeletedUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Returns true if the given object has been soft-deleted; otherwise, returns false
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool HasBeenSoftDeleted(this ISoftDeletable obj)
        {
            return obj.DeletedUtc != null;
        }

        /// <summary>
        /// Marks the given object as no longer soft-deleted
        /// </summary>
        /// <param name="obj"></param>
        public static void SoftRestore(this ISoftDeletable obj)
        {
            obj.DeletedUtc = null;
        }

        /// <summary>
        /// converts a stream to a unicode string
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ToDefaultString(this Stream stream)
        {
            stream.Position = 0;
            using (var reader = new StreamReader(stream, Encoding.Default))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// converts a stream to a unicode string
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ToUnicodeString(this Stream stream)
        {
            stream.Position = 0;
            using (var reader = new StreamReader(stream, Encoding.Unicode))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// converts a stream to a unicode string
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Task<string> ToUnicodeStringAsync(this Stream stream)
        {
            stream.Position = 0;
            using (var reader = new StreamReader(stream, Encoding.Unicode))
            {
                return reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Takes a unicode string and converts it to a MemoryStream
        /// </summary>
        /// <param name="src">unicode string</param>
        /// <returns></returns>
        public static MemoryStream ToStream(this string src)
        {
            var byteArray = Encoding.Unicode.GetBytes(src);
            return new MemoryStream(byteArray);
        }

        /// <summary>
        /// Converts the given string to the given plain text response in unicode
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static HttpResponseMessage ToPlainTextResponse(this string text)
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(text, Encoding.Unicode, "text/plain");
            return resp;
        }

        /// <summary>
        /// Combine Uri paths like System.IO Path.Combine
        /// </summary>
        /// <param name="uri">The base Uri</param>
        /// <param name="paths">The paths to append in</param>
        /// <returns>A combined Uri</returns>
        public static Uri AppendPath(this Uri uri, params string[] paths)
        {
           return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/'))));
        }

        /// <summary>
        /// Appends query string parameters
        /// </summary>
        /// <param name="uri">The base Uri</param>
        /// <param name="queries">The query strings</param>
        /// <returns></returns>
        public static Uri AppendQuery(this Uri uri, params string[] queries)
        {
   
            return new Uri(uri.AbsoluteUri + "?" + string.Join("&", queries));
        }

        /// <summary>
        /// Convert a JSON string into a UTF8 encoded media type 
        /// </summary>
        /// <param name="str">The JSON-ready string</param>
        /// <returns></returns>
        public static StringContent ToJSONContent(this string str)
        {
            return new StringContent(str, Encoding.UTF8, "application/json");
        }

    }
}
