namespace FFS.Application.DTOs.Common
{
    public class APIResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsSucceed { get; set; }
        public object? Data { get; set; }
    }
}
