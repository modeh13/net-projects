using System;
using System.Globalization;
using System.Web.Mvc;

namespace WebSanaAssessment.Util
{
   /// <summary>
   /// This class is used in Model validation to avoid that the properties of "double" type generate ModelView.IsValid = false when the value is correct.
   /// </summary>
   public class DecimalModelBinder : System.Web.Mvc.IModelBinder
   {
      public object BindModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
      {
         var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
         if (string.IsNullOrEmpty(valueResult.AttemptedValue))
         {
            return 0m;
         }
         var modelState = new System.Web.Mvc.ModelState { Value = valueResult };
         object actualValue = null;
         try
         {
            actualValue = Convert.ToDecimal(
                valueResult.AttemptedValue.Replace(",", "."),
                CultureInfo.InvariantCulture
            );
         }
         catch (FormatException e)
         {
            modelState.Errors.Add(e);
         }

         bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
         return actualValue;
      }
   }
}