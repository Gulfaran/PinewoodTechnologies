namespace PinewoodTechnologies.ViewModels
{
    public class PagedDataModel<T>
    {
        public int noofpages { get;set; } 
        public string defaultorder { get;set; } 
        public List<T> data { get;set; } 
    }
}
