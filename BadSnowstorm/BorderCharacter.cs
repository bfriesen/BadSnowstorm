using System;

namespace BadSnowstorm
{
    public class BorderCharacter : IBorderCharacter
    {
        private readonly Point _location;
        private BorderInfo _borderInfo;

        private BorderCharacter(BorderInfo borderInfo, Point location)
        {
            _borderInfo = borderInfo;
            _location = location;
        }

        public static BorderCharacter LeftBorder(BorderType borderType, Point location)
        {
            switch (borderType)
            {
                case BorderType.SingleLine:
                    return new BorderCharacter(BorderInfo.SingleLeft, location);
                case BorderType.DoubleLine:
                    return new BorderCharacter(BorderInfo.DoubleLeft, location);
                default:
                    throw new ArgumentOutOfRangeException("borderType");
            }
        }

        public static BorderCharacter RightBorder(BorderType borderType, Point location)
        {
            switch (borderType)
            {
                case BorderType.SingleLine:
                    return new BorderCharacter(BorderInfo.SingleRight, location);
                case BorderType.DoubleLine:
                    return new BorderCharacter(BorderInfo.DoubleRight, location);
                default:
                    throw new ArgumentOutOfRangeException("borderType");
            }
        }

        public static BorderCharacter TopBorder(BorderType borderType, Point location)
        {
            switch (borderType)
            {
                case BorderType.SingleLine:
                    return new BorderCharacter(BorderInfo.SingleTop, location);
                case BorderType.DoubleLine:
                    return new BorderCharacter(BorderInfo.DoubleTop, location);
                default:
                    throw new ArgumentOutOfRangeException("borderType");
            }
        }

        public static BorderCharacter BottomBorder(BorderType borderType, Point location)
        {
            switch (borderType)
            {
                case BorderType.SingleLine:
                    return new BorderCharacter(BorderInfo.SingleBottom, location);
                case BorderType.DoubleLine:
                    return new BorderCharacter(BorderInfo.DoubleBottom, location);
                default:
                    throw new ArgumentOutOfRangeException("borderType");
            }
        }

        public Point Location
        {
            get { return _location; }
        }

        public BorderInfo BorderInfo
        {
            get { return _borderInfo; }
        }

        public void MergeWith(BorderCharacter other)
        {
            _borderInfo |= other._borderInfo;
        }

