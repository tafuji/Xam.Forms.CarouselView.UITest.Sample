using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace CarouselViewDemos.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }

        [Test]
        public void Repl()
        {
            app.Repl();
        }


        [Test]
        public void ScrollRightCarouselView()
        {
            // move to Horizontal layout (data template)
            string cellClassName;
            int cellIndex;
            if(platform == Platform.Android)
            {
                cellClassName = "TextCellRenderer_TextCellView";
                cellIndex = 3;
            }
            else
            {
                cellClassName = "Xamarin_Forms_Platform_iOS_CellTableViewCell";
                cellIndex = 2;
            }
            app.Tap(x => x.Class(cellClassName).Index(cellIndex));


            Func<AppQuery, AppQuery> caroucelViewQuery;
            string croucelViewClassName;
            if(platform == Platform.Android)
            {
                croucelViewClassName = "CarouselViewRenderer";
                caroucelViewQuery = x => x.Class(croucelViewClassName).Index(0).Descendant("LabelRenderer").Index(0);
            }
            else
            {
                croucelViewClassName = "Xamarin_Forms_Platform_iOS_CarouselTemplatedCell";
                caroucelViewQuery = x => x.Class(croucelViewClassName).Index(0).Class("Xamarin_Forms_Platform_iOS_LabelRenderer").Index(1).Class("UILabel");
            }

            app.WaitFor(() =>
            {
                var foundItem = false;
                while(!foundItem)
                {
                    var carouselItemTitleLabel = app.Query(caroucelViewQuery).FirstOrDefault();
                    if(carouselItemTitleLabel.Text == "Tonkin Snub-nosed Monkey")
                    {
                        foundItem = true;
                    }
                    else
                    {
                        app.SwipeRightToLeft(x => x.Class(croucelViewClassName).Index(0), swipeSpeed : 10000);
                    }
                }
                return foundItem;
            }, timeout: TimeSpan.FromMinutes(1));
        }
    }
}
