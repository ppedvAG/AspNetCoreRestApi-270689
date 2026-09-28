namespace WebAPIKurs.DTOs
{
    public class GuidServiceResponseDTO
    {
        public string SingletonResult1 { get; set; }
        public string SingletonResult2 { get; set; }

        public string ScopedResult1 { get; set;  }
        public string ScopedResult2 { get; set;  }

        public string Transient1 { get; set;  }
        public string Transient2 { get; set;  }
    }
}
