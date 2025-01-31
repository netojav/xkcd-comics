namespace xkcd_comics.Models
{
    public class ComicModel
    {
        public int num { get; set; }
        public string title { get; set; }

        public string safe_title { get; set; }

        public string img { get; set; }

        public string alt { get; set; }

        public string day { get; set; }

        public string month { get; set; }

        public string year { get; set; }

        public string news { get; set; }

        public string transcript { get; set; }

        public string link { get; set; }

        public int prevNum{ get; set;}

        public int nextNum{ get; set;}

    }
}