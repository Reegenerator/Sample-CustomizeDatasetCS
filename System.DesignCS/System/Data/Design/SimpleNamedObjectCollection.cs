namespace System.Data.Design
{
    using System.Collections;
    using System.Runtime;

    internal class SimpleNamedObjectCollection : ArrayList, INamedObjectCollection, ICollection, IEnumerable
    {
        private static SimpleNameService myNameService;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public INameService GetNameService()
        {
            return this.NameService;
        }

        protected virtual INameService NameService
        {
            get
            {
                if (myNameService == null)
                {
                    myNameService = new SimpleNameService();
                }
                return myNameService;
            }
        }
    }
}

