namespace Smart_Invoice.Utility
{
    public class Toastr
    {
       
        public string ToastType { get; set; }
        public string ToastMessage { get; set; }
        public Toastr(string ToastType, string ToastMesssage)
        {
            this.ToastType = ToastType;
            this.ToastMessage = ToastMesssage;
        }
    }
}
