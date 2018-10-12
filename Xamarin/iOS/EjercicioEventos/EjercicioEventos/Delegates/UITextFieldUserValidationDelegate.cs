using Foundation;
using UIKit;

namespace EjercicioEventos.Delegates
{
    //Implementation of Strong Delegate
    public class UITextFieldUserValidationDelegate : UITextFieldDelegate
    {
        public UITextFieldUserValidationDelegate()
        {

        }

        public override bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
        {
            return Validations.ValidateInput(replacementString, Validations.ValidationType.User);
        }
    }
}