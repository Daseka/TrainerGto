using Poker.GameReader.ImageHashing;
using Poker.GameReader.ScreenUtilities;
using System.Drawing;
using System.Text;
using Tesseract;
using Rect = Poker.GameReader.ScreenUtilities.Rect;

namespace Poker.GameReader.Reporters;

public class GameStateReporter
{
    private const int CallAmountHeight = 70;
    private const int CallAmountWidth = 130;

    private const int CardWidth = 107;
    private const string Check = "Check";

    private const int LeftEdgeToBigBlind = 1128;
    private const int LeftEdgeToButton = 630;

    private const int LeftEdgeToCallAmount = 1108;
    private const int LeftEdgeToCard = 432;
    private const int LeftEdgeToCardLeftHand = 642;

    private const int LeftEdgeToCardRightHand = 723;
    private const int LeftEdgeToCutOff = 320;
    private const int LeftEdgeToHighJack = 320;

    private const int LeftEdgeToPotTotal = 604;
    private const int LeftEdgeToSmallBlind = 1130;
    private const int LeftEdgeToUnderTheGun = 662;
    private const string LeftHandCards = "Left cards";

    private const string MiddleCards = "Middle cards";
    private const int PotTotalHeight = 34;
    private const int PotTotalWidth = 260;
    private const string RightHandCards = "Right cards";
    private const int SpaceBetweenCards = 17;

    private const int SymbolAreaHeight = 39;
    private const int SymbolAreaWidth = 39;

    private const int TopEdgeToBigBlind = 350;
    private const int TopEdgeToButton = 724;
    private const int TopEdgeToCallAmount = 928;
    private const int TopEdgeToCard = 420;
    private const int TopEdgeToCardLeftHand = 772;
    private const int TopEdgeToCardRightHand = 766;
    private const int TopEdgeToCutOff = 695;
    private const int TopEdgeToHighJack = 350;
    private const int TopEdgeToPotTotal = 368;
    private const int TopEdgeToSmallBlind = 694;
    private const int TopEdgeToUnderTheGun = 292;

    private readonly Dictionary<int, ulong> _leftHandCardHashes;
    private readonly Dictionary<int, ulong> _middleCardHashes;
    private readonly Dictionary<int, ulong> _rightHandCardHashes;
    private readonly ScreenGrabber _screenGrabber;

    private Rect _rect;

    public GameStateReporter()
    {
        _screenGrabber = new ScreenGrabber();
        _middleCardHashes = LoadCardHashes(MiddleCards);
        _leftHandCardHashes = LoadCardHashes(LeftHandCards);
        _rightHandCardHashes = LoadCardHashes(RightHandCards);
        PreLoadTesseractEngine();
    }

    private static void PreLoadTesseractEngine()
    {
        using TesseractEngine engine2 = new(@"tessdata", "eng", EngineMode.Default);
    }

    public bool ConnectToGame(string gameName)
    {
        List<nint> visibleWindows = WindowHandles.GetVisibleWindows();
        foreach (var handle in visibleWindows)
        {
            var title = WindowHandles.GetWindowTitle(handle);
            if (title.StartsWith(gameName, StringComparison.InvariantCultureIgnoreCase))
            {
                _rect = WindowHandles.GetWindowRect(handle);

                return true;
            }
        }

        return false;
    }

    public GameData GetGameState()
    {
        return new GameData
        {
            MiddleCards = GetMiddleCards(_rect, _screenGrabber),
            HandCards = GetHandCards(_rect, _screenGrabber),
            CallAmount = GetCallAmount(_rect, _screenGrabber),
            PotTotal = GetPotTotal(_rect, _screenGrabber),
            Position = GetPosition(_rect, _screenGrabber)
        };
    }

