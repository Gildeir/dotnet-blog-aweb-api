using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogWebApi.Extensions
{
    public static class ModelStateExtension
    {
        public static List<string> GetErros(this ModelStateDictionary modelstate)
        {
            var result = new List<string>();
            
            foreach (var item in modelstate.Values)
            {
                result.AddRange(item.Errors.Select(error => error.ErrorMessage));

                //foreach (var error in item.Errors)
                //{
                //    result.Add(error.ErrorMessage);
                //}
            }

            return result;
        }
    }
}
