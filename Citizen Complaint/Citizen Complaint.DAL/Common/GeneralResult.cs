namespace Citizen_Complaint.BL.Common
{
    public class GeneralResult
    {
        public bool Status { get; set; }
        public ResultError[] Errors { get; set; } = [];
    }

    public class GeneralResult<T> : GeneralResult
    {
        public T? Data { get; set; }
    }

    public class ResultError
    {
        public string Message { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}