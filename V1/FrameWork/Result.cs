using System;
using System.Collections;

namespace FMS.FrameWork
{
   public class Result<T> : IEnumerable
   {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
       public IEnumerator GetEnumerator()
       {
           throw new NotImplementedException();
       }
   }
}
