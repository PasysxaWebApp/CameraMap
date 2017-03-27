using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CameraMap.Models.Samples
{
    public class TestModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
    }

    public class TestModelList
    {
        public List<TestModel> Items { get; set; }
    }
}