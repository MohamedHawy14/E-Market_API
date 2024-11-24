namespace FirstWebApiProject.Models
{
    public enum PaymentStatus
    {
       
        Paid = 1,
        Failed = 2
    }

    public enum OrderStatus 
    { 
        Completed=1,
        InCompleted=2

    }

    public class Helper
    {
        public const string nvarchar = "nvarchar";
        public const string varchar = "varchar";
        public const string _char = "char";
        public const string bit = "bit";
        public const string date = "date";
        public const string time = "time";
    }
}
