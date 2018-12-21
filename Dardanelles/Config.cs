using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dardanelles
{
    class Config
    {
        private readonly FileInfo Config;
        public Config(string filename)
        {
            if (File.Exists(filename))
                path = (new FileInfo(filename)).
        }
    }
}
