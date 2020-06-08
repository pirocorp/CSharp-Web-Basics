namespace ModePanel.App.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Log
    {
        [Key]
        public int Id { get; set; }

        public string Admin { get; set; }

        public LogType Type { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
