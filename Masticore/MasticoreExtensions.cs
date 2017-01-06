using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

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
            using (StreamReader reader = new StreamReader(stream, Encoding.Default))
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
            using (StreamReader reader = new StreamReader(stream, Encoding.Unicode))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Takes a unicode string and converts it to a MemoryStream
        /// </summary>
        /// <param name="src">unicode string</param>
        /// <returns></returns>
        public static MemoryStream ToStream(this string src)
        {
            byte[] byteArray = Encoding.Unicode.GetBytes(src);
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
    }
}
