using Poker.GameReader.ImageHashing;
using Poker.GameReader.ScreenUtilities;
using System.Drawing;
using System.Text;
using Tesseract;
using Rect = Poker.GameReader.ScreenUtilities.Rect;

namespace Poker.GameReader.Reporters;

public class GameStateReporter
{
    private const int BetSeat0LeftEdge = 700;
    private const int BetSeat0TopEdge = 685;
    private const int BetSeat1LeftEdge = 300;
    private const int BetSeat1TopEdge = 635;
    private const int BetSeat2LeftEdge = 330;
    private const int BetSeat2TopEdge = 375;
    private const int BetSeat3LeftEdge = 680;
    private const int BetSeat3TopEdge = 325;
    private const int BetSeat4LeftEdge = 1030;
    private const int BetSeat4TopEdge = 375;
    private const int BetSeat5LeftEdge = 1080;
    private const int BetSeat5TopEdge = 635;
    private const int BigBlindLeftEdge = 1128;
    private const int BigBlindTopEdge = 350;
    private const int ButtonLeftEdge = 630;
    private const int ButtonTopEdge = 724;
    private const int CallAmountHeight = 70;
    private const int CallAmountLeftEdge = 1108;
    private const int CallAmountTopEdge = 928;
    private const int CallAmountWidth = 130;

    private const int CardLeftEdge = 432;
    private const int CardLeftHandLeftEdge = 642;
    private const int CardLeftHandTopEdge = 772;
    private const int CardRightHandLeftEdge = 723;
    private const int CardRightHandTopEdge = 766;
    private const int CardTopEdge = 420;
    private const int CardWidth = 107;
    private const string Check = "Check";
    private const int CutOffLeftEdge = 320;
    private const int CutOffTopEdge = 695;
    private const int HighJackLeftEdge = 320;
    private const int HighJackTopEdge = 350;

    private const int PlayerName1LeftEdge = 244;
    private const int PlayerName1TopEdge = 630;
    private const int PlayerName2LeftEdge = 284;
    private const int PlayerName2TopEdge = 210;
    private const int PlayerName3LeftEdge = 814;
    private const int PlayerName3TopEdge = 105;
    private const int PlayerName4LeftEdge = 1344;
    private const int PlayerName4TopEdge = 210;
    private const int PlayerName5LeftEdge = 1384;
    private const int PlayerName5TopEdge = 630;
    private const int PlayerNameSampleSize = 10;
    private const string PlayerNameWhiteColor = "ffffffff";

    private const int PositionHeight = 26;
    private const int PositionWidth = 130;
    private const int PotTotalHeight = 34;
    private const int PotTotalLeftEdge = 604;
    private const int PotTotalTopEdge = 368;
    private const int PotTotalWidth = 260;
    private const int RankAreaHeight = 39;
    private const int RankAreaWidth = 39;
    private const int SmallBlindLeftEdge = 1130;
    private const int SmallBlindTopEdge = 694;
    private const int SpaceBetweenCards = 17;
    private const int UnderTheGunLeftEdge = 662;
    private const int UnderTheGunTopEdge = 292;

    private readonly Dictionary<int, ulong> _leftHandCardHashes;
    private readonly Dictionary<int, ulong> _middleCardHashes;
    private readonly Dictionary<int, ulong> _rightHandCardHashes;
    private readonly ScreenGrabber _screenGrabber;
    private double _bigBlind;
    private Rect _rect;
    private double _smallBlind;

    public GameStateReporter()
    {
        _screenGrabber = new ScreenGrabber();
        _middleCardHashes = LoadCardHashes(CardImages.MiddleImages);
        _leftHandCardHashes = LoadCardHashes(CardImages.LeftImages);
        _rightHandCardHashes = LoadCardHashes(CardImages.RightImages);

        PreLoadTesseractEngine();
    }

    public bool ConnectToGame(string[] gameName)
    {
        List<nint> visibleWindows = WindowHandles.GetVisibleWindows();
        foreach (var handle in visibleWindows)
        {
            var title = WindowHandles.GetWindowTitle(handle);
            if (gameName.Any(name => title.StartsWith(name, StringComparison.InvariantCultureIgnoreCase)))
            {
                _rect = WindowHandles.GetWindowRect(handle);

                (_smallBlind, _bigBlind) = GetBlindAmounts(title);

                return true;
            }
        }

        return false;
    }

