﻿using BadSnowstorm;

namespace HelloWorld
{
    public class HelloWorldView : View<HelloWorldView, HelloWorldViewModel>
    {
        private readonly ContentArea mainArea;
        private readonly ContentArea messageArea;
        private readonly ContentArea contentArea;

        public HelloWorldView()
        {
            mainArea = new ContentArea("mainArea")
            {
                Border = new Border(BorderType.SingleLine, BorderType.DoubleLine, BorderType.DoubleLine, BorderType.DoubleLine),
                Bounds = new Rectangle(35, 0, 63, 30)
            };

            messageArea = new ContentArea("messageArea")
            {
                ContentType = ContentType.Text,
                ContentAlignment = ContentAlignment.Centered,
                Bounds = new Rectangle(38, 4, 57, 2)
            };

            contentArea = new ContentArea("contentArea")
            {
                ContentType = ContentType.AsciiArt,
                ContentAlignment = ContentAlignment.Centered,
                Bounds = new Rectangle(35, 8, 63, 13)
            };

            mainArea.Children.Add(messageArea);
            mainArea.Children.Add(contentArea);

            Children.Add(this.InputArea);
            Children.Add(mainArea);

            Bindings.Add(view => view.messageArea, viewModel => viewModel.Message);
            Bindings.Add(view => view.contentArea, viewModel => viewModel.MessageArt);
        }

        protected override InputArea CreateInputArea()
        {
            return new InputArea
            {
                Border = new Border(BorderType.DoubleLine, BorderType.SingleLine, BorderType.DoubleLine, BorderType.DoubleLine),
                Bounds = new Rectangle(0, 0, 35, 30),
                Padding = new Padding(2, 2, 1, 1),
                ContentAlignment = ContentAlignment.TopLeft
            };
        }
    }
}
