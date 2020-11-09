using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuVilator.Xam.Forms.Support.UX
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private static readonly ResourceGetter resourceGetter = new ResourceGetter(Thread.CurrentThread.CurrentUICulture);

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            var translation = resourceGetter.GetSpecifiedResource(Text);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources for culture '{1}'.", Text, Thread.CurrentThread.CurrentUICulture.Name),
                    "Text");
#else
                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}