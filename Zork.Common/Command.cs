using System;
using System.Linq;
using System.Collections.Generic;

namespace Zork
{
    public class Command 
    {
        public string Name { get; set; }

        public string[] Verbs { get; }

        public Action<CommandContext> Action { get; }

        public Command(string name, IEnumerable<string> verbs, Action<CommandContext> action)
        {
            Assert.IsNotNull(name);
            Assert.IsNotNull(verbs);
            Assert.IsNotNull(action);

            Name = name;
            Verbs = verbs.ToArray();
            Action = action;
        }

        public override string ToString() => Name;
    }
}