namespace ModePanel.App.Models.Admin
{
    using System;
    using Data.Models;

    public class AdminLogModel
    {
        public string Admin { get; set; }

        public LogType Type { get; set; }

        public string AdditionalInformation { get; set; }

        /// <summary>
        /// C# 8.0 Switch syntax
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var message = this.Type switch
            {
                LogType.CreatePost => $" created the post {this.AdditionalInformation}",
                LogType.EditPost => $" edited the post {this.AdditionalInformation}",
                LogType.DeletePost => $" deleted the post {this.AdditionalInformation}",
                LogType.UserApproval => $" approved the registration of {this.AdditionalInformation}",
                LogType.OpenMenu => $" opened {this.AdditionalInformation} menu",
                _ => throw new InvalidOperationException($"Invalid log type")
            };

            return $"{this.Admin}{message}";
        }
    }
}
