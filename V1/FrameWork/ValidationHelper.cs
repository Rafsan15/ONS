using System;

namespace FMS.FrameWork
{
   public class ValidationHelper
    {
        public static bool IsStringValid(string data)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrWhiteSpace(data))
                return false;
            return true;
        }
        public static bool IsIntValid(string data)
        {
            int value;
            return Int32.TryParse(data, out value);
        }
    }
}
