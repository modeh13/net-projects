using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebViewTest
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ErrorPage : ContentPage
	{
        private List<ErrorMessage> Errors;

		public ErrorPage ()
		{
			InitializeComponent ();

            Errors = new List<ErrorMessage>();


            for (int i = 0; i <= 5; i++)
            {
                Errors.Add(new ErrorMessage()
                {
                    Message = "Este es un error al momento de validar los 'Resources' y tratar de renderizar el formulario web.",
                    Details = new List<ErrorDetailMessage>() {
                        new ErrorDetailMessage() { Property = $"Resource {i}", Message = "Aderson Rangel" },
                        new ErrorDetailMessage() { Property = "Resource Item", Message = "Wilmar Ortiz" },
                        new ErrorDetailMessage() { Property = $"Resource {i+1}", Message = "Héctor García" },
                        new ErrorDetailMessage() { Property = "Resource Item", Message = "Germán Ramírez" },
                    }
                });
            }


            foreach (ErrorMessage error in Errors)
            {
                StackLayout stcError = new StackLayout() {
                    Padding = new Thickness(10, 5, 10, 10),
                    Children = {
                        new StackLayout() {
                            Children = {
                                new Label() {
                                    Text = error.Message,
                                    HorizontalTextAlignment = TextAlignment.Start,
                                    VerticalTextAlignment = TextAlignment.Center,
                                    HorizontalOptions = new LayoutOptions() { Alignment = LayoutAlignment.Fill, Expands = true },
                                }
                            }
                        },
                    }
                };

                foreach (ErrorDetailMessage detail in error.Details)
                {
                    Grid grid = new Grid();
                    grid.Padding = new Thickness(5, 2, 5, 2);
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.5, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

                    grid.Children.Add(new Label()
                    {
                        Text = $"- {detail.Property}",
                        FontAttributes = FontAttributes.Bold,                        
                    }, 0, 0);

                    grid.Children.Add(new Label()
                    {
                        Text = detail.Message
                    }, 1, 0);

                    stcError.Children.Add(grid);
                }
             
                stcErros.Children.Add(stcError);
            }
        }
	}

    public class ErrorMessage
    {
        public string Message { get; set; }

        public List<ErrorDetailMessage> Details { get; set; }
    }

    public class ErrorDetailMessage
    {
        public string Property { get; set; }
        public string Message { get; set; }
    }
}