using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace serverpayer
{
    public class DataCaller<T>
    {
        public Action<T> dataLoad;

        T result;

        public DataCaller()
        {

        }
    }
}