    private static double GetCallAmount(Rect rect, ScreenGrabber screenGrabber)
    {
        using Bitmap bitmap = screenGrabber
            .GrabScreenBlock(LeftEdgeToCallAmount + rect.Left, TopEdgeToCallAmount + rect.Top, CallAmountWidth, CallAmountHeight);
        Color pixelColor = bitmap.GetPixel(bitmap.Width - 1, bitmap.Height - 1);
        if (pixelColor.R <= 100)
        {
            return 0;
        }

        using TesseractEngine engine2 = new(@"tessdata", "eng", EngineMode.Default);
        using Page callAmountPage = engine2.Process(bitmap);

        string text = callAmountPage.GetText();
        StringBuilder callAmount = new();
        if (text.Trim().Equals(Check, StringComparison.InvariantCultureIgnoreCase))
        {
            _ = callAmount.Append(Check);
        }
        else
        {
            foreach (char character in text)
            {
                if (char.IsNumber(character) || character == '.')
                {
                    _ = callAmount.Append(character);
                }
            }
        }

        return double.TryParse(callAmount.ToString(), out double result) ? result: 0;
    }

    private static Position GetPosition(Rect rect, ScreenGrabber screenGrabber)
    {
        int y = TopEdgeToButton + rect.Top;
        int x = LeftEdgeToButton + rect.Left;
        if (TryFindPosition(x, y, screenGrabber))
        {
            return Position.Button;
        }

        y = TopEdgeToSmallBlind + rect.Top;
        x = LeftEdgeToSmallBlind + rect.Left;
        if (TryFindPosition(x, y, screenGrabber))
        {
            return Position.SmallBlind;
        }

        y = TopEdgeToBigBlind + rect.Top;
        x = LeftEdgeToBigBlind + rect.Left;
        if (TryFindPosition(x, y, screenGrabber))
        {
            return Position.BigBlind;
        }

        y = TopEdgeToUnderTheGun + rect.Top;
        x = LeftEdgeToUnderTheGun + rect.Left;
        if (TryFindPosition(x, y, screenGrabber))
        {
            return Position.UnderTheGun;
        }

        y = TopEdgeToHighJack + rect.Top;
        x = LeftEdgeToHighJack + rect.Left;
        if (TryFindPosition(x, y, screenGrabber))
        {
            return Position.HighJack;
        }

        y = TopEdgeToCutOff + rect.Top;
        x = LeftEdgeToCutOff + rect.Left;
        if (TryFindPosition(x, y, screenGrabber))
        {
            return Position.CutOff;
        }

        return Position.None;
    }

    private static double GetPotTotal(Rect rect, ScreenGrabber screenGrabber)
    {
        using TesseractEngine engine = new(@"tessdata", "eng", EngineMode.Default);
        using Bitmap potBitmap = screenGrabber
            .GrabScreenBlock(LeftEdgeToPotTotal + rect.Left, TopEdgeToPotTotal + rect.Top, PotTotalWidth, PotTotalHeight);
        using Page page = engine.Process(potBitmap);

        string text = page.GetText();
        StringBuilder potTotal = new();
        foreach (char character in text)
        {
            if (char.IsNumber(character) || character == '.')
            {
                _ = potTotal.Append(character);
            }
        }

        return double.TryParse(potTotal.ToString(), out double result) ? result : 0;
    }

    private static Dictionary<int, ulong> LoadCardHashes(string cardFolderName)
    {
        DirectoryInfo info = new(cardFolderName);
        FileInfo[] files = info.GetFiles();

        var hashes = new Dictionary<int, ulong>();
        PerceptualHash hasher = new();
        foreach (FileInfo item in files)
        {
            int cardValue = int.TryParse(item.Name[0..^4], out int result) ? result: 0;
            hashes[cardValue] = hasher.Hash(item.FullName);
        }

        return hashes;
    }

