namespace FileImport.Application.Models
{
    public class ImportFileRequestResponse
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string ResponseMessage { get; set; }
        public int ProcessedItems { get; set; }
    }
}
