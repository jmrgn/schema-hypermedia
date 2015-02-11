using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema.Hypermedia.Test.Utility
{
    public class FileHelper
    { 
        public string GetResourceTextFile(string filename)
        {
            string result = string.Empty;

            using (var stream = GetType().Assembly.
                       GetManifestResourceStream("Schema.Hypermedia.Test.Mocks." + filename))
            {
                using (var sr = new StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }
    }
}
