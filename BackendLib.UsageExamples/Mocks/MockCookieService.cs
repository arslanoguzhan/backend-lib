using BackendLib.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendLib.UsageExamples.Mocks
{
    internal class MockCookieService<TCookie> : ICookieService<TCookie> where TCookie : ICookie
    {
        public void Delete()
        {
            // do nothing;
        }

        public string Read()
        {
            return "";
        }

        public void Write(string value)
        {
            // do nothing;
        }
    }
}
