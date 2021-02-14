namespace Mastermind
{
    public readonly struct ChallengeResponse
    {
        public readonly CharResponse Response;

        private ChallengeResponse(CharResponse c)
        {
            Response = c;
        }

        public CharResponse GetEnum()
        {
            return Response;
        }

        public static ChallengeResponse? TryParse(char c)
        {
            switch (c)
            {
                case '?':
                case '!':
                case ' ':
                    return new ChallengeResponse((CharResponse)c);
                default:
                    return null;
            }
        }
    }

    public enum CharResponse
    {
        RightCharWrongPos = '?',
        RightCharRightPos = '!',
        WrongChar = ' '
    }
}
