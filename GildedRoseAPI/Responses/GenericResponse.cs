using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRoseAPI.Responses
{
    public class GenericResponse
    {
        public bool success { get; set; }
        public string message { get; set; }

        public GenericResponse()
        {
            this.success = false;
            this.message = "An error occurred.";
        }
    }
}
