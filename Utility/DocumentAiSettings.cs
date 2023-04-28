using Google.Cloud.DocumentAI.V1;
using Google.Protobuf;
using NuGet.DependencyResolver;

namespace Smart_Invoice.Utility
{
    public class DocumentAiSettings
    {
        public string type { get; set; }
        public string project_id { get; set; }
        public string private_key_id { get; set;}
        public string private_key { get; set; }
        public string client_email { get; set; }
        public string client_id { get; set; }
        public string auth_uri { get; set; }
        public string token_uri { get; set; }
        public string auth_provider_x509_cert_url { get; set; }
        public string client_x509_cert_url { get; set; }
        public string localPath { get; set; }


    }
}
