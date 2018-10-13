using System;

using UIKit;

namespace EjercicioActivosPasivos
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

            btnCalcular.TouchUpInside += BtnCalcular_TouchUpInside;
        }

        private void BtnCalcular_TouchUpInside(object sender, EventArgs e)
        {
            double caja, banco, cuentasCobrar, pagoCredito, pagoProveedores, renta, capital;

            try
            {
                caja = double.Parse(txtCaja.Text);
                banco = double.Parse(txtBanco.Text);
                cuentasCobrar = double.Parse(txtCuentasCobrar.Text);
                pagoCredito = double.Parse(txtPagoCredito.Text);
                pagoProveedores = double.Parse(txtPagoProveedores.Text);
                renta = double.Parse(txtRenta.Text);

                capital = (caja + banco + cuentasCobrar) - (pagoCredito + pagoProveedores + renta);
                txtCapital.Text = capital.ToString();
            }
            catch (Exception ex)
            {
                using (UIAlertView error = new UIAlertView())
                {
                    error.Title = "";
                    error.Message = ex.Message;
                    error.AddButton("Aceptar");
                    error.Show();
                }                
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}