        public char GetValue()
        {
            switch (_borderInfo)
            {
                case BorderInfo.SingleLeft:
                case BorderInfo.SingleRight:
                case BorderInfo.SingleLeft | BorderInfo.SingleRight:
                    return (char)179; // │
                case BorderInfo.SingleRight | BorderInfo.SingleTop | BorderInfo.SingleBottom:
                    return (char)180; // ┤
                case BorderInfo.SingleRight | BorderInfo.DoubleTop | BorderInfo.DoubleBottom:
                    return (char)181; // ╡
                case BorderInfo.DoubleRight | BorderInfo.SingleTop | BorderInfo.SingleBottom:
                    return (char)182; // ╢
                case BorderInfo.SingleTop | BorderInfo.DoubleRight:
                    return (char)183; // ╖
                case BorderInfo.DoubleTop | BorderInfo.SingleRight:
                    return (char)184; // ╕
                case BorderInfo.DoubleRight | BorderInfo.DoubleTop | BorderInfo.DoubleBottom:
                    return (char)185; // ╣
                case BorderInfo.DoubleLeft:
                case BorderInfo.DoubleRight:
                case BorderInfo.DoubleLeft | BorderInfo.DoubleRight:
                    return (char)186; // ║
                case BorderInfo.DoubleTop | BorderInfo.DoubleRight:
                    return (char)187; // ╗
                case BorderInfo.DoubleBottom | BorderInfo.DoubleRight:
                    return (char)188; // ╝
                case BorderInfo.SingleBottom | BorderInfo.DoubleRight:
                    return (char)189; // ╜
                case BorderInfo.DoubleBottom | BorderInfo.SingleRight:
                    return (char)190; // ╛
                case BorderInfo.SingleTop | BorderInfo.SingleRight:
                    return (char)191; // ┐
                case BorderInfo.SingleBottom | BorderInfo.SingleLeft:
                    return (char)192; // └
                case BorderInfo.SingleBottom | BorderInfo.SingleLeft | BorderInfo.SingleRight:
                    return (char)193; // ┴
                case BorderInfo.SingleTop | BorderInfo.SingleLeft | BorderInfo.SingleRight:
                    return (char)194; // ┬
                case BorderInfo.SingleTop | BorderInfo.SingleBottom | BorderInfo.SingleLeft:
                    return (char)195; // ├
                case BorderInfo.SingleTop:
                case BorderInfo.SingleBottom:
                case BorderInfo.SingleTop | BorderInfo.SingleBottom:
                    return (char)196; // ─
                case BorderInfo.SingleTop | BorderInfo.SingleBottom | BorderInfo.SingleLeft | BorderInfo.SingleRight:
                    return (char)197; // ┼
                case BorderInfo.DoubleTop | BorderInfo.DoubleBottom | BorderInfo.SingleLeft:
                    return (char)198; // ╞
                case BorderInfo.SingleTop | BorderInfo.SingleBottom | BorderInfo.DoubleLeft:
                    return (char)199; // ╟
                case BorderInfo.DoubleBottom | BorderInfo.DoubleLeft:
                    return (char)200; // ╚
                case BorderInfo.DoubleTop | BorderInfo.DoubleLeft:
                    return (char)201; // ╔
                case BorderInfo.DoubleBottom | BorderInfo.DoubleLeft | BorderInfo.DoubleRight:
                    return (char)202; // ╩
                case BorderInfo.DoubleTop | BorderInfo.DoubleLeft | BorderInfo.DoubleRight:
                    return (char)203; // ╦
                case BorderInfo.DoubleTop | BorderInfo.DoubleBottom | BorderInfo.DoubleLeft:
                    return (char)204; // ╠
                case BorderInfo.DoubleTop:
                case BorderInfo.DoubleBottom:
                case BorderInfo.DoubleTop | BorderInfo.DoubleBottom:
                    return (char)205; // ═
                case BorderInfo.DoubleTop | BorderInfo.DoubleBottom | BorderInfo.DoubleLeft | BorderInfo.DoubleRight:
                    return (char)206; // ╬
                case BorderInfo.DoubleBottom | BorderInfo.SingleLeft | BorderInfo.SingleRight:
                    return (char)207; // ╧
                case BorderInfo.SingleBottom | BorderInfo.DoubleLeft | BorderInfo.DoubleRight:
                    return (char)208; // ╨
                case BorderInfo.DoubleTop | BorderInfo.SingleLeft | BorderInfo.SingleRight:
                    return (char)209; // ╤
                case BorderInfo.SingleTop | BorderInfo.DoubleLeft | BorderInfo.DoubleRight:
                    return (char)210; // ╥
                case BorderInfo.SingleBottom | BorderInfo.DoubleLeft:
                    return (char)211; // ╙
                case BorderInfo.DoubleBottom | BorderInfo.SingleLeft:
                    return (char)212; // ╘
                case BorderInfo.DoubleTop | BorderInfo.SingleLeft:
                    return (char)213; // ╒
                case BorderInfo.SingleTop | BorderInfo.DoubleLeft:
                    return (char)214; // ╓
                case BorderInfo.SingleTop | BorderInfo.SingleBottom | BorderInfo.DoubleLeft | BorderInfo.DoubleRight:
                    return (char)215; // ╫
                case BorderInfo.DoubleTop | BorderInfo.DoubleBottom | BorderInfo.SingleLeft | BorderInfo.SingleRight:
                    return (char)216; // ╪
                case BorderInfo.SingleBottom | BorderInfo.SingleRight:
                    return (char)217; // ┘
                case BorderInfo.SingleTop | BorderInfo.SingleLeft:
                    return (char)218; // ┌
                default:
                    throw new InvalidOperationException("Invalid border configuration. TODO: provide details on which and why.");
            }
        }
    }
}