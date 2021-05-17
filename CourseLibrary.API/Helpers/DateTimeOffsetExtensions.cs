using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetCurrentAge(this DateTimeOffset dateTimeOffset
               )
        {

            
            var dateToCalculateTo = DateTime.UtcNow;


          //  if(dataOfDeath != null)
         //   {
          //      dateToCalculateTo = dataOfDeath.Value.UtcDateTime;
          //  }
            int age = dateToCalculateTo.Year - dateTimeOffset.Year;

            if (dateToCalculateTo < dateTimeOffset.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}
