using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace EjercicioEventos
{
    public static class PopupFactory
    {
        public static void ShowPopup(this UIViewController controller, string title, string message)
        {
            UIAlertController alert = new UIAlertController();
            alert.Title = title;
            alert.Message = message;
            UIAlertAction positiveAction = UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null);
            UIAlertAction negativeAction = UIAlertAction.Create("Cancel", UIAlertActionStyle.Default, null);
            alert.AddAction(positiveAction);
            alert.AddAction(negativeAction);
            controller.PresentViewController(alert, true, null);
        }
    }
}