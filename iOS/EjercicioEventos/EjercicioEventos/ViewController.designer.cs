// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace EjercicioEventos
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnIniciar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtClave { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCodigo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtUsuario { get; set; }

        [Action ("txtUsuario_Changed:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void txtUsuario_Changed (UIKit.UITextField sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnIniciar != null) {
                btnIniciar.Dispose ();
                btnIniciar = null;
            }

            if (txtClave != null) {
                txtClave.Dispose ();
                txtClave = null;
            }

            if (txtCodigo != null) {
                txtCodigo.Dispose ();
                txtCodigo = null;
            }

            if (txtUsuario != null) {
                txtUsuario.Dispose ();
                txtUsuario = null;
            }
        }
    }
}