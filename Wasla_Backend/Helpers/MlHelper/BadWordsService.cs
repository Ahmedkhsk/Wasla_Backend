namespace Wasla_Backend.Helpers.MlHelper
{
    using FileIO = System.IO.File;

    public class BadWordsService
    {
        private readonly HashSet<string> _badWords;

        public BadWordsService(IWebHostEnvironment env)
        {
            var path = Path.Combine(env.ContentRootPath, "badwords.txt");

            if (!FileIO.Exists(path))
                throw new Exception("badwords.txt not found");

            _badWords = FileIO.ReadAllLines(path)
                            .Select(x => x.Trim().ToLower())
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .ToHashSet();
        }

        public bool ContainsBadWord(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            var normalized = Normalize(text);

            foreach (var bad in _badWords)
            {
                if (normalized.Contains(bad))
                    return true;
            }

            return false;
        }

        private string Normalize(string text)
        {
            text = text.ToLower();

            text = Regex.Replace(text, @"[\W_]+", "");

            text = text.Replace("أ", "ا")
                       .Replace("إ", "ا")
                       .Replace("آ", "ا")
                       .Replace("ة", "ه")
                       .Replace("ى", "ي");

            return text;
        }
    }

}
