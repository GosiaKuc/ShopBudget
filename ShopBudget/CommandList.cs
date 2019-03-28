using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ShopBudget
{
    // klasa bazowa 
    // do wzorca Adapter -> interfejs adaptowany
    public abstract class CommandsList
    {
        protected CommandsList(CommandsListAdapter parent) { this.parent = parent; }

        public abstract void Redo();  // definicja interfejsu metody przywracającej
        public abstract void Undo();  // definicja interfejsu metody cofającej

        protected CommandsListAdapter parent;
    }

    // do wzorca Command -> inicjator polecenia
    // do wzorca Adapter -> adapter obiektowy
    // ObservableCollection<Post> -> interfejs docelowy
    [Serializable]
    public class CommandsListAdapter : ObservableCollection<Post>
    {
        [NonSerialized]
        private CommandsList command;
        [NonSerialized]
        private LinkedList<CommandsList> redoList;  // lista poleceń, które można przywrócić (np. polecenie usunięcia przychodu)
        [NonSerialized]
        private LinkedList<CommandsList> undoList;     // lista polecń, które można cofnąć (np. polecenie dodania nowego dochodu)

        public CommandsListAdapter() : base()
        {
            redoList = new LinkedList<CommandsList>();
            undoList = new LinkedList<CommandsList>();
        }

        public new void Add(Post item)
        {
            redoList.Clear();
            base.Add(item);
            undoList.AddLast(new CommandAddPost(this, item));          
        }

        public void AddSet(Post[] items)
        {
            redoList.Clear();

            foreach (Post e in items)
                base.Add(e);
            undoList.AddLast(new CommandAddPost(this, items));
        }

        public new void Clear()
        {
            redoList.Clear();
            undoList.Clear();
            base.Clear();
        }

        public new void RemoveAt(int index)
        {
            Post item = this[index];
            base.RemoveAt(index);
            undoList.AddLast(new CommandRemovePost(this, item));
        }

        public new Post this[int index]
        {
            get { return base[index]; }
            set
            {
                Post prev = base[index];
                base[index] = value;
                undoList.AddLast(new CommandEditPost(this, new Post[] {prev, value}, index));
            }
        }

        public Post[] ToArray()
        {
            Post[] entries = new Post[base.Count];

            for (int i = 0; i < entries.Length; i++)
            {
                Post entry = base[i];

                if (entry is Expense)
                    entries[i] = new Expense((Expense)entry);
                else if (entry is Income)
                    entries[i] = new Income((Income)entry);
            }

            return entries;
        }


        // czy jest możliwe przywrócenie polecenia
        public bool IsRedoPossible
        { 
            get 
            {
                if (redoList == null)
                    redoList = new LinkedList<CommandsList>();
                return redoList.Count != 0; 
            } 
        }

        // czy jest możliwe cofnięcie polecenia
        public bool IsUndoPossible
        { 
            get 
            {
                if (undoList == null)
                    undoList = new LinkedList<CommandsList>();
                return undoList.Count != 0; 
            } 
        }

        // przywrócenie polecenia
        public void Redo()
        {
            command = redoList.Last<CommandsList>();
            redoList.RemoveLast();
            undoList.AddLast(command);
            command.Redo();
        }

        // cofnięcie polecenia
        public void Undo()
        {
            command = undoList.Last<CommandsList>();
            undoList.RemoveLast();
            redoList.AddLast(command);
            command.Undo();
        }
    }

    #region List Commands

    // do wzorca Command -> klasa konkretnego polecenia
    // dotyczy dodawania wpisów
    public class CommandAddPost : CommandsList
    {
        public CommandAddPost(CommandsListAdapter parent, Post item)
            : base(parent)
        {
            this.items = new Post[] { item };
        }

        public CommandAddPost(CommandsListAdapter parent, Post[] items)
            : base(parent)
        {
            this.items = items;
        }

        // przywrócenie dodania wpisu
        public override void Redo()
        {
            foreach(Post e in items)
                ((ObservableCollection<Post>)parent).Add(e); 
        }

        // cofnięcie dodania wpisu
        public override void Undo()
        {
            foreach(Post e in items)
                ((ObservableCollection<Post>)parent).Remove(e);
        }

        private Post[] items; // wykonawca polecenia
    }

    // do wzorca Command -> klasa konkretnego polecenia
    // dotyczy usuwania wpisów
    public class CommandRemovePost : CommandsList
    {
        public CommandRemovePost(CommandsListAdapter parent, Post item)
            : base(parent)
        {
            this.item = item;
        }

        // przywrócenie usunięcia wpisu
        public override void Redo()
        {
            ((ObservableCollection<Post>)parent).Remove(item);
        }

        // cofnięcie usunięcia wpisu
        public override void Undo()
        {
            ((ObservableCollection<Post>)parent).Add(item);
        }

        private Post item;  // wykonawca polecenia
    }

    // do wzorca Command -> klasa konkretnego polecenia
    // dotyczy edytowania wpisów
    public class CommandEditPost : CommandsList
    {
        public CommandEditPost(CommandsListAdapter parent, Post[] items, int index)
            : base(parent)
        {
            this.items = items;
            this.index = index;
        }

        // przywrócenie edytowania wpisu
        public override void Redo()
        {
            ((ObservableCollection<Post>)parent)[index] = items[1];
        }

        // cofnięcie edytowania wpisu
        public override void Undo()
        {
            ((ObservableCollection<Post>)parent)[index] = items[0];
        }

        private Post[] items;  // wykonawca polecenia
        private int index;  
    }

    #endregion
}
