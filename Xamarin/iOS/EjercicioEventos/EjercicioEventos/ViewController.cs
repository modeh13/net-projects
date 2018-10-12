using EjercicioEventos.Delegates;
using Foundation;
using System;

using UIKit;

namespace EjercicioEventos
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            btnIniciar.TouchUpInside += BtnIniciar_TouchUpInside;
            txtUsuario.Delegate = new UITextFieldUserValidationDelegate();
            txtClave.WeakDelegate = this;
        }

        private void BtnIniciar_TouchUpInside(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(txtClave.Text))
            {
                txtCodigo.Enabled = true;
                btnIniciar.TouchUpInside -= BtnIniciar_TouchUpInside;
                btnIniciar.TouchUpInside += delegate
                {
                    this.ShowPopup("Alerta", "Usuario Logueado correctamente.");
                };
            }
        }

        partial void txtUsuario_Changed(UITextField sender)
        {
            txtClave.Enabled = !string.IsNullOrEmpty(txtUsuario.Text);
        }

        //Implementation WeakDelegate
        [Export("textField:shouldChangeCharactersInRange:replacementString:")]
        public bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
        {
            return Validations.ValidateInput(replacementString, Validations.ValidationType.Pass);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}