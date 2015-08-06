// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Yamool.CWSharp
{
    using System;
    using System.IO;
    using System.Net;

    internal class Utils
    {
        public static Stream LoadDawgFile(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentException("file");
            }
            file = file.ToLower();
            if (file.StartsWith("http://") || file.StartsWith("https://"))
            {
                var request = (HttpWebRequest)HttpWebRequest.Create(file);
                try
                {
                    var response = request.GetResponse();
                    return response.GetResponseStream();
                }
                catch (WebException)
                {
                    throw new FileLoadException("Cannot load from the remote resource.", file);
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                if (!File.Exists(file))
                {
                    throw new FileNotFoundException("The file not found.", file);
                }
                return new FileStream(file, FileMode.Open, FileAccess.Read);
            }
        }
    }
}
