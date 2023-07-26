using Blace.Editing;

namespace WebBuilder2.Client.Models
{
    public class ScriptEditorFile : EditorFile
    {
        public ScriptEditorFile(string name, string content) : base(name)
        {
            Name = name;
            Content = content;
        }

        protected override Task<string> LoadContent()
        {
            return Task.FromResult(Content);
        }

        protected override Task<bool> SaveContent()
        {
            return Task.FromResult(true);
        }
    }
}
