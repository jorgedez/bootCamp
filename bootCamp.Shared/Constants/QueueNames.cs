namespace bootCamp.Shared.Constants
{
    public static class QueueNames
    {
        public const string Terminal4 = "T4";
        public const string Terminal1 = "T1";
        public const string Terminal2 = "T2";
        public const string DeadLetterSorterT1 = "T1/$DeadLetterQueue";
        public const string DeadLetterSorterT2 = "T2/$DeadLetterQueue";
        public const string DeadLetterSorterT4 = "T4/$DeadLetterQueue";
    }
}
