using Data.Models;

namespace ECatalogueManager.Extensions
{
    public static class RankExtension
    {
        public static string RankToName(this Rank rank)
        {
            return (int)rank switch
            {
                2 => "Assistant Professor",
                3 => "Associate Professor",
                4 => "Professor",
                _ => "Instructor",
            };
        }
    }
}
