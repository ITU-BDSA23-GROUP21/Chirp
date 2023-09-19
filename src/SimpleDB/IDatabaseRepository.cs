namespace SimpleDB {
    public interface IDatabaseRepository<T> {
        public IEnumerable<T> Read(bool test, int? limit = null);
        public void Store(bool test, T record);
    }

}
