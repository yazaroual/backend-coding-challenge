namespace BackendApi.Dto
{
    /// <summary>
    /// An achievement for a specific user
    /// </summary>
    public class AchievementDto
    {
        /// <summary>
        /// Identifier for the achievement
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// If the achievement is completed or not
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// What the progress of the achievement is.
        /// Ex : if the user solved three lessons, it should return "3" for the lesson completion achievements
        /// </summary>
        public int Progress { get; set; }
    }
}