    private static (int cardSymbol, int cardSuit) RunHashComparison(Bitmap bitmap, Dictionary<int, ulong> cardHashes)
    {
        Color pixelColor = bitmap.GetPixel(bitmap.Width - 1, bitmap.Height - 1);

        int treshold = 90;
        int suit = pixelColor switch
        {
            Color when pixelColor.R < treshold && pixelColor.G < treshold && pixelColor.B < treshold => CardSuit.Spade,
            Color when pixelColor.B > pixelColor.R && pixelColor.B > pixelColor.G => CardSuit.Diamond,
            Color when pixelColor.R > pixelColor.B && pixelColor.R > pixelColor.G => CardSuit.Hart,
            Color when pixelColor.G > pixelColor.R && pixelColor.G > pixelColor.B => CardSuit.Club,
            _ => CardSuit.None
        };

        int correctCard = 0;
        double highestCertainty = 0;

        PerceptualHash hasher = new();
        foreach (var cardHash in cardHashes)
        {
            ulong hash1 = hasher.Hash(bitmap);
            ulong hash2 = cardHash.Value;

            double percentage = CompareHash.Similarity(hash1, hash2);

            if (percentage > highestCertainty)
            {
                highestCertainty = percentage;
                correctCard = cardHash.Key;
            }
        }

        return highestCertainty < 80 ? (CardSymbol.None, CardSuit.None) : (correctCard, suit);
    }

    private static Bitmap[] TakeScreenShotHandCards(Rect rect, ScreenGrabber screenGrabber)
    {
        int y1 = TopEdgeToCardLeftHand + rect.Top;
        int x1 = LeftEdgeToCardLeftHand + rect.Left;
        Bitmap left = screenGrabber.GrabScreenBlock(x1, y1, SymbolAreaWidth, SymbolAreaHeight);

        int y2 = TopEdgeToCardRightHand + rect.Top;
        int x2 = LeftEdgeToCardRightHand + rect.Left;
        Bitmap right = screenGrabber.GrabScreenBlock(x2, y2, SymbolAreaWidth, SymbolAreaHeight);

        return [left, right];
    }

    private static Bitmap[] TakeScreenShotMiddleCards(Rect rect, ScreenGrabber screenGrabber)
    {
        const int MiddleCardCount = 5;
        var middleCards = new Bitmap[MiddleCardCount];

        int y = TopEdgeToCard + rect.Top;

        for (int i = 0; i < MiddleCardCount; i++)
        {
            int x = LeftEdgeToCard + rect.Left + SpaceBetweenCards * i + CardWidth * i;
            Bitmap screenBlock = screenGrabber.GrabScreenBlock(x, y, SymbolAreaWidth, SymbolAreaHeight);

            middleCards[i] = screenBlock;
        }

        return middleCards;
    }

    private static bool TryFindPosition(int x, int y, ScreenGrabber screenGrabber)
    {
        using Bitmap bitmap = screenGrabber.GrabScreenBlock(x, y, 4, 4);

        return bitmap.GetPixel(1, 1).R > 100;
    }

    private (int cardSymbol, int cardSuit)[] GetHandCards(Rect rect, ScreenGrabber screenGrabber)
    {
        Bitmap[] bitmaps = TakeScreenShotHandCards(rect, screenGrabber);

        try
        {
            return [RunHashComparison(bitmaps[0], _leftHandCardHashes), RunHashComparison(bitmaps[1], _rightHandCardHashes)];
        }
        finally
        {
            foreach (Bitmap bitmap in bitmaps)
            {
                bitmap.Dispose();
            }
        }
    }

    private (int cardSymbol, int cardSuit)[] GetMiddleCards(Rect rect, ScreenGrabber screenGrabber)
    {
        Bitmap[] bitmaps = TakeScreenShotMiddleCards(rect, screenGrabber);
        (int cardSymbol, int cardSuit)[] cards = new (int , int)[bitmaps.Length];

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i] = RunHashComparison(bitmaps[i], _middleCardHashes);
        }

        foreach (Bitmap bitmap in bitmaps)
        {
            bitmap.Dispose();
        }

        return cards;
    }
}