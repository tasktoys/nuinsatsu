using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUInsatsu.Document
{
    class DocumentNotFoundException : Exception
    {
        public DocumentNotFoundException() : base() { }
        public DocumentNotFoundException(String message) : base(message) { }
    }
}