    public GameData GetGameState()
    {
        return new GameData
        {
            SmallBlind = _smallBlind,
            BigBlind = _bigBlind,
            CommunityCards = GetMiddleCards(_rect, _screenGrabber),
            HandCards = GetHandCards(_rect, _screenGrabber),
            CallAmount = GetCallAmount(_rect, _screenGrabber),
            PotTotal = GetPotTotal(_rect, _screenGrabber),
            Position = GetPosition(_rect, _screenGrabber),
            VillainsPlaying =
            [
                GetPlayingSeat(PlayerName1LeftEdge,PlayerName1TopEdge, _rect, _screenGrabber),
                GetPlayingSeat(PlayerName2LeftEdge,PlayerName2TopEdge, _rect, _screenGrabber),
                GetPlayingSeat(PlayerName3LeftEdge,PlayerName3TopEdge, _rect, _screenGrabber),
                GetPlayingSeat(PlayerName4LeftEdge,PlayerName4TopEdge, _rect, _screenGrabber),
                GetPlayingSeat(PlayerName5LeftEdge,PlayerName5TopEdge, _rect, _screenGrabber),
            ],
            Bets =
            [
                GetBetSeat(BetSeat0LeftEdge,BetSeat0TopEdge, _rect, _screenGrabber),
                GetBetSeat(BetSeat1LeftEdge,BetSeat1TopEdge, _rect, _screenGrabber),
                GetBetSeat(BetSeat2LeftEdge,BetSeat2TopEdge, _rect, _screenGrabber),
                GetBetSeat(BetSeat3LeftEdge,BetSeat3TopEdge, _rect, _screenGrabber),
                GetBetSeat(BetSeat4LeftEdge,BetSeat4TopEdge, _rect, _screenGrabber),
                GetBetSeat(BetSeat5LeftEdge,BetSeat5TopEdge, _rect, _screenGrabber),
            ]
        };
    }

    private static double GetBetSeat(int left, int top, Rect rect, ScreenGrabber screenGrabber)
    {
        using TesseractEngine engine = new(@"tessdata", "eng", EngineMode.Default);
        using Bitmap potBitmap = screenGrabber
            .GrabScreenBlock(left + rect.Left, top + rect.Top, PositionWidth, PositionHeight);
        using Page page = engine.Process(potBitmap);

        string text = page.GetText();
        StringBuilder betTotal = new();
        foreach (char character in text)
        {
            if (char.IsNumber(character) || character == '.')
            {
                _ = betTotal.Append(character);
            }
        }

        return double.TryParse(betTotal.ToString(), out double result) ? result : 0;
    }

    private static (double smallBlind, double bigBlind) GetBlindAmounts(string name)
    {
        bool inSecond = false;
        string smallBlind = string.Empty;
        string bigBlind = string.Empty;
        for (int i = 0; i < name.Length; i++)
        {
            char leter = name[i];
            if (leter == '$' && !inSecond)
            {
                i++;
                while (name[i] != ' ')
                {
                    smallBlind += name[i];
                    i++;
                }
                inSecond = true;
            }

            leter = name[i];
            if (leter == '$' && inSecond)
            {
                i++;
                while (i < name.Length)
                {
                    bigBlind += name[i];
                    i++;
                }
            }
        }

        if (!double.TryParse(smallBlind, out double small))
        {
            Console.Write("Error: Getting small blind");
        }

        if (!double.TryParse(bigBlind, out double big))
        {
            Console.Write("Error: Getting bid blind");
        }

        return (small, big);
    }

