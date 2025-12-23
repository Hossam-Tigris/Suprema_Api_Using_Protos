using Suprema_Api_Using_Protos.DTOs;

namespace Suprema_Api_Using_Protos.Helper
{
    public class LookUpsExtention
    {
        public static List<LookUpsDTO<T>> ToDtoList<T>() where T : Enum
        {

            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .Select(e => new LookUpsDTO<T>
                       {
                           Name = e.ToString(),
                           Value = e
                       }).ToList();
        }
    }
}






    