using System;
using System.Collections.Generic;
namespace payload
{
    public class directory
    {
        public string id;
        public directory()
        {
            id = Guid.NewGuid().ToString("N");
        }
        public string name;
        public List<string> contentIds = new List<string>();
        public List<string> contentNames = new List<string>();
    }
}