    private static double GetCallAmount(Rect rect, ScreenGrabber screenGrabber)
    {
        using Bitmap bitmap = screenGrabber
            .GrabScreenBlock(CallAmountLeftEdge + rect.Left, CallAmountTopEdge + rect.Top, CallAmountWidth, CallAmountHeight);
        Color pixelColor = bitmap.GetPixel(bitmap.Width - 1, bitmap.Height - 1);
        if (pixelColor.R <= 100)
        {
            return -1;
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

        return double.TryParse(callAmount.ToString(), out double result) ? result : 0;
    }

    private static bool GetPlayingSeat(int left, int top, Rect rect, ScreenGrabber screenGrabber)
    {
        using Bitmap bitmap = screenGrabber
            .GrabScreenBlock(left + rect.Left, top + rect.Top, PlayerNameSampleSize, PlayerNameSampleSize);

        for (int i = 0; i < PlayerNameSampleSize; i++)
        {
            var color = bitmap.GetPixel(i, 0);
            if (color.Name == PlayerNameWhiteColor)
            {
                return true;
            }
        }

        return false;
    }

    private static Position GetPosition(Rect rect, ScreenGrabber screenGrabber)
    {
        int y = ButtonTopEdge + rect.Top;
        int x = ButtonLeftEdge + rect.Left;
        if (TryFindPosition(x, y, screenGrabber))
        {
            return Position.Button;
        }

        y = SmallBlindTopEdge + rect.Top;
        x = SmallBlindLeftEdge + rect.Left;
        if (TryFindPosition(x, y, screenGrabber))
        {
            return Position.SmallBlind;
        }

        y = BigBlindTopEdge + rect.Top;
        x = BigBlindLeftEdge + rect.Left;
        if (TryFindPosition(x, y, screenGrabber))
        {
            return Position.BigBlind;
        }

        y = UnderTheGunTopEdge + rect.Top;
        x = UnderTheGunLeftEdge + rect.Left;
        if (TryFindPosition(x, y, screenGrabber))
        {
            return Position.UnderTheGun;
        }

        y = HighJackTopEdge + rect.Top;
        x = HighJackLeftEdge + rect.Left;
        if (TryFindPosition(x, y, screenGrabber))
        {
            return Position.HighJack;
        }

        y = CutOffTopEdge + rect.Top;
        x = CutOffLeftEdge + rect.Left;

        return TryFindPosition(x, y, screenGrabber)
            ? Position.CutOff
            : Position.None;
    }

    private static double GetPotTotal(Rect rect, ScreenGrabber screenGrabber)
    {
        using TesseractEngine engine = new(@"tessdata", "eng", EngineMode.Default);
        using Bitmap potBitmap = screenGrabber
            .GrabScreenBlock(PotTotalLeftEdge + rect.Left, PotTotalTopEdge + rect.Top, PotTotalWidth, PotTotalHeight);
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

    private static Dictionary<int, ulong> LoadCardHashes(List<byte[]> cardImages)
    {
        var hashes = new Dictionary<int, ulong>();
        PerceptualHash hasher = new();

        for (int i = 0; i < cardImages.Count; i++)
        {
            byte[] cardImage = cardImages[i];
            MemoryStream memoryStream = new(cardImage);
            Bitmap bitMap = new(memoryStream);
            hashes[i + 1] = hasher.Hash(bitMap);
        }

        return hashes;
    }

    private static void PreLoadTesseractEngine()
    {
        using TesseractEngine engine2 = new(@"tessdata", "eng", EngineMode.Default);
    }

    private static (int cardRank, int cardSuit) RunHashComparison(Bitmap bitmap, Dictionary<int, ulong> cardHashes)
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

        return highestCertainty < 80 ? (CardRank.None, CardSuit.None) : (correctCard, suit);
    }

    private static Bitmap[] TakeScreenShotHandCards(Rect rect, ScreenGrabber screenGrabber)
    {
        int y1 = CardLeftHandTopEdge + rect.Top;
        int x1 = CardLeftHandLeftEdge + rect.Left;
        Bitmap left = screenGrabber.GrabScreenBlock(x1, y1, RankAreaWidth, RankAreaHeight);

        int y2 = CardRightHandTopEdge + rect.Top;
        int x2 = CardRightHandLeftEdge + rect.Left;
        Bitmap right = screenGrabber.GrabScreenBlock(x2, y2, RankAreaWidth, RankAreaHeight);

        return [left, right];
    }

    private static Bitmap[] TakeScreenShotMiddleCards(Rect rect, ScreenGrabber screenGrabber)
    {
        const int MiddleCardCount = 5;
        var middleCards = new Bitmap[MiddleCardCount];

        int y = CardTopEdge + rect.Top;

        for (int i = 0; i < MiddleCardCount; i++)
        {
            int x = CardLeftEdge + rect.Left + SpaceBetweenCards * i + CardWidth * i;
            Bitmap screenBlock = screenGrabber.GrabScreenBlock(x, y, RankAreaWidth, RankAreaHeight);

            middleCards[i] = screenBlock;
        }

        return middleCards;
    }

    private static bool TryFindPosition(int x, int y, ScreenGrabber screenGrabber)
    {
        using Bitmap bitmap = screenGrabber.GrabScreenBlock(x, y, 4, 4);

        return bitmap.GetPixel(1, 1).R > 100;
    }

    private (int cardRank, int cardSuit)[] GetHandCards(Rect rect, ScreenGrabber screenGrabber)
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

    private (int cardRank, int cardSuit)[] GetMiddleCards(Rect rect, ScreenGrabber screenGrabber)
    {
        Bitmap[] bitmaps = TakeScreenShotMiddleCards(rect, screenGrabber);
        (int cardRank, int cardSuit)[] cards = new (int, int)[bitmaps.Length];

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