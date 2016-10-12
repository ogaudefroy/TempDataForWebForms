namespace TempDataForWebForms.TestUtils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// Set of extension methods on IDictionary.
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Deserializes an base 64 encoded string in a dictionary with binary formatter.
        /// </summary>
        /// <param name="base64EncodedSerializedTempData">The encoded string.</param>
        /// <returns>The deserialized dictionary.</returns>
        public static IDictionary<string, object> DeserializeBase64EncodedString(this string base64EncodedSerializedTempData)
        {
            var bytes = Convert.FromBase64String(base64EncodedSerializedTempData);
            using (var memStream = new MemoryStream(bytes))
            {
                var binFormatter = new BinaryFormatter();
                return binFormatter.Deserialize(memStream, null) as IDictionary<string, object>;
            }
        }

        /// <summary>
        /// Serializes a dictionary in a base 64 encoded string.
        /// </summary>
        /// <param name="values">The values to serialize.</param>
        /// <returns>The encoded string.</returns>
        public static string SerializeToBase64EncodedString(this IDictionary<string, object> values)
        {
            using (var memStream = new MemoryStream())
            {
                var binFormatter = new BinaryFormatter();
                binFormatter.Serialize(memStream, values);
                byte[] bytes = memStream.ToArray();
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
