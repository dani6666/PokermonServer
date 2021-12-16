namespace Pokermon.Core.Model.Enums
{
    public enum OperationError
    {
        NoError,
        TableDoesNotExist,
        PlayerDoesNotExist,
        TableAlreadyExists,
        NoSeatLeftAtTable,
        OtherPlayersTurn,
        BetTooLow,
        NotEnoughCashToBet,
        PlayerCannotRaise
    }
}
