using DialogueDisplayFramework.Data;

namespace DialogueDisplayFramework.Api
{
    public class DialogueBoxDrawEvents
    {
        public DrawEvent<IDialogueDisplayData> RenderingDialogueBox { get; }
        public DrawEvent<IDialogueDisplayData> RenderedDialogueBox { get; }

        public DrawEvent<IDialogueStringData> RenderingDialogueString { get; }
        public DrawEvent<IDialogueStringData> RenderedDialogueString { get; }

        public DrawEvent<IPortraitData> RenderingPortrait { get; }
        public DrawEvent<IPortraitData> RenderedPortrait { get; }

        public DrawEvent<IBaseData> RenderingJewel { get; }
        public DrawEvent<IBaseData> RenderedJewel { get; }

        public DrawEvent<IBaseData> RenderingButton { get; }
        public DrawEvent<IBaseData> RenderedButton { get; }

        public DrawEvent<IGiftsData> RenderingGifts { get; }
        public DrawEvent<IGiftsData> RenderedGifts { get; }

        public DrawEvent<IHeartsData> RenderingHearts { get; }
        public DrawEvent<IHeartsData> RenderedHearts { get; }

        public DrawEvent<IImageData> RenderingImage { get; }
        public DrawEvent<IImageData> RenderedImage { get; }

        public DrawEvent<ITextData> RenderingText { get; }
        public DrawEvent<ITextData> RenderedText { get; }

        public DrawEvent<IDividerData> RenderingDivider { get; }
        public DrawEvent<IDividerData> RenderedDivider { get; }

        public DialogueBoxDrawEvents()
        {
            RenderingDialogueBox = new();
            RenderedDialogueBox = new();

            RenderingDialogueString = new();
            RenderedDialogueString = new();

            RenderingPortrait = new();
            RenderedPortrait = new();

            RenderingJewel = new();
            RenderedJewel = new();

            RenderingButton = new();
            RenderedButton = new();

            RenderingGifts = new();
            RenderedGifts = new();

            RenderingHearts = new();
            RenderedHearts = new();

            RenderingImage = new();
            RenderedImage = new();

            RenderingText = new();
            RenderedText = new();

            RenderingDivider = new();
            RenderedDivider = new();
        }
    }
}
