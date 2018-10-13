// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace EjercicioActivosPasivos
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCalcular { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtBanco { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCaja { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCapital { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCuentasCobrar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtPagoCredito { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtPagoProveedores { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtRenta { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnCalcular != null) {
                btnCalcular.Dispose ();
                btnCalcular = null;
            }

            if (txtBanco != null) {
                txtBanco.Dispose ();
                txtBanco = null;
            }

            if (txtCaja != null) {
                txtCaja.Dispose ();
                txtCaja = null;
            }

            if (txtCapital != null) {
                txtCapital.Dispose ();
                txtCapital = null;
            }

            if (txtCuentasCobrar != null) {
                txtCuentasCobrar.Dispose ();
                txtCuentasCobrar = null;
            }

            if (txtPagoCredito != null) {
                txtPagoCredito.Dispose ();
                txtPagoCredito = null;
            }

            if (txtPagoProveedores != null) {
                txtPagoProveedores.Dispose ();
                txtPagoProveedores = null;
            }

            if (txtRenta != null) {
                txtRenta.Dispose ();
                txtRenta = null;
            }
        }
    }
}